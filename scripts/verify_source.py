from pathlib import Path
import sys
ROOT = Path(__file__).resolve().parents[1]
PROJECT = ROOT / "EseogheneMovies"
checks = {
 "project targets .NET 10": "net10.0" in (PROJECT / "EseogheneMovies.csproj").read_text(),
 "student application title": "Eseoghene Movies" in (PROJECT / "Views/Shared/_Layout.cshtml").read_text(),
 "personalised listing heading": "My Favorite Movies" in (PROJECT / "Views/Movies/Index.cshtml").read_text(),
 "Lord of the Rings seed": "The Lord of the Rings: The Fellowship of the Ring" in (PROJECT / "Data/DbInitializer.cs").read_text(),
 "Merlin seed": 'Title = "Merlin"' in (PROJECT / "Data/DbInitializer.cs").read_text(),
 "Interstellar seed": 'Title = "Interstellar"' in (PROJECT / "Data/DbInitializer.cs").read_text(),
 "year-or-newer filter": "ReleaseDate >= firstDayOfSelectedYear" in (PROJECT / "Controllers/MoviesController.cs").read_text(),
 "SQLite provider": "UseSqlite" in (PROJECT / "Program.cs").read_text(),
 "global form control padding": "padding: 11px 13px;" in (PROJECT / "wwwroot/css/site.css").read_text(),
 "custom palette documented": "The Vellum Screening Room" in (ROOT / "docs/PERSONALISATION_AND_DATA_NOTES.md").read_text(),
 "decimal limits parsed with invariant culture": "ParseLimitsInInvariantCulture = true" in (PROJECT / "Models/Movie.cs").read_text(),
 "decimal values converted with invariant culture": "ConvertValueInInvariantCulture = true" in (PROJECT / "Models/Movie.cs").read_text(),
 "price validation bounds retained": '"0.01"' in (PROJECT / "Models/Movie.cs").read_text() and '"999.99"' in (PROJECT / "Models/Movie.cs").read_text(),
 "decimal culture troubleshooting guide exists": (ROOT / "docs/TROUBLESHOOTING_DECIMAL_CULTURE.md").exists(),
 "Merlin TV rating accepted by model": "TV-PG" in (PROJECT / "Models/Movie.cs").read_text(),
 "Merlin TV rating selectable in form": 'value="TV-PG"' in (PROJECT / "Views/Movies/_MovieFields.cshtml").read_text(),
 "GitHub guide exists": (ROOT / "docs/GITHUB_UPLOAD.md").exists(),
 "deployment guide exists": (ROOT / "deployment/AZURE_APP_SERVICE.md").exists(),
 "tutorial mapping exists": (ROOT / "docs/TUTORIAL_STEP_MAP.md").exists(),
}
for label, ok in checks.items(): print(f"[{'PASS' if ok else 'FAIL'}] {label}")
failed = [label for label, ok in checks.items() if not ok]
print(f"\n{len(checks)-len(failed)}/{len(checks)} static checks passed.")
if failed: sys.exit(1)
print("NOTE: Static checks do not compile or run the ASP.NET application. Run dotnet restore/build and browser tests locally.")
