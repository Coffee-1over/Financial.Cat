using AutoMapper;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Domain.Models.Queries.Purchase;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Application.UseCases.Queries.Purchase
{
	internal class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, IList<CategoryOutDto>>
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;

		public GetCategoryListQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}

		public async Task<IList<CategoryOutDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
		{
			var categories = await _categoryRepository.GetCategoryTreeAsync();
			return _mapper.Map<IList<CategoryOutDto>>(categories);
		}

	}
}
