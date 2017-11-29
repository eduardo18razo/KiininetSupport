using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceEstatus
    {
        [OperationContract]
        List<EstatusTicket> ObtenerEstatusTicket(bool insertarSeleccion);
        [OperationContract]
        List<EstatusAsignacion> ObtenerEstatusAsignacion(bool insertarSeleccion);

        [OperationContract]
        List<EstatusTicket> ObtenerEstatusTicketUsuario(int idUsuario, int idGrupo, int idEstatusActual, bool esPropietario, bool insertarSeleccion);

        [OperationContract]
        List<EstatusAsignacion> ObtenerEstatusAsignacionUsuario(int idUsuario, int idGrupo, int idSubRol, int estatusAsignacionActual, bool esPropietario, bool insertarSeleccion);

        [OperationContract]
        bool HasComentarioObligatorio(int idUsuario, int idGrupo, int idSubRol, int estatusAsignacionActual, int estatusAsignar, bool esPropietario);
    }
}
