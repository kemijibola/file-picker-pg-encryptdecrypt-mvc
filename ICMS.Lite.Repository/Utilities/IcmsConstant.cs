using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Utilities
{
    public class IcmsConstant
    {
        public class STOREDPROC
        {
            public static string AUTHUSER = "AUTHENTICATEUSER";
            public static string GETUSERBYUSERID = "GETUSERBYUSERID";
            public static string GENERATEINDENTFORPROC = "GETALLGENERATEDFORPROCESSING";
            public static string GENERATEDINDENTLIST = "GETAVAILABLEINDENTS";
            public static string INSERTMFBUPLOAD = "INSERTMFBUPLOAD";
            public static string UPLOADMFBEXCEL = "UPLOADMFBBULK";
            public static string UPDATEGENERATEDINDENTS = "UPDATEINDENTSFROMFINACCE";
            public static string ADDINDENTSTOGINDENTSTBL = "ADDINDENTS";
            public static string GETGENERATEDINDENTSBYACCTNO = "GETGENINDENTSBYACCTNO";
            public static string UPDATEINDENTTABLE = "MyPackage.GetMyTableByIDs";
            public static string GETBRANCHCODEBYBRANCHNAME = "GETBRANCHCODEBYBRANCHNAME";
            public static string GETLASTSERIALNUMBER = "GETLASTSERIALNUM";
            public static string UPDATELASTSERIALNUMBER = "UPDATELASTSERIALNUMBER";
            public static string INSERTINTOLASTSERIALNUMBER = "INSERTNEWLASTSERIAL";
        }

        public class PlatformCustomException : Exception
        {
            public PlatformCustomException(string message)
            : base(message) { }
        }
    }
}
