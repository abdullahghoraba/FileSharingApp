using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FileSharingApp.Helper.Email
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _config;

        public MailHelper(IConfiguration configuration)
        {
            this._config = configuration;
        }
        public void SendEmail(InputEmailMessage model)
        {
            using (SmtpClient Client = new SmtpClient(_config.GetValue<string>("Mail:Host"),
                _config.GetValue<int>("Mail:Port")))
            {
                try
                {
                    Client.Credentials = new NetworkCredential(
                          _config.GetValue<string>("Mail:From"),
                        _config.GetValue<string>("Mail:PWD"));
                    var mailMsg = new MailMessage();
                    mailMsg.To.Add(model.Email);
                    mailMsg.Subject = model.Subject;
                    mailMsg.Body = model.Body;
                    mailMsg.From = new MailAddress(
                        _config.GetValue<string>("Mail:From"),
                        _config.GetValue<string>("Mail:Sender"),
                        System.Text.Encoding.UTF8);
                    Client.SendAsync(mailMsg,"");
                }
                catch (Exception ex)
                {


                }

            }


        }
        //private MimeMessage CreateEmailMessage(InputEmailMessage message)
        //{
        //    var emailMessage = new MimeMessage();
        //    emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
        //    emailMessage.To.AddRange(message.To);
        //    emailMessage.Subject = message.Subject;
        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        //    return emailMessage;
        //}
    }
}
