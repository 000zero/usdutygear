using System;
using System.Collections.Generic;
using USDutyGear.Core.Common;

namespace USDutyGear.Core.Models
{
    public class Order
    {
        public Order()
        {
            Created = DateTime.UtcNow;
            Status = OrderStatuses.Pending;
            Country = "US";
        }

        public int OrderId { get; set; }
        public Guid CartId { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public string UpsServiceCode { get; set; }
        public string UpsTrackingId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal ItemTotal { get; set; }
        public string Email { get; set; }
        // address info
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
