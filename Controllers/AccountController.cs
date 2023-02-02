using System;
using System.Diagnostics;
using HomeQuest.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeQuest.Controllers
{
    public class AccountController : Controller
    {
         private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger) => _logger = logger;

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
