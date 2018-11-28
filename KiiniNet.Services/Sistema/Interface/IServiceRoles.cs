using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceRoles 
    {
        [OperationContract]
        List<Rol> ObtenerRoles(int idTipoUsuario, bool insertarSeleccion);

        [OperationContract]
        List<RolTipoArbolAcceso> ObtenerRolesArbolAcceso(int idTipoArbolAcceso);
    }
}
