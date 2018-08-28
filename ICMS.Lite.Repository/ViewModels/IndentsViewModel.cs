using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.ViewModels
{
    public class IndentsViewModel
    {
    }
    public class LocalValidateIndents
    {

        //public string GENERATED_ID { get; set; }
        //public string BATCHID { get; set; }
        public string INDENT_ID { get; set; }
        public string ACCOUNTNO { get; set; }
        public string ACCOUNTNAME { get; set; }
        //public decimal STATUSID { get; set; }
        public string DOM_BRANCH_ADD { get; set; }
        public string SORTCODE { get; set; }
        public string CHEAQUETYPE { get; set; }
        public string NUMBER_OF_LEAVES { get; set; }
        public string DEL_BRANCH_ADD { get; set; }
        public string SCHEMECODE { get; set; }
        public string DOM_BRANCH_CODE { get; set; }
        public string RANGE_START { get; set; }
        public string RANGE_STOP { get; set; }
        //public string GENERATEDBY { get; set; }
        //public System.DateTime GENERATEDON { get; set; }

        //public System.DateTime DATECAPTURED { get; set; }
       
    }

    public class SecurityNumberUpdateVM
    {
        public string PREFIX { get; set; }
        public string SECURITY_NUMBER { get; set; }
        public string NUMBER_OF_CHEQUES_PRINTED { get; set; }
        public string CHEQUE_NUMBER { get; set; }
        public string INDENT_ID { get; set; }
        public string OPERATOR_ID { get; set; }
        public string SN { get; set; }
        public string UPLOADED { get; set; }
        public string ACCOUNTNUMBER { get; set; }
        public string APPROVED { get; set; }
        public string REPLACES { get; set; }
        public string REPLACESPREFIX { get; set; }
        public string INITUPLOAD { get; set; }
        public string CHEQUESTATE { get; set; }
        public string DATECAPTURED { get; set; }
        public string SEQ { get; set; }
        public string CMC_CODE { get; set; }
    }

    public class CompareObjectFromDb
    {
        public string INDENTID { get; set; }
        public string ACCOUNTNUMBER { get; set; }
        public string ACCOUNTNAME { get; set; }
        public string DOMICILEBRANCHADDRESS { get; set; }
        public string SORTCODE { get; set; }
        public string CHEQUETYPE { get; set; }
        public string NOOFLEAVES { get; set; }
        public string DELIVERYBRANCHADDRESS { get; set; }
        public string SCHEMECODE { get; set; }
        public string DOMICILEBRANCHCODE { get; set; }
        public string RANGESTART { get; set; }
        public string RANGESTOP { get; set; }
        

    }
    public class GeneratedIndents
    {
        public string GENERATED_ID { get; set; }
        public string BATCHID { get; set; }
        public string INDENT_ID { get; set; }
        public decimal STATUSID { get; set; }
        public string DOM_BRANCH_ADD { get; set; }
        public string SORTCODE { get; set; }
        public string CHEAQUETYPE { get; set; }
        public string NUMBER_OF_LEAVES { get; set; }
        public string DEL_BRANCH_ADD { get; set; }
        public string SCHEMECODE { get; set; }
        public string DOM_BRANCH_CODE { get; set; }
        public string RANGE_START { get; set; }
        public string RANGE_STOP { get; set; }
        public string GENERATEDBY { get; set; }
        public string GENERATEDON { get; set; }
        public string SECNUM { get; set; }
        public string DATECAPTURED { get; set; }
        public string ACCOUNTNO { get; set; }
        public string DEL_BRANCH_CODE { get; set; }
        public string ACCOUNTNAME { get; set; }
        public string CURRENCY { get; set; }
    }

    public class DecryptedViewModel
    {
        public string GENERATED_ID { get; set; }
        public string BATCH_ID { get; set; }
        public string INDENT_ID { get; set; }
        public int STATUSID { get; set; }
        public string DOM_BRANCH_ADD { get; set; }
        public string SORTCODE { get; set; }
        public string CHEAQUETYPE { get; set; }
        public string CHEQUETYPENAME { get; set; }
        public int NUMBER_OF_LEAVES { get; set; }
        public string DEL_BRANCH_ADD { get; set; }
        public string DEL_BRANCH_CODE { get; set; }
        public string SCHEMECODE { get; set; }
        public string DOM_BRANCH_CODE { get; set; }
        public long RANGE_START { get; set; }
        public decimal RANGE_STOP { get; set; }
        public string GENERATEDBY { get; set; }
        public string GENERATEDON { get; set; }
        public string CHEQUENUMBER { get; set; }
        public string DATECAPTURED { get; set; }
        public string STARTAUDIT { get; set; }
        public string ENDAUDIT { get; set; }
    }

    public class IndentsToGenerate 
    {
        public string ACCOUNTNUMBER { get; set; }
        public string ACCOUNTNAME { get; set; }
        public string DOMICILEBRANCHNAME { get; set; }
        public string DOMICILEBRANCHADDRESS { get; set; }
        public string SORTCODE { get; set; }
        public string CHEQUETYPE { get; set; }
        public System.Int32 NOOFLEAVES { get; set; }
        public string DELIVERYBRANCHADDRESS { get; set; }
        public string SCHEMECODE { get; set; }
        public string DOMICILEBRANCHCODE { get; set; }
        public string DELIVERYBRANCHCODE { get; set; }
        public System.Int64 RANGESTART { get; set; }
        public decimal RANGESTOP { get; set; }
        public string CURRENCY { get; set; }
        public string INDENT_ID { get; set; }
    }

    public class AvailableIndentsViewModel
    {
        public string INDENT_ID { get; set; }
        public string ACCOUNTNUMBER { get; set; }
        public string ACCOUNTNAME { get; set; }
        public string SORTCODE { get; set; }
        public string CHEQUETYPE { get; set; }
        public int NOOFLEAVES { get; set; }
    }

    public class INDENTS
    {
        public string INDENT_ID { get; set; }
        public string MICR_ACCOUNT_NO { get; set; }
        public string SORTCODE { get; set; }
        public string BRANCHNAME { get; set; }
        public string BRANCHCODE { get; set; }
        public int FIRST_CHEQUE_NUMBER { get; set; }
        public int NUMBEROFCHEQUES { get; set; }
        public int NUMBEROFCHEQUESPRINTED { get; set; }
        public string CHEQUETYPE { get; set; }
        public DateTime DATEOFREQUEST { get; set; }
        public DateTime DATEENTERED { get; set; }
        public string PULLED { get; set; }
        public string DATEPULLED { get; set; }
        public string PRINTED { get; set; }
        public DateTime DATEOFPRINTING { get; set; }
        public string RECEIVEDAPPROVED_BY { get; set; }
        public string ORIGINATOR { get; set; }
        public string APPROVED { get; set; }
        public string APPROVER { get; set; }
        public string UPLOADED { get; set; }
        public int REQUISITION_ID { get; set; }
        public string SCHEDULE_NO { get; set; }
        public string INDENTSOURCE { get; set; }
        public string DELIVERY_BRANCHCODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string RANGED { get; set; }
        public string RECEIVED { get; set; }
        public DateTime DATERECEIVED { get; set; }
        public string DELETED { get; set; }
        public string ACKNOWLEDGED { get; set; }
        public DateTime DATEACKNOWLEDGED { get; set; }
        public string DISPATCHED { get; set; }
        public DateTime DATEDISPATCHED { get; set; }
        public string DISPATCHNUMBER { get; set; }
        public DateTime DATEAPPROVED { get; set; }
        public int COST_OF_BOOKLET { get; set; }
        public string CANCEL { get; set; }
        public string VARCHAR2 { get; set; }
        public int BATCHID { get; set; }
        public string GENERATED { get; set; }
       
    }

    public class DecryptViewModel
    {
        public string EncryptedFilePath { get; set; }
        public string OutputPath { get; set; }
    }

}
