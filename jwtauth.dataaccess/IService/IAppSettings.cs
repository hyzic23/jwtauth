using System;
using Microsoft.Extensions.Configuration;

namespace jwtauth.dataaccess.IService
{
	public interface IAppSettings
	{
		string dbConnect { get; }
		string Key { get; }
        string Issuer { get; }
        string Audience { get; }
        string Subject { get; }
        string GetConnectionString(string connectionName);
		IConfigurationSection GetConfigurationSection(string key);
    }
}

