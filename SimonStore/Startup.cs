using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
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
        }
    }
}