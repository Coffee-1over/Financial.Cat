namespace Financial.Cat.Domain.Models.Business.AuthModels.Context
{
	public class UserContextModel
	{
		/// <summary>
		/// Identificator of user
		/// </summary>
		public long Id { get; init; }

		/// <summary>
		/// user email
		/// </summary>
		public string Email { get; init; }
	}
}
