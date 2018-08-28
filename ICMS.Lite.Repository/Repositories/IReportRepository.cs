using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;

namespace ICMS.Lite.Repository.Repositories
{
    public interface IReportRepository
    {
        Task<DataResult<List<ReportViewModel>>> GETINDENTSREPORTS(string startDate, string endDate);
    }
}
