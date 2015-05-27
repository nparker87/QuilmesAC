namespace QuilmesAC.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Models;

    public class MatchViewModel : BaseViewModel
    {
        public MatchViewModel()
        {
            CurrentTab = "Match";
        }

        public MatchViewModel(QuilmesDataContext model)
        {
            CurrentTab = "Match";
            PopulateSelectLists(model);
            AllTimeRecord = model.GetAllTimeRecord();
        }

        public MatchViewModel(MatchViewModel match)
        {
            var viewModel = new MatchViewModel
            {
                MatchDay = match.MatchDay,
                MatchDate = match.MatchDate,
                OpponentID = match.OpponentID,
                GoalsFor = match.GoalsFor,
                GoalsAgainst = match.GoalsAgainst,
                Result = match.Result,
                SeasonID = match.SeasonID
            };
        }

        public MatchViewModel(QuilmesDataContext model, Match match)
        {
            CurrentTab = "Match";
            PopulateSelectLists(model);
            Add(match);
            AllTimeRecord = model.GetAllTimeRecord();
        }

        [DisplayName("ID")]
        public long ID { get; set; }

        [DisplayName("Match day:")]
        public int? MatchDay { get; set; }

        [DisplayName("Match date:")]
        public DateTime? MatchDate { get; set; }

        [DisplayName("Opponent:")]
        [Required(ErrorMessage = "Opponent Required")]
        public long OpponentID { get; set; }

        public SelectList Opponents { get; set; }

        [DisplayName("Goals for:")]
        public int? GoalsFor { get; set; }

        [DisplayName("Goals against:")]
        public int? GoalsAgainst { get; set; }

        [DisplayName("Result:")]
        [Required(ErrorMessage = "Result Required")]
        public char? Result { get; set; }

        public List<SelectListItem> Results { get; set; }

        [DisplayName("Season:")]
        [Required(ErrorMessage = "Season Required")]
        public long SeasonID { get; set; }

        public SelectList Seasons { get; set; }

        public AllTimeRecord AllTimeRecord { get; set; }

        private void PopulateSelectLists(QuilmesDataContext model)
        {
            Opponents = new SelectList(model.GetOpponents(), "ID", "Name");
            Results = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "", Value = ""},
                new SelectListItem() {Text = "W", Value = "W"},
                new SelectListItem() {Text = "L", Value = "L"},
                new SelectListItem() {Text = "D", Value = "D"},
            };
            Seasons = new SelectList(model.GetSeasons(), "ID", "DisplayName");
        }

        private void Add(Match match)
        {
            ID = match.ID;
            MatchDay = match.MatchDay;
            MatchDate = match.MatchDate;
            OpponentID = match.OpponentID;
            GoalsFor = match.GoalsFor;
            GoalsAgainst = match.GoalsAgainst;
            Result = match.Result;
            SeasonID = match.SeasonID;
        }
    }
}