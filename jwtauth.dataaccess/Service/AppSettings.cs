using System;
using jwtauth.dataaccess.IService;
using Microsoft.Extensions.Configuration;

namespace jwtauth.dataaccess.Service
{
	public class AppSettings : IAppSettings
	{
        private readonly IConfiguration _configuration;

		public AppSettings(IConfiguration configuration)
		{
            _configuration = configuration;
		}

        public string dbConnect
        {
            get { return _configuration["ConnectionStrings:dbConnect"]; }
        }

        public string Key
        {
            get{ return _configuration["Jwt:Key"]; }
        }

        public string Issuer
        {
            get { return _configuration["Jwt:Issuer"]; }
        }

        public string Audience
        {  
            get { return _configuration["Jwt:Audience"]; }
        }

        public string Subject
        {
            get { return _configuration["Jwt:Subject"]; }
        }

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return _configuration.GetSection(key);
        }

        public string GetConnectionString(string connectionName)
        {
            return _configuration.GetConnectionString(connectionName);
        }
    }
}

