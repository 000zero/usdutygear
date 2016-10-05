namespace USDutyGear.UPS.Models
{
    public class ShipmentRequestInfo
    {
        public ShipmentRequestInfo()
        {
            Request = new RequestInfo();
            Shipment = new Shipment();
            LabelSpecification = new LabelSpecs();
        }

        public RequestInfo Request { get; set; }
        public Shipment Shipment { get; set; }
        public LabelSpecs LabelSpecification { get; set; }
    }
}
