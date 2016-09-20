namespace USDutyGear.UPS.Models
{
    public class PackageWeight
    {
        public PackageWeight()
        {
            UnitOfMeasurement = new UnitOfMeasurement();
        }

        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public int Weight { get; set; }
    }
}
