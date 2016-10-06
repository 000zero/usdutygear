using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using USDutyGear.UPS.Common;
using USDutyGear.UPS.Models;

namespace USDutyGear.UPS.Services
{
    public static class ShipmentService
    {
        static ShipmentService()
        {
            // NOTE: global application effects???
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public static ShipmentResponse RequestShipment(string requestId, string serviceCode, ShippingInfo from, ShippingInfo to, Dimensions dimensions = null, PackageWeight weight = null)
        {
            ShipmentResponse response;

            // set the UPS license number
            from.ShipperNumber = UpsConfig.Account;
            from.Phone = new Phone
            {
                Number = "9093918800"
            };
            to.Phone = new Phone
            {
                Number = "9093918800"
            };

            // create the UPS rating request
            var request = new ShippingRequest
            {
                UPSSecurity = UpsConfig.Credentials,
                ShipmentRequest = new ShipmentRequestInfo
                {
                    Request = new RequestInfo
                    {
                        RequestOption = "validate",
                        TransactionReference = new TransactionRef
                        {
                            CustomerContext = requestId
                        }
                    },
                    Shipment = new Shipment
                    {
                        Shipper = from,
                        ShipTo = to,
                        Service = new CodeSet
                        {
                            Code = serviceCode
                        },
                        PaymentInformation = new PaymentInfo
                        {
                            ShipmentCharge = new ShipmentCharge
                            {
                                Type = "01",
                                BillShipper = new BillShipperInfo
                                {
                                    AccountNumber = UpsConfig.Account
                                }
                            }
                        },
                        Package = new Package
                        {
                            Packaging = new CodeSet
                            {
                                Code = "02",
                                Description = "Customer Supplied"
                            },
                            Dimensions = dimensions ?? new Dimensions
                            {
                                UnitOfMeasurement = new CodeSet
                                {
                                    Code = "IN",
                                    Description = "Inches"
                                },
                                Length = "5",
                                Width = "4",
                                Height = "3"
                            },
                            PackageWeight = weight ?? new PackageWeight
                            {
                                UnitOfMeasurement = new CodeSet
                                {
                                    Code = "LBS",
                                    Description = "Pounds"
                                },
                                Weight = "1"
                            }
                        },
                        ShipmentRatingOptions = new ShipmentRatingOptions
                        {
                            NegotiatedRatesIndicator = "0"
                        }
                    },
                    LabelSpecification = new LabelSpecs
                    {
                        LabelImageFormat = new CodeSet
                        {
                            Code = "GIF",
                            Description = "GIF"
                        }
                    }
                }
            };

            var serializer = new JavaScriptSerializer();

            // create the web request for the rating API
            var httpRequest = (HttpWebRequest)WebRequest.Create(UpsConfig.ShipmentUrl);
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "POST";
            using (var stream = new StreamWriter(httpRequest.GetRequestStream()))
            {
                var json = serializer.Serialize(request);

                stream.Write(json);
                stream.Flush();
                stream.Close();
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                response = serializer.Deserialize<ShipmentResponse>(result);
            }

            return response;
        }
    }
}
