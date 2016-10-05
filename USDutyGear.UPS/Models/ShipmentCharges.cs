namespace USDutyGear.UPS.Models
{
    public class ShipmentCharges
    {
        public ShipmentCharges()
        {
            TransportationCharges = new Charge();
            ServiceOptionsCharges = new Charge();
            TotalCharges = new Charge();
        }

        public Charge TransportationCharges { get; set; }
        public Charge ServiceOptionsCharges { get; set; }
        public Charge TotalCharges { get; set; }
    }
}
