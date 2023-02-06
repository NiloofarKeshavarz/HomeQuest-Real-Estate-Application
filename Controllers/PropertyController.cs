using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeQuest.Data;
using HomeQuest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HomeQuest.Data;
using HomeQuest.Models;
using Microsoft.AspNetCore.Authorization;
namespace HomeQuest.Controllers
{
    [Route("[controller]")]
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> logger;
         private IWebHostEnvironment environment;
        private HomeQuestDbContext db;

        public PropertyController(HomeQuestDbContext db, ILogger<PropertyController> logger,IWebHostEnvironment environment)
        {
            this.logger = logger;
            this.db = db;
            this.environment = environment;
        }
        
        [BindProperty]
        public Property? Property { get; set; }

        [BindProperty]
        public Image? Image { get; set; }

        [BindProperty]
         public IFormFile Upload { get; set; }


        [Route("/Index")]
        public IActionResult Index()
        {
            IEnumerable<Property> propList = db.Properties;
            return View(propList);
        }

        // Redirect to add property page
        [Authorize(Policy = "RequireAgentRole")]
        [Route("/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("/imageUploadDownload")]
        public IActionResult imageUploadDownload()
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

        [Route("/imageManager")]
        [HttpPost]
        public IActionResult imageManager(int Id)
        {
            ViewBag.currentPropertyId = Id;
            Console.WriteLine("going to image page with id:" + Id);


            // query all images url
            var imageUrls = db.Images
                .Where(i => i.PropertyId == Id)
                .Select(i => i.URL)
                .ToList();
            ViewBag.urlList = imageUrls;

            return View("~/Views/Image/ImageManager.cshtml");

            // TO-DO passing a list of images frm db as list
        }
        

    }
}


