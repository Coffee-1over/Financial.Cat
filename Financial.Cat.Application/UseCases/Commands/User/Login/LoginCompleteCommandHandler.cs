using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Business.AuthModels.Tokens;
using Financial.Cat.Domain.Models.Commands.User.LogIn;
using Financial.Cat.Domain.Models.Commands.User.Otp;
using Financial.Cat.Infrustructure.Generators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Application.UseCases.Commands.User.Login
{
    internal class LoginCompleteCommandHandler : IRequestHandler<LoginCompleteCommand, TokensModel>
    {
        private readonly IAuthOperationRepository _authOperationRepository;
        private readonly IMediator _mediator;
        private readonly AccountTokenGenerator _accountTokenGenerator;
        public LoginCompleteCommandHandler(IAuthOperationRepository authOperationRepository, IMediator mediator, AccountTokenGenerator accountTokenGenerator)
        {
            _authOperationRepository = authOperationRepository;
            _mediator = mediator;
            _accountTokenGenerator = accountTokenGenerator;
        }

        public async Task<TokensModel> Handle(LoginCompleteCommand request, CancellationToken cancellationToken)
        {
            var operationModel = await _authOperationRepository.GetOneAsync(entity => entity.Id == request.OperationId && entity.OperationStatus == OperationStatusType.Init && entity.AuthType == AuthType.Login, cancellationToken);

            if (operationModel == null)
                throw new ApplicationNotFoundException("Операція підтвердження входу не знайдена");

            await _mediator.Send(new ValidateOtpCodeCommand(operationModel.UserId, request.OtpCode), cancellationToken);

            var accessToken = _accountTokenGenerator.GenerateAccessToken(operationModel.User);

            operationModel.OperationStatus = OperationStatusType.Complete;

            await _authOperationRepository.UpdateAsync(operationModel, cancellationToken);

            return new TokensModel
            {
                AccessToken = accessToken,
                RefreshToken = "",
            };
        }
    }
}
