using System.Collections.Generic;

namespace USDutyGear.Core.Common
{
    public static class Categories
    {
        public const string Belts = "Belts";
        public const string BeltKeepers = "Belt Keepers";
        public const string Pouches = "Pouches";
    }

    public class ProductCategory
    {
        public string Category { get; set; }
        public List<KeyValuePair<string, string>> Products { get; set; }
    }
}
