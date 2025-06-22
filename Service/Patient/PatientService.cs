
using System.ComponentModel;
using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisitDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Service.Location;
using Harendra_Ayurveda_HMS_WEB_API.Service.SequenceGenerator;
using Microsoft.EntityFrameworkCore;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils.FileUploadHandler;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Patient
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IPatientService> logger;
        private readonly ILocationService locationService;
        TimeZoneInfo localtimeZone;
        private readonly ISequenceGeneratorService sequenceGeneratorService;

        public PatientService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientService> logger, ILocationService locationService, ISequenceGeneratorService sequenceGeneratorService)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            localtimeZone = TimeZoneInfo.Local;
            this.locationService = locationService;
            this.sequenceGeneratorService = sequenceGeneratorService;
        }

        public async Task<BaseResponse> Create(AddPatientRequest request)
        {
            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                // Check if the NIC number already exists
                bool isNicDuplicate = await _context.Set<PatientModel>().AnyAsync(p => p.NiceNumber == request.NicNumber && p.IsDelete == false);

                if (isNicDuplicate)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            message = "NIC number already exists."
                        }
                    };
                }

                string registrationNumber = await sequenceGeneratorService.GenerateRegistrationNumberAsync();

                // Upload QR code
                FileUploadResponse fileUploadResponse = await UploadFileAsync(request.QrCode, $"qr_code_{registrationNumber}.png", registrationNumber, 0);

                PatientModel patient = new PatientModel
                {
                    Name = request.Name,
                    RegistrationNumber = registrationNumber,
                    Age = request.Age,
                    Gender = request.Gender,
                    ContactNo = request.ContactNo,
                    QrCode = fileUploadResponse.FilePath,
                    MaritalStatus = request.MaritalStatus,
                    Occupation = request.Occupation,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    City = request.City,
                    District = request.District,
                    Province = request.Province,
                    NiceNumber = request.NicNumber,
                    IsActive = true,
                    IsDelete = false,
                    CreatedBy = createdBy,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localtimeZone)
                };

                // Check if the city is new
                locationService.CheckCityExist(request.District, request.City);

                _context.Set<PatientModel>().Add(patient);
                await _context.SaveChangesAsync();

                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Patient Created Successfully",
                        qr_code = $"{GlobalAttributeHelper.fileUploadSettings.BaseUrl}{patient.QrCode}",
                        reg_no = registrationNumber,
                    }
                };
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating a patient");
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
                        ContactNumber = patient.ContactNo, 
                        NicNumber = patient.NiceNumber,
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
                    QrCode = patient.QrCode,
                    AddressLine1 = patient.AddressLine1,
                    AddressLine2 = patient.AddressLine2,
                    City = patient.City,
                    Region = patient.District,
                    Province = patient.Province,
                    //Weight = patient.Weight,
                    MaritalStatus = patient.MaritalStatus,
                    Occupation = patient.Occupation,
                    NicNumber = patient.NiceNumber
                    
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
                // Find the patient to update
                PatientModel? patient = await _context.Set<PatientModel>().FindAsync(id);
                if (patient == null)
                {
                    return new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Patient not found"
                        }
                    };
                }

                // Check if the NIC number already exists for other patients
                bool isNicDuplicate = await _context.Set<PatientModel>()
                                                     .AnyAsync(p => p.NiceNumber == request.NicNumber && p.IsDelete == false && p.Id != id);

                if (isNicDuplicate)
                {
                    return new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            message = "NIC number already exists for another patient."
                        }
                    };
                }

                string updatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                // Update the patient's information
                patient.Name = request.Name;
                patient.Age = request.Age;
                patient.NiceNumber = request.NicNumber;
                patient.ContactNo = request.ContactNo;
                patient.Gender = request.Gender;
                //patient.Weight = request.Weight;
                patient.MaritalStatus = request.MaritalStatus;
                patient.Occupation = request.Occupation;
                patient.AddressLine1 = request.AddressLine1;
                patient.AddressLine2 = request.AddressLine2;
                patient.City = request.City;
                patient.District = request.District;
                patient.Province = request.Province;
                patient.UpdatedBy = updatedBy;
                patient.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localtimeZone); // You can update the timestamp if needed

                // Save the changes
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
                logger.LogError(ex, "Error occurred while updating patient data by ID");
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




       



        public async Task<BaseResponse> ImportPatientsFromCsv(PatientImportFromExcelRequest request)
        {
            try
            {
                int importedCount = 0;
                foreach (var req in request.Patients)
                {
                    
                    bool isNicDuplicate = await _context.Patients.AnyAsync(p => p.NiceNumber == req.NicNumber && !p.IsDelete);
                    if (isNicDuplicate)
                        continue; 

                    string registrationNumber = await sequenceGeneratorService.GenerateRegistrationNumberAsync();

                    var patient = new PatientModel
                    {
                        RegistrationNumber = registrationNumber,
                        NiceNumber = req.NicNumber,
                        Gender = req.Gender,
                        City = req.City,
                        District = req.District,
                        Province = req.Province,
                        ContactNo = req.ContactNo,
                        Name = req.Name,
                        Age = req.Age,
                        MaritalStatus = req.MaritalStatus,
                        Occupation = req.Occupation,
                        AddressLine1 = req.AddressLine1,
                        AddressLine2 = req.AddressLine2,
                        QrCode = req.QrCode,
                        IsActive = true,
                        IsDelete = false,
                        CreatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString(),
                        CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localtimeZone)
                    };
                    _context.Patients.Add(patient);
                    importedCount++;
                }
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = $"{importedCount} patients imported successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error importing patients from JSON.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "Error importing patients: " + ex.Message }
                };
            }
        }

        public async Task<BaseResponse> GetPatientRecordForView(long id)
        {
            try
            {
                // Fetch patient and validate
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
                if (patient == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Patient not found" }
                    };
                }

                var patientDto = new PatientDataDTO
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    RegistrationNumber = patient.RegistrationNumber,
                    Age = patient.Age,
                    Gender = patient.Gender,
                    ContactNo = patient.ContactNo,
                    QrCode = $"{GlobalAttributeHelper.fileUploadSettings.BaseUrl}{patient.QrCode}",
                    AddressLine1 = patient.AddressLine1,
                    AddressLine2 = patient.AddressLine2,
                    City = patient.City,
                    Region = patient.District,
                    Province = patient.Province,
                    MaritalStatus = patient.MaritalStatus,
                    Occupation = patient.Occupation,
                    NicNumber = patient.NiceNumber
                };

                // Fetch patient visits
                var patientVisits = await _context.PatientVisits
                    .Where(p => p.PatientId == id && !p.IsDelete)
                    .OrderByDescending(p => p.VisitDate)
                    .ToListAsync();

                var visitList = patientVisits.Select(visit => new PatientVisitListDTO
                {
                    Id = visit.Id,
                    ChitNumber = visit.ChitNumber,
                    NicNumber = patient.NiceNumber,
                    DoctorName = GetDoctorName(visit.DoctorId), // Optional: cache or batch load
                    Status = visit.Status == PatientVisitStatus.PENDING_PAYMENT ? "COMPLETED" : "PENDING",
                    PatientName = patient.Name,
                    PatientRegNo = patient.RegistrationNumber,
                    PatientType = visit.PatientType.ToString(),
                    VisitDate = visit.VisitDate,
                    VisitType = visit.VisitType.ToString()
                }).ToList();

                // Fetch patient logs
                var patientLogs = await _context.PatientLogs
                    .Where(pl => pl.PatientId == id && !pl.IsDelete)
                    .ToListAsync();

                var logList = patientLogs.Select(pl => new PatientLogListDTO
                {
                    Id = pl.Id,
                    PatientId = pl.PatientId,
                    ChitNumber = pl.ChitNumber,
                    PatientData = patientDto, // reuse already fetched patient data
                    VisitId = pl.VisitId,
                    VisitDate = pl.VisitDate,
                    Remarks = pl.Remarks,
                    IsAssignedToClinic = pl.IsAssignedToClinic,
                    NextVisitDate = pl.NextVisitDate,
                    DoctorId = pl.DoctorId,
                    DoctorName = pl.DoctorName,
                    IsActive = pl.IsActive,
                    CreatedAt = pl.CreatedAt,
                    CreatedBy = pl.CreatedBy,
                    Prescription = pl.Prescription,
                    Treatments = pl.Treatments,
                    Clinics = pl.Clinics,
                    Diagnosis = pl.Diagnosis,
                    CaseSheet = pl.CaseSheet,
                    CaseSheetType = pl.CaseSheetType
                }).ToList();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        patient_data = patientDto,
                        visit_data = visitList,
                        patient_log = logList
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving patient data by ID");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
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
    }



}
