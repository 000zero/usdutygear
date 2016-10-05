using System.Configuration;
using USDutyGear.UPS.Models;

namespace USDutyGear.UPS.Common
{
    public static class UpsConfig
    {
        private const string UserKey = "UpsUser";
        private const string PwKey = "UpsPw";
        private const string RatingUrlKey = "UpsRatingUrl";
        private const string ShipmentUrlKey = "UpsShipmentUrl";
        private const string LicenseKey = "UpsLicense";
        private const string AccountKey = "UpsAccount";

        private static string User { get; }
        private static string Pw { get; }
        public static string RatingUrl { get; }
        public static string ShipmentUrl { get; }
        public static string License { get; }
        public static string Account { get; set; }

        public static Security Credentials { get; }

        static UpsConfig()
        {
            User = ConfigurationManager.AppSettings[UserKey];
            Pw = ConfigurationManager.AppSettings[PwKey];
            RatingUrl = ConfigurationManager.AppSettings[RatingUrlKey];
            ShipmentUrl = ConfigurationManager.AppSettings[ShipmentUrlKey];
            License = ConfigurationManager.AppSettings[LicenseKey];
            Account = ConfigurationManager.AppSettings[AccountKey];

            Credentials = new Security
            {
                UsernameToken = new UserToken
                {
                    Username = User,
                    Password = Pw
                },
                ServiceAccessToken = new ServiceAccessToken
                {
                    AccessLicenseNumber = License
                }
            };
        }
    }

}
