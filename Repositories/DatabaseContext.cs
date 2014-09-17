using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Codeifier.OrangeCMS.Domain;
using Codeifier.OrangeCMS.Repositories.Conventions;
using OrangeCMS.Domain;

namespace Codeifier.OrangeCMS.Repositories
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

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id)
                .Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }
    }
}
