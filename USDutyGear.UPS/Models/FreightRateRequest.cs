namespace USDutyGear.UPS.Models
{
    public class FreightRateRequest
    {
        public FreightRateRequest()
        {
            Security = new Security();
            RateRequest = new FreightRateRequestInfo();
        }

        public Security Security { get; set; }
        public FreightRateRequestInfo RateRequest { get; set; }
    }
}
