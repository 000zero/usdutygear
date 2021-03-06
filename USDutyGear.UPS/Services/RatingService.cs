﻿using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using USDutyGear.UPS.Common;
using USDutyGear.UPS.Models;


namespace USDutyGear.UPS.Services
{
    public static class RatingServices
    {
        static RatingServices()
        {
            // NOTE: global application effects???
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        
        public static RateResponse GetRatings(Guid requestId, ShippingInfo from, ShippingInfo to, Dimensions dimensions = null, PackageWeight weight = null)
        {
            RateResponse response;

            // set the UPS license number
            from.ShipperNumber = UpsConfig.License;

            // create the UPS rating request
            var request = new RatesRequest
            {
                UPSSecurity = UpsConfig.Credentials,
                RateRequest = new RateRequestInfo
                {
                    Request = new RequestInfo
                    {
                        RequestOption = "Shop",
                        TransactionReference = new TransactionRef
                        {
                            CustomerContext = requestId.ToString()
                        }
                    },
                    Shipment = new RateShipment
                    {
                        Shipper = from,
                        ShipTo = to,
                        Package = new Package
                        {
                            PackagingType = new CodeSet
                            {
                                Code = "02",
                                Description = "Rate"
                            },
                            Dimensions = dimensions ?? new Dimensions
                            {
                                UnitOfMeasurement = new CodeSet
                                {
                                    Code = "IN",
                                    Description = "inches"
                                },
                                Length = "5",
                                Width = "4",
                                Height = "3"
                            },
                            PackageWeight = weight ?? new PackageWeight
                            {
                                UnitOfMeasurement = new CodeSet
                                {
                                    Code = "Lbs",
                                    Description = "pounds"
                                },
                                Weight = "1"
                            }
                        },
                        ShipmentRatingOptions = new ShipmentRatingOptions
                        {
                            NegotiatedRatesIndicator = "" // TODO: figure out if we need this
                        }
                    },
                }
            };

            var serializer = new JavaScriptSerializer();

            // create the web request for the rating API
            var httpRequest = (HttpWebRequest)WebRequest.Create(UpsConfig.RatingUrl);
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
            using(var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                response = serializer.Deserialize<RateResponseWrapper>(result).RateResponse;
            }

            return response;
        }
    }
}
