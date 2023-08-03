using System;
namespace jwtauth.models.Response
{
	public class UserInfoResponse : Response
	{
        //public int UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

