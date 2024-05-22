using Financial.Cat.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Dto.Out.UserSetting
{
	public class UserSettingOutDto
	{
		public long Id { get; set; }
		public decimal Limit { get; set; }
		public PeriodType PeriodType { get; set; }
		public bool IsActive { get; set; }
	}
}
