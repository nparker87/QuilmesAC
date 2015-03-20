namespace QuilmesAC.Controllers
{
    using System.Web.Mvc;
    using ViewModels;

    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            var viewModel = new BaseViewModel
            {
                CurrentTab = "Admin"
            };
            return View(viewModel);
        }
    }
}