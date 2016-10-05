namespace USDutyGear.UPS.Models
{
    public class ShipmentResults
    {
        public ShipmentResults()
        {
            ShipmentCharges = new ShipmentCharges();
            NegotiatedRateCharges = new NegotiatedRateCharges();
            BillingWeight = new PackageWeight();
        }

        public ShipmentCharges ShipmentCharges { get; set; }
        public NegotiatedRateCharges NegotiatedRateCharges { get; set; }
        public PackageWeight BillingWeight { get; set; }
        public string ShipmentIdentificationNumber { get; set; }
    }
}
