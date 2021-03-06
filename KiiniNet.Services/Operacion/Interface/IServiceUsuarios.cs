﻿using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceUsuarios
    {
        [OperationContract]
        void ValidaLimiteOperadores();
        [OperationContract]
        void GuardarUsuario(Usuario usuario, Domicilio domicilio);

        [OperationContract]
        int RegistrarCliente(Usuario usuario);

        [OperationContract]
        void GuardarUsuarioAdicional(string nombre, string ap, string correo, string celular, string edad, string numeroTarjeta, string fechavto, string cvv);

        [OperationContract]
        List<Usuario> ObtenerAgentes(bool insertarSeleccion);

        [OperationContract]
        List<Usuario> ObtenerUsuarios(int? idTipoUsuario);

        [OperationContract]
        List<Usuario> ConsultaUsuariosUsuarios(int? idTipoUsuario, string filtro);

        [OperationContract]
        Usuario ObtenerDetalleUsuario(int idUsuario);

        [OperationContract]
        List<HelperDetalleUsuarioGrupo> ObtenerUsuariosByGrupo(int idGrupo);

        [OperationContract]
        List<Usuario> ObtenerUsuariosByGrupoAgente(int idGrupo, int idNivel);

        [OperationContract]
        List<Usuario> ObtenerUsuariosByGrupoAtencion(int idGrupo, bool insertarSeleccion);

        [OperationContract]
        void ActualizarUsuario(int idUsuario, Usuario usuario, Domicilio domicilio);

        [OperationContract]
        void GuardarFoto(int idUsuario, byte[] imagen);

        [OperationContract]
        byte[] ObtenerFoto(int idUsuario);

        [OperationContract]
        void HabilitarUsuario(int idUsuario, bool habilitado, string tmpurl);

        [OperationContract]
        List<Usuario> ObtenerAtendedoresEncuesta(int idUsuario, List<int?> encuestas);

        [OperationContract]
        bool ValidaUserName(string nombreUsuario);

        [OperationContract]
        bool ValidaConfirmacion(int idUsuario, string guid);
        [OperationContract]
        bool ValidaConfirmacionCambioCorreo(int idUsuario, string guid);

        [OperationContract]
        string ValidaCodigoVerificacionSms(int idUsuario, int idTipoNotificacion, int idTelefono, string codigo);

        [OperationContract]
        void EnviaCodigoVerificacionSms(int idUsuario, int idTipoNotificacion, int idTelefono);

        [OperationContract]
        void ActualizarTelefono(int idUsuario, int idTelefono, string numero);

        [OperationContract]
        void ConfirmaCuenta(int idUsuario, string password, Dictionary<int, string> confirmaciones, List<PreguntaReto> pregunta, string link);

        [OperationContract]
        void ReenviarActivacion(int idUsuario);

        [OperationContract]
        Usuario BuscarUsuario(string usuario);

        [OperationContract]
        List<Usuario> BuscarUsuarios(string usuario);

        [OperationContract]
        string EnviaCodigoVerificacionCorreo(int idUsuario, int idTipoNotificacion, int idCorreo);

        [OperationContract]
        void ValidaCodigoVerificacionCorreo(int idUsuario, int idTipoNotificacion, string link, int idCorreo, string codigo);

        [OperationContract]
        void ValidaRespuestasReto(int idUsuario, Dictionary<int, string> preguntasReto);

        [OperationContract]
        HelperUsuario ObtenerDatosTicketUsuario(int idUsuario);

        [OperationContract]
        Usuario GetUsuarioByCorreo(string correo);

        [OperationContract]
        List<HelperUsuarioAgente> ObtenerUsuarioAgenteByGrupoUsuario(int idGrupo, int idUsuarioSolicita, List<int> lstSubRoles);

        [OperationContract]
        List<Usuario> ObtenerAgentesPermitidos(int idUsuarioSolicita, bool insertarSeleccion);
    }
}
