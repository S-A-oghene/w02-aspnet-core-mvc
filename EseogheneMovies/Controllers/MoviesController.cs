using EseogheneMovies.Data;
using EseogheneMovies.Models;
using EseogheneMovies.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EseogheneMovies.Controllers;

public class MoviesController(MovieDbContext context) : Controller
{
    // GET: /Movies or /Movies?searchString=inception&year=2010
    public async Task<IActionResult> Index(string? searchString, int? year)
    {
        IQueryable<Movie> movieQuery = context.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            var searchTerm = searchString.Trim();
            var pattern = $"%{searchTerm}%";
            movieQuery = movieQuery.Where(movie =>
                EF.Functions.Like(movie.Title, pattern) ||
                EF.Functions.Like(movie.Genre, pattern));
        }

        // The assignment asks for movies from the selected year OR newer.
        if (year.HasValue && year.Value >= 1800 && year.Value <= 2100)
        {
            var firstDayOfSelectedYear = new DateTime(year.Value, 1, 1);
            movieQuery = movieQuery.Where(movie => movie.ReleaseDate >= firstDayOfSelectedYear);
        }
        else if (year.HasValue)
        {
            year = null;
        }

        var movies = await movieQuery
            .OrderByDescending(movie => movie.ReleaseDate)
            .ThenBy(movie => movie.Title)
            .ToListAsync();

        var availableYears = await context.Movies
            .AsNoTracking()
            .Select(movie => movie.ReleaseDate.Year)
            .Distinct()
            .OrderByDescending(movieYear => movieYear)
            .ToListAsync();

        var viewModel = new MovieSearchViewModel
        {
            Movies = movies,
            SearchString = searchString,
            Year = year,
            AvailableYears = availableYears
        };

        return View(viewModel);
    }

    // GET: /Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var movie = await context.Movies.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id.Value);

        return movie is null ? NotFound() : View(movie);
    }

    // GET: /Movies/Create
    public IActionResult Create()
    {
        return View(new Movie
        {
            ReleaseDate = DateTime.Today,
            Price = 9.99m,
            Rating = "PG-13"
        });
    }

    // POST: /Movies/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        context.Movies.Add(movie);
        await context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"{movie.Title} was added to your collection.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var movie = await context.Movies.FindAsync(id.Value);
        return movie is null ? NotFound() : View(movie);
    }

    // POST: /Movies/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        try
        {
            context.Update(movie);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var stillExists = await context.Movies.AnyAsync(item => item.Id == movie.Id);
            if (!stillExists)
            {
                return NotFound();
            }

            throw;
        }

        TempData["SuccessMessage"] = $"{movie.Title} was updated.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Movies/Delete/5. This only displays the confirmation page.
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var movie = await context.Movies.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id.Value);

        return movie is null ? NotFound() : View(movie);
    }

    // POST: /Movies/Delete/5. The actual deletion happens only after confirmation.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await context.Movies.FindAsync(id);
        if (movie is not null)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"{movie.Title} was removed from your collection.";
        }

        return RedirectToAction(nameof(Index));
    }
}
