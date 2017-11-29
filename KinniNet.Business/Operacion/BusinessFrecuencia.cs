using System;
using System.Collections.Generic;
using System.Linq;
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
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso)
                    });
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
                                   where f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion
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
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso)
                    });
                }

                //List<Frecuencia> frecuencias = db.Frecuencia.Where(w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion).OrderByDescending(o => o.NumeroVisitas).Take(10).ToList();
                //foreach (var frecuencia in frecuencias.Select())
                //{

                //}
                //result = frecuencias.Select(frecuencia => new HelperFrecuencia
                //{
                //    IdArbol = frecuencia.IdArbolAcceso,
                //    DescripcionOpcion = bArbol.ObtenerTipificacion(frecuencia.IdArbolAcceso)
                //}).ToList();
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
                                   where f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio
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
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso)
                    });
                }
                //List<Frecuencia> frecuencias = db.Frecuencia.Where(w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio).OrderByDescending(o => o.NumeroVisitas).Take(10).ToList();
                //result = frecuencias.Select(frecuencia => new HelperFrecuencia
                //{
                //    IdArbol = frecuencia.IdArbolAcceso,
                //    DescripcionOpcion = bArbol.ObtenerTipificacion(frecuencia.IdArbolAcceso)
                //}).ToList();
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
                                   where f.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas
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
                    result.Add(new HelperFrecuencia
                    {
                        IdArbol = type.IdArbolAcceso,
                        DescripcionOpcion = bArbol.ObtenerTipificacion(type.IdArbolAcceso)
                    });
                }
                //List<Frecuencia> frecuencias = db.Frecuencia.Where(w => w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas).OrderByDescending(o => o.NumeroVisitas).Take(10).ToList();
                //result = frecuencias.Select(frecuencia => new HelperFrecuencia
                //{
                //    IdArbol = frecuencia.IdArbolAcceso,
                //    DescripcionOpcion = bArbol.ObtenerTipificacion(frecuencia.IdArbolAcceso)
                //}).ToList();
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
