using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.ViewModels
{
    public class ReportViewModel
    {
        public string ACCOUNTNO { get; set; }
        public Int64 CHEQUENUMBER { get; set; }
        public string BRANCHNAME { get; set; }
        public string DEL_BRANCH_ADD { get; set; }
        public Int64 NUMBER_OF_LEAVES { get; set; }
        public string CHEAQUETYPE { get; set; }
        public string BATCHID { get; set; }
        public DateTime DATEGENERATED { get; set; }
        public string INDENTSOURCE { get; set; }
    }
}
