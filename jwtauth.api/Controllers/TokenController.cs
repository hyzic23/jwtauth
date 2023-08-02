using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using jwtauth.models;
using jwtauth.dataaccess.IService;
using jwtauth.api.Config;
using Microsoft.Extensions.Options;
using FluentValidation;
using jwtauth.api.Dtos;

namespace jwtauth.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _configuration;
        private IOptions<Jwt> _jwtSettings;
        private IUserInfoService _userService;
        private IAppSettings _appSettings;
        private readonly IValidator<UserInfoDto> _validator;

        public TokenController(IConfiguration configuration,
                               IUserInfoService userService,
                               IOptions<Jwt> jwtSettings,
                               IAppSettings appSettings,
                               IValidator<UserInfoDto> validator)
        {
            _configuration = configuration;
            _userService = userService;
            _jwtSettings = jwtSettings;
            _appSettings = appSettings;
            _validator = validator;
        }

        [HttpPost]
        [Route("generatetoken")]
        public async Task<IActionResult> Post([FromBody] UserInfoDto userInfoDto)
        {
            var result = await Task.FromResult(_validator.Validate(userInfoDto));
            if (!result.IsValid)
                return BadRequest(result);

            if (userInfoDto != null && userInfoDto.Email != null && userInfoDto.Password != null)
            {
                UserInfo userInfo = new UserInfo
                {
                        UserId = userInfoDto.UserId,
                        UserName = userInfoDto.UserName,
                        DisplayName = userInfoDto.DisplayName,
                        Email = userInfoDto.Email,
                        Password = userInfoDto.Password
                };
                var user = AuthenticateUser(userInfo);
                if (userInfo != null)
                {
                    //create claims details based on the user information
                    var claims = new[]{
                        //new Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Value.Subject),
                        new Claim(JwtRegisteredClaimNames.Sub, _appSettings.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("UserId", user.UserId.ToString()),
                            new Claim("DisplayName", user.DisplayName),
                            new Claim("UserName", user.UserName),
                            new Claim("Email", user.Email)
                    };

                    //var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _appSettings.Issuer,
                        _appSettings.Audience,
                        //_jwtSettings.Value.Issuer,
                        //_jwtSettings.Value.Audience,
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

        //[HttpPost]
        //[Route("generatetoken")]
        //public async Task<IActionResult> Post([FromBody]UserInfo userInfo)
        //{
        //    if(userInfo != null && userInfo.Email != null && userInfo.Password != null)
        //    {
        //        var user = AuthenticateUser(userInfo);
        //        if(userInfo != null)
        //        {
        //            //create claims details based on the user information
        //            var claims = new[]{
        //                //new Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Value.Subject),
        //                new Claim(JwtRegisteredClaimNames.Sub, _appSettings.Subject),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        //                    new Claim("UserId", user.UserId.ToString()),
        //                    new Claim("DisplayName", user.DisplayName),
        //                    new Claim("UserName", user.UserName),
        //                    new Claim("Email", user.Email)
        //            };

        //            //var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
        //            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Key));
        //            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //            var token = new JwtSecurityToken(
        //                _appSettings.Issuer,
        //                _appSettings.Audience,
        //                //_jwtSettings.Value.Issuer,
        //                //_jwtSettings.Value.Audience,
        //                claims,
        //                expires: DateTime.UtcNow.AddMinutes(10),
        //                signingCredentials: signIn
        //            );
        //            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        //        }
        //        else
        //        {
        //            return BadRequest("Invalid credentials");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        private UserInfo AuthenticateUser(UserInfo request)
        {
            UserInfo response = _userService.AuthenticateUser(request);
            return response;
        }
    }
}