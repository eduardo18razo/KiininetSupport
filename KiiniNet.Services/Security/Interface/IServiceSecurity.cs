using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Services.Security.Interface
{
    [ServiceContract]
    public interface IServiceSecurity
    {
        [OperationContract]
        bool Autenticate(string user, string password);

        [OperationContract]
        List<Rol> ObtenerRolesUsuario(int idUsuario);

        [OperationContract]
        Usuario GetUserDataAutenticate(string user, string password);

        [OperationContract]
        Usuario GetUserInvitadoDataAutenticate(int idTipoUsuario);

        [OperationContract]
        void ChangePassword(int idUsuario, string contrasenaActual, string contrasenaNueva);

        [OperationContract]
        List<Menu> ObtenerMenuUsuario(int idUsuario, int idRol, bool arboles);

        [OperationContract]
        List<Menu> ObtenerMenuPublico(int idTipoUsuario, int idArea, bool arboles);

        [OperationContract]
        void RecuperarCuenta(int idUsuario, int idTipoNotificacion, string link, int idCorreo, string codigo, string contrasena, string tipoRecuperacion);

        [OperationContract]
        void ValidaPassword(string pwd);

        [OperationContract]
        bool CaducaPassword(int idUsuario);
    }
}
