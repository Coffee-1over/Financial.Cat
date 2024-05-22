using Financial.Cat.Domain.Models.Business.AuthModels.Auth;
using Financial.Cat.Domain.Models.Business.AuthModels.Tokens;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Commands.User.LogIn
{
	public class LoginCompleteCommand : IRequest<TokensModel>
    {
        public LoginCompleteCommand(long operationId, string otpCode)
		{
			OperationId = operationId;
			OtpCode = otpCode;
		}
		public long OperationId { get; set; }
		public string OtpCode { get; set; }
	}
}
