using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLoginController : ControllerBase
    {

        private readonly IService<IAdmin, AdminDTO> _adminService;

        private IConfiguration _config;

        public AdminLoginController(IService<IAdmin, AdminDTO> adminService, IConfiguration config)
        {
            this._adminService = adminService;
            this._config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<JwtDTO>> Login([FromBody] LoginDTO login)
        {
            string token = String.Empty;

            var admin = Authenticate(login);

            if (admin != null)
            {
                token = Generate(admin);
            }
            return await Task.FromResult(new JwtDTO(token));
        }

        private string Generate(IAdmin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.GivenName, admin.GivenName),
                new Claim(ClaimTypes.Surname, admin.Surname),
                new Claim(ClaimTypes.Role, admin.Role)

                //TODO: controlla se primary sid va bene
                //,new Claim(ClaimTypes.PrimarySid, admin.Id.ToString())
                // NameIdentifier probabilmente e quello giusto

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpPost("get-admin")]
        public IActionResult GetUserByJwt([FromBody] string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = (JwtSecurityToken)securityToken;

            if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                                                StringComparison.InvariantCultureIgnoreCase))
            {
                var userName = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Ok(_adminService.FindFirstBy(admin => new ValueTask<bool>
                                                                (admin.Name.Equals(userName))));
            }

            return NotFound("token strano");
        }

        private IAdmin? Authenticate(LoginDTO login)
        {
            var currentAdmin =  this._adminService.FindFirstBy(dbAdmin => new ValueTask<bool>
                                        (dbAdmin.Name.ToLower() == login.UserName.ToLower() && dbAdmin.Password == login.Password));

            if (currentAdmin.Result != null)
                return currentAdmin.Result;
            else
                return null;
        }
    }
}
