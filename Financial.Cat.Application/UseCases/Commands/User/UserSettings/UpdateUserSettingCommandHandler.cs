using AutoMapper;
using Financial.Cat.Application.Accessors;
using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Commands.User.UserSettings;
using Financial.Cat.Domain.Models.Dto.Out.UserSetting;
using MediatR;

namespace Financial.Cat.Application.UseCases.Commands.User.UserSettings
{
	public class UpdateUserSettingCommandHandler : IRequestHandler<UpdateUserSettingCommand, UserSettingOutDto>
	{
		private readonly ISettingLimitRepository _settingLimitRepository;
		private readonly IMapper _mapper;
		private readonly IUserContextAccessor _userContextAccessor;

		public UpdateUserSettingCommandHandler(ISettingLimitRepository settingLimitRepository, IMapper mapper, IUserContextAccessor userContextAccessor)
		{
			_settingLimitRepository = settingLimitRepository;
			_mapper = mapper;
			_userContextAccessor = userContextAccessor;
		}

		public async Task<UserSettingOutDto> Handle(UpdateUserSettingCommand request, CancellationToken cancellationToken)
		{
			var setting = await _settingLimitRepository.GetOneAsync(sl => sl.UserId == _userContextAccessor.UserContextModel.Id && sl.Id == request.Id, cancellationToken);

			if (setting == null)
			{
				throw new ApplicationBadRequestException("Налаштування не знайдено.");
			}

			setting.Limit = request.Limit;
			setting.PeriodType = request.PeriodType;
			setting.IsActive = request.IsActive;

			await _settingLimitRepository.UpdateAsync(setting, cancellationToken);

			return _mapper.Map<UserSettingOutDto>(setting);
		}
	}
}
