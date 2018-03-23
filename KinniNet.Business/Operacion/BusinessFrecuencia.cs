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

        public List<HelperFrecuencia> ObtenerTopTenGeneral(int idTipoUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                var frecuencias = (from f in db.Frecuencia
                                   where f.IdTipoUsuario == idTipoUsuario && !f.ArbolAcceso.Sistema
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
        public List<HelperFrecuencia> ObtenerTopTenConsulta(int idTipoUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                var frecuencias = (from f in db.Frecuencia
                                   where f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion && f.IdTipoUsuario == idTipoUsuario && !f.ArbolAcceso.Sistema
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
        public List<HelperFrecuencia> ObtenerTopTenServicio(int idTipoUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                var frecuencias = (from f in db.Frecuencia
                                   where f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio && f.IdTipoUsuario == idTipoUsuario && !f.ArbolAcceso.Sistema
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
                    List<ArbolAcceso> opciones = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio 
                        && w.EsTerminal && !w.Sistema && w.Habilitado && w.IdTipoUsuario == idTipoUsuario && !arbolesAgregados.Contains(w.Id)).OrderByDescending(d => d.FechaAlta).Take(take).ToList();
                    foreach (ArbolAcceso opcion in opciones)
                    {
                        ArbolAcceso arbol = bArbol.ObtenerArbolAcceso(opcion.Id);
                        result.Add(new HelperFrecuencia
                        {
                            IdArbol = arbol.Id,
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
        public List<HelperFrecuencia> ObtenerTopTenIncidente(int idTipoUsuario)
        {
            List<HelperFrecuencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                BusinessArbolAcceso bArbol = new BusinessArbolAcceso();
                var frecuencias = (from f in db.Frecuencia
                                   where f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas && f.IdTipoUsuario == idTipoUsuario && !f.ArbolAcceso.Sistema
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


    }
}
