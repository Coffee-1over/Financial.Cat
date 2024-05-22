using Financial.Cat.Domain.Models.Entities.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Entities
{
	public class DbLogEntity : IBaseEntity, IAuditDateInfo
	{
		public long Id { get; set; }

		public LogLevel Level { get; set; }

		public string Message { get; set; }

		public DateTime Created { get; set; }

		public DateTime Updated { get; set; }
	}
}
