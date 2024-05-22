using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Commands.User.Otp;
using Financial.Cat.Infrastructure.Generators;
using MediatR;

namespace Financial.Cat.Application.UseCases.Commands.User.Otp
{
    internal class ValidateOtpCodeCommandHandler : IRequestHandler<ValidateOtpCodeCommand, bool>
    {
        private readonly IOtpRepository _otpRepository;
        private readonly OtpGenerator _otpProvider;

        public ValidateOtpCodeCommandHandler(IOtpRepository otpRepository, OtpGenerator otpProvider)
        {
            _otpRepository = otpRepository;
            _otpProvider = otpProvider;
        }

        public async Task<bool> Handle(ValidateOtpCodeCommand request, CancellationToken cancellationToken)
        {
            var hashCode = _otpProvider.HashVerificationCode(request.OtpCode);

            var otpCodeExist = await _otpRepository.GetOneAsync(x => x.HashCode == hashCode && x.Expired >= DateTime.UtcNow, cancellationToken: cancellationToken);

            if (otpCodeExist == null)
                throw new ApplicationBadRequestException("Просрочений отп код");

            await _otpRepository.DeleteAsync(otpCodeExist, cancellationToken);
            return true;
        }
    }
}
