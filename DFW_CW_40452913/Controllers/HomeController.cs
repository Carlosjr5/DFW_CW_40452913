using DFW_CW_40452913.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DFW_CW_40452913.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace DFW_CW_40452913.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment hostingEnvironment, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _signInManager = signInManager;
        }


        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var petitions = _context.Petitions.ToList(); // Retrieves all petitions from the database
            return View(petitions); // Passes the petitions to the view
        }


        [AllowAnonymous]
        public async Task<IActionResult> SearchPetitions(string searchQuery)
        {
            var filteredPetitions = string.IsNullOrEmpty(searchQuery) ?
                _context.Petitions.ToList() :
                _context.Petitions.Where(p => p.Title.Contains(searchQuery) || p.Description.Contains(searchQuery)).ToList();

            return PartialView("PetitionsList", filteredPetitions);
        }


        public IActionResult CreatePetition()
        {
            // This action method returns the view with the form
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Petition model)
        {
           
                _context.Petitions.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
        
        }








        //[HttpPost]
        //[ValidateAntiForgeryToken] // Ensure this is included if you're using anti-forgery tokens for security.
        //public async Task<IActionResult> Create(string Title, string Description, IFormFile Image)
        //{
        //    try
        //    {
        //        // Retrieve the current user's email to use as the author.
        //        // Ensure the user is logged in; otherwise, UserManager won't be able to retrieve the user.
        //        var user = await _userManager.GetUserAsync(User);
        //        var userEmail = user?.Email;

        //        // Create a new petition object.
        //        var petition = new Petition
        //        {
        //            Title = Title,
        //            Description = Description,
        //            Author = userEmail, // Assuming the author is the current logged-in user's email.
        //            DateCreated = DateTime.UtcNow // Set the current time as the creation time.
        //        };

        //        // Optionally handle the image file here if needed.

        //        // Add the new petition to the context and save changes to the database.
        //        _context.Petitions.Add(petition);
        //        await _context.SaveChangesAsync();

        //        // Redirect to a confirmation page or back to the list of petitions, for example.
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error.
        //        _logger.LogError($"Error creating petition: {ex.Message}");
        //        // Return an error view or display a user-friendly error message.
        //        return View("Error");
        //    }
        //}

        [AllowAnonymous]
        public async Task<IActionResult> About()
        {
            ViewBag.Message = "This is a petition page with an aim to achieve a consensus between humanity.";

            var petitions = await _context.Petitions.ToListAsync(); // Fetch petitions from the database
            return View(petitions); // Pass petitions to the view
        }

   


        [AllowAnonymous] 
        public ActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous] 
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous] 
        public IActionResult PetitionList()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult PetitionDetails(int id)
        {
            var petition = GetPetitionById(id);
            if (petition == null)
            {
                return NotFound();
            }
            return View(petition);
        }

        private Petition GetPetitionById(int id)
        {
            return _context.Petitions.FirstOrDefault(p => p.Id == id);
        }


        [AllowAnonymous] 
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Petition> GetPetitions()
        {
            return _context.Petitions.ToList();
        }
    }
}
