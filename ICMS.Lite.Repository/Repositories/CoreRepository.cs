using ICMS.Lite.Repository.ViewModels;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using static ICMS.Lite.Repository.Utilities.General;
using ICMS.Lite.Repository.Utilities;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;
using ICMS.Lite.Repository.Data;
using static ICMS.Lite.Repository.Utilities.IcmsConstant;
using System.Data.Common;

namespace ICMS.Lite.Repository.Repositories
{
    public class CoreRepository : ICoreRepository
    {

        public CoreRepository()
        {
            _connectionString = AuthGuard();
            _db = new DBManager(_connectionString);
           
        }
        private readonly DBManager _db;
        private static string _connectionString;

        public async Task<UserViewModel> GETUSERBYUSERID(string username)
        {

            try
            {
                
                var parameters = new List<IDbDataParameter>();

                parameters.Add(_db.CreateParameter("P_USER_ID", 10, username, DbType.String, ParameterDirection.Input));
                parameters.Add(_db.CreateParameter("P_USER_NAME", 10, "", DbType.String, ParameterDirection.Output));
                parameters.Add(_db.CreateParameter("P_USER_ROLE", 10, "", DbType.String, ParameterDirection.Output));
                parameters.Add(_db.CreateParameter("P_APPROVED", 10, "", DbType.String, ParameterDirection.Output));
                parameters.Add(_db.CreateParameter("P_DELETED", 10, "", DbType.String, ParameterDirection.Output));
                parameters.Add(_db.CreateParameter("P_ERROR_MSG", 100, "", DbType.String, ParameterDirection.Output));

                DbConnection connection = null;

                var userCommand = _db.GetExecuteNonQuery(STOREDPROC.GETUSERBYUSERID, CommandType.StoredProcedure, parameters, 0, out connection );

                string uId = userCommand.Parameters["P_USER_ID"].Value.ToString();
                string uname = userCommand.Parameters["P_USER_NAME"].Value.ToString();
                string urole = userCommand.Parameters["P_USER_ROLE"].Value.ToString();
                string uapproved = userCommand.Parameters["P_APPROVED"].Value.ToString();
                string udeleted = userCommand.Parameters["P_DELETED"].Value.ToString();
                string error = userCommand.Parameters["P_ERROR_MSG"].Value.ToString();

                if (string.IsNullOrEmpty(error))
                {
                    var objUser = new UserViewModel()
                    {
                        USER_ID = uId,
                        USER_NAME = uname,
                        USER_ROLE = urole,
                        APPROVED = uapproved,
                        DELETED = udeleted,
                        Status = true,
                        Message = "Success"

                    };

                    ErrorWriter.WriteLog($"Fetch user data returned : { objUser.Message } at { DateTime.Now }");
                    return objUser;
                }
                else
                {
                    ErrorWriter.WriteLog($"Fetch user data returned : { error } at { DateTime.Now }");
                    throw new Exception(error);     
                }

            }
            catch(Exception ex)
            {
                var error = await ExceptionRefiner.LogError(ex);
                ErrorWriter.WriteLog($"Fetch user data returned : { ex.Message } at { DateTime.Now }");

                throw new Exception(error);
            }
        }
    }
}
