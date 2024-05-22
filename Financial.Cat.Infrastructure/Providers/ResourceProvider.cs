using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Financial.Cat.Infrustructure.Providers
{
	/// <summary>
	/// Resource provider
	/// </summary>
	public class ResourceProvider
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ResourceProvider()
		{
		}

		/// <summary>
		/// Read supported block chain from json
		/// </summary>
		/// <param name="code">Block chain code</param>
		/// <returns></returns>
		public static string GetErrorMessage(ModelError error)
		{
			return string.IsNullOrEmpty(error.ErrorMessage) ?
			"The input was not valid." :
			error.ErrorMessage;
		}
	}
}
