using System;
using System.Collections.Generic;
using System.Linq;
using USDutyGear.Common;

namespace USDutyGear.Models
{
    // TODO: determine if this view model is used at all
    public class HomeViewModel : USDutyGearBaseViewModel
    {
        public List<ProductFeatureViewModel> Products { get; set; }

        protected HomeViewModel(List<KeyValuePair<string, List<string>>> products)
        {
            Products = products
                .Select(x => new ProductFeatureViewModel
                {
                    Name = x.Key,
                    Description = x.Value.Count > 1
                        ? $"Available in {string.Join(", ", x.Value.Take(x.Value.Count - 1))}, and {x.Value.Last()}"
                        : $"Available in {x.Value.First()}",
                    RouteKey = x.Key,
                    ImagePath = ""
                }).ToList();
        }

        public static HomeViewModel Create()
        {
            return new HomeViewModel(Data.Products.GetProductFeatures());
        }
    }
}