using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data.Models;

namespace MovieCrew.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Rate> Rates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Rate>()
            .HasKey(r => new { r.MovieId, r.UserId });

        modelBuilder.Entity<Rate>()
            .HasOne(m => m.Movie)
            .WithMany(r => r.Rates)
            .HasForeignKey(r => r.MovieId);

        modelBuilder.Entity<Rate>()
            .HasOne(u => u.User)
            .WithMany(r => r.Rates)
            .HasForeignKey(r => r.UserId);
    }
}