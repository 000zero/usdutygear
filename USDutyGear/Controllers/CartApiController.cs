using System.Net;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using USDutyGear.Data;
using USDutyGear.Models;
using USDutyGear.Core.Models;
using USDutyGear.Core.Common;

namespace USDutyGear.Controllers
{
    [RoutePrefix("api")]
    public class CartApiController : ApiController
    {
        [HttpPost]
        [Route("cart")]
        public HttpResponseMessage GetCartViewModel(CartViewModel cart)
        {
            // cart comes in with only the models and quantities, need to set the rest
            foreach (var item in cart.Items)
            {
                var tokens = item.Model.Split('-');

                // get the product model
                var product = Products.GetProductByModel(tokens[0]);
                var adjustments = Products.GetProductAdjustmentsByModel(product.Model);

                // calculate the price of this particular object
                item.Price = CalculateProductPrice(item.Model);
                // TODO: now set the title
                // find finish adjustment

                // get the data from the db

                // set the data for the item
            }

            // go through the cart

            // get the product object from the model number

            return Request.CreateResponse(HttpStatusCode.OK, cart);
        }

        private decimal CalculateProductPrice(string model)
        {
            var productModel = model.Substring(0, model.IndexOf('-'));

            // get the product model
            var product = Products.GetProductByModel(productModel);
            var adjustments = Products.GetProductAdjustmentsByModel(product.Model);
            var match = product.ModelTemplate.Match(model);

            // set the base price
            var price = product.Price;

            // adjust for finish
            if (adjustments.Any(x => x.Name == ProductAdjustmentTypes.Finish))
                price += GetPriceAdjustment(adjustments, match, ProductAdjustmentTypes.Finish);
            // adjust for size
            if (adjustments.Any(x => x.Name == ProductAdjustmentTypes.Size))
                price += GetPriceAdjustment(adjustments, match, ProductAdjustmentTypes.Size);
            // adjust for snap
            if (adjustments.Any(x => x.Name == ProductAdjustmentTypes.Snap))
                price += GetPriceAdjustment(adjustments, match, ProductAdjustmentTypes.Snap);
            // adjust for package
            if (adjustments.Any(x => x.Name == ProductAdjustmentTypes.Package))
                price += GetPriceAdjustment(adjustments, match, ProductAdjustmentTypes.Package);

            return price;
        }

        private static decimal GetPriceAdjustment(List<ProductAdjustment> adjustments, Match match, string adjustmentType)
        {
            // extract the adjustment model from the model string via regex
            var adjustmentModel = match.Groups[adjustmentType.ToLower()].Value;
            // get the price adjustment by the adjustment model and type
            var adjustment = adjustments.First(x => x.Model == adjustmentModel && x.Name == adjustmentType);
            return adjustment.PriceAdjustment;
        }
    }
}
