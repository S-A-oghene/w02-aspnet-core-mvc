using EseogheneMovies.Models;

namespace EseogheneMovies.ViewModels;

/// <summary>
/// Carries both the movie results and the values selected in the search form.
/// </summary>
public class MovieSearchViewModel
{
    public List<Movie> Movies { get; set; } = [];
    public string? SearchString { get; set; }
    public int? Year { get; set; }
    public List<int> AvailableYears { get; set; } = [];
}
