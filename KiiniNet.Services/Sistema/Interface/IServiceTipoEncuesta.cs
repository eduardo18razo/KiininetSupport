using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceTipoEncuesta
    {
        [OperationContract]
        List<TipoEncuesta> ObtenerTiposEncuesta(bool insertarSeleccion);

        [OperationContract]
        TipoEncuesta TipoEncuestaId(int idTipoEncuesta);
    }
}
