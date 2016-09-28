namespace QuilmesAC.Controllers
{
	using System.Web.Mvc;
	using ViewModels;

	public class SponsorController : BaseController
	{
		public ActionResult Index()
		{
			return View(new BaseViewModel() { CurrentTab = "Sponsors" });
		}
	}
}