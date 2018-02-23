namespace QuilmesAC.ViewModels
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

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
			StatusID = 1;
			SeasonID = model.GetCurrentSeason();
			Players = model.GetPlayersByStatusID(StatusID);
			Goals = model.GetGoalsBySeasonAndPlayer(ID, SeasonID);
            CurrentRoster = model.GetCurrentRoster();
		}

		public PlayerViewModel(QuilmesDataContext model, Player player)
		{
			CurrentTab = "Player";
			PopulateSelectLists(model);
			Add(player);
		}

		[DisplayName("ID")]
		public int ID { get; set; }

		[DisplayName("First Name:")]
		[Required(ErrorMessage = "Required")]
		public string FirstName { get; set; }

		[DisplayName("Last Name:")]
		[Required(ErrorMessage = "Required")]
		public string LastName { get; set; }

		[DisplayName("Number:")]
		public int? Number { get; set; }

		[DisplayName("Created Date:")]
		public DateTime? CreatedDate { get; set; }

		[DisplayName("Status:")]
		public int StatusID { get; set; }

		public SelectList Statuses { get; set; }

		[DisplayName("Season:")]
		public int SeasonID { get; set; }

		public SelectList Seasons { get; set; }

		public List<Player> Players { get; set; }

        public List<CurrentRoster> CurrentRoster { get; set; }

		public int Goals { get; set; }

		public int Assists { get; set; }

		public int YellowCards { get; set; }

		public int RedCards { get; set; }

		private void PopulateSelectLists(QuilmesDataContext model)
		{
			Statuses = new SelectList(model.GetStatuses(), "ID", "Name");
			Seasons = new SelectList(model.GetSeasons(), "ID", "DisplayName");
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