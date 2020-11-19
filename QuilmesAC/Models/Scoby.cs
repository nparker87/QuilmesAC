namespace QuilmesAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Scoby")]
    public partial class Scoby
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Scoby()
        {
            Brews = new HashSet<Brew>();
            Scoby1 = new HashSet<Scoby>();
        }

        public int ID { get; set; }

        public int? MotherID { get; set; }

        public int? BrewID { get; set; }

        public DateTime? FormationDate { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateChanged { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Brew> Brews { get; set; }

        public virtual Brew Brew { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scoby> Scoby1 { get; set; }

        public virtual Scoby Scoby2 { get; set; }
    }
}
