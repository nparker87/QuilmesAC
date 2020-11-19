namespace QuilmesAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Brew")]
    public partial class Brew
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Brew()
        {
            Bottles = new HashSet<Bottle>();
            Brew1 = new HashSet<Brew>();
            BrewIngredients = new HashSet<BrewIngredient>();
            Scobies = new HashSet<Scoby>();
        }

        public int ID { get; set; }

        public int BrewTypeID { get; set; }

        public DateTime BrewDate { get; set; }

        public int? ScobyID { get; set; }

        public int? StarterLiquidID { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateChanged { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bottle> Bottles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Brew> Brew1 { get; set; }

        public virtual Brew Brew2 { get; set; }

        public virtual BrewType BrewType { get; set; }

        public virtual Scoby Scoby { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BrewIngredient> BrewIngredients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scoby> Scobies { get; set; }
    }
}
