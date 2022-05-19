using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using PDFGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _env;

        public EmailSender(
            IOptions<EmailSettings> emailSettings,
            IWebHostEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
        }

        public async Task SendEmailAsync(Message message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.Username, _emailSettings.From));

                mimeMessage.To.AddRange(message.To);

                mimeMessage.Subject = message.Subject;


                String mess = message.Content.ToString();
                mimeMessage.Body = new TextPart("html")
                {
                    Text = mess
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    if (_env.IsDevelopment())
                    {
                        // The third parameter is useSSL (true if the client should make an SSL-wrapped
                        // connection to the server; otherwise, false).
                        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, true);
                    }
                    else
                    {
                        await client.ConnectAsync(_emailSettings.SmtpServer);
                    }

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.From, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
