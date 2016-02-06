namespace QuilmesAC.Controllers
{
    using Helpers;
    using Models;
    using System;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Security;
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

            returnUrl = (string)TempData["returnUrl"];

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

            // create authentication cookie with roles
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
            if (ModelState.IsValid)
            {
                QuilmesModel.AddUser(submission);
                QuilmesModel.Save();
                return RedirectToAction("RegisterSuccess", "User");
            }

            // TODO: Return Error
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RegisterSuccess()
        {
            return View(new BaseViewModel());
        }

        public ActionResult ForgotPassword()
        {
            return View(new ForgotPassword());
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword submission)
        {
            var user = QuilmesModel.GetUserByEmail(submission.Email);
            if (user == null || user.Email == null)
                ModelState.AddModelError("", "The email you entered does not belong to any account.");

            if (ModelState.IsValid)
            {
                // Set up the reset password verification if blank
                if (user.PasswordReset == null)
                {
                    user.PasswordReset = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 19);
                    QuilmesModel.Save();
                }

                // Email the user
                var domain = WebConfigurationManager.AppSettings["Domain"];
                var resetUrl = String.Format("{0}/User/Reset/{1}", domain, user.PasswordReset);
                var emailMsg = "Your login username is: " + user.Username + "<br /><br />";
                emailMsg += "To reset your password to a new one, visit the special link below to complete your new password request:<br />";
                emailMsg += "<a href=\"" + resetUrl + "\">" + resetUrl + "</a><br /><br />";
                emailMsg += "If you did not request your username nor a password reset, please email <a href=\"mailto:quilmesrva@gmail.com\">quilmesrva@gmail.com</a>. ";
                emailMsg += "As long as you do not click the reset link contained in this email, no action will be taken and your password will remain the same.";

                Emailer.SendMsg(user.Email, "quilmesrva@gmail.com", "QuilmesRVA", "QuilmesRVA website login information", emailMsg, null, null);

                return RedirectToAction("EmailSent");
            }

            return View(submission);
        }

        public ActionResult Reset(string id)
        {
            User user = QuilmesModel.GetUserByPasswordReset(id);
            if (user != null && user.PasswordReset != null)
            {
                var viewModel = new PasswordReset
                {
                    UserName = user.Username,
                    OldPassword = id
                };

                return View(viewModel);
            }

            // password reset is null or invalid
            return View();
        }

        [HttpPost]
        public ActionResult Reset(PasswordReset passwordReset)
        {
            if (ModelState.IsValid)
            {
                // get user
                User user = QuilmesModel.GetUserByPasswordReset(passwordReset.OldPassword);

                if (user != null && user.PasswordReset != null)
                {
                    // change to new password
                    user.Password = LoginHelper.EncryptPassword(user.Username, passwordReset.NewPassword);
                    QuilmesModel.Save();

                    return RedirectToAction("ResetSuccess");
                }
            }

            return View();
        }

        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Index()
        {
            // Use last sorting choices if saved - otherwise use defaults.
            ViewBag.UserSortBy = (Session["UserLastSortID"] ?? "ID");
            ViewBag.UserSortOrder = (Session["UserLastSortOrder"] ?? "asc");
            ViewBag.UserPage = (Session["UserLastSortPage"] ?? 1);
            ViewBag.UserRows = (Session["UserLastSortRows"] ?? 50);

            return View(new UserViewModel());
        }

        /// <summary> Returns the JSON data to display a jqGrid of users </summary>
        /// POST: /User/GridData
        [HttpPost]
        public ActionResult GridData(string sidx, string sord, int page, int rows, bool _search, string filters)
        {
            // Save the last sort choices to session data.
            Session["UserLastSortID"] = sidx;
            Session["UserLastSortOrder"] = sord;
            Session["UserLastSortPage"] = page;
            Session["UserLastSortRows"] = rows;

            var allRecords = from user in QuilmesModel.Users
                             select user;

            // Check for any filtering and prepare Where clauses.
            if (_search)
            {
                // Deserialize the filters.
                // TODO: MVC3 supports automagic JSON->object model binding. Cleaner to use that instead.
                var js = new JavaScriptSerializer();
                var search = js.Deserialize<JqgridHelper.FilterSettings>(filters);

                // Note: Only groupOp="AND" and op="cn" (contains) is supported. Other options are ignored.
                foreach (JqgridHelper.FilterRule r in search.Rules)
                {
                    // simplest way to handle both nullables and non-nullables
                    try
                    {
                        // for strings and other non-nullables
                        allRecords = allRecords.Where(r.Field + ".ToString().Contains(@0)", r.Data);
                    }
                    catch (Exception)
                    {
                        // null types
                        allRecords = allRecords.Where(r.Field + ".Value.ToString().Contains(@0)", r.Data);
                    }
                }
            }

            var totalRecords = allRecords.Count();
            var currentPage = allRecords
                .OrderBy(sidx + " " + sord + ", ID asc")
                .Skip((Convert.ToInt32(page) - 1) * rows)
                .Take(rows)
                .ToList();

            // This JSON Documentation is here: http://www.secondpersonplural.ca/jqgriddocs/_2eb0f6jhe.htm
            var jsonData = new
            {
                total = (int)Math.Ceiling(totalRecords / (float)rows), // total number of pages
                page, // current page
                records = totalRecords, // total number of records of all pages
                rows = ( //actual data records for current page
                    from t in currentPage
                    select new
                    {
                        id = t.ID,
                        cell = new[]
                        {
                            t.ID.ToString(),
                            t.Username,
                            t.Email
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            ViewBag.RolesSortBy = (Session["RolesLastSortID"] ?? "ID");
            ViewBag.RolesSortOrder = (Session["RolesLastSortOrder"] ?? "desc");
            ViewBag.RolesPage = (Session["RolesLastSortPage"] ?? 1);
            ViewBag.RolesRows = (Session["RolesLastSortRows"] ?? 50);

            var user = QuilmesModel.GetUserByID(id);
            return View(new UserViewModel(user));
        }

        /// <summary> Returns the JSON data to display a jqGrid of users </summary>
        /// POST: /User/GridData
        [HttpPost]
        public ActionResult RolesGridData(string sidx, string sord, int page, int rows, bool _search, string filters, int userID)
        {
            // Save the last sort choices to session data.
            Session["RolesLastSortID"] = sidx;
            Session["RolesLastSortOrder"] = sord;
            Session["RolesLastSortPage"] = page;
            Session["RolesLastSortRows"] = rows;

            var allRecords = from userRole in QuilmesModel.UserRoles
                             where userRole.UserID == userID
                             select userRole;

            // Check for any filtering and prepare Where clauses.
            if (_search)
            {
                // Deserialize the filters.
                // TODO: MVC3 supports automagic JSON->object model binding. Cleaner to use that instead.
                var js = new JavaScriptSerializer();
                var search = js.Deserialize<JqgridHelper.FilterSettings>(filters);

                // Note: Only groupOp="AND" and op="cn" (contains) is supported. Other options are ignored.
                foreach (JqgridHelper.FilterRule r in search.Rules)
                {
                    // simplest way to handle both nullables and non-nullables
                    try
                    {
                        // for strings and other non-nullables
                        allRecords = allRecords.Where(r.Field + ".ToString().Contains(@0)", r.Data);
                    }
                    catch (Exception)
                    {
                        // null types
                        allRecords = allRecords.Where(r.Field + ".Value.ToString().Contains(@0)", r.Data);
                    }
                }
            }

            var totalRecords = allRecords.Count();
            var currentPage = allRecords
                .OrderBy(sidx + " " + sord + ", ID asc")
                .Skip((Convert.ToInt32(page) - 1) * rows)
                .Take(rows)
                .ToList();

            // This JSON Documentation is here: http://www.secondpersonplural.ca/jqgriddocs/_2eb0f6jhe.htm
            var jsonData = new
            {
                total = (int)Math.Ceiling(totalRecords / (float)rows), // total number of pages
                page, // current page
                records = totalRecords, // total number of records of all pages
                rows = ( //actual data records for current page
                    from t in currentPage
                    select new
                    {
                        id = t.ID,
                        cell = new[]
                        {
                            t.ID.ToString(),
                            t.Role.Name
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        public ContentResult AddUserRole(UserRoleViewModel userRoleViewModel)
        {
            QuilmesModel.Add(userRoleViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult EditUserRole(UserRoleViewModel userRoleViewModel)
        {
            var userRole = QuilmesModel.GetUserRoleByID(userRoleViewModel.ID);
            QuilmesModel.Update(userRole, userRoleViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult DeleteUserRole(int id)
        {
            var userRole = QuilmesModel.GetUserRoleByID(id);
            QuilmesModel.Delete(userRole);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ActionResult ResetSuccess()
        {
            return View(new BaseViewModel());
        }

        public ActionResult EmailSent()
        {
            return View(new BaseViewModel());
        }

        public ActionResult NotAuthorized()
        {
            return View(new UserViewModel());
        }
    }
}