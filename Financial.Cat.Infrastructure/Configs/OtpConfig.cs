namespace Financial.Cat.Infrastructure.Configs
{
	public class OtpConfig
	{
		public int ExpireIntervalInMinutes { get; set; }

		public string VerificationHashSalt { get; set; }

		public bool EnableMock { get; set; }
	}
}
