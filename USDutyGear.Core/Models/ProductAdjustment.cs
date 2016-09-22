namespace USDutyGear.Core.Models
{
    public class ProductAdjustment
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductModel { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal PriceAdjustment { get; set; }
        public string Model { get; set; }
        public int Priority { get; set; }
        public string Display { get; set; }
        public string[] DependentModels { get; set; }
    }
}
