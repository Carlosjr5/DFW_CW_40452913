using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq; // For the Contains method on arrays
using DFW_CW_40452913.Data;
using DFW_CW_40452913.Models;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging; // Required for ILogger

[ApiController]
[Route("[controller]")]
public class PetitionManager : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ILogger<PetitionManager> _logger; // Corrected to use ILogger<T>

    public PetitionManager(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment hostingEnvironment, ILogger<PetitionManager> logger)
    {
        _context = context;
        _userManager = userManager;
        _hostingEnvironment = hostingEnvironment;
        _logger = logger; // Initialize the logger
    }

    //[HttpPost]
    //public async Task<IActionResult> CreatePetition(Petition model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _context.Petitions.Add(model);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction("Index");
    //    }
    //    else
    //    {
    //        return RedirectToAction("Contact");
    //    }
    //    return View(model);
    //}


    [HttpPost("create"), DisableRequestSizeLimit]
    public async Task<IActionResult> CreatePetition([FromForm] string title, [FromForm] string description, IFormFile image)
    {
        var user = await _userManager.GetUserAsync(User);
        var userEmail = user?.Email; // Get the current user's email

        string imageUrl = null;
        try
        {
            if (image != null && image.Length > 0)
            {
                if (image.Length > 10 * 1024 * 1024) // 10 MB limit
                {
                    return BadRequest("File size exceeds the allowed limit.");
                }

                var allowedTypes = new[] { "image/jpeg", "image/png" };
                if (!allowedTypes.Contains(image.ContentType))
                {
                    return BadRequest("Invalid file type.");
                }

                var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                imageUrl = $"/uploads/{fileName}"; // Constructed relative URL to the uploaded image
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while uploading the file: {ex.Message}");
            return StatusCode(500, "An internal error occurred");
        }

        var petition = new Petition
        {
            Title = title,
            Description = description,
            ImageUrl = imageUrl
        };

        _context.Petitions.Add(petition);
        await _context.SaveChangesAsync();

        return Ok(petition);
    }
}
