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

        private static void TestingSetup()
        {
            var productId = 1;
            Products.Add(new Product
            {
                Id = productId++,
                Name = "Karma Liner Belt",
                Category = Categories.LinerBelt,
                Model = "12-1",
                Finish = Finishes.Karma,
                //Description = "1.5\" Karma liner belt.",
                Price = 19.99m,
                Sku = "121000"
            });

            Products.Add(new Product
            {
                Id = productId++,
                Name = "Basket Weave Liner Belt",
                Category = Categories.LinerBelt,
                Model = "12-2",
                Finish = Finishes.BasketWeave,
                //Description = "1.5\" Basketweave liner belt.",
                Price = 19.99m,
                Sku = "122000"
            });

            Products.Add(new Product
            {
                Id = productId++,
                Name = "High Gloss Liner Belt",
                Category = Categories.LinerBelt,
                Model = "12-3",
                Finish = Finishes.HighGloss,
                //Description = "1.5\" High gloss liner belt.",
                Price = 19.99m,
                Sku = "123000"
            });

            Products.Add(new Product
            {
                Id = productId++,
                Name = "Karma Liner Belt",
                Category = Categories.LinerBelt,
                Model = "12-1",
                Finish = Finishes.Karma,
                //Description = "1.5\" Karma liner belt.",
                Price = 19.99m,
                Sku = "121000"
            });

            Products.Add(new Product
            {
                Id = productId++,
                Name = "Leather Sam Browne Belt",
                Category = Categories.SamBrowneBelt,
                Model = "200-1",
                Finish = Finishes.Leather,
                //Description = "2.25\" Leather Sam Browne belt.",
                Price = 19.99m,
                Sku = "200100"
            });

            Products.Add(new Product
            {
                Id = productId++,
                Name = "Nylon Sam Browne Belt",
                Category = Categories.SamBrowneBelt,
                Model = "200-2",
                Finish = Finishes.Nylon,
                //Description = "2.25\" Nylon Sam Browne belt.",
                Price = 19.99m,
                Sku = "200200"
            });

            Products.Add(new Product
            {
                Id = productId++,
                Name = "Hidden Snap Leather Belt Keeper",
                Category = Categories.Keeper,
                Model = "72-1",
                Finish = Finishes.Leather,
                //Description = "1\" Hidden snap leather belt keeper.",
                Price = 19.99m,
                Sku = "121000"
            });
        }
    }
}
