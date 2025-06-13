using System.Net;
using System.Runtime.CompilerServices;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientLog;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.ClinicCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DiseaseCodeDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Service.UserContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public PatientLogService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<IPatientLogService> logger, IUserContext userContext)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.userContext = userContext;
        }

        public async Task<BaseResponse> Create(PatientLogCreateRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                long loggedInUserId = userContext.UserId;
                //var logExists = await _context.PatientLogs.AnyAsync(p =>
                //    p.PatientId == request.PatientId &&
                //    p.VisitId == request.VisitId &&
                //    p.VisitDate == DateTime.Today &&
                //    p.IsActive == true && p.IsDelete == false);

                //if (logExists)
                //{
                //    return new BaseResponse
                //    {
                //        StatusCode = (int)HttpStatusCode.BadRequest,
                //        Data = new
                //        {
                //            Message = "Patient log already exists for the same patient and visit date."
                //        }
                //    };
                //}


                var patientData = _context.Patients.Where(p => p.RegistrationNumber == request.PatientRegNo).FirstOrDefault();

                if(patientData == null)
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
                    CaseSheet = JsonConvert.SerializeObject(request.CaseSheet) // Serialize the CaseSheet to JSON string

                };

                await _context.PatientLogs.AddAsync(newPatientLog);
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
                logger.LogError(ex, "Error occurred while creating patient log.");
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
                        PatientName = PatientLog.PatientName,
                        VisitId = PatientLog.VisitId,
                        VisitDate = SupportUtils.ConvertUTCDateToLocalDate(PatientLog.VisitDate, localTimeZone),
                        Symptoms = PatientLog.Symptoms,
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


                        Prescription = JsonConvert.DeserializeObject<List<PrescriptionAttribute>>(JsonConvert.SerializeObject(PatientLog.Prescription)),


                        Treatments = JsonConvert.DeserializeObject<List<TreatmentsAttribute>>(JsonConvert.SerializeObject(PatientLog.Treatments)),
                        Clinics = JsonConvert.DeserializeObject<List<ClinicAttribute>>(JsonConvert.SerializeObject(PatientLog.Clinics)),
                        Diagnosis = JsonConvert.DeserializeObject<List<DiagnosisAttribute>>(JsonConvert.SerializeObject(PatientLog.Diagnosis))
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
                    .Select(p => new DrugNameListDTO { Id = p.Id, Name = p.Name })
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
    }
}
