using  System.Configuration;

namespace USDutyGear.Data
{
    public static class DbConfig
    {
        public static string ConnectionString { get; }

        static DbConfig()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
        }
    }
}
