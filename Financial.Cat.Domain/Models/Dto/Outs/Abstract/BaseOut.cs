using Financial.Cat.Domain.Constants;
using Financial.Cat.Domain.Models.Dto.Out.ErrorsOutDto;

namespace Financial.Cat.Domain.Models.Dto.Out.Abstract
{
	/// <summary>
	/// Base response struct
	/// </summary>
	public class BaseOut<T>
	{
		/// <summary>
		/// Empty OK result
		/// </summary>
		public readonly static BaseOut<bool?> Ok = new(null);

		/// <summary>
		/// Empty ERROR result
		/// </summary>
		public readonly static BaseOut<bool?> Failed = new(ErrorConstants.INTERNAL_ERROR_OUT_DTO, null);

		/// <summary>
		/// Success flag
		/// </summary>
		public bool Success => string.IsNullOrEmpty(Error.Message);

		/// <summary>
		/// Error description
		/// </summary>
		public ErrorOutDto Error { get; set; }

		/// <summary>
		/// Default constructor
		/// </summary>
		public BaseOut()
		{
			Error = new();
			Data = default;
		}

		/// <summary>
		/// Constructor OK response
		/// </summary>
		/// <param name="data">Response Data</param>
		public BaseOut(T? data)
		{
			Error = new();
			Data = data;
		}

		/// <summary>
		/// Constructor ERROR response
		/// </summary>
		/// <param name="errorMessage">Error message</param>
		/// <param name="data">Data for error</param>
		public BaseOut(ErrorOutDto errorOutDto, T? data = default)
		{
			Error = errorOutDto;
			Data = data;
		}

		/// <summary>
		/// Response data
		/// </summary>
		public T? Data { get; set; }
	}
}