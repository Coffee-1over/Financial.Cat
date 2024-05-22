using Financial.Cat.Domain.Models.Dto.In.AuthInDto;
using Financial.Cat.Infrustructure.Extensions;
using FluentValidation;

namespace Financial.Cat.Api.FluentValidators.Auth
{
	/// <summary>
	/// Class for Fluent validation for loging user
	/// </summary>
	public class LoginUserByPhoneFluentValidator : AbstractValidator<LoginUserByPhoneInDto>
	{
		/// <summary>
		/// Fluent validation for loging user
		/// </summary>
		public LoginUserByPhoneFluentValidator()
		{
			RuleFor(x => x.Phone)
				.PhoneNumber()
				.When(user => !string.IsNullOrEmpty(user.Phone));

			RuleFor(x => x.Password)
				.NotNull()
				.NotEmpty()
				.Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
		}
	}
}