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
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
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
                .MapToStoredProcedures(sp => {
                    sp.Update(x => x.HasName("UpdateBoundary"));
                    sp.Insert(x => x.HasName("InsertBoundary"));
                    sp.Delete(x => x.HasName("DeleteBoundary"));
                })
                .HasKey(c => c.Id)
                .Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            modelBuilder.Entity<Boundary>()
                .HasMany(x => x.Customers)
                .WithMany(x => x.Boundaries)
                .Map(x => {
                    x.MapLeftKey("CustomerId");
                    x.MapRightKey("BoundaryId");
                    x.ToTable("BoundaryCustomer");
                })
                .MapToStoredProcedures(sp => {
                    sp.Insert(x => x.HasName("DoNothingCustomerBoundary"));
                    sp.Insert(x => x.HasName("DoNothingCustomerBoundary"));
                    sp.Insert(x => x.HasName("DoNothingCustomerBoundary"));
                });

            modelBuilder.Entity<Customer>()
                .MapToStoredProcedures(sp =>
                {
                    sp.Update(x => x.HasName("UpdateCustomer"));
                    sp.Insert(x => x.HasName("InsertCustomer"));
                    sp.Delete(x => x.HasName("DeleteCustomer"));
                })
                .HasKey(c => c.Id)
                .Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Customer>()
                .HasMany(x => x.Boundaries)
                .WithMany(x => x.Customers)
                .Map(x =>
                {
                    x.MapLeftKey("CustomerId");
                    x.MapRightKey("BoundaryId");
                    x.ToTable("BoundaryCustomer");
                })
                .MapToStoredProcedures(sp =>
                {
                    sp.Insert(x => x.HasName("DoNothingCustomerBoundary"));
                    sp.Insert(x => x.HasName("DoNothingCustomerBoundary"));
                    sp.Insert(x => x.HasName("DoNothingCustomerBoundary"));
                });


            base.OnModelCreating(modelBuilder);
        }
    }
}
