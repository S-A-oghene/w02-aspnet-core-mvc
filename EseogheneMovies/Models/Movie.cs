using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EseogheneMovies.Models;

/// <summary>
/// Represents one movie stored in the application's SQLite database.
/// </summary>
public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter the movie title.")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "The title must be between 2 and 60 characters.")]
    [Display(Name = "Movie title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the release date.")]
    [DataType(DataType.Date)]
    [Display(Name = "Release date")]
    public DateTime ReleaseDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Please enter a genre.")]
    [StringLength(30, ErrorMessage = "The genre cannot be longer than 30 characters.")]
    [Display(Name = "Genre")]
    public string Genre { get; set; } = string.Empty;

    [Required]
    // The limits are written with a period because they are culture-neutral values.
    // These flags tell .NET to parse both the limits and submitted values using
    // invariant culture, preventing a crash on machines whose decimal separator
    // is a comma (for example, some regional Windows settings).
    [Range(
        typeof(decimal),
        "0.01",
        "999.99",
        ParseLimitsInInvariantCulture = true,
        ConvertValueInInvariantCulture = true,
        ErrorMessage = "Enter a price between 0.01 and 999.99.")]
    [Column(TypeName = "TEXT")]
    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Please select a movie rating.")]
    [StringLength(5)]
    [RegularExpression("^(G|PG|PG-13|R|NC-17|NR|TV-PG)$", ErrorMessage = "Choose G, PG, PG-13, R, NC-17, NR, or TV-PG.")]
    [Display(Name = "Rating")]
    public string Rating { get; set; } = "NR";
}
