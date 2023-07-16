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

    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        BuildUserTable(modelBuilder);

        BuildRateTable(modelBuilder);

        BuildMovieTable(modelBuilder);

        BuildClientTable(modelBuilder);
    }

    private static void BuildMovieTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .ToTable("movies")
            .Property(m => m.Id).HasColumnName("id_movie")
            .HasConversion<int>()
            .IsRequired();
        modelBuilder.Entity<Movie>()
            .Property(m => m.Name).HasColumnName("name_movie")
            .IsRequired();
        modelBuilder.Entity<Movie>()
            .Property(m => m.Description).HasColumnName("description_movie");
        modelBuilder.Entity<Movie>()
            .Property(m => m.Poster).HasColumnName("movie_poster");
        modelBuilder.Entity<Movie>()
            .Property(m => m.DateAdded).HasColumnName("date_added_movie")
            .HasConversion<DateTime>();
        modelBuilder.Entity<Movie>()
            .Property(m => m.SeenDate).HasColumnName("seen_date_movie")
            .HasConversion<DateTime>();
        modelBuilder.Entity<Movie>()
            .Property(m => m.ProposedById).HasColumnName("proposed_by_id")
            .HasConversion<long?>();
    }

    private static void BuildRateTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rate>()
            .ToTable("rates")
            .Property(r => r.MovieId).HasColumnName("fk_movie")
            .HasConversion<int>()
            .IsRequired();
        modelBuilder.Entity<Rate>()
            .Property(r => r.UserId).HasColumnName("fk_user")
            .HasConversion<long>()
            .IsRequired();
        modelBuilder.Entity<Rate>()
            .Property(r => r.Note).HasColumnName("rate")
            .HasConversion<decimal>();
        modelBuilder.Entity<Rate>()
            .HasKey(r => new { r.MovieId, r.UserId });
    }

    private static void BuildUserTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("users")
            .Property(u => u.Id).HasColumnName("id_user")
            .HasConversion<long>()
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Name).HasColumnName("name_user")
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.Role).HasColumnName("role_user")
            .HasConversion<int>();
    }

    private static void BuildClientTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .ToTable("clients")
            .Property(c => c.ApiKey).HasColumnName("api_key");
    }
}
