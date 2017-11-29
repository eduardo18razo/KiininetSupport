using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceCanal
    {
        [OperationContract]
        List<Canal> ObtenerCanales(bool insertarSeleccion);

        [OperationContract]
        List<Canal> ObtenerCanalesAll(bool insertarSeleccion);

        [OperationContract]
        Canal ObtenerCanalById(int idCanal);
    }
}
