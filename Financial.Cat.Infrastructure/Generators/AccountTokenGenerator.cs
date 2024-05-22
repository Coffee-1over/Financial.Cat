using Financial.Cat.Domain.Models.Business.AuthModels.Tokens;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Financial.Cat.Infrustructure.Generators
{
    public class AccountTokenGenerator
	{
		private readonly JwtConfig _jwtConfig;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logger">Logger</param>
		/// <param name="configuration">Total configuration</param>
		/// <param name="jwtConfig">JWT configuration</param>
		public AccountTokenGenerator(
			IOptions<JwtConfig> jwtConfig)
		{
			_jwtConfig = jwtConfig.Value;
		}

		/// <summary>
		/// Generate jwt
		/// </summary>
		/// <param name="user">User data for token</param>
		/// <returns>JWT</returns>
		public string GenerateAccessToken(UserEntity user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
		{
			new (nameof(ClaimTypes.NameIdentifier), user.Id.ToString()),
			new (nameof(ClaimTypes.Role),/* user.Level.ToString()*/ "Two"),
		};
			if (!string.IsNullOrEmpty(user.Email))
				claims.Add(new(nameof(ClaimTypes.Email), user.Email));


			var token = new JwtSecurityToken(_jwtConfig.Issuer,
				_jwtConfig.Issuer,
				claims,
				expires: DateTime.UtcNow.AddTicks(_jwtConfig.ExpireInterval.Ticks),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string HashRefreshToken(string refreshToken)
		{
			var encoding = new ASCIIEncoding();

			var secretKeyBytes = encoding.GetBytes(_jwtConfig.RefreshTokenSecret);

			string hashString = string.Empty;

			using (var hmacsha256 = new HMACSHA256(secretKeyBytes))
			{
				hmacsha256.ComputeHash(encoding.GetBytes(refreshToken));
				//Return the corresponding string for the signature request.
				hashString = hmacsha256.Hash.Aggregate(string.Empty, (current, b) => current + b.ToString("x2")).ToLower();
			}

			return hashString;
		}

		public RefreshTokenModel GenerateRefreshToken()
		{
			var secureRandomBytes = new byte[32];

			using var randomNumberGenerator = RandomNumberGenerator.Create();

			randomNumberGenerator.GetBytes(secureRandomBytes);

			var refreshToken = Convert.ToBase64String(secureRandomBytes);
			return new RefreshTokenModel { RefreshToken = refreshToken, RefreshTokenExpireInterval = _jwtConfig.RefreshTokenExpireInterval };
		}
	}
}