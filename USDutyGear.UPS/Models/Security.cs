namespace USDutyGear.UPS.Models
{
    public class Security
    {
        public Security()
        {
            UsernameToken = new UserToken();
            ServiceAccessToken = new ServiceAccessToken();
        }

        public UserToken UsernameToken { get; set; }
        public ServiceAccessToken ServiceAccessToken { get; set; }
    }
}
