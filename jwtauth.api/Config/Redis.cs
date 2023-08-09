using System;
namespace jwtauth.api.Config
{
	public class Redis
	{
		public string RedisCacheUrl { get; set; } = string.Empty;
		public int ExpirationTime { get; set; }

    }
}

