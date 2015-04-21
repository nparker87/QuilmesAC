namespace QuilmesAC.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;
    using Models;

    public static class LoginHelper
    {
        public static MembershipProvider Provider = new MembershipProvider();

        /// <summary> Produces a SHA256 hash string of the password </summary>
        /// <param name="username"></param>
        /// <param name="password">password to hash</param>
        /// <returns>SHA256 Hash string</returns>
        public static string EncryptPassword(string username, string password)
        {
            // this salt is different for every user to help prevent rainbow table attacks (even if this source code is compromised)
            var salt = "SomethingVeryCreativeShouldGoHere" + username;

            // use codepage 1252 because that is what sql server uses
            var passwordBytes = Encoding.Default.GetBytes(password);  // GetEncoding(1252)
            var saltBytes = Encoding.Default.GetBytes(salt);

            var hashBytes = new HMACSHA256(saltBytes).ComputeHash(passwordBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static bool ValidateLogin(this ModelStateDictionary modelState, string username, string password)
        {
            if (String.IsNullOrEmpty(username))
                modelState.AddModelError("username", "You must specify a username.");
            if (String.IsNullOrEmpty(password))
                modelState.AddModelError("password", "You must specify a password.");
            else if (!Provider.ValidateUser(username, password))
                modelState.AddModelError("", "The username or password provided is incorrect.");

            return modelState.IsValid;
        }
    }
}