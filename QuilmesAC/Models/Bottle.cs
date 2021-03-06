namespace QuilmesAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bottle")]
    public partial class Bottle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bottle()
        {
            BottleIngredients = new HashSet<BottleIngredient>();
        }

        public int ID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int BrewID { get; set; }

        public DateTime BottleDate { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateChanged { get; set; }

        public virtual Brew Brew { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BottleIngredient> BottleIngredients { get; set; }
    }
}
