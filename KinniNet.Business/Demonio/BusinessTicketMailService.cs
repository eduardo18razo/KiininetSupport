using System;
using KinniNet.Business.Utils;

namespace KinniNet.Core.Demonio
{
    public class BusinessTicketMailService: IDisposable
    {
        private readonly bool _proxy;

        public void Dispose()
        {

        }

        public BusinessTicketMailService(bool proxy = false)
        {
            _proxy = proxy;
        }

        public string RecibeCorreos()
        {
            try
            {
                //TODO: Cambiar cliente por parametro
                //return new Imap4Mail.Retrieve().GetMails();
                return new ManejadorCorreo.ClienteImap().LeeCorreo();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EnviaCorreoTicketGenerado(int idTicket, string clave, string subject, string body, string to)
        {
            try
            {
                //TODO: Cambiar cliente por parametro
                //new Imap4Mail.Retrieve().SendMailTicket(idTicket, clave, body, to);
                new ManejadorCorreo.ClienteSmtp().SendMailTicket(idTicket, clave, subject, body, to);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
