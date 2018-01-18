using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Demonio;
using KinniNet.Core.Parametros;
using KinniNet.Core.Sistema;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessUsuarios : IDisposable
    {
        private bool _proxy;

        public BusinessUsuarios(bool proxy = false)
        {
            _proxy = proxy;
        }

        public Usuario ObtenerUsuario(int idUsuario)
        {
            Usuario result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Usuario.SingleOrDefault(s => s.Id == idUsuario);
                if (result != null)
                {
                    db.LoadProperty(result, "CorreoUsuario");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public void GuardarUsuarioAdicional(string nombre, string ap, string correo, string celular, string edad, string numeroTarjeta, string fechavto, string cvv)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                const int idMascara = 8;
                Usuario user = new Usuario();
                user.IdTipoUsuario = (int)BusinessVariables.EnumTiposUsuario.Cliente;
                user.IdOrganizacion = 1;
                user.IdUbicacion = 1;
                user.Nombre = nombre;
                user.ApellidoMaterno = "";
                user.ApellidoPaterno = ap;
                user.Habilitado = true;
                user.NombreUsuario = (user.Nombre.Substring(0, 1).ToLower() + user.ApellidoPaterno.Trim().ToLower()).Replace(" ", string.Empty);
                user.Password = "/web/ConfirmacionCuenta.aspx";
                int limite = 9999;
                if (ValidaUserName(user.Nombre))
                {
                    for (int i = 1; i < limite; i++)
                    {
                        string tmpUsername = user.Nombre + i;
                        if (!ValidaUserName(tmpUsername))
                        {
                            user.Nombre = tmpUsername;
                            break;
                        }
                        limite++;
                    }
                }


                user.TelefonoUsuario = new List<TelefonoUsuario>();
                TelefonoUsuario tu = new TelefonoUsuario { Numero = celular, Obligatorio = true, IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Celular };
                user.TelefonoUsuario.Add(tu);

                user.CorreoUsuario = new List<CorreoUsuario>();
                CorreoUsuario cu = new CorreoUsuario { Correo = correo, Obligatorio = true };
                user.CorreoUsuario.Add(cu);
                user.UsuarioRol = new List<UsuarioRol>();
                user.UsuarioRol.Add(new UsuarioRol
                {
                    RolTipoUsuario = (new BusinessRoles().ObtenerRolTipoUsuario((int)BusinessVariables.EnumTiposUsuario.Cliente, (int)BusinessVariables.EnumRoles.Usuario)),
                });

                user.UsuarioGrupo = new List<UsuarioGrupo>();

                UsuarioGrupo ug = new UsuarioGrupo
                {
                    IdGrupoUsuario = (new BusinessGrupoUsuario().ObtenerGruposUsuarioByIdRolTipoUsuario(
                        (int)BusinessVariables.EnumRoles.Usuario, (int)BusinessVariables.EnumTiposUsuario.Cliente,
                        false)).First().Id,
                    IdRol = (int)BusinessVariables.EnumRoles.Usuario
                };
                user.UsuarioGrupo.Add(ug);


                int idUsuario = GuardarUsuario(user);
                Mascara mascara = new BusinessMascaras().ObtenerMascaraCaptura(idMascara);
                string store = string.Format("USPINSERTDATOSADICIONALESUSUARIO {0}, '{1}', '{2}', '{3}', '{4}'", idUsuario, edad, numeroTarjeta, fechavto, cvv);

                db.ExecuteStoreCommand(store);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public int GuardarUsuario(Usuario usuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                ValidaCorreos(usuario.CorreoUsuario.Where(w => w.Obligatorio).Select(s => s.Correo).ToList(), null);
                ValidaTelefonos(usuario.TelefonoUsuario.Where(w => w.Obligatorio && w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular).Select(s => s.Numero).ToList(), null);
                string tmpurl = usuario.Password;
                Guid g = Guid.NewGuid();
                ParametroCorreo correo = db.ParametroCorreo.SingleOrDefault(s => s.IdTipoCorreo == (int)BusinessVariables.EnumTipoCorreo.AltaUsuario && s.Habilitado);
                usuario.ApellidoPaterno = usuario.ApellidoPaterno.Trim();
                usuario.ApellidoMaterno = usuario.ApellidoMaterno.Trim();
                usuario.Nombre = usuario.Nombre.Trim();
                usuario.NombreUsuario = usuario.NombreUsuario.PadRight(30).Substring(0, 30).Trim();
                usuario.Password = BusinessQueryString.Encrypt(ConfigurationManager.AppSettings["siteUrl"] + tmpurl + "?confirmacionalta=" + usuario.Id + "_" + g);
                usuario.UsuarioLinkPassword = new List<UsuarioLinkPassword>
                {
                    new UsuarioLinkPassword
                    {
                        Activo = true,
                        Link = g,
                        Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                        IdTipoLink = (int) BusinessVariables.EnumTipoLink.Confirmacion
                    }
                };
                foreach (UsuarioRol rol in usuario.UsuarioRol)
                {
                    rol.IdRolTipoUsuario = new BusinessRoles().ObtenerRolTipoUsuario(usuario.IdTipoUsuario, rol.RolTipoUsuario.IdRol).Id;
                    GrupoUsuario gu = new BusinessGrupoUsuario().ObtenerGrupoDefaultRol(rol.RolTipoUsuario.IdRol, usuario.IdTipoUsuario);
                    if (gu != null)
                    {
                        if (usuario.UsuarioGrupo.All(a => a.IdGrupoUsuario != gu.Id))
                            if (gu.SubGrupoUsuario != null && gu.SubGrupoUsuario.Count > 0)
                            {
                                foreach (SubGrupoUsuario subGpoUsuario in gu.SubGrupoUsuario)
                                {
                                    usuario.UsuarioGrupo.Add(new UsuarioGrupo { IdRol = rol.RolTipoUsuario.IdRol, IdGrupoUsuario = gu.Id, IdSubGrupoUsuario = subGpoUsuario.Id });
                                }
                            }
                            else
                            {
                                usuario.UsuarioGrupo.Add(new UsuarioGrupo { IdRol = rol.RolTipoUsuario.IdRol, IdGrupoUsuario = gu.Id });
                            }

                    }
                    rol.RolTipoUsuario = null;
                }
                if (usuario.IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Empleado || usuario.IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Proveedor)
                    usuario.Habilitado = false;
                usuario.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                if (usuario.Id == 0)
                {
                    db.Usuario.AddObject(usuario);
                    db.SaveChanges();
                }
                usuario.Password = ConfigurationManager.AppSettings["siteUrl"] + tmpurl + "?confirmacionalta=" + usuario.Id + "_" + g;
                if (correo != null)
                {
                    String body = NamedFormat.Format(correo.Contenido, usuario);
                    foreach (CorreoUsuario correoUsuario in usuario.CorreoUsuario)
                    {
                        BusinessCorreo.SendMail(correoUsuario.Correo, "Confirma tu registro", body);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return usuario.Id;
        }

        public int RegistrarCliente(Usuario usuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                if (!db.TipoUsuario.Any(a => a.Id == usuario.IdTipoUsuario))
                    throw new Exception("Datos de usuario invalido.");
                if (usuario.TelefonoUsuario == null || usuario.TelefonoUsuario.Count <= 0 || !usuario.TelefonoUsuario.Any(a => a.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular && a.Obligatorio))
                {
                    //throw new Exception("Datos de usuario invalido.");
                }
                else
                    ValidaTelefonos(usuario.TelefonoUsuario.Where(w => w.Obligatorio && w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular).Select(s => s.Numero).ToList(), null);

                if (usuario.CorreoUsuario == null || usuario.CorreoUsuario.Count <= 0 || !usuario.CorreoUsuario.Any(a => a.Obligatorio))
                {
                    //throw new Exception("Datos de usuario invalido.");
                }
                else
                    ValidaCorreos(usuario.CorreoUsuario.Where(w => w.Obligatorio).Select(s => s.Correo).ToList(), null);

                string tmpurl = usuario.Password;
                Guid g = Guid.NewGuid();
                ParametroCorreo correo = db.ParametroCorreo.SingleOrDefault(s => s.IdTipoCorreo == (int)BusinessVariables.EnumTipoCorreo.AltaUsuario && s.Habilitado);
                usuario.ApellidoPaterno = usuario.ApellidoPaterno.Trim();
                usuario.ApellidoMaterno = usuario.ApellidoMaterno.Trim();
                usuario.Nombre = usuario.Nombre.Trim();
                usuario.NombreUsuario = string.IsNullOrEmpty(usuario.NombreUsuario) ? GeneraNombreUsuario(usuario.Nombre, usuario.ApellidoPaterno) : usuario.NombreUsuario;
                usuario.NombreUsuario = usuario.NombreUsuario.PadRight(30).Substring(0, 30).Trim();
                usuario.Password = BusinessQueryString.Encrypt(ConfigurationManager.AppSettings["siteUrl"] + tmpurl + "?confirmacionalta=" + usuario.Id + "_" + g);
                ParametrosUsuario parametros = db.ParametrosUsuario.SingleOrDefault(s => s.IdTipoUsuario == usuario.IdTipoUsuario);
                if (parametros == null)
                    throw new Exception("Parametros incorrectos.");
                Organizacion organizacion = new BusinessOrganizacion().ObtenerOrganizacionDefault(usuario.IdTipoUsuario);
                if (organizacion == null)
                    throw new Exception("Error al asignar datos contacte a su administrador.");
                Ubicacion ubicacion = new BusinessUbicacion().ObtenerOrganizacionDefault(usuario.IdTipoUsuario);
                if (ubicacion == null)
                    throw new Exception("Error al asignar datos contacte a su administrador.");
                usuario.IdOrganizacion = organizacion.Id;
                usuario.IdUbicacion = ubicacion.Id;
                usuario.UsuarioLinkPassword = new List<UsuarioLinkPassword>
                {
                    new UsuarioLinkPassword
                    {
                        Activo = true,
                        Link = g,
                        Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                        IdTipoLink = (int) BusinessVariables.EnumTipoLink.Confirmacion
                    }
                };
                usuario.UsuarioGrupo = usuario.UsuarioGrupo ?? new List<UsuarioGrupo>();
                if (usuario.UsuarioRol == null || usuario.UsuarioRol.Count <= 0)
                {
                    usuario.UsuarioRol = new List<UsuarioRol>();
                    foreach (Rol rol in new BusinessRoles().ObtenerRoles(usuario.IdTipoUsuario, false))
                    {
                        usuario.UsuarioRol.Add(new UsuarioRol
                        {
                            IdRolTipoUsuario = new BusinessRoles().ObtenerRolTipoUsuario(usuario.IdTipoUsuario, rol.Id).Id
                        });
                        GrupoUsuario gu = new BusinessGrupoUsuario().ObtenerGrupoDefaultRol(rol.Id, usuario.IdTipoUsuario);
                        if (gu != null)
                        {
                            if (usuario.UsuarioGrupo.All(a => a.IdGrupoUsuario != gu.Id))
                                if (gu.SubGrupoUsuario != null && gu.SubGrupoUsuario.Count > 0)
                                {
                                    foreach (SubGrupoUsuario subGpoUsuario in gu.SubGrupoUsuario)
                                    {
                                        usuario.UsuarioGrupo.Add(new UsuarioGrupo { IdRol = rol.Id, IdGrupoUsuario = gu.Id, IdSubGrupoUsuario = subGpoUsuario.Id });
                                    }
                                }
                                else
                                {
                                    usuario.UsuarioGrupo.Add(new UsuarioGrupo { IdRol = rol.Id, IdGrupoUsuario = gu.Id });
                                }

                        }
                        rol.RolTipoUsuario = null;
                    }
                }
                if (usuario.IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Empleado || usuario.IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Proveedor)
                    usuario.Habilitado = false;
                if (usuario.Id == 0)
                {
                    db.Usuario.AddObject(usuario);
                    db.SaveChanges();
                }

                if (usuario.IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Cliente || usuario.IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Operador)
                {
                    usuario.Password = ConfigurationManager.AppSettings["siteUrl"] + tmpurl + "?confirmacionalta=" + usuario.Id + "_" + g;
                    if (correo != null)
                    {
                        String body = NamedFormat.Format(correo.Contenido, usuario);
                        foreach (CorreoUsuario correoUsuario in usuario.CorreoUsuario)
                        {
                            BusinessCorreo.SendMail(correoUsuario.Correo, "Confirma tu registro", body);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return usuario.Id;
        }
        public string GeneraNombreUsuario(string nombre, string apellidoPaterno)
        {
            string result = null;
            try
            {
                string username = (nombre.Substring(0, 1).ToLower() + apellidoPaterno.Trim().ToLower()).Replace(" ", string.Empty);
                username = username.PadRight(30).Substring(0, 30).Trim();
                int limite = 10;
                if (ValidaUserName(username))
                {
                    for (int i = 1; i < limite; i++)
                    {
                        string tmpUsername = username + i;
                        if (!ValidaUserName(tmpUsername.PadRight(30).Substring(0, 30).Trim()))
                        {
                            username = tmpUsername;
                            break;
                        }
                        limite++;
                    }
                }
                result = username;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        public void ActualizarUsuario(int idUsuario, Usuario usuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario userData = db.Usuario.SingleOrDefault(u => u.Id == idUsuario);
                db.ContextOptions.LazyLoadingEnabled = true;
                if (userData != null)
                {
                    userData.ApellidoMaterno = usuario.ApellidoMaterno.Trim();
                    userData.ApellidoPaterno = usuario.ApellidoPaterno.Trim();
                    userData.Nombre = usuario.Nombre.Trim().Trim();
                    userData.IdPuesto = usuario.IdPuesto;
                    userData.IdOrganizacion = usuario.IdOrganizacion;
                    userData.IdUbicacion = usuario.IdUbicacion;
                    userData.Vip = usuario.Vip;
                    userData.DirectorioActivo = usuario.DirectorioActivo;
                    userData.PersonaFisica = usuario.PersonaFisica;
                    ValidaCorreos(usuario.CorreoUsuario.Where(w => w.Obligatorio).Select(s => s.Correo).ToList(), userData.Id);
                    ValidaTelefonos(usuario.TelefonoUsuario.Where(w => w.Obligatorio && w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular).Select(s => s.Numero).ToList(), userData.Id);
                    List<int> correoEliminar = (from correoUsuario in userData.CorreoUsuario
                                                where !usuario.CorreoUsuario.Any(a => a.Correo == correoUsuario.Correo)
                                                select correoUsuario.Id).ToList();
                    foreach (CorreoUsuario correoUsuario in usuario.CorreoUsuario)
                    {
                        if (!db.CorreoUsuario.Any(a => a.IdUsuario == idUsuario && a.Correo == correoUsuario.Correo))
                            userData.CorreoUsuario.Add(new CorreoUsuario
                            {
                                IdUsuario = idUsuario,
                                Correo = correoUsuario.Correo,
                                Obligatorio = correoUsuario.Obligatorio
                            });
                    }
                    foreach (int i in correoEliminar)
                    {
                        db.CorreoUsuario.DeleteObject(db.CorreoUsuario.SingleOrDefault(w => w.Id == i));
                    }
                    List<int> telefonoEliminar = (from telefonoUsuario in userData.TelefonoUsuario
                                                  where !usuario.TelefonoUsuario.Any(a => a.Numero == telefonoUsuario.Numero && a.IdTipoTelefono == telefonoUsuario.IdTipoTelefono)
                                                  select telefonoUsuario.Id).ToList();
                    foreach (TelefonoUsuario telefonoUsuario in usuario.TelefonoUsuario)
                    {
                        if (!db.TelefonoUsuario.Any(a => a.IdUsuario == idUsuario && a.Numero == telefonoUsuario.Numero && a.IdTipoTelefono == telefonoUsuario.IdTipoTelefono))
                            userData.TelefonoUsuario.Add(new TelefonoUsuario
                            {
                                IdUsuario = idUsuario,
                                Numero = telefonoUsuario.Numero,
                                IdTipoTelefono = telefonoUsuario.IdTipoTelefono,
                                Extension = telefonoUsuario.Extension,
                                Obligatorio = telefonoUsuario.Obligatorio
                            });
                    }
                    foreach (int i in telefonoEliminar)
                    {
                        db.TelefonoUsuario.DeleteObject(db.TelefonoUsuario.SingleOrDefault(w => w.Id == i));
                    }

                    foreach (UsuarioRol rol in usuario.UsuarioRol)
                    {
                        if (rol.RolTipoUsuario != null)
                        {
                            rol.IdRolTipoUsuario = new BusinessRoles().ObtenerRolTipoUsuario(rol.RolTipoUsuario.IdTipoUsuario, rol.RolTipoUsuario.IdRol).Id;
                            rol.IdUsuario = idUsuario;
                            GrupoUsuario gu = new BusinessGrupoUsuario().ObtenerGrupoDefaultRol(rol.RolTipoUsuario.IdRol, usuario.IdTipoUsuario);
                            if (gu != null)
                            {
                                if (usuario.UsuarioGrupo.All(a => a.IdGrupoUsuario != gu.Id))
                                    if (gu.SubGrupoUsuario != null && gu.SubGrupoUsuario.Count > 0)
                                    {
                                        foreach (SubGrupoUsuario subGpoUsuario in gu.SubGrupoUsuario)
                                        {
                                            usuario.UsuarioGrupo.Add(new UsuarioGrupo { IdUsuario = idUsuario, IdRol = rol.RolTipoUsuario.IdRol, IdGrupoUsuario = gu.Id, IdSubGrupoUsuario = subGpoUsuario.Id });
                                        }
                                    }
                                    else
                                    {
                                        usuario.UsuarioGrupo.Add(new UsuarioGrupo { IdUsuario = idUsuario, IdRol = rol.RolTipoUsuario.IdRol, IdGrupoUsuario = gu.Id });
                                    }

                            }
                            rol.RolTipoUsuario = null;
                        }
                    }

                    List<int> rolEliminar = (from usuarioRol in userData.UsuarioRol
                                             where !usuario.UsuarioRol.Any(a => a.IdUsuario == idUsuario && a.IdRolTipoUsuario == usuarioRol.IdRolTipoUsuario)
                                             select usuarioRol.Id).ToList();
                    foreach (UsuarioRol rol in usuario.UsuarioRol)
                    {
                        if (!db.UsuarioRol.Any(a => a.IdUsuario == idUsuario && a.IdRolTipoUsuario == rol.IdRolTipoUsuario))
                            userData.UsuarioRol.Add(new UsuarioRol
                            {
                                IdUsuario = idUsuario,

                                IdRolTipoUsuario = rol.IdRolTipoUsuario
                            });
                    }

                    foreach (int i in rolEliminar)
                    {
                        db.UsuarioRol.DeleteObject(db.UsuarioRol.SingleOrDefault(w => w.Id == i));
                    }

                    List<int> gruposEliminar = new List<int>();
                    foreach (UsuarioGrupo ugpoDb in userData.UsuarioGrupo)
                    {
                        if (ugpoDb.IdSubGrupoUsuario == null)
                        {
                            if (!usuario.UsuarioGrupo.Any(a => a.IdUsuario == idUsuario && a.IdRol == ugpoDb.IdRol && a.IdGrupoUsuario == ugpoDb.IdGrupoUsuario))
                                gruposEliminar.Add(ugpoDb.Id);
                        }
                        else
                        {
                            if (!usuario.UsuarioGrupo.Any(a => a.IdUsuario == idUsuario && a.IdRol == ugpoDb.IdRol && a.IdGrupoUsuario == ugpoDb.IdGrupoUsuario && a.IdSubGrupoUsuario == ugpoDb.IdSubGrupoUsuario))
                                gruposEliminar.Add(ugpoDb.Id);
                        }
                    }
                    foreach (UsuarioGrupo grupo in usuario.UsuarioGrupo)
                    {
                        if (grupo.IdSubGrupoUsuario == null)
                        {
                            if (!db.UsuarioGrupo.Any(a => a.IdUsuario == idUsuario && a.IdRol == grupo.IdRol && a.IdGrupoUsuario == grupo.IdGrupoUsuario))
                                userData.UsuarioGrupo.Add(new UsuarioGrupo
                                {
                                    IdUsuario = idUsuario,
                                    IdRol = grupo.IdRol,
                                    IdGrupoUsuario = grupo.IdGrupoUsuario,
                                    IdSubGrupoUsuario = grupo.IdSubGrupoUsuario
                                });
                        }
                        else
                        {
                            if (!db.UsuarioGrupo.Any(a => a.IdUsuario == idUsuario && a.IdRol == grupo.IdRol && a.IdGrupoUsuario == grupo.IdGrupoUsuario && a.IdSubGrupoUsuario == grupo.IdSubGrupoUsuario))
                                userData.UsuarioGrupo.Add(new UsuarioGrupo
                                {
                                    IdUsuario = idUsuario,
                                    IdRol = grupo.IdRol,
                                    IdGrupoUsuario = grupo.IdGrupoUsuario,
                                    IdSubGrupoUsuario = grupo.IdSubGrupoUsuario
                                });
                        }


                    }

                    foreach (int i in gruposEliminar)
                    {
                        db.UsuarioGrupo.DeleteObject(db.UsuarioGrupo.SingleOrDefault(w => w.Id == i));
                    }
                    userData.FechaActualizacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        private void ValidaCorreos(List<string> correos, int? idUsuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                if (idUsuario == null)
                    foreach (string correo in correos)
                    {
                        if (db.CorreoUsuario.Any(a => a.Correo == correo && a.Obligatorio))
                            throw new Exception(string.Format("Correo {0} ya se encuentra registrado", correo));
                    }
                else
                    foreach (string correo in correos)
                    {
                        if (db.CorreoUsuario.Any(a => a.Correo == correo && a.IdUsuario != idUsuario && a.Obligatorio))
                            throw new Exception(string.Format("Correo {0} ya se encuentra registrado", correo));
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        private void ValidaTelefonos(List<string> telefonos, int? idUsuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                if (idUsuario == null)
                    foreach (string numero in telefonos)
                    {
                        if (db.TelefonoUsuario.Any(a => a.Numero == numero && a.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular && a.Obligatorio))
                            throw new Exception(string.Format("Telefono {0} ya se encuentra registrado", numero));
                    }
                else
                    foreach (string numero in telefonos)
                    {
                        if (db.TelefonoUsuario.Any(a => a.Numero == numero && a.IdUsuario != idUsuario && a.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular && a.Obligatorio))
                            throw new Exception(string.Format("Telefono {0} ya se encuentra registrado", numero));
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void GuardarFoto(int idUsuario, byte[] imagen)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario inf = db.Usuario.SingleOrDefault(w => w.Id == idUsuario);
                if (inf != null) inf.Foto = imagen;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public byte[] ObtenerFoto(int idUsuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            byte[] result;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Usuario.Single(w => w.Id == idUsuario).Foto;
               // result = db.Usuario.Single(w => w.Id == idUsuario).Foto != null ? db.Usuario.Single(w => w.Id == idUsuario).Foto : new byte[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public void HabilitarUsuario(int idUsuario, bool habilitado, string tmpurl)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario user = db.Usuario.SingleOrDefault(w => w.Id == idUsuario);
                if (user != null)
                {
                    user.Habilitado = habilitado;
                    if (habilitado && user.Autoregistro)
                    {
                        Guid g = Guid.NewGuid();
                        ParametroCorreo correo = db.ParametroCorreo.SingleOrDefault(s => s.IdTipoCorreo == (int)BusinessVariables.EnumTipoCorreo.AltaUsuario && s.Habilitado);
                        if (correo != null)
                        {
                            db.LoadProperty(user, "CorreoUsuario");
                            db.LoadProperty(user, "UsuarioLinkPassword");
                            db.LoadProperty(correo, "TipoCorreo");

                            user.Password = ConfigurationManager.AppSettings["siteUrl"] + tmpurl + "?confirmacionalta=" + user.Id + "_" + g;
                            String body = NamedFormat.Format(correo.Contenido, user);
                            foreach (CorreoUsuario correoUsuario in user.CorreoUsuario)
                            {
                                BusinessCorreo.SendMail(correoUsuario.Correo, correo.TipoCorreo.Descripcion, body);
                            }
                        }
                        user.Password = BusinessQueryString.Encrypt(ConfigurationManager.AppSettings["siteUrl"] + tmpurl + "?confirmacionalta=" + user.Id + "_" + g);
                        foreach (UsuarioLinkPassword linkPassword in user.UsuarioLinkPassword)
                        {
                            linkPassword.Activo = false;
                        }
                        user.UsuarioLinkPassword = user.UsuarioLinkPassword ?? new List<UsuarioLinkPassword>();
                        user.UsuarioLinkPassword.Add(
                            new UsuarioLinkPassword
                            {
                                Activo = true,
                                Link = g,
                                Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                IdTipoLink = (int)BusinessVariables.EnumTipoLink.Confirmacion
                            });
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public List<Usuario> ObtenerUsuarios(int? idTipoUsuario)
        {
            List<Usuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Usuario> qry = db.Usuario;
                if (idTipoUsuario != null)
                    qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);
                result = qry.OrderBy(o => o.ApellidoPaterno).ThenBy(tb => tb.ApellidoMaterno).ThenBy(tb => tb.Nombre).ToList();
                foreach (Usuario usuario in result)
                {
                    db.LoadProperty(usuario, "TipoUsuario");
                    usuario.OrganizacionFinal =
                        new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(usuario.Id, true);
                    usuario.OrganizacionCompleta =
                        new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(usuario.Id, false);
                    usuario.UbicacionFinal = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(usuario.Id, true);
                    usuario.UbicacionCompleta = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(usuario.Id,
                        false);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public List<HelperDetalleUsuarioGrupo> ObtenerUsuariosByGrupo(int idGrupo)
        {
            List<HelperDetalleUsuarioGrupo> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> idsUsuarios = (db.Usuario.Join(db.UsuarioGrupo, u => u.Id, ug => ug.IdUsuario, (u, ug) => new { u, ug })
                    .Where(@t => @t.ug.IdGrupoUsuario == idGrupo).Select(@t => @t.u.Id)).Distinct().ToList();
                List<Usuario> usuarios = db.Usuario.Where(w => idsUsuarios.Contains(w.Id)).ToList();
                if (usuarios.Any())
                {
                    result = new List<HelperDetalleUsuarioGrupo>();
                    foreach (Usuario usuario in usuarios)
                    {

                        db.LoadProperty(usuario, "TipoUsuario");
                        db.LoadProperty(usuario, "UsuarioGrupo");
                        foreach (UsuarioGrupo usuarioGrupo in usuario.UsuarioGrupo.Where(w => w.IdGrupoUsuario == idGrupo))
                        {
                            db.LoadProperty(usuarioGrupo, "SubGrupoUsuario");
                            if (usuarioGrupo.SubGrupoUsuario != null)
                                db.LoadProperty(usuarioGrupo.SubGrupoUsuario, "SubRol");
                        }
                        HelperDetalleUsuarioGrupo add = new HelperDetalleUsuarioGrupo();
                        add.IdUsuario = usuario.Id;
                        add.NombreCompleto = usuario.NombreCompleto;
                        add.NombreUsuarioCompleto = usuario.NombreUsuario;
                        add.Supervisor = usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario == null) ?
                                "No" :
                                usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario.SubRol.Id == (int)BusinessVariables.EnumSubRoles.Supervisor)
                                ? "Si" : "No";
                        add.PrimerNivel = usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario == null) ?
                                "No" :
                                usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario.SubRol.Id == (int)BusinessVariables.EnumSubRoles.PrimererNivel)
                                ? "Si" : "No";
                        add.SegundoNivel = usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario == null) ?
                                "No" :
                                 usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario.SubRol.Id == (int)BusinessVariables.EnumSubRoles.SegundoNivel)
                                ? "Si" : "No";
                        add.TercerNivel = usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario == null) ?
                                "No" :
                                usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario.SubRol.Id == (int)BusinessVariables.EnumSubRoles.TercerNivel)
                                ? "Si" : "No";
                        add.CuartoNivel = usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario == null) ?
                                "No" :
                                usuario.UsuarioGrupo.Any(w => w.IdGrupoUsuario == idGrupo && w.SubGrupoUsuario.SubRol.Id == (int)BusinessVariables.EnumSubRoles.CuartoNivel)
                                ? "Si" : "No";
                        add.Activo = usuario.Activo ? "Si" : "No";
                        result.Add(add);
                        //    usuario.OrganizacionFinal = new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(usuario.Id, true);
                        //    usuario.OrganizacionCompleta = new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(usuario.Id, false);
                        //    usuario.UbicacionFinal = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(usuario.Id, true);
                        //    usuario.UbicacionCompleta = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(usuario.Id, false);
                        //result.Add(new HelperDetalleUsuarioGrupo
                        //{
                        //    IdUsuario = usuario.Id,
                        //    NombreCompleto = usuario.NombreCompleto,
                        //    NombreUsuarioCompleto = usuario.NombreUsuario,
                        //    Supervisor = usuario.UsuarioGrupo.Where(w => w.IdGrupoUsuario == idGrupo).Select(s=>s.SubGrupoUsuario).Any() ?
                        //                usuario.UsuarioGrupo.Any(a => a.IdGrupoUsuario == idGrupo && a.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor) ? "Si" : "No" : "No",
                        //    PrimerNivel = usuario.UsuarioGrupo.Where(w => w.IdGrupoUsuario == idGrupo).Select(s=>s.SubGrupoUsuario).Any() ?
                        //                usuario.UsuarioGrupo.Any(a => a.IdGrupoUsuario == idGrupo && a.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.PrimererNivel) ? "Si" : "No" : "No",
                        //    SegundoNivel = usuario.UsuarioGrupo.Where(w => w.IdGrupoUsuario == idGrupo).Select(s=>s.SubGrupoUsuario).Any() ?
                        //                usuario.UsuarioGrupo.Any(a => a.IdGrupoUsuario == idGrupo && a.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.SegundoNivel) ? "Si" : "No" : "No",
                        //    TercerNivel = usuario.UsuarioGrupo.Where(w => w.IdGrupoUsuario == idGrupo).Select(s=>s.SubGrupoUsuario).Any() ?
                        //                usuario.UsuarioGrupo.Any(a => a.IdGrupoUsuario == idGrupo && a.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.TercerNivel) ? "Si" : "No" : "No",
                        //    CuartoNivel = usuario.UsuarioGrupo.Where(w => w.IdGrupoUsuario == idGrupo).Select(s => s.SubGrupoUsuario).Any() ? 
                        //                usuario.UsuarioGrupo.Any(a => a.IdGrupoUsuario == idGrupo && a.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.CuartoNivel) ? "Si" : "No" : "No",
                        //    Activo = usuario.Activo ? "Si" : "No"
                        //});
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public List<Usuario> ObtenerUsuariosByGrupoAgente(int idGrupo, int idNivel)
        {
            List<Usuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> idsUsuarios = (db.Usuario.Join(db.UsuarioGrupo, u => u.Id, ug => ug.IdUsuario, (u, ug) => new { u, ug })
                    .Where(@t => @t.ug.IdGrupoUsuario == idGrupo && @t.ug.SubGrupoUsuario.IdSubRol == idNivel && @t.u.Habilitado)
                    .Select(@t => @t.u.Id)).Distinct().ToList();
                result = db.Usuario.Where(w => idsUsuarios.Contains(w.Id)).ToList();
                foreach (Usuario usuario in result)
                {
                    db.LoadProperty(usuario, "TipoUsuario");
                    //    usuario.OrganizacionFinal = new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(usuario.Id, true);
                    //    usuario.OrganizacionCompleta = new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(usuario.Id, false);
                    //    usuario.UbicacionFinal = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(usuario.Id, true);
                    //    usuario.UbicacionCompleta = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(usuario.Id, false);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public List<Usuario> ObtenerUsuariosByGrupoAtencion(int idGrupo, bool insertarSeleccion)
        {
            List<Usuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = from u in db.Usuario
                          join ug in db.UsuarioGrupo on u.Id equals ug.IdUsuario
                          where ug.IdGrupoUsuario == idGrupo && u.Habilitado
                          select u;
                result = new List<Usuario>();
                foreach (Usuario usuario in qry)
                {
                    result.Add(new Usuario
                    {
                        Id = usuario.Id,
                        Nombre = usuario.Nombre,
                        ApellidoMaterno = usuario.ApellidoMaterno,
                        ApellidoPaterno = usuario.ApellidoPaterno
                    });
                }
                //result = qry.Select(s => new Usuario
                //    {
                //        Id = s.Id,
                //        Nombre = s.Nombre,
                //        ApellidoMaterno = s.ApellidoMaterno,
                //        ApellidoPaterno = s.ApellidoPaterno
                //    }).ToList();
                //result = (db.Usuario.Join(db.UsuarioGrupo, u => u.Id, ug => ug.IdUsuario, (u, ug) => new { u, ug })
                //    .Where(@t => @t.ug.IdGrupoUsuario == idGrupo)
                //    .Select(@t => new Usuario
                //    {
                //        Id = @t.u.Id,
                //        Nombre = @t.u.Nombre,
                //        ApellidoMaterno = @t.u.ApellidoMaterno,
                //        ApellidoPaterno = @t.u.ApellidoPaterno

                //    })).Distinct().ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Usuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Nombre = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
                        });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public Usuario ObtenerDetalleUsuario(int idUsuario)
        {
            Usuario result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Usuario.SingleOrDefault(s => s.Id == idUsuario);
                if (result != null)
                {
                    db.LoadProperty(result, "BitacoraAcceso");
                    db.LoadProperty(result, "CorreoUsuario");
                    db.LoadProperty(result, "Puesto");
                    db.LoadProperty(result, "TelefonoUsuario");
                    db.LoadProperty(result, "PreguntaReto");
                    db.LoadProperty(result, "UsuarioRol");
                    db.LoadProperty(result, "TipoUsuario");
                    db.LoadProperty(result, "UsuarioGrupo");
                    db.LoadProperty(result, "TicketsLevantados");

                    result.Organizacion = new BusinessOrganizacion().ObtenerOrganizacionById(result.IdOrganizacion);
                    result.Ubicacion = new BusinessUbicacion().ObtenerUbicacionById(result.IdUbicacion);
                    result.OrganizacionFinal = new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(result.Id, true);
                    result.OrganizacionCompleta = new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(result.Id, false);
                    result.UbicacionFinal = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(result.Id, true);
                    result.UbicacionCompleta = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(result.Id, false);
                    foreach (TelefonoUsuario telefono in result.TelefonoUsuario)
                    {
                        db.LoadProperty(telefono, "TipoTelefono");
                    }
                    foreach (var rol in result.UsuarioRol)
                    {
                        db.LoadProperty(rol, "RolTipoUsuario");
                        db.LoadProperty(rol.RolTipoUsuario, "Rol");
                    }
                    foreach (UsuarioGrupo grupo in result.UsuarioGrupo)
                    {
                        db.LoadProperty(grupo, "GrupoUsuario");
                        db.LoadProperty(grupo, "SubGrupoUsuario");
                        if (grupo.SubGrupoUsuario != null)
                            db.LoadProperty(grupo.SubGrupoUsuario, "SubRol");
                    }
                    result.FechaUltimoAccesoExito = new BusinessUsuarios().ObtenerFechaUltimoAcceso(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public HelperUsuario ObtenerDatosTicketUsuario(int idUsuario)
        {
            HelperUsuario result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Usuario datosUsuario = db.Usuario.SingleOrDefault(s => s.Id == idUsuario);
                if (datosUsuario != null)
                {
                    result = new HelperUsuario();

                    result.IdUsuario = datosUsuario.Id;
                    result.NombreCompleto = string.Format("{0} {1} {2}", datosUsuario.ApellidoPaterno, datosUsuario.ApellidoMaterno, datosUsuario.Nombre);
                    result.TipoUsuarioDescripcion = datosUsuario.TipoUsuario.Descripcion;
                    result.Vip = datosUsuario.Vip;
                    result.FechaUltimoLogin = datosUsuario.BitacoraAcceso != null && datosUsuario.BitacoraAcceso.Count > 0 ? datosUsuario.BitacoraAcceso.Last().Fecha.ToString("dd/MM/yyyy HH:mm") : "";
                    result.NumeroTicketsAbiertos = datosUsuario.TicketsLevantados != null ? datosUsuario.TicketsLevantados.Count : 0;
                    result.TicketsAbiertos = datosUsuario.TicketsLevantados != null && datosUsuario.TicketsLevantados.Count > 0 ? new List<HelperTicketsUsuario>() : null;
                    if (datosUsuario.TicketsLevantados != null)
                        if (datosUsuario.TicketsLevantados != null)
                        {
                            result.NumeroTicketsAbiertos = datosUsuario.TicketsLevantados != null ? datosUsuario.TicketsLevantados.Count : 0;
                            result.TicketsAbiertos = datosUsuario.TicketsLevantados.Count > 0 ? new List<HelperTicketsUsuario>() : null;
                            result.NumeroTicketsAbiertos = datosUsuario.TicketsLevantados != null ? datosUsuario.TicketsLevantados.Count : 0;
                            result.TicketsAbiertos = datosUsuario.TicketsLevantados != null && datosUsuario.TicketsLevantados.Count > 0 ? new List<HelperTicketsUsuario>() : null;
                            if (datosUsuario.TicketsLevantados != null)
                                if (result.TicketsAbiertos != null)
                                    foreach (Ticket t in datosUsuario.TicketsLevantados)
                                    {
                                        result.TicketsAbiertos.Add(new HelperTicketsUsuario
                                        {
                                            IdTicket = t.Id,
                                            Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(t.IdArbolAcceso)
                                        });
                                    }


                        }


                    result.Puesto = datosUsuario.Puesto != null ? datosUsuario.Puesto.Descripcion : string.Empty;
                    result.Correos = datosUsuario.CorreoUsuario != null ? datosUsuario.CorreoUsuario.Select(s => s.Correo).ToList() : null;
                    result.Telefonos = datosUsuario.TelefonoUsuario.Select(s => s.Numero).ToList();
                    result.Organizacion = new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(datosUsuario.IdOrganizacion, true);
                    result.Ubicacion = new BusinessUbicacion().ObtenerDescripcionUbicacionById(datosUsuario.IdUbicacion, true);
                    TimeSpan ts = DateTime.Now - DateTime.Now.AddDays(-50);
                    result.Creado = ts.Days.ToString();
                    result.UltimaActualizacion = DateTime.Now.AddDays(-21).ToString("dd/MM/yyyy HH:mm");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public void Dispose()
        {

        }

        public List<Usuario> ObtenerAtendedoresEncuesta(int idUsuario, List<int?> encuestas)
        {
            List<Usuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug }).Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join gu in db.GrupoUsuario on tgu.IdGrupoUsuario equals gu.Id
                          join u in db.Usuario on t.IdUsuarioResolvio equals u.Id
                          where gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente
                          select new { t, e, tgu, u };

                if (!supervisor)
                    qry = from q in qry
                          where q.t.IdUsuarioResolvio == idUsuario
                          select q;

                if (encuestas.Any())
                    qry = from q in qry
                          where encuestas.Contains(q.e.Id)
                          select q;

                List<int> lstIdUsuario = qry.Select(s => s.u.Id).Distinct().ToList();

                result = db.Usuario.Where(w => lstIdUsuario.Contains(w.Id)).ToList();

                foreach (Usuario usuario in result)
                {
                    db.LoadProperty(usuario, "TipoUsuario");
                    usuario.OrganizacionFinal = new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(usuario.Id, true);
                    usuario.OrganizacionCompleta = new BusinessOrganizacion().ObtenerDescripcionOrganizacionUsuario(usuario.Id, false);
                    usuario.UbicacionFinal = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(usuario.Id, true);
                    usuario.UbicacionCompleta = new BusinessUbicacion().ObtenerDescripcionUbicacionUsuario(usuario.Id, false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public bool ValidaUserName(string nombreUsuario)
        {
            bool result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Usuario.Any(s => s.NombreUsuario == nombreUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public bool ValidaConfirmacion(int idUsuario, string guid)
        {
            bool result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Guid guidParam = Guid.Parse(guid);
                result =
                    db.UsuarioLinkPassword.Any(s => s.IdUsuario == idUsuario && s.Link == guidParam && s.IdTipoLink == (int)BusinessVariables.EnumTipoLink.Confirmacion && s.Activo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public string ValidaCodigoVerificacionSms(int idUsuario, int idTipoNotificacion, int idTelefono, string codigo)
        {
            string result = string.Empty;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                TelefonoUsuario telefono = db.TelefonoUsuario.Single(s => s.Id == idTelefono);
                if (!db.SmsService.Any(a => a.IdUsuario == idUsuario && a.IdTipoLink == idTipoNotificacion && a.Numero == telefono.Numero && a.Mensaje == codigo && a.Enviado && a.Habilitado))
                {
                    throw new Exception(string.Format("Codigo incorrecto para Numero Telefonico {0}", telefono.Numero));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public string TerminaCodigoVerificacionSms(int idUsuario, int idTipoNotificacion, int idTelefono, string codigo)
        {
            string result = string.Empty;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                TelefonoUsuario telefono = db.TelefonoUsuario.Single(s => s.Id == idTelefono);
                List<SmsService> sms = db.SmsService.Where(a => a.IdUsuario == idUsuario && a.Habilitado).ToList();
                foreach (SmsService mensaje in sms)
                {
                    mensaje.Habilitado = false;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public void EnviaCodigoVerificacionSms(int idUsuario, int idTipoNotificacion, int idTelefono)
        {
            try
            {
                Random generator = new Random();
                String codigo = generator.Next(0, 99999).ToString("D5");
                switch (idTipoNotificacion)
                {
                    case (int)BusinessVariables.EnumTipoLink.Confirmacion:
                        new BusinessDemonioSms().InsertarMensaje(idUsuario, idTipoNotificacion, idTelefono, codigo);
                        break;
                    case (int)BusinessVariables.EnumTipoLink.Reset:
                        new BusinessDemonioSms().InsertarMensaje(idUsuario, idTipoNotificacion, idTelefono, codigo);
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarTelefono(int idUsuario, int idTelefono, string numero)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                TelefonoUsuario telefono =
                    db.TelefonoUsuario.Single(
                        s =>
                            s.Id == idTelefono && s.IdUsuario == idUsuario &&
                            s.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular);
                if (telefono != null)
                {
                    telefono.Numero = numero;
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void ConfirmaCuenta(int idUsuario, string password, Dictionary<int, string> confirmaciones, List<PreguntaReto> pregunta, string link)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                db.ContextOptions.ProxyCreationEnabled = true;
                Usuario user = db.Usuario.Single(s => s.Id == idUsuario);
                if (user != null)
                {
                    Guid linkLlave = Guid.Parse(link);
                    user.UsuarioLinkPassword.Single(s => s.IdTipoLink == (int)BusinessVariables.EnumTipoLink.Confirmacion && s.IdUsuario == idUsuario && s.Link == linkLlave).Activo = false;
                    user.PreguntaReto = new List<PreguntaReto>();
                    foreach (PreguntaReto reto in pregunta)
                    {
                        db.PreguntaReto.AddObject(new PreguntaReto
                        {
                            IdUsuario = user.Id,
                            Pregunta = reto.Pregunta,
                            Respuesta = SecurityUtils.CreateShaHash(reto.Respuesta)
                        });
                    }
                    if (db.ParametrosGenerales.First().StrongPassword)
                        user.UsuarioPassword = new List<UsuarioPassword>
                    {
                        new UsuarioPassword
                        {
                            Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),"yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            Password = SecurityUtils.CreateShaHash(password)
                        }
                    };
                    user.Password = SecurityUtils.CreateShaHash(password);
                    user.Activo = true;
                    db.SaveChanges();
                    foreach (KeyValuePair<int, string> confirmacion in confirmaciones)
                    {
                        new BusinessDemonioSms().ConfirmaMensaje(user.Id,
                            (int)BusinessVariables.EnumTipoLink.Confirmacion, confirmacion.Key);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public string EnviaCodigoVerificacionCorreo(int idUsuario, int idTipoNotificacion, int idCorreo)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            string result = null;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Random generator = new Random();
                String codigo = generator.Next(0, 99999).ToString("D5");
                Guid g = Guid.NewGuid();
                ParametroCorreo correo = db.ParametroCorreo.SingleOrDefault(s => s.IdTipoCorreo == (int)BusinessVariables.EnumTipoCorreo.RecuperarCuenta && s.Habilitado);
                if (correo != null)
                {

                    string to = db.CorreoUsuario.Single(s => s.Id == idCorreo).Correo;
                    db.LoadProperty(correo, "TipoCorreo");
                    Usuario usuario = db.Usuario.Single(u => u.Id == idUsuario);
                    db.LoadProperty(usuario, "CorreoUsuario");
                    String body = string.Format(correo.Contenido, usuario.NombreCompleto, ConfigurationManager.AppSettings["siteUrl"] + "/FrmRecuperar.aspx?confirmacionCodigo=" + BusinessQueryString.Encrypt(idUsuario + "_" + g) + "&correo=" + BusinessQueryString.Encrypt(idCorreo.ToString()) + "&code=" + BusinessQueryString.Encrypt(codigo), codigo);
                    BusinessCorreo.SendMail(to, correo.TipoCorreo.Descripcion, body);
                    usuario.UsuarioLinkPassword = new List<UsuarioLinkPassword>
                    {
                        new UsuarioLinkPassword
                        {
                            Activo = true,
                            Link = g,
                            Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            IdTipoLink = (int) BusinessVariables.EnumTipoLink.Reset,
                            Codigo = codigo
                        }
                    };
                    db.SaveChanges();
                    result = g.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public void ValidaCodigoVerificacionCorreo(int idUsuario, int idTipoNotificacion, string link, int idCorreo, string codigo)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                CorreoUsuario telefono = db.CorreoUsuario.Single(s => s.Id == idCorreo);
                Guid guidLink = Guid.Parse(link);
                if (!db.UsuarioLinkPassword.Any(a => a.IdUsuario == idUsuario && a.IdTipoLink == idTipoNotificacion && a.Link == guidLink && a.Codigo == codigo && a.Activo))
                {
                    throw new Exception(string.Format("Codigo incorrecto {0}", telefono.Correo));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void TerminaCodigoVerificacionCorreo(int idUsuario, int idTipoNotificacion, string link, int idCorreo, string codigo)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                CorreoUsuario telefono = db.CorreoUsuario.Single(s => s.Id == idCorreo);
                Guid guidLink = Guid.Parse(link);
                List<UsuarioLinkPassword> links = db.UsuarioLinkPassword.Where(a => a.IdUsuario == idUsuario && a.Activo).ToList();
                foreach (UsuarioLinkPassword linkValue in links)
                {
                    linkValue.Activo = false;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public Usuario BuscarUsuario(string usuario)
        {
            Usuario result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                int idUsuario;
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                if (db.CorreoUsuario.Any(w => w.Correo == usuario))
                {
                    idUsuario = db.CorreoUsuario.First(w => w.Correo == usuario).IdUsuario;
                    result = db.Usuario.SingleOrDefault(s => s.Id == idUsuario);
                }
                else if (db.TelefonoUsuario.Any(w => w.Numero == usuario && w.Obligatorio))
                {
                    idUsuario = db.TelefonoUsuario.First(w => w.Numero == usuario).IdUsuario;
                    result = db.Usuario.SingleOrDefault(s => s.Id == idUsuario);
                }
                else if (db.Usuario.Any(w => w.NombreUsuario == usuario))
                {
                    idUsuario = db.Usuario.First(w => w.NombreUsuario == usuario).Id;
                    result = db.Usuario.SingleOrDefault(s => s.Id == idUsuario);
                }
                db.LoadProperty(result, "PreguntaReto");
                db.LoadProperty(result, "TelefonoUsuario");
                db.LoadProperty(result, "CorreoUsuario");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public List<Usuario> BuscarUsuarios(string usuario)
        {
            List<Usuario> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                if (usuario.Trim() == string.Empty)
                {
                    throw new Exception("Especifique nombre de usuario");
                }
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = new List<Usuario>();
                int idUsuario;
                if (db.Usuario.Any(a => a.NombreUsuario == usuario))
                {
                    result.Add(db.Usuario.First(a => a.NombreUsuario == usuario));
                }
                else if (db.CorreoUsuario.Any(a => a.Correo == usuario))
                {
                    idUsuario = db.CorreoUsuario.First(a => a.Correo == usuario).IdUsuario;
                    result.Add(db.Usuario.First(f => f.Id == idUsuario));
                }
                else if (db.TelefonoUsuario.Any(a => a.Numero == usuario))
                {
                    idUsuario = db.TelefonoUsuario.First(a => a.Numero == usuario).IdUsuario;
                    result.Add(db.Usuario.First(f => f.Id == idUsuario));
                }
                //var qry = from u in db.Usuario
                //    join cu in db.CorreoUsuario on u.Id equals cu.IdUsuario
                //    join tu in db.TelefonoUsuario on u.Id equals tu.IdUsuario
                //    where u.NombreUsuario == usuario || cu.Correo == usuario || tu.Numero == usuario 

                //;
                //result = db.Usuario.Join(db.CorreoUsuario, u => u.Id, cu => cu.IdUsuario, (u, cu) => new { u, cu })
                //    .Join(db.TelefonoUsuario, @t => @t.u.Id, tu => tu.IdUsuario, (@t, tu) => new { @t, tu })
                //    .Where(@t => @t.@t.cu.Correo == usuario || @t.tu.Numero == usuario || @t.@t.u.NombreUsuario == usuario)
                //    .Select(@t => @t.@t.u).Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public void ValidaRespuestasReto(int idUsuario, Dictionary<int, string> preguntasReto)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                foreach (KeyValuePair<int, string> pregunta in preguntasReto)
                {
                    string respuesta = SecurityUtils.CreateShaHash(pregunta.Value);
                    if (!db.PreguntaReto.Any(w => w.IdUsuario == idUsuario && w.Id == pregunta.Key && w.Respuesta == respuesta))
                        throw new Exception("Verifique respuestas");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public string ObtenerFechaUltimoAcceso(Usuario usuario)
        {
            string fecha = "Hoy";
            try
            {
                CultureInfo ci = new CultureInfo("Es-Es");
                if (usuario.BitacoraAcceso.Any())
                {
                    var days = (DateTime.Now - usuario.BitacoraAcceso.Last(l => l.Success).Fecha).TotalDays;
                    switch (int.Parse(Math.Abs(Math.Round(days)).ToString()))
                    {
                        case 0:
                            fecha = "Hoy";
                            break;
                        case 1:
                            fecha = "Ayer";
                            break;
                        case 2:
                            fecha =
                                ci.DateTimeFormat.GetDayName(usuario.BitacoraAcceso.Last(l => l.Success).Fecha.DayOfWeek).ToString();
                            break;
                        case 3:
                            fecha =
                                ci.DateTimeFormat.GetDayName(usuario.BitacoraAcceso.Last(l => l.Success).Fecha.DayOfWeek).ToString();
                            break;
                        case 4:
                            fecha =
                                ci.DateTimeFormat.GetDayName(usuario.BitacoraAcceso.Last(l => l.Success).Fecha.DayOfWeek).ToString();
                            break;
                        case 5:
                            fecha =
                                ci.DateTimeFormat.GetDayName(usuario.BitacoraAcceso.Last(l => l.Success).Fecha.DayOfWeek).ToString();
                            break;
                        case 6:
                            fecha =
                                ci.DateTimeFormat.GetDayName(usuario.BitacoraAcceso.Last(l => l.Success).Fecha.DayOfWeek).ToString();
                            break;
                        default:
                            fecha = usuario.BitacoraAcceso.Last(l => l.Success).Fecha.ToString("dd-MM-yy");
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return string.Format("{0} {1} hrs.", fecha, usuario.BitacoraAcceso.Any() ? usuario.BitacoraAcceso.Last(l => l.Success).Fecha.ToString("HH:mm") : DateTime.Now.ToString("HH:mm"));
        }

        public Usuario GetUsuarioByCorreo(string correo)
        {
            Usuario result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Usuario.Join(db.CorreoUsuario, u => u.Id, cu => cu.IdUsuario, (u, cu) => new { u, cu })
                    .Where(@t => @t.cu.Correo == correo & @t.cu.Obligatorio)
                    .Select(@t => @t.u).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }
    }
}

