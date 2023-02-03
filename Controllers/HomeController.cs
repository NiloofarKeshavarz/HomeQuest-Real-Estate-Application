using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeQuest.Models;
using HomeQuest.Data;
using static HomeQuest.Models.Property;

namespace HomeQuest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private HomeQuestDbContext db;

    public HomeController(HomeQuestDbContext db, ILogger<HomeController> logger)
    {
        this.db = db;
        _logger = logger;
    }

    [BindProperty]
    public string Title { get; set; }

    [BindProperty]
    public string Description { get; set; }
    [BindProperty]
    public string Address { get; set; }
    [BindProperty]
    public string PostalCode { get; set; }
    public int Price { get; set; }
    public int Floors { get; set; }
    public int BedroomCount { get; set; }
    public int BathroomCount { get; set; }
    public int GarageCont { get; set; }
    public DateTime YearBuilt { get; set; }
    public int FloorArea { get; set; }
    public int LotArea { get; set; }
    public DateTime CreatedAt { get; set; }


    public PropertyStatus Status { get; set; }
    public PropertyType Type { get; set; }

    public IActionResult Index()
    {
        IEnumerable<Property> propList = db.Properties.Take(3);
        return View(propList);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
