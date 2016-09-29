using System.Linq;
using System.Collections.Generic;
using USDutyGear.Core.Models;

namespace USDutyGear.Models
{
    public class ProductsViewModel : USDutyGearBaseViewModel
    {
        public List<ProductFeatureViewModel> LeftColumn { get; set; }
        public List<ProductFeatureViewModel> MiddleColumn { get; set; }
        public List<ProductFeatureViewModel> RightColumn { get; set; }

        protected ProductsViewModel(IEnumerable<Product> products)
        {
            LeftColumn = new List<ProductFeatureViewModel>();
            MiddleColumn = new List<ProductFeatureViewModel>();
            RightColumn = new List<ProductFeatureViewModel>();

            var i = 0;
            foreach (var p in products.OrderBy(x => x.DisplayOrder))
            {
                switch (i % 3)
                {
                    case 0: // left column
                        LeftColumn.Add(new ProductFeatureViewModel
                        {
                            Name = p.Name,
                            Model = p.Model,
                            Description = p.Description,
                            StartingPrice = $"{p.Price.ToString("C")}+",
                            Images = p.FeatureImages
                        });
                        break;
                    case 1: // middle column
                        MiddleColumn.Add(new ProductFeatureViewModel
                        {
                            Name = p.Name,
                            Model = p.Model,
                            Description = p.Description,
                            StartingPrice = $"{p.Price.ToString("C")}+",
                            Images = p.FeatureImages
                        });
                        break;
                    case 2: // right column
                        RightColumn.Add(new ProductFeatureViewModel
                        {
                            Name = p.Name,
                            Model = p.Model,
                            Description = p.Description,
                            StartingPrice = $"{p.Price.ToString("C")}+",
                            Images = p.FeatureImages
                        });
                        break;
                }

                i++;
            }
        }

        public static ProductsViewModel Create(string category = null)
        {
            return new ProductsViewModel(Data.Products.GetProducts(category));
        }
    }
}