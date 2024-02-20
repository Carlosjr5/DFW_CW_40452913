using DFW_CW_40452913.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Generic;
using DFW_CW_40452913.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DFW_CW_40452913.Controllers
{
   //Only Authorizing Admin access to this class
   //[Authorize(Roles= "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            this.userManager = userManager;
        }







        public IActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is a petition page with an aim to achieve a consensus between humanity.";

            return View();
        }
       // [Authorize(Roles = "admin")]
        public ActionResult Contact()
        {
            return View();
        }
     //   [Authorize(Roles = "admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        public List<Petition> GetPetitions()
        {
          
            return _context.Petitions.ToList();
        }

        public IActionResult PetitionDetails()
        {
            var petitions = GetPetitions();
            return View(petitions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
