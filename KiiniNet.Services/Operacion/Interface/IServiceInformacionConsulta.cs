using System;
using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Reportes;
using KinniNet.Business.Utils;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceInformacionConsulta
    {
        [OperationContract]
        List<InformacionConsulta> ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta tipoinfoConsulta, bool insertarSeleccion);

        [OperationContract]
        List<InformacionConsulta> ObtenerInformacionConsultaArbol(int idArbol);

        [OperationContract]
        InformacionConsulta ObtenerInformacionConsultaById(int idInformacion);

        [OperationContract]
        InformacionConsulta GuardarInformacionConsulta(InformacionConsulta informacion, List<string> documentosDescarga);

        [OperationContract]
        InformacionConsulta ActualizarInformacionConsulta(int idInformacionConsulta, InformacionConsulta informacion, List<string> documentosDescarga);

        [OperationContract]
        void GuardarHit(int idArbol, int idTipoUsuario, int? idUsuario);

        [OperationContract]
        List<InformacionConsulta> ObtenerConsulta(string descripcion);

        [OperationContract]
        List<HelperInformacionConsulta> ObtenerInformacionReporte(string descripcion, Dictionary<string, DateTime> fechas);

        [OperationContract]
        void RateConsulta(int idArbol, int idConsulta, int idUsuario, bool meGusta);

        [OperationContract]
        void HabilitarInformacion(int idInformacion, bool habilitado);

        [OperationContract]
        ReporteInformacionConsulta ObtenerReporteInformacionConsulta(int idInformacionConsulta, Dictionary<string, DateTime> fechas, int tipoFecha);

    }
}
