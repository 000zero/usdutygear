using System.Collections.Generic;

namespace USDutyGear.UPS.Models
{
    public class RateResponse
    {
        public RateResponse()
        {
            Response = new ResponseInfo();
            RatedShipment = new List<RatedShipment>();
        }

        public ResponseInfo Response { get; set; }
        public List<RatedShipment> RatedShipment { get; set; }
    }
}
