using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceSla
    {
        [OperationContract]
        List<Sla> ObtenerSla(bool insertarSeleccion);

        [OperationContract]
        Sla ObtenerSlaById(int idSla);

        [OperationContract]
        void Guardar(Sla sla);
    }
}
