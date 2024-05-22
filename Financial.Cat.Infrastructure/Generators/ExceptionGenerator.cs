using Financial.Cat.Domain.Constants;
using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Models.Dto.Out.Abstract;
using Financial.Cat.Domain.Models.Dto.Out.ErrorsOutDto;
using Microsoft.Extensions.Logging;

namespace Financial.Cat.Infrustructure.Generators
{
	public class ExceptionGenerator
	{
		private readonly ILogger<ExceptionGenerator> _logger;
		public ExceptionGenerator(ILogger<ExceptionGenerator> logger)
		{
			_logger = logger;
		}

		public BaseOut<bool?> GenerateBaseOutDtoWithError(Exception ex)
		{
			var errorOutDto = GenerateErrorOutDto(ex);
			return new BaseOut<bool?>(errorOutDto);
		}

		public BaseOut<bool?> GenerateBaseOutDtoWithError(ApplicationBadRequestException ex)
		{
			var errorOutDto = GenerateErrorOutDto(ex, ex.AuxiliaryData);
			return new BaseOut<bool?>(errorOutDto);
		}

		public ErrorOutDto GenerateErrorOutDto(Exception ex, IList<string>? auxiliaryData = null)
		{
			string logErrorMsg = $"{ex.GetType()}: {ex} \n\n {ex.Message}";
			_logger.LogError(logErrorMsg);

			string? message = null;


			if (!string.IsNullOrEmpty(message) && auxiliaryData != null && auxiliaryData.Count != 0)
				message = string.Format(message, auxiliaryData.ToArray());

			return new ErrorOutDto
			{
				Message = message ?? ErrorConstants.MESSAGE,
				TechincalError = GenerateTechincalErrorOutDto(ex),
			};
		}

		public TechincalErrorOutDto? GenerateTechincalErrorOutDto(Exception ex)
		{
			var innerException = ex;

			if (innerException == null)
				return null;

			var innerExceptions = new List<string>();
			var stackTraces = new List<string?> { ex.StackTrace };

			for (int i = 0; i < ErrorConstants.COUNT_INNER_EXCEPTION && innerException != null; i++)
			{
				innerExceptions.Add(innerException.Message);
				stackTraces.Add(innerException.StackTrace);
				innerException = innerException.InnerException;
			}


			return new TechincalErrorOutDto
			{
				StackTraces = stackTraces,
				ExceptionMessage = ex.Message,
				ExceptionType = ex.GetType().Name,
				InnerExceptions = innerExceptions,
			};
		}
	}
}
