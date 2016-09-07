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
    [RoutePrefix("api")]
    public class CartApiController : ApiController
    {
        [HttpPost]
        [Route("cart")]
        public HttpResponseMessage GetCartViewModel(CartViewModel cart)
        {
            // cart comes in with only the models and quantities, need to set the rest
            var total = 0.0m;
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

                total += item.Total;
            }

            return Request.CreateResponse(HttpStatusCode.OK, cart);
        }

        private string BuildProductTitle(string model, Product product, List<ProductAdjustment> adjustments)
        {
            var match = product.ModelTemplate.Match(model);
            var title = new StringBuilder(product.Title);
            var adjustmentModel = match.Groups[ProductAdjustmentTypes.Finish].Value;

            title = title.Replace("{Finish}", GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Finish).Name);
            title = title.Replace("{Size}", GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Size).Name);
            title = title.Replace("{Snap}", GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Snap).Name);
            if (match.Groups["Package"].Success)
                title = title.Append($" ({GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Package).Name})");

            return title.ToString();
        }

        private decimal GetProductPrice(string model, Product product, List<ProductAdjustment> adjustments)
        {
            // get the product model
            var match = product.ModelTemplate.Match(model);

            // set the base price
            var price = product.Price;

            // adjust for finish
            if (adjustments.Any(x => x.Name == ProductAdjustmentTypes.Finish))
                price += GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Finish).PriceAdjustment;
            // adjust for size
            if (adjustments.Any(x => x.Name == ProductAdjustmentTypes.Size))
                price += GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Size).PriceAdjustment;
            // adjust for snap
            if (adjustments.Any(x => x.Name == ProductAdjustmentTypes.Snap))
                price += GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Snap).PriceAdjustment;
            // adjust for package
            if (adjustments.Any(x => x.Name == ProductAdjustmentTypes.Package))
                price += GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Package).PriceAdjustment;

            return price;
        }

        private static ProductAdjustment GetProductAdjustment(List<ProductAdjustment> adjustments, Match match, string adjustmentType)
        {
            // extract the adjustment model from the model string via regex
            var adjustmentModel = match.Groups[adjustmentType.ToLower()].Value;
            // get the price adjustment by the adjustment model and type
            var adjustment = adjustments.First(x => x.Model == adjustmentModel && x.Name == adjustmentType);
            return adjustment;
        }
    }
}
