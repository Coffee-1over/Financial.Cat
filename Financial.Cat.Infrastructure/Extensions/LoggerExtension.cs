using Financial.Cat.Infrustructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Financial.Cat.Infrastructure.Extensions
{
	public static class LoggerExtension
	{
		public static ILoggingBuilder AddDbLogger(
		   this ILoggingBuilder builder)
		{

			builder.Services.TryAddEnumerable(
				ServiceDescriptor.Singleton<ILoggerProvider, DbLoggerProvider>());

			return builder;
		}

		public static ILoggingBuilder AddDbLoggerWithConfig(
			this ILoggingBuilder builder)
		{
			builder.AddDbLogger();

			return builder;
		}
	}
}
