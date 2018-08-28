using ICMS.Lite.Business.Services;
using ICMS.Lite.Controllers.BaseController;
using ICMS.Lite.Repository.Repositories;
using ICMS.Lite.Repository.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Controllers
{
    [Authorize]
    public class HomeController : IcmsBaseMvcController
    {
        public HomeController(UserViewModel currentUserConfig,IAccountService accountService,ICoreRepository coreRepository)
            :base(currentUserConfig)

        {
            _accountService = accountService;
            _coreRepository = coreRepository;
            _currentUser = currentUserConfig;
        }
        private IAccountService _accountService;
        private ICoreRepository _coreRepository;

        public ActionResult Index()
        {
           
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            return View();
        }

        public ActionResult Keepalive()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(ICMS.Lite.Repository.ViewModels.AccountViewModel.LoginViewModel model)
        {
            ViewBag.Response = null;
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var isValid = await _accountService.AUTHENTICATEUSER(model);
                if (!isValid)
                {
                    ViewBag.Response = "Invalid username or password";
                    return View(model);
                }
                await this.LoginAuthenticationTicketInitializer(model);

                var returnUrl = string.Empty;

                if (string.IsNullOrEmpty(returnUrl))
                {
                    var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (authCookie == null)
                        ViewBag.Response = "Unable to determine your role. Contact your administrator";
                    //ModelState.AddModelError("", "Unable to determine your role. Contact your administrator");
                    else
                    {
                        //Extract the forms authentication cookie
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                        //If caching roles in userData field then extract
                        var role = authTicket.UserData;

                        var roleName = authTicket.UserData;
                        //Todo::Get menu for the user role

                        //if (string.IsNullOrEmpty(roleName))
                        //{
                        //    ModelState.AddModelError("", "Invalid login attempt.");
                        //    return View(model);
                        //}

                        return RedirectToAction("Index", new { Controller = "Indents", action = "Index" });

                    }
                }
            }
            catch(Exception ex)
            {
                var error = await ExceptionRefiner.LogError(ex);
                ViewBag.Response = error;
                return View(model);
            }

            return View(model);
        }


        public async Task LoginAuthenticationTicketInitializer(LoginViewModel model)
        {
            var objModel = new LoginViewModel(_coreRepository);
            objModel.UserId = model.UserId;
            objModel.Password = model.Password;

            var roleName = await objModel.GETCURRENTUSER(objModel.UserId);

            var userData = roleName;
            var isPersistent = true;
            var ticket = new FormsAuthenticationTicket(
              1,                                     // ticket version
              model.UserId,                              // authenticated username
              DateTime.UtcNow,                          // issueDate
              DateTime.UtcNow.AddMinutes(1),           // expiryDate
              isPersistent,                          // true to persist across browser sessions
              userData,                              // can be used to store additional user data
              FormsAuthentication.FormsCookiePath);  // the path for the cookie

            // Encrypt the ticket using the machine key
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Add the cookie to the request to save it
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}