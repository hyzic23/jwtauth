using System;
namespace jwtauth.api.Config
{
	public class Jwt
	{
		public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
    }
}

