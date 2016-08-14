namespace QuilmesAC.Models
{
	using Helpers;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Web.Mvc;
	using System.Web.Security;
	using ViewModels;

	public class Login : BaseViewModel
	{
		[Display(Name = "Username:")]
		[Required(ErrorMessage = "Required")]
		public string Username { get; set; }

		[Display(Name = "Password:")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Required")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

		[Display(Name = "Return Url")]
		public string ReturnUrl { get; set; }
	}

	public class ForgotPassword : BaseViewModel
	{
		[Required]
		[Display(Name = "Email:")]
		public string Email { get; set; }
	}

	public class PasswordReset : BaseViewModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string OldPassword { get; set; }

		[Required]
		//[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		[RegularExpression("[\\S]{5,}", ErrorMessage = "Must be at least 5 characters.")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	#region Providers

	public class MembershipProvider : System.Web.Security.MembershipProvider
	{
		#region Unimplemented MembershipProvider Methods

		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			throw new NotImplementedException();
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			throw new NotImplementedException();
		}

		public override bool EnablePasswordReset
		{
			get { throw new NotImplementedException(); }
		}

		public override bool EnablePasswordRetrieval
		{
			get { throw new NotImplementedException(); }
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}

		public override string GetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		public override string GetUserNameByEmail(string email)
		{
			throw new NotImplementedException();
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { throw new NotImplementedException(); }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new NotImplementedException(); }
		}

		public override int MinRequiredPasswordLength
		{
			get { throw new NotImplementedException(); }
		}

		public override int PasswordAttemptWindow
		{
			get { throw new NotImplementedException(); }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new NotImplementedException(); }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { throw new NotImplementedException(); }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { throw new NotImplementedException(); }
		}

		public override bool RequiresUniqueEmail
		{
			get { throw new NotImplementedException(); }
		}

		public override string ResetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		public override bool UnlockUser(string userName)
		{
			throw new NotImplementedException();
		}

		public override void UpdateUser(MembershipUser user)
		{
			throw new NotImplementedException();
		}

		#endregion Unimplemented MembershipProvider Methods

		private readonly QuilmesDataContext _model;

		public MembershipProvider()
			: this(null)
		{
		}

		public MembershipProvider(QuilmesDataContext model)
		{
			_model = model;
		}

		public User User
		{
			get;
			private set;
		}

		public User CreateUser(string fullName, string passWord, string email)
		{
			return (null);
		}

		public override bool ValidateUser(string username, string password)
		{
			if (string.IsNullOrEmpty(password.Trim())) return false;

			// TODO: turn on eager loading instead of making a new QuilmesDataContext each time.
			var user = new QuilmesDataContext().GetUserByUsername(username);
			if (user == null) return false;

			string hash = LoginHelper.EncryptPassword(user.Username, password);
			if (user.Password == hash)
			{
				User = user;
				return true;
			}

			// login failed. add log message to track down an obscure bug.
			// new Logger("Alumni Login", String.Format("User {0} failed with hash {1}. valid hash {2}", username, hash, user.Password), user.ID);

			return false;
		}
	}

	public class RoleProvider : System.Web.Security.RoleProvider
	{
		#region Unimplemented RoleProvider Methods

		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		#endregion Unimplemented RoleProvider Methods

		private readonly QuilmesDataContext _model = new QuilmesDataContext();

		public RoleProvider()
			: this(new QuilmesDataContext())
		{
		}

		public RoleProvider(QuilmesDataContext model)
		{
			_model = model;
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			User user = _model.GetUserByUsername(username);
			if (user != null)
				return true; //user.IsInRole(roleName);
			return false;
		}

		public override string[] GetRolesForUser(string username)
		{
			//User user = _repository.GetUserByUsername(username);
			//string[] roles = new string[user.Role.Rights.Count + 1];
			//roles[0] = user.Role.Description;
			//int idx = 0;
			//foreach (Right right in user.Role.Rights)
			//    roles[++idx] = right.Description;

			//return roles;
			return null;
		}
	}

	#endregion Providers

	#region Services

	// The FormsAuthentication type is sealed and contains static members, so it is difficult to
	// unit test code that calls its members. The interface and helper class below demonstrate
	// how to create an abstract wrapper around such a type in order to make the AccountController
	// code unit testable.
	public interface IMembershipService
	{
		int MinPasswordLength { get; }

		bool ValidateUser(string userName, string password);

		MembershipCreateStatus CreateUser(string userName, string password, string email);

		bool ChangePassword(string userName, string oldPassword, string newPassword);
	}

	public class AccountMembershipService : IMembershipService
	{
		private readonly System.Web.Security.MembershipProvider _provider;

		public AccountMembershipService()
			: this(null)
		{
		}

		public AccountMembershipService(System.Web.Security.MembershipProvider provider)
		{
			_provider = provider ?? Membership.Provider;
		}

		public int MinPasswordLength
		{
			get
			{
				return _provider.MinRequiredPasswordLength;
			}
		}

		public bool ValidateUser(string userName, string password)
		{
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
			if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

			return _provider.ValidateUser(userName, password);
		}

		public MembershipCreateStatus CreateUser(string userName, string password, string email)
		{
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
			if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
			if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

			MembershipCreateStatus status;
			_provider.CreateUser(userName, password, email, null, null, true, null, out status);
			return status;
		}

		public bool ChangePassword(string userName, string oldPassword, string newPassword)
		{
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
			if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
			if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

			// The underlying ChangePassword() will throw an exception rather
			// than return false in certain failure scenarios.
			try
			{
				MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
				return currentUser != null && currentUser.ChangePassword(oldPassword, newPassword);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (MembershipPasswordException)
			{
				return false;
			}
		}
	}

	public interface IFormsAuthenticationService
	{
		void SignIn(string userName, bool createPersistentCookie);

		void SignOut();
	}

	public class FormsAuthenticationService : IFormsAuthenticationService
	{
		public void SignIn(string userName, bool createPersistentCookie)
		{
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}
	}

	#endregion Services

	#region Validation

	public static class AccountValidation
	{
		public static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "Username already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A username for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
	{
		private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
		private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

		public ValidatePasswordLengthAttribute()
			: base(DefaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
				name, _minCharacters);
		}

		public override bool IsValid(object value)
		{
			var valueAsString = value as string;
			return (valueAsString != null && valueAsString.Length >= _minCharacters);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			return new[]{
				new ModelClientValidationStringLengthRule(FormatErrorMessage(metadata.GetDisplayName()), _minCharacters, int.MaxValue)
			};
		}
	}

	#endregion Validation
}