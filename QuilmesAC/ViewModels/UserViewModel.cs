namespace QuilmesAC.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class UserViewModel : BaseViewModel
    {
        public UserViewModel()
        {
        }

        [DisplayName("Username:")]
        public string Username { get; set; }

        [DisplayName("Password:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Email:")]
        public string Email { get; set; }
    }
}