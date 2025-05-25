using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DoctorAssignRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.Branch;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DoctorAssignDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Service.Branch;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.DoctorAssign
{
    public class DoctorAssignService : IDoctorAssignService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<DoctorAssignService> logger;
        TimeZoneInfo localTimeZone;

        public DoctorAssignService(ApplicationDBContext context, ILogger<DoctorAssignService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse> Create(CreateDoctorAssignRequest request)
        {
            BaseResponse response;
            try
            {
                // Validate doctor exists in a single query
                var doctorExists = await _context.Users
                    .AsNoTracking()
                    .AnyAsync(u => u.Id == request.DoctorId);

                if (!doctorExists)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "Invalid doctor ID." }
                    };
                }

                bool checkDoctorExisitInAssign = _context.DoctorAssgins.Where(d => d.Id == request.DoctorId && d.IsDelete == false && d.IsActive == true && d.AssignedUnit == request.AssginUnit).Any();

                if (checkDoctorExisitInAssign)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "This doctor already assigned, please update the new assignments" }
                    };
                }

                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                DoctorAssginModel doctorAssgin = new DoctorAssginModel()
                {
                    DoctorId = request.DoctorId,
                    AssignedUnit = request.AssginUnit,
                    ClinicCategories = await MapClinicCategoriesAsync(request.ClinicCategories),
                    Treatments = await MapTreatmentsAsync(request.Treatments),
                    DaysAndShift = request.DaysAndShift,
                    CreatedBy = createdBy
                };

                _context.Add(doctorAssgin);
                await _context.SaveChangesAsync();

                return response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Doctor assignment created successfully.",
                    }
                };
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating doctor assign.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while creating the doctor assign." }
                };
            }
        }



        private async Task<List<ClinicData>> MapClinicCategoriesAsync(List<long> clinicIds)
        {
            return await _context.ClinicCategories
                .AsNoTracking()
                .Where(c => clinicIds.Contains(c.Id))
                .Select(c => new ClinicData
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        private async Task<List<TreatmentData>> MapTreatmentsAsync(List<long> treatmentIds)
        {
            if (treatmentIds == null || !treatmentIds.Any())
                return new List<TreatmentData>();

            return await _context.Treatments
                .AsNoTracking()
                .Where(t => treatmentIds.Contains(t.Id))
                .Select(t => new TreatmentData
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        public async Task<BaseResponse> GetById(int id)
        {
            try
            {
                DoctorAssginModel? doctorAssgin = await _context.Set<DoctorAssginModel>().FindAsync(id);
                if (doctorAssgin == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "Docotor Assign not found." }
                    };
                }

                DoctorAssignDataDTO doctorAssignDataDto = new DoctorAssignDataDTO
                {
                   Id = doctorAssgin.Id,
                   ClincCategories = doctorAssgin.ClinicCategories,
                   DaysAndShifts = doctorAssgin.DaysAndShift,
                   Treatments = doctorAssgin.Treatments,
                   AssignUnit = doctorAssgin.AssignedUnit.ToString(),
                   DoctorId = doctorAssgin.DoctorId
                };

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = doctorAssignDataDto
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving doctor assign by ID.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while retrieving the doctor assign data." }
                };
            }
        }

        public async Task<BaseResponse> GetAll()
        {
            try
            {
                List<DoctorAssginModel> doctorAssign = await _context.DoctorAssgins.Where(b => !b.IsDelete).ToListAsync();

                var doctorIds = doctorAssign.Select(d => d.DoctorId).Distinct();

                var doctors = await _context.Users
                 .Where(u => doctorIds.Contains(u.Id))
                 .ToDictionaryAsync(u => (long)u.Id, u => $"{u.FirstName} {u.LastName}");

                // Create a list of tasks first
                var doctorAssignTasks = doctorAssign.Select(async drassign => new DoctorAssignListDTO
                {
                    Id = drassign.Id,
                    ClincCategories = drassign.ClinicCategories,
                    Treatments = drassign.Treatments,
                    DaysAndShifts = drassign.DaysAndShift,
                    AssignUnit = drassign.AssignedUnit.ToString(),
                    CreatedAt = drassign.CreatedAt,
                    CreatedBy = drassign.CreatedBy,
                    IsActive = drassign.IsActive,
                    DoctorName = doctors.TryGetValue(drassign.DoctorId, out var name) ? name : "Unknown"
                }).ToList();

                // Await all tasks
                List<DoctorAssignListDTO> doctorAssignList = (await Task.WhenAll(doctorAssignTasks)).ToList();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = doctorAssignList
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all doctor assign list.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while retrieving doctor assign list." }
                };
            }
        }

        public async Task<BaseResponse> Delete(int id)
        {
            BaseResponse response;
            try
            {
                DoctorAssginModel? doctorAssign = await _context.Set<DoctorAssginModel>().FindAsync(id);
                if (doctorAssign == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Doctor Assign not found"
                        }
                    };
                    return response;

                }
                doctorAssign.IsDelete = true;
                _context.Set<DoctorAssginModel>().Update(doctorAssign);
                await _context.SaveChangesAsync();
                
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Doctor assign deleted successfully"
                    }
                };
            }
            catch (Exception ex)
            {

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };
                logger.LogError(ex, "An error occurred while deleting the doctor assign");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            BaseResponse response;
            try
            {
                DoctorAssginModel? doctorAssgin = await _context.DoctorAssgins.FirstOrDefaultAsync(d => d.Id == id&& !d.IsDelete);

                if (doctorAssgin == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Doctor assign not found"
                        }
                    };
                    return response;
                }


                doctorAssgin.IsActive = !doctorAssgin.IsActive;
                _context.DoctorAssgins.Update(doctorAssgin);
                await _context.SaveChangesAsync();


                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = $"Drug status updated to {(doctorAssgin.IsActive ? "Active" : "Inactive")}."
                    }
                };
            }




            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while updating the doctor assign status");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while updating doctor assign status."
                    }
                };
            }
        }

        public async Task<BaseResponse> DeleteAll(List<int> drugIds)
        {
            try
            {
                List<DoctorAssginModel> doctorAssgins = await _context.DoctorAssgins
                    .Where(d => drugIds.Contains(d.Id) && !d.IsDelete).ToListAsync();

                if (!doctorAssgins.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            message = "No doctor assigns found to delete."
                        }
                    };
                }
                string deletedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
                foreach (var doctorAssgin in doctorAssgins)
                {
                    doctorAssgin.IsDelete = true;
                    doctorAssgin.UpdatedBy = deletedBy;
                }

                await _context.SaveChangesAsync();

                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Selected doctor assign deleted successfully."
                    }
                };
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while deleting doctor assign");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while deleting doctor assign."
                    }
                };
            }
        }

        public async Task<BaseResponse> Update(int id, CreateDoctorAssignRequest request)
        {
            try
            {
                var existingAssignment = await _context.DoctorAssgins.FindAsync(id);
                if (existingAssignment == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Assignment not found." }
                    };
                }

                // Validate doctor exists
                var doctorExists = await _context.Users
                    .AsNoTracking()
                    .AnyAsync(u => u.Id == request.DoctorId);

                if (!doctorExists)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "Invalid doctor ID." }
                    };
                }


                // Update assignment
                existingAssignment.DoctorId = request.DoctorId;
                existingAssignment.AssignedUnit = request.AssginUnit;
                existingAssignment.ClinicCategories = await MapClinicCategoriesAsync(request.ClinicCategories);
                existingAssignment.Treatments = await MapTreatmentsAsync(request.Treatments);
                existingAssignment.DaysAndShift = request.DaysAndShift;
                existingAssignment.UpdatedAt = DateTime.UtcNow;

                _context.DoctorAssgins.Update(existingAssignment);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Doctor assignment updated successfully.",
                        assignmentId = existingAssignment.Id
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating doctor assignment with ID {Id}", id);
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating the doctor assignment." }
                };
            }
        }

    }
}
