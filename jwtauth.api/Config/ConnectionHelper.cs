//using System;
//using StackExchange.Redis;

//namespace jwtauth.api.Config
//{
//	public class ConnectionHelper
//	{
//		private static Lazy<ConnectionMultiplexer> lazyConnection;

//		static ConnectionHelper()
//		{
//			ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
//			{
//				return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisCacheUrl"]);
//			});
//		}
//		public static ConnectionMultiplexer Connection {
//			get {
//				return lazyConnection.Value;
//			}
//		}
//	}
//}

