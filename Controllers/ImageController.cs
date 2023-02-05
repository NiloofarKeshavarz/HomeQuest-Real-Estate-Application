using Microsoft.AspNetCore.Mvc;
using HomeQuest.Data;
using HomeQuest.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure;
using Microsoft.Extensions.Logging;
using System;
using System.Web;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;



namespace HomeQuest.Controllers
{
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> logger;
        private IWebHostEnvironment environment;
        private HomeQuestDbContext db;

        public ImageController(HomeQuestDbContext db, ILogger<ImageController> logger, IWebHostEnvironment environment)
        {
            this.logger = logger;
            this.db = db;
            this.environment = environment;
        }

        [BindProperty]
        public Property Property { get; set; }

        [BindProperty]
        public Image Image { get; set; }




        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/UploadFiles")]
        [HttpPost]
        // public IActionResult UploadFiles(List<IFormFile> files){
        //     Console.WriteLine("connecting to azure blob...");
        //     return View();
        // }

        public async Task<IActionResult> UploadFiles(List<IFormFile> files, int propertyId)
        {

            Console.WriteLine("connecting to azure blob...");

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=blobstorageaccount2;AccountKey=LR02zvaUeLULuzrbJwMFyL7BeD1JHiQI1wJq41++ROBVRUWGpBH+6q5p71l4Fo6bTmTCqjB5g+A5+ASt04OcGg==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("homequest");
            await container.CreateIfNotExistsAsync();

            List<Image> uploadedFiles = new List<Image>();

            foreach (var file in files)
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
                using (var stream = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

                uploadedFiles.Add(new Image
                {
                    FileName = file.FileName,
                    URL = blockBlob.Uri.AbsoluteUri

                });

                Console.WriteLine("Upload infor to image table in db for property id: " + propertyId);
                Image newImage = new Image();
                newImage.FileName = file.FileName;
                newImage.URL = blockBlob.Uri.AbsoluteUri;
                newImage.PropertyId = propertyId;
                newImage.IsPrimaryImage = false;

                db.Images.Add(newImage);
                db.SaveChanges();
                Console.WriteLine("insertion new image DONE!");




            }

                        return View();


            // return View("imageUploadDownload", uploadedFiles);
        }






    }

    
}



