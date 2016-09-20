namespace USDutyGear.UPS.Models
{
    public class Package
    {
        public Package()
        {
            PackagingType = new PackageType();
            Dimensions = new Dimensions();
            PackageWeight = new PackageWeight();
        }

        public PackageType PackagingType { get; set; }
        public Dimensions Dimensions { get; set; }
        public PackageWeight PackageWeight { get; set; }
    }
}
