namespace USDutyGear.UPS.Models
{
    public class PackageWeight
    {
        public PackageWeight()
        {
            UnitOfMeasurement = new CodeSet();
        }

        public CodeSet UnitOfMeasurement { get; set; }
        public string Weight { get; set; }
    }
}
