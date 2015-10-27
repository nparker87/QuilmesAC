namespace QuilmesAC.Controllers
{
    using System;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Helpers;
    using ViewModels;

    public class MatchController : BaseController
    {
        public ActionResult Index()
        {
            // Use last sorting choices if saved - otherwise use defaults.
            ViewBag.SortBy = (Session["MatchLastSortID"] ?? "MatchDay");
            ViewBag.SortOrder = (Session["MatchLastSortOrder"] ?? "asc");
            ViewBag.Page = (Session["MatchLastSortPage"] ?? 1);
            ViewBag.Rows = (Session["MatchLastSortRows"] ?? 50);
            ViewBag.StandingSortBy = (Session["StandingLastSortID"] ?? "Position");
            ViewBag.StandingSortOrder = (Session["StandingLastSortOrder"] ?? "asc");
            ViewBag.StandingPage = (Session["StandingLastSortPage"] ?? 1);
            ViewBag.StandingRows = (Session["StandingLastSortRows"] ?? 50);

            return View(new MatchViewModel(QuilmesModel));
        }

        /// <summary> Returns the JSON data to display a jqGrid of matches </summary>
        /// POST: /Match/GridData
        [HttpPost]
        public ActionResult GridData(string sidx, string sord, int page, int rows, bool _search, string filters, string seasonID)
        {
            // Save the last sort choices to session data.
            Session["MatchLastSortID"] = sidx;
            Session["MatchLastSortOrder"] = sord;
            Session["MatchLastSortPage"] = page;
            Session["MatchLastSortRows"] = rows;

            var matches = from match in QuilmesModel.Matches
                          select match;

            // Get matches for a specific season else get all played matches, no future ones
            if (!String.IsNullOrWhiteSpace(seasonID))
                matches = matches.Where(x => x.SeasonID == Int32.Parse(seasonID));
            else
                matches = matches.Where(x => x.Result != null);

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
                        matches = matches.Where(r.Field + ".ToString().Contains(@0)", r.Data);
                    }
                    catch (Exception)
                    {
                        // null types
                        matches = matches.Where(r.Field + ".Value.ToString().Contains(@0)", r.Data);
                    }
                }
            }

            int totalRecords = matches.Count();
            var currentPage = matches
                .OrderBy(sidx + " " + sord + ", MatchDate, ID asc")
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
                           t.Season.DisplayName,
                           t.MatchDay.ToString(),
                           String.Format("{0:M/d/yyyy}", t.MatchDate),
                           t.Opponent.Name,
                           t.GoalsFor.ToString(),
                           t.GoalsAgainst.ToString(),
                           t.Result.ToString()
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        /// <summary> Returns the JSON data to display a jqGrid of the league table </summary>
        /// POST: /Match/StandingGridData
        [HttpPost]
        public ActionResult StandingGridData(string sidx, string sord, int page, int rows, bool _search, string filters, string seasonID)
        {
            // Save the last sort choices to session data.
            Session["StandingLastSortID"] = sidx;
            Session["StandingLastSortOrder"] = sord;
            Session["StandingLastSortPage"] = page;
            Session["StandingLastSortRows"] = rows;

            var standings = from standing in QuilmesModel.Standings
                            select standing;

            // Get matches for a specific season else get all played matches, no future ones
            if (!String.IsNullOrWhiteSpace(seasonID))
                standings = standings.Where(x => x.SeasonID == Int32.Parse(seasonID));

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
                        standings = standings.Where(r.Field + ".ToString().Contains(@0)", r.Data);
                    }
                    catch (Exception)
                    {
                        // null types
                        standings = standings.Where(r.Field + ".Value.ToString().Contains(@0)", r.Data);
                    }
                }
            }

            int totalRecords = standings.Count();
            var currentPage = standings
                .OrderBy(sidx + " " + sord + ", ID asc")
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

        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Add()
        {
            return View(new MatchViewModel(QuilmesModel));
        }

        [HttpPost]
        public ActionResult Add(MatchViewModel submission)
        {
            if (ModelState.IsValid)
            {
                QuilmesModel.AddMatch(submission);
                QuilmesModel.Save();

                return View(new MatchViewModel(QuilmesModel));
            }
            else
            {
                var invalid = new MatchViewModel(submission);
                return View(invalid);
            }
        }

        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var match = QuilmesModel.GetMatchByID(id);
            return View(new MatchViewModel(QuilmesModel, match));
        }

        [HttpPost]
        public ActionResult Edit(MatchViewModel submission)
        {
            var match = QuilmesModel.GetMatchByID(submission.ID);

            if (ModelState.IsValid && match != null)
            {
                QuilmesModel.UpdateMatch(match, submission);
                QuilmesModel.Save();
                return RedirectToAction("Index");
            }

            var viewModel = new MatchViewModel(QuilmesModel, match);
            return View(viewModel);
        }

        public int GetQuilmesStandingID(int seasonID)
        {
            return QuilmesModel.GetQuilmesStandingIDBySeason(seasonID).ID;
        }
    }
}