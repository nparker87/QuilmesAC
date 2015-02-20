namespace QuilmesAC.ViewModels
{
    public class GoalViewModel : BaseViewModel
    {
        public GoalViewModel()
        {
        }

        public int ID { get; set; }

        public int PlayerID { get; set; }

        public int MatchID { get; set; }
    }
}