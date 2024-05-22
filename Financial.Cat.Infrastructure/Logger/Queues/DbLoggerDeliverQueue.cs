using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Financial.Cat.Infrustructure.Logger.Queues
{
	public class DbLoggerDeliverQueue : IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource = new();
        private readonly BlockingCollection<DbLogEntity> _logs;
        private readonly IServiceScope _scope;

        public DbLoggerDeliverQueue(IServiceProvider serviceProvider)
        {
            _scope = serviceProvider.CreateScope();
            _logs = new BlockingCollection<DbLogEntity>();
            InitLoopWorker();
        }

        public void Enqueue(DbLogEntity model)
        {
            _logs.Add(model);
        }

        private void InitLoopWorker()
        {
            Task.Factory.StartNew(async () =>
                {
                    var dbLogRepository = _scope.ServiceProvider.GetRequiredService<IDbLogRepository>();
                    foreach (var log in _logs.GetConsumingEnumerable())
                    {
                        await dbLogRepository.AddAsync(log, _cancellationTokenSource.Token);
                    }
                },
                _cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        public void Dispose()
        {
            _logs.CompleteAdding();
            _scope.Dispose();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new();
        }
    }
}