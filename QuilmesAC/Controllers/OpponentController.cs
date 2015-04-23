namespace QuilmesAC.Controllers
{
    using System;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Helpers;
    using ViewModels;

    public class OpponentController : BaseController
    {
        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Index()
        {
            // Use last sorting choices if saved - otherwise use defaults.
            ViewBag.SortBy = (Session["OpponentLastSortID"] ?? "Name");
            ViewBag.SortOrder = (Session["OpponentLastSortOrder"] ?? "asc");
            ViewBag.Page = (Session["OpponentLastSortPage"] ?? 1);
            ViewBag.Rows = (Session["OpponentLastSortRows"] ?? 50);

            return View(new OpponentViewModel());
        }

        /// <summary> Returns the JSON data to display a jqGrid of opponents </summary>
        /// POST: /Opponent/GridData
        [HttpPost]
        public ActionResult GridData(string sidx, string sord, int page, int rows, bool _search, string filters)
        {
            // Save the last sort choices to session data.
            Session["OpponentLastSortID"] = sidx;
            Session["OpponentLastSortOrder"] = sord;
            Session["OpponentLastSortPage"] = page;
            Session["OpponentLastSortRows"] = rows;

            var allRecords = from opponent in QuilmesModel.Opponents
                             select opponent;

            // Check for any filtering and prepare Where clauses.
            if (_search)
            {
                // Deserialize the filters.
                // TODO: MVC3 supports automagic JSON->object model binding. Cleaner to use that instead.
                var js = new JavaScriptSerializer();
                var search = js.Deserialize<JqgridHelper.FilterSettings>(filters);

                // Note: Only groupOp="AND" and op="cn" (contains) is supported. Other options are ignored.
                foreach (JqgridHelper.FilterRule r in search.Rules)
                {
                    // simplest way to handle both nullables and non-nullables
                    try
                    {
                        // for strings and other non-nullables
                        allRecords = allRecords.Where(r.Field + ".ToString().Contains(@0)", r.Data);
                    }
                    catch (Exception)
                    {
                        // null types
                        allRecords = allRecords.Where(r.Field + ".Value.ToString().Contains(@0)", r.Data);
                    }
                }
            }

            int totalRecords = allRecords.Count();
            var currentPage = allRecords
                .OrderBy(sidx + " " + sord + ", Name, ID asc")
                .Skip((Convert.ToInt32(page) - 1) * rows)
                .Take(rows)
                .ToList();

            // This JSON Documentation is here: http://www.secondpersonplural.ca/jqgriddocs/_2eb0f6jhe.htm
            var jsonData = new
            {
                total = (int)Math.Ceiling(totalRecords / (float)rows), // total number of pages
                page, // current page
                records = totalRecords, // total number of records of all pages
                rows = ( //actual data records for current page
                    from t in currentPage
                    select new
                    {
                        id = t.ID,
                        cell = new[]
                        {
                           t.ID.ToString(),
                           t.Name
                        }
                    }).ToArray()
            };
            return Json(jsonData);
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
            QuilmesModel.Add(submission);
            QuilmesModel.Save();
            return RedirectToAction("Index");
        }

        /// <summary> Modal add </summary>
        public ContentResult AddOpponent(OpponentViewModel opponentViewModel)
        {
            QuilmesModel.Add(opponentViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        /// <summary> Modal edit </summary>
        public ContentResult EditOpponent(OpponentViewModel opponentViewModel)
        {
            var opponent = QuilmesModel.GetOpponentByID(opponentViewModel.ID);
            QuilmesModel.Update(opponent, opponentViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        /// <summary> Modal delete </summary>
        public ContentResult DeleteOpponent(long id)
        {
            var opponent = QuilmesModel.GetOpponentByID(id);
            QuilmesModel.Delete(opponent);
            QuilmesModel.Save();

            return Content("Success");
        }
    }
}