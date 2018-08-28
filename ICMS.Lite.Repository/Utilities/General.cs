using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using ICMS.Lite.Repository.PGP;
using ICMS.Lite.Repository.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static ICMS.Lite.Repository.Utilities.IcmsConstant;

namespace ICMS.Lite.Repository.Utilities
{
    public class General
    {
        public class Auditable : ServerResponse
        {
            public int CreatedBy { get; set; } = 0;
            public DateTime CreatedOn { get; set; }
            public int UpdatedBy { get; set; }
            public DateTime UpdatedOn { get; set; }
        }

        public class DataResult<T> : Auditable
        {
            public T Data { get; set; }
        }

        public class ServerResponse
        {
            public bool Status { get; set; }
            public string Message { get; set; }
        }

        public static string AuthGuard()
        {
            //return "DATA SOURCE=localhost:1521/orcl;PERSIST SECURITY INFO=True;Password=icms;USER ID=ICMS";

            string server = GetConfigValue("Server");
            string uid = HashUtil.DecryptStringValue(GetConfigValue("UserId"));
            string password = HashUtil.DecryptStringValue(GetConfigValue("Password"));

            return "DATA SOURCE=" + server + "; PERSIST SECURITY INFO=True; Password=" +
                password + "; USER ID=" + uid +"";
        }

        public class ICMSConstants
        {
            public static string Approved = "Y";
        }

        public T ConvertDataRowToClass<T>(DataRow dr)
        {
            T item = GetItem<T>(dr);
            return item;
        }

        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T ConvertDataTableToObject<T>(DataTable dt)
        {
            var row = dt.Rows[0];
            T item = GetItem<T>(row);
            return item;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        //public static async Task<bool> ValidatedIndentsForProcessing(List<IndentsToGenerate> model)
        //{
        //    var errorMessage = new StringBuilder();
        //    var totalErrors = 0;
        //    foreach (var item in model)
        //    {
                
        //        var innerErrorMsg = new StringBuilder();

        //        if (item.INDENT_ID.Length < 21 || item.INDENT_ID.Length > 21)
        //            innerErrorMsg.Append($"Indent number # { item.INDENT_ID} is invalid.");

        //        if(innerErrorMsg.Length > 0)
        //        {
        //            totalErrors++;

        //            if (totalErrors <= 5)
        //            {
        //                errorMessage.AppendLine($"Error {totalErrors}: {innerErrorMsg.ToString()} \n\n");
        //            }
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(errorMessage.ToString()))
        //        throw new PlatformCustomException(totalErrors > 5 ? $"{errorMessage.ToString()} \n\n There're {totalErrors - 5} more error(s) found. Attend before proceeding" : errorMessage.ToString());

        //    return true;
        //}

        public static string ServerPath()
        {

            var fileName = $"Indents_{DateTime.Now.ToFileTimeUtc()}";
            var indentPath = GetConfigValue("GeneratedIndentLocation");

            return Path.Combine(indentPath, fileName + ".xlsx");
        }

        public static void UploadExcelToSFTP(string host, string username, string password, string sourcefile, string destinationpath, int port)
        {

            //SFTP.Put(file_name, sftp_Servername);

            //IPHostEntry ip = Dns.GetHostByName(host);
            //SftpClient c = new SftpClient(ip.ToString(), username, password);
            //c.Connect();

            //using (var client = new SftpClient(host, port, username, password))
            //{
            //    client.Connect();
            //    client.ChangeDirectory(destinationpath);
            //    using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
            //    {
            //        client.BufferSize = 4 * 1024;
            //        client.UploadFile(fs, Path.GetFileName(sourcefile));
            //    }
            //}
        }

