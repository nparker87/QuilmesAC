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
            ViewBag.SortBy = (Session["MatchLastSortID"] ?? "MatchDate");
            ViewBag.SortOrder = (Session["MatchLastSortOrder"] ?? "desc");
            ViewBag.Page = (Session["MatchLastSortPage"] ?? 1);
            ViewBag.Rows = (Session["MatchLastSortRows"] ?? 50);

            return View(new MatchViewModel(QuilmesModel));
        }

        /// <summary> Returns the JSON data to display a jqGrid of beers </summary>
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

            if (!String.IsNullOrWhiteSpace(seasonID))
                matches = matches.Where(x => x.SeasonID == Int32.Parse(seasonID));

            
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
                page = page, // current page
                records = totalRecords, // total number of records of all pages
                rows = ( //actual data records for current page
                    from t in currentPage
                    select new
                    {
                        id = t.ID,
                        cell = new string[]
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

        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Add()
        {
            return View(new MatchViewModel(QuilmesModel));
        }

        [HttpPost]
        public ActionResult Add(MatchViewModel submission)
        {
            if (!ModelState.IsValid) return View(submission);
            QuilmesModel.AddMatch(submission);
            QuilmesModel.Save();

            return View(new MatchViewModel(QuilmesModel));
        }
    }
}