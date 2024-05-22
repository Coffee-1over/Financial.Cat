using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Commands.User.Otp
{
    public record ValidateOtpCodeCommand(long UserId, string OtpCode) : IRequest<bool>
    {
    }
}
