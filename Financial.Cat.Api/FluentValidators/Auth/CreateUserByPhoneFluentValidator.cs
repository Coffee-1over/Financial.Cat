using Financial.Cat.Domain.Models.Dto.In.AuthInDto;
using Financial.Cat.Infrustructure.Extensions;
using FluentValidation;

namespace Financial.Cat.Api.FluentValidators.Auth
{
	/// <summary>
	/// Class for Fluent validation for creating user
	/// </summary>
	public class CreateUserFluentByPhoneValidator : AbstractValidator<CreateUserByPhoneInDto>
	{
		/// <summary>
		/// Fluent validation for creating user
		/// </summary>
		public CreateUserFluentByPhoneValidator()
		{
			RuleFor(x => x.Phone)
				.PhoneNumber();

			RuleFor(x => x.Password)
				.NotNull()
				.NotEmpty()
				.Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
		}
	}
}