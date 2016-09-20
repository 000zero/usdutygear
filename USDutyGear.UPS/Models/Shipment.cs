namespace USDutyGear.UPS.Models
{
    public class Shipment
    {
        public Shipment()
        {
            Shipper = new ShippingInfo();
            ShipTo = new ShippingInfo();
            Package = new Package();
            ShipmentRatingOptions = new ShipmentRatingOptions();
        }

        public ShippingInfo Shipper { get; set; }
        public ShippingInfo ShipTo { get; set; }
        public Package Package { get; set; }
        public ShipmentRatingOptions ShipmentRatingOptions { get; set; }
    }
}
