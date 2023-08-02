using System;
using jwtauth.dataaccess.Data;
using jwtauth.dataaccess.IService;
using jwtauth.models;

namespace jwtauth.dataaccess.Service
{
	public class UserInfoService : IUserInfoService
    {
        private readonly DatabaseContext _dbContext;
		public UserInfoService(DatabaseContext dbContext)
		{
            _dbContext = dbContext;
		}

        public void AddUserInfo(UserInfo userInfo)
        {
            _dbContext.UserInfos.Add(userInfo);
            _dbContext.SaveChanges();
        }

        public bool CheckUserInfoExist(int id)
        {
            return _dbContext.UserInfos.Any(e => e.UserId == id);
        }

        public UserInfo DeleteUserInfo(int id)
        {
            UserInfo userInfo = _dbContext.UserInfos.Find(id);
            if (userInfo != null)
            {
                _dbContext.UserInfos.Remove(userInfo);
                _dbContext.SaveChanges();
                return userInfo;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public List<UserInfo> GetUserInfos()
        {
            return _dbContext.UserInfos.ToList();
        }

        public UserInfo GetUserInfoDetails(int id)
        {
            return _dbContext.UserInfos.Find(id);
        }


        public void UpdateUserInfo(UserInfo userInfo)
        {
            _dbContext.Entry(userInfo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public UserInfo AuthenticateUser(UserInfo userInfo)
        {
            UserInfo response = _dbContext.UserInfos.Where(x => x.Email == userInfo.Email &&
                                                           x.Password == userInfo.Password)
                .FirstOrDefault();
            return response;
        }
    }
}

