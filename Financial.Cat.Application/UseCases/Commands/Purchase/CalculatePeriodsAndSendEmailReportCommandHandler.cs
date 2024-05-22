using Financial.Cat.Application.Accessors;
using Financial.Cat.Domain.Constants;
using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Business.Notifications;
using Financial.Cat.Domain.Models.Commands.Purchase;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrastructure.ExternalProviders;
using MediatR;

namespace Financial.Cat.Application.UseCases.Commands.Purchase
{
	public class CalculatePeriodsAndSendEmailReportCommandHandler : IRequestHandler<CalculatePeriodsAndSendEmailReportCommand>
	{
		private readonly IMediator _mediator;
		private readonly EmailExternalProvider _emailExternalProvider;
		private readonly IPurchaseRepository _purchaseRepository;
		private readonly ISettingLimitRepository _settingLimitRepository;
		private readonly IUserContextAccessor _userContextAccessor;

		public CalculatePeriodsAndSendEmailReportCommandHandler(IMediator mediator,
														  EmailExternalProvider emailExternalProvider,
														  IPurchaseRepository purchaseRepository,
														  ISettingLimitRepository settingLimitRepository,
														  IUserContextAccessor userContextAccessor)
		{
			_mediator = mediator;
			_emailExternalProvider = emailExternalProvider;
			_purchaseRepository = purchaseRepository;
			_settingLimitRepository = settingLimitRepository;
			_userContextAccessor = userContextAccessor;
		}

		public async Task Handle(CalculatePeriodsAndSendEmailReportCommand request, CancellationToken cancellationToken)
		{
			var periodLimits = await GetActivePeriodLimitsAsync(request.UserId, cancellationToken);

			foreach (var limit in periodLimits)
			{
				var startDate = CalculateStartDate(limit.PeriodType);
				var totalExpenses = await CalculateTotalExpensesAsync(request.UserId, startDate, cancellationToken);

				if (totalExpenses > limit.Limit)
				{
					await GenerateAndSendReportAsync(request.UserId, startDate, cancellationToken);
				}
			}
		}

		private async Task<IList<SettingLimitEntity>> GetActivePeriodLimitsAsync(long userId, CancellationToken cancellationToken)
		{
			return await _settingLimitRepository
				.GetAsync(sl => sl.UserId == userId && sl.IsActive, cancellationToken);
		}

		private DateTime CalculateStartDate(PeriodType periodType)
		{
			var startDate = DateTime.UtcNow;
			return periodType switch
			{
				PeriodType.Day => startDate.Date,
				PeriodType.Week => startDate.AddDays(-(int)startDate.DayOfWeek),
				PeriodType.Month => new DateTime(startDate.Year, startDate.Month, 1),
				_ => throw new ArgumentOutOfRangeException(),
			};
		}

		private async Task<decimal> CalculateTotalExpensesAsync(long userId, DateTime startDate, CancellationToken cancellationToken)
		{
			var purchases = await _purchaseRepository.GetAsync(p => p.UserId == userId && p.PurchaseTime >= startDate && p.PurchaseTime <= DateTime.UtcNow, cancellationToken);
			return  purchases.SelectMany(p => p.Items).Sum(i => i.TotalPrice);
		}

		private async Task GenerateAndSendReportAsync(long userId, DateTime startDate, CancellationToken cancellationToken)
		{
			var reportCommand = new CreateReportCommand
			{
				UserId = userId,
				Start = startDate,
				End = DateTime.UtcNow,
				PageSize = 1,
				PageNumber = 1
			};
			var report = await _mediator.Send(reportCommand, cancellationToken);

			var message = $"Звіт про витрати з {startDate} по {DateTime.UtcNow}";
			var htmlMessage = $"<h1>Звіт про витрати</h1><p>Звіт про витрати з {startDate} по {DateTime.UtcNow}</p>";

			var byteData = await ConvertStreamToByteArrayAsync(report.Content);
			var emailNotification = new SendMsgNotification(_userContextAccessor.UserContextModel.Email, "Звіт про витрати", message, htmlMessage, new SendGrid.Helpers.Mail.Attachment
			{
				Content = Convert.ToBase64String(byteData),
				Filename = $"{ReportConstants.FileName}.xlsx",
				Disposition = "attachment",
				Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
			});
			await _emailExternalProvider.SendMsgAsync(emailNotification, cancellationToken);
		}

		private async Task<byte[]> ConvertStreamToByteArrayAsync(MemoryStream stream)
		{
			var resultArray = stream.ToArray();

			stream.Dispose();

			return resultArray;
		}
	}
}
