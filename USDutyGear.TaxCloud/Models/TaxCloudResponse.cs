using System.Collections.Generic;

namespace USDutyGear.TaxCloud.Models
{
    public class TaxCloudResponse
    {
        public TaxCloudResponse()
        {
            Messages = new List<string>();
        }

        public int ResponseType { get; set; }
        public List<string> Messages { get; set; }
    }
}
