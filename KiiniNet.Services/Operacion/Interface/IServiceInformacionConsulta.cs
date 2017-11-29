using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Operacion;
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
        void GuardarHit(int idArbol, int idUsuario);

        [OperationContract]
        List<InformacionConsulta> ObtenerConsulta(string descripcion);

        [OperationContract]
        void RateConsulta(int idConsulta, int idUsuario, bool meGusta);

        [OperationContract]
        void HabilitarInformacion(int idInformacion, bool habilitado);

    }
}
