using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IoT.Mvc.Models;
using System.Net.Http;
using System.Net;
using System;

namespace IoT.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult React()
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
