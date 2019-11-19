using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Net.Http;

namespace IoT.Mvc.Controllers
{
 
    public class UserController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            
            return View();
        }
    }
}