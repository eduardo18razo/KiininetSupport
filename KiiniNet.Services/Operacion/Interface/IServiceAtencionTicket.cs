using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Helper;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceAtencionTicket
    {
        [OperationContract]
        void AutoAsignarTicket(int idTicket, int idUsuario);

        [OperationContract]
        void CambiarEstatus(int idTicket, int idEstatus, int idUsuario, string comentario);
        
        [OperationContract]
        void CambiarAsignacionTicket(int idTicket, int idEstatusAsignacion, int idUsuarioAsignado, int idNivelAsignado, int idUsuarioAsigna, string comentario);

        [OperationContract]
        void AgregarComentarioConversacionTicket(int idTicket, int idUsuario, string mensaje, bool sistema, List<string> archivos, bool privado, bool enviaCorreo);
        
        [OperationContract]
        void MarcarAsignacionLeida(int idAsignacion);

        [OperationContract]
        HelperticketEnAtencion ObtenerTicketEnAtencion(int idTicket, int idUsuario);

        [OperationContract]
        int ObtenerNumeroTicketsEnAtencionNuevos(int idUsuario);

    }
}
