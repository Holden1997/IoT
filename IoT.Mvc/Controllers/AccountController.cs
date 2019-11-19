using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IoT.Mvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace IoT.Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            var claimsIdentity = HttpContext.User.Identities.ToList();
            var claimsUser = claimsIdentity[0].Claims.ToList();

            return View(new AccountViewModel
            {
                ClientId = Guid.Parse(claimsUser[1].Value),
                Email = claimsUser[0].Value
            });
        }

        [HttpGet]
        public IActionResult SupportedDevace()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Profile()
        {
            var claimsIdentity = HttpContext.User.Identities.ToList();
            var claimsUser = claimsIdentity[0].Claims.ToList();

            return View(new AccountViewModel
            {
                ClientId = Guid.Parse(claimsUser[1].Value),
                Email = claimsUser[0].Value
            });
        }

        [HttpGet]
        public IActionResult Access() => StatusCode(200);
      

    }
}
