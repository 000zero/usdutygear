using System;
using System.Collections.Generic;

namespace USDutyGear.Models
{
    public class CheckoutViewModel : USDutyGearBaseViewModel
    {
        private const int _maxSequence = 999999999;

        public CheckoutViewModel()
        {
            TestMode = true;
            Sequence = new Random().Next(0, _maxSequence);
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            TimeStamp = secondsSinceEpoch;
        }

        public List<CartItem> Items { get; set; }
        public decimal Shipping { get; set; }
        public string ShippingServiceCode { get; set; }
        public string ShippingDescription { get; set; }
        public decimal Tax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PayeezyPostUrl { get; set; }
        public string PayeezyPageId { get; set; }
        public bool TestMode { get; set; }
        public string OrderConfirmedEmail { get; set; }
        public int OrderId { get; set; }
        public int Sequence { get; set; }
        public string Hash { get; set; }
        public long TimeStamp { get; set; }

        public string FirstName
        {
            get
            {
                var tokens = Name.Split(' ');
                if (tokens.Length < 1)
                    return string.Empty;
                return tokens[0];
            }
        }

        public string LastName
        {
            get
            {
                var tokens = Name.Split(' ');
                if (tokens.Length < 2)
                    return string.Empty;
                return Name.Substring(Name.IndexOf(' ')).Trim();
            }
        }
    }
}