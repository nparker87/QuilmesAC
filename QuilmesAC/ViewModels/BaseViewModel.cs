namespace QuilmesAC.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BaseViewModel
    {
        public BaseViewModel()
        {
            CurrentTab = "Home";
            PopulateSelectLists();
        }

        public BaseViewModel(string currentTab)
        {
            CurrentTab = currentTab;
            PopulateSelectLists();
        }

        public void PopulateSelectLists()
        {
            Themes = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "Quilmes Blue", Value = "#35428A"},
                new SelectListItem() {Text = "Home", Value = "#95031A"},
                new SelectListItem() {Text = "Away", Value = "#000000"}
            };
        }

        public string CurrentTab { get; set; }

        public string Theme { get; set; }

        public List<SelectListItem> Themes { get; set; }

        /// <summary>Returns a CSS class for whether the given tab name equals the current tab.</summary>
        /// <param name="tabName">Name of the tab to check</param>
        /// <returns>Active if the tab is the current tab, empty string if not.</returns>
        public string TabClass(string tabName)
        {
            return (CurrentTab == tabName
                ? "active"
                : "");
        }
    }
}