using Accounting.Core.Entity.AccountingManagement;
using Microsoft.EntityFrameworkCore;

namespace Accounts.DataAcess.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> contextOptionsBuilder)
                : base(contextOptionsBuilder)
        { }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(c => c.UserName)
                        .IsRequired();

                entity.Property(c => c.Domain)
                        .IsRequired();

                entity.Property(c => c.Password)
                        .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}