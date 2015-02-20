namespace QuilmesAC.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;
    using Models;

    public class PlayerViewModel : BaseViewModel
    {
        public PlayerViewModel()
        {
            CurrentTab = "Player";
        }

        public PlayerViewModel(QuilmesDataContext model)
        {
            CurrentTab = "Player";
            PopulateSelectLists(model);
        }

        public PlayerViewModel(QuilmesDataContext model, Player player)
        {
            CurrentTab = "Player";
            PopulateSelectLists(model);
            Add(player);
        }

        [DisplayName("ID")]
        public long ID { get; set; }

        [DisplayName("First Name:")]
        public string FirstName { get; set; }

        [DisplayName("Last Name:")]
        public string LastName { get; set; }

        [DisplayName("Number:")]
        public int? Number { get; set; }

        [DisplayName("Created Date:")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Status:")]
        public long StatusID { get; set; }

        public SelectList Statuses { get; set; }

        private void PopulateSelectLists(QuilmesDataContext model)
        {
            Statuses = new SelectList(model.GetStatuses(), "ID", "Name");
        }

        private void Add(Player player)
        {
            ID = player.ID;
            FirstName = player.FirstName;
            LastName = player.LastName;
            Number = player.Number;
            StatusID = player.StatusID;
        }
    }
}