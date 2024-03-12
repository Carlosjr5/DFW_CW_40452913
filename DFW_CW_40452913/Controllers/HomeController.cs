using DFW_CW_40452913.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Generic;
using DFW_CW_40452913.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFW_CW_40452913.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous] 
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous] 
        public IActionResult CreatePetition()
        {
            return View();
        }

        [AllowAnonymous] 
        public ActionResult About()
        {
            ViewBag.Message = "This is a petition page with an aim to achieve a consensus between humanity.";
            return View();
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
