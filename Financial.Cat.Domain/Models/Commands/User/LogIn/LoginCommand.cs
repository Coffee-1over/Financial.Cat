using Financial.Cat.Domain.Models.Business.AuthModels.Auth;
using Financial.Cat.Domain.Models.Business.AuthModels.Tokens;
using Financial.Cat.Domain.Models.Dto.In.AuthInDto;
using MediatR;

namespace Financial.Cat.Domain.Models.Commands.User.LogIn
{
    public class LoginCommand : LoginUserByEmailInDto, IRequest<AuthStartOperationResult>
    {
        public LoginCommand(LoginUserByEmailInDto login)
        {
            Email = login.Email;
            Password = login.Password;
        }
    }
}
