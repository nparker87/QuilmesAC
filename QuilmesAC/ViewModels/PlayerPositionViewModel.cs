namespace QuilmesAC.ViewModels
{
	using System.ComponentModel;

	public class PlayerPositionViewModel : BaseViewModel
	{
		public PlayerPositionViewModel()
		{
			CurrentTab = "Admin";
		}

		[DisplayName("ID")]
		public int ID { get; set; }

		[DisplayName("PlayerID")]
		public int PlayerID { get; set; }

		[DisplayName("PositionID")]
		public int PositionID { get; set; }

		[DisplayName("PrimaryPosition")]
		public bool PrimaryPosition { get; set; }
	}
}