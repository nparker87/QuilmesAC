namespace QuilmesAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BrewIngredient")]
    public partial class BrewIngredient
    {
        public int ID { get; set; }

        public int BrewID { get; set; }

        public int IngredientID { get; set; }

        public int MeasurementID { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateChanged { get; set; }

        public virtual Brew Brew { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual Measurement Measurement { get; set; }
    }
}
