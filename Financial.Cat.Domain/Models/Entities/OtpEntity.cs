using Financial.Cat.Domain.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Entities
{
	public class OtpEntity : IBaseEntity, IAuditDateInfo
	{
		public long Id { get; set; }
		public long UserId { get; set; }

		public virtual UserEntity User { get; set; } = null!;

		public string HashCode { get; set; }
		public DateTime Expired { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
	}
}
