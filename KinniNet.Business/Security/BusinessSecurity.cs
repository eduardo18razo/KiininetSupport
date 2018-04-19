using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Operacion;
using KinniNet.Data.Help;

namespace KinniNet.Core.Security
{
    public class BusinessSecurity : IDisposable
    {
        public void Dispose()
        {

        }
        public class Autenticacion : IDisposable
        {
            private readonly bool _proxy;
            public Autenticacion(bool proxy = false)
            {
                _proxy = proxy;
            }

            public void Dispose()
            {

            }

            public bool Autenticate(string user, string password)
            {
                DesbloqueaUsuarios();
                bool result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    string hashedPdw = SecurityUtils.CreateShaHash(password);
                    var qry = (from u in db.Usuario
                               join tu in db.TelefonoUsuario on u.Id equals tu.IdUsuario
                               join cu in db.CorreoUsuario on u.Id equals cu.IdUsuario
                               where u.NombreUsuario == user || tu.Numero == user || cu.Correo == user
                               select u.Id).Distinct().ToList();
                    if (qry.Count > 1)
                        throw new Exception("Error al ingresar consulte a su administrador.");
                    if (qry.Count <= 0)
                        throw new Exception("Usuario y/o Contraseña incorrectos.");

                    int idUsuario = qry[0];
                    Usuario usuario = db.Usuario.SingleOrDefault(s => s.Id == idUsuario && s.Activo && s.Habilitado);
                    result = usuario != null;
                    if (usuario != null)
                    {
                        if (db.ParametrosGenerales.First().StrongPassword)
                        {
                            if (usuario.FechaBloqueo == null)
                            {
                                if (usuario.Password != hashedPdw)
                                {
                                    usuario.Tries++;
                                    if (usuario.Tries >= db.ParametroPassword.First().Tries)
                                        usuario.FechaBloqueo = DateTime.ParseExact(DateTime.Now.AddMinutes(db.ParametroPassword.First().TimeoutFail).ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                                    db.SaveChanges();
                                    throw new Exception("Usuario y/o Contraseña incorrectos.");
                                }
                            }
                            else
                                throw new Exception(string.Format("Usuario bloqueado espere {0} minutos",
                                    db.ParametroPassword.First().TimeoutFail));
                        }
                        usuario.Tries = 0;
                        usuario.FechaBloqueo = null;
                        db.BitacoraAcceso.AddObject(new BitacoraAcceso
                        {
                            IdUsuario = usuario.Id,
                            Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            Success = true
                        });
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Usuario y/o Contraseña incorrectos.");
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

            public List<Rol> ObtenerRolesUsuario(int idUsuario)
            {
                List<Rol> result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    result = db.UsuarioRol.Join(db.RolTipoUsuario, ur => ur.IdRolTipoUsuario, rtu => rtu.Id,
                            (ur, rtu) => new { ur, rtu })
                            .Join(db.Rol, @t => @t.rtu.IdRol, r => r.Id, (@t, r) => new { @t, r })
                            .Where(@t => @t.@t.ur.IdUsuario == idUsuario)
                            .Select(@t => @t.r).Distinct().ToList();
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

            public Usuario GetUserDataAutenticate(string user, string password)
            {
                Usuario result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    string hashedPdw = SecurityUtils.CreateShaHash(password);
                    var qry = (from u in db.Usuario
                               join tu in db.TelefonoUsuario on u.Id equals tu.IdUsuario
                               join cu in db.CorreoUsuario on u.Id equals cu.IdUsuario
                               where u.NombreUsuario == user || tu.Numero == user || cu.Correo == user
                               select u.Id).Distinct().ToList();
                    if (qry.Count() > 1)
                        throw new Exception("Error al ingresar consulte a su administrador.");
                    int idUsuario = qry[0];
                    result = db.Usuario.SingleOrDefault(s => s.Id == idUsuario && s.Password == hashedPdw && s.Activo && s.Habilitado);
                    if (result != null)
                    {
                        db.LoadProperty(result, "Organizacion");
                        db.LoadProperty(result, "Ubicacion");
                        db.LoadProperty(result, "TipoUsuario");
                        db.LoadProperty(result, "UsuarioRol");
                        db.LoadProperty(result, "BitacoraAcceso");
                        foreach (UsuarioRol rol in result.UsuarioRol)
                        {
                            db.LoadProperty(rol, "RolTipoUsuario");
                        }
                        db.LoadProperty(result, "UsuarioGrupo");
                        foreach (UsuarioGrupo grupo in result.UsuarioGrupo)
                        {
                            db.LoadProperty(grupo, "GrupoUsuario");
                            db.LoadProperty(grupo, "SubGrupoUsuario");
                        }
                        result.Supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                       .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == result.Id);
                        result.Administrador = result.UsuarioRol.Any(a => a.RolTipoUsuario.IdRol == (int)BusinessVariables.EnumRoles.Administrador);
                        result.FechaUltimoAccesoExito = new BusinessUsuarios().ObtenerFechaUltimoAcceso(result);
                        var levantaTicket = (from u in db.Usuario
                                             join ug in db.UsuarioGrupo on u.Id equals ug.IdUsuario
                                             join gu in db.GrupoUsuario on ug.IdGrupoUsuario equals gu.Id
                                             join sgu in db.SubGrupoUsuario on gu.Id equals sgu.IdGrupoUsuario
                                             join etsrg in db.EstatusTicketSubRolGeneral on new { rolSolicita = ug.IdRol, subRolSolicita = sgu.IdSubRol, gpo = gu.Id, super = gu.TieneSupervisor } equals new { rolSolicita = etsrg.IdRolSolicita, subRolSolicita = (int)etsrg.IdSubRolSolicita, gpo = etsrg.IdGrupoUsuario, super = (bool)etsrg.TieneSupervisor }
                                             where u.Id == result.Id && etsrg.LevantaTicket == true
                                             select etsrg).FirstOrDefault();
                        if (levantaTicket != null)
                            result.LevantaTickets = levantaTicket.LevantaTicket ?? false;
                        else
                        {
                            result.LevantaTickets = result.UsuarioGrupo.Any(grupo => grupo.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && grupo.GrupoUsuario.LevantaTicket);
                        }
                        if (!result.LevantaTickets)
                            result.LevantaRecado = result.UsuarioGrupo.Any(grupo => grupo.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && grupo.GrupoUsuario.RecadoTicket);
                    }
                    else
                    {
                        throw new Exception("Usuario y/o Contraseña incorrectos.");
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

            public Usuario GetUserInvitadoDataAutenticate(int idTipoUsuario)
            {
                Usuario result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    if (db.Usuario.Count(w => w.IdTipoUsuario == idTipoUsuario && w.Habilitado && w.Activo) > 1)
                        throw new Exception("Error al obtener informacion consulte a su Administrador");
                    result = db.Usuario.SingleOrDefault(w => w.IdTipoUsuario == idTipoUsuario && w.Habilitado && w.Activo);
                    if (result != null)
                    {
                        db.LoadProperty(result, "Organizacion");
                        db.LoadProperty(result, "Ubicacion");
                        db.LoadProperty(result, "TipoUsuario");
                        db.LoadProperty(result, "UsuarioRol");
                        foreach (UsuarioRol rol in result.UsuarioRol)
                        {
                            db.LoadProperty(rol, "RolTipoUsuario");
                        }
                        db.LoadProperty(result, "UsuarioGrupo");
                        foreach (UsuarioGrupo grupo in result.UsuarioGrupo)
                        {
                            db.LoadProperty(grupo, "SubGrupoUsuario");
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

            public void ChangePassword(int idUsuario, string contrasenaActual, string contrasenaNueva)
            {
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    string hashedActualPdw = SecurityUtils.CreateShaHash(contrasenaActual);
                    string hashedNewPdw = SecurityUtils.CreateShaHash(contrasenaNueva);
                    Usuario user = db.Usuario.SingleOrDefault(w => w.Id == idUsuario && w.Habilitado);
                    if (user != null)
                    {
                        ParametrosGenerales parametrosG = db.ParametrosGenerales.FirstOrDefault();
                        if (parametrosG != null)
                        {
                            if (parametrosG.StrongPassword)
                            {

                            }
                            if (db.ParametroPassword.First().CaducaPassword)
                                user.FechaUpdate = DateTime.ParseExact(DateTime.Now.AddDays(db.ParametroPassword.First().TiempoCaducidad).ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                            if (db.UsuarioPassword.Any(a => a.IdUsuario == idUsuario && a.Password == hashedNewPdw))
                                throw new Exception("Contraseña antigua intente con una diferente");
                            if (user.Password != hashedActualPdw)
                                throw new Exception("Contraseña actual incorrecta");
                            user.Password = hashedNewPdw;
                            user.UsuarioPassword = new List<UsuarioPassword>
                        {
                            new UsuarioPassword
                            {
                                Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                Password = hashedNewPdw
                            }
                        };
                            db.SaveChanges();
                            LimpiaPasswordsAntiguos(idUsuario);
                        }
                        else
                            throw new Exception("Error al cambiar contraseña. Consulte a su Administrador");


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

            public void RecuperarCuenta(int idUsuario, int idTipoNotificacion, string link, int idCorreo, string codigo, string contrasena, string tipoRecuperacion)
            {
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    switch (int.Parse(tipoRecuperacion))
                    {
                        case 0:
                            new BusinessUsuarios().ValidaCodigoVerificacionCorreo(idUsuario, idTipoNotificacion, link, idCorreo, codigo);
                            new BusinessUsuarios().TerminaCodigoVerificacionCorreo(idUsuario, idTipoNotificacion, link, idCorreo, codigo);
                            break;
                        case 1:
                            new BusinessUsuarios().ValidaCodigoVerificacionSms(idUsuario, idTipoNotificacion, idCorreo, codigo);
                            new BusinessUsuarios().TerminaCodigoVerificacionSms(idUsuario, idTipoNotificacion, idCorreo, codigo);
                            break;
                    }

                    string hashedPdw = SecurityUtils.CreateShaHash(contrasena);
                    Usuario user = db.Usuario.SingleOrDefault(w => w.Id == idUsuario && w.Habilitado);
                    if (user != null)
                    {
                        ParametrosGenerales parametrosG = db.ParametrosGenerales.First();
                        if (parametrosG.StrongPassword)
                        {
                            if (db.ParametroPassword.First().CaducaPassword)
                                user.FechaUpdate = DateTime.ParseExact(DateTime.Now.AddDays(db.ParametroPassword.First().TiempoCaducidad).ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                            if (db.UsuarioPassword.Any(a => a.IdUsuario == idUsuario && a.Password == hashedPdw))
                                throw new Exception("Contraseña antigua intente con una diferente");
                        }
                        user.Password = hashedPdw;
                        user.UsuarioPassword = new List<UsuarioPassword>
                        {
                            new UsuarioPassword
                            {
                                Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                Password = SecurityUtils.CreateShaHash(contrasena)
                            }
                        };
                        db.SaveChanges();
                    }
                    LimpiaPasswordsAntiguos(idUsuario);
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

            public void ValidaPassword(string pwd)
            {
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    if (db.ParametrosGenerales.First().StrongPassword)
                    {
                        ParametroPassword parametros = db.ParametroPassword.FirstOrDefault();
                        if (parametros != null)
                        {
                            if (!(pwd.Length >= parametros.Min && pwd.Length <= parametros.Max))
                                throw new Exception(string.Format("El password debe contener entre {0} y {1} caracteres", parametros.Min, parametros.Max));
                            if (parametros.Letras)
                                if (!(Regex.Matches(pwd, @"[a-zA-Z]").Count > 0))
                                    throw new Exception(string.Format("El password debe contener caracteres alfanumericos"));

                            if (parametros.Numeros)
                                if (!(Regex.Matches(pwd, @"[0-9]").Count > 0))
                                    throw new Exception(string.Format("El password debe contener caracteres numericos"));

                            if (parametros.Especiales)
                                if (!(Regex.Matches(pwd, "[^a-z0-9]", RegexOptions.IgnoreCase).Count > 0))
                                    throw new Exception(string.Format("El password debe contener caracteres especiales"));

                            if (parametros.Mayusculas > 0)
                                if (!(Regex.Matches(pwd, "[A-Z]").Count >= parametros.Mayusculas))
                                    throw new Exception(string.Format("El password debe contener {0} mayusculas", parametros.Mayusculas));

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

            public void LimpiaPasswordsAntiguos(int idUsuario)
            {
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    List<UsuarioPassword> lst = db.UsuarioPassword.Where(w => w.IdUsuario == idUsuario).ToList();
                    int diferencia = lst.Count - db.ParametroPassword.First().AlmacenAnterior;
                    lst = lst.OrderBy(o => o.Fecha).ToList();
                    foreach (UsuarioPassword usuarioPassword in lst)
                    {
                        if (diferencia > 0)
                        {
                            db.UsuarioPassword.DeleteObject(usuarioPassword);
                            diferencia--;
                        }
                        else
                            break;
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

            public void DesbloqueaUsuarios()
            {
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    DateTime? fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    List<Usuario> usuariosBloqueados = db.Usuario.Where(w => w.FechaBloqueo != null && w.FechaBloqueo <= fecha).ToList();
                    foreach (Usuario usuario in usuariosBloqueados)
                    {
                        usuario.FechaBloqueo = null;
                        usuario.Tries = 0;
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

            public bool CaducaPassword(int idUsuario)
            {
                bool result = false;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    ParametroPassword parametrosPassword = db.ParametroPassword.FirstOrDefault();
                    if (parametrosPassword != null)
                    {
                        if (parametrosPassword.CaducaPassword)
                        {
                            DateTime? fecha = DateTime.ParseExact(DateTime.Now.AddDays((parametrosPassword.TiempoCaducidad*-1)).ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff",CultureInfo.InvariantCulture);
                            if (db.UsuarioPassword.Any(a => a.IdUsuario == idUsuario && a.Fecha >= fecha))
                            {
                                result = false;
                            }
                            else
                            {
                                result = true;
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
                return result;
            }
        }

        public class Menus : IDisposable
        {
            private readonly bool _proxy;

            public Menus(bool proxy = false)
            {
                _proxy = proxy;
            }

            public void Dispose()
            {

            }

            public List<Menu> ObtenerMenuUsuario(int idUsuario, int idRol, bool arboles)
            {
                List<Menu> result = new List<Menu>();
                DataBaseModelContext db = new DataBaseModelContext();
                const string menuPadre = "Menu2";
                const string menuHijo = "Menu1";
                try
                {
                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    IQueryable<Menu> qry = from ur in db.UsuarioRol
                                           join rtu in db.RolTipoUsuario on ur.IdRolTipoUsuario equals rtu.Id
                                           join rm in db.RolMenu on rtu.IdRol equals rm.IdRol
                                           join m in db.Menu on rm.IdMenu equals m.Id
                                           where ur.IdUsuario == idUsuario && m.Habilitado && rm.IdRol == idRol
                                           orderby m.Orden
                                           select m;
                    List<Menu> menusConsulta = qry.Where(w => w.Id == (int)BusinessVariables.EnumMenu.Consultas || w.Id == (int)BusinessVariables.EnumMenu.Servicio
                                           || w.Id == (int)BusinessVariables.EnumMenu.Incidentes).OrderBy(o => o.Id).Distinct().ToList();

                    var menusvalidos = qry.Where(w => w.IdPadre == null && w.Id != (int)BusinessVariables.EnumMenu.Consultas && w.Id != (int)BusinessVariables.EnumMenu.Servicio && w.Id != (int)BusinessVariables.EnumMenu.Incidentes).OrderBy(o => o.Orden).ThenBy(t => t.Id).Distinct();
                    foreach (Menu menu in qry.Where(w => w.IdPadre == null && w.Id != (int)BusinessVariables.EnumMenu.Consultas && w.Id != (int)BusinessVariables.EnumMenu.Servicio && w.Id != (int)BusinessVariables.EnumMenu.Incidentes).OrderBy(o => o.Orden).ThenBy(t => t.Id).Distinct())
                    {
                        result.Add(menu);
                        db.LoadProperty(menu, menuHijo);
                        if (menu.Menu1 != null && menu.Menu1.Count == 0) menu.Menu1 = null;
                        if (menu.Menu1 == null) continue;
                        foreach (Menu menu1 in menu.Menu1)
                        {
                            db.LoadProperty(menu1, menuHijo);
                            if (menu1.Menu1 != null && menu1.Menu1.Count == 0) menu1.Menu1 = null;
                            if (menu1.Menu1 == null) continue;
                            foreach (Menu menu2 in menu1.Menu1)
                            {
                                db.LoadProperty(menu2, menuHijo);
                                if (menu2.Menu1 != null && menu2.Menu1.Count == 0) menu2.Menu1 = null;
                                if (menu2.Menu1 == null) continue;
                                foreach (Menu menu3 in menu2.Menu1)
                                {
                                    db.LoadProperty(menu3, menuHijo);
                                    if (menu3.Menu1 != null && menu3.Menu1.Count == 0) menu3.Menu1 = null;
                                }
                            }
                        }
                    }
                    foreach (Menu menu in qry.Where(w => w.IdPadre != null).OrderBy(o => o.Orden).ThenBy(t => t.Id).Distinct())
                    {
                        db.LoadProperty(menu, menuPadre);
                        if (menu.Menu2 != null)
                        {
                            db.LoadProperty(menu.Menu2, menuPadre);
                            if (menu.Menu2.Menu2 != null)
                            {
                                db.LoadProperty(menu.Menu2.Menu2, menuPadre);
                                result.Add(menu.Menu2.Menu2.Menu2 ?? menu.Menu2.Menu2);
                            }
                            else
                                result.Add(menu.Menu2);
                        }
                        else
                            result.Add(menu);
                    }

                    result = result.Distinct().ToList();
                    result = result.OrderBy(o => o.Orden).ToList();
                    if (result.Any(a => a.Id == (int)BusinessVariables.EnumMenu.Admin))
                    {
                        result.Single(a => a.Id == (int)BusinessVariables.EnumMenu.Admin).Menu1 = result.Single(a => a.Id == (int)BusinessVariables.EnumMenu.Admin).Menu1.OrderBy(o => o.Orden).ToList();
                    }
                    if (result.Any(a => a.Id == (int)BusinessVariables.EnumMenu.Atención))
                    {
                        result.Single(a => a.Id == (int)BusinessVariables.EnumMenu.Atención).Menu1 = result.Single(a => a.Id == (int)BusinessVariables.EnumMenu.Atención).Menu1.OrderBy(o => o.Orden).ToList();
                    }
                    if (result.Any(a => a.Id == (int)BusinessVariables.EnumMenu.HelpCenter))
                    {
                        result.Single(a => a.Id == (int)BusinessVariables.EnumMenu.HelpCenter).Menu1 = result.Single(a => a.Id == (int)BusinessVariables.EnumMenu.HelpCenter).Menu1.OrderBy(o => o.Orden).ToList();
                    }

                    if (result.Any(a => a.Id == (int)BusinessVariables.EnumMenu.View))
                    {
                        result.Single(a => a.Id == (int)BusinessVariables.EnumMenu.View).Menu1 = result.Single(a => a.Id == (int)BusinessVariables.EnumMenu.View).Menu1.OrderBy(o => o.Orden).ToList();
                    }

                    List<Area> lstAreas = new BusinessArea().ObtenerAreasUsuarioByIdRol(idUsuario, idRol, false).Distinct().ToList();
                    
                    result.AddRange(lstAreas.Select(s => new Menu { Id = s.Id, Descripcion = s.Descripcion, Url = "DELSYSTEM" }).Distinct().ToList());
                    if (arboles)
                        foreach (Area area in lstAreas)
                        {
                            Menu menuToAdd = null;
                            List<ArbolAcceso> lstArboles;
                            if (menusConsulta.Any(a => a.Id == (int)BusinessVariables.EnumMenu.Consultas))
                            {
                                lstArboles = new BusinessArbolAcceso().ObtenerArbolesAccesoByUsuarioTipoArbol(idUsuario, (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion, area.Id).Where(w => !w.Sistema).Distinct().ToList();
                                if (lstArboles.Any())
                                {
                                    menuToAdd = result.Single(s => s.Id == area.Id && s.Url == "DELSYSTEM");
                                    GeneraSubMenus(menuToAdd, lstArboles, db, "~/Users/General/FrmNodoConsultas.aspx?IdArbol=");
                                }
                            }
                            if (menusConsulta.Any(a => a.Id == (int)BusinessVariables.EnumMenu.Servicio))
                            {
                                lstArboles = new BusinessArbolAcceso().ObtenerArbolesAccesoByUsuarioTipoArbol(idUsuario, (int)BusinessVariables.EnumTipoArbol.SolicitarServicio, area.Id).Where(w => !w.Sistema).Distinct().ToList();
                                if (lstArboles.Any())
                                {
                                    menuToAdd = result.Single(s => s.Id == area.Id && s.Url == "DELSYSTEM");
                                    GeneraSubMenus(menuToAdd, lstArboles, db, "~/Users/Ticket/FrmTicket.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=");
                                }
                            }
                            if (menusConsulta.Any(a => a.Id == (int)BusinessVariables.EnumMenu.Incidentes))
                            {
                                lstArboles = new BusinessArbolAcceso().ObtenerArbolesAccesoByUsuarioTipoArbol(idUsuario, (int)BusinessVariables.EnumTipoArbol.ReportarProblemas, area.Id).Where(w => !w.Sistema).Distinct().ToList();
                                if (lstArboles.Any())
                                {
                                    menuToAdd = result.Single(s => s.Id == area.Id && s.Url == "DELSYSTEM");
                                    GeneraSubMenus(menuToAdd, lstArboles, db, "~/Users/Ticket/FrmTicket.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=");
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
                return result;
            }

            public List<Menu> ObtenerMenuPublico(int idTipoUsuario, int idArea, bool arboles)
            {
                List<Menu> result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    IQueryable<Menu> qry = from rtu in db.RolTipoUsuario
                                           join rm in db.RolMenu on new { rol = rtu.IdRol, tu = rtu.IdTipoUsuario } equals new { rol = rm.IdRol, tu = rm.IdTipoUsuario }
                                           join m in db.Menu on rm.IdMenu equals m.Id
                                           where rtu.IdTipoUsuario == idTipoUsuario && m.Habilitado
                                           && (m.Id == (int)BusinessVariables.EnumMenu.Consultas || m.Id == (int)BusinessVariables.EnumMenu.Servicio
                                           || m.Id == (int)BusinessVariables.EnumMenu.Incidentes)
                                           select m;
                    List<Menu> menusConsulta = qry.Where(w => w.Id == (int)BusinessVariables.EnumMenu.Consultas || w.Id == (int)BusinessVariables.EnumMenu.Servicio
                                           || w.Id == (int)BusinessVariables.EnumMenu.Incidentes).OrderBy(o => o.Id).Distinct().ToList();
                    //result = qry.OrderBy(o => o.Id).Distinct().ToList();
                    result = qry.Where(w => w.Id != (int)BusinessVariables.EnumMenu.Consultas && w.Id != (int)BusinessVariables.EnumMenu.Servicio
                                           && w.Id != (int)BusinessVariables.EnumMenu.Incidentes).OrderBy(o => o.Id).Distinct().ToList();
                    List<Area> areas = new BusinessArea().ObtenerAreasUsuarioPublicoByIdRol((int)BusinessVariables.EnumRoles.Usuario, false).Where(w => !w.Sistema).Distinct().ToList();

                    result.AddRange(areas.Select(s => new Menu { Id = s.Id, Descripcion = s.Descripcion, Url = "DELSYSTEM" }).Distinct().ToList());
                    if (arboles)
                        foreach (Area area in areas)
                        {
                            Menu menuToAdd = null;
                            List<ArbolAcceso> lstArboles;
                            if (menusConsulta.Any(a => a.Id == (int)BusinessVariables.EnumMenu.Consultas))
                            {
                                lstArboles = new BusinessArbolAcceso().ObtenerArbolesAccesoByTipoUsuarioTipoArbol(idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion, area.Id).Where(w => !w.Sistema && w.Publico).Distinct().ToList();
                                if (lstArboles.Any())
                                {
                                    menuToAdd = result.Single(s => s.Id == area.Id && s.Url == "DELSYSTEM");
                                    GeneraSubMenus(menuToAdd, lstArboles, db, "~/Publico/FrmConsulta.aspx?IdArbol=");
                                }
                            }
                            if (menusConsulta.Any(a => a.Id == (int)BusinessVariables.EnumMenu.Servicio))
                            {
                                lstArboles = new BusinessArbolAcceso().ObtenerArbolesAccesoByTipoUsuarioTipoArbol(idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.SolicitarServicio, area.Id).Where(w => !w.Sistema && w.Publico).Distinct().ToList();
                                if (lstArboles.Any())
                                {
                                    menuToAdd = result.Single(s => s.Id == area.Id && s.Url == "DELSYSTEM");
                                    GeneraSubMenus(menuToAdd, lstArboles, db, "~/Publico/FrmTicketPublico.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=");
                                }
                            }
                            if (menusConsulta.Any(a => a.Id == (int)BusinessVariables.EnumMenu.Incidentes))
                            {
                                lstArboles = new BusinessArbolAcceso().ObtenerArbolesAccesoByTipoUsuarioTipoArbol(idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ReportarProblemas, area.Id).Where(w => !w.Sistema && w.Publico).Distinct().ToList();
                                if (lstArboles.Any())
                                {
                                    menuToAdd = result.Single(s => s.Id == area.Id && s.Url == "DELSYSTEM");
                                    GeneraSubMenus(menuToAdd, lstArboles, db, "~/Publico/FrmTicketPublico.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=");
                                }
                            }
                            if (result.Single(s => s.Id == area.Id && s.Url == "DELSYSTEM").Menu1 == null)
                            {
                                result.Remove(result.Single(s => s.Id == area.Id && s.Url == "DELSYSTEM"));
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

            private void GeneraSubMenus(Menu menu, List<ArbolAcceso> lstArboles, DataBaseModelContext db, string url)
            {
                try
                {
                    foreach (ArbolAcceso arbol in lstArboles)
                    {
                        if (arbol.Nivel1 != null)
                        {
                            if (menu.Menu1 == null)
                                menu.Menu1 = new List<Menu>();
                            Nivel1 n = db.Nivel1.SingleOrDefault(s => s.Id == arbol.Nivel1.Id);
                            if (n == null) continue;
                            if (!menu.Menu1.Any(a => a.Id == n.Id))
                            {
                                Menu menuNivel1 = new Menu
                                {
                                    Descripcion = n.Descripcion,
                                    Id = n.Id,
                                    Url = arbol.Nivel2 != null ? string.Empty : url + arbol.Id
                                };
                                menu.Menu1.Add(menuNivel1);
                            }
                        }

                        if (arbol.Nivel2 != null)
                        {
                            if (menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1 == null)
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1 = new List<Menu>();
                            Nivel2 n = db.Nivel2.SingleOrDefault(s => s.Id == arbol.Nivel2.Id);
                            if (n == null) continue;
                            if (!menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Any(a => a.Id == n.Id))
                            {
                                Menu menuNivel1 = new Menu
                                {
                                    Descripcion = n.Descripcion,
                                    Id = n.Id,
                                    Url = arbol.Nivel3 != null ? string.Empty : url + arbol.Id
                                };
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Add(menuNivel1);
                            }
                        }

                        if (arbol.Nivel3 != null)
                        {
                            if (menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1 == null)
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1 = new List<Menu>();
                            Nivel3 n = db.Nivel3.SingleOrDefault(s => s.Id == arbol.Nivel3.Id);
                            if (n == null) continue;
                            if (!menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Any(a => a.Id == n.Id))
                            {
                                Menu menuNivel1 = new Menu
                                {
                                    Descripcion = n.Descripcion,
                                    Id = n.Id,
                                    Url = arbol.Nivel4 != null ? string.Empty : url + arbol.Id
                                };
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Add(menuNivel1);
                            }
                        }

                        if (arbol.Nivel4 != null)
                        {
                            if (menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1 == null)
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1 = new List<Menu>();
                            Nivel4 n = db.Nivel4.SingleOrDefault(s => s.Id == arbol.Nivel4.Id);
                            if (n == null) continue;
                            if (!menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Any(a => a.Id == n.Id))
                            {
                                Menu menuNivel1 = new Menu
                                {
                                    Descripcion = n.Descripcion,
                                    Id = n.Id,
                                    Url = arbol.Nivel5 != null ? string.Empty : url + arbol.Id
                                };
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Add(menuNivel1);
                            }
                        }

                        if (arbol.Nivel5 != null)
                        {
                            if (menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1 == null)
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1 = new List<Menu>();
                            Nivel5 n = db.Nivel5.SingleOrDefault(s => s.Id == arbol.Nivel5.Id);
                            if (n == null) continue;
                            if (!menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Any(a => a.Id == n.Id))
                            {
                                Menu menuNivel1 = new Menu
                                {
                                    Descripcion = n.Descripcion,
                                    Id = n.Id,
                                    Url = arbol.Nivel6 != null ? string.Empty : url + arbol.Id
                                };
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Add(menuNivel1);
                            }
                        }

                        if (arbol.Nivel6 != null)
                        {
                            if (menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Single(s => s.Id == arbol.IdNivel5).Menu1 == null)
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Single(s => s.Id == arbol.IdNivel5).Menu1 = new List<Menu>();
                            Nivel6 n = db.Nivel6.SingleOrDefault(s => s.Id == arbol.Nivel6.Id);
                            if (n == null) continue;
                            if (!menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Single(s => s.Id == arbol.IdNivel5).Menu1.Any(a => a.Id == n.Id))
                            {
                                Menu menuNivel1 = new Menu
                                {
                                    Descripcion = n.Descripcion,
                                    Id = n.Id,
                                    Url = arbol.Nivel7 != null ? string.Empty : url + arbol.Id
                                };
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Single(s => s.Id == arbol.IdNivel5).Menu1.Add(menuNivel1);
                            }
                        }
                        if (arbol.Nivel7 != null)
                        {
                            if (menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Single(s => s.Id == arbol.IdNivel5).Menu1.Single(s => s.Id == arbol.IdNivel6).Menu1 == null)
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Single(s => s.Id == arbol.IdNivel5).Menu1.Single(s => s.Id == arbol.IdNivel6).Menu1 = new List<Menu>();
                            Nivel7 n = db.Nivel7.SingleOrDefault(s => s.Id == arbol.Nivel7.Id);
                            if (n == null) continue;
                            if (!menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Single(s => s.Id == arbol.IdNivel5).Menu1.Single(s => s.Id == arbol.IdNivel6).Menu1.Any(a => a.Id == n.Id))
                            {
                                Menu menuNivel1 = new Menu
                                {
                                    Descripcion = n.Descripcion,
                                    Id = n.Id,
                                    Url = url + arbol.Id
                                };
                                menu.Menu1.Single(s => s.Id == arbol.IdNivel1).Menu1.Single(s => s.Id == arbol.IdNivel2).Menu1.Single(s => s.Id == arbol.IdNivel3).Menu1.Single(s => s.Id == arbol.IdNivel4).Menu1.Single(s => s.Id == arbol.IdNivel5).Menu1.Single(s => s.Id == arbol.IdNivel6).Menu1.Add(menuNivel1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

    }
}
