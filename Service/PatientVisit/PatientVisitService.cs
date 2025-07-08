using System.Net;
using System.Numerics;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.ClinicCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.InvoiceDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisitDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Service.SequenceGenerator;
using Harendra_Ayurveda_HMS_WEB_API.Service.UserContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.PatientVisit
{
    public class PatientVisitService : IPatientVisitService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<PatientVisitService> logger;
        private readonly ISequenceGeneratorService sequenceGeneratorService;
        TimeZoneInfo localTimeZone;
        private readonly IUserContext userContext;

        public PatientVisitService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientVisitService> logger, ISequenceGeneratorService sequenceGeneratorService, IUserContext userContext)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.localTimeZone = TimeZoneInfo.Local;
            this.sequenceGeneratorService = sequenceGeneratorService;
            this.userContext = userContext;

        }

        public async Task<BaseResponse> Create(AddPatientVisitRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                PatientModel? patient = _context.Patients.Where(p => p.RegistrationNumber == request.PatientRegNo).FirstOrDefault();

                var visitExists = await _context.PatientVisits.AnyAsync(p =>
                    p.PatientId == patient.Id &&
                    p.ClinicCategoryId == request.ClinicCategoryId &&
                    p.DoctorId == request.DoctorId &&
                    p.VisitDate == DateTime.Today &&
                    p.IsActive == true && p.IsDelete == false);

                if (visitExists)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            Message = "Patient visit already exists for the same doctor and clinic category."
                        }
                    };
                }


                PatientVisitModel newPatientVisit = new PatientVisitModel
                {
                    PatientId = patient.Id,
                    ClinicCategoryId = request.ClinicCategoryId,
                    ChitNumber = await sequenceGeneratorService.GenerateChitNumberAsync(),
                    Status = PatientVisitStatus.PENDING, 
                    Remarks = request.Remarks,
                    VisitDate = SupportUtils.ConvertUTCDateToLocalDate(DateTime.UtcNow, localTimeZone),
                    DoctorId = request.DoctorId,
                    CreatedBy = createdBy,
                    FeeloAppRef = request.FeeloAppRef,
                    PatientType = request.PatientType,
                    Treatment = request.Treatments,
                    VisitType = request.VisitType
                };


                _context.PatientVisits.Add(newPatientVisit);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        Message = "Patient visit created successfully.",
                      
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating patient visit.");
                await transaction.RollbackAsync();
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        Message = "An error occurred while creating the patient visit."
                    }
                };
            }
        }
    
        public async Task<string> GetPatientName(long patientId)
        {
            var patientName = await _context.Patients
                .Where(p => p.Id == patientId)
                .Select(p => p.Name)
                .FirstOrDefaultAsync();

            if (patientName == null)
            {
                return "Unknown Patient";
            }

            return patientName;
        }

        public async Task<string> GetClinicCategory(int clinicCategoryId)
        {
            var clinicCategoryName = await _context.ClinicCategories
                .Where(c => c.Id == clinicCategoryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            if (clinicCategoryName == null)
            {
                return "Unknown Clinic Category";
            }

            return clinicCategoryName;
        }
        public Task<BaseResponse> Update(int id, AddPatientVisitRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> GetAll(bool IsActive)
        {
            BaseResponse response;
            try
            {
                List<PatientVisitModel> patientVisits = await _context.PatientVisits.Where(p => p.IsDelete == false).OrderByDescending(p => p.VisitDate).ToListAsync();

                List<PatientVisitListDTO> patientList = new();

                foreach(var visit in patientVisits)
                {
                    PatientDataDTO? patientRecord = GetPatientRecord(visit.PatientId);

                    PatientVisitListDTO record = new PatientVisitListDTO()
                    {
                        Id = visit.Id,
                        ChitNumber = visit.ChitNumber,
                        NicNumber = patientRecord?.NicNumber,
                        DoctorName = GetDoctorName(visit.DoctorId),
                        Status = visit.Status,
                        PatientName = patientRecord?.Name,
                        PatientRegNo = patientRecord?.RegistrationNumber,
                        PatientType = visit.PatientType.ToString(),
                        VisitDate = visit.VisitDate,
                        VisitType = visit.VisitType.ToString()

                    };

                    patientList.Add(record);
                }

                return response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = patientList
                };

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching patient visit-------------------------------------------------."+ex.Message);
                return response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        Message = "An error occurred while fetching the patient visits."
                    }
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

        private string GetDoctorName(long doctorId)
        {
            var doctor = _context.Users.Where(p => p.Id == doctorId).FirstOrDefault();
            return doctor.FirstName + " " + doctor.LastName;

        }

        public async Task<BaseResponse> GetById(long id)
        {
            BaseResponse response;
            try
            {
                PatientVisitModel? patientVisit = await _context.PatientVisits.FindAsync(id);

                if (patientVisit == null)
                {
                    return response = new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "invalid patient visit id" }

                    };
                         
                }

                PatientVisitDataDTO patientVisitData = new PatientVisitDataDTO();
                patientVisitData.Id = patientVisit.Id;
                patientVisitData.ChitNumber = patientVisit.ChitNumber;
                patientVisitData.ClinicCategory = await GetClinicCategory(patientVisit.ClinicCategoryId);
                patientVisitData.FeeloAppRef = patientVisit.FeeloAppRef;
                patientVisitData.DoctorName = GetDoctorName(patientVisit.DoctorId);
                patientVisitData.Treatments = patientVisit.Treatment;
                patientVisitData.PatientType = patientVisit.PatientType.ToString();
                patientVisitData.VisitType = patientVisit.VisitType.ToString();
                patientVisitData.PatientData = GetPatientRecord(patientVisit.PatientId);
                patientVisitData.Status = patientVisit.Status;
                patientVisitData.VisitDate = patientVisit.VisitDate;


                //get invoice 
                InvoiceModel? invoice = _context.Invoice.Where(inv => inv.VisitId == patientVisit.Id).FirstOrDefault();

                if(invoice != null)
                {
                    var doctor = _context.Users.Where(u => u.Id == invoice.DoctorId).FirstOrDefault();

                    PatientLogModel? patientLog = _context.PatientLogs.Where(pl => pl.VisitId == patientVisit.Id).FirstOrDefault();

                    patientVisitData.Invoice = new InvoiceDataDTO
                    {
                        InvoiceNo = invoice.InvoiceNo,
                        DoctorName = doctor.FirstName + " " + doctor.LastName,
                        DoctorFee = invoice.DoctorFee,
                        SubTotal = invoice.SubTotal,
                        InvoiceDescription = invoice.InvoiceDescription,
                        PatientData = GetPatientRecord(patientVisit.PatientId),
                        TotalAmount = invoice.TotalAmount,
                        Treatments = patientLog.Treatments,
                        Clinics =  patientLog.Clinics,
                        CreatedAt = invoice.CreatedAt,
                        
                    };
                }


                return response = new BaseResponse
                {
                    StatusCode = (int)(HttpStatusCode.OK),
                    Data = patientVisitData
                };

            }catch(Exception ex)
            {
                logger.LogError(ex, "Error occurred while check patient visit data get by id ----------------------------------------------------------");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while fetching visit data" }
                };
            }
        }

        public Task<BaseResponse> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> DeleteAll(List<int> PatientVisitIds)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> Filter(FilterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateStatus(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> GetAvailableClinicsOrTreatments(PatientVisitTypes patientVisitType)
        {
            try
            {
                if (patientVisitType == PatientVisitTypes.Treatment)
                {
                    // Return all treatments (unchanged)
                    var treatments = await _context.Treatments
                        .Where(t => !t.IsDelete)
                        .Select(t => new TreatmentListDTO
                        {
                            Id = t.Id,
                            TreatmentName = t.Name,
                            TreatmentPrice = t.TreatmentPrice,
                            
                        })
                        .ToListAsync();

                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Data = treatments
                    };
                }
                else // PatientVisitTypes.Clinic
                {
                    // Get today's day abbreviation (e.g., "Mon", "Tue")
                    var todayAbbreviation = DateTime.Now.DayOfWeek switch
                    {
                        DayOfWeek.Monday => "Mon",
                        DayOfWeek.Tuesday => "Tue",
                        DayOfWeek.Wednesday => "Wed",
                        DayOfWeek.Thursday => "Thu",
                        DayOfWeek.Friday => "Fri",
                        DayOfWeek.Saturday => "Sat",
                        DayOfWeek.Sunday => "Sun",
                        _ => string.Empty
                    };

                    // Return clinics available today
                    var clinicQuery = _context.ClinicCategories
                        .Where(c => !c.IsDelete && c.AvailableDays != null && c.IsActive == true);

                    // Materialize the query first to work with in-memory data
                    var clinics = await clinicQuery.ToListAsync();

                    var availableClinics = clinics
                        .Where(c => c.AvailableDays.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Contains(todayAbbreviation))
                        .Select(c => new ClinicCategoryDataDTO
                        {
                            Id = c.Id,
                            ClinicCategoryName = c.Name,
                            Description = c.Description,
                            AvailableDays = c.AvailableDays.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        })
                        .ToList();


                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Data = availableClinics
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting available clinics/treatments");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while fetching data." }
                };
            }
        }

        public async Task<BaseResponse> GetAvailableDoctorsForClinic(long clinicId)
        {
            try
            {
                string currentDay = DateTime.Now.DayOfWeek.ToString();;

                List<DoctorAssginModel> doctorAssign = _context.DoctorAssgins
                   .Where(d => d.AssignedUnit == AssignUnit.CLINIC && d.IsDelete == false && d.IsActive == true) 
                   .AsEnumerable()
                         .Where(d => d.ClinicCategories.Any(c => c.Id == clinicId) && 
                         d.DaysAndShift.Any(ds => ds.Day.Equals(currentDay, StringComparison.OrdinalIgnoreCase)))
                   .ToList();

                List<AvailableDoctorListDTO> availableDoctorsList = new();

                foreach(var assign in doctorAssign)
                {
                    var doctorProfile = await _context.Users
                         .Where(u => u.Id == assign.DoctorId)
                         .Join(
                             _context.Doctors,
                             user => user.Id,
                             doctor => doctor.UserId,
                             (user, doctor) => new {
                                 User = user,
                                 Doctor = doctor
                             }
                         )
                         .FirstOrDefaultAsync();

                    AvailableDoctorListDTO mappedData = new AvailableDoctorListDTO()
                    {
                        Id = doctorProfile.User.Id,
                        DoctorName = doctorProfile.User.FirstName + " " + doctorProfile.User.LastName,
                        AvailableDays = assign.DaysAndShift,
                        DoctorFee = doctorProfile.Doctor.DoctorFee
                    };

                    availableDoctorsList.Add( mappedData );
                }

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = availableDoctorsList
                };
            }
            catch(Exception ex)
            {

                logger.LogError(ex, "Error getting available doctors list");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while fetching doctors list." }
                };
            }
        }

        public async Task<BaseResponse> GetAvailableDoctorsForTreatments(TreatmentIdsRequest request)
        {
            try
            {
                string currentDay = DateTime.Now.DayOfWeek.ToString();

                List<DoctorAssginModel> doctorAssign = _context.DoctorAssgins
                   .Where(d => d.AssignedUnit == AssignUnit.TREATMENTS && d.IsDelete == false && d.IsActive == true)
                   .AsEnumerable() 
                   .Where(d =>
                       d.Treatments.Any(t => request.TreatmentIds.Contains(t.Id)) &&
                       d.DaysAndShift.Any(ds => ds.Day.Equals(currentDay, StringComparison.OrdinalIgnoreCase)))
                   .ToList();

                List<AvailableDoctorListDTO> availableDoctorsList = new();

                foreach (var assign in doctorAssign)
                {
                    var doctorProfile = await _context.Users
                         .Where(u => u.Id == assign.DoctorId)
                         .Join(
                             _context.Doctors,
                             user => user.Id,
                             doctor => doctor.UserId,
                             (user, doctor) => new {
                                 User = user,
                                 Doctor = doctor
                             }
                         )
                         .FirstOrDefaultAsync();

                    AvailableDoctorListDTO mappedData = new AvailableDoctorListDTO()
                    {
                        Id = doctorProfile.User.Id,
                        DoctorName = doctorProfile.User.FirstName + " " + doctorProfile.User.LastName,
                        AvailableDays = assign.DaysAndShift,
                        DoctorFee = doctorProfile.Doctor.DoctorFee
                    };

                    availableDoctorsList.Add(mappedData);
                }

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = availableDoctorsList
                };

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error getting available doctors for treatments");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while doctors for treatments." }
                };
            }
        }

        public async Task<BaseResponse> CheckPatientAccount(string nicNumber)
        {
            try
            {
                PatientModel? patient = await _context.Patients.FirstOrDefaultAsync(c => c.NiceNumber == nicNumber);
                CheckAccountDTO account = new CheckAccountDTO();
                if (patient != null)
                {
                    account.AccountExisit = true;
                    account.PatientRegNo = patient.RegistrationNumber;
                }
                else
                {
                    account.AccountExisit = false;
                }

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = account
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while check patient account ----------------------------------------------------------");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating patient status" }
                };
            }
        }

        public async Task<BaseResponse> GetPatientVisitListForDoctor()
        {
            BaseResponse response;
            try
            {
                long loggedInUserId = userContext.UserId;

                List<PatientVisitModel> patientVisits = await _context.PatientVisits.Where(p => p.IsDelete == false && p.DoctorId == loggedInUserId  && p.Status == PatientVisitStatus.PENDING && p.VisitDate.Date == DateTime.Today).ToListAsync();

                List<PatientVisitListDTO> patientList = new();

                foreach (var visit in patientVisits)
                {
                    PatientDataDTO? patientRecord = GetPatientRecord(visit.PatientId);

                    

                    PatientVisitListDTO record = new PatientVisitListDTO()
                    {
                        Id = visit.Id,
                        ChitNumber = visit.ChitNumber,
                        NicNumber = patientRecord?.NicNumber,
                        DoctorName = GetDoctorName(visit.DoctorId),
                        Status = visit.Status == PatientVisitStatus.PENDING_PAYMENT ? "COMPLETED" : "PENDING",
                        PatientName = patientRecord?.Name,
                        PatientRegNo = patientRecord?.RegistrationNumber,
                        PatientType = visit.PatientType.ToString(),
                        VisitDate = visit.VisitDate,
                        VisitType = visit.VisitType.ToString()

                    };

                    patientList.Add(record);
                }

                return response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = patientList
                };

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error getting patient visit list for doctor------------------------------------------------------------------------------------");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while getting patient visit list for doctor." }
                };
            }
        }

        public async Task<BaseResponse> GetPatientVisitsByPatientId(long patientId)
        {
            try
            {
                var patientVisits = await _context.PatientVisits
                    .Where(p => p.PatientId == patientId && p.IsDelete == false)
                    .OrderByDescending(p => p.VisitDate) // Latest first
                    .ToListAsync();

                if (patientVisits == null || !patientVisits.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { Message = "No visits found for the specified patient." }
                    };
                }

                List<PatientVisitListDTO> visitList = new();

                foreach (var visit in patientVisits)
                {
                    var patientRecord = GetPatientRecord(visit.PatientId);

                    var record = new PatientVisitListDTO
                    {
                        Id = visit.Id,
                        ChitNumber = visit.ChitNumber,
                        NicNumber = patientRecord?.NicNumber,
                        DoctorName = GetDoctorName(visit.DoctorId),
                        Status = visit.Status == PatientVisitStatus.PENDING_PAYMENT ? "COMPLETED" : "PENDING",
                        PatientName = patientRecord?.Name,
                        PatientRegNo = patientRecord?.RegistrationNumber,
                        PatientType = visit.PatientType.ToString(),
                        VisitDate = visit.VisitDate,
                        VisitType = visit.VisitType.ToString()
                    };

                    visitList.Add(record);
                }

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = visitList
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching patient visits by patient id.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { Message = "An error occurred while fetching the patient visits." }
                };
            }
        }

        public async Task<BaseResponse> GetAppointmentHistory()
        {
            BaseResponse response;
            try
            {
                long loggedInUserId = userContext.UserId;

                List<PatientVisitModel> patientVisits = await _context.PatientVisits.Where(p => p.IsDelete == false && p.DoctorId == loggedInUserId && p.Status == PatientVisitStatus.PENDING_PAYMENT || p.Status == PatientVisitStatus.COMPLETE).ToListAsync();

                List<PatientVisitListDTO> patientList = new();

                foreach (var visit in patientVisits)
                {
                    PatientDataDTO? patientRecord = GetPatientRecord(visit.PatientId);



                    PatientVisitListDTO record = new PatientVisitListDTO()
                    {
                        Id = visit.Id,
                        ChitNumber = visit.ChitNumber,
                        NicNumber = patientRecord?.NicNumber,
                        DoctorName = GetDoctorName(visit.DoctorId),
                        Status = visit.Status == PatientVisitStatus.PENDING_PAYMENT ? "COMPLETED" : "PENDING",
                        PatientName = patientRecord?.Name,
                        PatientRegNo = patientRecord?.RegistrationNumber,
                        PatientType = visit.PatientType.ToString(),
                        VisitDate = visit.VisitDate,
                        VisitType = visit.VisitType.ToString()

                    };

                    patientList.Add(record);
                }

                return response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = patientList
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting patient visit list for doctor------------------------------------------------------------------------------------");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while getting patient visit list for doctor." }
                };
            }
        }




        public async Task<List<PatientVisitTimelineDTO>> GetPatientTimeline(long patientId)
        {
            try
            {
                var visits = await _context.PatientVisits
                    .Where(v => v.PatientId == patientId && v.IsDelete == false)
                    .OrderBy(v => v.VisitDate)
                    .ToListAsync();

                var timeline = new List<PatientVisitTimelineDTO>();

                foreach (var visit in visits)
                {
                   
                    var doctor = await _context.Users.FindAsync(visit.DoctorId);
                    string doctorName = doctor != null ? $"{doctor.FirstName} {doctor.LastName}" : "Unknown";

                    
                    var treatments = visit.Treatment?.Select(t => t.Name).ToList() ?? new List<string>();

                    
                    var patientLog = await _context.PatientLogs.FirstOrDefaultAsync(pl => pl.VisitId == visit.Id);
                    var clinics = patientLog?.Clinics?.Select(c => c.name).ToList() ?? new List<string>();

                    timeline.Add(new PatientVisitTimelineDTO
                    {
                        VisitId = visit.Id,
                        VisitDate = visit.VisitDate,
                        DoctorName = doctorName,
                        Treatments = treatments,
                        Clinics = clinics
                    });
                }

                return timeline;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching patient timeline.");
                return new List<PatientVisitTimelineDTO>();
            }
        }

    }
}
