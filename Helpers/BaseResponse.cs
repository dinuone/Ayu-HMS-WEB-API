using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Harendra_Ayurveda_HMS_WEB_API.Helpers
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }

        public void GenerateResponse(object data, HttpStatusCode status)
        {
            StatusCode = (int)status;
            Data = data;
        }

        public static BaseResponse CreateResponse(object data, HttpStatusCode statusCode)
        {
            return new BaseResponse
            {
                StatusCode = (int)statusCode,
                Data = data,

            };

        }

        public IActionResult ToActionResult()
        {
            var result = new JsonResult(new
            {
                StatusCode,
                Data
            })
            {
                StatusCode = StatusCode
            };

            return result;
        }

    }
}

