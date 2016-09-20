namespace USDutyGear.UPS.Models
{
    public class RatesRequest
    {
        public RatesRequest()
        {
            UPSSecurity = new Security();
            RateRequest = new RateRequestInfo();
        }

        public Security UPSSecurity { get; set; }
        public RateRequestInfo RateRequest { get; set; }
    }
}
