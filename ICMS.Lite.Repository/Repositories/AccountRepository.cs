using ICMS.Lite.Repository.Data;
using ICMS.Lite.Repository.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.Utilities.IcmsConstant;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Repository.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        public AccountRepository()
        {
            _connectionString = AuthGuard();
            _db = new DBManager(_connectionString);
        }

        private readonly DBManager _db;
        private static string _connectionString;

        public async Task<bool> AUTHENTICATEUSER(LoginViewModel loginModel)
        {
            if (loginModel == null)
                throw new PlatformCustomException("Invalid username or password");
            try
            {
                var objUser = new DataResult<UserViewModel>();
                var parameters = new List<IDbDataParameter>();

                var authorizedRole = GetConfigValue("AuthorizedUserRole");

                //implement password hashing here
                loginModel.Password = HashUtil.EncryptStringValue(loginModel.Password);


                parameters.Add(_db.CreateParameter("p_user_id", 10, loginModel.UserId, DbType.String, ParameterDirection.Input));
                parameters.Add(_db.CreateParameter("p_password", 70, loginModel.Password, DbType.String, ParameterDirection.Input));
                parameters.Add(_db.CreateParameter("p_authorized_role", 70, authorizedRole, DbType.String, ParameterDirection.Input));
                parameters.Add(_db.CreateParameter("p_error_msg", 100, "", DbType.String, ParameterDirection.Output));
                parameters.Add(_db.CreateParameter("p_success", 100, "", DbType.String, ParameterDirection.Output));
                
                DbConnection connection = null;

                var authCommand = _db.GetExecuteNonQuery(STOREDPROC.AUTHUSER, CommandType.StoredProcedure, parameters, 0, out connection);

                string error = authCommand.Parameters["p_error_msg"].Value.ToString();
                string success = authCommand.Parameters["p_success"].Value.ToString();

                if(!string.IsNullOrEmpty(error))
                    throw new PlatformCustomException("Invalid username or password.");

                var message = string.IsNullOrEmpty(error) ? error : success;
                ErrorWriter.WriteLog($"Login operation returned : { message } at { DateTime.Now }");

                return true;            

            }
            catch (Exception ex)
            {
                var error = await ExceptionRefiner.LogError(ex);
                ErrorWriter.WriteLog($"Login operation returned : { ex.Message } at { DateTime.Now }");
                throw new Exception(error);
            }
        }
    }
}
