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
            cart.CartId = taxResponse.CartID;
            
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
                CartId = Guid.Parse(taxResponse.CartID),
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
                    Quantity = x.Quantity
                }).ToList()
            };
            cart.OrderId = Orders.SaveOrder(order);

            cart.PayeezyPostUrl = USDutyGearConfig.PayeezyPostUrl;
            cart.PayeezyPageId = USDutyGearConfig.PayeezyPageId;
            cart.OrderConfirmedEmail = USDutyGearConfig.OrdersEmailAddress;
            cart.Hash = CreateHash(cart.PayeezyPageId, cart.Sequence, cart.TimeStamp, cart.GrandTotal, USDutyGearConfig.PayeezyTransactionKey);

            return View(cart);
        }

        [HttpPost]
        [Route("complete")]
        public ActionResult Complete(PayeezyPaymentResultsModel paymentResults)
        {
            var vm = new CheckoutCompleteViewModel();

            if (paymentResults.Transaction_Approved != "YES")
            {
                // transaction FAILED update order to Rejected
                // show error on page
                vm.Success = false;

                return View(vm);
            }

            vm.Success = true;
            vm.OrderId = Convert.ToInt32(paymentResults.Reference_No);
            vm.Receipt = paymentResults.exact_ctr.Replace("\r\n", "<br />");
            
            // load the order
            var order = Orders.GetOrder(vm.OrderId);

            var taxDestination = new Address
            {
                Address1 = order.Street,
                City = order.City,
                State = order.State,
                Zip5 = order.PostalCode
            };

            // verify address for tax cloud; if a better address is not found then just use the destination supplied by the customer
            Address verifiedAddress;
            TaxCloudService.VerifyAddress(taxDestination, out verifiedAddress);

            //var cartIndex = 0;
            //var lookupResponse = TaxCloudService.GetTaxAmount(
            //    USDutyGearConfig.TaxCloudOrigin,
            //    verifiedAddress ?? destination,
            //    order.Items.Select(x => new TaxCloud.Models.CartItem
            //    {
            //        Index = cartIndex++,
            //        ItemID = x.Model,
            //        Price = x.Price,
            //        Qty = x.Quantity
            //    }).ToList());

            //cart.Tax = taxResponse.CartItemsResponse.Sum(x => x.TaxAmount);

            var taxResponse = TaxCloudService.CaptureSale(order.OrderId, order.CartId.ToString());
            vm.taxResponseJSON = taxResponse.Error;

            var destination = new ShippingInfo
            {
                Name = order.Name,
                EMailAddress = order.Email,
                Address = new UPS.Models.Address
                {
                    AddressLine = new List<string> { order.Street },
                    City = order.City,
                    StateProvinceCode = order.State,
                    CountryCode = order.Country,
                    PostalCode = order.PostalCode
                }
            };

            var upsResponse = ShipmentService.RequestShipment(order.CartId, order.UpsServiceCode, USDutyGearConfig.UpsOrigin, destination);

            if (upsResponse.Response.ResponseStatus.Code == "1")
            {
                order.UpsTrackingId = upsResponse.ShipmentResults.ShipmentIdentificationNumber;
            }

            // save the order update here
            Orders.CompleteOrder(order.OrderId, order.UpsTrackingId, paymentResults.x_trans_id);

            // clear the cart upon success (this needs to be done client side)

            return View(vm);
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
