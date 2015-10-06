namespace QuilmesAC.ViewModels
{
    using System.ComponentModel;

    public class LeagueTableViewModel : BaseViewModel
    {
        public LeagueTableViewModel()
        {
            CurrentTab = "LeagueTable";
        }

        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("SeasonID")]
        public int SeasonID { get; set; }

        [DisplayName("OpponentID")]
        public int OpponentID { get; set; }

        [DisplayName("Games Played")]
        public int GamesPlayed { get; set; }

        [DisplayName("Goals For")]
        public int GoalsFor { get; set; }

        [DisplayName("Goals Against")]
        public int GoalsAgainst { get; set; }
    }
}