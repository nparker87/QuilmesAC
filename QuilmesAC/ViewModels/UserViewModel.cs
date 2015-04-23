namespace QuilmesAC.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class UserViewModel : BaseViewModel
    {
        [DisplayName("Username:")]
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }

        [DisplayName("Password:")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [DisplayName("Email:")]
        public string Email { get; set; }
    }
}