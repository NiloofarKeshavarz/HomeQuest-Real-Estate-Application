using System;
using System.Diagnostics;
using HomeQuest.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeQuest.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(ILogger<PropertyController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddProperty()
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
