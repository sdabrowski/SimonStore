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
using SendGrid;
using SendGrid.Helpers.Mail;

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

                    string apiKey = System.Configuration.ConfigurationManager.AppSettings["SendGrid.ApiKey"];

                    SendGrid.ISendGridClient client = new SendGridClient(apiKey);

                    EmailAddress from = new EmailAddress("sales@ParacordStore.com", "Paracord Store");

                    EmailAddress to = new EmailAddress(model.ContactEmail);

                    string subject = string.Format("Your Paracord Store Order {0}", order.OrderID);

                    string htmlContent = CreateReceiptEmail(order);
                    string plainTextContent = CreatePlaintextEmail(order);

                    SendGridMessage message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    message.SetTemplateId("a179572c-a791-467e-aebb-5ea6cca0fb2c");

                    Response response = await client.SendEmailAsync(message);

                    TempData["EmailAddress"] = model.ContactEmail;

                    return RedirectToAction("Index", "Receipt", new { id = order.OrderID } );
                }
            }
            return View(model);
        }

        private static string CreatePlaintextEmail(Order p)
        {
            StringBuilder builder = new StringBuilder();
            //Your order: Name Description Unit Price Quantity Total Price

            builder.Append("Your Paracord Store Order: ");
            builder.Append(p.OrderID);
            foreach (var product in p.OrderedProducts)
            {
                builder.Append("Product Name: ");
                builder.Append(product.Product.Name);
                builder.Append(" ");

                builder.Append("Product Description: ");
                builder.Append(product.Product.Description);
                builder.Append(" ");
                builder.Append("Product Price: ");
                builder.Append((product.ProductPrice ?? 0).ToString("c"));
                builder.Append(" ");
                builder.Append("Product Quantity: ");
                builder.Append(product.Quantity);
                builder.Append(" ");
                builder.Append("Total Price: ");
                builder.Append(((product.ProductPrice ?? 0) * (product.Quantity ?? 0)).ToString("c"));
                builder.Append(" ");
            }
            builder.Append("Sum Total: ");
            builder.Append(p.OrderedProducts.Sum(x => (x.Product.Price ?? 0) * x.Quantity ?? 0).ToString("c"));
            return builder.ToString();
        }

        private static string CreateReceiptEmail(Order p)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<table>");
            builder.Append("<thead><tr><th></th><th>Name</th><th>Description</th><th>Unit Price</th><th>Quantity</th><th>Total Price</th></tr></thead>");
            builder.Append("<tbody>");
            foreach (var product in p.OrderedProducts)
            {
                builder.Append("<tr><td></td>");
                builder.Append("<td>");
                builder.Append(product.Product.Name);
                builder.Append("</td>");

                builder.Append("<td>");
                builder.Append(product.Product.Description);
                builder.Append("</td>");
                builder.Append("<td>");
                builder.Append((product.ProductPrice ?? 0).ToString("c"));
                builder.Append("</td>");
                builder.Append("<td>");
                builder.Append(product.Quantity);
                builder.Append("</td>");
                builder.Append("<td>");
                builder.Append(((product.ProductPrice ?? 0) * (product.Quantity ?? 0)).ToString("c"));
                builder.Append("</td>");

                builder.Append("</tr>");
            }
            builder.Append("</tbody><tfoot><tr><td colspan=\"5\">Total</td><td>");
            builder.Append(p.OrderedProducts.Sum(x => (x.Product.Price ?? 0) * x.Quantity ?? 0).ToString("c"));
            builder.Append("</td></tr></tfoot></table>");
            return builder.ToString();
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