using AutoMapper;
using Financial.Cat.Application.Accessors;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Dto.Out.UserSetting;
using Financial.Cat.Domain.Models.Queries.UserSettings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Application.UseCases.Queries.UserSettings
{
	public class GetUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, IList<UserSettingOutDto>>
	{
		private readonly ISettingLimitRepository _settingLimitRepository;
		private readonly IUserContextAccessor _userContextAccessor;
		private readonly IMapper _mapper;

		public GetUserSettingsQueryHandler(ISettingLimitRepository settingLimitRepository, IUserContextAccessor userContextAccessor, IMapper mapper)
		{
			_settingLimitRepository = settingLimitRepository;
			_userContextAccessor = userContextAccessor;
			_mapper = mapper;
		}

		public async Task<IList<UserSettingOutDto>> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
		{
			var settings = await _settingLimitRepository.GetAsync(sl => sl.UserId == _userContextAccessor.UserContextModel.Id, cancellationToken);
			return _mapper.Map<IList<UserSettingOutDto>>(settings);
		}
	}
}
