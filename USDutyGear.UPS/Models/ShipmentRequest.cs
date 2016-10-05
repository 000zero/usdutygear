namespace USDutyGear.UPS.Models
{
    public class ShippingRequest
    {
        public ShippingRequest()
        {
            UPSSecurity = new Security();
            ShipmentRequest = new ShipmentRequestInfo();
        }

        public Security UPSSecurity { get; set; }
        public ShipmentRequestInfo ShipmentRequest { get; set; }
    }
}
