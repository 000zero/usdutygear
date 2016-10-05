using System;
using System.Collections.Generic;
using USDutyGear.UPS.Models;
using USDutyGear.UPS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace USDutyGear.Tests.UPS
{
    [TestClass]
    public class UPSTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // TEST URL- https://wwwcie.ups.com/rest/FreightRate

            var from = new ShippingInfo
            {
                Name = "US DUTY GEAR",
                Address = new Address
                {
                    AddressLine = new List<string> { "2131 S Hellman Ave", "UNIT D" },
                    City = "Ontario",
                    StateProvinceCode = "CA",
                    PostalCode = "91761",
                    CountryCode = "US"
                }
            };

            var to = new ShippingInfo
            {
                Name = "Steven Garcia",
                Address = new Address
                {
                    AddressLine = new List<string> { "6 Andalusia" },
                    City = "Rancho Santa Margarita",
                    StateProvinceCode = "CA",
                    PostalCode = "92688",
                    CountryCode = "US"
                }
            };

            var guid = Guid.NewGuid();
            var result = RatingServices.GetRatings(guid, from, to);

            // test request
            Assert.IsNotNull(result, "The result should not be null");
        }
    }
}
