using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Business.AuthModels.Tokens;
using Financial.Cat.Domain.Models.Commands.User.Otp;
using Financial.Cat.Domain.Models.Commands.User.SignUp;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.Generators;
using MediatR;

namespace Financial.Cat.Application.UseCases.Commands.User.SignUp
{
    internal class SignUpCompleteCommandHandler : IRequestHandler<SignUpCompleteCommand, TokensModel>
    {
        private readonly IAuthOperationRepository _authOperationRepository;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly AccountTokenGenerator _accountTokenGenerator;
        private readonly ISettingLimitRepository _settingLimitRepository;

		public SignUpCompleteCommandHandler(IAuthOperationRepository authOperationRepository,
									  IMediator mediator,
									  IUserRepository userRepository,
									  AccountTokenGenerator accountTokenGenerator,
									  ISettingLimitRepository settingLimitRepository)
		{
			_authOperationRepository = authOperationRepository;
			_mediator = mediator;
			_userRepository = userRepository;
			_accountTokenGenerator = accountTokenGenerator;
			_settingLimitRepository = settingLimitRepository;
		}

		public async Task<TokensModel> Handle(SignUpCompleteCommand request, CancellationToken cancellationToken)
        {
            var operationModel = await _authOperationRepository.GetOneAsync(entity => entity.Id == request.OperationId && entity.OperationStatus == OperationStatusType.Init && entity.AuthType == AuthType.SignUp, cancellationToken);

            if (operationModel == null)
                throw new ApplicationNotFoundException("Операція підтвердження реєстрації не знайдена");

            await _mediator.Send(new ValidateOtpCodeCommand(operationModel.UserId, request.OtpCode), cancellationToken);

            operationModel!.User.EmailVerified = !string.IsNullOrEmpty(operationModel.User.Email);

            await _userRepository.UpdateAsync(operationModel.User, cancellationToken);

            var accessToken = _accountTokenGenerator.GenerateAccessToken(operationModel.User);

            operationModel.OperationStatus = OperationStatusType.Complete;

            await _authOperationRepository.UpdateAsync(operationModel, cancellationToken);

            var listSettings = new List<SettingLimitEntity>
            {
				new SettingLimitEntity
				{
					UserId = operationModel.User.Id,
					PeriodType = PeriodType.Day,
					Limit = 0,
					IsActive = false,
					Created = DateTime.UtcNow,
					Updated = DateTime.UtcNow
				},
				new SettingLimitEntity
				{
					UserId = operationModel.User.Id,
					PeriodType = PeriodType.Week,
					Limit = 0,
					IsActive = false,
					Created = DateTime.UtcNow,
					Updated = DateTime.UtcNow
				},
				new SettingLimitEntity
				{
					UserId = operationModel.User.Id,
					PeriodType = PeriodType.Month,
					Limit = 0,
					IsActive = false,
					Created = DateTime.UtcNow,
					Updated = DateTime.UtcNow
				}
			};

			await _settingLimitRepository.AddRangeAsync(listSettings, cancellationToken);

			return new TokensModel
            {
                AccessToken = accessToken,
                RefreshToken = "",
            };
        }
    }
}
