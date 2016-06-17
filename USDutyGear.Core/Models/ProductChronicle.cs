using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDutyGear.Core.Models
{
    public class ProductChronicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<string> Model { get; set; }
        public List<string> Finishes { get; set; }
        public List<int> Sizes { get; set; }
        public List<string> Details { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
    }
}
