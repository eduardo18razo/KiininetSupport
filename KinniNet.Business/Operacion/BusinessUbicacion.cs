using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Parametros;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessUbicacion : IDisposable
    {
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessUbicacion(bool proxy = false)
        {
            _proxy = proxy;
        }
        public List<Pais> ObtenerPais(int idTipoUsuario, bool insertarSeleccion)
        {
            List<Pais> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Pais.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Pais { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Campus> ObtenerCampus(int idTipoUsuario, int idPais, bool insertarSeleccion)
        {
            List<Campus> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Ubicacion.Where(w => w.IdPais == idPais).SelectMany(ubicacion => db.Campus.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Id == ubicacion.IdCampus && w.Habilitado)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Campus { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Torre> ObtenerTorres(int idTipoUsuario, int idCampus, bool insertarSeleccion)
        {
            List<Torre> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Ubicacion.Where(w => w.IdCampus == idCampus).SelectMany(ubicacion => db.Torre.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Id == ubicacion.IdTorre && w.Habilitado)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Torre { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Piso> ObtenerPisos(int idTipoUsuario, int idTorre, bool insertarSeleccion)
        {
            List<Piso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Ubicacion.Where(w => w.IdTorre == idTorre).SelectMany(ubicacion => db.Piso.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Id == ubicacion.IdPiso && w.Habilitado)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Piso { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Zona> ObtenerZonas(int idTipoUsuario, int idPiso, bool insertarSeleccion)
        {
            List<Zona> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Ubicacion.Where(w => w.IdPiso == idPiso).SelectMany(ubicacion => db.Zona.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Id == ubicacion.IdZona && w.Habilitado)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Zona { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<SubZona> ObtenerSubZonas(int idTipoUsuario, int idZona, bool insertarSeleccion)
        {
            List<SubZona> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Ubicacion.Where(w => w.IdZona == idZona).SelectMany(ubicacion => db.SubZona.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Id == ubicacion.IdSubZona && w.Habilitado)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new SubZona { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<SiteRack> ObtenerSiteRacks(int idTipoUsuario, int idSubZona, bool insertarSeleccion)
        {
            List<SiteRack> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Ubicacion.Where(w => w.IdSubZona == idSubZona).SelectMany(ubicacion => db.SiteRack.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Id == ubicacion.IdSiteRack && w.Habilitado)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new SiteRack { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public Ubicacion ObtenerUbicacion(int idPais, int? idCampus, int? idTorre, int? idPiso, int? idZona, int? idSubZona, int? idSiteRack)
        {
            Ubicacion result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = db.Ubicacion.Where(w => w.IdPais == idPais);
                if (idCampus.HasValue)
                    qry = qry.Where(w => w.IdCampus == idCampus);
                else
                    qry = qry.Where(w => w.IdCampus == null);

                if (idTorre.HasValue)
                    qry = qry.Where(w => w.IdTorre == idTorre);
                else
                    qry = qry.Where(w => w.IdTorre == null);

                if (idPiso.HasValue)
                    qry = qry.Where(w => w.IdPiso == idPiso);
                else
                    qry = qry.Where(w => w.IdPiso == null);

                if (idZona.HasValue)
                    qry = qry.Where(w => w.IdZona == idZona);
                else
                    qry = qry.Where(w => w.IdZona == null);

                if (idSubZona.HasValue)
                    qry = qry.Where(w => w.IdSubZona == idSubZona);
                else
                    qry = qry.Where(w => w.IdSubZona == null);

                if (idSiteRack.HasValue)
                    qry = qry.Where(w => w.IdSiteRack == idSiteRack);
                else
                    qry = qry.Where(w => w.IdSiteRack == null);

                result = qry.FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception("Error al Obtener Ubicacion");
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }
        public void GuardarUbicacion(Ubicacion ubicacion)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                ubicacion.Habilitado = true;
                ubicacion.Sistema = false;
                if (ubicacion.Campus != null)
                {
                    ubicacion.IdNivelUbicacion = 2;
                    ubicacion.Campus.Descripcion = ubicacion.Campus.Descripcion.Trim();
                    foreach (Domicilio domicilio in ubicacion.Campus.Domicilio)
                    {
                        domicilio.Calle = domicilio.Calle.Trim();
                        domicilio.NoExt = domicilio.NoExt.Trim();
                        domicilio.NoInt = domicilio.NoInt.Trim();
                    }
                    if (db.Ubicacion.Where(w => w.IdPais == ubicacion.IdPais).Any(a => a.Campus.Descripcion == ubicacion.Campus.Descripcion && a.IdTipoUsuario == ubicacion.Campus.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacion.IdTipoUsuario, 2).Descripcion));
                }
                if (ubicacion.Torre != null)
                {
                    ubicacion.IdNivelUbicacion = 3;
                    ubicacion.Torre.Descripcion = ubicacion.Torre.Descripcion.Trim();
                    if (db.Ubicacion.Where(w => w.IdPais == ubicacion.IdPais && w.IdCampus == ubicacion.IdCampus).Any(a => a.Torre.Descripcion == ubicacion.Torre.Descripcion && a.IdTipoUsuario == ubicacion.Torre.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacion.IdTipoUsuario, 3).Descripcion));
                }

                if (ubicacion.Piso != null)
                {
                    ubicacion.IdNivelUbicacion = 4;
                    ubicacion.Piso.Descripcion = ubicacion.Piso.Descripcion.Trim();
                    if (db.Ubicacion.Where(w => w.IdPais == ubicacion.IdPais && w.IdTorre == ubicacion.IdTorre).Any(a => a.Piso.Descripcion == ubicacion.Piso.Descripcion && a.IdTipoUsuario == ubicacion.Piso.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacion.IdTipoUsuario, 4).Descripcion));
                }

                if (ubicacion.Zona != null)
                {
                    ubicacion.IdNivelUbicacion = 5;
                    ubicacion.Zona.Descripcion = ubicacion.Zona.Descripcion.Trim();

                    if (db.Ubicacion.Where(w => w.IdPais == ubicacion.IdPais && w.IdPiso == ubicacion.IdPiso).Any(a => a.Zona.Descripcion == ubicacion.Zona.Descripcion && a.IdTipoUsuario == ubicacion.Zona.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacion.IdTipoUsuario, 5).Descripcion));
                }

                if (ubicacion.SubZona != null)
                {
                    ubicacion.IdNivelUbicacion = 6;
                    ubicacion.SubZona.Descripcion = ubicacion.SubZona.Descripcion.Trim();
                    if (db.Ubicacion.Where(w => w.IdPais == ubicacion.IdPais && w.IdZona == ubicacion.IdZona).Any(a => a.SubZona.Descripcion == ubicacion.SubZona.Descripcion && a.IdTipoUsuario == ubicacion.SubZona.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacion.IdTipoUsuario, 6).Descripcion));
                }

                if (ubicacion.SiteRack != null)
                {
                    ubicacion.IdNivelUbicacion = 7;
                    ubicacion.SiteRack.Descripcion = ubicacion.SiteRack.Descripcion.Trim();
                    if (db.Ubicacion.Where(w => w.IdPais == ubicacion.IdPais && w.IdSubZona == ubicacion.IdSubZona).Any(a => a.SiteRack.Descripcion == ubicacion.SiteRack.Descripcion && a.IdTipoUsuario == ubicacion.SiteRack.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacion.IdTipoUsuario, 7).Descripcion));
                }

                if (ubicacion.Id == 0)
                    db.Ubicacion.AddObject(ubicacion);
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
        public Ubicacion ObtenerUbicacionUsuario(int idUbicacion)
        {
            Ubicacion result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Ubicacion.SingleOrDefault(w => w.Id == idUbicacion && w.Habilitado);
                if (result != null)
                {
                    db.LoadProperty(result, "Pais");
                    db.LoadProperty(result, "Campus");
                    db.LoadProperty(result, "Torre");
                    db.LoadProperty(result, "Piso");
                    db.LoadProperty(result, "Zona");
                    db.LoadProperty(result, "SubZona");
                    db.LoadProperty(result, "SiteRack");
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
        public List<Ubicacion> ObtenerUbicacionByRegionCode(string regionCode)
        {
            List<Ubicacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = from u in db.Ubicacion
                          join d in db.Domicilio on u.IdCampus equals d.IdCampus
                          join col in db.Colonia on d.IdColonia equals col.Id
                          join m in db.Municipio on col.IdMunicipio equals m.Id
                          join e in db.Estado on m.IdEstado equals e.Id
                          where e.RegionCode == regionCode && !u.Sistema
                          select u;
                result = qry.Distinct().ToList();
                foreach (Ubicacion ubicacion in result)
                {
                    db.LoadProperty(ubicacion, "Pais");
                    db.LoadProperty(ubicacion, "Campus");
                    db.LoadProperty(ubicacion, "Torre");
                    db.LoadProperty(ubicacion, "Piso");
                    db.LoadProperty(ubicacion, "Zona");
                    db.LoadProperty(ubicacion, "SubZona");
                    db.LoadProperty(ubicacion, "SiteRack");
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
        public List<int> ObtenerUbicacionesByIdUbicacion(int idUbicacion)
        {
            List<int> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Ubicacion tmpUbicacion = db.Ubicacion.SingleOrDefault(s => s.Id == idUbicacion);
                IQueryable<Ubicacion> qry = db.Ubicacion.Where(w => w.Id != idUbicacion);
                if (tmpUbicacion != null)
                {
                    if (tmpUbicacion.IdSiteRack.HasValue)
                        qry = qry.Where(w => w.IdSiteRack == tmpUbicacion.IdSiteRack);
                    else if (tmpUbicacion.IdSubZona.HasValue)
                        qry = qry.Where(w => w.IdSubZona == tmpUbicacion.IdSubZona);
                    else if (tmpUbicacion.IdZona.HasValue)
                        qry = qry.Where(w => w.IdZona == tmpUbicacion.IdZona);
                    else if (tmpUbicacion.IdPiso.HasValue)
                        qry = qry.Where(w => w.IdPiso == tmpUbicacion.IdPiso);
                    else if (tmpUbicacion.IdTorre.HasValue)
                        qry = qry.Where(w => w.IdTorre == tmpUbicacion.IdTorre);
                    else if (tmpUbicacion.IdCampus.HasValue)
                        qry = qry.Where(w => w.IdCampus == tmpUbicacion.IdCampus);
                    else
                        qry = qry.Where(w => w.IdPais == tmpUbicacion.IdPais);
                }
                result = qry.Select(s => s.Id).ToList();
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
        public List<Ubicacion> ObtenerUbicaciones(int? idTipoUsuario, int? idPais, int? idCampus, int? idTorre, int? idPiso, int? idZona, int? idSubZona, int? idSiteRack)
        {
            List<Ubicacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Ubicacion> qry = db.Ubicacion.Where(w => !w.Sistema);
                if (idTipoUsuario.HasValue)
                    qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);

                if (idPais.HasValue)
                    qry = qry.Where(w => w.IdPais == idPais);

                if (idCampus.HasValue)
                    qry = qry.Where(w => w.IdCampus == idCampus);

                if (idTorre.HasValue)
                    qry = qry.Where(w => w.IdTorre == idTorre);

                if (idPiso.HasValue)
                    qry = qry.Where(w => w.IdPiso == idPiso);

                if (idZona.HasValue)
                    qry = qry.Where(w => w.IdZona == idZona);

                if (idSubZona.HasValue)
                    qry = qry.Where(w => w.IdSubZona == idSubZona);

                if (idSiteRack.HasValue)
                    qry = qry.Where(w => w.IdSiteRack == idSiteRack);
                qry = from q in qry
                      orderby q.Pais != null, q.Pais.Descripcion, q.Campus != null, q.Campus.Descripcion, q.Torre != null, q.Torre.Descripcion, q.Piso != null, q.Piso.Descripcion,
                      q.Zona != null, q.Zona.Descripcion, q.SubZona != null, q.SubZona.Descripcion, q.SiteRack != null, q.SiteRack.Descripcion ascending
                      select q;
                result = qry.ToList();
                foreach (Ubicacion ubicacion in result)
                {
                    db.LoadProperty(ubicacion, "TipoUsuario");
                    db.LoadProperty(ubicacion, "Pais");
                    db.LoadProperty(ubicacion, "Campus");
                    db.LoadProperty(ubicacion, "Torre");
                    db.LoadProperty(ubicacion, "Piso");
                    db.LoadProperty(ubicacion, "Zona");
                    db.LoadProperty(ubicacion, "SubZona");
                    db.LoadProperty(ubicacion, "SiteRack");
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
        public List<Ubicacion> BuscarPorPalabra(int? idTipoUsuario, int? idPais, int? idCampus, int? idTorre, int? idPiso, int? idZona, int? idSubZona, int? idSiteRack, string filtro)
        {
            {
                List<Ubicacion> result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    IQueryable<Ubicacion> qry = db.Ubicacion.Where(w => !w.Sistema);
                    if (idTipoUsuario.HasValue)
                        qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);

                    if (idPais.HasValue)
                        qry = qry.Where(w => w.IdPais == idPais);

                    if (idCampus.HasValue)
                        qry = qry.Where(w => w.IdCampus == idCampus);

                    if (idTorre.HasValue)
                        qry = qry.Where(w => w.IdTorre == idTorre);

                    if (idPiso.HasValue)
                        qry = qry.Where(w => w.IdPiso == idPiso);

                    if (idZona.HasValue)
                        qry = qry.Where(w => w.IdZona == idZona);

                    if (idSubZona.HasValue)
                        qry = qry.Where(w => w.IdSubZona == idSubZona);

                    if (idSiteRack.HasValue)
                        qry = qry.Where(w => w.IdSiteRack == idSiteRack);

                    if (filtro.Trim() != string.Empty)
                    {
                        filtro = filtro.ToLower().Trim();
                        qry = qry.Where(w => w.Pais.Descripcion.ToLower().Contains(filtro)
                            || w.Campus.Descripcion.ToLower().Contains(filtro)
                            || w.Torre.Descripcion.ToLower().Contains(filtro)
                            || w.Piso.Descripcion.ToLower().Contains(filtro)
                            || w.Zona.Descripcion.ToLower().Contains(filtro)
                            || w.SubZona.Descripcion.ToLower().Contains(filtro)
                            || w.SiteRack.Descripcion.ToLower().Contains(filtro));
                    }
                    qry = from q in qry
                          orderby q.Pais != null, q.Pais.Descripcion, q.Campus != null, q.Campus.Descripcion,
                              q.Torre != null, q.Torre.Descripcion, q.Piso != null, q.Piso.Descripcion,
                              q.Zona != null, q.Zona.Descripcion, q.SubZona != null, q.SubZona.Descripcion,
                              q.SiteRack != null, q.SiteRack.Descripcion ascending
                          select q;
                    result = qry.ToList();
                    foreach (Ubicacion ubicacion in result)
                    {
                        db.LoadProperty(ubicacion, "TipoUsuario");
                        db.LoadProperty(ubicacion, "Pais");
                        db.LoadProperty(ubicacion, "Campus");
                        db.LoadProperty(ubicacion, "Torre");
                        db.LoadProperty(ubicacion, "Piso");
                        db.LoadProperty(ubicacion, "Zona");
                        db.LoadProperty(ubicacion, "SubZona");
                        db.LoadProperty(ubicacion, "SiteRack");
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
        public List<Ubicacion> ObtenerUbicacionesGrupos(List<int> grupos)
        {
            List<Ubicacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Ubicacion> qry = from u in db.Ubicacion
                                            join gu in db.GrupoUsuario on u.IdTipoUsuario equals gu.IdTipoUsuario
                                            where grupos.Contains(gu.Id) && !u.Sistema
                                            select u;

                result = qry.Distinct().ToList();
                foreach (Ubicacion ubicacion in result)
                {
                    db.LoadProperty(ubicacion, "Pais");
                    db.LoadProperty(ubicacion, "Campus");
                    db.LoadProperty(ubicacion, "Torre");
                    db.LoadProperty(ubicacion, "Piso");
                    db.LoadProperty(ubicacion, "Zona");
                    db.LoadProperty(ubicacion, "SubZona");
                    db.LoadProperty(ubicacion, "SiteRack");
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
        public string ObtenerDescripcionUbicacionUsuario(int idUsuario, bool ultimoNivel)
        {
            string result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Usuario usuario = db.Usuario.SingleOrDefault(w => w.Id == idUsuario && w.Habilitado);
                if (usuario != null)
                {
                    if (usuario.Ubicacion != null)
                    {
                        if (ultimoNivel)
                        {
                            if (usuario.Ubicacion.Pais != null)
                                result = usuario.Ubicacion.Pais.Descripcion;
                            if (usuario.Ubicacion.Campus != null)
                                result = usuario.Ubicacion.Campus.Descripcion;
                            if (usuario.Ubicacion.Torre != null)
                                result = usuario.Ubicacion.Torre.Descripcion;
                            if (usuario.Ubicacion.Piso != null)
                                result = usuario.Ubicacion.Piso.Descripcion;
                            if (usuario.Ubicacion.Zona != null)
                                result = usuario.Ubicacion.Zona.Descripcion;
                            if (usuario.Ubicacion.SubZona != null)
                                result = usuario.Ubicacion.SubZona.Descripcion;
                            if (usuario.Ubicacion.SiteRack != null)
                                result = usuario.Ubicacion.SiteRack.Descripcion;
                        }
                        else
                        {
                            if (usuario.Ubicacion.Pais != null)
                                result += usuario.Ubicacion.Pais.Descripcion;
                            if (usuario.Ubicacion.Campus != null)
                                result += ">" + usuario.Ubicacion.Campus.Descripcion;
                            if (usuario.Ubicacion.Torre != null)
                                result += ">" + usuario.Ubicacion.Torre.Descripcion;
                            if (usuario.Ubicacion.Piso != null)
                                result += ">" + usuario.Ubicacion.Piso.Descripcion;
                            if (usuario.Ubicacion.Zona != null)
                                result += ">" + usuario.Ubicacion.Zona.Descripcion;
                            if (usuario.Ubicacion.SubZona != null)
                                result += ">" + usuario.Ubicacion.SubZona.Descripcion;
                            if (usuario.Ubicacion.SiteRack != null)
                                result += ">" + usuario.Ubicacion.SiteRack.Descripcion;
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
        public string ObtenerDescripcionUbicacionById(int idUbicacion, bool ultimoNivel)
        {
            string result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Ubicacion ubicacion = db.Ubicacion.SingleOrDefault(w => w.Id == idUbicacion && w.Habilitado);
                if (ubicacion != null)
                {
                    if (ultimoNivel)
                    {
                        if (ubicacion.Pais != null)
                            result = ubicacion.Pais.Descripcion;
                        if (ubicacion.Campus != null)
                            result = ubicacion.Campus.Descripcion;
                        if (ubicacion.Torre != null)
                            result = ubicacion.Torre.Descripcion;
                        if (ubicacion.Piso != null)
                            result = ubicacion.Piso.Descripcion;
                        if (ubicacion.Zona != null)
                            result = ubicacion.Zona.Descripcion;
                        if (ubicacion.SubZona != null)
                            result = ubicacion.SubZona.Descripcion;
                        if (ubicacion.SiteRack != null)
                            result = ubicacion.SiteRack.Descripcion;
                    }
                    else
                    {
                        if (ubicacion.Pais != null)
                            result += ubicacion.Pais.Descripcion;
                        if (ubicacion.Campus != null)
                            result += ">" + ubicacion.Campus.Descripcion;
                        if (ubicacion.Torre != null)
                            result += ">" + ubicacion.Torre.Descripcion;
                        if (ubicacion.Piso != null)
                            result += ">" + ubicacion.Piso.Descripcion;
                        if (ubicacion.Zona != null)
                            result += ">" + ubicacion.Zona.Descripcion;
                        if (ubicacion.SubZona != null)
                            result += ">" + ubicacion.SubZona.Descripcion;
                        if (ubicacion.SiteRack != null)
                            result += ">" + ubicacion.SiteRack.Descripcion;
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
        public void ActualizarUbicacion(Ubicacion ub)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Ubicacion ubicacionBase = db.Ubicacion.SingleOrDefault(s => s.Id == ub.Id);
                if (ubicacionBase != null)
                {
                    if (ubicacionBase.Pais != null)
                    {
                        if (db.Pais.Any(a => a.Descripcion == ub.Pais.Descripcion && a.IdTipoUsuario == ub.Pais.IdTipoUsuario && a.Id != ubicacionBase.Pais.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacionBase.IdTipoUsuario, 1).Descripcion));
                        ubicacionBase.Pais.Descripcion = ub.Pais.Descripcion.Trim();
                    }

                    if (ubicacionBase.Campus != null)
                    {
                        if (db.Ubicacion.Where(w => w.IdPais == ub.IdPais).Any(a => a.Campus.Descripcion == ub.Campus.Descripcion && a.IdTipoUsuario == ub.Campus.IdTipoUsuario && a.Campus.Id != ubicacionBase.Campus.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacionBase.IdTipoUsuario, 2).Descripcion));

                        ubicacionBase.Campus.Descripcion = ub.Campus.Descripcion.Trim();
                        if (ubicacionBase.Torre == null)
                        {
                            ub.Campus.Domicilio = ub.Campus.Domicilio ?? new List<Domicilio>();

                            if (ubicacionBase.Campus.Domicilio.Count == 0)
                                ubicacionBase.Campus.Domicilio.Add(new Domicilio());
                            ubicacionBase.Campus.Domicilio[0].IdColonia = ub.Campus.Domicilio[0].IdColonia;
                            ubicacionBase.Campus.Domicilio[0].Calle = ub.Campus.Domicilio[0].Calle.Trim();
                            ubicacionBase.Campus.Domicilio[0].NoExt = ub.Campus.Domicilio[0].NoExt.Trim();
                            ubicacionBase.Campus.Domicilio[0].NoInt = ub.Campus.Domicilio[0].NoInt.Trim();
                        }
                    }

                    if (ubicacionBase.Torre != null)
                    {
                        if (db.Ubicacion.Where(w => w.IdPais == ub.IdPais && w.IdCampus == ub.IdCampus).Any(a => a.Torre.Descripcion == ub.Torre.Descripcion && a.IdTipoUsuario == ub.Torre.IdTipoUsuario && a.Torre.Id != ubicacionBase.Torre.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacionBase.IdTipoUsuario, 3).Descripcion));
                        ubicacionBase.Torre.Descripcion = ub.Torre.Descripcion.Trim();
                    }

                    if (ubicacionBase.Piso != null)
                    {
                        if (db.Ubicacion.Where(w => w.IdPais == ub.IdPais && w.IdTorre == ub.IdTorre).Any(a => a.Piso.Descripcion == ub.Piso.Descripcion && a.IdTipoUsuario == ub.Piso.IdTipoUsuario && a.Piso.Id != ubicacionBase.Piso.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacionBase.IdTipoUsuario, 4).Descripcion));
                        ubicacionBase.Piso.Descripcion = ub.Piso.Descripcion.Trim();
                    }

                    if (ubicacionBase.Zona != null)
                    {
                        if (db.Ubicacion.Where(w => w.IdPais == ub.IdPais && w.IdPiso == ub.IdPiso).Any(a => a.Zona.Descripcion == ub.Zona.Descripcion && a.IdTipoUsuario == ub.Zona.IdTipoUsuario && a.Zona.Id != ubicacionBase.Zona.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacionBase.IdTipoUsuario, 5).Descripcion));
                        ubicacionBase.Zona.Descripcion = ub.Zona.Descripcion.Trim();
                    }

                    if (ubicacionBase.SubZona != null)
                    {
                        if (db.Ubicacion.Where(w => w.IdPais == ub.IdPais && w.IdZona == ub.IdZona).Any(a => a.SubZona.Descripcion == ub.SubZona.Descripcion && a.IdTipoUsuario == ub.SubZona.IdTipoUsuario && a.SubZona.Id != ubicacionBase.SubZona.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacionBase.IdTipoUsuario, 6).Descripcion));
                        ubicacionBase.SubZona.Descripcion = ub.SubZona.Descripcion.Trim();
                    }

                    if (ubicacionBase.SiteRack != null)
                    {
                        if (db.Ubicacion.Where(w => w.IdPais == ub.IdPais && w.IdCampus == ub.IdCampus).Any(a => a.SiteRack.Descripcion == ub.SiteRack.Descripcion && a.IdTipoUsuario == ub.SiteRack.IdTipoUsuario && a.SiteRack.Id != ubicacionBase.SiteRack.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasUbicacionNivel(ubicacionBase.IdTipoUsuario, 7).Descripcion));
                        ubicacionBase.SiteRack.Descripcion = ub.SiteRack.Descripcion.Trim();
                    }

                    if (ubicacionBase.Id == 0)
                        db.Ubicacion.AddObject(ubicacionBase);

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
        public Ubicacion ObtenerUbicacionById(int idUbicacion)
        {
            Ubicacion result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Ubicacion.SingleOrDefault(w => w.Id == idUbicacion);
                if (result != null)
                {
                    db.LoadProperty(result, "TipoUsuario");
                    db.LoadProperty(result, "Pais");
                    db.LoadProperty(result, "Campus");
                    db.LoadProperty(result, "Torre");
                    db.LoadProperty(result, "Piso");
                    db.LoadProperty(result, "Zona");
                    db.LoadProperty(result, "SubZona");
                    db.LoadProperty(result, "SiteRack");
                    if (result.IdNivelUbicacion == 2)
                    {
                        db.LoadProperty(result.Campus, "Domicilio");
                        foreach (Domicilio domicilio in result.Campus.Domicilio)
                        {
                            db.LoadProperty(domicilio, "Colonia");
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
        public void HabilitarUbicacion(int idUbicacion, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                db.ContextOptions.ProxyCreationEnabled = true;
                Ubicacion ub = db.Ubicacion.SingleOrDefault(w => w.Id == idUbicacion);
                if (ub != null)
                {
                    //if (ub.HitConsulta.Any() || ub.Ticket.Any() || ub.Usuario.Any())
                    //    throw new Exception("La ubicacion ya se encuetra con datos asociasdos no puede ser eliminada");
                    ub.Habilitado = habilitado;
                    var qry = db.Ubicacion.Where(w => w.IdTipoUsuario == ub.IdTipoUsuario && w.IdPais == ub.IdPais);
                    if (!habilitado)
                    {
                        qry = qry.Where(w => w.IdNivelUbicacion >= ub.IdNivelUbicacion && w.Habilitado);
                        foreach (Ubicacion source in qry.OrderBy(o => o.Id))
                        {
                            //if (ub.HitConsulta.Any() || ub.Ticket.Any() || ub.Usuario.Any())
                            //    throw new Exception("La ubicacion ya se encuetra con datos asociasdos no puede ser eliminada");
                            switch (ub.IdNivelUbicacion)
                            {
                                case 1:
                                    break;
                                case 2:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus)
                                    {
                                        source.Campus.Habilitado = false;
                                        if (source.IdTorre.HasValue)
                                            source.Torre.Habilitado = false;
                                        if (source.IdPiso.HasValue)
                                            source.Piso.Habilitado = false;
                                        if (source.IdZona.HasValue)
                                            source.Zona.Habilitado = false;
                                        if (source.IdSubZona.HasValue)
                                            source.SubZona.Habilitado = false;
                                        if (source.IdSiteRack.HasValue)
                                            source.SiteRack.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 3:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre)
                                    {
                                        source.Torre.Habilitado = false;
                                        if (source.IdPiso.HasValue)
                                            source.Piso.Habilitado = false;
                                        if (source.IdZona.HasValue)
                                            source.Zona.Habilitado = false;
                                        if (source.IdSubZona.HasValue)
                                            source.SubZona.Habilitado = false;
                                        if (source.IdSiteRack.HasValue)
                                            source.SiteRack.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 4:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre && source.IdPiso == ub.IdPiso)
                                    {
                                        source.Piso.Habilitado = false;
                                        if (source.IdZona.HasValue)
                                            source.Zona.Habilitado = false;
                                        if (source.IdSubZona.HasValue)
                                            source.SubZona.Habilitado = false;
                                        if (source.IdSiteRack.HasValue)
                                            source.SiteRack.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 5:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre && source.IdPiso == ub.IdPiso && source.IdZona == ub.IdZona)
                                    {
                                        source.Zona.Habilitado = false;
                                        if (source.IdSubZona.HasValue)
                                            source.SubZona.Habilitado = false;
                                        if (source.IdSiteRack.HasValue)
                                            source.SiteRack.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 6:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre && source.IdPiso == ub.IdPiso && source.IdZona == ub.IdZona && source.IdSubZona == ub.IdSubZona)
                                    {
                                        source.SubZona.Habilitado = false;
                                        if (source.IdSiteRack.HasValue)
                                            source.SiteRack.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 7:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre && source.IdPiso == ub.IdPiso && source.IdZona == ub.IdZona && source.IdSubZona == ub.IdSubZona && source.IdSiteRack == ub.IdSiteRack)
                                    {
                                        source.SiteRack.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        qry = qry.Where(w => w.IdNivelUbicacion <= ub.IdNivelUbicacion && !w.Habilitado);
                        switch (ub.IdNivelUbicacion)
                        {
                            case 1:
                                ub.Pais.Habilitado = true;
                                break;
                            case 2:
                                ub.Pais.Habilitado = true;
                                ub.Campus.Habilitado = true;
                                break;
                            case 3:
                                ub.Pais.Habilitado = true;
                                ub.Campus.Habilitado = true;
                                ub.Torre.Habilitado = true;
                                break;
                            case 4:
                                ub.Pais.Habilitado = true;
                                ub.Campus.Habilitado = true;
                                ub.Torre.Habilitado = true;
                                ub.Piso.Habilitado = true;
                                break;
                            case 5:
                                ub.Pais.Habilitado = true;
                                ub.Campus.Habilitado = true;
                                ub.Torre.Habilitado = true;
                                ub.Piso.Habilitado = true;
                                ub.Zona.Habilitado = true;
                                break;
                            case 6:
                                ub.Pais.Habilitado = true;
                                ub.Campus.Habilitado = true;
                                ub.Torre.Habilitado = true;
                                ub.Piso.Habilitado = true;
                                ub.Zona.Habilitado = true;
                                ub.SubZona.Habilitado = true;
                                break;
                            case 7:
                                ub.Pais.Habilitado = true;
                                ub.Campus.Habilitado = true;
                                ub.Torre.Habilitado = true;
                                ub.Piso.Habilitado = true;
                                ub.Zona.Habilitado = true;
                                ub.SubZona.Habilitado = true;
                                ub.SiteRack.Habilitado = true;
                                break;
                        }
                        if (ub.IdCampus.HasValue)
                            qry = qry.Where(w => w.IdCampus == ub.IdCampus);
                        foreach (Ubicacion source in qry.OrderBy(o => o.Id))
                        {
                            //if (ub.HitConsulta.Any() || ub.Ticket.Any() || ub.Usuario.Any())
                            //    throw new Exception("La ubicacion ya se encuetra con datos asociasdos no puede ser eliminada");
                            switch (source.IdNivelUbicacion)
                            {
                                case 1:
                                    break;
                                case 2:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus)
                                    {
                                        source.Campus.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 3:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre)
                                    {
                                        source.Campus.Habilitado = true;
                                        source.Torre.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 4:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre && source.IdPiso == ub.IdPiso)
                                    {
                                        source.Campus.Habilitado = true;
                                        source.Torre.Habilitado = true;
                                        source.Piso.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 5:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre && source.IdPiso == ub.IdPiso && source.IdZona == ub.IdZona)
                                    {
                                        source.Campus.Habilitado = true;
                                        source.Torre.Habilitado = true;
                                        source.Piso.Habilitado = true;
                                        source.Zona.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 6:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre && source.IdPiso == ub.IdPiso && source.IdZona == ub.IdZona && source.IdSubZona == ub.IdSubZona)
                                    {
                                        source.Campus.Habilitado = true;
                                        source.Torre.Habilitado = true;
                                        source.Piso.Habilitado = true;
                                        source.Zona.Habilitado = true;
                                        source.SubZona.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 7:
                                    if (source.IdPais == ub.IdPais && source.IdCampus == ub.IdCampus && source.IdTorre == ub.IdTorre && source.IdPiso == ub.IdPiso && source.IdZona == ub.IdZona && source.IdSubZona == ub.IdSubZona && source.IdSiteRack == ub.IdSiteRack)
                                    {
                                        source.Campus.Habilitado = true;
                                        source.Torre.Habilitado = true;
                                        source.Piso.Habilitado = true;
                                        source.Zona.Habilitado = true;
                                        source.SubZona.Habilitado = true;
                                        source.SiteRack.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                            }
                        }
                    }
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

        public Ubicacion ObtenerUbicacionFiscal(int idColonia, string calle, string noExt, string noInt)
        {
            Ubicacion result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                db.ContextOptions.ProxyCreationEnabled = true;
                bool insertaNueva = false;
                Colonia colonia = db.Colonia.SingleOrDefault(s => s.Id == idColonia);
                if (colonia != null)
                {
                    result = new Ubicacion
                    {
                        IdPais = colonia.Municipio.Estado.IdPais,
                        IdCampus = colonia.Municipio.IdEstado
                    };

                    Piso piso = db.Piso.SingleOrDefault(w => w.Descripcion == colonia.Descripcion) ?? new Piso { Descripcion = colonia.Descripcion, Habilitado = true };
                    if (piso.Id == 0)
                        result.Piso = piso;
                    else
                        result.IdPiso = piso.Id;

                    Zona zona = db.Zona.SingleOrDefault(w => w.Descripcion == calle) ?? new Zona { Descripcion = calle, Habilitado = true };
                    if (zona.Id == 0)
                        result.Zona = zona;
                    else
                        result.IdZona = zona.Id;
                    SubZona subZona = db.SubZona.SingleOrDefault(w => w.Descripcion == noExt) ?? new SubZona { Descripcion = noExt, Habilitado = true };
                    if (subZona.Id == 0)
                        result.SubZona = subZona;
                    else
                        result.IdSubZona = subZona.Id;
                    if (noInt.Trim() != string.Empty)
                    {
                        SiteRack siteRack = db.SiteRack.SingleOrDefault(w => w.Descripcion == noInt) ?? new SiteRack { Descripcion = noInt, Habilitado = true };
                        if (siteRack.Id == 0)
                            result.SiteRack = siteRack;
                        else
                            result.IdSiteRack = siteRack.Id;
                        result.IdNivelUbicacion = 7;
                    }

                    result.IdNivelUbicacion = result.IdNivelUbicacion == 0 ? 6 : result.IdNivelUbicacion;

                    if (result.Piso != null || result.Zona != null || result.SubZona != null || result.SiteRack != null)
                        insertaNueva = true;
                    if (insertaNueva)
                    {
                        db.Ubicacion.AddObject(result);
                        db.SaveChanges();
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
        public Ubicacion ObtenerOrganizacionDefault(int idTipoUsuario)
        {
            Ubicacion result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                ParametrosUsuario parameters = db.ParametrosUsuario.SingleOrDefault(s => s.IdTipoUsuario == idTipoUsuario);
                if (parameters != null)
                {
                    result = db.Ubicacion.SingleOrDefault(w => w.Id == parameters.IdUbicacion);
                    if (result != null)
                    {
                        db.LoadProperty(result, "TipoUsuario");
                        db.LoadProperty(result, "Pais");
                        db.LoadProperty(result, "Campus");
                        db.LoadProperty(result, "Torre");
                        db.LoadProperty(result, "Piso");
                        db.LoadProperty(result, "Zona");
                        db.LoadProperty(result, "SubZona");
                        db.LoadProperty(result, "SiteRack");
                        if (result.IdNivelUbicacion == 2)
                        {
                            db.LoadProperty(result.Campus, "Domicilio");
                            foreach (Domicilio domicilio in result.Campus.Domicilio)
                            {
                                db.LoadProperty(domicilio, "Colonia");
                            }
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
}
