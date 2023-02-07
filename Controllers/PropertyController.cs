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

        public PropertyController(HomeQuestDbContext db, ILogger<PropertyController> logger, IWebHostEnvironment environment)
        {
            this.logger = logger;
            this.db = db;
            this.environment = environment;
        }

        [BindProperty]
        public Property? Property { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Image? Image { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public Property? PropertyToEdit { get; set; }


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

        //Update  aproperty 

        [Route("/PopulateToUpdate")]
        [HttpPost]
        public IActionResult GetPropertyToUpdate(int? Id)
        {
            PropertyToEdit = db.Properties.Where(a => a.Id == Id).FirstOrDefault();
            Console.WriteLine("Property ID is :" + PropertyToEdit.Id);
            if (PropertyToEdit == null)

            {
                logger.LogWarning("Property not found");
                return NotFound();
            }
            return View("~/Views/Property/Update.cshtml",PropertyToEdit);


        }
        [Route("/Update")]
        [HttpPost]
        public IActionResult Update()
        {

            Console.WriteLine("PropertyToEdit Title is :" + PropertyToEdit.Title);
            Console.WriteLine("PropertyToEdit ID is :" + PropertyToEdit.Id);

            Property UpdatedProperty = db.Properties.Find(PropertyToEdit.Id);
            UpdatedProperty.Title = PropertyToEdit.Title;
            UpdatedProperty.Description = PropertyToEdit.Description;
            UpdatedProperty.Address = PropertyToEdit.Address;
            UpdatedProperty.PostalCode = PropertyToEdit.PostalCode;
            UpdatedProperty.Price = PropertyToEdit.Price;
            UpdatedProperty.Floors = PropertyToEdit.Floors;
            UpdatedProperty.BedroomCount = PropertyToEdit.BedroomCount;
            UpdatedProperty.BathroomCount = PropertyToEdit.BathroomCount;
            UpdatedProperty.GarageCont = PropertyToEdit.GarageCont;
            UpdatedProperty.YearBuilt = PropertyToEdit.YearBuilt;
            UpdatedProperty.FloorArea = PropertyToEdit.FloorArea;
            UpdatedProperty.LotArea = PropertyToEdit.LotArea;
            UpdatedProperty.CreatedAt = PropertyToEdit.CreatedAt;
            UpdatedProperty.Status = PropertyToEdit.Status;
            UpdatedProperty.Type = PropertyToEdit.Type;

            db.Properties.Update(UpdatedProperty);
            db.SaveChanges();
            return View("~/Views/Property/Detail.cshtml",UpdatedProperty);
        }

        [Route("/Delete")]
        [HttpPost]
        public IActionResult Delete(int Id)
        {

            Console.WriteLine("PropertyToEdit ID is :" + Id);

            // Delete from Properties table in db
            Property PropertyToDelete = db.Properties.Find(Id);
            db.Properties.Remove(PropertyToDelete);
            db.SaveChanges();

            //Delete images from images table in db
            IEnumerable<Image> ImageListToDelete = db.Images.Where(x => x.PropertyId == Id).ToList();
            db.Images.RemoveRange(ImageListToDelete);

            return RedirectToAction("Index");
        }


    }
}


