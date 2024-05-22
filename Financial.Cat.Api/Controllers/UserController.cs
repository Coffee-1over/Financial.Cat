using AutoMapper;
using Financial.Cat.Api.Controllers.Abstract;
using Financial.Cat.Domain.Attributes;
using Financial.Cat.Domain.Models.Dto.In.AuthInDto;
using Financial.Cat.Domain.Models.Dto.Out.Abstract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Financial.Cat.Domain.Models.Business.AuthModels.Auth;
using Financial.Cat.Domain.Models.Business.AuthModels.Tokens;
using Financial.Cat.Domain.Models.Commands.User.SignUp;
using Financial.Cat.Domain.Models.Commands.User.LogIn;
using Financial.Cat.Domain.Models.Queries.UserSettings;
using Financial.Cat.Domain.Models.Commands.User.UserSettings;

namespace Financial.Cat.Api.Controllers
{
	public class UserController : BaseControllerApi
	{
		private readonly IMediator _mediator;

		public UserController(ILogger<UserController> logger, IMapper mapper, IMediator mediator) : base(logger, mapper)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Create user and hash password
		/// </summary>
		/// <param name="newUserByEmail">New user</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("signUp/email")]
		[Transaction]
		[ProducesResponseType(typeof(BaseOut<AuthStartOperationResult>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> SignUpByEmail([FromBody] CreateUserByEmailInDto newUserByEmail, CancellationToken cancellationToken)
		{
			var operation = await _mediator.Send(new SignUpStartCommand(newUserByEmail), cancellationToken);

			return MakeResponse(operation);
		}

		/// <summary>
		/// Verify sign up
		/// </summary>
		/// <param name="otpCode">Verification code</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <param name="operationId">Operation Id</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("signUp/{operationId:long}/verify")]
		[Transaction]
		[ProducesResponseType(typeof(BaseOut<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> SignUpVerify(
			[FromRoute] long operationId,
			[FromQuery, BindRequired] string otpCode,
			CancellationToken cancellationToken)
		{
			var tokens = await _mediator.Send(new SignUpCompleteCommand(operationId, otpCode), cancellationToken);
			return MakeResponse(tokens);
		}

		/// <summary>
		/// Login
		/// </summary>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <param name="newUserByPhone">User from form</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("login/email")]
		[Transaction]
		[ProducesResponseType(typeof(BaseOut<TokensModel>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> LoginByEmail([FromBody] LoginUserByEmailInDto newUserByPhone, CancellationToken cancellationToken)
		{
			var loginUser = await _mediator.Send(new LoginCommand(newUserByPhone), cancellationToken);
			return MakeResponse(loginUser);
		}

		/// <summary>
		/// Verify дщпшт
		/// </summary>
		/// <param name="otpCode">Verification code</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <param name="operationId">Operation Id</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("login/{operationId:long}/verify")]
		[Transaction]
		[ProducesResponseType(typeof(BaseOut<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> LoginVerify(
			[FromRoute] long operationId,
			[FromQuery, BindRequired] string otpCode,
			CancellationToken cancellationToken)
		{
			var tokens = await _mediator.Send(new LoginCompleteCommand(operationId, otpCode), cancellationToken);
			return MakeResponse(tokens);
		}

		[HttpGet("settings")]
		public async Task<IActionResult> GetUserSettings(CancellationToken cancellationToken)
		{
			var query = new GetUserSettingsQuery();
			var settings = await _mediator.Send(query, cancellationToken);
			return Ok(settings);
		}

		[HttpPut("settings")]
		public async Task<IActionResult> UpdateUserSetting([FromBody] UpdateUserSettingCommand command, CancellationToken cancellationToken)
		{
			var updatedSetting = await _mediator.Send(command, cancellationToken);
			return Ok(updatedSetting);
		}
	}
}
