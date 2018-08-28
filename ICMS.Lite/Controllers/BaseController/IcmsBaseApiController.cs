using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ICMS.Lite.Controllers.BaseController
{
    public abstract class IcmsBaseApiController : ApiController
    {
        public HttpResponseMessage CustomResponseMessage(HttpStatusCode httpStatusCode, bool statusCode, string message, object data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            });
        }
    }
}
