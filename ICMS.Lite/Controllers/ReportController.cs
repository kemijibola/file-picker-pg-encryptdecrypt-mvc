using ICMS.Lite.Business.Services;
using ICMS.Lite.Controllers.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Controllers
{
    public class ReportController : IcmsBaseMvcController
    {
        public ReportController(UserViewModel currentUserConfig, IReportService reportService)
            :base(currentUserConfig)
        {
            _reportService = reportService;

        }
        private IReportService _reportService;
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
    }
}