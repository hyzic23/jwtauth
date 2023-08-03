using System;
namespace jwtauth.models.Response
{
	public class TokenResponse : Response
	{
		public string Token { get; set; } = string.Empty;
	}
}

