namespace Financial.Cat.Infrustructure.Configs
{
	/// <summary>
	/// JWT configuration
	/// </summary>
	public class JwtConfig
	{
		/// <summary>
		/// Token
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Who issued
		/// </summary>
		public string Issuer { get; set; }
		/// <summary>
		/// refresh token secret
		/// </summary>
		public string RefreshTokenSecret { get; set; }

		/// <summary>
		/// Lifetime of token
		/// </summary>
		public TimeSpan ExpireInterval { get; set; }

		/// <summary>
		/// Lifetime refresh token
		/// </summary>
		public TimeSpan RefreshTokenExpireInterval { get; set; }
	}
}