using System.Collections.Generic;

namespace USDutyGear.UPS.Models
{
    public class Address
    {
        public Address()
        {
            AddressLine = new List<string>();
        }

        public List<string> AddressLine { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }
}
