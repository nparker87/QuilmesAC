namespace QuilmesAC.Controllers
{
    using Helpers;
    using System;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using ViewModels;

    public class StandingController : BaseController
    {
        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Edit()
        {
            // Use last sorting choices if saved - otherwise use defaults.
            ViewBag.SortBy = (Session["StandingLastSortID"] ?? "ID");
            ViewBag.SortOrder = (Session["StandingLastSortOrder"] ?? "asc");
            ViewBag.Page = (Session["StandingLastSortPage"] ?? 1);
            ViewBag.Rows = (Session["StandingLastSortRows"] ?? 50);

            return View(new StandingViewModel());
        }

        [HttpPost]
        public ActionResult GridData(string sidx, string sord, int page, int rows, bool _search, string filters)
        {
            // Save the last sort choices to session data.
            Session["StandingLastSortID"] = sidx;
            Session["StandingLastSortOrder"] = sord;
            Session["StandingLastSortPage"] = page;
            Session["StandingLastSortRows"] = rows;

            var table = from standing in QuilmesModel.Standings
                        select standing;

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
                        table = table.Where(r.Field + ".ToString().Contains(@0)", r.Data);
                    }
                    catch (Exception)
                    {
                        // null types
                        table = table.Where(r.Field + ".Value.ToString().Contains(@0)", r.Data);
                    }
                }
            }

            int totalRecords = table.Count();
            var currentPage = table
                .OrderBy(sidx + " " + sord)
                .Skip((Convert.ToInt32(page) - 1) * rows)
                .Take(rows)
                .ToList();

            // This JSON Documentation is here: http://www.secondpersonplural.ca/jqgriddocs/_2eb0f6jhe.htm
            var jsonData = new
            {
                total = (int)Math.Ceiling(totalRecords / (float)rows), // total number of pages
                page, // current page
                rows = ( // actual data records for current page
                    from t in currentPage
                    select new
                    {
                        id = t.ID,
                        cell = new[]
                        {
                            t.ID.ToString(),
                            t.Season.DisplayName,
                            t.Opponent.Name,
                            t.GamesPlayed.ToString(),
                            t.Win.ToString(),
                            t.Draw.ToString(),
                            t.Loss.ToString(),
                            t.GoalsFor.ToString(),
                            t.GoalsAgainst.ToString(),
                            Convert.ToString(t.GoalsFor - t.GoalsAgainst),
                            Convert.ToString((t.Win * 3) + t.Draw)
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        /// <summary> Modal add </summary>
        public ContentResult AddStanding(StandingViewModel standingViewModel)
        {
            QuilmesModel.Add(standingViewModel);
            QuilmesModel.Save();
            return Content("Success");
        }

        /// <summary> Modal edit </summary>
        public ContentResult EditStanding(StandingViewModel standingViewModel)
        {
            var standing = QuilmesModel.GetStandingByID(standingViewModel.ID);
            QuilmesModel.Update(standing, standingViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        /// <summary> Modal delete </summary>
        public ContentResult DeleteStanding(int id)
        {
            var standing = QuilmesModel.GetStandingByID(id);
            QuilmesModel.Delete(standing);
            QuilmesModel.Save();

            return Content("Success");
        }
    }
}