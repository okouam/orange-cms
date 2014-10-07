using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Repositories.Conventions;

namespace CodeKinden.OrangeCMS.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<DatabaseContext>(null);
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Boundary> Boundaries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            modelBuilder.Entity<User>()
                .HasKey(c => c.Id)
                .Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Boundary>()
                .HasKey(c => c.Id)
                .Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Boundary>()
                .HasMany(x => x.Customers)
                .WithMany(x => x.Boundaries)
                .Map(x =>
                {
                    x.MapLeftKey("BoundaryId");
                    x.MapRightKey("CustomerId");
                    x.ToTable("BoundaryCustomer");
                });

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id)
                .Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Customer>()
                .HasMany(x => x.Boundaries)
                .WithMany(x => x.Customers)
                .Map(x =>
                {
                    x.MapLeftKey("CustomerId");
                    x.MapRightKey("BoundaryId");
                    x.ToTable("BoundaryCustomer");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
