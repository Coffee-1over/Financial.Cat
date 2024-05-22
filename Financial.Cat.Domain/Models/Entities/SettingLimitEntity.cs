using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Entities
{
	public class SettingLimitEntity : IBaseEntity, IAuditDateInfo
	{
		public long Id { get; set; }

		public long UserId { get; set; }
		public UserEntity User { get; set; }

		public decimal Limit { get; set; }
		public PeriodType PeriodType { get; set; }

		public bool IsActive { get; set; }

		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
	}
}
