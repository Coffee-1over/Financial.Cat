namespace Financial.Cat.Domain.Exceptions
{
	/// <summary>
	/// Exception handled on the "Wanda" project
	/// </summary>
	public class BaseApplicationException : System.ApplicationException
	{
		/// <summary>
		/// Constructor exception
		/// </summary>
		/// <param name="message">Exception message</param>
		public BaseApplicationException(string message) : base(message)
		{ }

		/// <summary>
		/// Constructor exception
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Other exception</param>
		public BaseApplicationException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}