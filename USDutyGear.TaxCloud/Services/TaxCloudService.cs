using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Configuration;
using System.Web.Script.Serialization;
using NLog;
using USDutyGear.TaxCloud.Models;

namespace USDutyGear.TaxCloud.Services
{
    public static class TaxCloudService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

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

        public static bool VerifyAddress(Address address, out Address verifiedAddress)
        {
            var request = new VerifyAddressRequest
            {
                Address1 = address.Address1,
                Address2 = address.Address2,
                City = address.City,
                State = address.State,
                Zip5 = address.Zip5,
                Zip4 = address.Zip4
            };

            var response = VerifyAddress(request);

            if (response.ErrNumber == "0")
            {
                verifiedAddress = new Address
                {
                    Address1 = response.Address1,
                    Address2 = response.Address2,
                    City = response.City,
                    State = response.State,
                    Zip5 = response.Zip5,
                    Zip4 = response.Zip4
                };
                return true;
            }

            verifiedAddress = null;
            return false;
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

        public static SalesTaxResponse GetTaxAmount(Address origin, Address destination, List<CartItem> items, Guid? cartId = null)
        {
            var request = new SalesTaxRequest
            {
                origin = origin,
                destination = destination,
                cartItems = items
            };

            return GetTaxAmount(request, cartId);
        }

        public static SalesTaxResponse GetTaxAmount(SalesTaxRequest request, Guid? cartId = null)
        {
            // set the login and key
            request.apiLoginID = ApiId;
            request.apiKey = ApiKey;
            request.customerID = Guid.NewGuid().ToString();
            request.deliverdBySeller = false;
            request.cartID = (cartId ?? Guid.NewGuid()).ToString();

            var serializer = new JavaScriptSerializer();

            // create the web request for the rating API
            var httpRequest = (HttpWebRequest)WebRequest.Create($"{ApiUrl}Lookup");
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

            SalesTaxResponse response;
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                response = serializer.Deserialize<SalesTaxResponse>(result);
            }

            return response;
        }

        public static CaptureResponse CaptureSale(int orderId, string cartId)
        {
            return CaptureSale(new CaptureRequest
            {
                customerID = orderId,
                orderID = orderId,
                cartID = cartId,
            });
        }

        public static CaptureResponse CaptureSale(CaptureRequest request)
        {
            // set the login and key
            request.apiLoginID = ApiId;
            request.apiKey = ApiKey;

            var serializer = new JavaScriptSerializer();

            // create the web request for the rating API
            var httpRequest = (HttpWebRequest)WebRequest.Create($"{ApiUrl}Authorized");
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

            CaptureResponse response;
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                try
                {
                    response = serializer.Deserialize<CaptureResponse>(result);
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Failed to capture tax cloud transaction. CartID - {request.cartID}, OrderID - {request.orderID} :: {result}");
                    response = new CaptureResponse
                    {
                        Success = false,
                        Error = result
                    };
                }
                
            }

            return response;
        }
    }
}
