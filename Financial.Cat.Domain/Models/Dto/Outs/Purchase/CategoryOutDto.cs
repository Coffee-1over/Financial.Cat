using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Dto.Out.Purchase
{
	public class CategoryOutDto
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public IList<CategoryOutDto>? ChildCategories { get; set; }
		public long? ParentCategoryId { get; set; }
	}
}
