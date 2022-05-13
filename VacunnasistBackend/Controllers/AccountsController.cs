using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VacunassistBackend.Models;
using VacunassistBackend.Services;

namespace VacunassistBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;

        public AccountsController(IUsersService usersService, IConfiguration configuration)
        {
            this._usersService = usersService;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel model)
        {
            if (ModelState.IsValid == false)
                return BadRequest("Invalid Request");

            var user = _usersService.Authenticate(model.Username, model.Password);

            if (user != null)
            {
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                var token = GetToken(authClaims);
                return Ok(new
                {
                    username = user.UserName,
                    role = user.Role,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}