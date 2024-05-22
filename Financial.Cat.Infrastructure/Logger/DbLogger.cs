

using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.Logger.Queues;
using Microsoft.Extensions.Logging;

namespace Financial.Cat.Infrustructure.Logger
{
    public class DbLogger : ILogger, IDisposable
    {
        private readonly DbLoggerDeliverQueue _dbLoggerDeliverQueue;

        public DbLogger(DbLoggerDeliverQueue dbLoggerDeliverQueue)
        {
            _dbLoggerDeliverQueue = dbLoggerDeliverQueue;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var logLevelInt = (int)logLevel;

            if (logLevelInt <= 3)
            {
                return;
            }

            _dbLoggerDeliverQueue.Enqueue(new DbLogEntity
			{
                Level = logLevel,
                Message = formatter(state, exception)
            });
        }

        public void Dispose()
        {
            _dbLoggerDeliverQueue.Dispose();
        }
    }
}