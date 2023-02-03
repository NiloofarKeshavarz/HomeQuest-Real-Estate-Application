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
    [Route("[controller]")]
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> logger;
        private HomeQuestDbContext db;

        public PropertyController(HomeQuestDbContext db, ILogger<PropertyController> logger)
        {
            this.logger = logger;
            this.db = db;
        }
        
        [BindProperty]
        public Property Property { get; set; }


        [Route("/Index")]
        public IActionResult Index()
        {
            IEnumerable<Property> propList = db.Properties;
            return View(propList);
        }

        // Redirect to add property page 
        [Route("/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("/CreateNewProperty")]
        public IActionResult CreateNewProperty()
        {
          
                db.Properties.Add(Property);
                db.SaveChangesAsync();
                Console.WriteLine("insertion DONE!");


            // }
            return View();
        }

        public IActionResult Detail(int Id)
        {
            Property property = db.Properties.Where(x => x.Id == Id).FirstOrDefault();
            return View(property);
        }


    }
}


