//using System;
//using jwtauth.dataaccess.IService;
//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.Extensions.Configuration;


//namespace jwtauth.dataaccess.Service
//{
//	public class CacheService : ICacheService
//	{
//        private IDatabase _db;
//        private IConfiguration _configuration;

//		public CacheService()
//		{
//            ConfigureRedis();

//        }

//        public T GetData<T>(string key)
//        {
//            throw new NotImplementedException();
//        }

//        public object RemoveData(string key)
//        {
//            throw new NotImplementedException();
//        }

//        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
//        {
//            throw new NotImplementedException();
//        }

//        private void ConfigureRedis() {
//            _db = ConnectionHelper.Connection.GetDatabase();
//        }
//    }
//}

