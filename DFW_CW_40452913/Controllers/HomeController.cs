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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Adding the user to the 'user' role
                    var roleResult = await _userManager.AddToRoleAsync(user, "user");
                    if (roleResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    else
                    {
                        // Handle the case where adding the role fails
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public IActionResult Login()
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
            var petitions = _context.Petitions.ToList(); // Retrieves all petitions
            var comments = _context.Comments.ToList(); // Retrieves all comments


            var model = new Tuple<List<Petition>, List<Comment>>(petitions, comments);

            return View(model); 
        }
       
        public IActionResult CreatePetition()
        {
        
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> Create(Petition model, IFormFile image)
        {
        
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    // Construct the file path
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/petitions", fileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    // Save the path in the database
                    model.ImageUrl = $"/images/petitions/{fileName}";
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle cases where the image is not provided
                    ModelState.AddModelError("", "Image is required.");
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> About(string searchQuery = "")
        {
            ViewBag.CurrentSearchQuery = searchQuery;

            IEnumerable<Petition> petitions;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                petitions = await _context.Petitions
                    .Include(p => p.Comments)
                    .Where(p => p.Title.Contains(searchQuery) || p.Description.Contains(searchQuery))
                    .ToListAsync();
            }
            else
            {
                petitions = await _context.Petitions.Include(p => p.Comments).ToListAsync();
            }

            var comments = await _context.Comments.ToListAsync();
            var model = Tuple.Create(petitions.ToList(), comments);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] Petition model)
        {
            if (model == null || model.Id <= 0)
            {
                return Json(new { success = false, message = "Invalid petition ID." });
            }

            // Ensure unique session for each user or browser
            var sessionId = HttpContext.Session.GetString("SessionID") ?? Guid.NewGuid().ToString();
            HttpContext.Session.SetString("SessionID", sessionId);

            // Check if the user has already voted
            var hasVoted = HttpContext.Session.GetString("Voted_" + model.Id);
            if (!string.IsNullOrEmpty(hasVoted))
            {
                return Json(new { success = false, message = "You have already voted for this petition." });
            }

            // Retrieve the petition from the database
            var petition = await _context.Petitions.FirstOrDefaultAsync(p => p.Id == model.Id);
            if (petition == null)
            {
                return Json(new { success = false, message = "Petition not found." });
            }

            // Increment votes and save changes
            petition.Votes++;
            await _context.SaveChangesAsync();

            // Mark in the session that the user has voted
            HttpContext.Session.SetString("Voted_" + model.Id, "true");

            return Json(new { success = true, votes = petition.Votes });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePetition(int id)
        {
            var petition = await _context.Petitions.FindAsync(id);
            if (petition == null)
            {
                return NotFound();
            }

            _context.Petitions.Remove(petition);
            await _context.SaveChangesAsync();

           // TempData["Message"] = "Petition deleted successfully.";
            return RedirectToAction(nameof(Index));
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
            ViewData["CurrentSortOrder"] = "default"; // Default sorting
            var petitions = _context.Petitions.OrderByDescending(p => p.Votes).ToList();
            return View(petitions);
        }

        public IActionResult SortedPetitions(string sortBy)
        {
            IEnumerable<Petition> petitions;
            ViewData["CurrentSortOrder"] = sortBy; 

            if (sortBy == "leastVoted")
            {
                petitions = _context.Petitions.OrderBy(p => p.Votes).ToList();
            }
            else // Default to "mostVoted"
            {
                petitions = _context.Petitions.OrderByDescending(p => p.Votes).ToList();
            }
            return View("PetitionList", petitions);
        }

        [AllowAnonymous]
        public async Task<IActionResult> PetitionDetails(int id)
        {
            var petition = await _context.Petitions
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

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

        [AllowAnonymous]
        public async Task<IActionResult> IndexComments()
        {
            var petitions = await _context.Petitions
                                          .Include(p => p.Comments)
                                          .ToListAsync();


            foreach (var petition in petitions)
            {
                petition.Comments ??= new List<Comment>();
            }

            return View(petitions);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int petitionId, string commentText)
        {
            var petition = await _context.Petitions.FindAsync(petitionId);
            if (petition == null)
            {
                TempData["Message"] = "Petition not found.";
                return NotFound();
            }

            var comment = new Comment
            {
                Text = commentText,
                PetitionId = petitionId,
                DatePosted = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // For JSON response
            //return Json(new { text = comment.Text, datePosted = comment.DatePosted.ToString("dd MMM yyyy") });

            TempData["Message"] = "Comment added successfully.";
            return Redirect($"/Home/About#{petitionId}");


        }




    }
}