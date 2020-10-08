using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraemSheppardOnline.Models;
using MailKit.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace GraemSheppardOnline.Code
{
    public class EmailManager
    {

        private static SmtpClient _client;
        private static EmailManager _instance;

        private static readonly object _lock = new object();



        public static EmailManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {

                        _instance = new EmailManager();
                        _client = new SmtpClient();
                         
                    }
                }
            }

            return _instance;
        } 

        public async void SendEmail(EmailMessage message)
        {
            try
            {
                await _client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                var config = ConfigManager.GetInstance();
                var username = config.GetValue<string>("Emailer:Username");
                var password = config.GetValue<string>("Emailer:Password");
                await _client.AuthenticateAsync(username, password);

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("Website", "contact@graemsheppard.com"));
                mimeMessage.Subject = message.Subject;
                mimeMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<strong>From:</strong> {message.Name ?? "Anonymous"},<br />" +
                           $"<strong>Email:</strong> {message.Email ?? "N/A"},<br />" +
                           $"<hr />" +
                           $"<p>{message.Body}</p>"
                };
                mimeMessage.To.Add(new MailboxAddress("", "contact@graemsheppard.com"));
                await _client.SendAsync(mimeMessage);
                await _client.DisconnectAsync(true);
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            

            
        }
    }
}
  