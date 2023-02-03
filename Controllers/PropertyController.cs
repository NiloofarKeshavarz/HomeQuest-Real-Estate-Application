using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeQuest.Data;
using HomeQuest.Models;
using static HomeQuest.Models.Property;

namespace HomeQuest.Controllers
{
    // [Route("[controller]")]
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> _logger;
        private HomeQuestDbContext db;

        public PropertyController(HomeQuestDbContext db, ILogger<PropertyController> logger)
        {
            _logger = logger;
            this.db = db;
        }
        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string PostalCode { get; set; }
        public int Price { get; set;}
        public int Floors { get; set; }
        public int BedroomCount { get; set; }
        public int BathroomCount { get; set; }
        public int GarageCont { get; set; }
        public DateTime YearBuilt { get; set; }
        public int FloorArea { get; set; }
        public int LotArea { get; set; }
        public DateTime CreatedAt { get; set; }

        
        public PropertyStatus Status{get; set;}
        public PropertyType Type{get; set;}

        public IActionResult Index()
        {
            IEnumerable<Property> propList = db.Properties;
              return View(propList);
        }



        public IActionResult Create()
        {
             if (ModelState.IsValid)
            {
                var newProperty = new HomeQuest.Models.Property{Title = Title, Description = Description, Address = Address, PostalCode = PostalCode};
                db.Add(newProperty);
                db.SaveChangesAsync();
              
         }
            return View();
       }

        public IActionResult Detail(int Id)
        {   
            Property property = db.Properties.Where(x => x.Id == Id).FirstOrDefault();
            return View(property);
        }


        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}


