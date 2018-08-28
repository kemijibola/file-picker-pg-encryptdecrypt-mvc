using ICMS.Lite.Repository.Repositories;
using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;

namespace ICMS.Lite.Business.Services
{
    public class ReportService : IReportService
    {
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        private IReportRepository _reportRepository;

        public async Task<DataResult<List<ReportViewModel>>> GETINDENTSREPORTS(string startDate, string endDate)
        {
            var objIndents = await _reportRepository.GETINDENTSREPORTS(startDate, endDate);
            return objIndents;
        }
    }
}
