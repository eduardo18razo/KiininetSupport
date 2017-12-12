using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;


namespace KinniNet.Core.Operacion
{
    public class BusinessInformacionConsulta : IDisposable
    {
        private readonly bool _proxy;

        public void Dispose()
        {

        }

        public BusinessInformacionConsulta(bool proxy = false)
        {
            _proxy = proxy;
        }

        public List<InformacionConsulta> ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta tipoinfoConsulta, bool insertarSeleccion)
        {
            List<InformacionConsulta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result =
                    db.InformacionConsulta.Where(w => w.IdTipoInfConsulta == (int)tipoinfoConsulta && w.Habilitado)
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new InformacionConsulta
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

        private int ObtenerTipoDocumento(string s)
        {
            int result = 0;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                string extension = s.Substring(s.Length - 3);
                result = db.TipoDocumento.First(f => f.Extension.Contains(extension)).Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return result;
        }
        public InformacionConsulta GuardarInformacionConsulta(InformacionConsulta informacion, List<string> documentosDescarga)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                if (db.InformacionConsulta.Any(a => a.Descripcion == informacion.Descripcion))
                    throw new Exception("Este Artículo ya existe.");
                informacion.Descripcion = informacion.Descripcion.Trim();
                informacion.Habilitado = true;
                informacion.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                if (informacion.Id == 0)
                    db.InformacionConsulta.AddObject(informacion);
                db.SaveChanges();
                informacion = db.InformacionConsulta.Single(s => s.Id == informacion.Id);
                foreach (string s in documentosDescarga)
                {
                    informacion.InformacionConsultaDocumentos.Add(new InformacionConsultaDocumentos
                    {
                        IdTipoDocumento = ObtenerTipoDocumento(s),
                        Archivo = informacion.Id + s,
                        FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                        IdUsuarioAlta = informacion.IdUsuarioAlta,
                    });
                }
                db.SaveChanges();

                BusinessFile.MoverTemporales(BusinessVariables.Directorios.RepositorioTemporalInformacionConsulta, BusinessVariables.Directorios.RepositorioInformacionConsulta, documentosDescarga);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return informacion;
        }

        public InformacionConsulta ActualizarInformacionConsulta(int idInformacionConsulta, InformacionConsulta informacion, List<string> documentosDescarga)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                if (db.InformacionConsulta.Any(a => a.Descripcion == informacion.Descripcion && a.Id != idInformacionConsulta))
                    throw new Exception("Este Artículo ya existe.");
                db.ContextOptions.LazyLoadingEnabled = true;
                InformacionConsulta info = db.InformacionConsulta.SingleOrDefault(s => s.Id == idInformacionConsulta);
                if (info == null) return null;
                info.Descripcion = informacion.Descripcion.Trim();
                info.IdUsuarioModifico = informacion.IdUsuarioAlta;
                info.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                info.Habilitado = true;
                info.InformacionConsultaDatos.First().Datos = informacion.InformacionConsultaDatos.First().Datos;
                info.InformacionConsultaDatos.First().Busqueda = informacion.InformacionConsultaDatos.First().Busqueda;
                info.InformacionConsultaDatos.First().Tags = informacion.InformacionConsultaDatos.First().Tags;
                info.InformacionConsultaDatos.First().Habilitado = informacion.InformacionConsultaDatos.First().Habilitado;

                List<InformacionConsultaDocumentos> documentosEliminar = new List<InformacionConsultaDocumentos>();
                List<string> archivosNuevos = new List<string>();
                foreach (InformacionConsultaDocumentos doctoExist in info.InformacionConsultaDocumentos)
                {
                    if (!documentosDescarga.Any(a => a.Contains(doctoExist.Archivo) || (info.Id + a).Contains(doctoExist.Archivo)))
                    {
                        documentosEliminar.Add(doctoExist);
                    }
                }

                foreach (string doctoNuevo in documentosDescarga)
                {
                    if (!info.InformacionConsultaDocumentos.Any(a => a.Archivo == doctoNuevo))
                    {
                        info.InformacionConsultaDocumentos.Add(new InformacionConsultaDocumentos
                        {
                            Archivo = info.Id + doctoNuevo,
                            FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            IdTipoDocumento = ObtenerTipoDocumento(doctoNuevo),
                            IdUsuarioAlta = informacion.IdUsuarioAlta
                        });
                        archivosNuevos.Add(doctoNuevo);
                    }
                }

                foreach (InformacionConsultaDocumentos documentos in documentosEliminar)
                {
                    db.InformacionConsultaDocumentos.DeleteObject(documentos);
                }
                db.SaveChanges();
                foreach (InformacionConsultaDocumentos documentos in documentosEliminar)
                {
                    BusinessFile.EliminarArchivo(BusinessVariables.Directorios.RepositorioTemporalInformacionConsulta, documentos.Archivo);
                    BusinessFile.EliminarArchivo(BusinessVariables.Directorios.RepositorioInformacionConsulta, documentos.Archivo);
                }

                BusinessFile.MoverTemporales(BusinessVariables.Directorios.RepositorioTemporalInformacionConsulta, BusinessVariables.Directorios.RepositorioInformacionConsulta, documentosDescarga);
                BusinessFile.RenombrarArchivosConsulta(archivosNuevos, info.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return informacion;
        }

