//using System.Net;
//using System.Runtime.CompilerServices;
//using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
//using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientLog;
//using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO;
//using Harendra_Ayurveda_HMS_WEB_API.Helpers;
//using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
//using Harendra_Ayurveda_HMS_WEB_API.Models;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

//namespace Harendra_Ayurveda_HMS_WEB_API.Service.PatientLog
//{
//    public class PatientLogService : IPatientLogService
//    {
//        private readonly ApplicationDBContext _context;
//        private readonly IHttpContextAccessor httpContextAccessor;
//        private readonly ILogger<IPatientLogService> logger;
//        TimeZoneInfo localTimeZone;

//        public PatientLogService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<IPatientLogService> logger)
//        {
//            _context = context;
//            this.httpContextAccessor = httpContextAccessor;
//            this.logger = logger;
//            localTimeZone = TimeZoneInfo.Local;
//        }

//        public async Task<BaseResponse> Create(PatientLogCreateRequest request)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

//                var logExists = await _context.patientLogs.AnyAsync(p =>
//                    p.PatientId == request.PatientId &&
//                    p.VisitId == request.VisitId &&
//                    p.VisitDate == DateTime.Today &&
//                    p.IsActive == true && p.IsDelete == false);

//                if (logExists)
//                {
//                    return new BaseResponse
//                    {
//                        StatusCode = (int)HttpStatusCode.BadRequest,
//                        Data = new
//                        {
//                            Message = "Patient log already exists for the same patient and visit date."
//                        }
//                    };
//                }

//                var newPatientLog = new PatientLogModel
//                {
//                    PatientId = request.PatientId,
//                    PatientName = request.PatientName,
//                    VisitId = request.VisitId,
//                    VisitDate = SupportUtils.ConvertUTCDateToLocalDate(request.VisitDate, localTimeZone),
//                    Symptoms = request.symptoms,
//                    Remarks = request.remarks,
//                    IsAssignedToClinic = request.isAssignedToClinic,
//                    NextVisitDate = SupportUtils.ConvertUTCDateToLocalDate(request.nextVisitDate, localTimeZone),
//                    CreatedBy = createdBy,
//                    DoctorId = request.DoctorId,
//                    DoctorName = request.DoctorName,
//                    IsActive = true,
//                    IsDelete = false,
//                    CreatedAt = DateTime.UtcNow,
//                    Prescription = request.Prescription,
//                    Treatments = request.Treatments,
//                    Clinics = request.Clinics,
//                    Diagnosis = request.Diagnosis,
//                    ChitNumber ="12",

//                };

//                await _context.patientLogs.AddAsync(newPatientLog);
//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();

//                return new BaseResponse
//                {
//                    StatusCode = (int)HttpStatusCode.OK,
//                    Data = new
//                    {
//                        Message = "Patient log created successfully.",
//                        newPatientLog.Id
//                    }
//                };
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "Error occurred while creating patient log.");
//                await transaction.RollbackAsync();
//                return new BaseResponse
//                {
//                    StatusCode = (int)HttpStatusCode.InternalServerError,
//                    Data = new
//                    {
//                        Message = "An error occurred while creating the patient log."
//                    }
//                };
//            }
//        }

//        public async Task<BaseResponse> GetAll()
//        {
//            BaseResponse response;

//            try
//            {
//                List<PatientLogModel> PatientLogs = await _context.patientLogs.Where(pl => pl.IsDelete == false).ToListAsync();
//                List<PatientLogListDTO> PatientLogListMapp = new List<PatientLogListDTO>();

//                foreach (var PatientLog in PatientLogs)
//                {
//                    PatientLogListMapp.Add(new PatientLogListDTO
//                    {
//                        Id = PatientLog.Id,
//                        PatientId = PatientLog.PatientId,
//                        PatientName = PatientLog.PatientName,
//                        VisitId = PatientLog.VisitId,
//                        VisitDate = SupportUtils.ConvertUTCDateToLocalDate(PatientLog.VisitDate, localTimeZone),
//                        Symptoms = PatientLog.Symptoms,
//                        Remarks = PatientLog.Remarks,
//                        IsAssignedToClinic = PatientLog.IsAssignedToClinic,
//                        NextVisitDate = SupportUtils.ConvertUTCDateToLocalDate(PatientLog.NextVisitDate, localTimeZone),
//                        DoctorId = PatientLog.DoctorId,
//                        DoctorName = PatientLog.DoctorName,
//                        IsActive = PatientLog.IsActive,
//                        CreatedAt = PatientLog.CreatedAt,
//                        CreatedBy = PatientLog.CreatedBy,

//                        // Fix: Serialize the List<Prescription> to JSON string before deserialization


//                        Prescription = JsonConvert.DeserializeObject<List<PrescriptionAttribute>>(JsonConvert.SerializeObject(PatientLog.Prescription)),


//                        Treatments = JsonConvert.DeserializeObject<List<TreatmentsAttribute>>(JsonConvert.SerializeObject(PatientLog.Treatments)),
//                        Clinics = JsonConvert.DeserializeObject<List<ClinicAttribute>>(JsonConvert.SerializeObject(PatientLog.Clinics)),
//                        Diagnosis = JsonConvert.DeserializeObject<List<DiagnosisAttribute>>(JsonConvert.SerializeObject(PatientLog.Diagnosis))
//                    });
//                }

//                response = new BaseResponse
//                {
//                    StatusCode = (int)HttpStatusCode.OK,
//                    Data = PatientLogListMapp
//                };
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "Error occurred while fetching patient logs.");
//                response = new BaseResponse
//                {
//                    StatusCode = (int)HttpStatusCode.InternalServerError,
//                    Data = new
//                    {
//                        Message = "An error occurred while fetching the patient logs."
//                    }
//                };
//            }

//            return response;
//        }



//    }
//}
