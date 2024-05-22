using Financial.Cat.Domain.Models.Dto.Out.UserSetting;
using MediatR;

namespace Financial.Cat.Domain.Models.Queries.UserSettings
{
	public class GetUserSettingsQuery : IRequest<IList<UserSettingOutDto>>
	{
	}
}
