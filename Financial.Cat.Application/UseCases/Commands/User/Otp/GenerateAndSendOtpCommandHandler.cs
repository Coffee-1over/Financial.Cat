using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Business.AuthModels.Otp;
using Financial.Cat.Domain.Models.Business.Notifications;
using Financial.Cat.Domain.Models.Commands.User.Otp;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrastructure.Configs;
using Financial.Cat.Infrastructure.ExternalProviders;
using Financial.Cat.Infrastructure.Generators;
using MediatR;
using Microsoft.Extensions.Options;

namespace Financial.Cat.Application.UseCases.Commands.User.Otp
{
    internal class GenerateAndSendOtpCommandHandler : IRequestHandler<GenerateAndSendSendOtpCommand, GenerateAndSendOtpResult>
    {
        private readonly OtpConfig _otpConfig;
        private readonly OtpGenerator _otpProvider;
        private readonly IOtpRepository _otpRepository;
        private readonly EmailExternalProvider _emailExternalProvider;

        public GenerateAndSendOtpCommandHandler(OtpGenerator otpProvider, IOptions<OtpConfig> otpConfig, IOtpRepository otpRepository, EmailExternalProvider emailExternalProvider)
        {
            _otpProvider = otpProvider;
            _otpConfig = otpConfig.Value;
            _otpRepository = otpRepository;
            _emailExternalProvider = emailExternalProvider;
        }

        public async Task<GenerateAndSendOtpResult> Handle(GenerateAndSendSendOtpCommand request, CancellationToken cancellationToken)
        {
            var expInterval = _otpConfig.ExpireIntervalInMinutes;
            var expireDate = DateTime.UtcNow.AddMinutes(expInterval);
            var code = _otpProvider.GenerateOtp();
            var hashedCode = _otpProvider.HashVerificationCode(code);

            var otp = await _otpRepository.GetOneAsync(x => x.UserId == request.UserId, cancellationToken);

            if (otp != null)
                await _otpRepository.DeleteAsync(otp, cancellationToken);

            var model = new OtpEntity()
            {
                HashCode = hashedCode,
                Expired = expireDate,
                UserId = request.UserId,
            };

            otp = await _otpRepository.AddAsync(model, cancellationToken);

            await _emailExternalProvider.SendMsgAsync(new SendMsgNotification
            {
                Subject = "Код підтвердження для реєстрації у fin-cat",
                Email = request.Email,
                Message = $"Ваш код підтвердження {code}",
            }, cancellationToken);


            return new GenerateAndSendOtpResult(otp, code);
        }
    }
}
