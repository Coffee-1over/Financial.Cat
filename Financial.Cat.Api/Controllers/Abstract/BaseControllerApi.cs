using AutoMapper;
using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Models.Dto.Out.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financial.Cat.Api.Controllers.Abstract
{
	/// <summary>
	/// Base controller
	/// </summary>
	//[AllowAnonymous]
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public abstract class BaseControllerApi : ControllerBase
	{
		/// <summary>
		/// Automapper
		/// </summary>
		protected IMapper Mapper { get; }

		/// <summary>
		/// Logger
		/// </summary>
		protected ILogger Logger { get; }


		protected BaseControllerApi(ILogger logger, IMapper mapper)
		{
			Logger = logger;
			Mapper = mapper;
		}

		/// <summary>
		/// Do <paramref name="action"/> and return <see cref="BaseOut{TModel}"/> with operation result,
		/// convert it before to <typeparamref name="TResult"/>.
		/// </summary>
		/// <param name="action">Method of service</param>
		/// <typeparam name="TModel">Operation type</typeparam>
		/// <typeparam name="TResult">Result type</typeparam>
		/// <returns><see cref="BaseOut{TResult}"/>.</returns>
		protected async Task<IActionResult> ExecuteAndReturnResponseAsync<TModel, TResult>(Task<TModel> action)
		{
			try
			{
				var response = await action;
				return MakeResponse(Mapper.Map<TResult>(response));
			}
			catch (Domain.Exceptions.BaseApplicationException e)
			{
				return MakeResponse((TResult?)default, e.Message);
			}
			catch (Exception ex)
			{
				Logger.LogError($"Exception on call: {ex.Message} {ex.StackTrace}");
				return base.BadRequest(BaseOut<TResult>.Failed);
			}
		}

		/// <summary>
		/// Convert data to frontend response obj, if method throw exception back error message
		/// </summary>
		/// <param name="data">Response Data</param>
		/// <param name="errorMessage">Error of response</param>
		/// <typeparam name="T">Type</typeparam>
		/// <returns>Frontend response</returns>
		protected IActionResult MakeResponse<T>(T? data, string? errorMessage = null)
		{
			if (errorMessage != null)
			{
				throw new Domain.Exceptions.BaseApplicationException(errorMessage);
			}

			return Ok(new BaseOut<T>(data));
		}

		/// <summary>
		/// Success empty response
		/// </summary>
		/// <returns>Success response without data</returns>
		protected IActionResult OkResponse()
			=> Ok(BaseOut<bool?>.Ok);

	}
}