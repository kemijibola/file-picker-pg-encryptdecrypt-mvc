using ICMS.Lite.Repository.Data;
using ICMS.Lite.Repository.Utilities;
using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;

namespace ICMS.Lite.Repository.Repositories
{
    public class ReportRepository : IReportRepository
    {
        public ReportRepository()
        {
            _connectionString = AuthGuard();
            _db = new DBManager(_connectionString);
            _dr = new DataRead();
        }

        private readonly DBManager _db;
        private static string _connectionString;
        private static DataRead _dr;

        public async Task<DataResult<List<ReportViewModel>>> GETINDENTSREPORTS(string startDate, string endDate)
        {
            var objReport = new DataResult<List<ReportViewModel>>();
            try
            {
                var dt = new DataTable();

                var ds = _dr.GETINDENTSREPORTS(startDate, endDate);
                dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    objReport.Data = null;
                    objReport.Status = false;
                    objReport.Message = "No record currently available for processing";
                    return objReport;
                }

                var reportList = ConvertDataTableToList<ReportViewModel>(dt);

                objReport.Data = reportList;
                objReport.Status = true;
                objReport.Message = "Successful";
            }
            catch (Exception ex)
            {
                var error = await ExceptionRefiner.LogError(ex);
                ErrorWriter.WriteLog($"Indent reports returned error: { ex.Message } at { DateTime.Now }");
                objReport.Data = null;
                objReport.Status = false;
                objReport.Message = error;
            }

            return objReport;
        }
    }
}
