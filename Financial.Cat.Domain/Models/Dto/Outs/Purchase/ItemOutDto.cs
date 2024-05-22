using Financial.Cat.Domain.Models.Commands.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Dto.Out.Purchase
{
	public class ItemOutDto
	{
		public long Id { get; set; }
		public decimal Quantity { get; set; }
		public decimal Price { get; set; }
		public ItemNomenclatureOutDto ItemNomenclature { get; set; }
	}
}
