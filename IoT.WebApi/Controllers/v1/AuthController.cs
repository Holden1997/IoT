using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoT.Common.Models;
using IoT.Domain.Interfaces;
using IoT.WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IoT.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }
      

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthViewModel authViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = await _accountService.SignInAsync(new Common.Models.User
            {
                Email = authViewModel.Email,
                Password = authViewModel.Password
            });


            return Content(token);
        }

        [HttpPost("logintest")]
        public async Task<IActionResult> LoginFromHeader([FromHeader] string login, [FromHeader] string password)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = await _accountService.SignInAsync(new Common.Models.User
            {
                Email = login,
                Password = password
            });


            return Content(token);
        }


        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] AuthViewModel authViewModel)
        {
         
            if (!ModelState.IsValid)
                return BadRequest("Model not is valid");

            object @object = await _accountService.CreateAccountAsync(new User
            {
                Email = authViewModel.Email,
                Password = authViewModel.Password
            });
            
            switch(@object)
            {
                case TokenModel token:
                    return Ok(JsonConvert.SerializeObject(token));

                case IEnumerable<IdentityError> errors:
                    return BadRequest(JsonConvert.SerializeObject(errors));

                default: throw new Exception("type is not sapport");

            }
            
        }
        
    }
}