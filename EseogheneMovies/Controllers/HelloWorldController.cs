using Microsoft.AspNetCore.Mvc;

namespace EseogheneMovies.Controllers;

/// <summary>
/// Small controller retained from the Microsoft tutorial to demonstrate
/// controller actions, route parameters, query strings, and Razor views.
/// </summary>
public class HelloWorldController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Welcome(string? name, int numTimes = 1)
    {
        var safeName = string.IsNullOrWhiteSpace(name) ? "friend" : name.Trim();
        ViewData["Message"] = $"Hello, {safeName}!";
        ViewData["NumTimes"] = Math.Clamp(numTimes, 1, 10);
        return View();
    }
}
