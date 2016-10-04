using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using USDutyGear.Common;
using USDutyGear.TaxCloud.Models;
using USDutyGear.TaxCloud.Services;
using Address = USDutyGear.TaxCloud.Models.Address;

namespace USDutyGear.Controllers
{
    [RoutePrefix("api/taxes")]
    public class TaxApiController : ApiController
    {
        [HttpPost]
        [Route("address/verify")]
        public HttpResponseMessage VerifyAddress(VerifyAddressRequest request)
        {
            var response = TaxCloudService.VerifyAddress(request);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("sales")]
        public HttpResponseMessage GetSalesTax(SalesTaxRequest request)
        {
            request.origin = USDutyGearConfig.TaxCloudOrigin;
            var response = TaxCloudService.GetTaxAmount(request);
            var resp = new
            {
                Success = true,
                TaxAmount = response.CartItemsResponse.Sum(x => x.TaxAmount)
            };

            return Request.CreateResponse(HttpStatusCode.OK, resp);
        }
    }
}
