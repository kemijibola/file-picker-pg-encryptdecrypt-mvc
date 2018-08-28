using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Data
{
    public class OracleDataAccess : IDatabaseHandler
    {
        private string _connectionString { get; set; }

        public OracleDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            return new OracleConnection(_connectionString);
        }
        
        public OracleConnection CreateConnectionForBulkCopy()
        {
            return new OracleConnection(_connectionString);
        }
        public void CloseConnection(DbConnection connection)
        {
            var oracleConnection = (OracleConnection)connection;
            oracleConnection.Close();
            oracleConnection.Dispose();
        }
        public DbCommand CreateCommand(string commandText, CommandType commandType, DbConnection connection, int commandTimeout)
        {
            return new OracleCommand
            {
                CommandText = commandText,
                Connection = (OracleConnection)connection,
                CommandType = commandType,
                CommandTimeout = commandTimeout
            };
        }

        public DbCommand CreateCommandArray(string commandText, CommandType commandType, DbConnection connection, int arraySize, int commandTimeout)
        {
            return new OracleCommand
            {
                CommandText = commandText,
                Connection = (OracleConnection)connection,
                CommandType = commandType,
                CommandTimeout = commandTimeout
            };
        }

        public DataAdapter CreateAdapter(DbCommand command)
        {
            return new OracleDataAdapter((OracleCommand)command);
        }
        public DbParameter CreateParameter(DbCommand command)
        {
            OracleCommand SQLcommand = (OracleCommand)command;
            return SQLcommand.CreateParameter();
        }
    }
}
