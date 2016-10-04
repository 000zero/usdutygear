using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using USDutyGear.Common;
using USDutyGear.UPS.Models;
using USDutyGear.UPS.Services;

namespace USDutyGear.Controllers
{
    [RoutePrefix("api/shipping")]
    public class ShippingApiController : ApiController
    {
        [HttpPost]
        [Route("rates")]
        public HttpResponseMessage GetShippingRates(ShippingInfo to)
        {
            // make sure the address is populated
            string errorMessage;
            if (!ValidateShippingInfo(to, out errorMessage))
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);

            // TODO: validate address here

            var guid = Guid.NewGuid();
            var result = UpsServices.GetRatings(guid, USDutyGearConfig.UpsOrigin, to);
            result.RatedShipment = result.RatedShipment.OrderBy(x => x.TotalCharges.MonetaryValue).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        private static bool ValidateShippingInfo(ShippingInfo info, out string error)
        {
            if (string.IsNullOrWhiteSpace(info.Name))
            {
                error = "No shipping name provided.";
                return false;
            }

            if (info.Address.AddressLine.Count < 1 || info.Address.AddressLine.All(x => string.IsNullOrWhiteSpace(x)))
            {
                error = "No street address provided.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(info.Address.City))
            {
                error = "No city provided.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(info.Address.StateProvinceCode))
            {
                error = "No state provided.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(info.Address.CountryCode))
            {
                error = "No country provided.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(info.Address.PostalCode))
            {
                error = "No zip code provided.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
