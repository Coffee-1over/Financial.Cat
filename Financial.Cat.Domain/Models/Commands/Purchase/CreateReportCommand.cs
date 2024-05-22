using Financial.Cat.Domain.Models.Business.History;
using Financial.Cat.Domain.Models.Business.Reports;
using MediatR;

namespace Financial.Cat.Domain.Models.Commands.Purchase
{
	public class CreateReportCommand : BaseHistoryFilterModel, IRequest<FileModel>
	{
	}
}
