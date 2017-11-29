using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceNotificacion
    {
        [OperationContract]
        List<TipoNotificacion> ObtenerTipos(bool insertarSeleccion);
    }
}
