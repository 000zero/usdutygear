using System.Collections.Generic;
using System.Linq;
using USDutyGear.Data;

namespace USDutyGear.Models
{
    public class USDutyGearBaseViewModel
    {
        public USDutyGearBaseViewModel()
        {
            Categories = Products
                .GetProductCategories()
                .Select(x => new CategoryViewModel(x))
                .ToList();
        }

        public List<CategoryViewModel> Categories { get; set; }
    }
}