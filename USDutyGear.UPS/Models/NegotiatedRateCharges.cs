namespace USDutyGear.UPS.Models
{
    public class NegotiatedRateCharges
    {
        public NegotiatedRateCharges()
        {
            TotalCharge = new Charge();
        }

        public Charge TotalCharge { get; set; }
    }
}
