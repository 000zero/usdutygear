namespace USDutyGear.UPS.Models
{
    public class RatedPackage
    {
        public Charge TransportationCharges { get; set; }
        public Charge ServiceOptionsCharges { get; set; }
        public Charge TotalCharges { get; set; }
        public decimal Weight { get; set; }
        public PackageWeight BillingWeight { get; set; }
    }
}
