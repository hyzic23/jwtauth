using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using jwtauth.dataaccess.IService;
using jwtauth.models;
using Microsoft.EntityFrameworkCore;

namespace jwtauth.api.Controllers
{
    [Authorize]
    [Route("api/user/")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService _service;

        public UserInfoController(IUserInfoService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<List<UserInfo>> GetAllUsers()
        {
            var users = await Task.FromResult(_service.GetUserInfos());
            return users;
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