using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using USDutyGear.Controllers;
using USDutyGear.Models;

namespace USDutyGear.Tests.Controllers
{
    [TestClass]
    public class CheckoutControllerTests
    {
        [TestMethod]
        public void Complete()
        {
            var controller = new CheckoutController();

            var result = new PayeezyPaymentResultsModel
            {
                Transaction_Approved = "YES",
                Reference_No = "4",
                exact_ctr = "TEST RECEIPT"
            };

            controller.Complete(result);
        }
    }
}
