using System;
using Microsoft.AspNetCore.Mvc;

namespace HomeQuest.Controllers
{
    public class AgentController : Controller
    {
        private readonly ILogger<AgentController> _logger;
        public AgentController(ILogger<AgentController> logger)
        {
            _logger = logger;
        }


        [Route("/Subscription")]
        public IActionResult Subscription()
        {
            return View();
        }


    }
}
