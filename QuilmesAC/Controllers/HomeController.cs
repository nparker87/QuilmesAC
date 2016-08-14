namespace QuilmesAC.Controllers
{
	using System.Web.Mvc;
	using ViewModels;

	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			var viewModel = new BaseViewModel
			{
				CurrentTab = "Home"
			};
			return View(viewModel);
		}
	}
}