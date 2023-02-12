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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HomeQuest.Services;
using System.Net.Mail;

namespace HomeQuest.Controllers
{
    [Route("[controller]")]
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> logger;
        private IWebHostEnvironment environment;
        private HomeQuestDbContext db;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IMailService mailService;


        public PropertyController(HomeQuestDbContext db, ILogger<PropertyController> logger, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager, IMailService mailService)
        {
            this.logger = logger;
            this.db = db;
            this.environment = environment;
            this.userManager = userManager;
            this.mailService = mailService;
        }

        [BindProperty]
        public Property? Property { get; set; }

        [BindProperty]
        public DateTime Today{get; set;} = DateTime.Today;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Image? Image { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public Property? PropertyToEdit { get; set; }

        [BindProperty]
        public Offer? Offer { get; set; }

       [Route("/Index")]
        public IActionResult Index()
        {
            IEnumerable<Property> propList = db.Properties.Include(p => p.Images).ToList();
            var currentUserId = userManager.GetUserId(User);
            var favoritePropertyList = db.Favorites.Where(f => f.UserId == currentUserId).ToList();
            ViewBag.favoritePropertyList = favoritePropertyList;
            return View(propList);
        }


        [HttpPost]
        public IActionResult AddFavoriteProperty(int favoritePropertyId, string favoriteButton)
        {
            var currentUserId = userManager.GetUserId(User);
            if (favoriteButton == "Add To Favorite")
            {
                var propertyId = favoritePropertyId;


                Favorite favorite = new Favorite();
                favorite.PropertyId = propertyId;
                favorite.UserId = currentUserId;
                db.Favorites.Add(favorite);
            }
            if (favoriteButton == "Remove From Favorite")
            {
                // var property = db.Properties.Include(x => x.Favorites).FirstOrDefault(x => x.Id == favoritePropertyId);
                // var favorite = property.Favorites.FirstOrDefault(x => x.UserId == currentUserId);
                // if (favorite != null)
                // {
                //     property.Favorites.Remove(favorite);
                // }
                var favoriteProperty = db.Favorites.Where(f => f.PropertyId == favoritePropertyId).Where(f => f.UserId == currentUserId).FirstOrDefault();
                if(favoriteProperty != null){
                    db.Favorites.Remove(favoriteProperty);
                }
            }


            db.SaveChanges();
            return RedirectToAction("Index");

        }


        [Route("/SearchResult")]
        public IActionResult SearchResult()
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

        // [Route("/imageUploadDownload")]
        // public IActionResult imageUploadDownload()
        // {
        //     return View();
        // }


        [Route("/CreateNewProperty")]
        [HttpPost]
        public IActionResult CreateNewProperty()
        {

            db.Properties.Add(Property);
            db.SaveChangesAsync();
            Console.WriteLine("insertion DONE!");


            // }
            return View();
        }
        // Show images of each property////////////////////////////////////////////////////
        private void FetchImageUrlListToViewBag(int id)
        {

            // Fetch property images from the db for image gallery
            var imageUrls = db.Images.Where(i => i.PropertyId == Id).Select(i => i.URL).ToList();
            ViewBag.urlList = imageUrls;

        }

        [Route("/Property/{id}")]
        public IActionResult Detail(int Id)
        {
            // Fetch property from the DB
            Property property = db.Properties.Where(x => x.Id == Id).FirstOrDefault();
            FetchImageUrlListToViewBag(Id);

            return View(property);
        }

        // [Route("/imageManager/{id}")]
        [Route("/image")]
        [HttpPost]
        public IActionResult image(int Id)
        {
            ViewBag.currentPropertyId = Id;
            Console.WriteLine("going to image page with id:" + Id);


            FetchImageUrlListToViewBag(Id);


            return View("~/Views/Image/Index.cshtml");

            // TO-DO passing a list of images frm db as list
        }

        //Update  aproperty 
        // [Route("/PopulateToUpdate/{id}")]
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
            return View("~/Views/Property/Update.cshtml", PropertyToEdit);
        }


        [Route("/Update")]
        [HttpPost]
        public IActionResult Update()
        {

            Console.WriteLine("PropertyToEdit is :" + PropertyToEdit.Title);
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

            FetchImageUrlListToViewBag(PropertyToEdit.Id);

            return View("~/Views/Property/Detail.cshtml", UpdatedProperty);
        }

