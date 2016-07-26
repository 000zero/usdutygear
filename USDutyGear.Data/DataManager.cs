using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDutyGear.Core.Models;
using USDutyGear.Core.Common;

namespace USDutyGear.Data
{
    public static class DataManager
    {
        public static List<Product> Products { get; set; }

        static DataManager()
        {
            Products = new List<Product>();

            
        }

    }
}
