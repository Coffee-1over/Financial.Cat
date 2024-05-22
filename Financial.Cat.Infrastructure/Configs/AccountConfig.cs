namespace Financial.Cat.Infrustructure.Configs
{
	public class AccountConfig
	{
		public string PasswordSalt { get; set; }
		public string IdSalt { get; set; }
		public long ExpireIntervalInDays { get; set; }
	}
}
