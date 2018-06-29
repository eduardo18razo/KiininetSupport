using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Services.Parametros.Interface
{
    [ServiceContract]
    public interface IServiceParametros
    {
        [OperationContract]
        List<ParametrosTelefonos> TelefonosObligatorios(int idTipoUsuario);

        [OperationContract]
        List<TelefonoUsuario> ObtenerTelefonosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion);

        [OperationContract]
        List<CorreoUsuario> ObtenerCorreosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion);

        [OperationContract]
        ParametrosGenerales ObtenerParametrosGenerales();

        [OperationContract]
        List<AliasOrganizacion> ObtenerAliasOrganizacion(int idTipoUsuario);

        [OperationContract]
        List<AliasUbicacion> ObtenerAliasUbicacion(int idTipoUsuario);

        [OperationContract]
        ParametroDatosAdicionales ObtenerDatosAdicionales(int idTipoUsuario);

        [OperationContract]
        ParametroPassword ObtenerParemtrosPassword();
    }
}
