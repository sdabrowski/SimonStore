using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

[assembly: Microsoft.Owin.OwinStartup(typeof(CodingTemple.CodingCookware.Web.Startup))]
namespace CodingTemple.CodingCookware.Web
{

    public class Startup
    {
        public void Configuration(Owin.IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user            
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new Microsoft.Owin.PathString("/Account/LogOn")
            });

            app.CreatePerOwinContext(() =>
            {
                UserStore<IdentityUser> store = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
                manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();

                manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 4,
                    RequireDigit = false,
                    RequireUppercase = false,
                    RequireLowercase = false,
                    RequireNonLetterOrDigit = false

                };
                return manager;
            });

        }
    }
}