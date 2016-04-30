namespace QuilmesAC.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class FormationViewModel : BaseViewModel
    {
        public FormationViewModel()
        {
            CurrentTab = "Admin";
        }

        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Name:")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
    }
}