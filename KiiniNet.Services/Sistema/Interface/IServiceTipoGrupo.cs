using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceTipoGrupo
    {
        [OperationContract]
        List<TipoGrupo> ObtenerTiposGrupo(bool insertarSeleccion);

         [OperationContract]
         List<TipoGrupo> ObtenerTiposGruposByRol(int idrol, bool insertarSeleccion);

        [OperationContract]
        List<TipoGrupo> ObtenerTiposGruposByTipoUsuario(int idTipoUsuario, bool insertarSeleccion);
    }
}
