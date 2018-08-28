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
    public interface IDatabaseHandler
    {
        DbConnection CreateConnection();
        OracleConnection CreateConnectionForBulkCopy();
        void CloseConnection(DbConnection connection);
        DbCommand CreateCommand(string commandText, CommandType commandType, DbConnection connection, int commandTimeout);
        DbCommand CreateCommandArray(string commandText, CommandType commandType, DbConnection connection, int arraySize, int commandTimeout);
        DataAdapter CreateAdapter(DbCommand command);
        DbParameter CreateParameter(DbCommand command);
    }
}
