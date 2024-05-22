using Financial.Cat.Infrustructure.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Financial.Cat.Infrustructure.Generators
{
	public class AccountPasswordGenerator
	{
		private readonly AccountConfig _accountConfig;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logger">Logger</param>
		/// <param name="configuration">Total configuration</param>
		/// <param name="jwtConfig">JWT configuration</param>
		public AccountPasswordGenerator(
			IOptions<AccountConfig> accountConfig)
		{
			_accountConfig = accountConfig.Value;
		}

		/// <summary>
		/// Hashing password
		/// </summary>
		/// <param name="password">Password</param>
		/// <returns>Hash password</returns>
		public string HashPassword(string password)
		{
			var salt = Encoding.UTF8.GetBytes(_accountConfig.PasswordSalt);
			var passwordBytes = Encoding.UTF8.GetBytes(password);
			using var hasher = new HMACSHA256(salt);
			return Convert.ToBase64String(hasher.ComputeHash(passwordBytes));
		}
	}
}