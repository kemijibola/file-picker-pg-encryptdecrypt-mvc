using ICMS.Lite.Repository.Data;
using ICMS.Lite.Repository.PGP;
using ICMS.Lite.Repository.Utilities;
using ICMS.Lite.Repository.ViewModels;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.Utilities.IcmsConstant;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Repository.Repositories
{
    
    public class IndentsRepository : IIndentsRepository
    {
        public IndentsRepository()
        {
            _connectionString = AuthGuard();
            _db = new DBManager(_connectionString);
        }
        private readonly DBManager _db;
        private static string _connectionString;
        

        public async Task<DataResult<IndentsToGenerate>> GENERATEDINDENTSANDENCRYPT(string userId)
        {
            var objIndents = new DataResult<IndentsToGenerate>();
            var parameter = new List<IDbDataParameter>();
            var exportDataToExcel = false;
            var listIndents = new List<GeneratedIndents>();
            //var copyOfGeneratedIndents = new DataTable();
            DbConnection db = null;
            var transCode = GetConfigValue("TransactionCode");


            try
            {
                parameter.Add(_db.CreateParameter("P_ERROR_MSG", string.Empty, DbType.String));
                parameter.Add(_db.CreateParameter("P_INDENTGENERATED", OracleDbType.RefCursor, null));
                parameter.Add(_db.CreateParameter("p_transcode", 13, transCode, DbType.String, ParameterDirection.Input));

                var generateIndentsForProcessing = _db.GetDataTable(STOREDPROC.GENERATEINDENTFORPROC, CommandType.StoredProcedure, 0, parameter);
                if (generateIndentsForProcessing.Rows.Count == 0)
                {
                    objIndents.Data = null;
                    objIndents.Status = false;
                    objIndents.Message = "No record currently available for processing.";
                    ErrorWriter.WriteLog($"Fetch indetns for encryption returned : { objIndents.Message } at { DateTime.Now }");

                    return objIndents;
                }

                //This part is used to remove indentId column from excel report. Uncomment if you want to generate indents without Indent_Id
                //copyOfGeneratedIndents = generateIndentsForProcessing.Copy();
                //generateIndentsForProcessing.Columns.RemoveAt(0);

                DeleteExistingFilesInFolder(GetConfigValue("GeneratedIndentLocation"));
                var fullPath = ServerPath();
                var fileName = Path.GetFileName(fullPath);
                exportDataToExcel = await ExportDataToExcelAndEncrypt(generateIndentsForProcessing, fileName, fullPath);

                var convertDTToList = ConvertDataTableToList<IndentsToGenerate>(generateIndentsForProcessing);

                string id = DateTime.Now.ToFileTimeUtc().ToString();


                foreach (var item in convertDTToList)
                {
                    var model = new GeneratedIndents();
                    model.GENERATED_ID = id;
                    model.BATCHID = id;
                    model.INDENT_ID = item.INDENT_ID;
                    model.STATUSID = 0;
                    model.DOM_BRANCH_ADD = item.DOMICILEBRANCHADDRESS;
                    model.DOM_BRANCH_CODE = item.DOMICILEBRANCHCODE;
                    model.SORTCODE = item.SORTCODE;
                    model.CHEAQUETYPE = item.CHEQUETYPE;
                    model.NUMBER_OF_LEAVES = item.NOOFLEAVES.ToString();
                    model.DEL_BRANCH_ADD = item.DELIVERYBRANCHADDRESS;
                    model.SCHEMECODE = item.SCHEMECODE;
                    model.DEL_BRANCH_CODE = item.DELIVERYBRANCHCODE;
                    model.RANGE_START = item.RANGESTART.ToString();
                    model.RANGE_STOP = item.RANGESTOP.ToString();
                    model.GENERATEDBY = userId;
                    model.GENERATEDON = DateTime.Now.ToString("dd-MMMM-yy");
                    model.SECNUM = null;
                    model.DATECAPTURED = DateTime.Now.ToString("dd-MMMM-yy");
                    model.ACCOUNTNO = item.ACCOUNTNUMBER;
                    model.ACCOUNTNAME = item.ACCOUNTNAME;
                    model.CURRENCY = item.CURRENCY;

                    listIndents.Add(model);
                }

                var listIndentDT = new ListtoDataTable().ToDataTable(listIndents);

                var updateParam = new List<IDbDataParameter>();
                updateParam.Add(_db.CreateParameter("P_ERROR_MSG", 200, string.Empty, DbType.String, ParameterDirection.Output));

                var updateCommand = _db.UpdateWithTransaction(STOREDPROC.UPDATEGENERATEDINDENTS, CommandType.StoredProcedure, updateParam, 0);

                _db.BulkCopyWriteToServer(listIndentDT, "GENERATEDINDENTS");

                objIndents.Data = null;
                objIndents.Status = true;
                objIndents.Message = generateIndentsForProcessing.Rows.Count.ToString();
                return objIndents;
            }
            catch(Exception ex)
            {
                var error = await ExceptionRefiner.LogError(ex);
                ErrorWriter.WriteLog($"Encryption process returned error: { ex.Message } at { DateTime.Now }");
                objIndents.Data = null;
                objIndents.Status = false;
                objIndents.Message = error;

                return objIndents;
            }
            
        }
        public async Task<DataResult<List<AvailableIndentsViewModel>>> GETINDENTSFORENCRYPTION()
        {
            var objIndents = new DataResult<List<AvailableIndentsViewModel>>();
            try
            {
                var parameter = new List<IDbDataParameter>();

                parameter.Add(_db.CreateParameter("P_ERROR_MSG", string.Empty, DbType.String));
                parameter.Add(_db.CreateParameter("P_INDENTFORENCRYPTION", OracleDbType.RefCursor, null));

                var indentsDT = _db.GetDataTable(STOREDPROC.GENERATEDINDENTLIST, CommandType.StoredProcedure, 0, parameter);
                if (indentsDT.Rows.Count == 0)
                {
                    objIndents.Data = null;
                    objIndents.Status = false;
                    objIndents.Message = "No record currently available for processing";
                }

                var indentsList = ConvertDataTableToList<AvailableIndentsViewModel>(indentsDT);

                objIndents.Data = indentsList;
                objIndents.Status = true;
                objIndents.Message = "Successful";
            }
            catch(Exception ex)
            {
                var error = await ExceptionRefiner.LogError(ex);
                ErrorWriter.WriteLog($"Fetch indents returned error: { ex.Message } at { DateTime.Now }");
                objIndents.Data = null;
                objIndents.Status = false;
                objIndents.Message = error;
            }

            ErrorWriter.WriteLog($"Fetch indents returned : { objIndents.Message } at { DateTime.Now }");
            return objIndents;
        }
        public async Task<DataResult<DecryptViewModel>> DECRYPTINDENTS(DecryptViewModel model)
        {
            var objDecrypt = new DataResult<DecryptViewModel>();
            var listOfIndentId = new List<string>();
            var listSec = new List<SecurityNumberUpdateVM>();
            try
            {
                var dModel = new DecryptViewModel();

                string outputFile = Path.Combine($"{model.OutputPath}\\DecryptedFile.xls");

                var decrypt = PGPDecryptMain.Decrypt(model.EncryptedFilePath, GetConfigValue("PFSPrivateKey"), HashUtil.DecryptStringValue(GetConfigValue("PGPPassword")), outputFile);
                dModel.OutputPath = outputFile;
                if(!string.IsNullOrEmpty(dModel.OutputPath))
                {
                   
                   var excelToDT = ParseExcelNow(outputFile);

                    var excelToList = ConvertDataTableToList<ExcelFileViewModel>(excelToDT).Select(x => new ExcelFileViewModel {
                        //INDENTID =  x.INDENTID.Remove(0,1),
                        INDENTID = x.INDENTID,
                        ACCOUNTNUMBER = x.ACCOUNTNUMBER,
                        ACCOUNTNAME =  x.ACCOUNTNAME,
                        //DOMICILEBRANCHNAME = x.DOMICILEBRANCHNAME,
                        DOMICILEBRANCHADDRESS = x.DOMICILEBRANCHADDRESS,
                        SORTCODE = x.SORTCODE,
                        CHEQUETYPE = x.CHEQUETYPE,
                        NOOFLEAVES = x.NOOFLEAVES,
                        DELIVERYBRANCHADDRESS = x.DELIVERYBRANCHADDRESS,
                        SCHEMECODE = x.SCHEMECODE,
                        DOMICILEBRANCHCODE = x.DOMICILEBRANCHCODE,
                        //DELIVERYBRANCHCODE = x.DELIVERYBRANCHCODE,
                        RANGESTART = x.RANGESTART,
                        RANGESTOP = x.RANGESTOP,
                        STARTAUDIT = x.STARTAUDIT

                    });
                    
                    var accountNo = excelToList.FirstOrDefault().ACCOUNTNUMBER;

                    var parameter = new List<IDbDataParameter>();
                    parameter.Add(_db.CreateParameter("P_ACCOUNTNO", accountNo, OracleDbType.Varchar2, ParameterDirection.Input));
                    parameter.Add(_db.CreateParameter("P_BATCHGENERATED", OracleDbType.RefCursor, null));
                    parameter.Add(_db.CreateParameter("P_ERROR_MSG", 100, string.Empty, DbType.String, ParameterDirection.Output));

                    var generatedIndentsByAccountNo = _db.GetDataTable(STOREDPROC.GETGENERATEDINDENTSBYACCTNO, CommandType.StoredProcedure, 0, parameter);

                    if (generatedIndentsByAccountNo.Rows.Count == 0)
                    {
                        objDecrypt.Data = null;
                        objDecrypt.Status = true;
                        objDecrypt.Message = "No matching record in database";
                    }
                    var generatedIndentsByAccountNoToList = ConvertDataTableToList<LocalValidateIndents>(generatedIndentsByAccountNo);

                    foreach (var item in excelToList)
                    {
                        var objIndent = generatedIndentsByAccountNoToList.Where(x => x.ACCOUNTNO == item.ACCOUNTNUMBER && x.INDENT_ID == item.INDENTID).Select(x => new CompareObjectFromDb
                        {
                            INDENTID = x.INDENT_ID,
                            ACCOUNTNUMBER = x.ACCOUNTNO,
                            ACCOUNTNAME = x.ACCOUNTNAME,
                            DOMICILEBRANCHADDRESS = x.DOM_BRANCH_ADD,
                            SORTCODE = x.SORTCODE,
                            CHEQUETYPE = x.CHEAQUETYPE,
                            NOOFLEAVES = x.NUMBER_OF_LEAVES,
                            DELIVERYBRANCHADDRESS = x.DEL_BRANCH_ADD,
                            SCHEMECODE = x.SCHEMECODE,
                            DOMICILEBRANCHCODE = x.DOM_BRANCH_CODE,
                            RANGESTART = x.RANGE_START,
                            RANGESTOP = x.RANGE_STOP,
                            

                        }).FirstOrDefault();

                        if(objIndent == null)
                        {
                            objDecrypt.Data = null;
                            objDecrypt.Status = false;
                            objDecrypt.Message = "Excel record(s) does not match generated indents";
                            ErrorWriter.WriteLog($"Decryption process returned : { objDecrypt.Message } at { DateTime.Now }");

                            return objDecrypt;
                        }

                        if( item.CHEQUETYPE != objIndent.CHEQUETYPE || item.DELIVERYBRANCHADDRESS != objIndent.DELIVERYBRANCHADDRESS || item.DOMICILEBRANCHADDRESS != objIndent.DOMICILEBRANCHADDRESS
                            ||item.DOMICILEBRANCHCODE != objIndent.DOMICILEBRANCHCODE || item.RANGESTART != Convert.ToDouble(objIndent.RANGESTART) || item.RANGESTOP != Convert.ToDouble(objIndent.RANGESTOP)
                            || item.NOOFLEAVES != Convert.ToDouble(objIndent.NOOFLEAVES ) || item.SORTCODE != objIndent.SORTCODE)
                        {
                            objDecrypt.Data = null;
                            objDecrypt.Status = false;
                            objDecrypt.Message = "Excel record(s) does not match generated indents";
                            ErrorWriter.WriteLog($"Decryption process returned : { objDecrypt.Message } at { DateTime.Now }");

                            return objDecrypt;
                        }
                        listOfIndentId.Add(item.INDENTID);

                        var secModel = new SecurityNumberUpdateVM()
                        {
                            ACCOUNTNUMBER = item.ACCOUNTNUMBER,
                            APPROVED = "0",
                            CHEQUESTATE = "0",
                            CHEQUE_NUMBER = item.RANGESTART.ToString(),
                            CMC_CODE = "985",
                            DATECAPTURED = DateTime.Now.ToString("dd-MMM-yy"),
                            INDENT_ID = item.INDENTID,
                            INITUPLOAD = "1",
                            NUMBER_OF_CHEQUES_PRINTED = item.NOOFLEAVES.ToString(),
                            OPERATOR_ID = "Superflux",
                            PREFIX = "PO",
                            REPLACES = "",
                            REPLACESPREFIX = "",
                            SECURITY_NUMBER = item.STARTAUDIT.ToString(),
                            SEQ = "",
                            SN = "1",
                            UPLOADED = "0"
                        };

                        listSec.Add(secModel);

                    }

                    using (var orcl = new OracleConnection(_connectionString))
                    {
                        orcl.Open();
                        var command = new OracleCommand()
                        {
                            Connection = orcl,
                            CommandText = STOREDPROC.UPDATEINDENTTABLE,
                            CommandType = CommandType.StoredProcedure,
                            BindByName = true
                        };

                        command.Parameters.Add(new OracleParameter("P_ERROR_MSG", OracleDbType.Varchar2)).Direction = ParameterDirection.Output;
                        command.Parameters.Add(new OracleParameter("n_array", OracleDbType.Varchar2)
                        {
                            CollectionType = OracleCollectionType.PLSQLAssociativeArray,
                            Value = listOfIndentId.ToArray()
                        });
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch(Exception ex)
                        {
                            ErrorWriter.WriteLog($"Decryption process returned error : { ex.Message } at { DateTime.Now }");
                        }
                        finally
                        {
                            orcl.Close();
                        }

                        var listSecDT = new ListtoDataTable().ToDataTable(listSec);
                        _db.BulkCopyWriteToServer(listSecDT, "SECURITYCHEQUENUMBEROLD");
                    }
                }

                objDecrypt.Status = true;
                objDecrypt.Message = $"File successfully decrypted to { outputFile } at { DateTime.Now }";
                objDecrypt.Data = dModel;

                ErrorWriter.WriteLog($"Decryption process returned : { objDecrypt.Message } at { DateTime.Now }");

                return objDecrypt;
            }
            catch(Exception ex)
            {
                var error = await ExceptionRefiner.LogError(ex);
                ErrorWriter.WriteLog($"Decryption process returned error: { ex.Message } at { DateTime.Now }");
                objDecrypt.Status = false;
                objDecrypt.Message = error;
                objDecrypt.Data = null;

                return objDecrypt;
            }
            
            
        }

    }
}
