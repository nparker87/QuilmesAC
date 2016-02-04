namespace QuilmesAC.ViewModels
{
    using Models;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    public class BaseViewModel
    {
        private readonly QuilmesDataContext _model = new QuilmesDataContext();

        public BaseViewModel()
        {
            CurrentTab = "Home";
            User = _model.GetUserByUsername(HttpContext.Current.User.Identity.Name);
            PopulateSelectLists();
        }

        public void PopulateSelectLists()
        {
            Themes = new List<SelectListItem>
            {
                new SelectListItem() {Text = "Quilmes Blue", Value = "#35428A"},
                new SelectListItem() {Text = "Home", Value = "#95031A"},
                new SelectListItem() {Text = "Away", Value = "#000000"}
            };
        }

        public string CurrentTab { get; set; }

        public string Theme { get; set; }

        public List<SelectListItem> Themes { get; set; }

        public User User { get; set; }
    }
}