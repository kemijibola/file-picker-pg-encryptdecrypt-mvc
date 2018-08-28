using ICMS.Lite.Business.Services;
using ICMS.Lite.Controllers.BaseController;
using ICMS.Lite.ErrorHandler;
using ICMS.Lite.Repository.ViewModels;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tamir.SharpSsh;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Controllers
{
    [Authorize]
    public class IndentsController : IcmsBaseMvcController
    {
        public IndentsController(UserViewModel currentUserConfig, IIndentService indentService, IMFBService mfbService)
            : base(currentUserConfig)
        {
            _indentService = indentService;
            _mfbService = mfbService;
            _currentUser = currentUserConfig;
        }
        private IIndentService _indentService;
        private IMFBService _mfbService;

        // GET: Indents
        
        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> ProcessIndentsGeneration()
        {
            var userId = _currentUser.USER_ID;
            
            var objIndents = await _indentService.GENERATEDINDENTSANDENCRYPT(userId);
            
            return await CustomResponse(objIndents.Status,objIndents.Message,objIndents.Data);
        }

        public async Task<ActionResult> GetAvailableIndentsForEncryption()
        {
            var objIndents = await _indentService.GETINDENTSFORENCRYPTION();

            return await CustomResponse(objIndents.Status, objIndents.Message, objIndents.Data);
        }

        [HttpGet]
        public async Task<ActionResult> GetGeneratedIndents()
        {
            return await CustomResponse(true, "Successful", "");
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            var userId = _currentUser.USER_ID;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Home");
            }
            var upload = await _mfbService.UPLOADMFBEXCEL(file, userId);
            return await CustomResponse(upload.Status, upload.Message, null);
        }
    }
}