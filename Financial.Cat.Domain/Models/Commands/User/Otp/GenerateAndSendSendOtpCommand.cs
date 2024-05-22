using Financial.Cat.Domain.Models.Business.AuthModels.Otp;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Commands.User.Otp
{
    public record GenerateAndSendSendOtpCommand(long UserId, string Email) : IRequest<GenerateAndSendOtpResult>
    {
    }
}
