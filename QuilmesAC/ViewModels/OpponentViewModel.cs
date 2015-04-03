namespace QuilmesAC.ViewModels
{
    using System.ComponentModel;
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
        public string Name { get; set; }

        private void Add(Opponent opponent)
        {
            ID = opponent.ID;
            Name = opponent.Name;
        }
    }
}