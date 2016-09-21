using System.Linq;
using System.Collections.Generic;
using USDutyGear.Common;
using USDutyGear.Core.Common;

namespace USDutyGear.Models
{
    public class CategoryViewModel
    {
        public CategoryViewModel(ProductCategory productCategory)
        {
            Category = productCategory.Category;
            Products = productCategory
                .Products
                .Select(x => new CategoryProductViewModel(x))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public string Category { get; set; }
        public List<CategoryProductViewModel> Products { get; set; }
    }

    public class CategoryProductViewModel
    {
        public string Name { get; set; }
        public string RouteKey { get; set; }

        public CategoryProductViewModel(string name)
        {
            Name = name;
            RouteKey = CategoryHelper.MapProductNameToRouteKey(name);
        }
    }
}