using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceDomicilioSistema
    {
        [OperationContract]
        List<Colonia> ObtenerColoniasCp(int cp, bool insertarSeleccion);

        [OperationContract]
        Colonia ObtenerDetalleColonia(int idColonia);
    }
}
