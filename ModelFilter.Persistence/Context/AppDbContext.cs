using Microsoft.EntityFrameworkCore;
using ModelFilter.Domain.Models;

namespace ModelFilter.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<UserModel> Users { get; set; }

    }
}
