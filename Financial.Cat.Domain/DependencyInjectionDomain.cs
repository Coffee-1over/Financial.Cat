using Microsoft.Extensions.DependencyInjection;

namespace Financial.Cat.Domain
{
	public static class DependencyInjectionDomain
	{
		public static IServiceCollection AddDomainMediator(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjectionDomain).Assembly));
			return services;
		}
	}
}
