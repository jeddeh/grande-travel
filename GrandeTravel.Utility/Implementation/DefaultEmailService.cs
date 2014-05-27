using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace GrandeTravel.Utility.Implementation
{
    internal class DefaultEmailService : IEmailService
    {
        // Fields
        private readonly DefaultEmailAuthentication authentication;

        // Constructor
        internal DefaultEmailService(DefaultEmailAuthentication authentication)
        {
            this.authentication = authentication;
        }

        // Methods
        public Task SendEmail(Email email)
        {
            return Task.Run(() =>
                {
                    try
                    {
                        MailMessage mail = new MailMessage();
                        mail.To.Add(email.To);
                        mail.From = new MailAddress(email.From);
                        mail.Subject = email.Subject;
                        string Body = email.Body;
                        mail.Body = Body;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = authentication.Host;
                        smtp.Port = authentication.Port;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials =
                            new System.Net.NetworkCredential(authentication.UserName, authentication.Password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                    catch
                    {
                    }
                }
            );
        }
    }
}
