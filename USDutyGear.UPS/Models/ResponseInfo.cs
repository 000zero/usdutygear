using System.Collections.Generic;

namespace USDutyGear.UPS.Models
{
    public class ResponseInfo
    {
        public ResponseInfo()
        {
            ResponseStatus = new CodeSet();
            Alert = new List<CodeSet>();
            TransactionReference = new TransactionRef();
        }

        public CodeSet ResponseStatus { get; set; }
        public List<CodeSet> Alert { get; set; }
        public TransactionRef TransactionReference { get; set; }
    }
}
