using Financial.Cat.Domain.Attributes;
using Financial.Cat.Infrustructure.DB.Contexts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Storage;

namespace Financial.Cat.Api.Middlewares
{
	public class TransactionMiddleware
	{
		private readonly ILogger<TransactionMiddleware> _logger;
		private readonly RequestDelegate _next;

		public TransactionMiddleware(RequestDelegate next,
			ILogger<TransactionMiddleware> logger
		)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext, ApplicationContext applicationContext)
		{
			var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
			var transactionAttribute = endpoint?.Metadata.GetMetadata<TransactionAttribute>();

			if (transactionAttribute is null)
			{
				await _next(httpContext);
				return;
			}

			IDbContextTransaction transaction = null;
			try
			{
				transaction = await applicationContext.Database.BeginTransactionAsync();

				await _next(httpContext);

				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				if (transaction != null)
					await transaction.RollbackAsync();

				throw;
			}
			finally
			{
				if (transaction != null)
					await transaction.DisposeAsync();
			}
		}
	}
}