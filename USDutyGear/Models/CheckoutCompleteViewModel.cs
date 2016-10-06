using USDutyGear.Core.Models;

namespace USDutyGear.Models
{
    public class CheckoutCompleteViewModel : USDutyGearBaseViewModel
    {
        public bool Success { get; set; }
        public Order Order { get; set; }
        public string Receipt { get; set; }
        public string responseJSON { get; set; }
        public string UpsResponseJSON { get; set; }
        public string taxResponseJSON { get; set; }
    }
}