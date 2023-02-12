using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeQuest.Models;
using HomeQuest.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static HomeQuest.Models.Property;

namespace HomeQuest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private HomeQuestDbContext db;

    private readonly UserManager<ApplicationUser> userManager;

    public HomeController(HomeQuestDbContext db, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
    {
        this.db = db;
        _logger = logger;
        this.userManager = userManager;
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
        IEnumerable<Property> propList = db.Properties.Include(p => p.Images).ToList().Take(3);
        // var currentUserId = userManager.GetUserId(User);
        // var favoritePropertyList = db.Favorites.Where(f => f.UserId == currentUserId).ToList(); 
        // ViewBag.favoritePropertyList = favoritePropertyList;
        return View(propList);
    }

    // [HttpPost]
    // public IActionResult AddFavoriteProperty(int favoritePropertyId, string favoriteButton){
    //     var currentUserId = userManager.GetUserId(User);
    //     if(favoriteButton == "Add To Favorite"){
    //         var propertyId = favoritePropertyId;
            

    //         Favorite favorite = new Favorite();
    //         favorite.PropertyId = propertyId;
    //         favorite.UserId = currentUserId;
    //         db.Favorites.Add(favorite);
    //     }
    //     if(favoriteButton == "Remove From Favorite"){
    //         var property = db.Properties.Include(x => x.Favorites).FirstOrDefault(x => x.Id == favoritePropertyId);
    //         var favorite = property.Favorites.FirstOrDefault(x => x.UserId == currentUserId );
    //         if(favorite != null){
    //             property.Favorites.Remove(favorite);
    //         }
    //     }
        

    //     db.SaveChanges();

    //     return RedirectToAction("Index");
    // }

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