        //Delete a property /////////////////////////////////////////////////////////////
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


        // Filter Module                  //////////////////////////////////////////////// 

        [Route("/FilterProperty")]
        [HttpPost]
        public IActionResult FilterProperty(string PropertyAddress, int PropertyMinPriceFilter, int PropertyMaxPriceFilter, string PropertyPostalCodeFilter, int PropertyTypeFilter)
        {
            Console.WriteLine("Start Filtering index with:");
            Console.WriteLine("min price:" + PropertyMinPriceFilter);
            Console.WriteLine("max price:" + PropertyMaxPriceFilter);
            Console.WriteLine("postal code:" + PropertyPostalCodeFilter);
            Console.WriteLine("type:" + PropertyTypeFilter);

            IEnumerable<Property> filteredList = db.Properties;

            if(PropertyAddress != "Search By Address" ){
                filteredList = filteredList.Where(p => p.Address.Contains(PropertyAddress, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            

            // 1st filter: all min price filters are acceptable and logical
            filteredList = filteredList.Where(p => p.Price >= PropertyMinPriceFilter);

            // 2n filter: max price is logical if is equal or more than min price
            if (PropertyMaxPriceFilter > PropertyMinPriceFilter) filteredList = filteredList.Where(p => p.Price <= PropertyMaxPriceFilter);

            // 3rd filter: if postal code is valid
            if (PropertyPostalCodeFilter != null) filteredList = filteredList.Where(p => p.PostalCode == PropertyPostalCodeFilter);

            // 4th filter: if property type filter is not "ALL"
            switch (PropertyTypeFilter)
            {
                case 10:
                    filteredList = filteredList.Where(p => p.Type == Property.PropertyType.House);
                    break;
                case 20:
                    filteredList = filteredList.Where(p => p.Type == Property.PropertyType.Apartment);
                    break;
                case 30:
                    filteredList = filteredList.Where(p => p.Type == Property.PropertyType.Duplex);
                    break;
                case 40:
                    filteredList = filteredList.Where(p => p.Type == Property.PropertyType.TownHouse);
                    break;
                case 50:
                    filteredList = filteredList.Where(p => p.Type == Property.PropertyType.Condor);
                    break;
            }





            return View("~/Views/Property/SearchResult.cshtml", filteredList);

        }





        // //////////   Offer form to email handler/controller  Module      ///////////// 
        [Route("SendOffer")]
        [HttpPost]
        public async Task<IActionResult> SendOffer(int OfferAmount, string OfferMessage, int PropertyId, int Id, DateTime OfferDate)
        {

            var currentUserId = userManager.GetUserId(User);

            // IEnumerable<Offer> offerList = db.Offers;

            // // from different Model in the same view
            // db.Offers.Add(Offer);
            // db.SaveChanges();


            Console.WriteLine("Sending new Offer ...");
            Console.WriteLine("new offer PropertyId " + PropertyId);
            Console.WriteLine("new offer OfferAmount: " + OfferAmount);
            Console.WriteLine("new offer expiry date: " + OfferDate);
            Console.WriteLine("new offer OfferMessage " + OfferMessage);
            Console.WriteLine("new offer UserId: " + currentUserId);
            Console.WriteLine("new offer user email: " + userManager.GetUserName(User));


            // making offer object 
            var newOffer = new Offer();
            newOffer.OfferAmount = OfferAmount;
            newOffer.OfferDate = DateTime.Now;
            newOffer.OfferMessage = OfferMessage;
            newOffer.PropertyId = PropertyId;
            //System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            // Console.WriteLine("This is currentUser: " + currentUser.ToString());

            var user = await userManager.GetUserAsync(User);
            user.Email = "a@a.com ";
            // making MailRequet object
            MailRequest newMailRequest = new MailRequest();
            newMailRequest.UserEmail = userManager.GetUserName(User);
            newMailRequest.OfferAmount = OfferAmount;
            newMailRequest.OfferMessage = OfferMessage;

            Console.WriteLine(newMailRequest.UserEmail + "/// " + newMailRequest.OfferAmount + "/// " + newMailRequest.OfferMessage);



            // call and send "request obj" to the email api mail controller with post method : /api/mail
            try
            {
                Console.WriteLine("sending Request Email ...");
                await mailService.SendEmailAsync(newMailRequest);
                Console.WriteLine("sending Request Email was successful");
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/Error.cshtml");

            }
            
        }




    }
}


