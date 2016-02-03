namespace QuilmesAC.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;

    public class PositionViewModel : BaseViewModel
    {
        public PositionViewModel()
        {
            CurrentTab = "Admin";
        }

        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("ShortName")]
        public string ShortName { get; set; }
    }
}