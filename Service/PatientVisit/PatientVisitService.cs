using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.PatientVisit
{
    public class PatientVisitService : IPatientVisitService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<PatientVisitService> logger;
        TimeZoneInfo localTimeZone;

        public PatientVisitService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientVisitService> logger, TimeZoneInfo localTimeZone)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.localTimeZone = localTimeZone;
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

       

    }
}
