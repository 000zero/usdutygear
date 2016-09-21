using System.Net;
using System.Text;
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
    [RoutePrefix("api/cart")]
    public class CartApiController : ApiController
    {
        [HttpPost]
        [Route("")]
        public HttpResponseMessage GetCartViewModel(CartViewModel cart)
        {
            // cart comes in with only the models and quantities, need to set the rest
            cart.SubTotal = 0.0m;
            foreach (var item in cart.Items)
            {
                var tokens = item.Model.Split('-');

                // get the product model
                var product = Products.GetProductByModel(tokens[0]);
                var adjustments = Products.GetProductAdjustmentsByModel(product.Model);

                // calculate the price of this particular object
                item.Price = GetProductPrice(item.Model, product, adjustments);
                item.Title = BuildProductTitle(item.Model, product, adjustments);
                item.Total = item.Price * item.Quantity;

                cart.SubTotal += item.Total;
            }

            // apply shipping and tax
            cart.Shipping = 7.99m;

            // TODO: apply tax

            cart.GrandTotal = cart.SubTotal + cart.Shipping;

            return Request.CreateResponse(HttpStatusCode.OK, cart);
        }

        private static string BuildProductTitle(string model, Product product, List<ProductAdjustment> adjustments)
        {
            var match = product.ModelRegex.Match(model);
            var title = new StringBuilder(product.Title);

            title = title.Replace("{Finish}", GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Finish)?.Name ?? string.Empty);
            title = title.Replace("{Size}", GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Size)?.Name ?? string.Empty);
            title = title.Replace("{Snap}", GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Snap)?.Name ?? string.Empty);
            if (match.Groups["Package"].Success)
                title = title.Append($" ({GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Package).Name})");

            return title.ToString();
        }

        private static decimal GetProductPrice(string model, Product product, List<ProductAdjustment> adjustments)
        {
            // get the product model
            var match = product.ModelRegex.Match(model);

            // set the base price
            var price = product.Price;

            // adjust for finish
            if (adjustments.Any(x => x.Type == ProductAdjustmentTypes.Finish))
                price += GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Finish).PriceAdjustment;
            // adjust for size
            if (adjustments.Any(x => x.Type == ProductAdjustmentTypes.Size))
                price += GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Size).PriceAdjustment;
            // adjust for snap
            if (adjustments.Any(x => x.Type == ProductAdjustmentTypes.Snap))
                price += GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Snap).PriceAdjustment;
            // adjust for package
            if (adjustments.Any(x => x.Type == ProductAdjustmentTypes.Package))
                price += GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Package).PriceAdjustment;

            return price;
        }

        private static ProductAdjustment GetProductAdjustment(List<ProductAdjustment> adjustments, Match match, string adjustmentType)
        {
            // extract the adjustment model from the model string via regex
            var adjustmentModel = match.Groups[adjustmentType].Value;
            // get the price adjustment by the adjustment model and type
            var adjustment = adjustments.FirstOrDefault(x => x.Model == adjustmentModel && x.Type == adjustmentType);
            return adjustment;
        }
    }
}
