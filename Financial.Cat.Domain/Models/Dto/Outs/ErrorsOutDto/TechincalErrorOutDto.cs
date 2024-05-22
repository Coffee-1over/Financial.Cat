namespace Financial.Cat.Domain.Models.Dto.Out.ErrorsOutDto
{
	public class TechincalErrorOutDto
	{
		public string ExceptionType { get; set; }
		public string ExceptionMessage { get; set; }
		public IList<string?> StackTraces { get; set; }
		public IList<string> InnerExceptions { get; set; } = Array.Empty<string>();
	}
}
