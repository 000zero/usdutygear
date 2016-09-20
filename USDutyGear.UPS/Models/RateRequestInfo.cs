﻿namespace USDutyGear.UPS.Models
{
    public class RateRequestInfo
    {
        public RateRequestInfo()
        {
            Request = new RequestInfo();
            Shipment = new Shipment();
        }

        public RequestInfo Request { get; set; }
        public Shipment Shipment { get; set; }
    }
}