using ICMS.Lite.Repository.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Data
{
   public class DatabaseHandlerFactory
   {
        private ConnectionStringSettings connectionStringSettings;
        public DatabaseHandlerFactory(string connectionStringName)
        {
            var conStringSettings =
            connectionStringSettings = ConfigurationManager.ConnectionStrings["ICMSDataConnect"];
        }
        public IDatabaseHandler CreateDatabase()
        {
           IDatabaseHandler database = null;
            var conString = General.AuthGuard();
            switch (connectionStringSettings.ProviderName.ToLower())
                {
                    //case "system.data.sqlclient":
                    //database = new SqlDataAccess(connectionStringSettings.ConnectionString);
                    //    break;
                    case "system.data.oracleclient":
                        database = new OracleDataAccess(conString);
                        break;
                    //case "system.data.oleDb":
                    //    database = new OledbDataAccess(connectionStringSettings.ConnectionString);
                    //    break;
                    //case "system.data.odbc":
                    //    database = new OdbcDataAccess(connectionStringSettings.ConnectionString);
                    //    break;
                 }
               return database;
        }
        public string GetProviderName()
        {
            return connectionStringSettings.ProviderName;
        }
   }
    
}
