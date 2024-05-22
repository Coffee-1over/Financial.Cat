using Financial.Cat.Domain.Models.Dto.Out.Abstract;
using Financial.Cat.Domain.Models.Dto.Out.ErrorsOutDto;
using Financial.Cat.Infrustructure.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Financial.Cat.Api.FluentValidators.FluentValidatorsResponses
{
	public static class CustomProblemDetails
	{
		public static BaseOut<bool?> ResponseObj { get; set; }
		public static IActionResult MakeValidationResponse(ActionContext context)
		{

			var validationErrorList = new List<ValidationErrorOutDto>();
			foreach (var keyModelStatePair in context.ModelState)
			{
				var errors = keyModelStatePair.Value.Errors;
				if (errors != null && errors.Count > 0)
				{
					if (errors.Count == 1)
					{
						var errorMessage = ResourceProvider.GetErrorMessage(errors[0]);

						validationErrorList.Add(new ValidationErrorOutDto { Message = errorMessage, Property = keyModelStatePair.Key });
					}
					else
					{
						for (var i = 0; i < errors.Count; i++)
						{
							var errorMessage = ResourceProvider.GetErrorMessage(errors[i]);
							validationErrorList.Add(new ValidationErrorOutDto { Message = errorMessage, Property = keyModelStatePair.Key });
						}
					}
				}
			}
			var errorOutDto = new ErrorOutDto
			{
				Validation = validationErrorList
			};

			ResponseObj = new(errorOutDto);
			var result = new BadRequestObjectResult(ResponseObj);

			result.ContentTypes.Add("application/problem+json");

			return result;
		}

	}
}
