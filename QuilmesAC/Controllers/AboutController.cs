namespace QuilmesAC.Controllers
{
	using System.Web.Mvc;
	using ViewModels;

	public class AboutController : BaseController
	{
		public ActionResult Index()
		{
			return View(new BaseViewModel() { CurrentTab = "About" });
		}
	}
}