namespace QuilmesAC.ViewModels
{
    public class CardViewModel : BaseViewModel
    {
        public CardViewModel()
        {
        }

        public int ID { get; set; }

        public int PlayerID { get; set; }

        public int MatchID { get; set; }

        public int TypeID { get; set; }
    }
}