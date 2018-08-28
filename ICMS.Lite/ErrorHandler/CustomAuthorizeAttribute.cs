using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ICMS.Lite.ErrorHandler
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            var routeData = HttpContext.Current.Request.RequestContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            var action = routeData.GetRequiredString("action");

            filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary
            {
                    {"action","Login" },
                    {"controller","Home" }
            });

        }
    }
}