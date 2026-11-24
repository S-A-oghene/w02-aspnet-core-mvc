using EseogheneMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace EseogheneMovies.Data;

/// <summary>
/// Applies EF Core migrations and inserts the student's selected screen titles
/// only when the database is empty. Prices are illustrative demo values only.
/// </summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<MovieDbContext>();
        await context.Database.MigrateAsync();
        if (await context.Movies.AnyAsync()) return;

        var selectedTitles = new[]
        {
            new Movie
            {
                // The first film in Peter Jackson's The Lord of the Rings trilogy.
                Title = "The Lord of the Rings: The Fellowship of the Ring",
                ReleaseDate = new DateTime(2001, 12, 19),
                Genre = "Fantasy / Adventure", Price = 9.99m, Rating = "PG-13"
            },
            new Movie
            {
                // BBC fantasy television series; retained because the student selected Merlin.
                // Its first episode aired on BBC One on 20 September 2008.
                Title = "Merlin",
                ReleaseDate = new DateTime(2008, 9, 20),
                Genre = "Fantasy Television", Price = 7.99m, Rating = "TV-PG"
            },
            new Movie
            {
                Title = "Interstellar",
                ReleaseDate = new DateTime(2014, 11, 7),
                Genre = "Science Fiction / Adventure", Price = 12.99m, Rating = "PG-13"
            }
        };
        context.Movies.AddRange(selectedTitles);
        await context.SaveChangesAsync();
    }
}
