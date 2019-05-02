using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Reportes;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;


namespace KinniNet.Core.Operacion
{
    public class BusinessInformacionConsulta : IDisposable
    {
        CultureInfo cultureInfo = new CultureInfo("es-MX");
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
            int result;
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
                    //if (!documentosDescarga.Any(a => a.Contains(doctoExist.Archivo) || (info.Id + a).Contains(doctoExist.Archivo)))
                    //{
                    documentosEliminar.Add(doctoExist);
                    //}
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
                    else
                    {
                        documentosEliminar.RemoveAll(r => r.Archivo == doctoNuevo);
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

        public List<HelperInformacionConsulta> ObtenerInformacionReporte(string descripcion, Dictionary<string, DateTime> fechas)
        {
            List<HelperInformacionConsulta> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<InformacionConsulta> qry = db.InformacionConsulta;
                descripcion = descripcion.Trim().ToLower();
                qry = qry.Where(w => w.Descripcion.ToLower().Contains(descripcion));
                List<InformacionConsulta> info = qry.ToList();

                if (qry.Any())
                {
                    List<InformacionConsultaRate> qryLike = db.InformacionConsultaRate.ToList();

                    if (fechas != null)
                    {
                        var qryJoin = from q in qry
                                      join icr in db.InformacionConsultaRate on q.Id equals icr.IdInformacionConsulta
                                      select new { q, icr };
                        DateTime fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                        DateTime fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                        var infoJoin = qryJoin.ToList();
                        infoJoin = infoJoin.Where(w => DateTime.ParseExact(w.icr.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(fechaInicio.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.icr.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(fechaFin.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)).ToList();
                        info = infoJoin.Select(s => s.q).Distinct().ToList();

                        qryLike = qryLike.Where(w => DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(fechaInicio.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(fechaFin.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)).Distinct().ToList();
                    }

                    result = new List<HelperInformacionConsulta>();
                    foreach (InformacionConsulta consulta in info)
                    {
                        db.LoadProperty(consulta, "TipoInfConsulta");
                        db.LoadProperty(consulta, "UsuarioAlta");

                        result.Add(new HelperInformacionConsulta
                        {
                            Id = consulta.Id,
                            Titulo = consulta.Descripcion,
                            Autor = consulta.UsuarioAlta.NombreCompleto,
                            Creacion = consulta.FechaAlta,
                            UltEdicion = consulta.FechaModificacion,
                            MeGusta = qryLike.Count(c => c.MeGusta && c.IdInformacionConsulta == consulta.Id),
                            NoMeGusta = qryLike.Count(c => c.NoMeGusta && c.IdInformacionConsulta == consulta.Id),
                            Habilitado = consulta.Habilitado
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
                    IdUbicacion = idUsuario.HasValue ? new BusinessUbicacion().ObtenerUbicacionUsuario(new BusinessUsuarios().ObtenerUsuario(int.Parse(idUsuario.ToString())).IdUbicacion).Id : (int?)null,
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

        public void RateConsulta(int idArbol, int idConsulta, int idUsuario, bool meGusta)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                InformacionConsultaRate rate = db.InformacionConsultaRate.SingleOrDefault(s => s.IdInformacionConsulta == idConsulta && s.IdUsuario == idUsuario);
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(s => s.Id == idArbol);
                if (arbol != null)
                {
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
                        if (meGusta)
                            arbol.MeGusta++;
                        else
                            arbol.MeGusta++;
                    }
                    else
                    {
                        if (meGusta != rate.MeGusta)
                        {
                            rate.MeGusta = meGusta;
                            rate.NoMeGusta = !rate.MeGusta;
                            rate.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                            if (meGusta)
                            {
                                arbol.MeGusta++;
                                arbol.NoMeGusta--;
                            }
                            else
                            {
                                arbol.MeGusta--;
                                arbol.NoMeGusta++;
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

        public ReporteInformacionConsulta ObtenerReporteInformacionConsulta(int idInformacionConsulta, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            ReporteInformacionConsulta result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            int conteo = 1;
            try
            {
                bool restaMes = false;
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                result = new ReporteInformacionConsulta { IdInformacionConsulta = idInformacionConsulta };
                string titulo = db.InformacionConsulta.Single(s => s.Id == idInformacionConsulta).Descripcion;

                string rango = string.Empty;
                DateTime fechaInicio;
                DateTime fechaFin;

                var qry = from icr in db.InformacionConsultaRate
                          join iic in db.InventarioInfConsulta on icr.IdInformacionConsulta equals iic.IdInfConsulta
                          join iaa in db.InventarioArbolAcceso on iic.IdInventario equals iaa.Id
                          join aa in db.ArbolAcceso on iaa.IdArbolAcceso equals aa.Id
                          where icr.IdInformacionConsulta == idInformacionConsulta
                          && aa.Evaluacion
                          select icr;

                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));
                dtBarras.Columns.Add("Color", typeof(string));

                dtBarras.Rows.Add("Like", ConfigurationManager.AppSettings["ColorLike"]);
                dtBarras.Rows.Add("Dont Like", ConfigurationManager.AppSettings["ColorDontLike"]);

                if (fechas != null)
                {
                    restaMes = true;
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    DateTime tmpFecha = (DateTime)fechaInicio;
                    bool continua = true;
                    while (continua)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                if (tmpFecha < fechaFin)
                                {
                                    dtBarras.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy"), typeof(int)));
                                    tmpFecha = tmpFecha.AddDays(1);
                                }
                                else
                                    continua = false;
                                rango = "Diario";
                                break;
                            case 2:
                                if (tmpFecha < fechaFin)
                                {
                                    if (!dtBarras.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                        dtBarras.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy"), typeof(int)));
                                    tmpFecha = tmpFecha.AddDays(7);
                                }
                                else
                                    continua = false;
                                rango = "Semanal";
                                break;
                            case 3:
                                if (tmpFecha < fechaFin)
                                {
                                    var firstDayOfMonth = new DateTime(tmpFecha.Year, tmpFecha.Month, 1);
                                    if (!dtBarras.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                        dtBarras.Columns.Add(firstDayOfMonth.ToString("dd/MM/yyyy"));
                                    tmpFecha = tmpFecha.AddMonths(1);
                                }
                                else
                                    continua = false;
                                rango = "Mensual";
                                break;
                            case 4:
                                if (tmpFecha < fechaFin)
                                {
                                    if (!dtBarras.Columns.Contains(tmpFecha.ToString("dd/MM/yyyy")))
                                        dtBarras.Columns.Add(tmpFecha.ToString("dd/MM/yyyy"));
                                    tmpFecha = tmpFecha.AddYears(1);
                                }
                                else
                                    continua = false;
                                rango = "Anual";
                                break;
                        }

                    }

                    qry = from icr in db.InformacionConsultaRate
                          where icr.IdInformacionConsulta == idInformacionConsulta
                          && icr.FechaModificacion >= fechaInicio
                                && icr.FechaModificacion <= fechaFin
                          select icr;
                    var rate = qry.Distinct().ToList();

                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.FechaModificacion.ToString("dd/MM/yyyy") == column.ColumnName && w.MeGusta);
                                    dtBarras.Rows[1][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.FechaModificacion.ToString("dd/MM/yyyy") == column.ColumnName && w.NoMeGusta);
                                }
                            }
                            break;
                        case 2:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.MeGusta
                                        && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[1][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.NoMeGusta
                                        && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                }
                            }
                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.MeGusta
                                        && w.FechaModificacion.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[1][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.NoMeGusta
                                        && w.FechaModificacion.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.MeGusta
                                        && w.FechaModificacion.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[1][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.NoMeGusta
                                        && w.FechaModificacion.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                }
                            }
                            break;
                    }
                }
                else
                {
                    restaMes = false;
                    var rate = qry.Distinct().ToList();
                    List<string> lstFechas = rate.OrderBy(o => o.FechaModificacion).Distinct().ToList().Select(s => s.FechaModificacion.ToString("dd/MM/yyyy")).Distinct().ToList();
                    fechaInicio = DateTime.ParseExact(lstFechas.First(), "dd/MM/yyyy", null);
                    fechaFin = DateTime.ParseExact(lstFechas.Last(), "dd/MM/yyyy", null);
                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (string fecha in lstFechas)
                            {
                                dtBarras.Columns.Add(fecha, typeof(int));
                            }
                            break;
                        case 2:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy"));
                                conteo++;
                            }
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                var firstDayOfMonth = new DateTime(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Month, 1);

                                if (!dtBarras.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(firstDayOfMonth.ToString("dd/MM/yyyy"));
                            }
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                DateTime firstDay = new DateTime(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, 1, 1);
                                if (!dtBarras.Columns.Contains(firstDay.ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(firstDay.ToString("dd/MM/yyyy"));
                            }
                            break;
                    }
                    foreach (DataColumn column in dtBarras.Columns)
                    {
                        if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                        {
                            switch (tipoFecha)
                            {
                                case 1:
                                    dtBarras.Rows[0][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.FechaModificacion.ToString("dd/MM/yyyy") == column.ColumnName && w.MeGusta);
                                    dtBarras.Rows[1][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.FechaModificacion.ToString("dd/MM/yyyy") == column.ColumnName && w.NoMeGusta);
                                    break;
                                case 2:
                                    dtBarras.Rows[0][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.MeGusta
                                            && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[1][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.NoMeGusta
                                            && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.FechaModificacion.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    break;
                                case 3:
                                    dtBarras.Rows[0][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.FechaModificacion.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.MeGusta);
                                    dtBarras.Rows[1][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.FechaModificacion.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.NoMeGusta);
                                    break;
                                case 4:
                                    dtBarras.Rows[0][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.FechaModificacion.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.MeGusta);
                                    dtBarras.Rows[1][column.ColumnName] = rate.Count(w => w.IdInformacionConsulta == idInformacionConsulta && w.FechaModificacion.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.NoMeGusta);
                                    break;
                            }
                        }
                    }
                }
                switch (tipoFecha)
                {
                    case 1:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 2:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 3:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 4:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                }

                result.GraficoBarras = dtBarras;

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Color", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));

                dtPie.Rows.Add("Like", ConfigurationManager.AppSettings["ColorLike"]);
                dtPie.Rows.Add("Dont Like", ConfigurationManager.AppSettings["ColorDontLike"]);
                int totalLike = 0;
                int totaldontLike = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                    {
                        totalLike += int.Parse(dtBarras.Rows[0][column.ColumnName].ToString());
                        totaldontLike += int.Parse(dtBarras.Rows[1][column.ColumnName].ToString());
                    }
                }

                dtPie.Rows[0][2] = totalLike;
                dtPie.Rows[1][2] = totaldontLike;
                result.GraficoPie = dtPie;
                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy", cultureInfo), fechaFin.ToString("dd/MM/yyyy", cultureInfo));
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy", cultureInfo), fechaFin.ToString("dd/MM/yyyy", cultureInfo));
                        break;
                    case 3:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("MMM", cultureInfo), restaMes ? fechaFin.AddDays(-1).ToString("MMM", cultureInfo) : fechaFin.ToString("MMM", cultureInfo));
                        break;
                    case 4:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("yyyy", cultureInfo), restaMes ? fechaFin.AddDays(-1).ToString("yyyy", cultureInfo) : fechaFin.ToString("yyyy", cultureInfo));
                        break;
                }


                result.Titulo = titulo;
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
