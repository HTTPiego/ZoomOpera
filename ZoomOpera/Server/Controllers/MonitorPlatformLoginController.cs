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
    public class MonitorPlatformLoginController : ControllerBase
    {

        private readonly IService<IMonitorPlatform, MonitorPlatformDTO> _platformService;

        private IConfiguration _config;
        public MonitorPlatformLoginController(IService<IMonitorPlatform, MonitorPlatformDTO> platformService, IConfiguration config)
        {
            _platformService = platformService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<JwtDTO>> Login([FromBody] LoginDTO login)
        {
            string token = String.Empty;

            var platform = Authenticate(login);

            if (platform != null)
            {
                token = Generate(platform);
            }
            return await Task.FromResult(new JwtDTO(token));
        }

        [HttpPost("get-platform")]
        public async Task<IMonitorPlatform> GetPlatformByJwt([FromBody] JwtDTO jwt)
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var principle = tokenHandler.ValidateToken(jwt.Token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = (JwtSecurityToken)securityToken;

            if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                                                StringComparison.InvariantCultureIgnoreCase))
            {
                var userName = principle.FindFirst(ClaimTypes.Name)?.Value;
                var platform = await _platformService.FindFirstBy(platform => new ValueTask<bool>(platform.Name.Equals(userName)));
                //return Ok(platform);
                return await _platformService.FindFirstBy(platform => new ValueTask<bool>(platform.Name.Equals(userName)));
            }

            return null;
            //return NotFound("Token not found");
        }

        private string Generate(IMonitorPlatform platform)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                //new Claim(ClaimTypes.NameIdentifier platform.Id),
                new Claim(ClaimTypes.Name, platform.Name),
                new Claim(ClaimTypes.Role, platform.Role)

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(9),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IMonitorPlatform? Authenticate(LoginDTO login)
        {
            var currentPlatform = this._platformService.FindFirstBy(dbPlatform => new ValueTask<bool>
                (dbPlatform.Name.ToLower() == login.UserName.ToLower() && dbPlatform.Password == login.Password));

            if (currentPlatform.Result != null)
                return currentPlatform.Result;
            else
                return null;
        }
    }
}
