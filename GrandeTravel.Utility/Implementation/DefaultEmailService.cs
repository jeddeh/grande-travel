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
        public Task SendEmailAsync(Email email)
        {
            return Task.Run(() =>
                {
                    try
                    {
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress(email.From),
                            Subject = email.Subject,
                            Body = email.Body,
                            IsBodyHtml = true
                        };

                        mail.To.Add(email.To);

                        SmtpClient smtp = new SmtpClient
                        {
                            Host = authentication.Host,
                            Port = authentication.Port,
                            UseDefaultCredentials = false,
                            Credentials = new System.Net.NetworkCredential(authentication.UserName, authentication.Password),
                            EnableSsl = true
                        };
                      
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
