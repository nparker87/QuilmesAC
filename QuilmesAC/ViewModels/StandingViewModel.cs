namespace QuilmesAC.ViewModels
{
	using QuilmesAC.Models;
	using System.ComponentModel;
	using System.Web.Mvc;

	public class StandingViewModel : BaseViewModel
	{
		public StandingViewModel()
		{
			CurrentTab = "Admin";
		}

		public StandingViewModel(QuilmesDataContext model)
		{
			CurrentTab = "Admin";
			PopulateSelectLists(model);
			SeasonID = model.GetCurrentSeason().ID;
		}

		[DisplayName("ID")]
		public int ID { get; set; }

		[DisplayName("SeasonID")]
		public int SeasonID { get; set; }

		public SelectList Seasons { get; set; }

		[DisplayName("OpponentID")]
		public int OpponentID { get; set; }

		[DisplayName("Games Played")]
		public int? GamesPlayed { get; set; }

		[DisplayName("Win")]
		public int? Win { get; set; }

		[DisplayName("Draw")]
		public int? Draw { get; set; }

		[DisplayName("Loss")]
		public int? Loss { get; set; }

		[DisplayName("Goals For")]
		public int? GoalsFor { get; set; }

		[DisplayName("Goals Against")]
		public int? GoalsAgainst { get; set; }

		[DisplayName("Position")]
		public int? Position { get; set; }

		private void PopulateSelectLists(QuilmesDataContext model)
		{
			Seasons = new SelectList(model.GetSeasons(), "ID", "DisplayName");
		}
	}
}