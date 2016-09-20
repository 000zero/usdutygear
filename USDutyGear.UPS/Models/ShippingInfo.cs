namespace USDutyGear.UPS.Models
{
    public class ShippingInfo
    {
        public ShippingInfo()
        {
            Address = new Address();
            Phone = new Phone();
        }

        public string Name { get; set; }
        public Address Address { get; set; }
        public string AttentionName { get; set; }
        public Phone Phone { get; set; }
        public string EMailAddress { get; set; }
        public string ShipperNumber { get; set; }
    }
}
