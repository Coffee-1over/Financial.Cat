using System.ComponentModel.DataAnnotations;

namespace Financial.Cat.Domain.Models.Dto.In.AuthInDto
{
	/// <summary>
	/// Create user in dto
	/// </summary>
	public class CreateUserByPhoneInDto
	{
		/// <summary>
		/// User phone
		/// </summary>
		[Required]
		public string Phone { get; set; }

		/// <summary>
		/// User password
		/// </summary>
		[Required]
		public string Password { get; set; }
	}
}