using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Business.AuthModels.Auth;
using Financial.Cat.Domain.Models.Business.AuthModels.Tokens;
using Financial.Cat.Domain.Models.Commands.User.LogIn;
using Financial.Cat.Domain.Models.Commands.User.Otp;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.Generators;
using MediatR;

namespace Financial.Cat.Application.UseCases.Commands.User.Login
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, AuthStartOperationResult>
    {
        private readonly IAuthOperationRepository _authOperationRepository;
        private readonly IUserRepository _userRepository;
        private readonly AccountPasswordGenerator _accountPasswordGenerator;
        private readonly AccountTokenGenerator _accountTokenGenerator;
        private readonly IMediator _mediator;

        public LoginCommandHandler(IAuthOperationRepository authOperationRepository, IUserRepository userRepository, AccountPasswordGenerator accountPasswordGenerator, AccountTokenGenerator accountTokenGenerator, IMediator mediator)
        {
            _authOperationRepository = authOperationRepository;
            _userRepository = userRepository;
            _accountPasswordGenerator = accountPasswordGenerator;
            _accountTokenGenerator = accountTokenGenerator;
            _mediator = mediator;
        }

        public async Task<AuthStartOperationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetOneAsync(x => x.Email == request.Email && x.IsActive, cancellationToken);

            if (user == null || !user.IsActive)
                throw new ApplicationNotFoundException("Користувач не знайдений");

            var hashedPassword = _accountPasswordGenerator.HashPassword(request.Password);

            if (user.PasswordHash != hashedPassword)
                throw new ApplicationBadRequestException("Невірний пароль");

            var otpModel = await _mediator.Send(new GenerateAndSendSendOtpCommand(user.Id, user.Email), cancellationToken);

            var signupOperation = await _authOperationRepository.AddAsync(new AuthOperationEntity
            {
                OperationStatus = OperationStatusType.Init,
                AuthType = AuthType.Login,
                UserId = user.Id,
            },
            cancellationToken);

            return new AuthStartOperationResult(signupOperation.Id, otpModel.OtpCode);

        }
    }
}
