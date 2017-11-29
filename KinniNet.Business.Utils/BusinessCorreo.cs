using System;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using MailMessage = System.Net.Mail.MailMessage;

namespace KinniNet.Business.Utils
{
    public class BusinessCorreo
    {
        public static bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static void SendMail(string addressTo, string subject, string content)
        {
            try
            {
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                MailAddress fromAddress = new MailAddress(section.From, "Eduardo Cerritos");
                MailAddress toAddress = new MailAddress(addressTo, "Prueba");

                var smtp = new SmtpClient
                {
                    Host = section.Network.Host,//"smtp.gmail.com",
                    Port = section.Network.Port,
                    EnableSsl = section.Network.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = section.Network.DefaultCredentials,
                    Credentials = new NetworkCredential(fromAddress.Address, section.Network.Password)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = content
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
