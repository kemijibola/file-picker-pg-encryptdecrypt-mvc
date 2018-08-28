using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace ICMS.Lite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            try
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    //Extract the forms authentication cookie
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    // If caching roles in userData field then extract
                    var role = authTicket.UserData;

                    // Create the IIdentity instance
                    IIdentity id = new FormsIdentity(authTicket);

                    // Create the IPrinciple instance
                    IPrincipal principal = new GenericPrincipal(id, new string[] { role });

                    // Set the context user 
                    Context.User = principal;
                }

            }
            catch (Exception ex)
            {
                //var error = Task.Run(async () => await ExceptionRefiner.LogError(ex)).Result;
                throw new Exception(ex.Message);
            }
        }
    }
}
