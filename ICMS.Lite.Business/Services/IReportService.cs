using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;

namespace ICMS.Lite.Business.Services
{
    public interface IReportService
    {
        Task<DataResult<List<ReportViewModel>>> GETINDENTSREPORTS(string startDate, string endDate);
    }
}
