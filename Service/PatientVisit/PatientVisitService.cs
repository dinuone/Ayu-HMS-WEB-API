using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.PatientVisit
{
    public class PatientVisitService: IPatientVisitService
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

        public Task<BaseResponse> Create(AddPatientVisitRequest request)
        {
            throw new NotImplementedException();
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
