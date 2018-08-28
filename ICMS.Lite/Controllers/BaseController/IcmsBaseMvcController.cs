using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Controllers.BaseController
{
    [Authorize]
    public abstract class IcmsBaseMvcController : Controller
    {
        
        public IcmsBaseMvcController(UserViewModel userConfig)
        {
            _currentUser = userConfig;
        }
        public UserViewModel _currentUser;

        public async Task<ActionResult> CustomResponse(bool returnStatus, string message, object returnData)
        {
            return Json(new
            {
                isSuccessful = returnStatus,
                Message = message,
                Data = returnData
            }, JsonRequestBehavior.AllowGet);
        }
    }
}