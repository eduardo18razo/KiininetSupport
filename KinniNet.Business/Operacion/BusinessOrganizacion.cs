using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Parametros;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessOrganizacion : IDisposable
    {
        private bool _proxy;

        public void Dispose()
        {

        }

        public BusinessOrganizacion(bool proxy = false)
        {
            _proxy = proxy;
        }

        public List<Holding> ObtenerHoldings(int idTipoUsuario, bool insertarSeleccion)
        {
            List<Holding> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result =
                    db.Holding.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Habilitado)
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Holding
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione,
                            Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado
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

        public List<Compania> ObtenerCompañias(int idTipoUsuario, int idHolding, bool insertarSeleccion)
        {
            List<Compania> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Organizacion.Where(w => w.IdHolding == idHolding)
                        .SelectMany(organizacion => db.Compañia.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Id == organizacion.IdCompania && w.Habilitado))
                        .Distinct()
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Compania
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione,
                            Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado
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

        public List<Direccion> ObtenerDirecciones(int idTipoUsuario, int idCompañia, bool insertarSeleccion)
        {
            List<Direccion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result =
                    db.Organizacion.Where(w => w.IdCompania == idCompañia)
                        .SelectMany(organizacion => db.Direccion.Where(w => w.IdTipoUsuario == idTipoUsuario && w.Id == organizacion.IdDireccion && w.Habilitado))
                        .Distinct()
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Direccion
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione,
                            Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado
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

        public List<SubDireccion> ObtenerSubDirecciones(int idTipoUsuario, int idDireccoin, bool insertarSeleccion)
        {
            List<SubDireccion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result =
                    db.Organizacion.Where(w => w.IdDireccion == idDireccoin)
                        .SelectMany(
                            organizacion =>
                                db.SubDireccion.Where(
                                    w =>
                                        w.IdTipoUsuario == idTipoUsuario && w.Id == organizacion.IdSubDireccion &&
                                        w.Habilitado))
                        .Distinct()
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new SubDireccion
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione,
                            Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado
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

        public List<Gerencia> ObtenerGerencias(int idTipoUsuario, int idSubdireccion, bool insertarSeleccion)
        {
            List<Gerencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result =
                    db.Organizacion.Where(w => w.IdSubDireccion == idSubdireccion)
                        .SelectMany(
                            organizacion =>
                                db.Gerencia.Where(
                                    w =>
                                        w.IdTipoUsuario == idTipoUsuario && w.Id == organizacion.IdGerencia &&
                                        w.Habilitado))
                        .Distinct()
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Gerencia
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione,
                            Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado
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

        public List<SubGerencia> ObtenerSubGerencias(int idTipoUsuario, int idGerencia, bool insertarSeleccion)
        {
            List<SubGerencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result =
                    db.Organizacion.Where(w => w.IdGerencia == idGerencia)
                        .SelectMany(
                            organizacion =>
                                db.SubGerencia.Where(
                                    w =>
                                        w.IdTipoUsuario == idTipoUsuario && w.Id == organizacion.IdSubGerencia &&
                                        w.Habilitado))
                        .Distinct()
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new SubGerencia
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione,
                            Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado
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

        public List<Jefatura> ObtenerJefaturas(int idTipoUsuario, int idSubGerencia, bool insertarSeleccion)
        {
            List<Jefatura> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result =
                    db.Organizacion.Where(w => w.IdSubGerencia == idSubGerencia)
                        .SelectMany(
                            organizacion =>
                                db.Jefatura.Where(
                                    w =>
                                        w.IdTipoUsuario == idTipoUsuario && w.Id == organizacion.IdJefatura &&
                                        w.Habilitado))
                        .Distinct()
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Jefatura
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione,
                            Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado
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

        public Organizacion ObtenerOrganizacion(int idHolding, int? idCompania, int? idDireccion, int? idSubDireccion, int? idGerencia, int? idSubGerencia, int? idJefatura)
        {
            Organizacion result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                var qry = db.Organizacion.Where(w => w.IdHolding == idHolding);
                if (idCompania.HasValue)
                    qry = qry.Where(w => w.IdCompania == idCompania);
                else
                    qry = qry.Where(w => w.IdCompania == null);

                if (idDireccion.HasValue)
                    qry = qry.Where(w => w.IdDireccion == idDireccion);
                else
                    qry = qry.Where(w => w.IdDireccion == null);

                if (idSubDireccion.HasValue)
                    qry = qry.Where(w => w.IdSubDireccion == idSubDireccion);
                else
                    qry = qry.Where(w => w.IdSubDireccion == null);

                if (idGerencia.HasValue)
                    qry = qry.Where(w => w.IdGerencia == idGerencia);
                else
                    qry = qry.Where(w => w.IdGerencia == null);

                if (idSubGerencia.HasValue)
                    qry = qry.Where(w => w.IdSubGerencia == idSubGerencia);
                else
                    qry = qry.Where(w => w.IdSubGerencia == null);

                if (idJefatura.HasValue)
                    qry = qry.Where(w => w.IdJefatura == idJefatura);
                else
                    qry = qry.Where(w => w.IdJefatura == null);

                result = qry.FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception("Error al Obtener Organizacion");
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public List<Organizacion> BuscarPorPalabra(int? idTipoUsuario, int? idHolding, int? idCompania, int? idDireccion, int? idSubDireccion, int? idGerencia, int? idSubGerencia, int? idJefatura, string filtro)
        {
            {
                List<Organizacion> result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    db.ContextOptions.ProxyCreationEnabled = _proxy;
                    IQueryable<Organizacion> qry = db.Organizacion.Where(w => !w.Sistema && w.TipoUsuario.Habilitado);
                    if (idTipoUsuario.HasValue)
                        qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);

                    if (idHolding.HasValue)
                        qry = qry.Where(w => w.IdHolding == idHolding);

                    if (idCompania.HasValue)
                        qry = qry.Where(w => w.IdCompania == idCompania);

                    if (idDireccion.HasValue)
                        qry = qry.Where(w => w.IdDireccion == idDireccion);

                    if (idSubDireccion.HasValue)
                        qry = qry.Where(w => w.IdSubDireccion == idSubDireccion);

                    if (idGerencia.HasValue)
                        qry = qry.Where(w => w.IdGerencia == idGerencia);

                    if (idSubGerencia.HasValue)
                        qry = qry.Where(w => w.IdSubGerencia == idSubGerencia);

                    if (idJefatura.HasValue)
                        qry = qry.Where(w => w.IdJefatura == idJefatura);

                    if (filtro.Trim() != string.Empty)
                    {
                        filtro = filtro.ToLower().Trim();
                        qry = qry.Where(w => w.Holding.Descripcion.ToLower().Contains(filtro)
                                             || w.Compania.Descripcion.ToLower().Contains(filtro)
                                             || w.Direccion.Descripcion.ToLower().Contains(filtro)
                                             || w.SubDireccion.Descripcion.ToLower().Contains(filtro)
                                             || w.Gerencia.Descripcion.ToLower().Contains(filtro)
                                             || w.SubGerencia.Descripcion.ToLower().Contains(filtro)
                                             || w.Jefatura.Descripcion.ToLower().Contains(filtro));
                    }
                    qry = from q in qry
                          orderby q.Holding != null, q.Holding.Descripcion, q.Compania != null, q.Compania.Descripcion,
                              q.Direccion != null, q.Direccion.Descripcion, q.SubDireccion != null,
                              q.SubDireccion.Descripcion,
                              q.Gerencia != null, q.Gerencia.Descripcion, q.SubGerencia != null, q.SubGerencia.Descripcion,
                              q.Jefatura != null, q.Compania.Descripcion ascending
                          select q;
                    result = qry.ToList();
                    foreach (Organizacion organizacion in result)
                    {
                        db.LoadProperty(organizacion, "TipoUsuario");
                        db.LoadProperty(organizacion, "Holding");
                        db.LoadProperty(organizacion, "Compania");
                        db.LoadProperty(organizacion, "Direccion");
                        db.LoadProperty(organizacion, "SubDireccion");
                        db.LoadProperty(organizacion, "Gerencia");
                        db.LoadProperty(organizacion, "SubGerencia");
                        db.LoadProperty(organizacion, "Jefatura");
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

        public void GuardarOrganizacion(Organizacion organizacion)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = true;

                //TODO: Cambiar habilitado por el embebido
                organizacion.Habilitado = true;
                organizacion.Sistema = false;
                List<AliasOrganizacion> alias = new BusinessParametros().ObtenerAliasOrganizacion(organizacion.IdTipoUsuario);
                if (organizacion.Holding != null)
                {
                    organizacion.Holding.Descripcion = organizacion.Holding.Descripcion.Trim();
                    organizacion.IdNivelOrganizacion = 1;
                    if (db.Holding.Any(a => a.Descripcion == organizacion.Holding.Descripcion && a.IdTipoUsuario == organizacion.Holding.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacion.IdTipoUsuario, 1).Descripcion));
                }

                if (organizacion.Compania != null)
                {
                    organizacion.Compania.Descripcion = organizacion.Compania.Descripcion.Trim();
                    organizacion.IdNivelOrganizacion = 2;
                    //if (db.Compañia.Any(a => a.Descripcion == organizacion.Compania.Descripcion && a.IdTipoUsuario == organizacion.Compania.IdTipoUsuario))
                    //throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacion.IdTipoUsuario, 2).Descripcion));
                    if (db.Organizacion.Where(w => w.IdHolding == organizacion.IdHolding).Any(a => a.Compania.Descripcion == organizacion.Compania.Descripcion && a.IdTipoUsuario == organizacion.Compania.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacion.IdTipoUsuario, 2).Descripcion));

                }

                if (organizacion.Direccion != null)
                {
                    organizacion.Direccion.Descripcion = organizacion.Direccion.Descripcion.Trim();
                    organizacion.IdNivelOrganizacion = 3;
                    if (db.Organizacion.Where(w => w.IdHolding == organizacion.IdHolding && w.IdCompania == organizacion.IdCompania).Any(a => a.Direccion.Descripcion == organizacion.Direccion.Descripcion && a.IdTipoUsuario == organizacion.Direccion.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacion.IdTipoUsuario, 3).Descripcion));
                }

                if (organizacion.SubDireccion != null)
                {
                    organizacion.SubDireccion.Descripcion = organizacion.SubDireccion.Descripcion.Trim();
                    organizacion.IdNivelOrganizacion = 4;
                    if (db.Organizacion.Where(w => w.IdHolding == organizacion.IdHolding && w.IdDireccion == organizacion.IdDireccion).Any(a => a.SubDireccion.Descripcion == organizacion.SubDireccion.Descripcion && a.IdTipoUsuario == organizacion.SubDireccion.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacion.IdTipoUsuario, 4).Descripcion));
                }

                if (organizacion.Gerencia != null)
                {
                    organizacion.Gerencia.Descripcion = organizacion.Gerencia.Descripcion.Trim();
                    organizacion.IdNivelOrganizacion = 5;
                    if (db.Organizacion.Where(w => w.IdHolding == organizacion.IdHolding && w.IdSubDireccion == organizacion.IdSubDireccion).Any(a => a.Gerencia.Descripcion == organizacion.Gerencia.Descripcion && a.IdTipoUsuario == organizacion.Gerencia.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacion.IdTipoUsuario, 5).Descripcion));
                }

                if (organizacion.SubGerencia != null)
                {
                    organizacion.SubGerencia.Descripcion = organizacion.SubGerencia.Descripcion.Trim();
                    organizacion.IdNivelOrganizacion = 6;

                    if (db.Organizacion.Where(w => w.IdHolding == organizacion.IdHolding && w.IdGerencia == organizacion.IdGerencia).Any(a => a.SubGerencia.Descripcion == organizacion.SubGerencia.Descripcion && a.IdTipoUsuario == organizacion.SubGerencia.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacion.IdTipoUsuario, 6).Descripcion));
                }

                if (organizacion.Jefatura != null)
                {
                    organizacion.Jefatura.Descripcion = organizacion.Jefatura.Descripcion.Trim();
                    organizacion.IdNivelOrganizacion = 7;
                    if (db.Organizacion.Where(w => w.IdHolding == organizacion.IdHolding && w.IdSubGerencia == organizacion.IdSubGerencia).Any(a => a.Jefatura.Descripcion == organizacion.Jefatura.Descripcion && a.IdTipoUsuario == organizacion.Jefatura.IdTipoUsuario))
                        throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacion.IdTipoUsuario, 7).Descripcion));
                }

                if (organizacion.Id == 0)
                    db.Organizacion.AddObject(organizacion);
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

        public void GuardarHolding(Holding entidad)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                entidad.Descripcion = entidad.Descripcion.Trim();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                entidad.Habilitado = true;
                if (entidad.Id == 0)
                    db.Holding.AddObject(entidad);
                else
                {
                    Holding tmpHolding = db.Holding.SingleOrDefault(s => s.Id == entidad.Id);
                    if (tmpHolding == null) return;
                    tmpHolding.Descripcion = entidad.Descripcion;
                    tmpHolding.Habilitado = entidad.Habilitado;
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

        public void GuardarCompania(Compania entidad)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                entidad.Descripcion = entidad.Descripcion.Trim();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                entidad.Habilitado = true;
                if (entidad.Id == 0)
                    db.Compañia.AddObject(entidad);
                else
                {
                    Compania tmpCompania = db.Compañia.SingleOrDefault(s => s.Id == entidad.Id);
                    if (tmpCompania == null) return;
                    tmpCompania.Descripcion = entidad.Descripcion;
                    tmpCompania.Habilitado = entidad.Habilitado;
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

        public void GuardarDireccion(Direccion entidad)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                entidad.Descripcion = entidad.Descripcion.Trim();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                entidad.Habilitado = true;
                if (entidad.Id == 0)
                    db.Direccion.AddObject(entidad);
                else
                {
                    Direccion tmpDireccion = db.Direccion.SingleOrDefault(s => s.Id == entidad.Id);
                    if (tmpDireccion == null) return;
                    tmpDireccion.Descripcion = entidad.Descripcion;
                    tmpDireccion.Habilitado = entidad.Habilitado;
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

        public void GuardarSubDireccion(SubDireccion entidad)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                entidad.Descripcion = entidad.Descripcion.Trim();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                entidad.Habilitado = true;
                if (entidad.Id == 0)
                    db.SubDireccion.AddObject(entidad);
                else
                {
                    SubDireccion tmpSubDireccion = db.SubDireccion.SingleOrDefault(s => s.Id == entidad.Id);
                    if (tmpSubDireccion == null) return;
                    tmpSubDireccion.Descripcion = entidad.Descripcion;
                    tmpSubDireccion.Habilitado = entidad.Habilitado;
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

        public void GuardarGerencia(Gerencia entidad)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                entidad.Descripcion = entidad.Descripcion.Trim();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                entidad.Habilitado = true;
                if (entidad.Id == 0)
                    db.Gerencia.AddObject(entidad);
                else
                {
                    Gerencia tmpGerencia = db.Gerencia.SingleOrDefault(s => s.Id == entidad.Id);
                    if (tmpGerencia == null) return;
                    tmpGerencia.Descripcion = entidad.Descripcion;
                    tmpGerencia.Habilitado = entidad.Habilitado;
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

        public void GuardarSubGerencia(SubGerencia entidad)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                entidad.Descripcion = entidad.Descripcion.Trim();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                entidad.Habilitado = true;
                if (entidad.Id == 0)
                    db.SubGerencia.AddObject(entidad);
                else
                {
                    SubGerencia tmpSubGerencia = db.SubGerencia.SingleOrDefault(s => s.Id == entidad.Id);
                    if (tmpSubGerencia == null) return;
                    tmpSubGerencia.Descripcion = entidad.Descripcion;
                    tmpSubGerencia.Habilitado = entidad.Habilitado;
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

        public void GuardarJefatura(Jefatura entidad)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                entidad.Descripcion = entidad.Descripcion.Trim();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                entidad.Habilitado = true;
                if (entidad.Id == 0)
                    db.Jefatura.AddObject(entidad);
                else
                {
                    Jefatura tmpJefatura = db.Jefatura.SingleOrDefault(s => s.Id == entidad.Id);
                    if (tmpJefatura == null) return;
                    tmpJefatura.Descripcion = entidad.Descripcion;
                    tmpJefatura.Habilitado = entidad.Habilitado;
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

        public Organizacion ObtenerOrganizacionUsuario(int idOrganizacion)
        {
            Organizacion result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Organizacion.SingleOrDefault(w => w.Id == idOrganizacion && w.Habilitado);
                if (result != null)
                {
                    db.LoadProperty(result, "Holding");
                    db.LoadProperty(result, "Compania");
                    db.LoadProperty(result, "Direccion");
                    db.LoadProperty(result, "SubDireccion");
                    db.LoadProperty(result, "Gerencia");
                    db.LoadProperty(result, "SubGerencia");
                    db.LoadProperty(result, "Jefatura");
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

        public List<Organizacion> ObtenerOrganizaciones(int? idTipoUsuario, int? idHolding, int? idCompania, int? idDireccion, int? idSubDireccion, int? idGerencia, int? idSubGerencia, int? idJefatura)
        {
            List<Organizacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Organizacion> qry = db.Organizacion.Where(w => !w.Sistema && w.TipoUsuario.Habilitado);
                if (idTipoUsuario.HasValue)
                    qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);

                if (idHolding.HasValue)
                    qry = qry.Where(w => w.IdHolding == idHolding);

                if (idCompania.HasValue)
                    qry = qry.Where(w => w.IdCompania == idCompania);

                if (idDireccion.HasValue)
                    qry = qry.Where(w => w.IdDireccion == idDireccion);

                if (idSubDireccion.HasValue)
                    qry = qry.Where(w => w.IdSubDireccion == idSubDireccion);

                if (idGerencia.HasValue)
                    qry = qry.Where(w => w.IdGerencia == idGerencia);

                if (idSubGerencia.HasValue)
                    qry = qry.Where(w => w.IdSubGerencia == idSubGerencia);

                if (idJefatura.HasValue)
                    qry = qry.Where(w => w.IdJefatura == idJefatura);

                qry = from q in qry
                      orderby q.Holding != null, q.Holding.Descripcion, q.Compania != null, q.Compania.Descripcion,
                          q.Direccion != null, q.Direccion.Descripcion, q.SubDireccion != null, q.SubDireccion.Descripcion,
                          q.Gerencia != null, q.Gerencia.Descripcion, q.SubGerencia != null, q.SubGerencia.Descripcion,
                          q.Jefatura != null, q.Compania.Descripcion ascending
                      select q;
                result = qry.ToList();
                foreach (Organizacion organizacion in result)
                {
                    db.LoadProperty(organizacion, "TipoUsuario");
                    db.LoadProperty(organizacion, "Holding");
                    db.LoadProperty(organizacion, "Compania");
                    db.LoadProperty(organizacion, "Direccion");
                    db.LoadProperty(organizacion, "SubDireccion");
                    db.LoadProperty(organizacion, "Gerencia");
                    db.LoadProperty(organizacion, "SubGerencia");
                    db.LoadProperty(organizacion, "Jefatura");
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

        public List<Organizacion> ObtenerOrganizacionesGrupos(List<int> grupos)
        {
            List<Organizacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Organizacion> qry = from o in db.Organizacion
                                               join gu in db.GrupoUsuario on o.IdTipoUsuario equals gu.IdTipoUsuario
                                               where grupos.Contains(gu.Id) && !o.Sistema && o.TipoUsuario.Habilitado
                                               select o;

                result = qry.Distinct().ToList();
                foreach (Organizacion organizacion in result)
                {
                    db.LoadProperty(organizacion, "Holding");
                    db.LoadProperty(organizacion, "Compania");
                    db.LoadProperty(organizacion, "Direccion");
                    db.LoadProperty(organizacion, "SubDireccion");
                    db.LoadProperty(organizacion, "Gerencia");
                    db.LoadProperty(organizacion, "SubGerencia");
                    db.LoadProperty(organizacion, "Jefatura");
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

        public string ObtenerDescripcionOrganizacionUsuario(int idUsuario, bool ultimoNivel)
        {
            string result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Usuario usuario = db.Usuario.SingleOrDefault(w => w.Id == idUsuario && w.Habilitado);
                if (usuario != null)
                {
                    if (usuario.Organizacion != null)
                    {
                        if (ultimoNivel)
                        {
                            if (usuario.Organizacion.Holding != null)
                                result = usuario.Organizacion.Holding.Descripcion;
                            if (usuario.Organizacion.Compania != null)
                                result = usuario.Organizacion.Compania.Descripcion;
                            if (usuario.Organizacion.Direccion != null)
                                result = usuario.Organizacion.Direccion.Descripcion;
                            if (usuario.Organizacion.SubDireccion != null)
                                result = usuario.Organizacion.SubDireccion.Descripcion;
                            if (usuario.Organizacion.Gerencia != null)
                                result = usuario.Organizacion.Gerencia.Descripcion;
                            if (usuario.Organizacion.SubGerencia != null)
                                result = usuario.Organizacion.SubGerencia.Descripcion;
                            if (usuario.Organizacion.Jefatura != null)
                                result = usuario.Organizacion.Jefatura.Descripcion;
                        }
                        else
                        {
                            if (usuario.Organizacion.Holding != null)
                                result += usuario.Organizacion.Holding.Descripcion;
                            if (usuario.Organizacion.Compania != null)
                                result += ">" + usuario.Organizacion.Compania.Descripcion;
                            if (usuario.Organizacion.Direccion != null)
                                result += ">" + usuario.Organizacion.Direccion.Descripcion;
                            if (usuario.Organizacion.SubDireccion != null)
                                result += ">" + usuario.Organizacion.SubDireccion.Descripcion;
                            if (usuario.Organizacion.Gerencia != null)
                                result += ">" + usuario.Organizacion.Gerencia.Descripcion;
                            if (usuario.Organizacion.SubGerencia != null)
                                result += ">" + usuario.Organizacion.SubGerencia.Descripcion;
                            if (usuario.Organizacion.Jefatura != null)
                                result += ">" + usuario.Organizacion.Jefatura.Descripcion;
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

        public string ObtenerDescripcionOrganizacionById(int idOrganizacion, bool ultimoNivel)
        {
            string result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Organizacion organizacion = db.Organizacion.SingleOrDefault(w => w.Id == idOrganizacion && w.Habilitado);
                if (organizacion != null)
                {
                    if (ultimoNivel)
                    {
                        if (organizacion.Holding != null)
                            result = organizacion.Holding.Descripcion;
                        if (organizacion.Compania != null)
                            result = organizacion.Compania.Descripcion;
                        if (organizacion.Direccion != null)
                            result = organizacion.Direccion.Descripcion;
                        if (organizacion.SubDireccion != null)
                            result = organizacion.SubDireccion.Descripcion;
                        if (organizacion.Gerencia != null)
                            result = organizacion.Gerencia.Descripcion;
                        if (organizacion.SubGerencia != null)
                            result = organizacion.SubGerencia.Descripcion;
                        if (organizacion.Jefatura != null)
                            result = organizacion.Jefatura.Descripcion;
                    }
                    else
                    {
                        if (organizacion.Holding != null)
                            result += organizacion.Holding.Descripcion;
                        if (organizacion.Compania != null)
                            result += ">" + organizacion.Compania.Descripcion;
                        if (organizacion.Direccion != null)
                            result += ">" + organizacion.Direccion.Descripcion;
                        if (organizacion.SubDireccion != null)
                            result += ">" + organizacion.SubDireccion.Descripcion;
                        if (organizacion.Gerencia != null)
                            result += ">" + organizacion.Gerencia.Descripcion;
                        if (organizacion.SubGerencia != null)
                            result += ">" + organizacion.SubGerencia.Descripcion;
                        if (organizacion.Jefatura != null)
                            result += ">" + organizacion.Jefatura.Descripcion;
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

        public List<int> ObtenerOrganizacionesByIdOrganizacion(int idOrganizacion)
        {
            List<int> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Organizacion tmpOrganizacion = db.Organizacion.SingleOrDefault(s => s.Id == idOrganizacion);
                IQueryable<Organizacion> qry = db.Organizacion.Where(w => w.Id != idOrganizacion);
                if (tmpOrganizacion != null)
                {
                    if (tmpOrganizacion.IdJefatura.HasValue)
                        qry = qry.Where(w => w.IdJefatura == tmpOrganizacion.IdJefatura);
                    else if (tmpOrganizacion.IdSubGerencia.HasValue)
                        qry = qry.Where(w => w.IdSubGerencia == tmpOrganizacion.IdSubGerencia);
                    else if (tmpOrganizacion.IdGerencia.HasValue)
                        qry = qry.Where(w => w.IdGerencia == tmpOrganizacion.IdGerencia);
                    else if (tmpOrganizacion.IdSubDireccion.HasValue)
                        qry = qry.Where(w => w.IdSubDireccion == tmpOrganizacion.IdSubDireccion);
                    else if (tmpOrganizacion.IdDireccion.HasValue)
                        qry = qry.Where(w => w.IdDireccion == tmpOrganizacion.IdDireccion);
                    else if (tmpOrganizacion.IdCompania.HasValue)
                        qry = qry.Where(w => w.IdCompania == tmpOrganizacion.IdCompania);
                    else
                        qry = qry.Where(w => w.IdHolding == tmpOrganizacion.IdHolding);
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

        public void HabilitarOrganizacion(int idOrganizacion, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                db.ContextOptions.ProxyCreationEnabled = true;
                Organizacion org = db.Organizacion.SingleOrDefault(w => w.Id == idOrganizacion);
                if (org != null)
                {
                    //if (org.HitConsulta.Any() || org.Ticket.Any() || org.Usuario.Any() && !habilitado)
                    //    throw new Exception("La ubicacion ya se encuetra con datos asociasdos no puede ser eliminada");
                    org.Habilitado = habilitado;


                    var qry = db.Organizacion.Where(w => w.IdTipoUsuario == org.IdTipoUsuario && w.IdHolding == org.IdHolding);
                    if (!habilitado)
                    {
                        qry = qry.Where(w => w.IdNivelOrganizacion >= org.IdNivelOrganizacion && w.Habilitado);
                        foreach (Organizacion source in qry.OrderBy(o => o.Id))
                        {
                            //if (source.HitConsulta.Any() || source.Ticket.Any() || source.Usuario.Any() && !habilitado)
                            //    throw new Exception(
                            //        "La ubicacion ya se encuetra con datos asociasdos no puede ser eliminada");
                            switch (org.IdNivelOrganizacion)
                            {
                                case 1:
                                    if (source.IdHolding == org.IdHolding)
                                    {
                                        source.Holding.Habilitado = false;
                                        if (source.IdCompania.HasValue)
                                            source.Compania.Habilitado = false;
                                        if (source.IdDireccion.HasValue)
                                            source.Direccion.Habilitado = false;
                                        if (source.IdSubDireccion.HasValue)
                                            source.SubDireccion.Habilitado = false;
                                        if (source.IdGerencia.HasValue)
                                            source.Gerencia.Habilitado = false;
                                        if (source.IdSubGerencia.HasValue)
                                            source.SubGerencia.Habilitado = false;
                                        if (source.IdJefatura.HasValue)
                                            source.Jefatura.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 2:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania)
                                    {
                                        source.Compania.Habilitado = false;
                                        if (source.IdDireccion.HasValue)
                                            source.Direccion.Habilitado = false;
                                        if (source.IdSubDireccion.HasValue)
                                            source.SubDireccion.Habilitado = false;
                                        if (source.IdGerencia.HasValue)
                                            source.Gerencia.Habilitado = false;
                                        if (source.IdSubGerencia.HasValue)
                                            source.SubGerencia.Habilitado = false;
                                        if (source.IdJefatura.HasValue)
                                            source.Jefatura.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 3:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania && source.IdDireccion == org.IdDireccion)
                                    {
                                        source.Direccion.Habilitado = false;
                                        if (source.IdSubDireccion.HasValue)
                                            source.SubDireccion.Habilitado = false;
                                        if (source.IdGerencia.HasValue)
                                            source.Gerencia.Habilitado = false;
                                        if (source.IdSubGerencia.HasValue)
                                            source.SubGerencia.Habilitado = false;
                                        if (source.IdJefatura.HasValue)
                                            source.Jefatura.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 4:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania && source.IdDireccion == org.IdDireccion && source.IdSubDireccion == org.IdSubDireccion)
                                    {
                                        source.SubDireccion.Habilitado = false;
                                        if (source.IdGerencia.HasValue)
                                            source.Gerencia.Habilitado = false;
                                        if (source.IdSubGerencia.HasValue)
                                            source.SubGerencia.Habilitado = false;
                                        if (source.IdJefatura.HasValue)
                                            source.Jefatura.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 5:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania && source.IdDireccion == org.IdDireccion && source.IdSubDireccion == org.IdSubDireccion && source.IdGerencia == org.IdGerencia)
                                    {
                                        source.Gerencia.Habilitado = false;
                                        if (source.IdSubGerencia.HasValue)
                                            source.SubGerencia.Habilitado = false;
                                        if (source.IdJefatura.HasValue)
                                            source.Jefatura.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 6:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania && source.IdDireccion == org.IdDireccion && source.IdSubDireccion == org.IdSubDireccion && source.IdGerencia == org.IdGerencia && source.IdSubGerencia == org.IdSubGerencia)
                                    {
                                        source.SubGerencia.Habilitado = false;
                                        if (source.IdJefatura.HasValue)
                                            source.Jefatura.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                                case 7:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania && source.IdDireccion == org.IdDireccion && source.IdSubDireccion == org.IdSubDireccion && source.IdGerencia == org.IdGerencia && source.IdSubGerencia == org.IdSubGerencia && source.IdJefatura == org.IdJefatura)
                                    {
                                        source.Jefatura.Habilitado = false;
                                        source.Habilitado = false;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        qry = qry.Where(w => w.IdNivelOrganizacion <= org.IdNivelOrganizacion && !w.Habilitado);
                        //if (org.HitConsulta.Any() || org.Ticket.Any() || org.Usuario.Any() && !habilitado)
                        //    throw new Exception("La ubicacion ya se encuetra con datos asociasdos no puede ser eliminada");
                        switch (org.IdNivelOrganizacion)
                        {
                            case 1:
                                org.Holding.Habilitado = true;
                                break;
                            case 2:
                                org.Holding.Habilitado = true;
                                org.Compania.Habilitado = true;
                                break;
                            case 3:
                                org.Holding.Habilitado = true;
                                org.Compania.Habilitado = true;
                                org.Direccion.Habilitado = true;
                                break;
                            case 4:
                                org.Holding.Habilitado = true;
                                org.Compania.Habilitado = true;
                                org.Direccion.Habilitado = true;
                                org.SubDireccion.Habilitado = true;
                                break;
                            case 5:
                                org.Holding.Habilitado = true;
                                org.Compania.Habilitado = true;
                                org.Direccion.Habilitado = true;
                                org.SubDireccion.Habilitado = true;
                                org.Gerencia.Habilitado = true;
                                break;
                            case 6:
                                org.Holding.Habilitado = true;
                                org.Compania.Habilitado = true;
                                org.Direccion.Habilitado = true;
                                org.SubDireccion.Habilitado = true;
                                org.Gerencia.Habilitado = true;
                                org.SubGerencia.Habilitado = true;
                                break;
                            case 7:
                                org.Holding.Habilitado = true;
                                org.Compania.Habilitado = true;
                                org.Direccion.Habilitado = true;
                                org.SubDireccion.Habilitado = true;
                                org.Gerencia.Habilitado = true;
                                org.SubGerencia.Habilitado = true;
                                org.Jefatura.Habilitado = true;
                                break;
                        }

                        if (org.IdCompania.HasValue)
                            qry = qry.Where(w => w.IdCompania == org.IdCompania);
                        foreach (Organizacion source in qry.OrderBy(o => o.Id))
                        {
                            //if (source.HitConsulta.Any() || source.Ticket.Any() || source.Usuario.Any())
                            //throw new Exception(
                            //    "La ubicacion ya se encuetra con datos asociasdos no puede ser eliminada");
                            switch (source.IdNivelOrganizacion)
                            {
                                case 1:
                                    break;
                                case 2:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania)
                                    {
                                        source.Compania.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 3:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania &&
                                        source.IdDireccion == org.IdDireccion)
                                    {
                                        source.Compania.Habilitado = true;
                                        source.Direccion.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 4:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania &&
                                        source.IdDireccion == org.IdDireccion &&
                                        source.IdSubDireccion == org.IdSubDireccion)
                                    {
                                        source.Compania.Habilitado = true;
                                        source.Direccion.Habilitado = true;
                                        source.SubDireccion.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 5:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania &&
                                        source.IdDireccion == org.IdDireccion &&
                                        source.IdSubDireccion == org.IdSubDireccion &&
                                        source.IdGerencia == org.IdGerencia)
                                    {
                                        source.Compania.Habilitado = true;
                                        source.Direccion.Habilitado = true;
                                        source.SubDireccion.Habilitado = true;
                                        source.Gerencia.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 6:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania &&
                                        source.IdDireccion == org.IdDireccion &&
                                        source.IdSubDireccion == org.IdSubDireccion &&
                                        source.IdGerencia == org.IdGerencia && source.IdSubGerencia == org.IdSubGerencia)
                                    {
                                        source.Compania.Habilitado = true;
                                        source.Direccion.Habilitado = true;
                                        source.SubDireccion.Habilitado = true;
                                        source.Gerencia.Habilitado = true;
                                        source.SubGerencia.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                                case 7:
                                    if (source.IdHolding == org.IdHolding && source.IdCompania == org.IdCompania &&
                                        source.IdDireccion == org.IdDireccion &&
                                        source.IdSubDireccion == org.IdSubDireccion &&
                                        source.IdGerencia == org.IdGerencia && source.IdSubGerencia == org.IdSubGerencia &&
                                        source.IdJefatura == org.IdJefatura)
                                    {
                                        source.Compania.Habilitado = true;
                                        source.Direccion.Habilitado = true;
                                        source.SubDireccion.Habilitado = true;
                                        source.Gerencia.Habilitado = true;
                                        source.SubGerencia.Habilitado = true;
                                        source.Jefatura.Habilitado = true;
                                        source.Habilitado = true;
                                    }
                                    break;
                            }
                        }
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

        public Organizacion ObtenerOrganizacionById(int idOrganizacion)
        {
            Organizacion result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Organizacion.SingleOrDefault(w => w.Id == idOrganizacion);
                if (result != null)
                {
                    db.LoadProperty(result, "TipoUsuario");
                    db.LoadProperty(result, "Holding");
                    db.LoadProperty(result, "Compania");
                    db.LoadProperty(result, "Direccion");
                    db.LoadProperty(result, "SubDireccion");
                    db.LoadProperty(result, "Gerencia");
                    db.LoadProperty(result, "SubGerencia");
                    db.LoadProperty(result, "Jefatura");
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

        public void ActualizarOrganizacion(Organizacion org)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;

                Organizacion organizacionBase = db.Organizacion.SingleOrDefault(s => s.Id == org.Id);

                if (organizacionBase != null)
                {
                    if (organizacionBase.Holding != null)
                    {
                        if (db.Holding.Any(a => a.Descripcion == org.Holding.Descripcion && a.IdTipoUsuario == org.Holding.IdTipoUsuario && a.Id != org.Holding.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacionBase.IdTipoUsuario, 1).Descripcion));
                        organizacionBase.Holding.Descripcion = org.Holding.Descripcion.Trim();
                    }

                    if (organizacionBase.Compania != null)
                    {
                        if (db.Organizacion.Where(w => w.IdHolding == org.IdHolding).Any(a => a.Compania.Descripcion == org.Compania.Descripcion && a.IdTipoUsuario == org.Compania.IdTipoUsuario && a.Compania.Id != org.Compania.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacionBase.IdTipoUsuario, 2).Descripcion));
                        organizacionBase.Compania.Descripcion = org.Compania.Descripcion.Trim();
                    }

                    if (organizacionBase.Direccion != null)
                    {
                        if (db.Organizacion.Where(w => w.IdHolding == org.IdHolding && w.IdCompania == org.IdCompania).Any(a => a.Direccion.Descripcion == org.Direccion.Descripcion && a.IdTipoUsuario == org.Direccion.IdTipoUsuario && a.Direccion.Id != organizacionBase.Direccion.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacionBase.IdTipoUsuario, 3).Descripcion));
                        organizacionBase.Direccion.Descripcion = org.Direccion.Descripcion.Trim();
                    }

                    if (organizacionBase.SubDireccion != null)
                    {
                        if (db.Organizacion.Where(w => w.IdHolding == org.IdHolding && w.IdDireccion == org.IdDireccion).Any(a => a.SubDireccion.Descripcion == org.SubDireccion.Descripcion && a.IdTipoUsuario == org.SubDireccion.IdTipoUsuario && a.SubDireccion.Id != organizacionBase.SubDireccion.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacionBase.IdTipoUsuario, 4).Descripcion));
                        organizacionBase.SubDireccion.Descripcion = org.SubDireccion.Descripcion.Trim();
                    }

                    if (organizacionBase.Gerencia != null)
                    {
                        if (db.Organizacion.Where(w => w.IdHolding == org.IdHolding && w.IdSubDireccion == org.IdSubDireccion).Any(a => a.Gerencia.Descripcion == org.Gerencia.Descripcion && a.IdTipoUsuario == org.Gerencia.IdTipoUsuario && a.Gerencia.Id != organizacionBase.Gerencia.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacionBase.IdTipoUsuario, 5).Descripcion));
                        organizacionBase.Gerencia.Descripcion = org.Gerencia.Descripcion.Trim();
                    }

                    if (organizacionBase.SubGerencia != null)
                    {
                        if (db.Organizacion.Where(w => w.IdHolding == org.IdHolding && w.IdGerencia == org.IdGerencia).Any(a => a.SubGerencia.Descripcion == org.SubGerencia.Descripcion && a.IdTipoUsuario == org.SubGerencia.IdTipoUsuario && a.SubGerencia.Id != organizacionBase.SubGerencia.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacionBase.IdTipoUsuario, 6).Descripcion));
                        organizacionBase.SubGerencia.Descripcion = org.SubGerencia.Descripcion.Trim();
                    }

                    if (organizacionBase.Jefatura != null)
                    {
                        if (db.Organizacion.Where(w => w.IdHolding == org.IdHolding && w.IdSubGerencia == org.IdSubGerencia).Any(a => a.Jefatura.Descripcion == org.Jefatura.Descripcion && a.IdTipoUsuario == org.Jefatura.IdTipoUsuario && a.Jefatura.Id != organizacionBase.Jefatura.Id))
                            throw new Exception(string.Format("Este {0} se encuetra registrado", new BusinessParametros().ObtenerAliasOrganizacionNivel(organizacionBase.IdTipoUsuario, 7).Descripcion));
                        organizacionBase.Jefatura.Descripcion = org.Jefatura.Descripcion.Trim();
                    }

                    if (organizacionBase.Id == 0)
                        db.Organizacion.AddObject(organizacionBase);

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

        public Organizacion ObtenerOrganizacionDefault(int idTipoUsuario)
        {
            Organizacion result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                ParametrosUsuario parameters = db.ParametrosUsuario.SingleOrDefault(s => s.IdTipoUsuario == idTipoUsuario);
                if (parameters != null)
                {
                    result = db.Organizacion.SingleOrDefault(w => w.Id == parameters.IdOrganizacion);
                    if (result != null)
                    {
                        db.LoadProperty(result, "TipoUsuario");
                        db.LoadProperty(result, "Holding");
                        db.LoadProperty(result, "Compania");
                        db.LoadProperty(result, "Direccion");
                        db.LoadProperty(result, "SubDireccion");
                        db.LoadProperty(result, "Gerencia");
                        db.LoadProperty(result, "SubGerencia");
                        db.LoadProperty(result, "Jefatura");
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
