using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeQuest.Data;
using HomeQuest.Models;
using HomeQuest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace HomeQuest.Controllers
{
    [Route("[controller]")]
    public class SMTPController : Controller
    {

        private readonly ILogger<PropertyController> logger;
        private IWebHostEnvironment environment;
        private Data.HomeQuestDbContext db;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IMailService mailService;


        public SMTPController(IMailService mailService, HomeQuestDbContext db, ILogger<SMTPController> logger, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            // this.logger = logger;
            this.db = db;
            this.environment = environment;
            this.userManager = userManager;
            this.mailService = mailService;
        }

        [BindProperty]
        public Property? Property { get; set; }

            // //////////   Offer form to email handler/controller  Module      ///////////// 
        [Route("SendSMTPOffer")]
        [HttpPost]
        public async Task<IActionResult> SendSMTPOffer(int OfferAmount, string OfferMessage, int PropertyId, int Id, DateTime OfferDate)
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

                return View("~/Views/Property/Detail.cshtml");

            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/Error.cshtml");

            }




        }

        private IActionResult View(string v)
        {
            throw new NotImplementedException();
        }
    }
}