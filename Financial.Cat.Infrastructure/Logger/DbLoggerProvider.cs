using Financial.Cat.Infrustructure.Logger.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Financial.Cat.Infrustructure.Logger
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<string, DbLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);

        public DbLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            foreach (var logger in _loggers)
            {
                logger.Value.Dispose();

			}
            _loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
            => _loggers.GetOrAdd(categoryName, name => new DbLogger(_serviceProvider.GetRequiredService<DbLoggerDeliverQueue>()));
    }
}