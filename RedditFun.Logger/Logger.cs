using log4net;
using log4net.Config;
using System.Reflection;

namespace RedditFun
{
	public interface ILogger
	{
		void Debug(string message);
		void Info(string message);
		void Error(string message, Exception? ex = null);
	}

	public sealed class Logger : ILogger
	{
		private static ILog _logger;

		public static ILog Logs
		{
			get
			{
				if (_logger == null)
				{
					// config file stuff here:
					var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
					XmlConfigurator.Configure(logRepository, new FileInfo("log4netconfig.config"));

					_logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
				}

				return _logger;
			}
		}

		public void Debug(string message)
		{
			_logger.Debug(message);
		}
		public void Info(string message)
		{
			_logger.Info(message);
		}
		public void Error(string message, Exception? ex = null)
		{
			_logger.Error(message, ex?.InnerException);
		}
	}
}
