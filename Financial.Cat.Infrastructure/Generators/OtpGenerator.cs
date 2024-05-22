using Financial.Cat.Infrastructure.Configs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Financial.Cat.Infrastructure.Generators
{
	public class OtpGenerator
	{

		private readonly OtpConfig _otpConfig;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="otpConfig"></param>
		public OtpGenerator(IOptions<OtpConfig> otpConfig)
		{
			_otpConfig = otpConfig.Value;
		}
		/// <summary>
		/// Generate Otp code
		/// </summary>
		/// <returns>Otp code</returns>
		public string GenerateOtp()
		{
			if (_otpConfig.EnableMock)
				return "000000";

			var rnd = new Random(DateTime.UtcNow.Millisecond);
			var code = rnd.Next(100000, 999999).ToString("D6");
			return code;
		}

		/// <summary>
		/// Hashing Verification Code
		/// </summary>
		/// <param name="code">Code before hashing</param>
		/// <returns></returns>
		public string HashVerificationCode(string code)
		{
			var salt = Encoding.UTF8.GetBytes(_otpConfig.VerificationHashSalt);
			var codeBytes = Encoding.UTF8.GetBytes(code);
			using var hasher = new HMACSHA256(salt);
			return Convert.ToBase64String(hasher.ComputeHash(codeBytes));
		}
	}

}