        public static DataTable ParseExcelNow(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {

                IExcelDataReader excelReader;

                DataSet result = null;

                bool isxls = filePath.EndsWith(".xls");
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                if (isxls)
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    
                }
                else
                {
                    //...
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                //...
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                var dt = new DataTable();

                //result = excelReader.AsDataSet();

                result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                if (result != null && result.Tables.Count > 0)
                {

                    try
                    {

                        dt = result.Tables[0];

                    }
                    catch
                    {
                        dt = result.Tables["Sheet1"];
                    }
                    var b = from row in dt.AsEnumerable() where !string.IsNullOrEmpty(row[0].ToString().Trim()) select row;

                    if (b.Count() != 0) dt = b.CopyToDataTable();
                }

                excelReader.Close();

                stream.Dispose();


                return dt;
            }
            catch (Exception r)
            {
                if (r.Message.ToLower().Contains("cannot find table 0"))
                {
                    throw new Exception("No Record Found");
                }
                else
                {
                    throw (r);
                }
            }
            finally
            {
                stream.Close();
            }
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        public static async Task<bool> ExportDataToExcelAndEncrypt(DataTable data, string fileName, string fullPath)
        {
          
            try
            {
                FileInfo file = new FileInfo(fullPath);
                using (var wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(data);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    using (var ms = new MemoryStream())
                    {
                        wb.SaveAs(ms);

                        using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                        {
                            ms.WriteTo(fs);
                        
                            fs.Close();
                        }
                    }
                    ReleaseObject(wb);

                    var encryptionLocation = $"{GetConfigValue("EncryptedFileLocation")}/Encrypted.xls.pgp";
                    var encrypLoc = $"{GetConfigValue("EncryptedFileLocation")}";

                    ErrorWriter.WriteLog(encryptionLocation);
                    DeleteExistingFilesInFolder(encrypLoc);

                    var str = new FileStream(encryptionLocation, FileMode.Create);
                    await EncryptExcelContentAndSendToSFTP(str,file);

                    return true;
                }

            }
            catch (Exception ex)
            {
                ErrorWriter.WriteLog(ex.Message);
                return false;
            }

        }


        public static bool DeleteExistingFilesInFolder(string path)
        {
            var di = new DirectoryInfo(path);

            foreach (FileInfo fi in di.GetFiles())
            {
                fi.Delete();
                
            }
            return true;
        }
        private static async Task<bool> EncryptExcelContentAndSendToSFTP(FileStream fs, FileInfo file)
        {
            try
            {
                var clientKey = GetConfigValue("SFPublicKey");
                var pfsPrivateKey = GetConfigValue("PFSPrivateKey");
                var password = GetConfigValue("PGPPassword");
                
                //encrypt the generated file
                var objPgpEncryptionKeys = new PgpEncryptionKeys(clientKey, pfsPrivateKey, password);
                var objPgpEncrypt = new PgpEncrypt(objPgpEncryptionKeys);

                objPgpEncrypt.EncryptAndSign(fs, file);

                //UploadExcelToSFTP(sftpHost, sftpUsername, sftpPassword, file.FullName, sftpDestination, sftpPort);

                return true;
            }
            catch(Exception ex)
            {
                throw new PlatformCustomException(ex.Message);
            }

        }


        public static string GetConfigValue(string _key)
        {
            return ConfigurationManager.AppSettings[_key];
        }
        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        public static async Task<bool> MfbExcelValidation(HttpPostedFileBase uploaded)
        {
            if (uploaded == null)
            {
                throw new PlatformCustomException("No file was uploaded.");
            }
            if (System.IO.Path.GetExtension(uploaded.FileName) != ".xlsx")
            {
                throw new PlatformCustomException("Invalid file uploaded.");
            }

            long micrAccountNo = 0;
            var requisitionId = 0;
            var branchName = string.Empty;
            var noOfCheques = 0;
            var customerName = string.Empty;

            var errorMessage = new StringBuilder();
            var totalErrors = 0;
            var reqIdList = new List<int>();

            using (var package = new ExcelPackage(uploaded.InputStream))
            {

                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();

                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;

                var objRecords = workSheet.Cells.Where(x => x != null).ToList();

                foreach (var p in objRecords)
                {
                    var result = p.Value;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    var innerErrorMsg = new StringBuilder();

                    micrAccountNo = Convert.ToInt64(workSheet.Cells[rowIterator, 1].Value);
                    branchName =  workSheet.Cells[rowIterator, 2].Value.ToString();
                    noOfCheques = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                    requisitionId = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                    customerName = workSheet.Cells[rowIterator, 5].Value.ToString();

                    if (micrAccountNo.ToString().Length < 10 || micrAccountNo.ToString().Length > 10)
                        innerErrorMsg.Append($"Micr_Account_No on row #{ rowIterator} is invalid. Must be 10 digits. ");

                    if (string.IsNullOrEmpty(branchName))
                        innerErrorMsg.Append($"Branch name on row #{ rowIterator} can not be empty. ");

                    if (noOfCheques < 25 )
                        innerErrorMsg.Append($"Number of cheques on row #{ rowIterator} is invalid. Minimum number of cheques is 25.");

                    if (requisitionId < 1)
                        innerErrorMsg.Append($"Requisition Id on row #{ rowIterator} can not be empty");

                    if (string.IsNullOrEmpty(customerName))
                        innerErrorMsg.Append($"Customer name on row #{ rowIterator} can not be empty.");

                    if (innerErrorMsg.Length > 0)
                    {
                        totalErrors++;
                        if (totalErrors < 5)
                        {
                            errorMessage.AppendLine($"Data {rowIterator} error: {innerErrorMsg.ToString()} ");
                        }
                    }
                    reqIdList.Add(requisitionId);
                }
                workSheet.Dispose();
                package.Dispose();

                var reqDuplicate = reqIdList.GroupBy(x => x)
                             .Where(g => g.Count() > 1)
                             .Select(g => g.Key)
                             .ToList();

                if(reqDuplicate.Count > 0)
                {
                    var duplicateToString = new StringBuilder();
                    for(int i = 0; i < reqDuplicate.Count; i++)
                    {
                        duplicateToString.AppendLine($"{ reqDuplicate[i].ToString()}");
                    }
                    throw new PlatformCustomException($"{ duplicateToString } is duplicated.");
                }

                if (!string.IsNullOrEmpty(errorMessage.ToString()))
                    throw new PlatformCustomException(totalErrors > 5 ? $"{errorMessage.ToString()} \n\n There're {totalErrors - 5} more error(s) found. Attend before proceeding" : errorMessage.ToString());

                return true;
            }
        }

        public static DataTable FileStreamToDataTable(MemoryStream ms)
        {
            DataTable dt = new DataTable();
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                 dt = package.ToDataTable();
            }
            return dt;
        }
    
    public static DataTable FileUploadToDataTable(System.Web.HttpPostedFileBase file)
    {
            DataTable dt = new DataTable();

            if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                using (ExcelPackage package = new ExcelPackage(file.InputStream))
                {
                    dt = package.ToDataTable();
                }


            }

            return dt;
        }
    }

    public static class ExcelPackageExtensions
    {
        public static DataTable ToDataTable(this ExcelPackage package)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }
    }

    public class ListtoDataTable
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
