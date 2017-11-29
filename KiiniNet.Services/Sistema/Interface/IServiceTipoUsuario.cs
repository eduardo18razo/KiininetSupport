using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceTipoUsuario
    {
        [OperationContract]
        List<TipoUsuario> ObtenerTiposUsuarioResidentes(bool insertarSeleccion);
        [OperationContract]
        List<TipoUsuario> ObtenerTiposUsuarioInvitados(bool insertarSeleccion);

        [OperationContract]
        List<TipoUsuario> ObtenerTiposUsuario(bool insertarSeleccion);

        [OperationContract]
        TipoUsuario ObtenerTipoUsuarioById(int id);
    }
}
