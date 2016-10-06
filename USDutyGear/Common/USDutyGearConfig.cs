using System.Web.Configuration;
using System.Collections.Generic;
using USDutyGear.UPS.Models;

namespace USDutyGear.Common
{
    public static class USDutyGearConfig
    {
        public static string ShippingName { get; set; }
        public static List<string> AddressLines { get; set; }
        public static string City { get; set; }
        public static string State { get; set; }
        public static string CountryCode { get; set; }
        public static string ZipCode { get; set; }
        public static ShippingInfo UpsOrigin { get; set; }
        public static TaxCloud.Models.Address TaxCloudOrigin { get; set; }
        public static string OrdersEmailAddress { get; set; }
        public static bool TestMode { get; }
        // payeezy settings
        public static string PayeezyPostUrl { get; set; }
        public static string PayeezyPageId { get; set; }
        public static string PayeezyTransactionKey { get; set; }

        static USDutyGearConfig()
        {
            ShippingName = WebConfigurationManager.AppSettings["UsdgShippingName"];
            AddressLines = new List<string> { WebConfigurationManager.AppSettings["UsdgStreet1"], WebConfigurationManager.AppSettings["UsdgStreet2"] };
            City = WebConfigurationManager.AppSettings["UsdgCity"];
            State = WebConfigurationManager.AppSettings["UsdgState"];
            CountryCode = WebConfigurationManager.AppSettings["UsdgCountry"];
            ZipCode = WebConfigurationManager.AppSettings["UsdgZip"];
            PayeezyPostUrl = WebConfigurationManager.AppSettings["PayeezyUrl"];
            PayeezyPageId = WebConfigurationManager.AppSettings["PayeezyPageId"];
            PayeezyTransactionKey = WebConfigurationManager.AppSettings["PayeezyTransactionKey"];
            OrdersEmailAddress = WebConfigurationManager.AppSettings["UsdgOrdersEmail"];
            TestMode = WebConfigurationManager.AppSettings["UsdgTestMode"].ToLower() == "true";

            UpsOrigin = new ShippingInfo
            {
                Name = ShippingName,
                Address = new UPS.Models.Address
                {
                    AddressLine = AddressLines,
                    City = City,
                    StateProvinceCode = State,
                    CountryCode = CountryCode,
                    PostalCode = ZipCode
                }
            };

            TaxCloudOrigin = new TaxCloud.Models.Address
            {
                Address1 = string.Join(" ", AddressLines),
                Address2 = string.Empty,
                City = City,
                State = State,
                Zip5 = ZipCode
            };
        }
    }
}