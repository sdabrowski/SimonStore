using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimonStore.Models;
using Braintree;
using System.Configuration;
using System.Threading.Tasks;

namespace SimonStore.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            if (!Request.Cookies.AllKeys.Contains("cart"))
                return RedirectToAction("Index", "Cart");
            CheckoutModel model = new CheckoutModel();

            return View(model);
        }

        //POST: Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CheckoutModel model)
        {
            //Check if the model-state is valid -- this will catch anytime someone hacks your client-side validation
            if (ModelState.IsValid)
            {
                var gateway = new BraintreeGateway
                {
                    Environment = Braintree.Environment.SANDBOX,
                    MerchantId = ConfigurationManager.AppSettings["Braintree.MerchantID"],
                    PublicKey =  ConfigurationManager.AppSettings["Braintree.PublicKey"],
                    PrivateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"]
                };
                CustomerRequest customer = new CustomerRequest();
                customer.Email = model.ContactEmail;
                customer.Phone = model.ContactPhone;
                var customerResult = await gateway.Customer.CreateAsync(customer);

                //TODO: Save the checkout information somewhere
                return RedirectToAction("Index", "Receipt");
            }
            return View(model);
        }
    }
}