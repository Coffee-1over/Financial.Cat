using SendGrid.Helpers.Mail;

namespace Financial.Cat.Domain.Models.Business.Notifications
{
	public class SendMsgNotification
	{
		public SendMsgNotification(string email,string subject, string message, string htmlMessage)
		{
			Email = email;
			Subject = subject;
			Message = message;
			HtmlMessage = htmlMessage;
		}

		public SendMsgNotification(string email, string subject, string message, string htmlMessage, params Attachment[] attachments)
		{
			Email = email;
			Subject = subject;
			Message = message;
			HtmlMessage = htmlMessage;
			Attachments = attachments;
		}
		public SendMsgNotification()
		{
		}


		public string Email { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public string HtmlMessage { get; set; }

		public IList<Attachment> Attachments { get; set; } = new List<Attachment>();
	}
}
