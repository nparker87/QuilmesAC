namespace QuilmesAC.ViewModels
{
	using Models;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class UserViewModel : BaseViewModel
	{
		public UserViewModel()
		{
			CurrentTab = "Admin";
		}

		public UserViewModel(User user)
		{
			CurrentTab = "Admin";
			Add(user);
		}

		public int ID { get; set; }

		[DisplayName("Username:")]
		[Required(ErrorMessage = "Username Required")]
		public string Username { get; set; }

		[DisplayName("Password:")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Password Required")]
		public string Password { get; set; }

		[DisplayName("Email:")]
		public string Email { get; set; }

		private void Add(User user)
		{
			ID = user.ID;
			Username = user.Username;
			Email = user.Email;
		}
	}
}