using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.Generators;
using Financial.Cat.Domain.Enums;
using MediatR;
using Financial.Cat.Domain.Models.Business.AuthModels.Auth;
using Financial.Cat.Domain.Models.Commands.User.SignUp;
using Financial.Cat.Domain.Models.Commands.User.Otp;

namespace Financial.Cat.Application.UseCases.Commands.User.SignUp
{
    internal class SignUpStartCommandHandler : IRequestHandler<SignUpStartCommand, AuthStartOperationResult>
    {
        private readonly AccountPasswordGenerator _accountPasswordGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthOperationRepository _authOperationRepository;
        private readonly IMediator _mediator;

        public SignUpStartCommandHandler(AccountPasswordGenerator accountPasswordGenerator, IUserRepository userRepository, IAuthOperationRepository authOperationRepository, IMediator mediator)
        {
            _accountPasswordGenerator = accountPasswordGenerator;
            _userRepository = userRepository;
            _authOperationRepository = authOperationRepository;
            _mediator = mediator;
        }

        public async Task<AuthStartOperationResult> Handle(SignUpStartCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = _accountPasswordGenerator.HashPassword(request.Password);

            var user = await _userRepository.GetOneAsync(x => x.Email == request.Email && x.IsActive, cancellationToken);

            AuthOperationEntity? signupOperation = null;

            if (user != null)
            {
                signupOperation = await _authOperationRepository.GetOneAsync(x => x.UserId == user.Id, cancellationToken);

                if (signupOperation != null && signupOperation.OperationStatus != OperationStatusType.Init)
                    throw new ApplicationBadRequestException("Користувач вже існує");

                if (user.PasswordHash != hashedPassword)
                {
                    user.PasswordHash = hashedPassword;
                    user = await _userRepository.UpdateAsync(user, cancellationToken);
                }
            }
            else
            {
                user = await _userRepository.AddAsync(new UserEntity()
                {
                    IsActive = true,
                    PasswordHash = hashedPassword,
                    Email = request.Email
                }, cancellationToken);
            }

            var otpModel = await _mediator.Send(new GenerateAndSendSendOtpCommand(user.Id, user.Email), cancellationToken);

            signupOperation ??= await _authOperationRepository.AddAsync(new AuthOperationEntity
            {
                OperationStatus = OperationStatusType.Init,
                AuthType = AuthType.SignUp,
                UserId = user.Id,
            },
                cancellationToken);

            return new AuthStartOperationResult(signupOperation.Id, otpModel.OtpCode);
        }
    }
}
