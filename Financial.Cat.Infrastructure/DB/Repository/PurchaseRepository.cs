using AutoMapper;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.DB.Contexts;
using Financial.Cat.Infrustructure.DB.Repository.Abstract;
using Microsoft.Extensions.Logging;

namespace Financial.Cat.Infrastructure.DB.Repository
{
	public class PurchaseRepository : BaseRepository<PurchaseEntity>, IPurchaseRepository
	{
		public PurchaseRepository(ApplicationContext context, ILogger<PurchaseRepository> logger, IMapper mapper)
		: base(context, logger, mapper)
		{ }
	}
}
