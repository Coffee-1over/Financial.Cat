using Microsoft.Extensions.DependencyInjection;
using Financial.Cat.Domain;

namespace Financial.Cat.Application.UseCases
{
	public static class DependencyInjectionApplication
	{
		public static IServiceCollection AddApplicationMediator(this IServiceCollection services)
		{
			services.AddDomainMediator();
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjectionApplication).Assembly));
			return services;
		}

	}
}
