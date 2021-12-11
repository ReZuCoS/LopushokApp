using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace LopushokApp.Model
{
    public partial class LopushokDatabaseModel : DbContext
    {
        public LopushokDatabaseModel()
            : base("name=LopushokDatabaseModel")
        {
        }

        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MaterialType> MaterialTypes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductMaterial> ProductMaterials { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
                .Property(e => e.Cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.ProductMaterials)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MaterialType>()
                .HasMany(e => e.Materials)
                .WithRequired(e => e.MaterialType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.MinCostForAgent)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductMaterials)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);
        }
    }
}
