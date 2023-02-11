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
using System.Linq;



namespace HomeQuest.Controllers
{
    [Route("[controller]")]
    public class ImageController : Controller
    {
        public int currentPropertyId = 0;
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=blobstorageaccount2;AccountKey=LR02zvaUeLULuzrbJwMFyL7BeD1JHiQI1wJq41++ROBVRUWGpBH+6q5p71l4Fo6bTmTCqjB5g+A5+ASt04OcGg==;EndpointSuffix=core.windows.net";
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
        public Property? Property { get; set; }

        [BindProperty]
        public Image? Image { get; set; }




        public void ReloadViewBagWithImageUrlList(int currentPropertyId)
        {
            var imageUrls = db.Images
                .Where(i => i.PropertyId == currentPropertyId)
                .Select(i => i.URL)
                .ToList();
                ViewBag.urlList = imageUrls;
                ViewBag.currentPropertyId = currentPropertyId;
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

                Console.WriteLine("Upload info to image table in db for property id: " + propertyId);
                Image newImage = new Image();
                newImage.FileName = file.FileName;
                newImage.URL = blockBlob.Uri.AbsoluteUri;
                currentPropertyId = propertyId;
                newImage.PropertyId = propertyId;
                newImage.IsPrimaryImage = false;

                db.Images.Add(newImage);
                db.SaveChanges();
                Console.WriteLine("insertion new image DONE!");
                Console.WriteLine("View bag captured with the value: " + currentPropertyId);

        


            }
            ReloadViewBagWithImageUrlList(currentPropertyId);

            return View("~/Views/Image/Index.cshtml");
        }










        [Route("DeleteImage")]
        [HttpPost]
        public ActionResult DeleteImage(string urlToDelete, int propertyId){

            Console.WriteLine("Deleting image for property id" + propertyId + "| url: " + urlToDelete);

            // Delete from the db
            // Image image = db.Images.SingleOrDefault(i => i.URL == urlToDelete);
            Image image = db.Images.Where(i => i.URL == urlToDelete).FirstOrDefault();
            if (image != null)
            {
                db.Images.Remove(image);
                db.SaveChanges();
                Console.WriteLine("Image deleted from DB successfully");
            }

            // Delete Image from the azure blob

            Console.WriteLine("connecting to azure blob...");

            //FIXME: the delete operation removes the image from the database but not the blob storage.
            //I just committed it to the Git to have the delete function but I still need to work on that delete from blob. 


            // Replace "connectionString" with your actual connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Replace "fileUrl" with the actual URL of the file you want to delete
            Uri fileUri = new Uri(urlToDelete);
            string blobName = fileUri.Segments[fileUri.Segments.Length - 1];
            CloudBlobContainer container = blobClient.GetContainerReference(fileUri.Host);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            Console.WriteLine("urlto delete: "+urlToDelete);
            Console.WriteLine("blobname:" + blobName);
            Console.WriteLine("blockBlob:" + blockBlob);
            Console.WriteLine("fileUri:" + fileUri);


            // Delete the blob
             blockBlob.DeleteAsync();
            //  if (container.Properties.GetValueOrDefault(BlobContainerPublicAccessType.Blob).HasFlag(BlobContainerPublicAccessType.SoftDelete))
        // {
            // Delete the blob and its previous versions if soft delete is enabled
        //     blobClient.DeleteBlobIfExists(container.GetBlockBlobReference(blobName), true, AccessCondition.GenerateEmptyCondition(), new BlobRequestOptions(), new OperationContext());
        // }
        // else
        // {
            // Delete the blob
            //blockBlob.DeleteAsync();
        // }

            //     var imageUrls = db.Images
            //         .Where(i => i.PropertyId == propertyId)
            //         .Select(i => i.URL)
            //         .ToList();
            //     ViewBag.urlList = imageUrls;

            //     Console.WriteLine("id =" + propertyId);

            // return View("~/Views/Image/Index.cshtml");
            ReloadViewBagWithImageUrlList(propertyId);

            return View("~/Views/Image/Index.cshtml");
        }







        [Route("SetAsPrimary")]
        [HttpPost]
        public IActionResult SetAsPrimary(string SetAsPrimary, int propertyId){
            Image image = db.Images.Where(i => i.URL == SetAsPrimary).FirstOrDefault();
            Console.WriteLine("This is Image Id"+  image.Id);
            Console.WriteLine("This is Image Id"+  image.IsPrimaryImage);
            // Reset isPrimary column in image to zero for specific PropertyId
            var imageList = db.Images.Where(i => i.PropertyId == image.PropertyId).ToList();

            foreach(Image img in imageList ){
                img.IsPrimaryImage = false;
            }


            image.IsPrimaryImage = true;
            
            db.SaveChanges();
            Console.WriteLine("This is Image Id"+  image.IsPrimaryImage);

            
            ReloadViewBagWithImageUrlList(propertyId);

            return View("~/Views/Image/Index.cshtml");

        }

        [Route("ReturnToDetail")]
        public IActionResult ReturnToPropertyDetailPage(){
            
            return View("~/Views/Property/Detail.cshtml");

        }




    }

    
}



