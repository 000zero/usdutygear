namespace USDutyGear.UPS.Models
{
    public class Package
    {
        public Package()
        {
            PackagingType = new CodeSet();
            Dimensions = new Dimensions();
            PackageWeight = new PackageWeight();
        }

        public CodeSet Packaging { get; set; }
        public CodeSet PackagingType { get; set; }
        public Dimensions Dimensions { get; set; }
        public PackageWeight PackageWeight { get; set; }
    }
}
