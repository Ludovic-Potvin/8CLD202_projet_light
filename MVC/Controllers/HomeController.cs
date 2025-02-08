using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Voir le IOptionsSnapshot qui importe dans l'object la configuraiton du AppConfig.
        // Ainsi que le IFeatureManager pour la gestion des Flags
        public HomeController(ILogger<HomeController> logger)
        {

            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
