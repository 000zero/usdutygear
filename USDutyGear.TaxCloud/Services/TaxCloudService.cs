using System.IO;
using System.Net;
using System.Configuration;
using System.Web.Script.Serialization;
using USDutyGear.TaxCloud.Models;

namespace USDutyGear.TaxCloud.Services
{
    public static class TaxCloudService
    {
        private const string ApiIdKey = "TaxCloudApiId";
        private const string ApiKeyKey = "TaxCloudApiKey";
        private const string ApiUrlKey = "TaxCloudApiUrl";

        private static string ApiId { get; }
        private static string ApiKey { get; }
        private static string ApiUrl { get; }


        static TaxCloudService()
        {
            ApiId = ConfigurationManager.AppSettings[ApiIdKey];
            ApiKey = ConfigurationManager.AppSettings[ApiKeyKey];
            ApiUrl = ConfigurationManager.AppSettings[ApiUrlKey];
        }

        public static VerifyAddressResponse VerifyAddress(VerifyAddressRequest request)
        {
            // set the login and key
            request.apiLoginID = ApiId;
            request.apiKey = ApiKey;

            var serializer = new JavaScriptSerializer();

            // create the web request for the rating API
            var httpRequest = (HttpWebRequest)WebRequest.Create($"{ApiUrl}VerifyAddress");
            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "POST";
            using (var stream = new StreamWriter(httpRequest.GetRequestStream()))
            {
                var json = serializer.Serialize(request);

                stream.Write(json);
                stream.Flush();
                stream.Close();
            }

            VerifyAddressResponse response;
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                response = serializer.Deserialize<VerifyAddressResponse>(result);
            }

            return response;
        }

        public static void GetTaxAmount()
        {
            
        }

        public static void CaptureSale()
        {
            
        }
    }
}
