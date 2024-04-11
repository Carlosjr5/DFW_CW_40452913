using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DFW_CW_40452913.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public IActionResult Index()
        {
            var items = GetFiles();
            return View(items);
        }


        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/petitions");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Define the standard size for all images
                    const int width = 800; 
                    const int height = 600; 

                    string fullPath = Path.Combine(uploadPath, file.FileName);

                    // Resize the image
                    using (var image = Image.Load(file.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(width, height));
                        await image.SaveAsync(fullPath); // Save the resized image
                    }

                    ViewBag.Message = "File uploaded and resized successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            var items = GetFiles();
            return View(items);
        }


        public IActionResult Download(string ImageName)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/petitions", ImageName);
            string contentType = "APPLICATION/octet-stream";
            return File(fullPath, contentType, ImageName);
        }

        private List<string> GetFiles()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/petitions");
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> items = new List<string>();

            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            return items;
        }
    }
}
