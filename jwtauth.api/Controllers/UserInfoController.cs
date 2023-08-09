using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using jwtauth.dataaccess.IService;
using jwtauth.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Newtonsoft.Json;
using jwtauth.api.Config;

namespace jwtauth.api.Controllers
{
    [Authorize]
    [Route("api/user/")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService _service;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;

        public UserInfoController(IUserInfoService service,
                                  IDistributedCache cache,
                                  IConfiguration configuration)
        {
            _service = service;
            _cache = cache;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<List<UserInfo>>> GetAllUsers()
        {
            var cacheKey = "GET_ALL_USERS";
            var users = new List<UserInfo>();
          
            //Get data from cache
            var cacheData = await _cache.GetAsync(cacheKey);
            if (cacheData != null)
            {
                //If data found in cache, encode and deserialize cached data
                var cachedDataString = Encoding.UTF8.GetString(cacheData);
                users = JsonConvert.DeserializeObject<List<UserInfo>>(cachedDataString);
            }
            else
            {
                //If not found, then fetch from database
                users = await Task.FromResult(_service.GetUserInfos());

                //Serialize data
                var cachedDataString = JsonConvert.SerializeObject(users);
                var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                //set cache options
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:ExpirationTime"])))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                //Add data in cache
                await _cache.SetAsync(cacheKey, newDataToCache, options);

            }
            return Ok(users);
        }

        [HttpGet]
        [Route("GetAllUser/{id}")]
        public async Task<ActionResult> GetAllUserInfo(int id)
        {
            var user = await Task.FromResult(_service.GetUserInfoDetails(id));
            if (user == null)
            {
                return NotFound();
            }  
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<UserInfo>> AddUser(UserInfo request)
        {
            if(request != null)
            {
                _service.AddUserInfo(request);
                return await Task.FromResult(request);
            }
            else
            {
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPut]
        [Route("UpdateUser/{id}")]
        public async Task<ActionResult<UserInfo>> UpdateUser(int id, UserInfo user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            try
            {
                _service.UpdateUserInfo(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return await Task.FromResult(user);
        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<ActionResult<UserInfo>> Delete(int id)
        {
            var user = _service.DeleteUserInfo(id);
            return await Task.FromResult(user);
        }

        private bool UserInfoExists(int id)
        {
            return _service.CheckUserInfoExist(id);
        }

    }
}