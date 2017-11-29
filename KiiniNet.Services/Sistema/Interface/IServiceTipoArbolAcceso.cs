using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceTipoArbolAcceso
    {
        [OperationContract]
        List<TipoArbolAcceso> ObtenerTiposArbolAcceso(bool insertarSeleccion);

        [OperationContract]
        List<TipoArbolAcceso> ObtenerTiposArbolAccesoByGruposTercero(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, bool insertarSeleccion);

        [OperationContract]
        List<TipoArbolAcceso> ObtenerTiposArbolAccesoByGrupos(List<int> grupos, bool insertarSeleccion);
    }
}
