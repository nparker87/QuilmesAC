namespace QuilmesAC.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;
    using Helpers;
    using Models;
    using ViewModels;

    public class UserController : BaseController
    {
        public ActionResult Login(string returnUrl)
        {
            // check if already logged in
            if (!String.IsNullOrEmpty(User.Identity.Name))
            {
                if (!String.IsNullOrEmpty(returnUrl) && returnUrl != "/")
                    return Redirect(returnUrl);
                return Redirect(WebConfigurationManager.AppSettings["DefaultReturnUrl"]);
            }

            // store and remove the query string from the url
            if (returnUrl != null)
            {
                TempData["returnUrl"] = returnUrl;
                var redirectToLogin = new Login();
                return View(redirectToLogin);
            }

            returnUrl = (string) TempData["returnUrl"];

            // show login view with returnurl
            var viewModel = new Login
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            // show error if wrong username or password
            if (!ModelState.ValidateLogin(username, password))
                return View(new Login { ReturnUrl = returnUrl }); // maintain the returnUrl

            var user = QuilmesModel.GetUserByUsername(username);

            // populate roles
            var roles = "";
            if (user.UserRoles.Any())
                roles = user.UserRoles.Aggregate(roles, (current, role) => current + (role.Role.Name + ","));
            roles = roles.TrimEnd(',');

            // create autentication cookie with roles
            var ticket = new FormsAuthenticationTicket(
                1,                          // version
                user.Username,              // username
                DateTime.Now,               // create time
                DateTime.Now.AddHours(2),   // expire time
                false,                      // persist
                roles);                     // user data
            var strEncryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strEncryptedTicket);
            Response.Cookies.Add(cookie);

            // TODO: login succeeded. add log message to track usage, and update the user's last login time
            if (!String.IsNullOrEmpty(returnUrl) && returnUrl != "/")
                return Redirect(returnUrl);
            return Redirect(WebConfigurationManager.AppSettings["DefaultReturnUrl"]);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(WebConfigurationManager.AppSettings["DefaultReturnUrl"]);
        }

        public ActionResult Register()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Register(UserViewModel submission)
        {
            QuilmesModel.AddUser(submission);
            QuilmesModel.Save();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult NotAuthorized()
        {
            return View(new UserViewModel());
        }
    }
}