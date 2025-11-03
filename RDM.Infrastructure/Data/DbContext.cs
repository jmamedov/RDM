using Microsoft.EntityFrameworkCore;
using RDM.Core.Entities;

namespace RDM.Infrastructure.Data
{
    public class RdmDbContext : DbContext
    {
        public RdmDbContext(DbContextOptions<RdmDbContext> options) : base(options)
        {
        }

        public DbSet<RegionalLocale> RegionalLocales { get; set; }
        public DbSet<ProductEntityType> ProductEntityTypes { get; set; }
        public DbSet<SourceSystem> SourceSystems { get; set; }
        public DbSet<SourceNode> SourceNodes { get; set; }
        public DbSet<NodeBillOfMaterial> NodeBillOfMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Regional Locale
            modelBuilder.Entity<RegionalLocale>(entity =>
            {
                entity.HasKey(e => e.Code);
                entity.Property(e => e.Code).HasColumnName("CD");
                entity.Property(e => e.Name).HasColumnName("NM");
                entity.Property(e => e.ContinentCode).HasColumnName("CONT_CD");
                entity.Property(e => e.Description).HasColumnName("DN");
            });

            // Product Entity Type
            modelBuilder.Entity<ProductEntityType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .UseIdentityColumn(13, 1);
            });

            // Source System (Self-referencing hierarchy)
            modelBuilder.Entity<SourceSystem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .UseIdentityColumn(10, 1);

                entity.HasOne(e => e.Parent)
                    .WithMany(e => e.Children)
                    .HasForeignKey(e => e.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Source Node
            modelBuilder.Entity<SourceNode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .UseIdentityColumn(99, 1);

                entity.Property(e => e.LocaleCode)
                    .HasDefaultValue("en-US");

                entity.HasOne(e => e.SourceSystem)
                    .WithMany(e => e.SourceNodes)
                    .HasForeignKey(e => e.SourceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.EntityType)
                    .WithMany(e => e.SourceNodes)
                    .HasForeignKey(e => e.EntityTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Locale)
                    .WithMany(e => e.SourceNodes)
                    .HasForeignKey(e => e.LocaleCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Node Bill of Material (Composite Key)
            modelBuilder.Entity<NodeBillOfMaterial>(entity =>
            {
                entity.HasKey(e => new { e.HierarchyTypeId, e.ParentNodeId, e.ChildNodeId });

                entity.HasOne(e => e.HierarchyType)
                    .WithMany(e => e.Hierarchies)
                    .HasForeignKey(e => e.HierarchyTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ParentNode)
                    .WithMany(e => e.ParentRelationships)
                    .HasForeignKey(e => e.ParentNodeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ChildNode)
                    .WithMany(e => e.ChildRelationships)
                    .HasForeignKey(e => e.ChildNodeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}