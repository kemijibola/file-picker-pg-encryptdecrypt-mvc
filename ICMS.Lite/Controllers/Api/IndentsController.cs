using ICMS.Lite.Business.Services;
using ICMS.Lite.Controllers.BaseController;
using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ICMS.Lite.Controllers.Api
{
    [RoutePrefix("api/indents")]
    public class IndentsController : IcmsBaseApiController
    {

        public IndentsController(IIndentService indentService)
        {
            _indentService = indentService;
        }
        private IIndentService _indentService;
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            var result = string.Empty;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;

        }

        [HttpPost, Route("DecryptFile")]
        public async Task<HttpResponseMessage> ProcessDecryption(DecryptViewModel model)
        {
            var objDecrypt = await _indentService.DECRYPTINDENTS(model);
            var obj = new
            {
                Status = objDecrypt.Status,
                Message = objDecrypt.Message
            };

            //var convert

            return CustomResponseMessage(HttpStatusCode.OK, objDecrypt.Status, objDecrypt.Message, obj);
        }
    }
}
