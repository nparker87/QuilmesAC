namespace QuilmesAC.Controllers
{
    using Helpers;
    using System;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using ViewModels;

    public class SeasonController : BaseController
    {
        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Index()
        {
            // Use last sorting choices if saved - otherwise use defaults.
            ViewBag.SortBy = (Session["SeasonLastSortID"] ?? "ID");
            ViewBag.SortOrder = (Session["SeasonLastSortOrder"] ?? "asc");
            ViewBag.Page = (Session["SeasonLastSortPage"] ?? 1);
            ViewBag.Rows = (Session["SeasonLastSortRows"] ?? 50);

            return View(new SeasonViewModel());
        }

        /// <summary> Returns the JSON data to display a jqGrid of seasons </summary>
        /// POST: /Season/GridData
        [HttpPost]
        public ActionResult GridData(string sidx, string sord, int page, int rows, bool _search, string filters)
        {
            // Save the last sort choices to session data.
            Session["SeasonLastSortID"] = sidx;
            Session["SeasonLastSortOrder"] = sord;
            Session["SeasonLastSortPage"] = page;
            Session["SeasonLastSortRows"] = rows;

            var allRecords = from season in QuilmesModel.Seasons
                             select season;

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

            var totalRecords = allRecords.Count();
            var currentPage = allRecords
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
                            t.DisplayName,
                            String.Format("{0:M/d/yyyy}", t.StartDate),
                            t.Division.Name,
                            t.IsCurrent.ToString()
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        /// <summary> Modal add </summary>
        public ContentResult AddSeason(SeasonViewModel seasonViewModel)
        {
            if (seasonViewModel.IsCurrent)
                foreach (var s in QuilmesModel.Seasons)
                    s.IsCurrent = false;

            QuilmesModel.Add(seasonViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        /// <summary> Modal edit </summary>
        public ContentResult EditSeason(SeasonViewModel seasonViewModel)
        {
            if (seasonViewModel.IsCurrent)
                foreach (var s in QuilmesModel.Seasons)
                    s.IsCurrent = false;

            var season = QuilmesModel.GetSeasonByID(seasonViewModel.ID);
            QuilmesModel.Update(season, seasonViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        /// <summary> Modal delete </summary>
        public ContentResult DeleteSeason(int id)
        {
            var season = QuilmesModel.GetSeasonByID(id);
            QuilmesModel.Delete(season);
            QuilmesModel.Save();

            return Content("Success");
        }

        public string GetDivisions()
        {
            var result = "<select><option value=''>-- Select --</option>";
            foreach (var division in QuilmesModel.Divisions.OrderBy(x => x.Ranking))
            {
                result += "<option value='" + division.ID + "'>" + division.Name + "</option>";
            }
            result += "</select>";
            return result;
        }
    }
}