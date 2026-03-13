using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("georgeazar456@gmail.com", "wdta rhvr npke gvea")
            };

            return client.SendMailAsync(
                new MailMessage(from: "georgeazar456@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                                     { IsBodyHtml = true}
                                );
        }
    }
}
