namespace QuilmesAC.Controllers
{
    using System.Web.Mvc;
    using Helpers;
    using ViewModels;

    public class OpponentController : BaseController
    {
        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(new OpponentViewModel());
        }

        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Add()
        {
            return View(new OpponentViewModel());
        }

        [HttpPost]
        public ActionResult Add(OpponentViewModel submission)
        {
            if (!ModelState.IsValid) return View(submission);
            QuilmesModel.AddOpponent(submission);
            QuilmesModel.Save();
            return View(new OpponentViewModel());
        }
	}
}