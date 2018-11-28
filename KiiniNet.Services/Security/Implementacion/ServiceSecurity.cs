using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Services.Security.Interface;
using KinniNet.Core.Security;

namespace KiiniNet.Services.Security.Implementacion
{
    public class ServiceSecurity : IServiceSecurity
    {
        public bool Autenticate(string user, string password, string activeNavigator)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    return negocio.Autenticate(user, password, activeNavigator);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Rol> ObtenerRolesUsuario(int idUsuario)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    return negocio.ObtenerRolesUsuario(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Usuario GetUserDataAutenticate(string user, string password)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    return negocio.GetUserDataAutenticate(user, password);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Usuario GetUserInvitadoDataAutenticate(int idTipoUsuario)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    return negocio.GetUserInvitadoDataAutenticate(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ChangePassword(int idUsuario, string contrasenaActual, string contrasenaNueva)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    negocio.ChangePassword(idUsuario, contrasenaActual, contrasenaNueva);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Menu> ObtenerMenuUsuario(int idUsuario, int idRol, bool arboles)
        {
            try
            {
                using (BusinessSecurity.Menus negocio = new BusinessSecurity.Menus())
                {
                    return negocio.ObtenerMenuUsuario(idUsuario, idRol, arboles);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Menu> ObtenerMenuPublico(int idTipoUsuario, int idArea, bool arboles)
        {
            try
            {
                using (BusinessSecurity.Menus negocio = new BusinessSecurity.Menus())
                {
                    return negocio.ObtenerMenuPublico(idTipoUsuario, idArea, arboles);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RecuperarCuenta(int idUsuario, int idTipoNotificacion, string link, int idCorreo, string codigo, string contrasena, string tipoRecuperacion)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    negocio.RecuperarCuenta(idUsuario, idTipoNotificacion, link, idCorreo, codigo, contrasena, tipoRecuperacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ValidaPassword(string pwd)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    negocio.ValidaPassword(pwd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CaducaPassword(int idUsuario)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    return negocio.CaducaPassword(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UsuariorActivo(string user, string password, string activeNavigator)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    return negocio.UsuariorActivo(user, password, activeNavigator);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CerrarSessionActiva(string user, string password)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    negocio.CerrarSessionActiva(user, password);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void TerminarSesion(int idUsuario, string activeNavigator)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    negocio.TerminarSesion(idUsuario, activeNavigator);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidaSesion(int idUsuario, string activeNavigator)
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    return negocio.ValidaSesion(idUsuario, activeNavigator);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GeneraLlaveMaquina()
        {
            try
            {
                using (BusinessSecurity.Autenticacion negocio = new BusinessSecurity.Autenticacion())
                {
                    return negocio.GeneraLlaveMaquina();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
