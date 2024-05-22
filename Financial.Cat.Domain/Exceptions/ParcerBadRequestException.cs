namespace Financial.Cat.Domain.Exceptions
{
	/// <summary>
	/// Financial.Cat.Api enum bad request exception
	/// </summary>
	public class ApplicationBadRequestException : BaseApplicationException
	{
		private readonly IList<string> _auxiliaryData;

		/// <summary>
		/// Auxiliary data for format message
		/// </summary>
		public IList<string> AuxiliaryData { get { return _auxiliaryData; } }

		/// <summary>
		/// Constructor exception
		/// </summary>
		///  <param name="message">Exception message</param>
		public ApplicationBadRequestException(string message) : base(message)
		{
			_auxiliaryData = Array.Empty<string>();
		}

		/// <summary>
		/// Constructor exception
		/// </summary>
		///  <param name="message">Exception message</param>
		/// <param name="auxiliaryData">Auxiliary data</param>
		public ApplicationBadRequestException(string message, params string[] auxiliaryData) : base(message)
		{
			_auxiliaryData = auxiliaryData.ToList();
		}

		/// <summary>
		/// Constructor exception
		/// </summary>
		///  <param name="message">Exception message</param>
		/// <param name="auxiliaryData">Auxiliary data</param>
		/// <param name="innerException">Other exception</param>
		public ApplicationBadRequestException(string message, Exception innerException, params string[] auxiliaryData)
			: base(message, innerException)
		{
			_auxiliaryData = auxiliaryData.ToList();
		}

	}
}
