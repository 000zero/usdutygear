﻿namespace USDutyGear.TaxCloud.Models
{
    public class VerifyAddressRequest : TaxCloudRequest
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip5 { get; set; }
        public string Zip4 { get; set; }
    }
}
