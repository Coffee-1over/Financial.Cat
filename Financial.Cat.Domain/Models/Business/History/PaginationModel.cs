using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Business.History
{
	public class PaginationModel
	{
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}
}
