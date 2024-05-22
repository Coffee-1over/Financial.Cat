namespace Financial.Cat.Infrustructure.Extensions
{
	public static class DateTimeExtension
	{
		public static DateTime ToDateTime(this long unixtime)
		{
			DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToUniversalTime();
			return dtDateTime;
		}

		public static long ToUnix(this DateTime MyDateTime)
		{
			TimeSpan timeSpan = MyDateTime - new DateTime(1970, 1, 1, 0, 0, 0);

			return (long)timeSpan.TotalMilliseconds;
		}
	}
}
