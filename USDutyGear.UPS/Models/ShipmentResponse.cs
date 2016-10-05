namespace USDutyGear.UPS.Models
{
    public class ShipmentResponse
    {
        public ShipmentResponse()
        {
            Response = new ResponseInfo();
            ShipmentResults = new ShipmentResults();
        }

        public ResponseInfo Response { get; set; }
        public ShipmentResults ShipmentResults { get; set; }
    }
}
