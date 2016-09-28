using System.Collections.Generic;

namespace USDutyGear.Models
{
    public class ProductFeatureViewModel
    {
        public ProductFeatureViewModel()
        {
            Images = new List<string>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public List<string> Images { get; set; }
        public string StartingPrice { get; set; }
    }
}