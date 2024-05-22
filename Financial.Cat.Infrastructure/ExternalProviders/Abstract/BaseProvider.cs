using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Infrustructure.Configs.Abstract;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Financial.Cat.Infrustructure.ExternalProviders.Abstract
{
	public abstract class BaseProvider<T> where T : class
	{
		private readonly ILogger<T> _logger;
		private readonly string _baseUrl;

		public string BaseUrl { get; }

		public BaseProvider(IOptions<IProviderConfig> providerConfig, ILogger<T> logger)
		{
			_baseUrl = providerConfig.Value.ApiUrl;
			_logger = logger;
		}

		protected virtual async Task<HttpResponseMessage?> MakeRequest(HttpRequestMessage requestMessage)
		{
			using var client = new HttpClient
			{
				BaseAddress = new Uri(_baseUrl)
			};

			var response = await client.SendAsync(requestMessage);
			//var debug = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				_logger.LogError(content);
				throw new Domain.Exceptions.BaseApplicationException(content);
			}

			return response;
		}

		protected virtual HttpRequestMessage SetRequestData(HttpMethod httpMethod, string url, string? requestBody,
			IDictionary<string, string> requestHeaders, IDictionary<string, string?>? queryParameters = null, string contentType = "application/json")
		{
			var requestMessage = new HttpRequestMessage(httpMethod, url);

			if (/*!string.IsNullOrEmpty(requestBody) &&*/ httpMethod != HttpMethod.Get)
			{
				var requestContent = new StringContent(requestBody ?? "");
				requestContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
				requestMessage.Content = requestContent;
			}

			if (requestHeaders != null)
				SetRequestHeaders(ref requestMessage, requestHeaders);

			return requestMessage;
		}

		protected virtual string SetQuerryParameters(string url, IDictionary<string, string?> queryPrarameters)
			=> QueryHelpers.AddQueryString(url, queryPrarameters);

		protected virtual void SetRequestHeaders(ref HttpRequestMessage requestMessage, IDictionary<string, string> headers)
		{
			foreach (var header in headers)
				requestMessage.Headers.Add(header.Key, header.Value);
		}
	}
}
