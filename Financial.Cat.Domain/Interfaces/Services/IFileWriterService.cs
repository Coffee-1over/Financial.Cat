using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Models.Business.Reports;

namespace Financial.Cat.Domain.Interfaces.Services
{
	public interface IFileWriterService
	{
		FileModel WriteFile<TRowModel>(IReadOnlyList<TRowModel> rows, FileFormat format, IList<string>? customHeaders = null, string? fileName = "File");
	}
}
