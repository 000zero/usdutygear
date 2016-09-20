namespace USDutyGear.UPS.Models
{
    public class RequestInfo
    {
        public RequestInfo()
        {
            TransactionReference = new TransactionRef();
        }

        public string RequestOption { get; set; }
        public TransactionRef TransactionReference { get; set; }
    }
}
