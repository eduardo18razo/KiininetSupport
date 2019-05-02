using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceTicket
    {
        [OperationContract]
        Ticket CrearTicketAndroid(int idUsuario, int idUsuarioSolicito, int idArbol, string latitudinicio,
            string longitudinicio,
            string fechaalta,
            string latitudfin,
            string longitudfin, string costo, int idCanal, bool campoRandom, bool esTercero, bool esMail);
        [OperationContract]
        Ticket CrearTicket(int idUsuario, int idUsuarioSolicito, int idArbol, List<HelperCampoMascaraCaptura> lstCaptura, int idCanal, bool campoRandom, bool esTercero, bool esMail);

        [OperationContract]
        List<HelperTickets> ObtenerTicketsUsuario(int idUsuario);

        [OperationContract]
        List<HelperTickets> ObtenerTickets(int idUsuario, List<int> estatus, int pageIndex, int pageSize);
        
        [OperationContract]
        HelperDetalleTicket ObtenerDetalleTicket(int idTicket);

        [OperationContract]
        HelperTicketDetalle ObtenerTicket(int idTicket, int idUsuario);

        [OperationContract]
        HelperDetalleTicket ObtenerDetalleTicketNoRegistrado(int idTicket, string cveRegistro);

        [OperationContract]
        PreTicket GeneraPreticket(int idArbol, int idUsuarioSolicita, int idUsuarioLevanto, string observaciones);

        [OperationContract]
        List<int> CapturaCasillaTicket(int idTicket, string nombreCampo);

        [OperationContract]
        PreTicketCorreo ObtenerPreticketCorreo(string guid);

        [OperationContract]
        void ConfirmaPreTicket(string guid, int idUsuario);
    }

}
