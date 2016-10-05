namespace USDutyGear.UPS.Models
{
    public class Shipment : RateShipment
    {
        public Shipment()
        {
            PaymentInformation = new PaymentInfo();
            Service = new CodeSet();
        }

        public string Description { get; set; }
        public ShippingInfo Shipper { get; set; }
        public ShippingInfo ShipTo { get; set; }
        public Package Package { get; set; }
        public ShipmentRatingOptions ShipmentRatingOptions { get; set; }
        // TODO: see if below works
        public PaymentInfo PaymentInformation { get; set; }
        public CodeSet Service { get; set; }
        
    }

    public class RateShipment
    {
        public RateShipment()
        {
            Shipper = new ShippingInfo();
            ShipTo = new ShippingInfo();
            Package = new Package();
            ShipmentRatingOptions = new ShipmentRatingOptions();
        }

        public string Description { get; set; }
        public ShippingInfo Shipper { get; set; }
        public ShippingInfo ShipTo { get; set; }
        public Package Package { get; set; }
        public ShipmentRatingOptions ShipmentRatingOptions { get; set; }
    }
}
