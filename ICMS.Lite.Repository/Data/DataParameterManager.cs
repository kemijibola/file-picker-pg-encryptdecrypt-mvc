using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Oracle.DataAccess.Client;

namespace ICMS.Lite.Repository.Data
{
    public class DataParameterManager
    {
        public static DbParameter CreateParameter(string providerName, string name, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            DbParameter parameter = null;
            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    break;
                case "system.data.oracleclient":
                    return CreateOracleParameter(name, value, dbType, direction);
                case "system.data.oleDb":
                    break;
                case "system.data.odbc":
                    break;
            }
            return parameter;
        }

        public static DbParameter CreateParameter(string providerName, string name, int size, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            DbParameter parameter = null;
            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    break;
                case "system.data.oracleclient":
                    return CreateOracleParameter(name, size, value, dbType, direction);
                case "system.data.oleDb":
                    break;
                case "system.data.odbc":
                    break;
            }
            return parameter;
        }

        public static DbParameter CreateParameterDbTypeWithSize(string providerName, string name, int size, OracleDbType dbType, object value, ParameterDirection direction)
        {
            DbParameter parameter = null;
            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    break;
                case "system.data.oracleclient":
                    return CreateOracleParameterWithSize(name, size,value,dbType, direction);
                case "system.data.oleDb":
                    break;
                case "system.data.odbc":
                    break;
            }
            return parameter;
        }

        public static DbParameter CreateParameterArray(string providerName, string name, OracleDbType dbType, object value, ParameterDirection direction)
        {
            DbParameter parameter = null;
            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    break;
                case "system.data.oracleclient":
                    return CreateOracleParameter(name, dbType, value, direction);
                case "system.data.oleDb":
                    break;
                case "system.data.odbc":
                    break;
            }
            return parameter;
        }

        public static DbParameter CreateParameter(string providerName, string name, OracleDbType dbType, object value, ParameterDirection direction = ParameterDirection.Output)
        {
            DbParameter parameter = null;
            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    break;
                case "system.data.oracleclient":
                    return CreateOracleParameter(name, dbType, value, direction);
                case "system.data.oleDb":
                    break;
                case "system.data.odbc":
                    break;
            }
            return parameter;
        }

        private static DbParameter CreateOracleParameterWithSize(string name, int size,object value, OracleDbType dbType, ParameterDirection direction)
        {
            return new OracleParameter
            {
                OracleDbType = dbType,
                Size = size,
                ParameterName = name,
                Direction = direction,
                Value = value
            };
        }

        private static DbParameter CreateOracleParameter(string name, object value, DbType dbType, ParameterDirection direction)
        {
            return new OracleParameter
            {
                DbType = dbType,
                ParameterName = name,
                Direction = direction,
                Value = value
            };
        }
        private static DbParameter CreateOracleParameter(string name, OracleDbType dbType, object value, ParameterDirection direction)
        {
            return new OracleParameter
            {
                OracleDbType = dbType,
                ParameterName = name,
                Direction = direction,
                Value = value,
                ArrayBindSize  =  new int[] { 255, 255, 255 }
        };
            
            

        }
        private static DbParameter CreateOracleParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return new OracleParameter
            {
                DbType = dbType,
                Size = size,
                ParameterName = name,
                Direction = direction,
                Value = value
            };
        }
    }
}
