using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICMS.Lite.Repository.Utilities
{
    public class ErrorWriter
    {
        HttpContext context = HttpContext.Current;
        public static void WriteLog(string msg)
        {
            HttpContext context = HttpContext.Current;
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Logs/ErrorLog.txt");
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(msg);
            writer.WriteLine(DateTime.Now);
            writer.Close();
        }
    }
}
