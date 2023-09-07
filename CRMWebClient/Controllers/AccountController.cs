using CRMWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRMWebClient.Controllers
{
    public class AccountController : Controller
    {
        private List<Users> users = new List<Users>()
        {
            new Users(){Login = "Admin", Password = "123", Role = "Administrator"},
            new Users() { Login = "User", Password = "123", Role = "User"}
        };

        [HttpPost("/token")]
        public IActionResult Token(string userName, string password)
        {
            var identity = GetIdentity(userName, password);
            if (identity == null)
            {
                return BadRequest();
            }

            var now = DateTime.UtcNow;
            //Создание токена
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LifeTIme)),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
                var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodeJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(string userName, string password)
        {
            Users user = users.FirstOrDefault(e => e.Login == userName && e.Password == password);

            if (user != null)
            {
                var claim = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                    claim,
                    "Token",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}
