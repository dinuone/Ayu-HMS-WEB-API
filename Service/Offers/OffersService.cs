using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.OffersRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.OffersDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Offers
{
    public class OffersService : IOffersService
    {

        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IOffersService> logger;
        TimeZoneInfo localTimezone;


        public OffersService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<IOffersService> logger)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            localTimezone = TimeZoneInfo.Local;
        }


        public async Task<BaseResponse> Create(AddOffersRequest request)
        {
            try
            {
                string? CreatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"].ToString();


                OffersModel offer = new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    DiscountPercentage = request.DiscountPercentage,
                    StartDate = SupportUtils.ConvertUTCDateToLocalDate(request.StartDate, localTimezone),
                    EndDate = SupportUtils.ConvertUTCDateToLocalDate(request.EndDate, localTimezone),
                    TreatmentId = request.TreatmentId,
                    CreatedBy = CreatedBy,
                    Platform = request.Platform,
                    IsActive = true
                   
                };

                await _context.Offers.AddAsync(offer);
                await _context.SaveChangesAsync();


                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Offer created successfully", offer.Id }
                };

                return response;
            }

            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while creating an offer");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
        }

        public async Task<BaseResponse> GetAll(bool IsActive)
        {
            try
            {
                var offer = await _context.Offers
                    .Where(o => !o.IsDelete && o.IsActive == IsActive)
                    .Join(
                        _context.Treatments,
                        offer => offer.TreatmentId,
                        treatment => treatment.Id,
                        (offer, treatment) => new OffersListDTO
                        {
                            Id = offer.Id,
                            Name = offer.Name,
                            Description = offer.Description,
                            DiscountPercentage = offer.DiscountPercentage,
                            Platform = offer.Platform,
                            StartDate = offer.StartDate,
                            EndDate = offer.EndDate,
                            CreatedBy = offer.CreatedBy,
                            CreatedAt = offer.CreatedAt,
                            IsActive = offer.IsActive,
                            Treatment = new TreatmentDataDTO
                            {

                                TreatmentName = treatment.Name,

                            }
                        }
                    ).ToListAsync();


                var response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = offer
                };
                return response;
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while retrieving all offers");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> GetById(int id)
        {
            try
            {
                OffersModel? offer = await _context.Offers.Where(o => o.Id == id && o.IsDelete == false).FirstOrDefaultAsync();
                if (offer == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Offer not found" }
                    };
                }

                var offerDto = new OffersDataDTO
                {
                    Id = offer.Id,
                    Name = offer.Name,
                    Description = offer.Description,
                    DiscountPercentage = offer.DiscountPercentage,
                    StartDate = TimeZoneInfo.ConvertTimeFromUtc(offer.StartDate, localTimezone),
                    EndDate = TimeZoneInfo.ConvertTimeFromUtc(offer.EndDate, localTimezone),
                    Treatment = offer.TreatmentId,
                    Platform = offer.Platform
                };

                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = offerDto
                };

                return response;

            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while retrieving the offer by ID");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }

            
        }

        public async Task<BaseResponse> Update(int id, AddOffersRequest request)
        {
            BaseResponse response;
            try
            {
                OffersModel? offer = await _context.Set<OffersModel>().FindAsync(id);
                if (offer == null) 
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Offer not found" }
                    };

                    
                }

                string? updatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"].ToString();
                offer.Name = request.Name;
                offer.Description = request.Description;
                offer.DiscountPercentage = request.DiscountPercentage;
                offer.StartDate = SupportUtils.ConvertUTCDateToLocalDate(request.StartDate, localTimezone);
                offer.EndDate = SupportUtils.ConvertUTCDateToLocalDate(request.EndDate, localTimezone);
                offer.TreatmentId = request.TreatmentId;
                offer.UpdatedBy = updatedBy;
                offer.Platform = request.Platform;

                _context.Set<OffersModel>().Update(offer);
                await _context.SaveChangesAsync();
                response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Offer updated successfully" }
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
                logger.LogError(ex, "An error occurred while updating the Offer");
            }

            return response;
        }


        public async Task<BaseResponse> Delete(int id)
        {
            BaseResponse response;
            try
            {
                OffersModel? offer = await _context.Set<OffersModel>().FindAsync(id);
                if (offer == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Offer not found"
                        }
                    };
                    return response;

                }
                offer.IsDelete = true;
                _context.Set<OffersModel>().Update(offer);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Offer deleted successfully"
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
                logger.LogError(ex, "An error occurred while deleting the offer");
            }
            return response;
        }
        

        public async Task<BaseResponse> DeleteAll(List<int> OfferIds)
        {
            try
            {
                var offers = await _context.Offers
                    .Where(o => OfferIds.Contains(o.Id) && !o.IsDelete).ToListAsync();

                if (!offers.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            message = "No offers found to delete."
                        }
                    };
                }
                string deletedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
                foreach (var offer in offers)
                {
                    offer.IsDelete = true;
                    offer.UpdatedBy = deletedBy;
                }

                await _context.SaveChangesAsync();

                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Selected offers deleted successfully."
                    }
                };
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while deleting offers");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while deleting offers."
                    }
                };
            }
        }

        public async Task<BaseResponse> Filter(FilterRequest request)
        {
           
                try
                {
                    IQueryable<OffersModel> query = _context.Offers.Where(o => !o.IsDelete);

                    if (request.FromDate != null)
                    {
                    var fromDate = request.FromDate.Value.Date;
                    query = query.Where(o=> o.CreatedAt.Date >= fromDate);
                    }

                    if (request.ToDate != null)
                    {
                        var toDate = request.ToDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                        query = query.Where(o => o.CreatedAt.Date <= toDate);
                    }
                    if (request.IsActive != "All")
                    {
                        if (request.IsActive == "active")
                            query = query.Where(o => o.IsActive);
                        else if (request.IsActive == "inactive")
                            query = query.Where(o => !o.IsActive);
                    }

                var offerList = await query.Select(data => new OffersListDTO { 
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    DiscountPercentage = data.DiscountPercentage,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    CreatedBy = data.CreatedBy,
                    CreatedAt = data.CreatedAt,
                    Platform = data.Platform,
                    Treatment = new TreatmentDataDTO
                    {
                        TreatmentName = _context.Treatments.Where(t => t.Id == data.TreatmentId).Select(t => t.Name).FirstOrDefault()
                    }

                }).ToListAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = offerList
                };


            }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while filtering offers");
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Data = new { message = "An error occurred while filtering offers." }
                    };
                }

            }

       

       
        public async Task<BaseResponse> UpdateStatus(int id)
        {
            BaseResponse response;
            try
            {
                var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == id
                && !o.IsDelete);

                if (offer == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Offer not found"
                        }
                    };
                    return response;
                }


                offer.IsActive = !offer.IsActive;
                _context.Offers.Update(offer);
                await _context.SaveChangesAsync();


                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = $"Offer status updated to {(offer.IsActive ? "Active" : "Inactive")}."
                    }
                };
            }




            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while updating the offer status");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while updating offer status."
                    }
                };
            }
        }

        public async Task<BaseResponse> CalTreatmentTotal(GetOffersPriceRequest request)
        {
            try
            {
                var today = DateTime.UtcNow;

                // Fetch treatments
                var treatmentData = await _context.Treatments
                    .Where(t => request.TreatmentId.Contains(t.Id) && !t.IsDelete && t.IsActive)
                    .ToListAsync();

                // Fetch matching offers
                var offers = await _context.Offers
                    .Where(o => request.TreatmentId.Contains(o.TreatmentId)
                                && !o.IsDelete
                                && o.StartDate <= today
                                && o.EndDate >= today)
                    .ToListAsync();

                var treatmentResponseList = new List<object>();
                decimal totalOriginalPrice = 0;
                decimal totalDiscountedPrice = 0;

                foreach (var treatment in treatmentData)
                {
                    var matchingOffer = offers.FirstOrDefault(o => o.TreatmentId == treatment.Id);
                    decimal discountPercentage = matchingOffer?.DiscountPercentage ?? 0;
                    decimal originalPrice = treatment.TreatmentPrice;
                    decimal discountedPrice = originalPrice;

                    if (discountPercentage > 0)
                    {
                        discountedPrice = originalPrice - (originalPrice * discountPercentage / 100);
                    }

                    totalOriginalPrice += originalPrice;
                    totalDiscountedPrice += discountedPrice;

                    treatmentResponseList.Add(new
                    {
                        treatmentId = treatment.Id,
                        name = treatment.Name,
                        originalPrice,
                        discountPercentage,
                        discountedPrice
                    });
                }

                var result = new
                {
                    treatments = treatmentResponseList,
                    totalOriginalPrice,
                    totalDiscountedPrice
                };

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting available offers");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while checking offers."
                    }
                };
            }
        }



    }
}
