using System;
namespace jwtauth.models.Response
{
	public class ErrorResponse : Response
	{
		public List<ErrorObject> ErrorMessage { get; set; } = new List<ErrorObject>();
	}

	public class ErrorObject
	{
		public string ErrorPropertyName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

