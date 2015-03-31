namespace QuilmesAC.ViewModels
{
    using System.ComponentModel;
    using Models;

    public class OpponentViewModel : BaseViewModel
    {
        public OpponentViewModel()
        {
            CurrentTab = "Opponent";
        }

        public OpponentViewModel(Opponent opponent)
        {
            CurrentTab = "Oponent";
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