        public List<InformacionConsulta> ObtenerInformacionConsultaArbol(int idArbol)
        {
            List<InformacionConsulta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.InventarioArbolAcceso.Join(db.InventarioInfConsulta, iaa => iaa.Id, iic => iic.IdInventario,
                    (iaa, iic) => new { iaa, iic })
                    .Join(db.InformacionConsulta, @t => @t.iic.IdInfConsulta, ic => ic.Id, (@t, ic) => new { @t, ic })
                    .Where(@t => @t.@t.iaa.IdArbolAcceso == idArbol)
                    .Select(@t => @t.ic).ToList();
                foreach (InformacionConsulta informacionConsulta in result)
                {
                    db.LoadProperty(informacionConsulta, "TipoInfConsulta");
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

        public InformacionConsulta ObtenerInformacionConsultaById(int idInformacion)
        {
            InformacionConsulta result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.InformacionConsulta.SingleOrDefault(w => w.Id == idInformacion);
                if (result != null)
                {
                    db.LoadProperty(result, "InformacionConsultaDatos");
                    db.LoadProperty(result, "InformacionConsultaDocumentos");
                    db.LoadProperty(result, "InformacionConsultaRate");
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

        public List<InformacionConsulta> ObtenerInformacionConsulta(string descripcion)
        {
            List<InformacionConsulta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<InformacionConsulta> qry = db.InformacionConsulta;
                descripcion = descripcion.Trim().ToLower();
                qry = qry.Where(w => w.Descripcion.ToLower().Contains(descripcion));
                result = qry.ToList();
                foreach (InformacionConsulta consulta in result)
                {
                    db.LoadProperty(consulta, "TipoInfConsulta");
                    db.LoadProperty(consulta, "UsuarioAlta");

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

        public void GuardarHit(int idArbol, int idTipoUsuario, int? idUsuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                new BusinessArbolAcceso().HitArbolAcceso(idArbol);
                ArbolAcceso arbol = new BusinessArbolAcceso().ObtenerArbolAcceso(idArbol);
                HitConsulta hit = new HitConsulta
                {
                    IdTipoArbolAcceso = arbol.IdTipoArbolAcceso,
                    IdArbolAcceso = idArbol,
                    IdTipoUsuario = idTipoUsuario,
                    IdUsuario = idUsuario.HasValue ? idUsuario : null,
                    IdUbicacion = idUsuario.HasValue ? new BusinessUbicacion().ObtenerUbicacionUsuario(new BusinessUsuarios().ObtenerUsuario(int.Parse(idUsuario.ToString())).IdUbicacion).Id : (int?) null,
                    IdOrganizacion = idUsuario.HasValue ? new BusinessOrganizacion().ObtenerOrganizacionUsuario(new BusinessUsuarios().ObtenerUsuario(int.Parse(idUsuario.ToString())).IdOrganizacion).Id : (int?)null,
                    HitGrupoUsuario = new List<HitGrupoUsuario>(),
                    FechaHoraAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture)
                };
                foreach (GrupoUsuarioInventarioArbol guia in new BusinessArbolAcceso().ObtenerGruposUsuarioArbol(idArbol))
                {
                    hit.HitGrupoUsuario.Add(new HitGrupoUsuario
                    {
                        IdRol = guia.IdRol,
                        IdGrupoUsuario = guia.IdGrupoUsuario,
                        IdSubGrupoUsuario = guia.IdSubGrupoUsuario
                    });
                }

                DateTime fechaTicket = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Frecuencia frecuencia = db.Frecuencia.SingleOrDefault(s => s.IdTipoUsuario == arbol.IdTipoUsuario && s.IdTipoArbolAcceso == arbol.IdTipoArbolAcceso && s.IdArbolAcceso == arbol.Id && s.Fecha == fechaTicket);
                if (frecuencia == null)
                {
                    frecuencia = new Frecuencia
                    {
                        IdTipoUsuario = arbol.IdTipoUsuario,
                        IdTipoArbolAcceso = arbol.IdTipoArbolAcceso,
                        IdArbolAcceso = arbol.Id,
                        NumeroVisitas = 1,
                        Fecha = fechaTicket,
                        UltimaVisita = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture)
                    };
                    db.Frecuencia.AddObject(frecuencia);
                }
                else
                {
                    frecuencia.NumeroVisitas++;
                    frecuencia.UltimaVisita = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                }

                if (hit.Id == 0)
                    db.HitConsulta.AddObject(hit);
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

        public void RateConsulta(int idConsulta, int idUsuario, bool meGusta)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                InformacionConsultaRate rate = db.InformacionConsultaRate.SingleOrDefault(s => s.IdInformacionConsulta == idConsulta && s.IdUsuario == idUsuario);
                if (rate == null)
                {
                    rate = new InformacionConsultaRate
                    {
                        IdInformacionConsulta = idConsulta,
                        IdUsuario = idUsuario,
                        MeGusta = meGusta
                    };
                    rate.NoMeGusta = !rate.MeGusta;
                    rate.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    db.InformacionConsultaRate.AddObject(rate);
                }
                else
                {
                    rate.MeGusta = meGusta;
                    rate.NoMeGusta = !rate.MeGusta;
                    rate.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
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

        public void HabilitarInformacion(int idInformacion, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                InformacionConsulta inf = db.InformacionConsulta.SingleOrDefault(w => w.Id == idInformacion);
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

    }
}
