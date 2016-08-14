namespace QuilmesAC.Controllers
{
	using System.Web.Mvc;
	using ViewModels;

	public class AdminController : BaseController
	{
		public ActionResult Index()
		{
			return View(new AdminViewModel());
		}
	}
}