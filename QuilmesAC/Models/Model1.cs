namespace QuilmesAC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Bottle> Bottles { get; set; }
        public virtual DbSet<BottleIngredient> BottleIngredients { get; set; }
        public virtual DbSet<Brew> Brews { get; set; }
        public virtual DbSet<BrewIngredient> BrewIngredients { get; set; }
        public virtual DbSet<BrewType> BrewTypes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Measurement> Measurements { get; set; }
        public virtual DbSet<Scoby> Scobies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bottle>()
                .HasMany(e => e.BottleIngredients)
                .WithRequired(e => e.Bottle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BottleIngredient>()
                .Property(e => e.Amount)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Brew>()
                .HasMany(e => e.Bottles)
                .WithRequired(e => e.Brew)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Brew>()
                .HasMany(e => e.Brew1)
                .WithOptional(e => e.Brew2)
                .HasForeignKey(e => e.StarterLiquidID);

            modelBuilder.Entity<Brew>()
                .HasMany(e => e.BrewIngredients)
                .WithRequired(e => e.Brew)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Brew>()
                .HasMany(e => e.Scobies)
                .WithOptional(e => e.Brew)
                .HasForeignKey(e => e.BrewID);

            modelBuilder.Entity<BrewIngredient>()
                .Property(e => e.Amount)
                .HasPrecision(6, 2);

            modelBuilder.Entity<BrewType>()
                .HasMany(e => e.Brews)
                .WithRequired(e => e.BrewType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ingredient>()
                .HasMany(e => e.BottleIngredients)
                .WithRequired(e => e.Ingredient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ingredient>()
                .HasMany(e => e.BrewIngredients)
                .WithRequired(e => e.Ingredient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Measurement>()
                .HasMany(e => e.BottleIngredients)
                .WithRequired(e => e.Measurement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Measurement>()
                .HasMany(e => e.BrewIngredients)
                .WithRequired(e => e.Measurement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Scoby>()
                .HasMany(e => e.Brews)
                .WithOptional(e => e.Scoby)
                .HasForeignKey(e => e.ScobyID);

            modelBuilder.Entity<Scoby>()
                .HasMany(e => e.Scoby1)
                .WithOptional(e => e.Scoby2)
                .HasForeignKey(e => e.MotherID);
        }
    }
}
