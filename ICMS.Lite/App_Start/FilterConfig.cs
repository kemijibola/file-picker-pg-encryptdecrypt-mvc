using ICMS.Lite.ErrorHandler;
using System.Web;
using System.Web.Mvc;

namespace ICMS.Lite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new AiHandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute());
        }
    }
}
