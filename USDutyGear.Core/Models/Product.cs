using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace USDutyGear.Core.Models
{
    public class Product
    {
        public Product()
        {
            IsActive = false;
            FeatureImages = new List<string>();
        }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Model { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string ModelTemplate { get; set; }
        public Regex ModelRegex { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public List<string> FeatureImages { get; set; } 
    }
}
