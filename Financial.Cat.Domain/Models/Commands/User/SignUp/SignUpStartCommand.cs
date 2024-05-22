using Financial.Cat.Domain.Models.Business.AuthModels.Auth;
using Financial.Cat.Domain.Models.Dto.In.AuthInDto;
using MediatR;

namespace Financial.Cat.Domain.Models.Commands.User.SignUp
{
    public class SignUpStartCommand : CreateUserByEmailInDto, IRequest<AuthStartOperationResult>
    {
        public SignUpStartCommand(CreateUserByEmailInDto createUserByEmailInDto)
        {
            Email = createUserByEmailInDto.Email;
            Password = createUserByEmailInDto.Password;
        }
    }
}
