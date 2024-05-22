using Financial.Cat.Domain.Models.Dto.In.AuthInDto;
using FluentValidation;

namespace Financial.Cat.Api.FluentValidators.Auth
{
	/// <summary>
	/// Class for Fluent validation for loging user
	/// </summary>
	public class LoginUserByEmailFluentValidator : AbstractValidator<LoginUserByEmailInDto>
	{
		/// <summary>
		/// Fluent validation for loging user
		/// </summary>
		public LoginUserByEmailFluentValidator()
		{
			RuleFor(x => x.Email)
			   .EmailAddress()
			   .When(user => !string.IsNullOrEmpty(user.Email))
			   .Matches(@"^(([^<>()[\]\\.,;:\s@']+(\.[^<>()[\]\\.,;:\s@']+)*)|('.+'))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");

			RuleFor(x => x.Password)
				.NotNull()
				.NotEmpty()
				.Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
		}
	}
}