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
            if (User.Identity.IsAuthenticated)
            {
                SimonStoreEntities entities = new SimonStoreEntities();
                var user = entities.Users.FirstOrDefault(X => X.AspNetUser.UserName == User.Identity.Name);
                if (user != null)
                {
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.ContactEmail = user.AspNetUser.Email;
                    model.ContactPhone = user.AspNetUser.PhoneNumber;

                    model.BillingAddress.Street1 = user.BillingStreetAddress1;
                    model.BillingAddress.Street2 = user.BillingStreetAddress2;
                    model.BillingAddress.City = user.BillingCity;
                    model.BillingAddress.State = user.BillingState;
                    model.BillingAddress.PostalCode = user.BillingZip;

                    model.ShippingAddress.Street1 = user.ShippingStreetAddress1;
                    model.ShippingAddress.Street2 = user.ShippingStreetAddress2;
                    model.ShippingAddress.City = user.ShippingCity;
                    model.ShippingAddress.State = user.ShippingState;
                    model.ShippingAddress.PostalCode = user.ShippingZip;
                }
            }

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

        public ActionResult ValidateAddress(string street1, string street2, string city, string state, string postalCode)
        {
            if (!string.IsNullOrEmpty(street1) && !string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(state)
                //&& (Request.UrlReferrer.Host == Request.Url.Host)     //Uncommenting this will make sure your validate function only works on your site
                )
            {
                SmartyStreets.ClientBuilder builder = new SmartyStreets.ClientBuilder(
                    ConfigurationManager.AppSettings["SmartyStreets.AuthId"],
                    ConfigurationManager.AppSettings["SmartyStreets.AuthToken"]);
                var client = builder.BuildUsStreetApiClient();

                SmartyStreets.USStreetApi.Lookup lookup = new SmartyStreets.USStreetApi.Lookup();
                lookup.Street = street1;
                lookup.Street2 = street2;
                lookup.City = city;
                lookup.State = state;
                lookup.ZipCode = postalCode;
                client.Send(lookup);
                return Json(lookup.Result.Select(x => new { Street1 = x.DeliveryLine1, Street2 = x.DeliveryLine2, City = x.Components.CityName, State = x.Components.State, PostalCode = x.Components.ZipCode }), JsonRequestBehavior.AllowGet);
            }
            return Json(new object[0], JsonRequestBehavior.AllowGet);
        }
    }
}