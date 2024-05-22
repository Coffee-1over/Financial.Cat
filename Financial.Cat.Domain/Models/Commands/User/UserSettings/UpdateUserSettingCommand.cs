using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Models.Dto.Out.UserSetting;
using MediatR;

namespace Financial.Cat.Domain.Models.Commands.User.UserSettings
{
	public class UpdateUserSettingCommand : IRequest<UserSettingOutDto>
	{
		public long Id { get; set; }
		public decimal Limit { get; set; }
		public PeriodType PeriodType { get; set; }
		public bool IsActive { get; set; }
	}
}
