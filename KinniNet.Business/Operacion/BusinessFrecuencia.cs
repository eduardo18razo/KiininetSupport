using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessFrecuencia : IDisposable
    {
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessFrecuencia(bool proxy = false)
        {
            _proxy = proxy;
        }

        #region Top General
        private List<HelperFrecuencia> GeneraTopGeneralPublico(int idTipoUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                var frecuencias = (from f in db.Frecuencia
                                   where f.IdTipoUsuario == idTipoUsuario && !f.ArbolAcceso.Sistema && f.ArbolAcceso.Habilitado
                                   group f by f.IdArbolAcceso
                                       into frec
                                       orderby frec.Key
                                       select new
                                       {
                                           IdArbolAcceso = frec.Key,
                                           NumeroVisitas = frec.Sum(s => s.NumeroVisitas)
                                       }).Take(10);

                result = new List<HelperFrecuencia>();
                foreach (var type in frecuencias.Distinct())
                {
                    ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso);
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        IdArea = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso).IdArea,
                        IdTipoArbol = arbol.IdTipoArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso),
                        DescripcionOpcionLarga = arbol.Descripcion
                    });
                }

                List<int> arbolesAgregados = new List<int>();
                arbolesAgregados.AddRange(result.Select(s => s.IdArbol));
                int take = 10 - result.Count;
                if (result.Count < 10)
                {
                    List<ArbolAcceso> opciones = db.ArbolAcceso.Where(w => w.EsTerminal && !w.Sistema && w.Habilitado && w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id)).OrderByDescending(d => d.FechaAlta).Take(take).ToList();
                    foreach (ArbolAcceso opcion in opciones)
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
                            IdArea = arbol.IdArea,
                            IdTipoArbol = arbol.IdTipoArbolAcceso,
                            DescripcionOpcion = bArbol.ObtenerTipificacion(arbol.Id),
                            DescripcionOpcionLarga = arbol.Descripcion
                        });
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
        private List<HelperFrecuencia> GeneraTopGeneralPrivado(int idTipoUsuario, int idUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();

                List<int> gpos = db.UsuarioGrupo.Where(w => w.IdUsuario == idUsuario && w.IdRol == (int)BusinessVariables.EnumRoles.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                var frecuencias = (
                        from f in db.Frecuencia
                        join aa in db.ArbolAcceso on new { idarbol = f.IdArbolAcceso, idtu = f.IdTipoUsuario } equals new { idarbol = aa.Id, idtu = aa.IdTipoUsuario }
                        join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                        join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                        join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario, idsbgpo = guia.IdSubGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario, idsbgpo = ug.IdSubGrupoUsuario }
                        //join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario, idsbgpo = guia.IdSubGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario, idsbgpo = ug.IdSubGrupoUsuario }
                        where gpos.Contains(guia.IdGrupoUsuario) //ug.IdUsuario == idUsuario 
                            && f.IdTipoUsuario == idTipoUsuario && !aa.Sistema && aa.Habilitado && aa.EsTerminal
                        select f).Distinct().GroupBy(g => new { g.IdArbolAcceso, g.NumeroVisitas })
                            .Select(s => new { s.Key.IdArbolAcceso, NumeroVisitas = s.Sum(sa => sa.NumeroVisitas) })
                            .OrderByDescending(o => o.NumeroVisitas).Take(10).ToList();
                result = new List<HelperFrecuencia>();
                foreach (var type in frecuencias.Distinct())
                {
                    ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso);
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        IdArea = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso).IdArea,
                        IdTipoArbol = arbol.IdTipoArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso),
                        DescripcionOpcionLarga = arbol.Descripcion
                    });
                }

                List<int> arbolesAgregados = new List<int>();
                arbolesAgregados.AddRange(result.Select(s => s.IdArbol));
                int take = 10 - result.Count;
                if (result.Count < 10)
                {
                    List<ArbolAcceso> opciones = (from aa in db.ArbolAcceso
                                                  join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                                  join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                                  join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario }
                                                  where aa.EsTerminal
                                                        && !aa.Sistema
                                                        && aa.Habilitado
                                                        && ug.IdUsuario == idUsuario
                                                        && aa.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(aa.Id)
                                                        && ug.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario
                                                  select aa).Distinct().ToList();
                    opciones.AddRange(db.ArbolAcceso.Where(
                            w => w.EsTerminal && !w.Sistema && w.Habilitado && w.Publico &&
                                w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id))
                            .OrderByDescending(d => d.FechaAlta)
                            .Take(take)
                            .ToList());
                    foreach (ArbolAcceso opcion in opciones)
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
                            IdArea = arbol.IdArea,
                            IdTipoArbol = arbol.IdTipoArbolAcceso,
                            DescripcionOpcion = bArbol.ObtenerTipificacion(arbol.Id),
                            DescripcionOpcionLarga = arbol.Descripcion
                        });
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
        public List<HelperFrecuencia> ObtenerTopTenGeneral(int idTipoUsuario, int? idUsuario)
        {
            List<HelperFrecuencia> result;
            try
            {
                result = idUsuario == null ? GeneraTopGeneralPublico(idTipoUsuario) : GeneraTopGeneralPrivado(idTipoUsuario, (int)idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        #endregion Top General

        #region Top Consulta
        public List<HelperFrecuencia> GeneraTopConsultaPublico(int idTipoUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                var frecuencias = (from f in db.Frecuencia
                                   where f.IdTipoUsuario == idTipoUsuario && !f.ArbolAcceso.Sistema && f.ArbolAcceso.Habilitado && f.ArbolAcceso.Publico
                                   && f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion
                                   group f by f.IdArbolAcceso
                                       into frec
                                       orderby frec.Key
                                       select new
                                       {
                                           IdArbolAcceso = frec.Key,
                                           NumeroVisitas = frec.Sum(s => s.NumeroVisitas)
                                       }).Take(10);

                result = new List<HelperFrecuencia>();
                foreach (var type in frecuencias.Distinct())
                {
                    ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso);
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        IdArea = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso).IdArea,
                        IdTipoArbol = arbol.IdTipoArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso),
                        DescripcionOpcionLarga = arbol.Descripcion
                    });
                }

                List<int> arbolesAgregados = new List<int>();
                arbolesAgregados.AddRange(result.Select(s => s.IdArbol));
                int take = 10 - result.Count;
                if (result.Count < 10)
                {
                    List<ArbolAcceso> opciones = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion && w.EsTerminal && !w.Sistema && w.Habilitado && w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id)).OrderByDescending(d => d.FechaAlta).Take(take).ToList();
                    foreach (ArbolAcceso opcion in opciones)
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
                            IdArea = arbol.IdArea,
                            IdTipoArbol = arbol.IdTipoArbolAcceso,
                            DescripcionOpcion = bArbol.ObtenerTipificacion(arbol.Id),
                            DescripcionOpcionLarga = arbol.Descripcion
                        });
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

        public List<HelperFrecuencia> GeneraTopConsultaPrivado(int idTipoUsuario, int idUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                List<int> gpos = db.UsuarioGrupo.Where(w => w.IdUsuario == idUsuario && w.IdRol == (int)BusinessVariables.EnumRoles.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                var frecuencias = (
                        from f in db.Frecuencia
                        join aa in db.ArbolAcceso on new { idarbol = f.IdArbolAcceso, idtu = f.IdTipoUsuario } equals new { idarbol = aa.Id, idtu = aa.IdTipoUsuario }
                        join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                        join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                        //join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario, idsbgpo = guia.IdSubGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario, idsbgpo = ug.IdSubGrupoUsuario }
                        where gpos.Contains(guia.IdGrupoUsuario) //ug.IdUsuario == idUsuario 
                        && aa.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion
                            && f.IdTipoUsuario == idTipoUsuario && !aa.Sistema && aa.Habilitado && aa.EsTerminal
                        select f).Distinct().GroupBy(g => new { g.IdArbolAcceso, g.NumeroVisitas })
                            .Select(s => new { s.Key.IdArbolAcceso, NumeroVisitas = s.Sum(sa => sa.NumeroVisitas) })
                            .OrderByDescending(o => o.NumeroVisitas).Take(10).ToList();
                result = new List<HelperFrecuencia>();
                foreach (var type in frecuencias.Distinct())
                {
                    ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso);
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        IdArea = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso).IdArea,
                        IdTipoArbol = arbol.IdTipoArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso),
                        DescripcionOpcionLarga = arbol.Descripcion
                    });
                }

                List<int> arbolesAgregados = new List<int>();
                arbolesAgregados.AddRange(result.Select(s => s.IdArbol));
                int take = 10 - result.Count;
                if (result.Count < 10)
                {
                    List<ArbolAcceso> opciones = (from aa in db.ArbolAcceso
                                                  join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                                  join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                                  join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario }
                                                  where aa.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion
                                                        && aa.EsTerminal
                                                        && !aa.Sistema
                                                        && aa.Habilitado
                                                        && ug.IdUsuario == idUsuario
                                                        && aa.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(aa.Id)
                                                        && ug.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario
                                                  select aa).Distinct().ToList();
                    opciones.AddRange(db.ArbolAcceso.Where(
                            w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion &&
                                w.EsTerminal && !w.Sistema && w.Habilitado && w.Publico &&
                                w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id))
                            .OrderByDescending(d => d.FechaAlta)
                            .Take(take)
                            .ToList());
                    foreach (ArbolAcceso opcion in opciones.Distinct())
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
                            IdArea = arbol.IdArea,
                            IdTipoArbol = arbol.IdTipoArbolAcceso,
                            DescripcionOpcion = bArbol.ObtenerTipificacion(arbol.Id),
                            DescripcionOpcionLarga = arbol.Descripcion
                        });
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

        public List<HelperFrecuencia> ObtenerTopTenConsulta(int idTipoUsuario, int? idUsuario)
        {
            List<HelperFrecuencia> result;
            try
            {
                result = idUsuario == null ? GeneraTopConsultaPublico(idTipoUsuario) : GeneraTopConsultaPrivado(idTipoUsuario, (int)idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        #endregion Top Consulta

        #region Top Servicio
        public List<HelperFrecuencia> GeneraTopServicioPublico(int idTipoUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();

                var frecuencias = (from f in db.Frecuencia
                                   where f.IdTipoUsuario == idTipoUsuario && !f.ArbolAcceso.Sistema && f.ArbolAcceso.Habilitado && f.ArbolAcceso.Publico
                                   && f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio
                                   group f by f.IdArbolAcceso
                                       into frec
                                       orderby frec.Key
                                       select new
                                       {
                                           IdArbolAcceso = frec.Key,
                                           NumeroVisitas = frec.Sum(s => s.NumeroVisitas)
                                       }).Take(10);

                result = new List<HelperFrecuencia>();
                foreach (var type in frecuencias.Distinct())
                {
                    ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso);
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        IdArea = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso).IdArea,
                        IdTipoArbol = arbol.IdTipoArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso),
                        DescripcionOpcionLarga = arbol.Descripcion
                    });
                }

                List<int> arbolesAgregados = new List<int>();
                arbolesAgregados.AddRange(result.Select(s => s.IdArbol));
                int take = 10 - result.Count;
                if (result.Count < 10)
                {
                    List<ArbolAcceso> opciones = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio && w.EsTerminal && !w.Sistema && w.Habilitado && w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id)).OrderByDescending(d => d.FechaAlta).Take(take).ToList();
                    foreach (ArbolAcceso opcion in opciones)
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
                            IdArea = arbol.IdArea,
                            IdTipoArbol = arbol.IdTipoArbolAcceso,
                            DescripcionOpcion = bArbol.ObtenerTipificacion(arbol.Id),
                            DescripcionOpcionLarga = arbol.Descripcion
                        });
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

        public List<HelperFrecuencia> GeneraTopServicioPrivado(int idTipoUsuario, int idUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                List<int> gpos = db.UsuarioGrupo.Where(w => w.IdUsuario == idUsuario && w.IdRol == (int)BusinessVariables.EnumRoles.Usuario).Select(s => s.IdGrupoUsuario).ToList();

                var frecuencias = (
                        from f in db.Frecuencia
                        join aa in db.ArbolAcceso on new { idarbol = f.IdArbolAcceso, idtu = f.IdTipoUsuario } equals new { idarbol = aa.Id, idtu = aa.IdTipoUsuario }
                        join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                        join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                        //join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario, idsbgpo = guia.IdSubGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario, idsbgpo = ug.IdSubGrupoUsuario }
                        where gpos.Contains(guia.IdGrupoUsuario) //ug.IdUsuario == idUsuario 
                            && aa.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio &&
                        f.IdTipoUsuario == idTipoUsuario && !aa.Sistema && aa.Habilitado && aa.EsTerminal
                        select f).Distinct().GroupBy(g => new { g.IdArbolAcceso, g.NumeroVisitas })
                            .Select(s => new { s.Key.IdArbolAcceso, NumeroVisitas = s.Sum(sa => sa.NumeroVisitas) })
                            .OrderByDescending(o => o.NumeroVisitas).Take(10).ToList();
                result = new List<HelperFrecuencia>();
                foreach (var type in frecuencias.Distinct())
                {
                    ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso);
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        IdArea = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso).IdArea,
                        IdTipoArbol = arbol.IdTipoArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso),
                        DescripcionOpcionLarga = arbol.Descripcion
                    });
                }

                List<int> arbolesAgregados = new List<int>();
                arbolesAgregados.AddRange(result.Select(s => s.IdArbol));
                int take = 10 - result.Count;
                if (result.Count < 10)
                {
                    List<ArbolAcceso> opciones = (from aa in db.ArbolAcceso
                                                  join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                                  join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                                  join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario }
                                                  where aa.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio
                                                        && aa.EsTerminal
                                                        && !aa.Sistema
                                                        && aa.Habilitado
                                                        && ug.IdUsuario == idUsuario
                                                        && aa.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(aa.Id)
                                                        && ug.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario
                                                  select aa).Distinct().ToList();
                    opciones.AddRange(db.ArbolAcceso.Where(
                            w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio &&
                                w.EsTerminal && !w.Sistema && w.Habilitado && w.Publico &&
                                w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id))
                            .OrderByDescending(d => d.FechaAlta)
                            .Take(take)
                            .ToList());
                    foreach (ArbolAcceso opcion in opciones)
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
                            IdArea = arbol.IdArea,
                            IdTipoArbol = arbol.IdTipoArbolAcceso,
                            DescripcionOpcion = bArbol.ObtenerTipificacion(arbol.Id),
                            DescripcionOpcionLarga = arbol.Descripcion
                        });
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

        public List<HelperFrecuencia> ObtenerTopTenServicio(int idTipoUsuario, int? idUsuario)
        {
            List<HelperFrecuencia> result;
            try
            {
                result = idUsuario == null ? GeneraTopServicioPublico(idTipoUsuario) : GeneraTopServicioPrivado(idTipoUsuario, (int)idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        #endregion Top Servicio

        #region Top Incidente
        public List<HelperFrecuencia> GeneraTopIncidentePublico(int idTipoUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                var frecuencias = (from f in db.Frecuencia
                                   where f.IdTipoUsuario == idTipoUsuario && !f.ArbolAcceso.Sistema && f.ArbolAcceso.Habilitado && f.ArbolAcceso.Publico
                                   && f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas
                                   group f by f.IdArbolAcceso
                                       into frec
                                       orderby frec.Key
                                       select new
                                       {
                                           IdArbolAcceso = frec.Key,
                                           NumeroVisitas = frec.Sum(s => s.NumeroVisitas)
                                       }).Take(10);

                result = new List<HelperFrecuencia>();
                foreach (var type in frecuencias.Distinct())
                {
                    ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso);
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        IdArea = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso).IdArea,
                        IdTipoArbol = arbol.IdTipoArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso),
                        DescripcionOpcionLarga = arbol.Descripcion
                    });
                }

                List<int> arbolesAgregados = new List<int>();
                arbolesAgregados.AddRange(result.Select(s => s.IdArbol));
                int take = 10 - result.Count;
                if (result.Count < 10)
                {
                    List<ArbolAcceso> opciones = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas && w.EsTerminal && !w.Sistema && w.Habilitado && w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id)).OrderByDescending(d => d.FechaAlta).Take(take).ToList();
                    foreach (ArbolAcceso opcion in opciones)
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
                            IdArea = arbol.IdArea,
                            IdTipoArbol = arbol.IdTipoArbolAcceso,
                            DescripcionOpcion = bArbol.ObtenerTipificacion(arbol.Id),
                            DescripcionOpcionLarga = arbol.Descripcion
                        });
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

        public List<HelperFrecuencia> GeneraTopIncidentePrivado(int idTipoUsuario, int idUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                List<int> gpos = db.UsuarioGrupo.Where(w => w.IdUsuario == idUsuario && w.IdRol == (int)BusinessVariables.EnumRoles.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                var frecuencias = (
                        from f in db.Frecuencia
                        join aa in db.ArbolAcceso on new { idarbol = f.IdArbolAcceso, idtu = f.IdTipoUsuario } equals new { idarbol = aa.Id, idtu = aa.IdTipoUsuario }
                        join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                        join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                        //join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario, idsbgpo = guia.IdSubGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario, idsbgpo = ug.IdSubGrupoUsuario }
                        where gpos.Contains(guia.IdGrupoUsuario) //ug.IdUsuario == idUsuario 
                        && aa.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas
                        && f.IdTipoUsuario == idTipoUsuario && !aa.Sistema && aa.Habilitado && aa.EsTerminal
                        select f).Distinct().GroupBy(g => new { g.IdArbolAcceso, g.NumeroVisitas })
                            .Select(s => new { s.Key.IdArbolAcceso, NumeroVisitas = s.Sum(sa => sa.NumeroVisitas) })
                            .OrderByDescending(o => o.NumeroVisitas).Take(10).ToList();
                result = new List<HelperFrecuencia>();
                foreach (var type in frecuencias.Distinct())
                {
                    ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso);
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        IdArea = bArbol.ObtenerArbolAcceso(type.IdArbolAcceso).IdArea,
                        IdTipoArbol = arbol.IdTipoArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso),
                        DescripcionOpcionLarga = arbol.Descripcion
                    });
                }

                List<int> arbolesAgregados = new List<int>();
                arbolesAgregados.AddRange(result.Select(s => s.IdArbol));
                int take = 10 - result.Count;
                if (result.Count < 10)
                {
                    List<ArbolAcceso> opciones = (from aa in db.ArbolAcceso
                                                  join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                                  join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                                  join ug in db.UsuarioGrupo on new { idgpo = guia.IdGrupoUsuario } equals new { idgpo = ug.IdGrupoUsuario }
                                                  where aa.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas
                                                        && aa.EsTerminal
                                                        && !aa.Sistema
                                                        && aa.Habilitado
                                                        && ug.IdUsuario == idUsuario
                                                        && aa.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(aa.Id)
                                                        && ug.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario
                                                  select aa).Distinct().ToList();
                    opciones.AddRange(db.ArbolAcceso.Where(
                            w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas &&
                                w.EsTerminal && !w.Sistema && w.Habilitado && w.Publico &&
                                w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id))
                            .OrderByDescending(d => d.FechaAlta)
                            .Take(take)
                            .ToList());
                    foreach (ArbolAcceso opcion in opciones)
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
                            IdArea = arbol.IdArea,
                            IdTipoArbol = arbol.IdTipoArbolAcceso,
                            DescripcionOpcion = bArbol.ObtenerTipificacion(arbol.Id),
                            DescripcionOpcionLarga = arbol.Descripcion
                        });
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

        public List<HelperFrecuencia> ObtenerTopTenIncidente(int idTipoUsuario, int? idUsuario)
        {
            List<HelperFrecuencia> result;
            try
            {
                result = idUsuario == null ? GeneraTopIncidentePublico(idTipoUsuario) : GeneraTopIncidentePrivado(idTipoUsuario, (int)idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        #endregion Top Incidente


    }
}
