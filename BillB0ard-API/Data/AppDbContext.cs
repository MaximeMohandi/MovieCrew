using BillB0ard_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
