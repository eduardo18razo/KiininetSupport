using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceTipoDocumento
    {
        [OperationContract]
        List<TipoDocumento> ObtenerTipoDocumentos(bool insertarSeleccion);

         [OperationContract]
         TipoDocumento ObtenerTiposDocumentoId(int idTipoDocumento);
    }
}
