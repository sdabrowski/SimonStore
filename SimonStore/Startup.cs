using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: Microsoft.Owin.OwinStartup(typeof(SimonStore.Startup))]

namespace SimonStore
{
    public class Startup
    {
        public void Configuration(Owin.IAppBuilder app)
        {
            //TODO: add stuff to "app" to set up login / authentication
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new Microsoft.Owin.PathString("/Account/LogOn")
            });
        }
    }
}