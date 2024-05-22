using System.ComponentModel.DataAnnotations;

namespace Financial.Cat.Domain.Models.Dto.In.AuthInDto
{
	/// <summary>
	/// Login user in dto
	/// </summary>
	public class LoginUserByEmailInDto
	{
		/// <summary>
		/// User email
		/// </summary>
		[Required]
		public string Email { get; set; }

		/// <summary>
		/// User password
		/// </summary>
		[Required]
		public string Password { get; set; }
	}
}