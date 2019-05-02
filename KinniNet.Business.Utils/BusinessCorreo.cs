using System;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using MailMessage = System.Net.Mail.MailMessage;

namespace KinniNet.Business.Utils
{
    public class BusinessCorreo
    {
        static bool _invalid = false;

        public static bool IsValidEmail(string strIn)
        {
            _invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (_invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);
        }
        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                _invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public static void SendMail(string addressTo, string subject, string content)
        {
            try
            {
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                string servidor;
                int puerto;
                bool ssl;
                string usuario;
                string contraseña;
                if (smtpSection != null)
                {
                    servidor = smtpSection.Network.Host;
                    puerto = int.Parse(smtpSection.Network.TargetName);
                    ssl = smtpSection.Network.EnableSsl;
                    usuario = smtpSection.Network.UserName;
                    contraseña = smtpSection.Network.Password;

                    SmtpClient smtpClient = new SmtpClient(servidor, puerto)
                    {
                        Credentials = new NetworkCredential(usuario, contraseña),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    var message = new MailMessage(usuario, addressTo)
                    {

                        Subject = subject,
                        IsBodyHtml = true,
                        Body = content
                    };
                    message.ReplyToList.Add("noreply@kiininetcxp.com");
                    message.Headers.Add("References", "AutoReply");
                    message.Headers.Add("X-Auto-Response-Suppress", "AutoReply");
                    message.Headers.Add("Auto-submitted", "auto-replied");
                    {
                        smtpClient.Send(message);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
