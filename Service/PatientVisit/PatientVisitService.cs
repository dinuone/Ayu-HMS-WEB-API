using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.ClinicCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using Harendra_Ayurveda_HMS_WEB_API.Models;
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
        TimeZoneInfo localTimeZone;

        public PatientVisitService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientVisitService> logger)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.localTimeZone = TimeZoneInfo.Local;

        }

        public async Task<BaseResponse> Create(AddPatientVisitRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                var visitExists = await _context.PatientVisits.AnyAsync(p =>
                    p.PatientId == request.PatientId &&
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
                    PatientId = request.PatientId,
                    ClinicCategoryId = request.ClinicCategoryId,
                    ChitNumber = Guid.NewGuid().ToString(), 
                    Status = PatientVisitStatus.PENDING, 
                    Remarks = request.Remarks,
                    VisitDate = SupportUtils.ConvertUTCDateToLocalDate(DateTime.Now, localTimeZone),
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

        public Task<BaseResponse> GetAll(bool IsActive)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetById(int id)
        {
            throw new NotImplementedException();
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
                        .Where(c => !c.IsDelete && c.AvailableDays != null);

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

     
    }
}
