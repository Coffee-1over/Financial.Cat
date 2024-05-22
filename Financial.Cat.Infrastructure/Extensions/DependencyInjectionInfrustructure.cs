using Microsoft.Extensions.DependencyInjection;

namespace Financial.Cat.Infrastructure.Extensions
{
	public static class DependencyInjectionInfrustructure
	{
		public static IServiceCollection AddInfrustructureMediator(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjectionInfrustructure).Assembly));
			return services;
		}
	}
}
