using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientLog;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.ClinicCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DiseaseCodeDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.RatesDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Service.SequenceGenerator;
using Harendra_Ayurveda_HMS_WEB_API.Service.UserContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.PatientLog
{
    public class PatientLogService : IPatientLogService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IPatientLogService> logger;
        TimeZoneInfo localTimeZone;
        private readonly IUserContext userContext;
        private readonly ISequenceGeneratorService sequenceGeneratorService;

        public PatientLogService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<IPatientLogService> logger, IUserContext userContext, ISequenceGeneratorService sequenceGeneratorService)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.userContext = userContext;
            this.sequenceGeneratorService = sequenceGeneratorService;
        }

        public async Task<BaseResponse> Create(PatientLogCreateRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                long loggedInUserId = userContext.UserId;


                var patientData = _context.Patients.Where(p => p.RegistrationNumber == request.PatientRegNo).FirstOrDefault();

                if (patientData == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            Message = "Invalid patient register number"
                        }
                    };

                }

                var doctor = _context.Users.Where(u => u.Id == loggedInUserId).FirstOrDefault();

                var newPatientLog = new PatientLogModel
                {
                    PatientId = patientData.Id,
                    PatientName = patientData.Name,
                    VisitId = request.VisitId,
                    VisitDate = SupportUtils.ConvertUTCDateToLocalDate(DateTime.UtcNow, localTimeZone),
                    Remarks = request.remarks,
                    IsAssignedToClinic = request.isAssignedToClinic,
                    NextVisitDate = SupportUtils.ConvertUTCDateToLocalDate(request.nextVisitDate, localTimeZone),
                    CreatedBy = createdBy,
                    DoctorId = loggedInUserId,
                    DoctorName = doctor.FirstName + " " + doctor.LastName,
                    IsActive = true,
                    IsDelete = false,
                    CreatedAt = DateTime.UtcNow,
                    Prescription = request.Prescription,
                    Treatments = request.Treatments,
                    Clinics = request.Clinics,
                    Diagnosis = request.Diagnosis,
                    ChitNumber = request.ChitNumber,
                    CaseSheet = request.CaseSheet,
                    CaseSheetType = request.CaseSheetType

                };

                await _context.PatientLogs.AddAsync(newPatientLog);
                await _context.SaveChangesAsync();

                var doctroProfile = _context.Doctors.Where(doc => doc.UserId == doctor.Id).FirstOrDefault();

                //get Hsotpital Charge

                RatesModel rates = _context.Rates.First();

                if(rates == null)
                {
                    await transaction.RollbackAsync();
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            Message = "please configure the rates"
                        }
                    };
                }

                //cal hospital charge + custom charges
                decimal hospitalCharge = rates.HospitalCharge;

                var customCharges = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(rates.CustomCharges);
                decimal customChargesTotal = customCharges?.Values.Sum() ?? 0;

                decimal totalHospitalCharge = hospitalCharge + customChargesTotal;

                //cal prescription cost
                var prescriptionDruglist = request.Prescription.Select(p => p.id).ToList();

                List<DrugModel> drugs = await _context.Drugs.Where(d => prescriptionDruglist.Contains(d.Id)).ToListAsync();

                decimal totalPrescriptionCost = request.Prescription.Sum(pres =>
                {
                    var drug = drugs.FirstOrDefault(d => d.Name == pres.name);
                    return drug != null ? drug.UnitPrice * pres.qty : 0;
                });



                //cal treatment chargest
                var today = DateTime.UtcNow.Date;

                var treatmentIds = request.Treatments.Select(t => t.id).ToList();

                var treatments = await _context.Treatments.Where(t => treatmentIds.Contains(t.Id)).ToListAsync();
                
                var offers = await _context.Offers.Where(o => treatmentIds.Contains(o.TreatmentId) && o.StartDate <= today && o.EndDate >= today).ToListAsync();

                decimal totalTreatmentCost = 0;

                foreach (var treatment in treatments)
                {
                    var offer = offers.FirstOrDefault(o => o.TreatmentId == treatment.Id);

                    decimal finalPrice = treatment.TreatmentPrice;

                    if (offer != null)
                    {
                        decimal discountAmount = (offer.DiscountPercentage / 100m) * treatment.TreatmentPrice;
                        finalPrice -= discountAmount;
                    }

                    totalTreatmentCost += finalPrice;
                }


                var newInvoice = new InvoiceModel
                {
                    InvoiceNo = await sequenceGeneratorService.GenerateInvoiceNumberAsync(patientData.RegistrationNumber),
                    VisitId = request.VisitId,
                    PatientId = patientData.Id,
                    DoctorId = loggedInUserId,
                    DoctorFee = doctroProfile.DoctorFee,
                    TotalAmount = totalHospitalCharge +  totalPrescriptionCost + totalTreatmentCost + doctroProfile.DoctorFee,
                    InvoiceDescription = new InvoiceDescription
                    {
                        Prescription = request.Prescription,
                        Treatments = request.Treatments,
                        Clinics = request.Clinics,
                        Diagnosis = request.Diagnosis,
                        Rates = new RatesDataDTO
                        {
                            CustomCharge = rates.CustomCharges,
                            HospitalCharge = rates.HospitalCharge,
                        }
                        
                    },
                    SubTotal = new SubTotal
                    {
                        CustomCharge = customChargesTotal,
                        HospitalCharge = rates.HospitalCharge,
                        PrescriptionCost = totalPrescriptionCost,
                        TreatmentCharge = totalTreatmentCost
                    },
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Invoice.AddAsync(newInvoice);
                await _context.SaveChangesAsync();


                PatientVisitModel? patientVisit = _context.PatientVisits.Where(pv => pv.Id == request.VisitId).FirstOrDefault();
                patientVisit.Status = PatientVisitStatus.PENDING_PAYMENT;
                await _context.SaveChangesAsync();


                await transaction.CommitAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        Message = "Patient log created successfully.",
                        newPatientLog.Id
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating patient log ----------------------------------------");
                await transaction.RollbackAsync();
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        Message = "An error occurred while creating the patient log."
                    }
                };
            }
        }



        public async Task<BaseResponse> GetAll()
        {
            BaseResponse response;

            try
            {
                List<PatientLogModel> PatientLogs = await _context.PatientLogs.Where(pl => pl.IsDelete == false).ToListAsync();
                List<PatientLogListDTO> PatientLogListMapp = new List<PatientLogListDTO>();

                foreach (var PatientLog in PatientLogs)
                {
                    PatientLogListMapp.Add(new PatientLogListDTO
                    {
                        Id = PatientLog.Id,
                        PatientId = PatientLog.PatientId,
                        PatientData = GetPatientRecord(PatientLog.PatientId),
                        VisitId = PatientLog.VisitId,
                        VisitDate = SupportUtils.ConvertUTCDateToLocalDate(PatientLog.VisitDate, localTimeZone),
                        Remarks = PatientLog.Remarks,
                        IsAssignedToClinic = PatientLog.IsAssignedToClinic,
                        NextVisitDate = SupportUtils.ConvertUTCDateToLocalDate(PatientLog.NextVisitDate, localTimeZone),
                        DoctorId = PatientLog.DoctorId,
                        DoctorName = PatientLog.DoctorName,
                        //Casesheet = JsonConvert.DeserializeObject<string>(PatientLog.CaseSheet), // Deserialize the CaseSheet from JSON string
                        IsActive = PatientLog.IsActive,
                        CreatedAt = PatientLog.CreatedAt,
                        CreatedBy = PatientLog.CreatedBy,

                        // Fix: Serialize the List<Prescription> to JSON string before deserialization


                        Prescription = PatientLog.Prescription,


                        Treatments = PatientLog.Treatments,
                        Clinics = PatientLog.Clinics,
                        Diagnosis = PatientLog.Diagnosis
                    });
                }

                response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = PatientLogListMapp
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching patient logs.");
                response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        Message = "An error occurred while fetching the patient logs."
                    }
                };
            }

            return response;
        }

        public async Task<BaseResponse> GetById(long id)
        {
            try
            {
                var patientLog = await _context.PatientLogs.FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);

                if (patientLog == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Patient log not found" }
                    };
                }

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = patientLog
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving the patient log by ID");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
        }

        public async Task<BaseResponse> GetMasterDataForPatientLog()
        {
            try
            {
                var diseaseCodes = await _context.DiseaseCodes
                    .Where(p => !p.IsDelete && p.IsActive)
                    .Select(p => new DiseaseCodeNameListDTO { Id = p.Id, Name = p.DiseaseName })
                    .ToListAsync();

                var drugs = await _context.Drugs
                    .Where(p => !p.IsDelete && p.IsActive)
                    .Select(p => new DrugNameListDTO { Id = p.Id, Name = p.Name, DrugCode =p.DrugCode })
                    .ToListAsync();

                var treatments = await _context.Treatments
                    .Where(p => !p.IsDelete && p.IsActive)
                    .Select(p => new TreatmentNameListDTO { Id = p.Id, Name = p.Name })
                    .ToListAsync();

                var clinics = await _context.ClinicCategories
                    .Where(p => !p.IsDelete && p.IsActive)
                    .Select(p => new ClinicNameListDTO { Id = p.Id, Name = p.Name })
                    .ToListAsync();

                var response = new MasterDataDTO
                {
                    DiseaseCodes = diseaseCodes,
                    Drugs = drugs,
                    Treatments = treatments,
                    Clinics = clinics
                };

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching master data for patient log");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
        }



        public async Task<BaseResponse> GetPatientLogByVisitId(long visitId)
        {
            try
            {
                var patientLogs = await _context.PatientLogs
                .Where(pl => pl.VisitId == visitId && !pl.IsDelete)
                .ToListAsync();
                  

                // Optionally map to DTOs if you have a PatientLogListDTO or similar
                PatientLogListDTO? patientLog = patientLogs.Select(pl => new PatientLogListDTO
                {
                    Id = pl.Id,
                    PatientId = pl.PatientId,
                    PatientData = GetPatientRecord(pl.PatientId),
                    ChitNumber = pl.ChitNumber,
                    VisitId = pl.VisitId,
                    VisitDate = SupportUtils.ConvertUTCDateToLocalDate(pl.VisitDate, localTimeZone),
                    Remarks = pl.Remarks,
                    IsAssignedToClinic = pl.IsAssignedToClinic,
                    NextVisitDate = SupportUtils.ConvertUTCDateToLocalDate(pl.NextVisitDate, localTimeZone),
                    DoctorId = pl.DoctorId,
                    DoctorName = pl.DoctorName,
                    IsActive = pl.IsActive,
                    CreatedAt = pl.CreatedAt,
                    CreatedBy = pl.CreatedBy,
                    Prescription = JsonConvert.DeserializeObject<List<PrescriptionAttribute>>(JsonConvert.SerializeObject(pl.Prescription)),
                    Treatments = JsonConvert.DeserializeObject<List<TreatmentsAttribute>>(JsonConvert.SerializeObject(pl.Treatments)),
                    Clinics = JsonConvert.DeserializeObject<List<ClinicAttribute>>(JsonConvert.SerializeObject(pl.Clinics)),
                    Diagnosis = JsonConvert.DeserializeObject<List<DiagnosisAttribute>>(JsonConvert.SerializeObject(pl.Diagnosis)),
                    CaseSheet = pl.CaseSheet,
                    CaseSheetType = pl.CaseSheetType
                }).FirstOrDefault();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = patientLog
                };

            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error occurred while fetching patient logs by visit ID.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { Message = "An error occurred while fetching the patient logs." }
                };
            }
        }

        private PatientDataDTO? GetPatientRecord(long patientId)
        {
            PatientDataDTO? patientData = _context.Patients.Where(p => p.Id == patientId).Select(p => new PatientDataDTO
            {
                Id = p.Id,
                Name = p.Name,
                NicNumber = p.NiceNumber,
                RegistrationNumber = p.RegistrationNumber,
                Gender = p.Gender,
                ContactNo = p.ContactNo,
                Age = p.Age,
                MaritalStatus = p.MaritalStatus,
                Province = p.Province,
                Region = p.District,
                AddressLine1 = p.AddressLine1,
                AddressLine2 = p.AddressLine2,
                City = p.City,
                Occupation = p.Occupation,
                QrCode = $"{GlobalAttributeHelper.fileUploadSettings.BaseUrl}{p.QrCode}",

            }).FirstOrDefault();

            return patientData;
        }


        public async Task<BaseResponse> GetPatientLogListByPatientId(long patientId) 
        {

            try
            {
                var patientLogs = await _context.PatientLogs
                    .Where(pl => pl.PatientId == patientId && !pl.IsDelete)
                    .ToListAsync();

               

                var logList = patientLogs.Select(pl => new PatientLogListDTO
                {
                    Id = pl.Id,
                    PatientId = pl.PatientId,
                    ChitNumber = pl.ChitNumber,
                    PatientData = GetPatientRecord(pl.PatientId),
                    VisitId = pl.VisitId,
                    VisitDate = SupportUtils.ConvertUTCDateToLocalDate(pl.VisitDate, localTimeZone),
                    Remarks = pl.Remarks,
                    IsAssignedToClinic = pl.IsAssignedToClinic,
                    NextVisitDate = SupportUtils.ConvertUTCDateToLocalDate(pl.NextVisitDate, localTimeZone),
                    DoctorId = pl.DoctorId,
                    DoctorName = pl.DoctorName,
                    IsActive = pl.IsActive,
                    CreatedAt = pl.CreatedAt,
                    CreatedBy = pl.CreatedBy,
                    Prescription = JsonConvert.DeserializeObject<List<PrescriptionAttribute>>(JsonConvert.SerializeObject(pl.Prescription)),
                    Treatments = JsonConvert.DeserializeObject<List<TreatmentsAttribute>>(JsonConvert.SerializeObject(pl.Treatments)),
                    Clinics = JsonConvert.DeserializeObject<List<ClinicAttribute>>(JsonConvert.SerializeObject(pl.Clinics)),
                    Diagnosis = JsonConvert.DeserializeObject<List<DiagnosisAttribute>>(JsonConvert.SerializeObject(pl.Diagnosis)),
                    CaseSheet = pl.CaseSheet,
                    CaseSheetType = pl.CaseSheetType
                }).ToList();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = logList
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching patient logs by patient ID.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { Message = "An error occurred while fetching the patient logs." }
                };
            }
        }
    }
}
