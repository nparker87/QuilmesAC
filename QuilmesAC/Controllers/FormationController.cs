namespace QuilmesAC.Controllers
{
    using Helpers;
    using System;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using ViewModels;

    public class FormationController : BaseController
    {
        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Index()
        {
            // Use last sorting choices if saved - otherwise use defaults.
            ViewBag.SortBy = (Session["FormationLastSortID"] ?? "ID");
            ViewBag.SortOrder = (Session["FormationLastSortOrder"] ?? "asc");
            ViewBag.Page = (Session["FormationLastSortPage"] ?? 1);
            ViewBag.Rows = (Session["FormationLastSortRows"] ?? 50);

            return View(new FormationViewModel());
        }

        /// <summary> Returns the JSON data to display a jqGrid of Formations </summary>
        /// POST: /Formation/GridData
        [HttpPost]
        public ActionResult GridData(string sidx, string sord, int page, int rows, bool _search, string filters)
        {
            // Save the last sort choices to session data.
            Session["FormationLastSortID"] = sidx;
            Session["FormationLastSortOrder"] = sord;
            Session["FormationLastSortPage"] = page;
            Session["FormationLastSortRows"] = rows;

            var allRecords = from formation in QuilmesModel.Formations
                             select formation;

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
                            t.Name
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        /// <summary> Modal add </summary>
        public ContentResult AddFormation(FormationViewModel formationViewModel)
        {
            QuilmesModel.Add(formationViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        /// <summary> Modal edit </summary>
        public ContentResult EditFormation(FormationViewModel formationViewModel)
        {
            var formation = QuilmesModel.GetFormationByID(formationViewModel.ID);
            QuilmesModel.Update(formation, formationViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        /// <summary> Modal delete </summary>
        public ContentResult DeleteFormation(int id)
        {
            var formation = QuilmesModel.GetFormationByID(id);
            QuilmesModel.Delete(formation);
            QuilmesModel.Save();

            return Content("Success");
        }
    }
}