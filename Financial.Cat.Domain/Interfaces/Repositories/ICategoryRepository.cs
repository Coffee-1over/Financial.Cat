using Financial.Cat.Domain.Interfaces.Repositories.Abstract;
using Financial.Cat.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Interfaces.Repositories
{
	public interface ICategoryRepository : IBaseRepository<CategoryEntity>
	{
		Task<List<CategoryEntity>> GetCategoryTreeAsync();
	}
}
