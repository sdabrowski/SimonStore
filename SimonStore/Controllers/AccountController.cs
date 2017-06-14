using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Web;
using System.Web.Mvc;

namespace SimonStore.Controllers
{
    public class AccountController : Controller
    {
        //Declaratively allow only users 
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Register(string username, string email, string password)
        {
            //var userStore = new UserStore<IdentityUser>();
            var manager = HttpContext.GetOwinContext().GetUserManager<UserManager<IdentityUser>>();
            var user = new IdentityUser() { UserName = username, Email = email, EmailConfirmed = false };

            manager.UserTokenProvider =
                 new EmailTokenProvider<IdentityUser>();

            IdentityResult result = await manager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                //I have some options: log them in, or I can send them an email to "Confirm" their account details
                //I dont' have email set up this week, so we'll come back to that.

                string confirmationToken = await manager.GenerateEmailConfirmationTokenAsync(user.Id);

                string confirmationLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Confirm" + user.Id + "?token=" + confirmationToken;

                string apiKey = System.Configuration.ConfigurationManager.AppSettings["SendGrid.ApiKey"];

                SendGrid.ISendGridClient client = new SendGridClient(apiKey);
                EmailAddress from = new EmailAddress("admin@ParacordStore.com", "Paracord Store");

                EmailAddress to = new EmailAddress(email);

                string subject = "Confirm your Paracord Store Account";

                string htmlContent = string.Format("<a href=\"{0}\" > Confirm Your Account </ a > ", confirmationLink);
                string plainTextContent = confirmationLink;

                SendGridMessage message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                message.SetTemplateId("a179572c-a791-467e-aebb-5ea6cca0fb2c");

                Response response = await client.SendEmailAsync(message);

                TempData["EmailAddress"] = email;

                return RedirectToAction("ConfirmationSent");

                //This authentication manager will create a cookie for the current user, and that cookie will be exchanged on each request until the user logs out
                //var authenticationManager = HttpContext.GetOwinContext().Authentication;
                //var userIdentity = await manager.CreateIdentityAsync(user, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                //authenticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties() { }, userIdentity);
            }
            else
            {
                ViewBag.Error = result.Errors;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmationSent()
        {
            return View();
        }

        // GET Account/LogOff
        public ActionResult LogOff()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        // GET Account/LogOn
        public ActionResult LogOn()
        {
            return View();
        }

        // POST Account/LogOn
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> LogOn(string username, string password, bool? staySignedIn, string returnUrl)
        {

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);



            var user = await manager.FindByNameAsync(username);


            bool result = await manager.CheckPasswordAsync(user, password);
            if (result)
            {
                if (user.EmailConfirmed)
                {
                    //I have some options: log them in, or I can send them an email to "Confirm" their account details.'
                    //I don't have email set up this week, so we'll come back to that.

                    //This authentication manager will create a cookie for the current user, and that cookie will be exchanged on each request until the user logs out
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = await manager.CreateIdentityAsync(user, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties() { }, userIdentity);
                }
                else
                {
                    ViewBag.Error = new string[] { "Your email address has not been confirmed." };
                    return View();
                }
            }
            else
            {
                ViewBag.Error = new string[] { "Unable to Log In, check your username and password" };
                return View();
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(returnUrl);
            }

        }

        public async System.Threading.Tasks.Task<ActionResult> Confirm(string id, string token)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            manager.UserTokenProvider =
                new EmailTokenProvider<IdentityUser>();


            var result = await manager.ConfirmEmailAsync(id, token);
            if (result.Succeeded)
            {
                TempData["Confirmed"] = true;
                return RedirectToAction("LogOn");
            }
            else
            {
                return HttpNotFound();
            }


        }

        /// <summary>
        /// Display a form for a user to enter their username / email address
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Validate the posted information and send an email with a reset token
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            return RedirectToAction("ResetPasswordSent");
        }


        /// <summary>
        /// Simple - Return a view
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPasswordSent()
        {
            return View();
        }

        //validate the reset token and display a form if it is valid
        public ActionResult ResetPassword(string token)
        {
            return View();
        }

        public ActionResult ResetPassword(string token, string newPassword)
        {

            return RedirectToAction("LogOn");
        }
    }
}