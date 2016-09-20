namespace USDutyGear.UPS.Models
{
    public class Dimensions
    {
        public Dimensions()
        {
            UnitOfMeasurement = new UnitOfMeasurement();
        }

        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
