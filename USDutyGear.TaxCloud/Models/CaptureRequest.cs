using System;

namespace USDutyGear.TaxCloud.Models
{
    public class CaptureRequest : TaxCloudRequest
    {
        public CaptureRequest()
        {
            dateAuthorized = DateTime.Today;
            dateCaptured = DateTime.Today;
        }

        public string customerID { get; set; }
        public string cartID { get; set; }
        public int orderID { get; set; }
        public DateTime dateAuthorized { get; set; }
        public DateTime dateCaptured { get; set; }
    }
}
