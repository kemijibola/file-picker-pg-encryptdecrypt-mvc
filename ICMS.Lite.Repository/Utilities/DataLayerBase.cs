using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Utilities
{
    public class DataLayerBase
    {
        public static void AddParameter(DbCommand command, string parameterName, DbType dbType, object value)
        {
            DbParameter p;
            p = command.CreateParameter();
            p.ParameterName = parameterName;
            p.DbType = dbType;
            p.Value = value;

            command.Parameters.Add(p);
        }

        public static void AddListParameters(List<Tuple<DbCommand, string, DbType, object, ParameterDirection>> parameters)
        {
            Parallel.ForEach(parameters, param =>
            {
                DbParameter p;
                p = param.Item1.CreateParameter();
                p.ParameterName = param.Item2;
                p.DbType = param.Item3;
                p.Value = param.Item4;
                p.Direction = param.Item5;

                param.Item1.Parameters.Add(p);

            });
        }
    }
}
