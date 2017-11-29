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

        public void RecibeCorreos()
        {
            try
            {
                //TODO: Cambiar cliente por parametro
                new Imap4Mail.Retrieve().GetMails(BusinessVariables.EnumtServerImap.Gmail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EnviaCorreoTicketGenerado(int idTicket, string clave, string body, string to)
        {
            try
            {
                //TODO: Cambiar cliente por parametro
                new Imap4Mail.Retrieve().SendMailTicket(BusinessVariables.EnumtServerImap.Gmail, idTicket, clave, body, to);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
