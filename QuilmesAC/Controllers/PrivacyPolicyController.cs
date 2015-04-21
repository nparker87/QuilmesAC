namespace QuilmesAC.Controllers
{
    using System.Web.Mvc;
    using ViewModels;

    public class PrivacyPolicyController : BaseController
    {
        public ActionResult Index()
        {
            return View(new BaseViewModel());
        }
    }
}