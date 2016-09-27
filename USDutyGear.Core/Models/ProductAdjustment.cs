using System.Text.RegularExpressions;

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
        public string DependentModelsRegexStr { get; set; }

        private Regex _dependentModelRegex;
        public Regex DependentModelsRegex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DependentModelsRegexStr))
                    return null;

                // a regex string is specified
                return _dependentModelRegex ?? (_dependentModelRegex = new Regex(DependentModelsRegexStr));
            }
        }
    }
}
