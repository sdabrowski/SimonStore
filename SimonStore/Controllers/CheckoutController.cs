using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimonStore.Models;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNet.Identity;

namespace SimonStore.Controllers
{
    public class CheckoutController : Controller
    {
        //private IAddressValidationService avs;
        private SimonStoreEntities  db = new SimonStoreEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private IBraintreeGateway _braintreeGateway;
        private IIdentityMessageService _emailService;
        private IIdentityMessageService _smsService;

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

                CustomerSearchRequest searchRequest = new CustomerSearchRequest();
                searchRequest.Email.Is(model.ContactEmail);

                Customer c = null;
                var existingCustomers = await gateway.Customer.SearchAsync(searchRequest);
                if (existingCustomers.Ids.Any())
                {
                    c = existingCustomers.FirstItem;
                }
                else
                {
                    CustomerRequest newCustomer = new CustomerRequest();
                    newCustomer.FirstName = model.FirstName;
                    newCustomer.LastName = model.LastName;
                    newCustomer.Email = model.ContactEmail;
                    newCustomer.Phone = model.ContactPhone;
                    var customerResult = await gateway.Customer.CreateAsync(newCustomer);
                    if (customerResult.IsSuccess())
                    {
                        c = customerResult.Target;
                    }
                    else
                    {
                        throw new Exception(customerResult.Errors.All().First().Message);
                    }
                }


                string token;

                Braintree.CreditCardRequest card = new Braintree.CreditCardRequest();
                card.Number = model.CreditCardNumber;
                card.CVV = model.CreditCardVerificationValue;
                card.ExpirationMonth = model.CreditCardExpirationMonth.ToString().PadLeft(2, '0');
                card.ExpirationYear = model.CreditCardExpirationYear.ToString();
                card.CardholderName = model.FirstName + " " + model.LastName;
                card.CustomerId = c.Id;
                var cardResult = await gateway.CreditCard.CreateAsync(card);
                if (cardResult.IsSuccess())
                {


                    token = cardResult.Target.Token;
                }
                else
                {
                    throw new Exception(cardResult.Errors.All().First().Message);
                }

                HttpCookie cartCookie = Request.Cookies["cart"];
                var order = db.Orders.Find(int.Parse(cartCookie.Value));

                Braintree.TransactionRequest transaction = new TransactionRequest();
                transaction.PaymentMethodToken = token;
                transaction.Amount = order.OrderedProducts.Sum(x => (x.Product.Price ?? 0) * (x.Quantity ?? 0));
                transaction.CustomerId = c.Id;
                var saleResult = await gateway.Transaction.SaleAsync(transaction);
                if (saleResult.IsSuccess())
                {
                    Response.SetCookie(new HttpCookie("cart") { Expires = DateTime.Now });
                    order.OrderCompletedDate = DateTime.UtcNow;
                    order.CustomerEmail = model.ContactEmail;

                    order.BillingStreetAddress1 = model.BillingAddress.Street1;
                    if(model.BillingAddress.Street2 != null)
                    {
                        order.BillingStreetAddress2 = model.BillingAddress.Street2;
                    }
                    order.BillingCity = model.BillingAddress.City;
                    order.BillingState = model.BillingAddress.State;
                    order.BillingZip = model.BillingAddress.PostalCode;

                    order.ShippingStreetAddress1 = model.ShippingAddress.Street1;
                    if (model.ShippingAddress.Street2 != null)
                    {
                        order.ShippingStreetAddress2 = model.ShippingAddress.Street2;
                    }
                    order.ShippingCity = model.ShippingAddress.City;
                    order.ShippingState = model.ShippingAddress.State;
                    order.ShippingZip = model.ShippingAddress.PostalCode;

                    if (User.Identity.IsAuthenticated)
                    {
                        var user = db.Users.FirstOrDefault(X => X.AspNetUser.UserName == User.Identity.Name);
                        order.AspNetUserID = user.AspNetUserID;
                    }
                    order.LastModifiedOn = DateTime.UtcNow;

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index", "Receipt");
                    //Need to create a Receipt Controller, as well as a View to show receipt, send email confirmation.
                    //Check Joe's Github for inspiration
                }
                
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