using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Commands.Purchase
{
	public class CalculatePeriodsAndSendEmailReportCommand : IRequest
	{
		public long UserId { get; set; }
	}
}
