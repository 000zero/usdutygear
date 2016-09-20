using System.Collections.Generic;

namespace USDutyGear.UPS.Models
{
    public class RatedShipment
    {
        public RatedShipment()
        {
            Service = new CodeSet();
            RatedShipmentAlert = new List<CodeSet>();
            BillingWeight = new PackageWeight();
            TransportationCharges = new Charge();
            ServiceOptionsCharges = new Charge();
            TotalCharges = new Charge();
            RatedPackage = new RatedPackage();
        }

        public CodeSet Service { get; set; }
        public List<CodeSet> RatedShipmentAlert { get; set; }
        public PackageWeight BillingWeight { get; set; }
        public Charge TransportationCharges { get; set; }
        public Charge ServiceOptionsCharges { get; set; }
        public Charge TotalCharges { get; set; }
        public Charge NegotiatedRateCharges { get; set; }
        public RatedPackage RatedPackage { get; set; }
    }
}
