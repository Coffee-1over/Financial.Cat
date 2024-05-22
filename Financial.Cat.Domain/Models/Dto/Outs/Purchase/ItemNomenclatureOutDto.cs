using Financial.Cat.Domain.Models.Commands.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Dto.Out.Purchase
{
	public class ItemNomenclatureOutDto
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public CategoryOutDto Category { get; set; }
		public bool IsHidden { get; set; }
	}
}
