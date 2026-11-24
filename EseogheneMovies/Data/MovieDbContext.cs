using EseogheneMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace EseogheneMovies.Data;

/// <summary>
/// EF Core database session. The Movies property represents the Movies table.
/// </summary>
public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movies");
            entity.HasKey(movie => movie.Id);
            entity.Property(movie => movie.Title).IsRequired().HasMaxLength(60);
            entity.Property(movie => movie.Genre).IsRequired().HasMaxLength(30);
            entity.Property(movie => movie.Rating).IsRequired().HasMaxLength(5);
            // SQLite stores decimal values as text to preserve their precision.
            entity.Property(movie => movie.Price).HasColumnType("TEXT");
        });
    }
}
