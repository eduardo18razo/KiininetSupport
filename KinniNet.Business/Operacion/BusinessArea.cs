﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Operacion;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessArea : IDisposable
    {
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessArea(bool proxy = false)
        {
            _proxy = proxy;
        }
        public List<Area> ObtenerAreasUsuario(int idUsuario, bool insertarSeleccion)
        {
            List<Area> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> lstAreas = (from a in db.Area
                                      join aa in db.ArbolAcceso on a.Id equals aa.IdArea
                                      join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                      join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                      join gu in db.GrupoUsuario on guia.IdGrupoUsuario equals gu.Id
                                      join ug in db.UsuarioGrupo on gu.Id equals ug.IdGrupoUsuario
                                      where ug.IdUsuario == idUsuario && guia.IdRol == (int)BusinessVariables.EnumRoles.AccesoCentroSoporte
                                      select a.Id).Distinct().ToList();
                result = db.Area.Where(w => lstAreas.Contains(w.Id)).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Area
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

        public List<Area> ObtenerAreasUsuarioPublicoByIdRol(int idRol, bool insertarSeleccion)
        {
            List<Area> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> lstAreas = (from a in db.Area
                                      join aa in db.ArbolAcceso on a.Id equals aa.IdArea
                                      join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                      join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                      where guia.IdRol == idRol && !aa.Sistema && aa.Habilitado && aa.Publico
                                      select a.Id).Distinct().ToList();
                result = db.Area.Where(w => lstAreas.Contains(w.Id)).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Area
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
        public List<Area> ObtenerAreasUsuarioByIdRol(int idUsuario, int idRol, bool insertarSeleccion)
        {
            List<Area> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> lstgpos = db.UsuarioGrupo.Where(w => w.IdUsuario == idUsuario && w.GrupoUsuario.IdTipoGrupo == idRol).Select(s => s.IdGrupoUsuario).Distinct().ToList();
                List<int> lstAreas = (from a in db.Area
                                      join aa in db.ArbolAcceso on a.Id equals aa.IdArea
                                      join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                      join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                      join gu in db.GrupoUsuario on new { gpo = guia.IdGrupoUsuario, tu = aa.IdTipoUsuario } equals new { gpo = gu.Id, tu = gu.IdTipoUsuario }
                                      where lstgpos.Contains(guia.IdGrupoUsuario) && guia.IdRol == idRol && !aa.Sistema && aa.Habilitado
                                      select a.Id).Distinct().ToList();
                result = db.Area.Where(w => lstAreas.Contains(w.Id)).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Area
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
        public List<Area> ObtenerAreasUsuarioByRol(int idUsuario, bool insertarSeleccion)
        {
            List<Area> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> lstAreas = (from a in db.Area
                                      join aa in db.ArbolAcceso on a.Id equals aa.IdArea
                                      join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                      join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                      join gu in db.GrupoUsuario on guia.IdGrupoUsuario equals gu.Id
                                      join ug in db.UsuarioGrupo on gu.Id equals ug.IdGrupoUsuario
                                      where ug.IdUsuario == idUsuario && guia.IdRol == (int)BusinessVariables.EnumRoles.AccesoCentroSoporte
                                      select a.Id).Distinct().ToList();
                result = db.Area.Where(w => lstAreas.Contains(w.Id)).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Area
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
        public Area ObtenerAreaById(int idArea)
        {
            Area result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Area.SingleOrDefault(w => w.Id == idArea);
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

        public List<Area> ObtenerAreasUsuarioTercero(int idUsuario, int idUsuarioTercero, bool insertarSeleccion)
        {
            List<Area> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                List<int> lstAreas = (from a in db.Area
                                      join aa in db.ArbolAcceso on a.Id equals aa.IdArea
                                      join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                      join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                      join gu in db.GrupoUsuario on guia.IdGrupoUsuario equals gu.Id
                                      join ug in db.UsuarioGrupo on gu.Id equals ug.IdGrupoUsuario
                                      where ug.IdUsuario == idUsuario && ug.IdUsuario == idUsuarioTercero
                                      select a.Id).Distinct().ToList();
                result = db.Area.Where(w => lstAreas.Contains(w.Id)).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Area
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

        public List<Area> ObtenerAreasUsuarioPublico(bool insertarSeleccion)
        {

            {
                List<Area> result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {

                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    List<int> lstAreas = (from a in db.Area
                                          join aa in db.ArbolAcceso on a.Id equals aa.IdArea
                                          where aa.Publico
                                          select a.Id).Distinct().ToList();
                    result = db.Area.Where(w => lstAreas.Contains(w.Id)).ToList();
                    if (insertarSeleccion)
                        result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                            new Area
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
        }

        public List<Area> ObtenerAreasTipoUsuario(int idTipoUsuario, bool insertarSeleccion)
        {

            {
                List<Area> result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    List<int> areas = (db.Area.Join(db.ArbolAcceso, a => a.Id, aa => aa.IdArea, (a, aa) => new { a, aa })
                        .Where(@t => @t.aa.Publico && @t.aa.EsTerminal && @t.aa.IdTipoUsuario == idTipoUsuario
                                     && !@t.a.Sistema && @t.a.Habilitado).Select(@t => @t.a.Id)).Distinct().ToList();
                    result = db.Area.Where(w => !w.Sistema && w.Habilitado && areas.Contains(w.Id)).ToList();
                    if (insertarSeleccion)
                        result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                            new Area
                            {
                                Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                                Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
                            });
                    if (result.Count <= 0)
                        result = null;
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

        public List<Area> ObtenerAreas(bool insertarSeleccion)
        {
            List<Area> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Area.Where(w => w.Habilitado && !w.Sistema).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Area
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

        public void Guardar(Area area)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                if (db.Area.Any(a => a.Descripcion == area.Descripcion))
                    throw new Exception("Esta Area ya existe.");
                //TODO: Cambiar habilitado por el embebido
                area.Habilitado = true;
                area.Descripcion = area.Descripcion.Trim();
                area.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                if (area.Id == 0)
                    db.Area.AddObject(area);
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

        public void GuardarAreaAndroid(Area descripcion)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                Area area = new Area();
                area.Habilitado = true;
                area.Descripcion = descripcion.Descripcion.Trim();
                area.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                if (area.Id == 0)
                    db.Area.AddObject(area);
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


        public void Actualizar(int idArea, Area area)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                if (db.Area.Any(a => a.Descripcion == area.Descripcion && a.Id != idArea))
                    throw new Exception("Esta Area ya existe.");
                db.ContextOptions.LazyLoadingEnabled = true;
                Area areaUpdate = db.Area.SingleOrDefault(s => s.Id == idArea);

                if (areaUpdate == null) return;
                areaUpdate.Descripcion = area.Descripcion.Trim();
                areaUpdate.Imagen = area.Imagen;
                areaUpdate.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                areaUpdate.IdUsuarioModifico = area.IdUsuarioModifico;
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

        public void Habilitar(int idArea, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Area inf = db.Area.SingleOrDefault(w => w.Id == idArea);
                if (inf != null) inf.Habilitado = habilitado;
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

        public List<Area> ObtenerAreaConsulta(string descripcion)
        {
            List<Area> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Area> qry = db.Area.Where(w => !w.Sistema);
                if (descripcion != string.Empty)
                    qry = qry.Where(w => w.Descripcion.Contains(descripcion));
                result = qry.OrderBy(o => o.Descripcion).ToList();
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
