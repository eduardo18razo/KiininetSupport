using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServicePuesto
    {
        [OperationContract]
        List<Puesto> ObtenerPuestosByTipoUsuario(int idTipoUsuario, bool insertarSeleccion);

        [OperationContract]
        Puesto ObtenerPuestoById(int idPuesto);

        [OperationContract]
        void Guardar(Puesto puesto);

        [OperationContract]
        void Actualizar(int idPuesto, Puesto puesto);

        [OperationContract]
        List<Puesto> ObtenerPuestoConsulta(int? idTipoUsuario);

        [OperationContract]
        void Habilitar(int idPuesto, bool habilitado);
    }
}
