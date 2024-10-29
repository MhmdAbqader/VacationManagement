using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VacationManagement.Models;

namespace VacationManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.time = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"); //.Add(TimeSpan.FromDays(1));
           // throw new Exception("Test My Error");
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