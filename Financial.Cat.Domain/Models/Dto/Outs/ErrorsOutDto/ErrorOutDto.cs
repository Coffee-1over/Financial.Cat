

namespace Financial.Cat.Domain.Models.Dto.Out.ErrorsOutDto
{
	public class ErrorOutDto
	{
		public string Message { get; set; }
		public TechincalErrorOutDto? TechincalError { get; set; }
		public IList<ValidationErrorOutDto>? Validation { get; set; }
	}
}
