namespace QuilmesAC.Controllers
{
    using QuilmesAC.ViewModels;
    using System.Web.Mvc;

    public class AboutController : BaseController
    {
        public ActionResult Index()
        {
            return View(new BaseViewModel() { CurrentTab = "About" });
        }
    }
}