using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using USDutyGear.Data;
using USDutyGear.Common;
using USDutyGear.Models;
using USDutyGear.UPS.Models;
using USDutyGear.Core.Models;
using USDutyGear.UPS.Services;
using USDutyGear.TaxCloud.Services;
using Address = USDutyGear.TaxCloud.Models.Address;

namespace USDutyGear.Controllers
{
    [RoutePrefix("checkout")]
    public class CheckoutController : Controller
    {
        [HttpPost]
        [Route("")]
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
            var lookupId = Guid.NewGuid();
            var taxResponse = TaxCloudService.GetTaxAmount(
                lookupId,
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
            cart.CartId = lookupId.ToString();
            
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
            var result = RatingServices.GetRatings(guid, USDutyGearConfig.UpsOrigin, to);
            var rate = result.RatedShipment.FirstOrDefault(x => x.Service.Code == cart.ShippingServiceCode);
            if (rate != null)
            {
                cart.Shipping = rate.TotalCharges.MonetaryValue;
            }
            
            // set the grand total
            cart.GrandTotal = cart.SubTotal + cart.Tax + cart.Shipping;

            var order = new Order
            {
                CartId = taxResponse.CartID,
                UpsServiceCode = cart.ShippingServiceCode,
                Tax = cart.Tax,
                Shipping = cart.Shipping,
                ItemTotal = cart.SubTotal,
                Email = cart.Email,
                Name = cart.Name,
                Street = cart.Street,
                City = cart.City,
                State = cart.State,
                PostalCode = cart.Zip,
                Items = cart.Items.Select(x => new OrderItem
                {
                    Model = x.Model,
                    Quantity = x.Quantity,
                    Name = x.Title,
                    Price = x.Price
                }).ToList()
            };
            cart.OrderId = Orders.SaveOrder(order);

            cart.PayeezyPostUrl = USDutyGearConfig.PayeezyPostUrl;
            cart.PayeezyPageId = USDutyGearConfig.PayeezyPageId;
            cart.OrderConfirmedEmail = USDutyGearConfig.OrdersEmailAddress;
            cart.Hash = CreateHash(cart.PayeezyPageId, cart.Sequence, cart.TimeStamp, cart.GrandTotal, USDutyGearConfig.PayeezyTransactionKey);

            return View(cart);
        }

        [HttpGet]
        [Route("complete/test/{orderId}/{success?}")]
        public ActionResult TestComplete(int orderId, bool success = true)
        {
            var vm = new CheckoutCompleteViewModel
            {
                Success = success,
                Order = Orders.GetOrder(orderId),
                Receipt = @"========== TRANSACTION RECORD ==========
US Duty Gear Test DEMO0347
2131 S Hellman Ave
Ontario, CA 91761
United States


TYPE: Purchase

ACCT: Visa $ 17.72 USD

CARDHOLDER NAME : Steven O Garcia
CARD NUMBER : ############1111
DATE/TIME : 06 Oct 16 13:18:12
REFERENCE # : 03 000013 M
AUTHOR. # : ET179708
TRANS. REF. : 17

Approved - Thank You 100


Please retain this copy for your records.

Cardholder will pay above amount to
card issuer pursuant to cardholder
agreement.
========================================"
            };

            vm.Receipt = vm.Receipt.Replace("\r\n", "<br />");

            return View("Complete", vm);
        }

        [HttpPost]
        [Route("complete")]
        public ActionResult Complete(PayeezyPaymentResultsModel paymentResults)
        {
            var vm = CreateCheckoutCompleteVm(paymentResults);

            return View(vm);
        }

        private CheckoutCompleteViewModel CreateCheckoutCompleteVm(PayeezyPaymentResultsModel paymentResults)
        {
            var vm = new CheckoutCompleteViewModel();

            if (paymentResults.Transaction_Approved != "YES")
            {
                // transaction FAILED update order to Rejected
                // show error on page
                vm.Success = false;

                return vm;
            }

            vm.Success = true;
            vm.Receipt = paymentResults.exact_ctr.Replace("\r\n", "<br />");
            vm.Order = Orders.GetOrder(Convert.ToInt32(paymentResults.Reference_No));

            var taxResponse = TaxCloudService.CaptureSale(vm.Order.OrderId, vm.Order.CartId.ToUpper());
            vm.taxResponseJSON = taxResponse.Error;

            var destination = new ShippingInfo
            {
                Name = vm.Order.Name,
                EMailAddress = vm.Order.Email,
                Address = new UPS.Models.Address
                {
                    AddressLine = new List<string> { vm.Order.Street },
                    City = vm.Order.City,
                    StateProvinceCode = vm.Order.State,
                    CountryCode = vm.Order.Country,
                    PostalCode = vm.Order.PostalCode
                }
            };

            var upsResponse = ShipmentService.RequestShipment(vm.Order.CartId, vm.Order.UpsServiceCode, USDutyGearConfig.UpsOrigin, destination);

            if (upsResponse.Response.ResponseStatus.Code == "1")
            {
                vm.Order.UpsTrackingId = upsResponse.ShipmentResults.ShipmentIdentificationNumber;
            }

            // save the order update here
            Orders.CompleteOrder(vm.Order.OrderId, vm.Order.UpsTrackingId, paymentResults.x_trans_id);

            // clear the cart upon success (this needs to be done client side)

            return vm;
        }

        private static string CreateHash(string pageId, int sequence, long timestamp, decimal amount, string transactionKey)
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
    }
}
