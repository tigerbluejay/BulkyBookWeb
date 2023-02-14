using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
	public class EmailSender : IEmailSender
	{
		public string SendGridSecret { get; set; }

		public EmailSender(IConfiguration _config)
		{
			SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
		}
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			// this commented code was for sending email in one way through gmail
			// the active (uncommented) implementation is using the SendGrid service
			// which doesn't work on gmail but it works on custom domains.
			//// configure properties of email to send
			//var emailToSend = new MimeMessage();
			//emailToSend.From.Add(MailboxAddress.Parse("hello@dotnetmastery.com"));
			//emailToSend.To.Add(MailboxAddress.Parse(email));
			//emailToSend.Subject = subject;
			//emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

			//// send email
			//using (var emailClient = new SmtpClient)
			//{
			//	// connect to gmail
			//	emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
			//	// log in with email and password
			//	emailClient.Authenticate("dotnetmastery@gmail.com", "DotNet213$");
			//	// send email
			//	emailClient.Send(emailToSend);
			//	emailClient.Disconnect(true);
			//}

			//return Task.CompletedTask;

			// this is using the SendGrid Package and Service
			var client = new SendGridClient(SendGridSecret);
			var from = new EmailAddress("hello@dotnetmastery.com", "Bulky Book");
			var to = new EmailAddress(email);
			var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
			return client.SendEmailAsync(msg);


		}
	}
}
