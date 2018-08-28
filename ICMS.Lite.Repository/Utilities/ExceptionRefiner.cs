using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Utilities
{
    public class ExceptionRefiner
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public async static Task<string> LogError(Exception ex)
        {
            var errorMessage = string.Empty;

            if (ex == null)
                return errorMessage;

            //Log into file using NLog and log into Database
            if (ex.InnerException != null)
                await LogError(ex.InnerException);
            else
                errorMessage = ex.Message;

            if (errorMessage.Contains("inner exception") || errorMessage.Contains("ORA-"))
                errorMessage = "Error occurred! Contact First Bank's support team.";

            logger.Error($"Error: { errorMessage}");
            return errorMessage;
        }

        public async static Task Log(string message)
        {
            try
            {
                logger.Error($"Error: {message}");
            }
            catch (Exception ex)
            {

            }
        }

    }
}
