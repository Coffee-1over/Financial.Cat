using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Queries.Purchase
{
	public class GetCategoryListQuery : IRequest<IList<CategoryOutDto>>
	{
		
	}
}
