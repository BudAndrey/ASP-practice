using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Config;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private Person person = new Person
        {
            Login = "admin",
            Password = "admin",
            Role = "admin"
        };

        public AccountController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Token([FromBody]Person p)
        {
            var identity = await GetIdentity(p.Login, p.Password);
            if (identity == null)
            {
                return BadRequest(new { error = "Invalid login or password" });
            }

            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer:AuthOptions.ISSUER,
                audience:AuthOptions.AUDIENCE,
                notBefore:now,
                claims:identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),SecurityAlgorithms.HmacSha256)
           );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Json(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            var user = await userManager.FindByNameAsync(login);
            var paswordResult = await userManager.CheckPasswordAsync(user, password);
            if (paswordResult)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType,person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType,person.Role)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}
