using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Dto.Out.Purchase
{
	public class AddressOutDto
	{
		public long? Id { get; set; }
		public string Name { get; set; }
		public string? Street1 { get; set; }
		public string? Zip { get; set; }
		public string? City { get; set; }
		public double? X { get; set; }
		public double? Y { get; set; }
		public string Country { get; set; } = null!;
	}
}
