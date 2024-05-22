using AutoMapper;
using Financial.Cat.Api.Controllers.Abstract;
using Financial.Cat.Domain.Attributes;
using Financial.Cat.Domain.Models.Business.AuthModels.Auth;
using Financial.Cat.Domain.Models.Business.History;
using Financial.Cat.Domain.Models.Commands.Purchase;
using Financial.Cat.Domain.Models.Commands.User.SignUp;
using Financial.Cat.Domain.Models.Dto.In.AuthInDto;
using Financial.Cat.Domain.Models.Dto.Out.Abstract;
using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Domain.Models.Queries.Purchase;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financial.Cat.Api.Controllers
{
	public class PurchaseController : BaseControllerApi
	{
		private readonly IMediator _mediator;

		public PurchaseController(ILogger<PurchaseController> logger, IMapper mapper, IMediator mediator) : base(logger, mapper)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// get purchase list
		/// </summary>
		/// <param name="data">Data for creation purchase</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(BaseOut<HistoryListModel<PurchaseOutDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetPurchaseList([FromQuery] GetPurchaseListQuery data, CancellationToken cancellationToken)
		{
			var operation = await _mediator.Send(data, cancellationToken);

			return MakeResponse(operation);
		}
		
		/// <summary>
		/// get purchase list
		/// </summary>
		/// <param name="data">Data for creation purchase</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		[HttpGet("report")]
		[ProducesResponseType(typeof(BaseOut<HistoryListModel<PurchaseOutDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetPurchaseList([FromQuery] CreateReportCommand data, CancellationToken cancellationToken)
		{
			var file = await _mediator.Send(data, cancellationToken);
			return File(file.Content, file.ContentType, file.Name);
		}

		/// <summary>
		/// Create user and hash password
		/// </summary>
		/// <param name="data">Data for creation purchase</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		[HttpPost]
		[Transaction]
		[ProducesResponseType(typeof(BaseOut<PurchaseEntity>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> CreatePurchase([FromBody] CreatePurchaseCommand data, CancellationToken cancellationToken)
		{
			var operation = await _mediator.Send(data, cancellationToken);

			return MakeResponse(operation);
		}

		/// <summary>
		/// get address list
		/// </summary>
		/// <param name="data">Data for creation purchase</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		[HttpGet("availible/addresses")]
		[ProducesResponseType(typeof(BaseOut<IList<AddressOutDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetAddressList([FromQuery] GetAddressListQuery data, CancellationToken cancellationToken)
		{
			var operation = await _mediator.Send(data, cancellationToken);

			return MakeResponse(operation);
		}

		/// <summary>
		/// get category list
		/// </summary>
		/// <param name="data">Data for creation purchase</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		[HttpGet("availible/categories")]
		[ProducesResponseType(typeof(BaseOut<IList<CategoryOutDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetCategoryList([FromQuery] GetCategoryListQuery data, CancellationToken cancellationToken)
		{
			var operation = await _mediator.Send(data, cancellationToken);

			return MakeResponse(operation);
		}

		/// <summary>
		/// get item nomenclature list
		/// </summary>
		/// <param name="data">Data for creation purchase</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		[HttpGet("availible/nomencluatures")]
		[ProducesResponseType(typeof(BaseOut<IList<ItemNomenclatureOutDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetItemNomenclatureList([FromQuery] GetItemNomenclatureListQuery data, CancellationToken cancellationToken)
		{
			var operation = await _mediator.Send(data, cancellationToken);

			return MakeResponse(operation);
		}
	}
}
