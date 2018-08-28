using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.ViewModels
{
    public class MfbViewModel : MfbAuditable
    {
        public string INDENT_ID { get; set; }
        public string MICR_ACCOUNT_NO { get; set; }
        public string SORTCODE { get; set; }
        public string BRANCHNAME { get; set; }
        public string BRANCHCODE { get; set; }
        public string FIRST_CHEQUE_NUMBER { get; set; }
        public string NUMBEROFCHEQUES { get; set; }
        public string CHEQUETYPE { get; set; }
        public string REQUISITION_ID { get; set; }
        public string INDENTSOURCE { get; set; }
        public string DELIVERY_NAME { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string RANGED { get; set; }
        public string CMC_CODE { get; set; }
    }

    public class MFBIndentsUpload
    {
        public string INDENT_ID { get; set; }
        public string MICR_ACCOUNT_NO { get; set; }
        public string SORTCODE { get; set; }
        public string BRANCHNAME { get; set; }
        public string BRANCHCODE { get; set; }
        public string FIRST_CHEQUE_NUMBER { get; set; }
        public string NUMBEROFCHEQUES { get; set; }
        public string NUMBEROFCHEQUESPRINTED { get; set; }
        public string CHEQUETYPE { get; set; }
        public string DATEOFREQUEST { get; set; }
        public string DATEENTERED { get; set; }
        public string PULLED { get; set; }
        public string DATEPULLED { get; set; }
        public string PRINTED { get; set; }
        public string DATEOFPRINTING { get; set; }
        public string RECEIVEDAPPROVED_BY { get; set; }
        public string ORIGINATOR { get; set; }
        public string APPROVED { get; set; }
        public string APPROVER { get; set; }
        public string UPLOADED { get; set; }
        public string REQUISITION_ID { get; set; }
        public string SCHEDULE_NO { get; set; }
        public string INDENTSOURCE { get; set; }
        public string DELIVERY_BRANCHCODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string RANGED { get; set; }
        public string RECEIVED { get; set; }
        public string DATERECEIVED { get; set; }
        public string DELETED { get; set; }
        public string ACKNOWLEDGED { get; set; }
        public string DATEACKNOWLEDGED { get; set; }
        public string DISPATCHED { get; set; }
        public string DATEDISPATCHED { get; set; }
        public string DISPATCHNUMBER { get; set; }
        public string DATEAPPROVED { get; set; }
        public string COST_OF_BOOKLET { get; set; }
        public string CANCEL { get; set; }
        public string MULT { get; set; }
        public string BATCHID { get; set; }
        public string GENERATED { get; set; }

    }
    public class MfbAuditable
    {
        public DateTime DATEOFREQUEST { get; set; }
        public DateTime DATEENTERED { get; set; }
        public string ORIGINATOR { get; set; }
    }
}
