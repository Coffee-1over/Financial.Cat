using Financial.Cat.Domain.Constants;
using Financial.Cat.Domain.Interfaces.Services;
using Financial.Cat.Domain.Models.Business.Reports;
using Financial.Cat.Domain.Models.Commands.Purchase;
using Financial.Cat.Domain.Models.Queries.Purchase;
using MediatR;

namespace Financial.Cat.Application.UseCases.Commands.Purchase
{
	internal class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, FileModel>
	{
		private readonly IFileWriterService _fileWriterService;
		private readonly IMediator _mediator;

		public CreateReportCommandHandler(IMediator mediator, IFileWriterService fileWriterService)
		{
			_mediator = mediator;
			_fileWriterService = fileWriterService;
		}

		public async Task<FileModel> Handle(CreateReportCommand request, CancellationToken cancellationToken)
		{
			var purchases = await _mediator.Send(new GetPurchaseListQuery(request), cancellationToken);

			var reportPurchases = purchases.HistoryList.SelectMany(purchase => purchase.Items.Select(item=> new PurchaseReportModel
			{
				CategoryName = item.ItemNomenclature.Category.Name,
				ItemNomenclatureName = item.ItemNomenclature.Name,
				Price = item.Price,
				PurchaseTime = purchase.PurchaseTime,
				Quantity = item.Quantity,
				Total = item.Quantity * item.Price,
			})).ToList();

			return _fileWriterService.WriteFile(reportPurchases, Domain.Enums.FileFormat.Xlsx, ReportConstants.ColumnsNames, ReportConstants.FileName);
		}
	}
}
