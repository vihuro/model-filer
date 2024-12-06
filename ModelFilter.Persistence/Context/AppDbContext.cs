using Microsoft.EntityFrameworkCore;
using ModelFilter.Domain.Models;

namespace ModelFilter.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(x => new { x.Name, x.UserName })
                .HasDatabaseName("IX_NAMES_ASCENDING");
        }

    }
}
