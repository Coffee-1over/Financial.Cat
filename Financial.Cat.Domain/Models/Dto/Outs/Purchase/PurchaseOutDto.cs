using Financial.Cat.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Dto.Out.Purchase
{
	public class PurchaseOutDto
	{
		public long Id { get; set; }

		public long? AddressId { get; set; }

		public AddressOutDto? Address { get; set; }

		public virtual IList<ItemOutDto> Items { get; set; } = new List<ItemOutDto>();

		public DateTime? PurchaseTime { get; set; }


		public DateTime Created { get; set; }

		public DateTime Updated { get; set; }
	}
}
