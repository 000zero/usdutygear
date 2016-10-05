namespace USDutyGear.UPS.Models
{
    public class ShipmentCharge
    {
        public ShipmentCharge()
        {
            BillShipper = new BillShipperInfo();
        }

        public string Type { get; set; }
        public BillShipperInfo BillShipper { get; set; }
    }
}
