using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using USDutyGear.Common;
using USDutyGear.Models;
using USDutyGear.TaxCloud.Services;
using USDutyGear.UPS.Models;
using USDutyGear.UPS.Services;
using Address = USDutyGear.TaxCloud.Models.Address;

namespace USDutyGear.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        [HttpPost]
        public ActionResult Checkout(CheckoutViewModel cart)
        {
            // validate address

            // validate email

            cart.Items = CartHelper.FillCartItemInfo(cart.Items);

            cart.SubTotal = cart.Items.Sum(x => x.Price*x.Quantity);

            var destination = new Address
            {
                Address1 = cart.Street,
                City = cart.City,
                State = cart.State,
                Zip5 = cart.Zip
            };

            // verify address for tax cloud; if a better address is not found then just use the destination supplied by the customer
            Address verifiedAddress;
            TaxCloudService.VerifyAddress(destination, out verifiedAddress);

            var cartIndex = 0;
            var taxResponse = TaxCloudService.GetTaxAmount(
                USDutyGearConfig.TaxCloudOrigin, 
                verifiedAddress ?? destination,
                cart.Items.Select(x => new TaxCloud.Models.CartItem
                {
                    Index = cartIndex++,
                    ItemID = x.Model,
                    Price = x.Price,
                    Qty = x.Quantity
                }).ToList());

            cart.Tax = taxResponse.CartItemsResponse.Sum(x => x.TaxAmount);

            // get the shipping price for a specific service
            var guid = Guid.NewGuid();
            var to = new ShippingInfo
            {
                Name = cart.Name,
                EMailAddress = cart.Email,
                Address = new UPS.Models.Address
                {
                    AddressLine = new List<string> { cart.Street },
                    City = cart.City,
                    CountryCode = "US",
                    StateProvinceCode = cart.State,
                    PostalCode = cart.Zip
                }
            };
            var result = UpsServices.GetRatings(guid, USDutyGearConfig.UpsOrigin, to);
            var rate = result.RatedShipment.FirstOrDefault(x => x.Service.Code == cart.ShippingServiceCode);
            if (rate != null)
            {
                cart.Shipping = rate.TotalCharges.MonetaryValue;
                cart.ShippingDescription = rate.Service.Description;
            }
            
            // set the grand total
            cart.GrandTotal = cart.SubTotal + cart.Tax + cart.Shipping;

            // TODO: save the order here

            cart.PayeezyPostUrl = USDutyGearConfig.PayeezyPostUrl;
            cart.PayeezyPageId = USDutyGearConfig.PayeezyPageId;
            cart.PayeezyLogin = USDutyGearConfig.PayeezyLogin;
            cart.OrderConfirmedEmail = USDutyGearConfig.OrdersEmailAddress;
            cart.Hash = CreateHash(cart.PayeezyPageId, cart.Sequence, cart.TimeStamp, cart.GrandTotal, USDutyGearConfig.PayeezyTransactionKey);

            return View(cart);
        }

        private string CreateHash(string pageId, int sequence, long timestamp, decimal amount, string transactionKey)
        {
            var payload = $"{pageId}^{sequence}^{timestamp}^{amount.ToString("F")}^";
            var data = Encoding.UTF8.GetBytes(payload);
            var key = Encoding.UTF8.GetBytes(transactionKey);

            using (var hmac = new HMACMD5(key))
            {
                var hashBytes = hmac.ComputeHash(data);

                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hash;
            }
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            // finalize UPS shipping API call

            // finalize TaxCloud API call

            // send emails

            return View();
        }
    }
}
