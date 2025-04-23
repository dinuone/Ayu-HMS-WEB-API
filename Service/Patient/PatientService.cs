
using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Service.Location;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Patient
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IPatientService> logger;
        private readonly ILocationService locationService;
        TimeZoneInfo localtimeZone;

        public PatientService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientService> logger, ILocationService locationService)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            localtimeZone = TimeZoneInfo.Local;
            this.locationService = locationService;
        }

        public async Task<BaseResponse> Create(AddPatientRequest request)
        {
            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
                PatientModel patient = new PatientModel
                {
                    Name = request.Name,
                    RegistrationNumber = request.RegistrationNumber,
                    Age = request.Age,
                    Gender = request.Gender,
                    PatientType = request.PatientType,
                    ContactNo = request.ContactNo,
                    QrCode = request.QrCode,
                    Weight = request.Weight,
                    MaritalStatus = request.MaritalStatus,
                    Occupation = request.Occupation,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    City = request.City,
                    District = request.District,
                    Province = request.Province,
                    IsActive = true,
                    IsDelete = false,
                    CreatedBy = createdBy,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localtimeZone)
                };

                //check city is new 
                locationService.CheckCityExist(request.District, request.City);


                _context.Set<PatientModel>().Add(patient);
                await _context.SaveChangesAsync();

                var response = new BaseResponse 
                {
                    StatusCode =(int)HttpStatusCode.OK,
                    Data = new 
                    {
                        message = "Patient Created Successfully",
                    }
                };
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while creating a patient");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };
                
            }
          
        }
        public async Task<BaseResponse> GetAll()
        {
            BaseResponse response;
            try
            {
                List<PatientModel> patients = await _context.Patients.Where(p => p.IsDelete == false).ToListAsync();
                List<PatientListDTO> patientListmapp = new List<PatientListDTO>();

                foreach (var patient in patients)
                {
                    patientListmapp.Add(new PatientListDTO
                    {
                        Id = patient.Id,
                        Name = patient.Name,
                        RegistrationNumber = patient.RegistrationNumber,
                        Age = patient.Age,
                        Gender = patient.Gender, 
                        ContactNo = patient.ContactNo, 
                        PatientType = patient.PatientType,
                        QrCode = patient.QrCode,
                        AddressLine1 = patient.AddressLine1,
                        AddressLine2 = patient.AddressLine2,
                        City = patient.City,
                        Region = patient.District,
                        Province = patient.Province,
                        Weight = patient.Weight,
                        MaritalStatus = patient.MaritalStatus,
                        Occupation = patient.Occupation,
                        IsActive = patient.IsActive,
                        CreatedAt = patient.CreatedAt,
                        CreatedBy = patient.CreatedBy,



                    });

                }
                response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = patientListmapp
                };
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving all Patients");
                response = new BaseResponse() 
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };
                return response;
            }
            
        }

        public async Task<BaseResponse> GetById(long id)
        {
            BaseResponse response;
            try
            {
                PatientModel? patient = await _context.Set<PatientModel>().FindAsync(id);
                if (patient == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Patient not found"
                        }
                    };
                   return response;
                }

                var patientDto = new PatientDataDTO
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    RegistrationNumber = patient.RegistrationNumber,
                    Age = patient.Age,
                    Gender = patient.Gender,
                    ContactNo = patient.ContactNo,
                    PatientType = patient.PatientType,
                    QrCode = patient.QrCode,
                    AddressLine1 = patient.AddressLine1,
                    AddressLine2 = patient.AddressLine2,
                    City = patient.City,
                    Region = patient.District,
                    Province = patient.Province,
                    Weight = patient.Weight,
                    MaritalStatus = patient.MaritalStatus,
                    Occupation = patient.Occupation,
                };
                response = new BaseResponse() 
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = patientDto
                };
                return response;
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
                logger.LogError(ex, "Error Occured while retrieving Pateint data by ID");
                
            }
            return response;
        }

        public async Task<BaseResponse> Update(long id, AddPatientRequest request)
        {
            BaseResponse response;
            try
            {
                PatientModel? patient = await _context.Set<PatientModel>().FindAsync(id);
                if (patient == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Patient not found"
                        }
                    };
                    return response;


                }

                string updatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                patient.Name = request.Name;
                patient.RegistrationNumber = request.RegistrationNumber;
                patient.QrCode = request.QrCode;
                patient.Age = request.Age;
                patient.ContactNo = request.ContactNo;
                patient.PatientType = request.PatientType;
                patient.Gender = request.Gender;
                patient.Weight = request.Weight;
                patient.MaritalStatus = request.MaritalStatus;
                patient.Occupation = request.Occupation;
                patient.AddressLine1 = request.AddressLine1;
                patient.AddressLine2 = request.AddressLine2;
                patient.City = request.City;
                patient.District = request.District;
                patient.Province = request.Province;
                patient.UpdatedBy = updatedBy;

                _context.Set<PatientModel>().Update(patient);
                await _context.SaveChangesAsync(); 

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Patient updated successfully"
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
                logger.LogError(ex, "Error Occured while updating Pateint data by ID");
            }
            return response;
        }
        

        public async Task<BaseResponse> Delete(long id)
        {
            BaseResponse response;
            try
            {
                PatientModel? patient = await _context.Set<PatientModel>().FindAsync(id);
                if (patient == null) 
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new 
                        {
                            message = "Patient not found"
                        }
                    };
                    return response;
                }
                patient.IsDelete = true;
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new 
                    {
                        message = "Patient deleted succesfully"
                    }
                };
                return response;    
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
                logger.LogError(ex, "Error occurred while deleting patient");
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAll(List<long> patientIds)
        {
            try
            {
                var patients = await _context.Patients
                    .Where(p => patientIds.Contains(p.Id) && !p.IsDelete)
                    .ToListAsync();
                if (!patients.Any()) 
                {
                    return new BaseResponse 
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new 
                        {
                            message = "No valid patients found to delete."
                        }
                    };
                }
                string? deletedBy = httpContextAccessor.HttpContext?.Items["Createdby"]?.ToString();

                foreach (var patient in patients) 
                {
                    patient.IsDelete = true;
                    patient.UpdatedBy = deletedBy;
                }

                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Patients deleted successfully." }
                };


            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error occurred while deleting multiple patients.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while deleting patients." }
                };
            }
        }

        public async Task<BaseResponse> Filter(FilterRequest request)
        {
            try
            {
                IQueryable<PatientModel> query = _context.Patients.Where(p => !p.IsDelete);
                if (request.FromDate != null) 
                {
                    var fromDate = request.FromDate.Value.Date;
                    query = query.Where(p => p.CreatedAt <= fromDate);
                };
                if (request.ToDate != null)
                {
                    var toDate = request.ToDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                    query = query.Where(p => p.CreatedAt <= toDate);
                }

                if (request.IsActive != "All")
                {
                    if (request.IsActive == "active")
                        query = query.Where(p => p.IsActive);
                    else if (request.IsActive == "inactive")
                        query = query.Where(p => !p.IsActive);
                }

                var patientLists = await query.Select(data => new PatientListDTO
                {
                    Id = data.Id,
                    Name = data.Name,
                    RegistrationNumber = data.RegistrationNumber,
                    Age = data.Age,
                    Gender = data.Gender,
                    ContactNo = data.ContactNo,
                    PatientType = data.PatientType, 
                    QrCode = data.QrCode,
                    AddressLine1 = data.AddressLine1,
                    AddressLine2 = data.AddressLine2,
                    City = data.City,
                    Region = data.District,
                    Province = data.Province,
                    Weight = data.Weight,
                    MaritalStatus = data.MaritalStatus,
                    Occupation = data.Occupation,
                    IsActive = data.IsActive,
                    CreatedAt = data.CreatedAt,
                    CreatedBy = data.CreatedBy

                }).ToListAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = patientLists
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while filtering Patients.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while Patients." }
                };
            }
        }


        public async Task<BaseResponse> UpdateStatus(long id)
        {
            BaseResponse response;
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);
                if (patient == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Patient not found"
                        }
                    };
                    return response;
                }
                patient.IsActive = !patient.IsActive;
                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = $"Patient status updated to {(patient.IsActive ? "Active" : "Inactive")}." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating patient status.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating patient status." }
                };
            }
        }
    }
}
