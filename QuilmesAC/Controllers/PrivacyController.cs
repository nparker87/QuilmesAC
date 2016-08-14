namespace QuilmesAC.Controllers
{
	using System.Web.Mvc;
	using ViewModels;

	public class PrivacyController : BaseController
	{
		public ActionResult Index()
		{
			return View(new BaseViewModel());
		}
	}
}