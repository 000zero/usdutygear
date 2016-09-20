namespace USDutyGear.UPS.Models
{
    public class Dimensions
    {
        public Dimensions()
        {
            UnitOfMeasurement = new CodeSet();
        }

        public CodeSet UnitOfMeasurement { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }
}
