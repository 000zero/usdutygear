namespace USDutyGear.UPS.Models
{
    public class FreightRateRequestInfo
    {
        public FreightRateRequestInfo()
        {
            Request = new RequestInfo();
            Shipment = new Shipment();
        }

        public RequestInfo Request { get; set; }
        public Shipment Shipment { get; set; }
    }
}
