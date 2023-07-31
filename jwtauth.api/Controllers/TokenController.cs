using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using jwtauth.models;

namespace jwtauth.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("generatetoken")]
        public async Task<IActionResult> Post([FromBody]UserInfo userInfo)
        {
            if(userInfo != null && userInfo.Email != null && userInfo.Password != null)
            {
                var user = AuthenticateUser(userInfo);
                if(userInfo != null)
                {
                    //create claims details based on the user information
                    var claims = new[]{
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("UserId", user.UserId.ToString()),
                            new Claim("DisplayName", user.DisplayName),
                            new Claim("UserName", user.UserName),
                            new Claim("Email", user.Email)
                    };

                    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private UserInfo AuthenticateUser(UserInfo request)
        {
            UserInfo response = new UserInfo();
            if(request.Email == "admin@admin.com" && request.Password == "admin")
            {
                response.UserName = "admin@admin.com";
                response.Email = "admin@admin.com";
                response.DisplayName = "Administration";
            }
            else
            {

            }
                return response;
        }
    }
}