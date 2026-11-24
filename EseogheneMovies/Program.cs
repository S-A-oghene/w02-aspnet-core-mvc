using EseogheneMovies.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC provides the Model-View-Controller web application pattern.
builder.Services.AddControllersWithViews();

// Locally, keep the SQLite file inside the project. On Azure App Service,
// set MOVIES_DB_PATH to /home/data/eseoghene-movies.db so it survives restarts.
var configuredDatabasePath = builder.Configuration["MOVIES_DB_PATH"];
string connectionString;

if (!string.IsNullOrWhiteSpace(configuredDatabasePath))
{
    var fullDatabasePath = Path.GetFullPath(configuredDatabasePath);
    var databaseDirectory = Path.GetDirectoryName(fullDatabasePath);

    if (!string.IsNullOrWhiteSpace(databaseDirectory))
    {
        Directory.CreateDirectory(databaseDirectory);
    }

    connectionString = $"Data Source={fullDatabasePath}";
}
else
{
    var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(dataDirectory);

    // The connection string in appsettings.json is relative to the project folder.
    connectionString = builder.Configuration.GetConnectionString("MovieDb")
        ?? $"Data Source={Path.Combine(dataDirectory, "eseoghene-movies.db")}";
}

builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Create/update the SQLite database and insert starter examples on the first run.
// Database migrations are included in the Migrations folder.
await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        await DbInitializer.InitializeAsync(services);
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "A problem occurred while preparing the movie database.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");

app.Run();
