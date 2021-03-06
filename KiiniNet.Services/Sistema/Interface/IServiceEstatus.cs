﻿using System.Collections.Generic;
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
        List<EstatusTicket> ObtenerEstatusTicketUsuario(int idUsuario, int idGrupo, int idEstatusActual, bool esPropietario, int? idSubRol, bool insertarSeleccion);

        [OperationContract]
        List<EstatusTicket> ObtenerEstatusTicketUsuarioPublico(int idTicket, int idGrupo, int idEstatusActual, bool esPropietario, int? idSubRol, bool insertarSeleccion);

        [OperationContract]
        List<EstatusAsignacion> ObtenerEstatusAsignacionUsuario(int idUsuario, int idGrupo, int estatusAsignacionActual, bool esPropietario, int subRolActual, bool insertarSeleccion);

        [OperationContract]
        bool HasComentarioObligatorio(int idUsuario, int idGrupo, int idSubRol, int estatusAsignacionActual, int estatusAsignar, bool esPropietario);

        [OperationContract]
        bool HasCambioEstatusComentarioObligatorio(int? idUsuario, int idTicket, int estatusAsignar, bool esPropietario, bool publico);
    }
}
