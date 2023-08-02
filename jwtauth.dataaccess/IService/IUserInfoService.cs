using jwtauth.models;

namespace jwtauth.dataaccess.IService
{
	public interface IUserInfoService
	{
        public List<UserInfo> GetUserInfos();
        public UserInfo GetUserInfoDetails(int id);
        public void AddUserInfo(UserInfo userInfo);
        public void UpdateUserInfo(UserInfo userInfo);
        public UserInfo DeleteUserInfo(int id);
        public bool CheckUserInfoExist(int id);
        public UserInfo AuthenticateUser(UserInfo userInfo);
    }
}

