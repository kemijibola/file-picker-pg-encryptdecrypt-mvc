using ICMS.Lite.Repository.Utilities;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Data
{
    public class DataRead
    {
        private readonly string _securedConnectionString = General.AuthGuard();

        public DataSet ExecSQL(string sqlText, string tableName)
        {
            DataSet ds = new DataSet();
            using (var ocon = new OracleConnection(_securedConnectionString))
            {
                ocon.Open();
                var cmd = new OracleCommand(sqlText, ocon);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(ds);
                ds.Tables[0].TableName = tableName;
            }
            return ds;
        }

        public int Insert(string sqlText)
        {
            int lastId = 0;
            using (var ocon = new OracleConnection(_securedConnectionString))
            {
                ocon.Open();
                var cmd = new OracleCommand(sqlText, ocon);
                //if(parameters != null)
                //{
                //    cmd.Parameters.AddRange(parameters);
                //}

                object newId = cmd.ExecuteScalar();
                lastId = Convert.ToInt32(newId);

            }
            return lastId;
        }

        public DataSet ExecSQL(string sqlText, OracleParameter[] parameters, string tableName)
        {
            var ds = new DataSet();
            using (var ocon = new OracleConnection(_securedConnectionString))
            {
                ocon.Open();
                var cmd = new OracleCommand(sqlText, ocon);
                cmd.Parameters.AddRange(parameters);

                var da = new OracleDataAdapter(cmd);
                da.Fill(ds);
                ds.Tables[0].TableName = tableName;
            }
            return ds;

        }

        public DataSet GetLastSerialNumberByBranchCode(string branchCode)
        {
            OracleParameter[] param = {
                new OracleParameter("ITEMCODE", OracleDbType.Varchar2) { Value = branchCode }
            };
            string sqlText = "SELECT LAST_SERIAL_NUMBER FROM LAST_SERIAL_NUMBER_STORE WHERE ITEMCODE IN (:ITEMCODE)";

            var ds = ExecSQL(sqlText, param, "LAST_SERIAL_NUMBER_STORE");

            return ds;
        }

        public DataSet GETINDENTSREPORTS(string startDate, string endDate)
        {
            OracleParameter[] param = {
                new OracleParameter("GENERATEDON", OracleDbType.Varchar2) { Value = startDate },
                new OracleParameter("GENERATEDON", OracleDbType.Varchar2) { Value = endDate }
            };
            string sqlText = "SELECT a.ACCOUNTNO, a.RANGE_START AS CHEQUENUMBER, i.branchname, i.indentsource, a.DEL_BRANCH_ADD, a.NUMBER_OF_LEAVES, a.CHEAQUETYPE, a.BATCHID, a.GENERATEDON AS DATEGENERATED  FROM GENERATEDINDENTS a, indents_test i WHERE i.indent_id = a.indent_id and a.generatedon between TO_DATE('" + startDate + "', 'YYYY/MM/DD') and TO_DATE('" + endDate + "', 'YYYY/MM/DD') ";
            var ds = ExecSQL(sqlText, param, "GENERATEDINDENTS");

            return ds;
        }

        public int INSERTLASTSERIALNUMBER(string branchCode, string sortCode,string lastSerialNumber, string cmcCode)
        {
            //OracleParameter[] param = {
            //    new OracleParameter("itemCode", OracleDbType.Varchar2) { Value = branchCode },
            //    new OracleParameter("sortCode", OracleDbType.Varchar2) { Value = sortCode },
            //    new OracleParameter("lastSerial", OracleDbType.Varchar2) { Value = lastSerialNumber },
            //    new OracleParameter("cmcCode", OracleDbType.Varchar2) { Value = cmcCode }
            //};
            string sqlText = "INSERT INTO LAST_SERIAL_NUMBER_STORE('ITEMCODE', 'sortcode', 'last_serial_number', 'cmc_code') VALUES("+branchCode+", "+sortCode+", "+lastSerialNumber+", "+cmcCode+") ";

            var newId = Insert(sqlText);
            return newId;
        }
    }
}
