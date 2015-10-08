namespace QuilmesAC.ViewModels
{
    using System.ComponentModel;

    public class StandingViewModel : BaseViewModel
    {
        public StandingViewModel()
        {
            CurrentTab = "Admin";
        }

        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("SeasonID")]
        public int SeasonID { get; set; }

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
    }
}