﻿namespace QuilmesAC.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Models;

    public class SeasonViewModel : BaseViewModel
    {
        public SeasonViewModel()
        {
            CurrentTab = "Admin";
        }

        public SeasonViewModel(QuilmesDataContext model)
        {
            CurrentTab = "Admin";
            PopulateSelectLists(model);
        }

        [DisplayName("ID")]
        public long ID { get; set; }

        [DisplayName("Start date:")]
        [Required(ErrorMessage = "Required")]
        public DateTime StartDate { get; set; }

        [DisplayName("Display name:")]
        [Required(ErrorMessage = "Required")]
        public string DisplayName { get; set; }

        [DisplayName("Division:")]
        [Required(ErrorMessage = "Required")]
        public long DivisionID { get; set; }

        public SelectList Divisions { get; set; }

        private void PopulateSelectLists(QuilmesDataContext model)
        {
            Divisions = new SelectList(model.GetDivisions(), "ID", "Name");
        }
    }
}