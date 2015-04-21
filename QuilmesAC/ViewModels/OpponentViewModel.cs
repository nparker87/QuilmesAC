namespace QuilmesAC.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Models;

    public class OpponentViewModel : BaseViewModel
    {
        public OpponentViewModel()
        {
            CurrentTab = "Admin";
        }

        public OpponentViewModel(Opponent opponent)
        {
            CurrentTab = "Admin";
            Add(opponent);
        }

        [DisplayName("ID")]
        public long ID { get; set; }

        [DisplayName("Name:")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        private void Add(Opponent opponent)
        {
            ID = opponent.ID;
            Name = opponent.Name;
        }
    }
}