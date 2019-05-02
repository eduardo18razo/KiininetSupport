using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceUsuarios : IServiceUsuarios
    {
        public void ValidaLimiteOperadores()
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.ValidaLimiteOperadores();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarUsuario(Usuario usuario, Domicilio domicilio)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.GuardarUsuario(usuario, domicilio);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int RegistrarCliente(Usuario usuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.RegistrarCliente(usuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarUsuarioAdicional(string nombre, string ap, string correo, string celular, string edad, string numeroTarjeta, string fechavto, string cvv)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.GuardarUsuarioAdicional(nombre, ap, correo, celular, edad, numeroTarjeta, fechavto, cvv);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Usuario> ObtenerAgentes(bool insertarSeleccion)
        {

            {
                try
                {
                    using (BusinessUsuarios negocio = new BusinessUsuarios())
                    {
                        return negocio.ObtenerAgentes(insertarSeleccion);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<Usuario> ObtenerUsuarios(int? idTipoUsuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerUsuarios(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Usuario> ConsultaUsuariosUsuarios(int? idTipoUsuario, string filtro)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ConsultaUsuariosUsuarios(idTipoUsuario, filtro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Usuario ObtenerDetalleUsuario(int idUsuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerDetalleUsuario(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperDetalleUsuarioGrupo> ObtenerUsuariosByGrupo(int idGrupo)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerUsuariosByGrupo(idGrupo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Usuario> ObtenerUsuariosByGrupoAgente(int idGrupo, int idNivel)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerUsuariosByGrupoAgente(idGrupo, idNivel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Usuario> ObtenerUsuariosByGrupoAtencion(int idGrupo, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerUsuariosByGrupoAtencion(idGrupo, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarUsuario(int idUsuario, Usuario usuario, Domicilio domicilio)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios(true))
                {
                    negocio.ActualizarUsuario(idUsuario, usuario, domicilio);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarFoto(int idUsuario, byte[] imagen)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.GuardarFoto(idUsuario, imagen);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] ObtenerFoto(int idUsuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerFoto(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarUsuario(int idUsuario, bool habilitado, string tmpurl)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.HabilitarUsuario(idUsuario, habilitado, tmpurl);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Usuario> ObtenerAtendedoresEncuesta(int idUsuario, List<int?> encuestas)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerAtendedoresEncuesta(idUsuario, encuestas);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidaUserName(string nombreUsuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ValidaUserName(nombreUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidaConfirmacion(int idUsuario, string guid)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ValidaConfirmacion(idUsuario, guid);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidaConfirmacionCambioCorreo(int idUsuario, string guid)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios(true))
                {
                    return negocio.ValidaConfirmacionCambioCorreo(idUsuario, guid);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ValidaCodigoVerificacionSms(int idUsuario, int idTipoNotificacion, int idTelefono, string codigo)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ValidaCodigoVerificacionSms(idUsuario, idTipoNotificacion, idTelefono, codigo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EnviaCodigoVerificacionSms(int idUsuario, int idTipoNotificacion, int idTelefono)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.EnviaCodigoVerificacionSms(idUsuario, idTipoNotificacion, idTelefono);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarTelefono(int idUsuario, int idTelefono, string numero)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.ActualizarTelefono(idUsuario, idTelefono, numero);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ConfirmaCuenta(int idUsuario, string password, Dictionary<int, string> confirmaciones, List<PreguntaReto> pregunta, string link)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.ConfirmaCuenta(idUsuario, password, confirmaciones, pregunta, link);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ReenviarActivacion(int idUsuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.ReenviarActivacion(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Usuario BuscarUsuario(string usuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.BuscarUsuario(usuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Usuario> BuscarUsuarios(string usuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.BuscarUsuarios(usuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string EnviaCodigoVerificacionCorreo(int idUsuario, int idTipoNotificacion, int idCorreo)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.EnviaCodigoVerificacionCorreo(idUsuario, idTipoNotificacion, idCorreo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ValidaCodigoVerificacionCorreo(int idUsuario, int idTipoNotificacion, string link, int idCorreo, string codigo)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.ValidaCodigoVerificacionCorreo(idUsuario, idTipoNotificacion, link, idCorreo, codigo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ValidaRespuestasReto(int idUsuario, Dictionary<int, string> preguntasReto)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    negocio.ValidaRespuestasReto(idUsuario, preguntasReto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperUsuario ObtenerDatosTicketUsuario(int idUsuario)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerDatosTicketUsuario(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Usuario GetUsuarioByCorreo(string correo)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.GetUsuarioByCorreo(correo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperUsuarioAgente> ObtenerUsuarioAgenteByGrupoUsuario(int idGrupo, int idUsuarioSolicita, List<int> lstSubRoles)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerUsuarioAgenteByGrupoUsuario(idGrupo, idUsuarioSolicita, lstSubRoles);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Usuario> ObtenerAgentesPermitidos(int idUsuarioSolicita, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUsuarios negocio = new BusinessUsuarios())
                {
                    return negocio.ObtenerAgentesPermitidos(idUsuarioSolicita, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
