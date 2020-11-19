namespace QuilmesAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BottleIngredient")]
    public partial class BottleIngredient
    {
        public int ID { get; set; }

        public int BottleID { get; set; }

        public int IngredientID { get; set; }

        public int MeasurementID { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateChanged { get; set; }

        public virtual Bottle Bottle { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual Measurement Measurement { get; set; }
    }
}
