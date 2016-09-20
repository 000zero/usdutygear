using System.Web.Configuration;

namespace USDutyGear.Common
{
    public static class USDutyGearConfig
    {
        public static string UpsUrl { get; set; }
        public static string UpsUser { get; set; }
        public static string UpsPw { get; set; }
        public static string UpsLicense { get; set; }

        static USDutyGearConfig()
        {
            UpsUrl = WebConfigurationManager.AppSettings["UpsUrl"];
            UpsUser = WebConfigurationManager.AppSettings["UpsUser"];
            UpsPw = WebConfigurationManager.AppSettings["UpsPw"];
            UpsLicense = WebConfigurationManager.AppSettings["UpsLicense"];
        }
    }
}