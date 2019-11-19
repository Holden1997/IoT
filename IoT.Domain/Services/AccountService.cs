using IoT.Common.Models;
using IoT.Domain.Interfaces;
using IoT.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Domain.Services
{
    public class AccountService : IAccountService
    {


        private readonly SignInManager<AppUser> _signinManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        public AccountService(
                          UserManager<AppUser> userManager,
                          SignInManager<AppUser> signinManager,
                          RoleManager<IdentityRole> roleManager,
                         
                          IConfiguration configuration)
        {
            _signinManager = signinManager;
            _userManager = userManager;
            _roleManager = roleManager;
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        public async Task<object> CreateAccountAsync(User user)
        {

            var appUser = new AppUser
            {
                Email = user.Email,
                UserName = user.Email,
                Id = Guid.NewGuid().ToString()
            };

            var identityResult = await _userManager.CreateAsync(appUser, user.Password)
                .ConfigureAwait(false);

            if (identityResult.Succeeded)
            {
                var state = await _roleManager.RoleExistsAsync("user");
                if (state)
                    await _userManager.AddToRoleAsync(appUser, "user");
      
                var token = await GetTokenAsync(appUser)
                    .ConfigureAwait(false);
                TokenModel tokenModel = new TokenModel(token, "");

                return tokenModel;

            }
            
            return identityResult.Errors;
        }

        private async Task<string>GetTokenAsync(AppUser appUser)
        {

            var taskIdentity = GetIdentityAsync(appUser).ConfigureAwait(false);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var nowUtc = DateTime.Now.ToUniversalTime();
            var expires = nowUtc.AddHours(int.Parse(Configuration["Tokens:Time"])).ToUniversalTime(); // не забыть подправить

            var identity = await taskIdentity;

            var token = new JwtSecurityToken(
            Configuration["Tokens:Issuer"],
            Configuration["Tokens:Audience"],
            claims: identity.Claims,
            expires: expires,
            signingCredentials: creds);

            var response = new JwtSecurityTokenHandler().WriteToken(token);

            identity.AddClaim(new Claim("jwt", response));
            identity.AddClaim(new Claim("clinetId", appUser.Id));   
           
            await _userManager.AddClaimsAsync(appUser, identity.Claims)
                .ConfigureAwait(false);

            return response;
        }

        public async Task<string> SignInAsync(User userModel)
        {

            var appUser = await _userManager.FindByNameAsync(userModel.Email)
                .ConfigureAwait(false);
            if (appUser == null)
                return "Email is not fount";

            var result = await _signinManager.CheckPasswordSignInAsync(appUser, userModel.Password, true)
                .ConfigureAwait(false);
           
            if (result.Succeeded == false)
                return "Invalid login or password";

            var claims = await _userManager.GetClaimsAsync(appUser).ConfigureAwait(false);

            //if (claims.Count == 0)
            //    return await GetTokenAsync(appUser)
            //        .ConfigureAwait(false);

            return await  GetTokenAsync(appUser);
        }
        private async Task<ClaimsIdentity> GetIdentityAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            
            return claimsIdentity;
        }
    }
}
