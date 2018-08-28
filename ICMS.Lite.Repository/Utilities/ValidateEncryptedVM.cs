using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Utilities
{
    public class ValidateEncryptedVM : IEquatable<ValidateEncryptedVM>
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


        public bool Equals(ValidateEncryptedVM other)
        {
            if (other == null) return false;

            return string.Equals(INDENTID, other.INDENTID) &&
                   string.Equals(ACCOUNTNUMBER, other.ACCOUNTNUMBER) &&
                   string.Equals(ACCOUNTNAME, other.ACCOUNTNAME) &&
                   string.Equals(DOMICILEBRANCHADDRESS, other.DOMICILEBRANCHADDRESS) &&
                   string.Equals(SORTCODE, other.SORTCODE) &&
                   string.Equals(CHEQUETYPE, other.CHEQUETYPE) &&
                   string.Equals(NOOFLEAVES, other.NOOFLEAVES) &&
                   string.Equals(DELIVERYBRANCHADDRESS, other.DELIVERYBRANCHADDRESS) &&
                   string.Equals(SCHEMECODE, other.SCHEMECODE) &&
                   string.Equals(DOMICILEBRANCHCODE, other.DOMICILEBRANCHCODE) &&
                   string.Equals(RANGESTART, other.RANGESTART) &&
                   string.Equals(RANGESTOP, other.RANGESTOP);                  

        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as ValidateEncryptedVM);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)2166136261;
                var accountHashCode = string.IsNullOrEmpty(INDENTID) ? INDENTID.GetHashCode() : 0;
                var accountNameHashCode = string.IsNullOrEmpty(ACCOUNTNUMBER) ? ACCOUNTNUMBER.GetHashCode() : 0;
                var domBranchNameHashCode = string.IsNullOrEmpty(ACCOUNTNAME) ? ACCOUNTNAME.GetHashCode() : 0;
                var domBranchAddHashCode = string.IsNullOrEmpty(DOMICILEBRANCHADDRESS) ? DOMICILEBRANCHADDRESS.GetHashCode() : 0;
                var sortCodeHashCode = string.IsNullOrEmpty(SORTCODE) ? SORTCODE.GetHashCode() : 0;
                var chequeTypeHashCode = string.IsNullOrEmpty(CHEQUETYPE) ? CHEQUETYPE.GetHashCode() : 0;
                var delBranchAddHashCode = string.IsNullOrEmpty(NOOFLEAVES) ? NOOFLEAVES.GetHashCode() : 0;
                var schemeCodeHashCode = string.IsNullOrEmpty(DELIVERYBRANCHADDRESS) ? DELIVERYBRANCHADDRESS.GetHashCode() : 0;
                var domBranchCodeHashCode = string.IsNullOrEmpty(SCHEMECODE) ? SCHEMECODE.GetHashCode() : 0;
                var delBranchCodeHashCode = string.IsNullOrEmpty(DOMICILEBRANCHCODE) ? DOMICILEBRANCHCODE.GetHashCode() : 0;
                var currencyHashCode = string.IsNullOrEmpty(RANGESTART) ? RANGESTART.GetHashCode() : 0;
                var rangeStop = string.IsNullOrEmpty(RANGESTOP) ? RANGESTOP.GetHashCode() : 0;

                hashCode = (hashCode * 16777619) ^ accountHashCode;
                hashCode = (hashCode * 16777619) ^ accountNameHashCode;
                hashCode = (hashCode * 16777619) ^ domBranchNameHashCode;
                hashCode = (hashCode * 16777619) ^ domBranchAddHashCode;
                hashCode = (hashCode * 16777619) ^ sortCodeHashCode;
                hashCode = (hashCode * 16777619) ^ chequeTypeHashCode;
                hashCode = (hashCode * 16777619) ^ delBranchAddHashCode;
                hashCode = (hashCode * 16777619) ^ domBranchCodeHashCode;
                hashCode = (hashCode * 16777619) ^ delBranchCodeHashCode;
                hashCode = (hashCode * 16777619) ^ currencyHashCode;

                hashCode = (hashCode * 16777619) ^ RANGESTART.GetHashCode();
                hashCode = (hashCode * 16777619) ^ RANGESTOP.GetHashCode();

                return hashCode;
            }
        }
    }

    public class ExcelFileViewModel
    {
        public string ACCOUNTNUMBER { get; set; }
        public string ACCOUNTNAME { get; set; }
        public string INDENTID { get; set; }
        //public string DOMICILEBRANCHNAME { get; set; }
        public string DOMICILEBRANCHADDRESS { get; set; }
        public string SORTCODE { get; set; }
        public string CHEQUETYPE { get; set; }
        public double NOOFLEAVES { get; set; }
        public string DELIVERYBRANCHADDRESS { get; set; }
        public string SCHEMECODE { get; set; }
        public string DOMICILEBRANCHCODE { get; set; }
        //public string DELIVERYBRANCHCODE { get; set; }
        public double RANGESTART { get; set; }
        public double RANGESTOP { get; set; }
        //public string CURRENCY { get; set; }
        public string STARTAUDIT { get; set; }
        //public double ENDAUDIT { get; set; }
        public string PRINTED { get; set; }

    }

}
