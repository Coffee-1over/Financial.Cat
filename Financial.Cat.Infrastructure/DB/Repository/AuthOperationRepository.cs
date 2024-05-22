using AutoMapper;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.DB.Contexts;
using Financial.Cat.Infrustructure.DB.Repository.Abstract;
using Microsoft.Extensions.Logging;

namespace Financial.Cat.Infrustructure.DB.Repository
{
	public class AuthOperationRepository :
		BaseRepository<AuthOperationEntity>,
		IAuthOperationRepository
	{
		public AuthOperationRepository(ApplicationContext context, ILogger<AuthOperationRepository> logger, IMapper mapper)
		: base(context, logger, mapper)
		{ }
	}
}
