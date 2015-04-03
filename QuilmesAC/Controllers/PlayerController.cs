namespace QuilmesAC.Controllers
{
    using System;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Helpers;
    using ViewModels;

    public class PlayerController : BaseController
    {
        public ActionResult Index()
        {
            // Use last sorting choices if saved - otherwise use defaults.
            ViewBag.SortBy = (Session["PlayerLastSortID"] ?? "CreatedDate");
            ViewBag.SortOrder = (Session["PlayerLastSortOrder"] ?? "desc");
            ViewBag.Page = (Session["PlayerLastSortPage"] ?? 1);
            ViewBag.Rows = (Session["PlayerLastSortRows"] ?? 50);

            return View(new PlayerViewModel(QuilmesModel));
        }

        /// <summary> Returns the JSON data to display a jqGrid of players </summary>
        /// POST: /Match/GridData
        [HttpPost]
        public ActionResult GridData(string sidx, string sord, int page, int rows, bool _search, string filters, string statusID, string seasonID)
        {
            // Save the last sort choices to session data.
            Session["PlayerLastSortID"] = sidx;
            Session["PlayerLastSortOrder"] = sord;
            Session["PlayerLastSortPage"] = page;
            Session["PlayerLastSortRows"] = rows;

            var players = from player in QuilmesModel.Players
                          select player;

            if (!String.IsNullOrWhiteSpace(statusID))
                players = players.Where(x => x.StatusID == Int32.Parse(statusID));

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
                        players = players.Where(r.Field + ".ToString().Contains(@0)", r.Data);
                    }
                    catch (Exception)
                    {
                        // null types
                        players = players.Where(r.Field + ".Value.ToString().Contains(@0)", r.Data);
                    }
                }
            }

            int totalRecords = players.Count();
            var currentPage = players
                .OrderBy(sidx + " " + sord + ", CreatedDate, ID asc")
                .Skip((Convert.ToInt32(page) - 1)*rows)
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
                            t.FirstName,
                            t.LastName,
                            t.Number.ToString(),
                            (String.IsNullOrEmpty(seasonID) 
                                ? t.Goals.Count.ToString()
                                : t.Goals.Count(x => x.Match.SeasonID == Int32.Parse(seasonID)).ToString()),
                            (String.IsNullOrEmpty(seasonID) 
                                ? t.Assists.Count.ToString()
                                : t.Assists.Count(x => x.Match.SeasonID == Int32.Parse(seasonID)).ToString()),
                            (String.IsNullOrEmpty(seasonID) 
                                ? t.Cards.Count(x => x.CardType.Name == "Yellow").ToString() 
                                : t.Cards.Count(x => x.CardType.Name == "Yellow" &&  x.Match.SeasonID == Int32.Parse(seasonID)).ToString()),
                            (String.IsNullOrEmpty(seasonID) 
                                ? t.Cards.Count(x => x.CardType.Name == "Red").ToString() 
                                : t.Cards.Count(x => x.CardType.Name == "Red" &&  x.Match.SeasonID == Int32.Parse(seasonID)).ToString()),
                            t.Status.Name
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }
        
        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Add()
        {
            return View(new PlayerViewModel(QuilmesModel));
        }

        [HttpPost]
        public ActionResult Add(PlayerViewModel submission)
        {
            if (!ModelState.IsValid) return View(submission);
            QuilmesModel.AddPlayer(submission);
            QuilmesModel.Save();
            return View(new PlayerViewModel(QuilmesModel));
        }

        [AuthorizeHelper(Roles = "Admin")]
        public ActionResult Edit(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            // Use last sorting choices if saved - otherwise use defaults.
            ViewBag.GoalSortBy = (Session["GoalLastSortID"] ?? "ID");
            ViewBag.GoalSortOrder = (Session["GoalLastSortOrder"] ?? "desc");
            ViewBag.GoalPage = (Session["GoalLastSortPage"] ?? 1);
            ViewBag.GoalRows = (Session["GoalLastSortRows"] ?? 50);
            ViewBag.AssistSortBy = (Session["AssistLastSortID"] ?? "ID");
            ViewBag.AssistSortOrder = (Session["AssistLastSortOrder"] ?? "desc");
            ViewBag.AssistPage = (Session["AssistLastSortPage"] ?? 1);
            ViewBag.AssistRows = (Session["AssistLastSortRows"] ?? 50);
            ViewBag.CardSortBy = (Session["CardLastSortID"] ?? "ID");
            ViewBag.CardSortOrder = (Session["CardLastSortOrder"] ?? "desc");
            ViewBag.CardPage = (Session["CardLastSortPage"] ?? 1);
            ViewBag.CardRows = (Session["CardLastSortRows"] ?? 50);

            var player = QuilmesModel.GetPlayerByID(id);
            var viewModel = new PlayerViewModel(QuilmesModel, player);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(PlayerViewModel submission)
        {
            var player = QuilmesModel.GetPlayerByID(submission.ID);

            if (ModelState.IsValid && player != null)
            {
                QuilmesModel.UpdatePlayer(player, submission);
                QuilmesModel.Save();
                return RedirectToAction("Index");
            }

            var viewModel = new PlayerViewModel(QuilmesModel, player);
            return View(viewModel);

        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            var player = QuilmesModel.GetPlayerByID(id);
            QuilmesModel.Delete(player);
            QuilmesModel.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GoalGridData(string sidx, string sord, int page, int rows, bool _search, string filters, int playerID)
        {
            // Save the last sort choices to session data.
            Session["GoalLastSortID"] = sidx;
            Session["GoalLastSortOrder"] = sord;
            Session["GoalLastSortPage"] = page;
            Session["GoalLastSortRows"] = rows;

            var allRecords = from goal in QuilmesModel.Goals
                             where goal.PlayerID == playerID
                             select goal;

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
                .OrderBy(sidx + " " + sord + ", ID asc")
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
                            t.PlayerID.ToString(),
                            t.MatchID.ToString(),
                            t.Match.Opponent.Name,
                            (t.Match.MatchDate == null ? "" : t.Match.MatchDate.Value.ToShortDateString())
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult AssistGridData(string sidx, string sord, int page, int rows, bool _search, string filters, int playerID)
        {
            // Save the last sort choices to session data.
            Session["AssistLastSortID"] = sidx;
            Session["AssistLastSortOrder"] = sord;
            Session["AssistLastSortPage"] = page;
            Session["AssistLastSortRows"] = rows;

            var allRecords = from assist in QuilmesModel.Assists
                             where assist.PlayerID == playerID
                             select assist;

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
                .OrderBy(sidx + " " + sord + ", ID asc")
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
                            t.PlayerID.ToString(),
                            t.MatchID.ToString(),
                            t.Match.Opponent.Name,
                            (t.Match.MatchDate == null ? "" : t.Match.MatchDate.Value.ToShortDateString())
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult CardGridData(string sidx, string sord, int page, int rows, bool _search, string filters, int playerID)
        {
            // Save the last sort choices to session data.
            Session["CardLastSortID"] = sidx;
            Session["CardLastSortOrder"] = sord;
            Session["CardLastSortPage"] = page;
            Session["CardLastSortRows"] = rows;

            var allRecords = from card in QuilmesModel.Cards
                             where card.PlayerID == playerID
                             select card;

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
                .OrderBy(sidx + " " + sord + ", ID asc")
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
                            t.PlayerID.ToString(),
                            t.MatchID.ToString(),
                            t.Match.Opponent.Name,
                            (t.Match.MatchDate == null ? "" : t.Match.MatchDate.Value.ToShortDateString()),
                            t.CardType.Name
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }

        public ContentResult AddGoal(GoalViewModel goalViewModel)
        {
            QuilmesModel.Add(goalViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult EditGoal(GoalViewModel goalViewModel)
        {
            var goal = QuilmesModel.GetGoalByID(goalViewModel.ID);
            QuilmesModel.Update(goal, goalViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult DeleteGoal(long id)
        {
            var goal = QuilmesModel.GetGoalByID(id);
            QuilmesModel.Delete(goal);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult AddAssist(AssistViewModel assistViewModel)
        {
            QuilmesModel.Add(assistViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult EditAssist(AssistViewModel assistViewModel)
        {
            var assist = QuilmesModel.GetAssitByID(assistViewModel.ID);
            QuilmesModel.Update(assist, assistViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult DeleteAssist(Int64 id)
        {
            var assist = QuilmesModel.GetAssitByID(id);
            QuilmesModel.Delete(assist);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult AddCard(CardViewModel cardViewModel)
        {
            QuilmesModel.Add(cardViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult EditCard(CardViewModel cardViewModel)
        {
            var card = QuilmesModel.GetCardByID(cardViewModel.ID);
            QuilmesModel.Update(card, cardViewModel);
            QuilmesModel.Save();

            return Content("Success");
        }

        public ContentResult DeleteCard(Int64 id)
        {
            var card = QuilmesModel.GetCardByID(id);
            QuilmesModel.Delete(card);
            QuilmesModel.Save();

            return Content("Success");
        }

        public string GetMatches()
        {
            var result = new StringBuilder();
            result.Append("<select>");

            foreach (var match in QuilmesModel.Matches)
                result.AppendFormat("<option value=\"{0}\">{1} - {2} - {3}</option>",
                    match.ID,
                    match.Season.DisplayName,
                    match.Opponent.Name,
                    (match.MatchDate == null ? "" : match.MatchDate.Value.ToShortDateString()));

            result.Append("</select>");
            return result.ToString();
        }

        public string GetCards()
        {
            var result = new StringBuilder();
            result.Append("<select>");

            foreach (var cardType in QuilmesModel.CardTypes)
                result.AppendFormat("<option value=\"{0}\">{1}</option>",
                    cardType.ID,
                    cardType.Name);

            result.Append("</select>");
            return result.ToString();
        }
    }
}