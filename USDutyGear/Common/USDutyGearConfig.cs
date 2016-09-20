using System.Web.Configuration;
using System.Collections.Generic;

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

        static USDutyGearConfig()
        {
            ShippingName = WebConfigurationManager.AppSettings["UsdgShippingName"];
            AddressLines = new List<string> { WebConfigurationManager.AppSettings["UsdgStreet1"], WebConfigurationManager.AppSettings["UsdgStreet2"] };
            City = WebConfigurationManager.AppSettings["UsdgCity"];
            State = WebConfigurationManager.AppSettings["UsdgState"];
            CountryCode = WebConfigurationManager.AppSettings["UsdgCountry"];
            ZipCode = WebConfigurationManager.AppSettings["UsdgZip"];
        }
    }
}