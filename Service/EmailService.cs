using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using api.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace api.Service
{
   
    public class EmailService:IEmailSender
    { 
        public IConfiguration _configuration { get; }

        public EmailService( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage){
            var mimeMsg = new MimeMessage();
            mimeMsg.From.Add(MailboxAddress.Parse(_configuration["NetMail:sender"]));
            mimeMsg.To.Add(MailboxAddress.Parse(email));
            mimeMsg.Subject = subject;

            mimeMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };
            // Console.WriteLine($"SendEmailAsync message to:{email} subject:{subject} message:{htmlMessage}");
            using (var smtp = new SmtpClient())
            {
                bool useSSL= _configuration["NetMail:smtpUseSSL"]=="true";
                // Console.WriteLine($"SmtpClient SEND {useSSL} - {_configuration["NetMail:smtpHost"]}:{int.Parse(useSSL?_configuration["NetMail:smtpPortSSL"]: _configuration["NetMail:smtpPort"])}");
                smtp.Connect(_configuration["NetMail:smtpHost"], 
                            int.Parse(useSSL?_configuration["NetMail:smtpPortSSL"]: _configuration["NetMail:smtpPort"]),
                            useSSL);
                smtp.Timeout=5000;
                smtp.Authenticate(_configuration["NetMail:sender"], _configuration["NetMail:senderpassword"]);
                // Console.WriteLine($"SmtpClient SEND 2 {_configuration["NetMail:sender"]}, {_configuration["NetMail:senderpassword"]}");
                
                await smtp.SendAsync(mimeMsg);
                smtp.Disconnect(true);
                
                // Console.WriteLine($"SmtpClient FINISHED");
                
            }
        }
    }
}