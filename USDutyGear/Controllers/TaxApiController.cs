using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using USDutyGear.TaxCloud.Models;
using USDutyGear.TaxCloud.Services;

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
    }
}
