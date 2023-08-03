using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using jwtauth.models;
using jwtauth.dataaccess.IService;
using jwtauth.api.Config;
using Microsoft.Extensions.Options;
using FluentValidation;
using jwtauth.api.Dtos;
using Microsoft.IdentityModel.Tokens;
using ValidationFailure = FluentValidation.Results.ValidationFailure;
using AutoMapper;
using jwtauth.models.Response;

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
        private readonly IMapper _mapper;

        public TokenController(IConfiguration configuration,
                               IUserInfoService userService,
                               IOptions<Jwt> jwtSettings,
                               IAppSettings appSettings,
                               IValidator<UserInfoDto> validator,
                               IMapper mapper)
        {
            _configuration = configuration;
            _userService = userService;
            _jwtSettings = jwtSettings;
            _appSettings = appSettings;
            _validator = validator;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("generatetoken")]
        public async Task<IActionResult> Post([FromBody] UserInfoDto userInfoDto)
        {
            var result = await Task.FromResult(_validator.Validate(userInfoDto));
            if (!result.IsValid)
            {
                string errorMessage = string.Empty;
                List<ErrorObject> errors = new List<ErrorObject>();

                //Looping through to give us the errors
                var validationErrors = result.Errors
                                             .Select(err => new { Name = err.PropertyName, Message = err.ErrorMessage});

                errors = validationErrors.Select(p => new ErrorObject {
                    ErrorPropertyName = p.Name, ErrorMessage = p.Message
                }).ToList();
              
                ErrorResponse response = new ErrorResponse()
                {
                    Code = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Bad Request",
                    ErrorMessage = errors.Select(msg => new ErrorObject() { ErrorMessage = msg.ErrorMessage}).ToList()
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            
                var userInfo = _mapper.Map<UserInfo>(userInfoDto);
                var user = AuthenticateUser(userInfo);
                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[]{
                        new Claim(JwtRegisteredClaimNames.Sub, _appSettings.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("UserId", user.UserId.ToString()),
                            new Claim("DisplayName", user.DisplayName),
                            new Claim("UserName", user.UserName),
                            new Claim("Email", user.Email)
                    };

                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var tokenGenerated = new JwtSecurityToken(
                        _appSettings.Issuer,
                        _appSettings.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                    );
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
                    TokenResponse response = new TokenResponse() {
                        Code = "00",
                        Message = "This transaction is successful",
                        Token = token
                    };
                    return Ok(response);
                }
                else
                {
                    Response response = new Response()
                    {
                        Code = "01",
                        Message = "Invalid credentials",
                    };
                    return BadRequest(response);
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