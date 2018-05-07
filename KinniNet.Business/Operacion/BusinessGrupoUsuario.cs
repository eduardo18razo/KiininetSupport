using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessGrupoUsuario : IDisposable
    {
        private readonly bool _proxy;
        public void Dispose()
        {

        }

        public BusinessGrupoUsuario(bool proxy = false)
        {
            _proxy = proxy;
        }
        public List<GrupoUsuario> ObtenerGrupos(bool insertarSeleccion)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.GrupoUsuario.OrderBy(o => o.Descripcion).ToList();
                foreach (GrupoUsuario gpo in result)
                {
                    db.LoadProperty(gpo, "TipoUsuario");
                }
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new GrupoUsuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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
        public List<GrupoUsuario> ObtenerGruposAtencionByIdUsuario(int idUsuario, bool insertarSeleccion)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<GrupoUsuario> result = null;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                //       .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                //if (supervisor)
                //{
                result = db.GrupoUsuario.Join(db.UsuarioGrupo, gu => gu.Id, ug => ug.IdGrupoUsuario, (gu, ug) => new { gu, ug })
                    .Where(@t => @t.ug.IdUsuario == idUsuario && @t.gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente && @t.gu.Habilitado)
                    .Select(@t => @t.gu).Distinct().ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new GrupoUsuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
                        });
                //}
                //else
                //{

                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }
        public List<GrupoUsuario> ObtenerGruposByIdUsuario(int idUsuario, bool insertarSeleccion)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.UsuarioGrupo.Where(w => w.IdUsuario == idUsuario).Select(s => s.GrupoUsuario).Where(w => w.Habilitado).Distinct().OrderBy(o => o.Descripcion).ToList();
                foreach (GrupoUsuario gpo in result)
                {
                    db.LoadProperty(gpo, "TipoUsuario");
                    db.LoadProperty(gpo, "TipoGrupo");
                }
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new GrupoUsuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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
        public List<GrupoUsuario> ObtenerGruposUsuarioTipoUsuario(int idTipoGrupo, int idTipoUsuario, bool insertarSeleccion)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.GrupoUsuario.Where(w => w.IdTipoGrupo == idTipoGrupo && w.IdTipoUsuario == idTipoUsuario && w.Habilitado)
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new GrupoUsuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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
        public List<GrupoUsuario> ObtenerGruposUsuarioSistema(int idTipoUsuario)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.GrupoUsuario.Where(w => w.Habilitado && w.IdTipoUsuario == idTipoUsuario && w.Sistema && w.IdTipoGrupo != (int)BusinessVariables.EnumTiposGrupos.Administrador)
                        .OrderBy(o => o.Id)
                        .ToList();
                foreach (GrupoUsuario grupo in result)
                {
                    db.LoadProperty(grupo, "TipoGrupo");
                    db.LoadProperty(grupo.TipoGrupo, "RolTipoGrupo");
                    db.LoadProperty(grupo, "SubGrupoUsuario");
                    foreach (SubGrupoUsuario subGrupo in grupo.SubGrupoUsuario)
                    {
                        db.LoadProperty(subGrupo, "SubRol");
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
        public List<GrupoUsuario> ObtenerGruposUsuarioNivel(int idtipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = db.GrupoUsuarioInventarioArbol.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdTipoArbolAcceso == idtipoArbol && w.InventarioArbolAcceso.ArbolAcceso.IdNivel1 == nivel1);
                if (nivel2.HasValue)
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel2 == nivel2);
                else
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel2 == null);

                if (nivel3.HasValue)
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel3 == nivel3);
                else
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel3 == null);

                if (nivel4.HasValue)
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel4 == nivel4);
                else
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel4 == null);

                if (nivel5.HasValue)
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel5 == nivel5);
                else
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel5 == null);

                if (nivel6.HasValue)
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel6 == nivel6);
                else
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel6 == null);

                if (nivel7.HasValue)
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel7 == nivel7);
                else
                    qry = qry.Where(w => w.InventarioArbolAcceso.ArbolAcceso.IdNivel7 == null);

                result = qry.Select(s => s.GrupoUsuario).Where(w => w.Habilitado).Distinct().ToList();

                foreach (GrupoUsuario grupo in result)
                {
                    db.LoadProperty(grupo, "TipoGrupo");
                    db.LoadProperty(grupo.TipoGrupo, "RolTipoGrupo");
                    db.LoadProperty(grupo, "SubGrupoUsuario");
                    foreach (SubGrupoUsuario subGrupo in grupo.SubGrupoUsuario)
                    {
                        db.LoadProperty(subGrupo, "SubRol");
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
        public List<GrupoUsuario> ObtenerGruposUsuarioSistemaNivelArbol()
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //db.GrupoUsuarioInventarioArbol.Where(w => w.InventarioArbolAcceso.ArbolAcceso.Nivel1.Id == 1);
                result = db.GrupoUsuario.Where(w => w.Habilitado && w.Sistema && w.IdTipoGrupo != (int)BusinessVariables.EnumTiposGrupos.Administrador)
                        .OrderBy(o => o.Id)
                        .ToList();
                foreach (GrupoUsuario grupo in result)
                {
                    db.LoadProperty(grupo, "TipoGrupo");
                    db.LoadProperty(grupo.TipoGrupo, "RolTipoGrupo");
                    db.LoadProperty(grupo, "SubGrupoUsuario");
                    foreach (SubGrupoUsuario subGrupo in grupo.SubGrupoUsuario)
                    {
                        db.LoadProperty(subGrupo, "SubRol");
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
        public List<GrupoUsuario> ObtenerGruposUsuarioByIdTipoSubGrupo(int idTipoSubgrupo, bool insertarSeleccion)
        {
            List<GrupoUsuario> result = new List<GrupoUsuario>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //result = db.SubGrupoUsuario.Where(w => w.Habilitado && w.IdTipoSubGrupo == idTipoSubgrupo).Select(s => s.GrupoUsuario).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new GrupoUsuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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
        public List<GrupoUsuario> ObtenerGruposUsuarioByIdRolTipoUsuario(int idRol, int idTipoUsuario, bool insertarSeleccion)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                if (idRol == (int)BusinessVariables.EnumRoles.ConsultasEspeciales)
                    result = db.RolTipoGrupo.Where(w => w.Rol.Habilitado && w.IdRol == idRol).SelectMany(s => s.TipoGrupo.GrupoUsuario).Where(w => w.IdTipoUsuario == idTipoUsuario && w.Habilitado).ToList();
                else
                    result = db.RolTipoGrupo.Where(w => w.Rol.Habilitado && w.IdRol == idRol).SelectMany(s => s.TipoGrupo.GrupoUsuario).Where(w => w.IdTipoUsuario == idTipoUsuario && w.Habilitado).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new GrupoUsuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione,
                            Habilitado = true
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
        public List<GrupoUsuario> ObtenerGruposUsuarioByIdRol(int idRol, bool insertarSeleccion)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.RolTipoGrupo.Where(w => w.Rol.Habilitado && w.IdRol == idRol).SelectMany(s => s.TipoGrupo.GrupoUsuario).Where(w => w.Habilitado).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new GrupoUsuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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
        public GrupoUsuario ObtenerGrupoUsuarioById(int idGrupoUsuario)
        {
            GrupoUsuario result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.GrupoUsuario.SingleOrDefault(s => s.Id == idGrupoUsuario);
                if (result != null)
                {
                    db.LoadProperty(result, "TipoUsuario");
                    db.LoadProperty(result, "TipoGrupo");
                    db.LoadProperty(result, "SubGrupoUsuario");
                    foreach (SubGrupoUsuario sb in result.SubGrupoUsuario)
                    {
                        db.LoadProperty(sb, "HorarioSubGrupo");
                        db.LoadProperty(sb, "DiaFestivoSubGrupo");
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
        public GrupoUsuario ObtenerGrupoDefaultRol(int idRol, int idTipoUsuario)
        {
            GrupoUsuario result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                RolTipoGrupo rolTipoGrupo = db.RolTipoGrupo.SingleOrDefault(w => w.IdRol == idRol);
                if (rolTipoGrupo != null)
                {
                    if (db.GrupoUsuario.Any(f => f.IdTipoGrupo == rolTipoGrupo.IdTipoGrupo && f.IdTipoUsuario == idTipoUsuario && f.Sistema))
                    {
                        result = db.GrupoUsuario.First(f => f.IdTipoGrupo == rolTipoGrupo.IdTipoGrupo && f.IdTipoUsuario == idTipoUsuario && f.Sistema);
                        db.LoadProperty(result, "SubGrupoUsuario");
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
        public List<GrupoUsuario> ObtenerGrupoDefaultRolOpcion(int idTipoArbol, int idTipoGrupo, int idTipoUsuario)
        {
            List<GrupoUsuario> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.GrupoUsuarioDefaultOpcion.Where(f => f.IdTipoGrupo == idTipoGrupo && f.IdTipoUsuario == idTipoUsuario).Select(s=>s.GrupoUsuario).ToList();
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

        public List<UsuarioGrupo> ObtenerGruposDeUsuario(int idUsuario)
        {
            List<UsuarioGrupo> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = ((IQueryable<UsuarioGrupo>)from ug in db.UsuarioGrupo
                                                    join gu in db.GrupoUsuario.Where(w => w.Habilitado) on ug.IdGrupoUsuario equals gu.Id into joingroups
                                                    from sgu in db.SubGrupoUsuario.Where(w => w.Id == ug.IdSubGrupoUsuario).DefaultIfEmpty()
                                                    from sr in db.SubRol.Where(w => w.Id == sgu.IdSubRol).DefaultIfEmpty()
                                                    where ug.IdUsuario == idUsuario
                                                    select ug).ToList();

                //result = db.UsuarioGrupo.Where(w=>w.IdUsuario == idUsuario).Select(s=>s.GrupoUsuario).ToList();
                foreach (UsuarioGrupo grupo in result)
                {
                    db.LoadProperty(grupo, "Rol");
                    db.LoadProperty(grupo, "GrupoUsuario");
                    db.LoadProperty(grupo.GrupoUsuario, "TipoGrupo");
                    db.LoadProperty(grupo, "SubGrupoUsuario");
                    if (grupo.SubGrupoUsuario != null)
                        db.LoadProperty(grupo.SubGrupoUsuario, "SubRol");
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
        public List<HorarioSubGrupo> ObtenerHorariosByIdSubGrupo(int idSubGrupo)
        {
            List<HorarioSubGrupo> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.HorarioSubGrupo.Where(w => w.IdSubGrupoUsuario == idSubGrupo).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }
        public List<DiaFestivoSubGrupo> ObtenerDiasByIdSubGrupo(int idSubGrupo)
        {
            List<DiaFestivoSubGrupo> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.DiaFestivoSubGrupo.Where(w => w.IdSubGrupoUsuario == idSubGrupo).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }
        public List<GrupoUsuario> ObtenerGruposUsuarioAll(int? idTipoUsuario, int? idTipoGrupo)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<GrupoUsuario> qry = db.GrupoUsuario.Where(w => !w.Sistema);
                if (idTipoUsuario != null)
                    qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);

                if (idTipoGrupo != null)
                    qry = qry.Where(w => w.IdTipoGrupo == idTipoGrupo);

                result = qry.ToList();
                foreach (GrupoUsuario grupo in result)
                {
                    db.LoadProperty(grupo, "TipoUsuario");
                    db.LoadProperty(grupo, "TipoGrupo");
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
        public List<GrupoUsuario> ObtenerGruposUsuarioResponsablesByGruposTipoServicio(int idUsuario, List<int> grupos, List<int> tipoServicio)
        {
            List<GrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join gu in db.GrupoUsuario on tgu.IdGrupoUsuario equals gu.Id
                          join ug in db.UsuarioGrupo on gu.Id equals ug.IdGrupoUsuario
                          where gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || gu.IdTipoUsuario == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido
                          select new { t, gu, ug };

                if (!supervisor)
                    qry = from q in qry
                          where q.ug.IdUsuario == idUsuario
                          select q;

                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.gu.Id)
                          select q;
                if (tipoServicio.Any())
                    qry = from q in qry
                          where tipoServicio.Contains(q.t.IdTipoArbolAcceso)
                          select q;

                result = qry.Select(s => s.gu).Distinct().ToList();
                foreach (GrupoUsuario grupo in result)
                {
                    db.LoadProperty(grupo, "TipoUsuario");
                    db.LoadProperty(grupo, "TipoGrupo");
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
        public void GuardarGrupoUsuario(GrupoUsuario grupoUsuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                grupoUsuario.Descripcion = grupoUsuario.Descripcion.Trim();
                if (db.GrupoUsuario.Any(a => a.Descripcion == grupoUsuario.Descripcion && a.IdTipoGrupo == grupoUsuario.IdTipoGrupo && a.IdTipoUsuario == grupoUsuario.IdTipoUsuario))
                {
                    throw new Exception("Ya existe un Grupo con esta descripcion");
                }
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el que viene embebido
                grupoUsuario.Habilitado = true;
                grupoUsuario.TieneSupervisor = grupoUsuario.SubGrupoUsuario.Any(a => a.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor);
                if (grupoUsuario.Id == 0)
                {
                    grupoUsuario.EstatusTicketSubRolGeneral = GeneraEstatusGrupoDefault(grupoUsuario);
                    grupoUsuario.EstatusAsignacionSubRolGeneral = GeneraEstatusAsignacionGrupoDefault(grupoUsuario);
                    db.GrupoUsuario.AddObject(grupoUsuario);
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
        public void GuardarGrupoUsuario(GrupoUsuario grupoUsuario, Dictionary<int, int> horarios, Dictionary<int, List<DiaFestivoSubGrupo>> diasDescanso)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                grupoUsuario.Descripcion = grupoUsuario.Descripcion.Trim();
                if (db.GrupoUsuario.Any(a => a.Descripcion == grupoUsuario.Descripcion && a.IdTipoGrupo == grupoUsuario.IdTipoGrupo && a.IdTipoUsuario == grupoUsuario.IdTipoUsuario))
                {
                    throw new Exception("Ya existe un Grupo con esta descripcion");
                }
                grupoUsuario.SubGrupoUsuario = new List<SubGrupoUsuario>();
                foreach (KeyValuePair<int, int> horario in horarios)
                {

                    List<HorarioSubGrupo> lstHorarioGpo = new List<HorarioSubGrupo>();
                    List<HorarioDetalle> detalle = db.HorarioDetalle.Where(w => w.IdHorario == horario.Value).ToList();
                    foreach (HorarioDetalle horarioDetalle in detalle)
                    {
                        HorarioSubGrupo horarioGpo = new HorarioSubGrupo
                        {
                            IdHorario = horario.Value,
                            IdSubGrupoUsuario = horario.Key,
                            Dia = horarioDetalle.Dia,
                            HoraInicio = horarioDetalle.HoraInicio,
                            HoraFin = horarioDetalle.HoraFin
                        };
                        lstHorarioGpo.Add(horarioGpo);
                    }

                    SubGrupoUsuario subGrupo = new SubGrupoUsuario();
                    subGrupo.IdSubRol = horario.Key;
                    subGrupo.Habilitado = true;
                    subGrupo.HorarioSubGrupo = subGrupo.HorarioSubGrupo ?? new List<HorarioSubGrupo>();
                    subGrupo.DiaFestivoSubGrupo = subGrupo.DiaFestivoSubGrupo ?? new List<DiaFestivoSubGrupo>();
                    subGrupo.HorarioSubGrupo.AddRange(lstHorarioGpo);
                    List<DiaFestivoSubGrupo> lstDiasDescanso = diasDescanso.SingleOrDefault(w => w.Key == horario.Key).Value;
                    if (lstDiasDescanso != null)
                    {
                        foreach (DiaFestivoSubGrupo dia in lstDiasDescanso)
                        {
                            dia.IdSubGrupoUsuario = horario.Key;
                        }
                        subGrupo.DiaFestivoSubGrupo.AddRange(lstDiasDescanso);
                    }

                    grupoUsuario.SubGrupoUsuario.Add(subGrupo);
                }
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el que viene embebido
                grupoUsuario.Habilitado = true;
                grupoUsuario.TieneSupervisor = grupoUsuario.SubGrupoUsuario.Any(a => a.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor);
                if (grupoUsuario.Id == 0)
                {
                    grupoUsuario.EstatusTicketSubRolGeneral = GeneraEstatusGrupoDefault(grupoUsuario);
                    grupoUsuario.EstatusAsignacionSubRolGeneral = GeneraEstatusAsignacionGrupoDefault(grupoUsuario);
                    db.GrupoUsuario.AddObject(grupoUsuario);
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
        public void ActualizarGrupo(GrupoUsuario gpo, Dictionary<int, int> horarios, Dictionary<int, List<DiaFestivoSubGrupo>> diasDescanso)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                gpo.Descripcion = gpo.Descripcion.Trim();
                if (db.GrupoUsuario.Any(a => a.Descripcion == gpo.Descripcion && a.IdTipoGrupo == gpo.IdTipoGrupo && a.Id != gpo.Id && a.IdTipoUsuario == gpo.IdTipoUsuario))
                {
                    throw new Exception("Ya existe un Grupo con esta descripción");
                }
                GrupoUsuario grupo = db.GrupoUsuario.SingleOrDefault(w => w.Id == gpo.Id);
                List<SubGrupoUsuario> sb = new List<SubGrupoUsuario>();
                if (grupo != null)
                {
                    grupo.Descripcion = gpo.Descripcion.Trim();

                    #region Manejo de Horarios
                    if (horarios != null && horarios.Count > 0)
                    {
                        foreach (KeyValuePair<int, int> horario in horarios)
                        {
                            var horariosGrupo = from hsg in db.HorarioSubGrupo
                                                join sbgu in db.SubGrupoUsuario on hsg.IdSubGrupoUsuario equals sbgu.Id
                                                where sbgu.IdGrupoUsuario == grupo.Id
                                                select hsg;


                            List<HorarioSubGrupo> lstHorarioSgpoEliminar = horariosGrupo.ToList();
                            foreach (HorarioSubGrupo horarioDbDelete in lstHorarioSgpoEliminar)
                            {
                                db.HorarioSubGrupo.DeleteObject(horarioDbDelete);
                            }



                            List<HorarioSubGrupo> lstHorarioGpo = new List<HorarioSubGrupo>();
                            List<HorarioDetalle> detalle = db.HorarioDetalle.Where(w => w.IdHorario == horario.Value).ToList();
                            foreach (HorarioDetalle horarioDetalle in detalle)
                            {
                                HorarioSubGrupo horarioGpo = new HorarioSubGrupo
                                {
                                    IdHorario = horario.Value,
                                    IdSubGrupoUsuario = horario.Key,
                                    Dia = horarioDetalle.Dia,
                                    HoraInicio = horarioDetalle.HoraInicio,
                                    HoraFin = horarioDetalle.HoraFin
                                };
                                lstHorarioGpo.Add(horarioGpo);
                            }

                            SubGrupoUsuario subGrupo = new SubGrupoUsuario
                            {
                                Id = grupo.SubGrupoUsuario.FirstOrDefault(f => f.IdSubRol == horario.Key) != null ? grupo.SubGrupoUsuario.First(f => f.IdSubRol == horario.Key).Id : 0,
                                IdGrupoUsuario = grupo.Id,
                                IdSubRol = horario.Key,
                                Habilitado = true
                            };
                            subGrupo.HorarioSubGrupo = subGrupo.HorarioSubGrupo ?? new List<HorarioSubGrupo>();
                            subGrupo.DiaFestivoSubGrupo = subGrupo.DiaFestivoSubGrupo ?? new List<DiaFestivoSubGrupo>();
                            subGrupo.HorarioSubGrupo.AddRange(lstHorarioGpo);

                            List<DiaFestivoSubGrupo> diasDb = db.DiaFestivoSubGrupo.Where(w => w.IdSubGrupoUsuario == subGrupo.Id).ToList();
                            foreach (DiaFestivoSubGrupo festivoSubGrupo in diasDb)
                            {
                                db.DiaFestivoSubGrupo.DeleteObject(festivoSubGrupo);
                            }

                            List<DiaFestivoSubGrupo> lstDiasDescanso = diasDescanso.SingleOrDefault(w => w.Key == horario.Key).Value;
                            if (lstDiasDescanso != null)
                            {
                                foreach (DiaFestivoSubGrupo dia in lstDiasDescanso)
                                {
                                    dia.IdSubGrupoUsuario = horario.Key;
                                }
                                subGrupo.DiaFestivoSubGrupo.AddRange(lstDiasDescanso);
                            }
                            if (grupo.SubGrupoUsuario == null)
                                grupo.SubGrupoUsuario = new List<SubGrupoUsuario>();

                            if (grupo.SubGrupoUsuario.SingleOrDefault(s => s.IdGrupoUsuario == subGrupo.IdGrupoUsuario && s.IdSubRol == subGrupo.IdSubRol) == null)
                                grupo.SubGrupoUsuario.Add(subGrupo);
                            else
                            {
                                grupo.SubGrupoUsuario.SingleOrDefault(s => s.IdGrupoUsuario == subGrupo.IdGrupoUsuario && s.IdSubRol == subGrupo.IdSubRol).HorarioSubGrupo = subGrupo.HorarioSubGrupo;

                            }

                            if (grupo.SubGrupoUsuario.SingleOrDefault(s => s.IdGrupoUsuario == subGrupo.IdGrupoUsuario && s.IdSubRol == subGrupo.IdSubRol) == null)
                                grupo.SubGrupoUsuario.Add(subGrupo);
                            else
                            {
                                grupo.SubGrupoUsuario.SingleOrDefault(s => s.IdGrupoUsuario == subGrupo.IdGrupoUsuario && s.IdSubRol == subGrupo.IdSubRol).DiaFestivoSubGrupo = subGrupo.DiaFestivoSubGrupo;

                            }
                            //sb.Add(subGrupo);
                        }
                    }

                    #endregion Manejo de Horarios
                    grupo.TieneSupervisor = grupo.SubGrupoUsuario.Any(a => a.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor);
                    #region Manejo de politicas
                    List<EstatusTicketSubRolGeneral> lstEliminarPoliticaEstatus = db.EstatusTicketSubRolGeneral.Where(w => w.IdGrupoUsuario == grupo.Id).ToList();
                    List<EstatusAsignacionSubRolGeneral> lstEliminarPoliticaEstatusAsignacion = db.EstatusAsignacionSubRolGeneral.Where(w => w.IdGrupoUsuario == grupo.Id).ToList();
                    foreach (EstatusTicketSubRolGeneral politicaEstatus in lstEliminarPoliticaEstatus)
                    {
                        db.EstatusTicketSubRolGeneral.DeleteObject(politicaEstatus);
                    }
                    foreach (EstatusAsignacionSubRolGeneral politicaAsignacion in lstEliminarPoliticaEstatusAsignacion)
                    {
                        db.EstatusAsignacionSubRolGeneral.DeleteObject(politicaAsignacion);
                    }
                    grupo.EstatusTicketSubRolGeneral = GeneraEstatusGrupoDefault(grupo);
                    grupo.EstatusAsignacionSubRolGeneral = GeneraEstatusAsignacionGrupoDefault(grupo);
                    #endregion Manejo de politicas

                    if (grupo.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal)
                    {
                        grupo.LevantaTicket = gpo.LevantaTicket;
                        grupo.RecadoTicket = gpo.RecadoTicket;
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
        public void HabilitarGrupo(int idGrupo, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                if (db.GrupoUsuario.Single(s => s.Id == idGrupo).IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Administrador)
                    if (db.GrupoUsuario.Count(w => w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Administrador && w.Habilitado && w.Id != idGrupo && w.UsuarioGrupo.Count(ug => ug.IdGrupoUsuario == w.Id) > 0) <= 0)
                    {
                        throw new Exception("Debe tener otro usuario activo para este tipo de grupo.");
                    }
                GrupoUsuario grpo = db.GrupoUsuario.SingleOrDefault(w => w.Id == idGrupo);
                if (grpo != null) grpo.Habilitado = habilitado;
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
        private List<EstatusTicketSubRolGeneral> GeneraEstatusGrupoDefault(GrupoUsuario grupo)
        {

            List<EstatusTicketSubRolGeneral> result = new List<EstatusTicketSubRolGeneral>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                if (grupo.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente)
                    result.AddRange(from subgpo in grupo.SubGrupoUsuario
                                    where subgpo != null
                                    from statusDefault in db.EstatusTicketSubRolGeneralDefault.Where(w => w.IdSubRolSolicita == subgpo.IdSubRol && w.TieneSupervisor == grupo.TieneSupervisor)
                                    select new EstatusTicketSubRolGeneral
                                    {
                                        IdRolSolicita = statusDefault.IdRolSolicita,
                                        IdSubRolSolicita = statusDefault.IdSubRolSolicita,
                                        IdRolPertenece = statusDefault.IdRolPertenece,
                                        IdSubRolPertenece = statusDefault.IdSubRolPertenece,
                                        IdEstatusTicketActual = statusDefault.IdEstatusTicketActual,
                                        IdEstatusTicketAccion = statusDefault.IdEstatusTicketAccion,
                                        ComentarioObligado = statusDefault.ComentarioObligado,
                                        TieneSupervisor = statusDefault.TieneSupervisor,
                                        Propietario = statusDefault.Propietario,
                                        LevantaTicket = statusDefault.LevantaTicket,
                                        Habilitado = statusDefault.Habilitado
                                    });
                else if (grupo.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario)
                {
                    int idRol = (int)BusinessVariables.EnumRoles.Usuario;
                    var qry = from statusDefault in db.EstatusTicketSubRolGeneralDefault.Where(
                                w => w.IdRolSolicita == idRol)
                              select statusDefault;

                    foreach (EstatusTicketSubRolGeneralDefault generalDefault in qry.ToList())
                    {
                        result.Add(new EstatusTicketSubRolGeneral
                              {
                                  IdRolSolicita = generalDefault.IdRolSolicita,
                                  IdSubRolSolicita = generalDefault.IdSubRolSolicita,
                                  IdRolPertenece = generalDefault.IdRolPertenece,
                                  IdSubRolPertenece = generalDefault.IdSubRolPertenece,
                                  IdEstatusTicketActual = generalDefault.IdEstatusTicketActual,
                                  IdEstatusTicketAccion = generalDefault.IdEstatusTicketAccion,
                                  TieneSupervisor = generalDefault.TieneSupervisor,
                                  Propietario = generalDefault.Propietario,
                                  LevantaTicket = generalDefault.LevantaTicket,
                                  Habilitado = generalDefault.Habilitado
                              });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { db.Dispose(); }
            return result;
        }
        public List<EstatusAsignacionSubRolGeneral> GeneraEstatusAsignacionGrupoDefault(GrupoUsuario grupo)
        {
            List<EstatusAsignacionSubRolGeneral> result = new List<EstatusAsignacionSubRolGeneral>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                result.AddRange(from subgpo in grupo.SubGrupoUsuario.OrderBy(o => o.IdSubRol)
                                where subgpo != null
                                from statusDefault in db.EstatusAsignacionSubRolGeneralDefault.Where(w => w.IdSubRol == subgpo.IdSubRol && w.TieneSupervisor == grupo.TieneSupervisor)
                                select new EstatusAsignacionSubRolGeneral
                                {
                                    IdRol = statusDefault.IdRol,
                                    IdSubRol = statusDefault.IdSubRol,
                                    IdEstatusAsignacionActual = statusDefault.IdEstatusAsignacionActual,
                                    IdEstatusAsignacionAccion = statusDefault.IdEstatusAsignacionAccion,
                                    ComentarioObligado = statusDefault.ComentarioObligado,
                                    TieneSupervisor = statusDefault.TieneSupervisor,
                                    Propietario = statusDefault.Propietario,
                                    Habilitado = statusDefault.Habilitado
                                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { db.Dispose(); }
            return result;
        }
        public List<EstatusAsignacionSubRolGeneralDefault> GeneraEstatusAsignacionGrupoDefault()
        {
            List<EstatusAsignacionSubRolGeneralDefault> result = new List<EstatusAsignacionSubRolGeneralDefault>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.EstatusAsignacionSubRolGeneralDefault.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { db.Dispose(); }
            return result;
        }
    }
}
