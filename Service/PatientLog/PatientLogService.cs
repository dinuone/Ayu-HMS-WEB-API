using System.Runtime.CompilerServices;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientLog;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.PatientLog
{
    public class PatientLogService : IPatientLogService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IPatientLogService> logger;
        TimeZoneInfo localTimeZone;

        public PatientLogService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<IPatientLogService> logger, TimeZoneInfo localTimeZone )
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.localTimeZone = localTimeZone;
        }

        //public async Task<BaseResponse> Create(PatientLogCreateRequest request)
        //{
            

        //}

        //public Task<BaseResponse> Update(long id, PatientLogCreateRequest request)
        //{
        //    // Implementation here
        //}

        //public Task<BaseResponse> GetById(long id)
        //{
        //    // Implementation here
        //}

        //public Task<BaseResponse> GetAll()
        //{
        //    // Implementation here
        //}

        //public Task<BaseResponse> Delete(long id)
        //{
        //    // Implementation here
        //}

        //public Task<BaseResponse> UpdateStatus(long id)
        //{
        //    // Implementation here
        //}

        //public Task<BaseResponse> DeleteAll(List<long> patientLogIds)
        //{
        //    // Implementation here
        //}

        //public Task<BaseResponse> Filter(FilterRequest request)
        //{
        //    // Implementation here
        //}
    }
}
