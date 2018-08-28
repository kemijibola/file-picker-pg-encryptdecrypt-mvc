using ICMS.Lite.Repository.Data;
using ICMS.Lite.Repository.Utilities;
using ICMS.Lite.Repository.ViewModels;
using Oracle.DataAccess.Client;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.Utilities.IcmsConstant;

namespace ICMS.Lite.Repository.Repositories
{
    public class MFBRepository : IMFBRepository
    {
        public MFBRepository()
        {
            _connectionString = AuthGuard();
            _db = new DBManager(_connectionString);
        }
        private readonly DBManager _db;
        private static string _connectionString;

        public async Task<DataResult<MfbViewModel>> UPLOADMFBEXCEL(HttpPostedFileBase upload, string username)
        {
            try
            {
                if(string.IsNullOrEmpty(username))
                    throw new PlatformCustomException("invalid user");

                var objResult = new DataResult<MfbViewModel>();
                var parameter = new List<IDbDataParameter>();

                //declare all variables for indents for excel
                #region IndentVariables
                var indentId = string.Empty;
                var micrAccountNo = string.Empty;
                var sortCode = string.Empty;
                var lastSerialNumber = 0;
                var branchCode = string.Empty;
                var branchName = string.Empty;
                var firstChequeNumber = string.Empty;
                var numberOfCheques = string.Empty;
                var numberOfChequesPrinted = string.Empty;
                var chequeType = string.Empty;
                var dateOfRequest = DateTime.Now.ToString("dd-MMM-yy");
                var dateEnterted = DateTime.Now.ToString("dd-MMM-yy");
                var pulled = string.Empty;
                var datePulled = DateTime.Now.ToString("dd-MMM-yy");
                var printed = string.Empty;
                var approved = string.Empty;
                var approver = string.Empty;
                var uploaded = string.Empty;
                var requisitionId = string.Empty;
                var scheduleNo = string.Empty;
                var indentSource = string.Empty;
                var deliveryBranchCode = string.Empty;
                var customerName = string.Empty;
                var ranged = string.Empty;
                var recieved = string.Empty;
                var dateRecieved = DateTime.Now.ToString("dd-MMM-yy");
                var deleted = string.Empty;
                var acknowledged = string.Empty;
                var dateAcknowledged = DateTime.Now.ToString("dd-MMM-yy");
                var dispatched = string.Empty;
                var dateDispatched = DateTime.Now.ToString("dd-MMM-yy");
                var dispatchNumber = string.Empty;
                var dateApproved = DateTime.Now.ToString("dd-MMM-yy");
                var costOfBooklet = string.Empty;
                var cancel = string.Empty;
                var mult = string.Empty;
                var cmcCode = string.Empty;
                var batchId = string.Empty;
                var generated = string.Empty;
                var dateOfPrinting = DateTime.Now.ToString("dd-MMM-yy");
                var receivedApprovedBy = string.Empty;
                int lastSerialCounter = 0;
#endregion

                var validateUpload = await MfbExcelValidation(upload);
                //convert excel to list 
                var convertExcelToDT = FileUploadToDataTable(upload);
                //convert databaTable to List
                var convertDtToList = ConvertDataTableToList<MFBIndentsUpload>(convertExcelToDT);
                var listMfb = new List<MFBIndentsUpload>();

                foreach (var item in convertDtToList)
                {
                    var branchParam = new List<IDbDataParameter>();
                    DbConnection connection = null;
                    branchParam.Add(_db.CreateParameter("p_branchname", 100, item.BRANCHNAME, DbType.String, ParameterDirection.Input));
                    branchParam.Add(_db.CreateParameter("p_branchcode", 5, string.Empty, DbType.String, ParameterDirection.Output));
                    branchParam.Add(_db.CreateParameter("p_sortcode", 9, string.Empty, DbType.String, ParameterDirection.Output));
                    branchParam.Add(_db.CreateParameter("p_cmcinuse", 5, string.Empty, DbType.String, ParameterDirection.Output));
                    branchParam.Add(_db.CreateParameter("p_error_msg", 200,string.Empty, DbType.String, ParameterDirection.Output));

                    if(string.IsNullOrEmpty(branchCode))
                    {
                        var getBranchCodeByName = _db.GetExecuteNonQuery(STOREDPROC.GETBRANCHCODEBYBRANCHNAME, CommandType.StoredProcedure, branchParam, 0, out connection);

                        branchCode = getBranchCodeByName.Parameters["p_branchcode"].Value.ToString();
                        sortCode = getBranchCodeByName.Parameters["p_sortcode"].Value.ToString();
                        cmcCode = getBranchCodeByName.Parameters["p_cmcinuse"].Value.ToString();
                        string error = getBranchCodeByName.Parameters["p_error_msg"].Value.ToString();

                        if (!string.IsNullOrEmpty(error))
                        {
                            objResult.Data = null;
                            objResult.Status = false;
                            objResult.Message = error;
                            ErrorWriter.WriteLog($"MFB upload process returned : { objResult.Message } at { DateTime.Now }");
                            return objResult;
                        }
                    }
                    var currentUser = username;
                    var reqId = item.REQUISITION_ID.ToString();
                    
                    micrAccountNo = item.MICR_ACCOUNT_NO;
                    requisitionId = Convert.ToInt32(item.REQUISITION_ID).ToString("D8");
                    approved = "N";
                    branchName = item.BRANCHNAME;
                    cancel = "N";
                    chequeType = GetConfigValue("MFBChequeType");
                    deleted = "N";
                    dispatched = "N";
                    numberOfCheques = item.NUMBEROFCHEQUES;
                    indentSource = GetConfigValue("IndentSource");
                    pulled = "N";
                    acknowledged = "N";
                    printed = "N";
                    scheduleNo = item.SCHEDULE_NO;
                    deliveryBranchCode = item.DELIVERY_BRANCHCODE;
                    customerName = item.CUSTOMER_NAME;
                    recieved = "N";
                    deleted = "N";
                    dispatched = "N";

                    indentId = $"{branchCode}{micrAccountNo}{requisitionId}";

                    var lastSerialParam = new List<IDbDataParameter>();


                    if (lastSerialCounter <= Convert.ToInt32(lastSerialNumber))
                    {
                        var dR = new DataRead();
                        var dt = new DataTable();

                        var ds = dR.GetLastSerialNumberByBranchCode(branchCode);
                        if(ds.Tables[0].Rows.Count < 1)
                        {
                            //SETLAST_SERIAL_NUMBER TO 1
                            lastSerialCounter = 1;

                            var insertParams = new List<IDbDataParameter>();

                            insertParams.Add(_db.CreateParameter("p_branchcode", 13, branchCode, DbType.String, ParameterDirection.Input));
                            insertParams.Add(_db.CreateParameter("p_sortcode", 9, sortCode, DbType.String, ParameterDirection.Input));
                            insertParams.Add(_db.CreateParameter("p_lastserial", 12, 0, DbType.Decimal, ParameterDirection.Input));
                            insertParams.Add(_db.CreateParameter("p_cmccode", 3, cmcCode, DbType.String, ParameterDirection.Input));
                            insertParams.Add(_db.CreateParameter("p_error_msg", 200, string.Empty, DbType.String, ParameterDirection.Output));

                            //INSERT NEW RECORD FOR THE BRANCH
                            _db.InsertWithTransaction(STOREDPROC.INSERTINTOLASTSERIALNUMBER, CommandType.StoredProcedure, insertParams, 0);
                        }
                        else
                        {
                            dt = ds.Tables[0];
                            var getLastSerial = dt.Rows[0]["LAST_SERIAL_NUMBER"].ToString();

                            lastSerialNumber = Convert.ToInt32(getLastSerial);
                            var lastSerialNumberToUse = Convert.ToInt32(lastSerialNumber) + 1;
                            lastSerialCounter = Convert.ToInt32(lastSerialNumberToUse);
                        }

                    }

                    var objMfb = new MFBIndentsUpload()
                    {
                        INDENT_ID = indentId,
                        MICR_ACCOUNT_NO = micrAccountNo,
                        SORTCODE = sortCode,
                        BRANCHNAME = branchName,
                        BRANCHCODE = branchCode,
                        FIRST_CHEQUE_NUMBER = lastSerialCounter.ToString(),
                        NUMBEROFCHEQUES = numberOfCheques,
                        NUMBEROFCHEQUESPRINTED = numberOfChequesPrinted,
                        CHEQUETYPE = chequeType,
                        DATEOFREQUEST = dateOfRequest,
                        DATEENTERED = dateEnterted,
                        PULLED = pulled,
                        DATEPULLED = datePulled,
                        PRINTED = printed,
                        DATEOFPRINTING = dateOfPrinting,
                        RECEIVEDAPPROVED_BY = receivedApprovedBy,
                        ORIGINATOR = currentUser,
                        APPROVED = approved,
                        APPROVER = approver,
                        UPLOADED = uploaded,
                        REQUISITION_ID = requisitionId,
                        SCHEDULE_NO = scheduleNo,
                        INDENTSOURCE = indentSource,
                        DELIVERY_BRANCHCODE = deliveryBranchCode,
                        CUSTOMER_NAME = customerName,
                        RANGED = ranged,
                        RECEIVED = recieved,
                        DATERECEIVED = dateRecieved,
                        DELETED = deleted,
                        ACKNOWLEDGED = acknowledged,
                        DATEACKNOWLEDGED = dateAcknowledged,
                        DISPATCHED = dispatched,
                        DATEDISPATCHED = dateDispatched,
                        DISPATCHNUMBER = dispatchNumber,
                        DATEAPPROVED = dateApproved,
                        COST_OF_BOOKLET = costOfBooklet,
                        CANCEL = cancel,
                        MULT = mult,
                        BATCHID = batchId,
                        GENERATED = generated
                    };

                    lastSerialCounter++;
                    listMfb.Add(objMfb);

                }

                var listMfbDT = new ListtoDataTable().ToDataTable(listMfb);

                _db.BulkCopyWriteToServer(listMfbDT, "INDENTS_TEST");

                //update last serial table with the last id used by branch code
                var updateParam = new List<IDbDataParameter>();
                
                updateParam.Add(_db.CreateParameter("p_lastserialnumber", 100, lastSerialCounter - 1, DbType.String, ParameterDirection.Input));
                updateParam.Add(_db.CreateParameter("p_branchcode", 5, branchCode, DbType.String, ParameterDirection.Input));
                updateParam.Add(_db.CreateParameter("p_error_msg", 200, string.Empty, DbType.String, ParameterDirection.Output));

                _db.Update(STOREDPROC.UPDATELASTSERIALNUMBER, CommandType.StoredProcedure, updateParam, 0);

                objResult.Data = null;
                objResult.Status = true;
                objResult.Message = listMfbDT.Rows.Count.ToString();

                ErrorWriter.WriteLog($"MFB upload process returned : { objResult.Message } indents processed at { DateTime.Now }");

                return objResult;

            }
            catch (Exception ex)
            {
                var error = await ExceptionRefiner.LogError(ex);
                ErrorWriter.WriteLog($"MFB upload process returned error: { ex.Message } at { DateTime.Now }");
                return new DataResult<MfbViewModel>()
                {
                    Data = null,
                    Message = error,
                    Status = false
                };
            }
        }

    }
}
