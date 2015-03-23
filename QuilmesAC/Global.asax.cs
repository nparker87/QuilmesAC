﻿namespace QuilmesAC
{
    using System;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using App_Start;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return;
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            if (authTicket == null) return;
            var roles = authTicket.UserData.Split(',');
            var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
            Context.User = userPrincipal;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }
    }
}