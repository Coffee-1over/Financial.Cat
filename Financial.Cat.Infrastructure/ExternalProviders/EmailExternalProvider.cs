using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Models.Business.Notifications;
using Financial.Cat.Infrustructure.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;

namespace Financial.Cat.Infrastructure.ExternalProviders
{
	public class EmailExternalProvider
	{
		private readonly ILogger<EmailExternalProvider> _logger;
		private readonly EmailConfig _emailConfig;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="emailConfig"></param>
		/// <param name="logger"></param>
		public EmailExternalProvider(
			IOptions<EmailConfig> emailConfig,
			ILogger<EmailExternalProvider> logger)
		{
			_logger = logger;
			_emailConfig = emailConfig.Value;
		}


		public async Task<bool> SendMsgAsync(SendMsgNotification model, CancellationToken cancellationToken)
		{
			var client = new SendGridClient(_emailConfig.ApiKey);
			var from = new EmailAddress(_emailConfig.EmailFrom, "");
			var to = new EmailAddress(model.Email, "");
			
			var msg = MailHelper.CreateSingleEmail(from, to, model.Subject, model.Message, model.HtmlMessage);

			if(model.Attachments != null && model.Attachments.Count > 0)
				msg.AddAttachments(model.Attachments);

			var response = await client.SendEmailAsync(msg, cancellationToken);

			if (response.IsSuccessStatusCode)
				return true;

			var content = await response.Body.ReadAsStringAsync(cancellationToken);
			_logger.LogError(content);
			throw new ApplicationBadRequestException("FailledSendingEmail");
		}
	}
}
