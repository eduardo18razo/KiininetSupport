using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Helper;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceFrecuencia
    {
        [OperationContract]
        List<HelperFrecuencia> ObtenerTopTenGeneral(int idTipoUsuario);
        [OperationContract]
        List<HelperFrecuencia> ObtenerTopTenConsulta(int idTipoUsuario);
        [OperationContract]
        List<HelperFrecuencia> ObtenerTopTenServicio(int idTipoUsuario);
        [OperationContract]
        List<HelperFrecuencia> ObtenerTopTenIncidente(int idTipoUsuario);
    }
}
