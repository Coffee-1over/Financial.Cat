using Financial.Cat.Domain.Models.Dto.Out.ErrorsOutDto;

namespace Financial.Cat.Domain.Constants
{
	public class ErrorConstants
	{
		public const string MESSAGE = "Internal Error";
		public const int COUNT_INNER_EXCEPTION = 5;
		public static ErrorOutDto INTERNAL_ERROR_OUT_DTO => new ErrorOutDto { Message = MESSAGE };
	}
}
