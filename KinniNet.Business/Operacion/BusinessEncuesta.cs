using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Helper.Reportes;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessEncuesta : IDisposable
    {
        CultureInfo cultureinfo = new CultureInfo("es-MX");
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessEncuesta(bool proxy = false)
        {
            _proxy = proxy;
        }

        public List<Encuesta> ObtenerEncuestas(bool insertarSeleccion)
        {
            List<Encuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Encuesta.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Encuesta
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

        public Encuesta ObtenerEncuestaById(int idEncuesta)
        {
            Encuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Encuesta.SingleOrDefault(w => w.Id == idEncuesta);
                if (result != null)
                {
                    db.LoadProperty(result, "TipoEncuesta");
                    db.LoadProperty(result, "EncuestaPregunta");
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

        public List<Encuesta> ObtenerEncuestaByGrupos(List<int> grupos, bool insertarSeleccion)
        {
            List<Encuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          where t.EncuestaRespondida
                          && grupos.Contains(tgu.IdGrupoUsuario)
                          select new { t, e };
                result = qry.Select(s => s.e).Distinct().ToList();
                foreach (Encuesta encuesta in result)
                {
                    db.LoadProperty(encuesta, "TipoEncuesta");
                    db.LoadProperty(encuesta, "EncuestaPregunta");
                }
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Encuesta
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

        public Encuesta ObtenerEncuestaByIdTicket(int idTicket)
        {
            Encuesta result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Ticket tk = db.Ticket.Single(s => s.Id == idTicket);
                if (tk != null)
                {
                    if (tk.EncuestaRespondida)
                        throw new Exception("Esta encuesta ya ha sido contestada anteriormente.");
                    db.LoadProperty(tk, "Encuesta");
                    result = tk.Encuesta;
                    if (result != null)
                    {
                        result.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(tk.IdArbolAcceso);
                        db.LoadProperty(result, "TipoEncuesta");
                        db.LoadProperty(result, "EncuestaPregunta");
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
        public Encuesta ObtenerEncuestaByIdConsulta(int idConsulta)
        {
            Encuesta result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                ArbolAcceso ac = db.ArbolAcceso.Single(s => s.Id == idConsulta);
                if (ac != null)
                {
                    db.LoadProperty(ac, "InventarioArbolAcceso");
                    foreach (InventarioArbolAcceso inventarioArbolAcceso in ac.InventarioArbolAcceso)
                    {
                        db.LoadProperty(inventarioArbolAcceso, "Encuesta");
                    }
                    result = ac.InventarioArbolAcceso.First().Encuesta;
                    if (result != null)
                    {
                        result.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ac.Id);
                        db.LoadProperty(result, "TipoEncuesta");
                        db.LoadProperty(result, "EncuestaPregunta");
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

        public void GuardarEncuesta(Encuesta encuesta)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                encuesta.Titulo = encuesta.Titulo.Trim();
                encuesta.TituloCliente = encuesta.TituloCliente.Trim();
                encuesta.Descripcion = encuesta.Descripcion.Trim();
                encuesta.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                //TODO: Cambiar habilitado por el embebido
                encuesta.Sistema = false;
                encuesta.Habilitado = true;
                if (db.Encuesta.Any(a => a.Titulo == encuesta.Titulo && a.IdTipoEncuesta == encuesta.IdTipoEncuesta))
                    throw new Exception("Esta encuesta ya Existe.");
                if (encuesta.Id == 0)
                    db.Encuesta.AddObject(encuesta);
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

        public void ActualizarEncuesta(Encuesta encuesta)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Encuesta encuestaActualizar = db.Encuesta.SingleOrDefault(s => s.Id == encuesta.Id);
                if (encuestaActualizar != null)
                {
                    encuestaActualizar.Titulo = encuesta.Titulo.Trim();
                    encuestaActualizar.TituloCliente = encuesta.TituloCliente.Trim();
                    encuestaActualizar.Descripcion = encuesta.Descripcion.Trim();
                    encuestaActualizar.IdUsuarioModifico = encuesta.IdUsuarioModifico;
                    encuestaActualizar.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    if (db.Encuesta.Any(a => a.Titulo == encuesta.Titulo && a.IdTipoEncuesta == encuesta.IdTipoEncuesta && a.Id != encuesta.Id))
                        throw new Exception("Esta encuesta ya Existe.");
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

        public List<Encuesta> Consulta(string descripcion)
        {
            List<Encuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Encuesta> qry = db.Encuesta;
                if (descripcion.Trim() != string.Empty)
                    qry = qry.Where(w => w.Titulo.ToLower().Contains(descripcion));
                result = qry.ToList();
                foreach (Encuesta encuesta in result)
                {
                    db.LoadProperty(encuesta, "TipoEncuesta");
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

        public List<HelperEncuesta> ObtenerEncuestasPendientesUsuario(int idUsuario)
        {
            List<HelperEncuesta> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<Ticket> lstEncuestas = db.Ticket.Where(w => w.EncuestaRespondida == false && w.IdEncuesta != null && w.IdUsuarioLevanto == idUsuario).ToList();
                if (lstEncuestas.Count > 0)
                {
                    result = new List<HelperEncuesta>();
                    foreach (Ticket ticket in lstEncuestas)
                    {

                        db.LoadProperty(ticket, "Encuesta");
                        if (ticket.IdEncuesta != null)
                        {
                            HelperEncuesta hEncuesta = new HelperEncuesta
                            {
                                NumeroTicket = ticket.Id,
                                IdEncuesta = (int)ticket.IdEncuesta,
                                Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso),
                                Descripcion = ticket.Encuesta.Descripcion,
                                Respondida = ticket.EncuestaRespondida,
                            };
                            result.Add(hEncuesta);
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

        public void HabilitarEncuesta(int idencuesta, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Encuesta encuesta = db.Encuesta.SingleOrDefault(w => w.Id == idencuesta);
                if (encuesta != null) encuesta.Habilitado = habilitado;
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

        public void ContestaEncuesta(List<RespuestaEncuesta> encuestaRespondida, int idTicket)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket t = db.Ticket.SingleOrDefault(s => s.Id == idTicket);
                if (t != null && !t.EncuestaRespondida)
                {
                    foreach (RespuestaEncuesta respuesta in encuestaRespondida)
                    {
                        respuesta.IdArbol = t.IdArbolAcceso;
                        respuesta.Ponderacion = (respuesta.Ponderacion * db.EncuestaPregunta.Single(s => s.Id == respuesta.IdPregunta).Ponderacion) / 100;
                        db.RespuestaEncuesta.AddObject(respuesta);
                    }
                    t.EncuestaRespondida = true;
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

        public List<Encuesta> ObtenerEncuestasContestadas(bool insertarSeleccion)
        {
            List<Encuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          where t.EncuestaRespondida
                          select new { t, e };
                result = qry.Select(s => s.e).Distinct().ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Encuesta
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

        #region Graficos

        #region NPS

        public HelperReporteEncuesta ObtenerGraficoNps(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
            int conteo = 1;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool restaMes;

                result = new HelperReporteEncuesta { IdArbolAcceso = idArbolAcceso };
                string titulo = db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).Descripcion;

                string rango = string.Empty;
                DateTime fechaInicio;
                DateTime fechaFin;

                var qryRespuestas = from re in db.RespuestaEncuesta
                                    join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                    join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                    where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                          && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                          && aa.Id == idArbolAcceso
                                    select new { re, te };

                var qryPromotores = from re in db.RespuestaEncuesta
                                    join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                    join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                    where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                          && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                          && re.ValorRespuesta >= 9
                                          && aa.Id == idArbolAcceso
                                    select new { re, te };

                List<int> neutros = new List<int> { 7, 8 };
                var qryNeutros = from re in db.RespuestaEncuesta
                                 join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                 join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                 where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                       && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                       && neutros.Contains(re.ValorRespuesta)
                                       && aa.Id == idArbolAcceso
                                 select new { re, te };

                var qryDetractores = from re in db.RespuestaEncuesta
                                     join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                     join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                     where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                           && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                           && re.ValorRespuesta <= 6
                                           && aa.Id == idArbolAcceso
                                     select new { re, te };

                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));
                dtBarras.Columns.Add("Color", typeof(string));

                dtBarras.Rows.Add("Promotores; 9 y 10", ConfigurationManager.AppSettings["ColorNPSPromotor"]);
                dtBarras.Rows.Add("Neutros; 7 y 8", ConfigurationManager.AppSettings["ColorNPSNeutro"]);
                dtBarras.Rows.Add("Detractores; menor a 7", ConfigurationManager.AppSettings["ColorNPSDetractor"]);

                if (fechas != null)
                {
                    restaMes = true;
                    fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);

                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    DateTime tmpFecha = fechaInicio;
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

                    qryPromotores = from q in qryPromotores
                                    where q.te.FechaMovimiento >= fechaInicio
                                           && q.te.FechaMovimiento < fechaFin
                                    select q;

                    qryNeutros = from q in qryNeutros
                                 where q.te.FechaMovimiento >= fechaInicio
                                           && q.te.FechaMovimiento < fechaFin
                                 select q;

                    qryDetractores = from q in qryDetractores
                                     where q.te.FechaMovimiento >= fechaInicio
                                           && q.te.FechaMovimiento < fechaFin
                                     select q;

                    var resultPromotores = qryPromotores.Distinct().ToList();
                    var resultNeutros = qryNeutros.Distinct().ToList();
                    var resultDetractores = qryDetractores.Distinct().ToList();


                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[1][column.ColumnName] = resultNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[2][column.ColumnName] = resultDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                }
                            }
                            break;
                        case 2:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPromotores.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[1][column.ColumnName] = resultNeutros.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[2][column.ColumnName] = resultDetractores.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                }
                            }
                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[1][column.ColumnName] = resultNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[2][column.ColumnName] = resultDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[1][column.ColumnName] = resultNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[2][column.ColumnName] = resultDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                }
                            }
                            break;
                    }
                }
                else
                {
                    restaMes = false;
                    var ratePromotores = qryPromotores.Distinct().ToList();
                    var rateNeutros = qryNeutros.Distinct().ToList();
                    var rateDetractores = qryDetractores.Distinct().ToList();
                    List<string> lstFechas = qryRespuestas.OrderBy(o => o.te.FechaMovimiento).Distinct().ToList().Select(s => s.te.FechaMovimiento.ToString("dd/MM/yyyy")).Distinct().ToList();
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
                                    dtBarras.Rows[0][column.ColumnName] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[1][column.ColumnName] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[2][column.ColumnName] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    break;
                                case 2:
                                    dtBarras.Rows[0][column.ColumnName] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[1][column.ColumnName] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[2][column.ColumnName] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    break;
                                case 3:
                                    dtBarras.Rows[0][column.ColumnName] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[1][column.ColumnName] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[2][column.ColumnName] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    break;
                                case 4:
                                    dtBarras.Rows[0][column.ColumnName] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[1][column.ColumnName] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[2][column.ColumnName] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
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
                            dtBarras.Columns[i].ColumnName = DateTime.Parse(dtBarras.Columns[i].ColumnName, cultureinfo).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 2:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.Parse(dtBarras.Columns[i].ColumnName, cultureinfo).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 3:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.Parse(dtBarras.Columns[i].ColumnName, cultureinfo).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 4:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.Parse(dtBarras.Columns[i].ColumnName, cultureinfo).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                }

                result.GraficoBarras = dtBarras;


                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Color", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));

                dtPie.Rows.Add("Promotores; 9 y 10", ConfigurationManager.AppSettings["ColorNPSPromotor"]);
                dtPie.Rows.Add("Neutros; 7 y 8", ConfigurationManager.AppSettings["ColorNPSNeutro"]);
                dtPie.Rows.Add("Detractores; menor a 7", ConfigurationManager.AppSettings["ColorNPSDetractor"]);

                int totalPromotores = 0;
                int totalNeutros = 0;
                int totalDetractores = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                    {
                        totalPromotores += int.Parse(dtBarras.Rows[0][column.ColumnName].ToString());
                        totalNeutros += int.Parse(dtBarras.Rows[1][column.ColumnName].ToString());
                        totalDetractores += int.Parse(dtBarras.Rows[2][column.ColumnName].ToString());
                    }
                }

                dtPie.Rows[0][2] = totalPromotores;
                dtPie.Rows[1][2] = totalNeutros;
                dtPie.Rows[2][2] = totalDetractores;
                result.GraficoPie = dtPie;

                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy", cultureinfo), fechaFin.ToString("dd/MM/yyyy", cultureinfo));
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy", cultureinfo), fechaFin.ToString("dd/MM/yyyy", cultureinfo));
                        break;
                    case 3:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("MMM", cultureinfo), restaMes ? fechaFin.AddDays(-1).ToString("MMM", cultureinfo) : fechaFin.ToString("MMM", cultureinfo));
                        break;
                    case 4:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("yyyy", cultureinfo), restaMes ? fechaFin.AddDays(-1).ToString("yyyy", cultureinfo) : fechaFin.ToString("yyyy", cultureinfo));
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
        public DataTable ObtenerGraficoNpsDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas)
        {
            DataTable result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                int idEncuesta = (int)db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).IdEncuesta;

                var qryFiltro = from re in db.RespuestaEncuesta
                                join ep in db.EncuestaPregunta on re.IdPregunta equals ep.Id
                                join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                      && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                      && aa.Id == idArbolAcceso
                                select new { re, te, ep };


                if (fechas != null)
                {

                    DateTime fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    DateTime fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    qryFiltro = from q in qryFiltro
                                where q.te.FechaMovimiento >= fechaInicio
                                       && q.te.FechaMovimiento < fechaFin
                                select q;
                }
                var qry = qryFiltro.ToList();

                result = new DataTable(idEncuesta.ToString());
                result.Columns.Add("Ticket", typeof(int));
                result.Columns.Add("GrupoAtencion", typeof(string));
                result.Columns.Add("AgenteAtendio", typeof(string));
                result.Columns.Add("Usuario", typeof(string));
                result.Columns.Add("Telefono", typeof(string));
                result.Columns.Add("Correo", typeof(string));
                result.Columns.Add("FechaCierre", typeof(string));

                foreach (EncuestaPregunta pregunta in db.EncuestaPregunta.Where(w => w.IdEncuesta == idEncuesta))
                {
                    DataColumn col = new DataColumn { ColumnName = pregunta.Pregunta, DataType = typeof(string), DefaultValue = "0" };
                    col.ExtendedProperties.Add("IdPregunta", pregunta.Id);
                    result.Columns.Add(col);
                }


                foreach (var source in qry.Select(s => new { s.te }).Distinct().ToList())
                {
                    int idTicket = (int)source.te.IdTicket;
                    Ticket t = db.Ticket.Single(s => s.Id == idTicket);
                    GrupoUsuario gpoAtencion = (from tgu in db.TicketGrupoUsuario
                                                join gu in db.GrupoUsuario on tgu.IdGrupoUsuario equals gu.Id
                                                where tgu.IdTicket == t.Id && gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente
                                                select gu).Distinct().FirstOrDefault();
                    if (gpoAtencion != null)
                        result.Rows.Add(idTicket, gpoAtencion.Descripcion,
                            t.IdUsuarioUltimoAgenteAsignado != null ? new BusinessUsuarios().ObtenerDetalleUsuario((int)t.IdUsuarioUltimoAgenteAsignado).NombreCompleto : string.Empty,
                            new BusinessUsuarios().ObtenerDetalleUsuario(t.IdUsuarioSolicito).NombreCompleto,
                            new BusinessUsuarios().ObtenerDetalleUsuario(t.IdUsuarioSolicito).TelefonoPrincipal,
                            new BusinessUsuarios().ObtenerDetalleUsuario(t.IdUsuarioSolicito).CorreoPrincipal,
                            source.te.FechaMovimiento);
                }
                foreach (DataRow row in result.Rows)
                {
                    for (int i = 0; i < result.Columns.Count; i++)
                    {
                        if (result.Columns[i].ColumnName != "Ticket" && result.Columns[i].ColumnName != "GrupoAtencion" && result.Columns[i].ColumnName != "AgenteAtendio" && result.Columns[i].ColumnName != "Usuario" && result.Columns[i].ColumnName != "Telefono" && result.Columns[i].ColumnName != "Correo" && result.Columns[i].ColumnName != "FechaCierre")
                        {

                            int idTicket = int.Parse(row[0].ToString());
                            int idPregunta = (int)result.Columns[i].ExtendedProperties["IdPregunta"];
                            RespuestaEncuesta respuesta =
                                db.RespuestaEncuesta.SingleOrDefault(
                                    w => w.IdPregunta == idPregunta && w.IdTicket == idTicket);
                            if (respuesta != null)
                                row[i] = respuesta.ValorRespuesta;
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

        #endregion NPS

        #region Calificacion
        public HelperReporteEncuesta ObtenerGraficoCalificacion(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
            int conteo = 1;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool restaMes;

                result = new HelperReporteEncuesta { IdArbolAcceso = idArbolAcceso };
                string titulo = db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).Descripcion;
                int idEncuesta = (int)db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).IdEncuesta;

                string rango = string.Empty;
                DateTime fechaInicio;
                DateTime fechaFin;

                var qryFiltro = from re in db.RespuestaEncuesta
                                join ep in db.EncuestaPregunta on re.IdPregunta equals ep.Id
                                join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.Calificacion
                                      && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                      && aa.Id == idArbolAcceso
                                select new { re, te, ep };

                var qryCeroCinco = from q in qryFiltro
                                   where q.re.ValorRespuesta <= 5
                                   select q;

                var qrySeisSiete = from q in qryFiltro
                                   where q.re.ValorRespuesta >= 6 && q.re.ValorRespuesta <= 7
                                   select q;

                var qryOchoNueve = from q in qryFiltro
                                   where q.re.ValorRespuesta >= 8 && q.re.ValorRespuesta <= 9
                                   select q;

                var qryDiez = from q in qryFiltro
                              where q.re.ValorRespuesta == 10
                              select q;

                result.Preguntas = new List<DataTable>();
                foreach (EncuestaPregunta encuestaPregunta in db.EncuestaPregunta.Where(w => w.IdEncuesta == idEncuesta))
                {
                    DataTable dt = new DataTable(encuestaPregunta.Id.ToString());
                    dt.Columns.Add("Descripcion", typeof(string));
                    dt.Columns.Add("Color", typeof(string));
                    dt.Rows.Add("0 - 5", ConfigurationManager.AppSettings["ColorEscala0_5"]);
                    dt.Rows.Add("6 - 7", ConfigurationManager.AppSettings["ColorEscala6_7"]);
                    dt.Rows.Add("8 - 9", ConfigurationManager.AppSettings["ColorEscala8_9"]);
                    dt.Rows.Add("10", ConfigurationManager.AppSettings["ColorEscala10"]);
                    dt.ExtendedProperties.Add("Pregunta", encuestaPregunta.Pregunta);
                    result.Preguntas.Add(dt);
                }


                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));
                dtBarras.Columns.Add("Color", typeof(string));
                dtBarras.Rows.Add("0 - 5", ConfigurationManager.AppSettings["ColorEscala0_5"]);
                dtBarras.Rows.Add("6 - 7", ConfigurationManager.AppSettings["ColorEscala6_7"]);
                dtBarras.Rows.Add("8 - 9", ConfigurationManager.AppSettings["ColorEscala8_9"]);
                dtBarras.Rows.Add("10", ConfigurationManager.AppSettings["ColorEscala10"]);

                if (fechas != null)
                {

                    restaMes = true;
                    fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    DateTime tmpFecha = fechaInicio;
                    bool continua = true;
                    while (continua)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                if (tmpFecha < fechaFin)
                                {
                                    dtBarras.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy"), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        dt.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy"), typeof(int)));
                                    }

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
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy"), typeof(int)));
                                    }
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
                                        dtBarras.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                    }
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
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy")));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy")));
                                    }
                                    tmpFecha = tmpFecha.AddYears(1);
                                }
                                else
                                    continua = false;
                                rango = "Anual";
                                break;
                        }

                    }

                    qryCeroCinco = from q in qryCeroCinco
                                   where q.te.FechaMovimiento >= fechaInicio
                                          && q.te.FechaMovimiento < fechaFin
                                   select q;

                    qrySeisSiete = from q in qrySeisSiete
                                   where q.te.FechaMovimiento >= fechaInicio
                                             && q.te.FechaMovimiento < fechaFin
                                   select q;

                    qryOchoNueve = from q in qryOchoNueve
                                   where q.te.FechaMovimiento >= fechaInicio
                                         && q.te.FechaMovimiento < fechaFin
                                   select q;
                    qryDiez = from q in qryDiez
                              where q.te.FechaMovimiento >= fechaInicio
                                    && q.te.FechaMovimiento < fechaFin
                              select q;

                    var resultCeroCinco = qryCeroCinco.Distinct().ToList();
                    var resultSeisSiete = qrySeisSiete.Distinct().ToList();
                    var resultOchoNueve = qryOchoNueve.Distinct().ToList();
                    var resultDiez = qryDiez.Distinct().ToList();

                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }

                            break;
                        case 2:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                }
                            }

                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }

                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.te.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.te.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.te.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.te.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    restaMes = false;
                    var rateCeroCinco = qryCeroCinco.Distinct().ToList();
                    var rateSeisSiete = qrySeisSiete.Distinct().ToList();
                    var rateOchoNueve = qryOchoNueve.Distinct().ToList();
                    var rateDiez = qryDiez.Distinct().ToList();


                    List<string> lstFechas = qryFiltro.OrderBy(o => o.te.FechaMovimiento).Distinct().ToList().Select(s => s.te.FechaMovimiento.ToString("dd/MM/yyyy")).Distinct().ToList();
                    fechaInicio = DateTime.ParseExact(lstFechas.First(), "dd/MM/yyyy", null);
                    fechaFin = DateTime.ParseExact(lstFechas.Last(), "dd/MM/yyyy", null);
                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (string fecha in lstFechas)
                            {
                                dtBarras.Columns.Add(fecha, typeof(int));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Columns.Add(fecha, typeof(int));
                                }
                            }
                            break;
                        case 2:

                            foreach (string fecha in lstFechas)
                            {

                                if (!dtBarras.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")));
                                }
                                conteo++;
                            }
                            break;
                        case 3:

                            foreach (string fecha in lstFechas)
                            {
                                var firstDayOfMonth = new DateTime(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Month, 1);

                                if (!dtBarras.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy"), typeof(int)));
                                }
                            }
                            break;
                        case 4:

                            foreach (string fecha in lstFechas)
                            {
                                DateTime firstDay = new DateTime(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, 1, 1);
                                if (!dtBarras.Columns.Contains(firstDay.ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(firstDay.ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(firstDay.ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(firstDay.ToString("dd/MM/yyyy"), typeof(int)));
                                }
                            }
                            rango = "Anual";
                            break;
                    }

                    for (int i = 2; i < dtBarras.Columns.Count; i++)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                dtBarras.Rows[0][i] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                dtBarras.Rows[1][i] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                dtBarras.Rows[2][i] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                dtBarras.Rows[3][i] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][i] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][i] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 2:
                                dtBarras.Rows[0][i] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                dtBarras.Rows[1][i] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                dtBarras.Rows[2][i] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                dtBarras.Rows[3][i] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][i] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][i] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                }

                                break;
                            case 3:
                                dtBarras.Rows[0][i] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                dtBarras.Rows[1][i] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                dtBarras.Rows[2][i] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                dtBarras.Rows[3][i] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][i] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][i] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 4:
                                dtBarras.Rows[0][i] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                dtBarras.Rows[1][i] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                dtBarras.Rows[2][i] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                dtBarras.Rows[3][i] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][i] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][i] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                        }

                    }
                }

                result.GraficoBarras = dtBarras;

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Color", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));

                dtPie.Rows.Add("0 - 5", ConfigurationManager.AppSettings["ColorEscala0_5"]);
                dtPie.Rows.Add("6 - 7", ConfigurationManager.AppSettings["ColorEscala6_7"]);
                dtPie.Rows.Add("8 - 9", ConfigurationManager.AppSettings["ColorEscala8_9"]);
                dtPie.Rows.Add("10", ConfigurationManager.AppSettings["ColorEscala10"]);


                int totalCeroCinco = 0;
                int totalSeisSiete = 0;
                int totalOchoNueve = 0;
                int totalDiez = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                    {
                        totalCeroCinco += int.Parse(dtBarras.Rows[0][column.ColumnName].ToString());
                        totalSeisSiete += int.Parse(dtBarras.Rows[1][column.ColumnName].ToString());
                        totalOchoNueve += int.Parse(dtBarras.Rows[2][column.ColumnName].ToString());
                        totalDiez += int.Parse(dtBarras.Rows[3][column.ColumnName].ToString());
                    }
                }

                dtPie.Rows[0][2] = totalCeroCinco;
                dtPie.Rows[1][2] = totalSeisSiete;
                dtPie.Rows[2][2] = totalOchoNueve;
                dtPie.Rows[3][2] = totalDiez;
                result.GraficoPie = dtPie;

                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy", cultureinfo), fechaFin.ToString("dd/MM/yyyy", cultureinfo));
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy", cultureinfo), fechaFin.ToString("dd/MM/yyyy", cultureinfo));
                        break;
                    case 3:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("MMM", cultureinfo), restaMes ? fechaFin.AddDays(-1).ToString("MMM", cultureinfo) : fechaFin.ToString("MMM", cultureinfo));
                        break;
                    case 4:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("yyyy", cultureinfo), restaMes ? fechaFin.AddDays(-1).ToString("yyyy", cultureinfo) : fechaFin.ToString("yyyy", cultureinfo));
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

        public DataTable ObtenerGraficoCalificacionDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas)
        {
            DataTable result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                int idEncuesta = (int)db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).IdEncuesta;

                var qryFiltro = from re in db.RespuestaEncuesta
                                join ep in db.EncuestaPregunta on re.IdPregunta equals ep.Id
                                join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.Calificacion
                                      && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                      && aa.Id == idArbolAcceso
                                select new { re, te, ep };


                if (fechas != null)
                {

                    DateTime fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    DateTime fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    qryFiltro = from q in qryFiltro
                                where q.te.FechaMovimiento >= fechaInicio
                                       && q.te.FechaMovimiento < fechaFin
                                select q;
                }
                var qry = qryFiltro.ToList();

                result = new DataTable(idEncuesta.ToString());
                result.Columns.Add("Ticket", typeof(int));
                result.Columns.Add("GrupoAtencion", typeof(string));
                result.Columns.Add("AgenteAtendio", typeof(string));
                result.Columns.Add("Usuario", typeof(string));
                result.Columns.Add("FechaCierre", typeof(string));

                foreach (EncuestaPregunta pregunta in db.EncuestaPregunta.Where(w => w.IdEncuesta == idEncuesta))
                {
                    DataColumn col = new DataColumn { ColumnName = pregunta.Pregunta, DataType = typeof(string), DefaultValue = "0" };
                    col.ExtendedProperties.Add("IdPregunta", pregunta.Id);
                    result.Columns.Add(col);
                }


                foreach (var source in qry.Select(s => new { s.te }).Distinct().ToList())
                {
                    int idTicket = (int)source.te.IdTicket;
                    Ticket t = db.Ticket.Single(s => s.Id == idTicket);
                    GrupoUsuario gpoAtencion = (from tgu in db.TicketGrupoUsuario
                                                join gu in db.GrupoUsuario on tgu.IdGrupoUsuario equals gu.Id
                                                where tgu.IdTicket == t.Id && gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente
                                                select gu).Distinct().FirstOrDefault();
                    if (gpoAtencion != null)
                        result.Rows.Add(idTicket, gpoAtencion.Descripcion,
                            t.IdUsuarioUltimoAgenteAsignado != null ? new BusinessUsuarios().ObtenerDetalleUsuario((int)t.IdUsuarioUltimoAgenteAsignado).NombreCompleto : string.Empty,
                            new BusinessUsuarios().ObtenerDetalleUsuario(t.IdUsuarioSolicito).NombreCompleto,
                            source.te.FechaMovimiento);
                }
                foreach (DataRow row in result.Rows)
                {
                    for (int i = 0; i < result.Columns.Count; i++)
                    {
                        if (result.Columns[i].ColumnName != "Ticket" && result.Columns[i].ColumnName != "GrupoAtencion" && result.Columns[i].ColumnName != "AgenteAtendio" && result.Columns[i].ColumnName != "Usuario" && result.Columns[i].ColumnName != "FechaCierre")
                        {

                            int idTicket = int.Parse(row[0].ToString());
                            int idPregunta = (int)result.Columns[i].ExtendedProperties["IdPregunta"];
                            RespuestaEncuesta respuesta =
                                db.RespuestaEncuesta.SingleOrDefault(
                                    w => w.IdPregunta == idPregunta && w.IdTicket == idTicket);
                            if (respuesta != null)
                                row[i] = respuesta.ValorRespuesta;
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
        #endregion Calificacion

        #region Satisfaccion
        public HelperReporteEncuesta ObtenerGraficoSatisfaccion(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
            int conteo = 1;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool restaMes;

                result = new HelperReporteEncuesta { IdArbolAcceso = idArbolAcceso };
                string titulo = db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).Descripcion;
                int idEncuesta = (int)db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).IdEncuesta;

                string rango = string.Empty;
                DateTime fechaInicio;
                DateTime fechaFin;

                var qryFiltro = from re in db.RespuestaEncuesta
                                join ep in db.EncuestaPregunta on re.IdPregunta equals ep.Id
                                join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.CalificacionPesimoMaloRegularBuenoExcelente
                                      && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                      && aa.Id == idArbolAcceso
                                select new { re, te, ep };

                var qryPesimo = from q in qryFiltro
                                where q.re.ValorRespuesta == 1
                                select q;

                var qryMalo = from q in qryFiltro
                              where q.re.ValorRespuesta == 2
                              select q;

                var qryRegular = from q in qryFiltro
                                 where q.re.ValorRespuesta == 3
                                 select q;

                var qryBueno = from q in qryFiltro
                               where q.re.ValorRespuesta == 4
                               select q;

                var qryExcelente = from q in qryFiltro
                                   where q.re.ValorRespuesta == 5
                                   select q;

                result.Preguntas = new List<DataTable>();
                foreach (EncuestaPregunta encuestaPregunta in db.EncuestaPregunta.Where(w => w.IdEncuesta == idEncuesta))
                {
                    DataTable dt = new DataTable(encuestaPregunta.Id.ToString());
                    dt.Columns.Add("Descripcion", typeof(string));
                    dt.Columns.Add("Color", typeof(string));
                    dt.Rows.Add("Pesimo", ConfigurationManager.AppSettings["ColorValoracionPesimo"]);
                    dt.Rows.Add("Malo", ConfigurationManager.AppSettings["ColorValoracionMalo"]);
                    dt.Rows.Add("Regular", ConfigurationManager.AppSettings["ColorValoracionRegular"]);
                    dt.Rows.Add("Bueno", ConfigurationManager.AppSettings["ColorValoracionBueno"]);
                    dt.Rows.Add("Excelente", ConfigurationManager.AppSettings["ColorValoracionExcelente"]);
                    dt.ExtendedProperties.Add("Pregunta", encuestaPregunta.Pregunta);
                    result.Preguntas.Add(dt);
                }

                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));
                dtBarras.Columns.Add("Color", typeof(string));
                dtBarras.Rows.Add("Pesimo", ConfigurationManager.AppSettings["ColorValoracionPesimo"]);
                dtBarras.Rows.Add("Malo", ConfigurationManager.AppSettings["ColorValoracionMalo"]);
                dtBarras.Rows.Add("Regular", ConfigurationManager.AppSettings["ColorValoracionRegular"]);
                dtBarras.Rows.Add("Bueno", ConfigurationManager.AppSettings["ColorValoracionBueno"]);
                dtBarras.Rows.Add("Excelente", ConfigurationManager.AppSettings["ColorValoracionExcelente"]);

                if (fechas != null)
                {
                    restaMes = true;
                    fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    DateTime tmpFecha = fechaInicio;
                    bool continua = true;
                    while (continua)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                if (tmpFecha < fechaFin)
                                {
                                    dtBarras.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy"), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        dt.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy"), typeof(int)));
                                    }

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
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy"), typeof(int)));
                                    }
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
                                        dtBarras.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                    }
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
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy")));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy")));
                                    }
                                    tmpFecha = tmpFecha.AddYears(1);
                                }
                                else
                                    continua = false;
                                rango = "Anual";
                                break;
                        }

                    }

                    qryPesimo = from q in qryPesimo
                                where q.te.FechaMovimiento >= fechaInicio
                                       && q.te.FechaMovimiento < fechaFin
                                select q;

                    qryMalo = from q in qryMalo
                              where q.te.FechaMovimiento >= fechaInicio
                                        && q.te.FechaMovimiento < fechaFin
                              select q;

                    qryRegular = from q in qryRegular
                                 where q.te.FechaMovimiento >= fechaInicio
                                       && q.te.FechaMovimiento < fechaFin
                                 select q;

                    qryBueno = from q in qryBueno
                               where q.te.FechaMovimiento >= fechaInicio
                                     && q.te.FechaMovimiento < fechaFin
                               select q;

                    qryExcelente = from q in qryExcelente
                                   where q.te.FechaMovimiento >= fechaInicio
                                         && q.te.FechaMovimiento < fechaFin
                                   select q;

                    var resultPesimo = qryPesimo.Distinct().ToList();
                    var resultMalo = qryMalo.Distinct().ToList();
                    var resultRegular = qryRegular.Distinct().ToList();
                    var resultBueno = qryBueno.Distinct().ToList();
                    var resultExcelente = qryExcelente.Distinct().ToList();

                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 2:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    restaMes = false;
                    var ratePesimo = qryPesimo.Distinct().ToList();
                    var rateMalo = qryMalo.Distinct().ToList();
                    var rateRegular = qryRegular.Distinct().ToList();
                    var rateBueno = qryBueno.Distinct().ToList();
                    var rateExcelente = qryExcelente.Distinct().ToList();

                    List<string> lstFechas = qryFiltro.OrderBy(o => o.te.FechaMovimiento).Distinct().ToList().Select(s => s.te.FechaMovimiento.ToString("dd/MM/yyyy")).Distinct().ToList();
                    fechaInicio = DateTime.ParseExact(lstFechas.First(), "dd/MM/yyyy", null);
                    fechaFin = DateTime.ParseExact(lstFechas.Last(), "dd/MM/yyyy", null);
                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (string fecha in lstFechas)
                            {
                                dtBarras.Columns.Add(fecha, typeof(int));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Columns.Add(fecha, typeof(int));
                                }
                            }
                            break;
                        case 2:

                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")));
                                }
                                conteo++;
                            }
                            break;
                        case 3:

                            foreach (string fecha in lstFechas)
                            {
                                var firstDayOfMonth = new DateTime(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Month, 1);

                                if (!dtBarras.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy"), typeof(int)));
                                }
                            }
                            break;
                        case 4:

                            foreach (string fecha in lstFechas)
                            {
                                DateTime firstDay = new DateTime(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, 1, 1);
                                if (!dtBarras.Columns.Contains(firstDay.ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(firstDay.ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(firstDay.ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(firstDay.ToString("dd/MM/yyyy"), typeof(int)));
                                }
                            }
                            rango = "Anual";
                            break;
                    }

                    for (int i = 2; i < dtBarras.Columns.Count; i++)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                dtBarras.Rows[0][i] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                dtBarras.Rows[1][i] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                dtBarras.Rows[2][i] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                dtBarras.Rows[3][i] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                dtBarras.Rows[4][i] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][i] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][i] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[4][i] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));

                                }
                                break;
                            case 2:
                                dtBarras.Rows[0][i] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                dtBarras.Rows[1][i] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                dtBarras.Rows[2][i] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                dtBarras.Rows[3][i] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                dtBarras.Rows[4][i] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][i] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][i] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[4][i] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 3:
                                dtBarras.Rows[0][i] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                dtBarras.Rows[1][i] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                dtBarras.Rows[2][i] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                dtBarras.Rows[3][i] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                dtBarras.Rows[4][i] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][i] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][i] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[4][i] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));

                                }
                                break;
                            case 4:
                                dtBarras.Rows[0][i] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                dtBarras.Rows[1][i] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                dtBarras.Rows[2][i] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                dtBarras.Rows[3][i] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                dtBarras.Rows[4][i] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][i] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][i] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[4][i] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                        }

                    }
                }
                result.GraficoBarras = dtBarras;

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Color", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));

                dtPie.Rows.Add("Pesimo", ConfigurationManager.AppSettings["ColorValoracionPesimo"]);
                dtPie.Rows.Add("Malo", ConfigurationManager.AppSettings["ColorValoracionMalo"]);
                dtPie.Rows.Add("Regular", ConfigurationManager.AppSettings["ColorValoracionRegular"]);
                dtPie.Rows.Add("Bueno", ConfigurationManager.AppSettings["ColorValoracionBueno"]);
                dtPie.Rows.Add("Excelente", ConfigurationManager.AppSettings["ColorValoracionExcelente"]);


                int totalPesimo = 0;
                int totalMalo = 0;
                int totalRegular = 0;
                int totalBueno = 0;
                int totalExcelente = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                    {
                        totalPesimo += int.Parse(dtBarras.Rows[0][column.ColumnName].ToString());
                        totalMalo += int.Parse(dtBarras.Rows[1][column.ColumnName].ToString());
                        totalRegular += int.Parse(dtBarras.Rows[2][column.ColumnName].ToString());
                        totalBueno += int.Parse(dtBarras.Rows[3][column.ColumnName].ToString());
                        totalExcelente += int.Parse(dtBarras.Rows[4][column.ColumnName].ToString());
                    }
                }

                dtPie.Rows[0][2] = totalPesimo;
                dtPie.Rows[1][2] = totalMalo;
                dtPie.Rows[2][2] = totalRegular;
                dtPie.Rows[3][2] = totalBueno;
                dtPie.Rows[4][2] = totalExcelente;
                result.GraficoPie = dtPie;
                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy"));
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy"));
                        break;
                    case 3:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("MMM"), restaMes ? fechaFin.AddDays(-1).ToString("MMM") : fechaFin.ToString("MMM"));
                        break;
                    case 4:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("yyyy"), restaMes ? fechaFin.AddDays(-1).ToString("yyyy") : fechaFin.ToString("yyyy"));
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

        public DataTable ObtenerGraficoSatisfaccionDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas)
        {
            DataTable result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                int idEncuesta = (int)db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).IdEncuesta;

                var qryFiltro = from re in db.RespuestaEncuesta
                                join ep in db.EncuestaPregunta on re.IdPregunta equals ep.Id
                                join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.CalificacionPesimoMaloRegularBuenoExcelente
                                      && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                      && aa.Id == idArbolAcceso
                                select new { re, te, ep };


                if (fechas != null)
                {

                    DateTime fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    DateTime fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    qryFiltro = from q in qryFiltro
                                where q.te.FechaMovimiento >= fechaInicio
                                       && q.te.FechaMovimiento < fechaFin
                                select q;
                }
                var qry = qryFiltro.ToList();

                result = new DataTable(idEncuesta.ToString());
                result.Columns.Add("Ticket", typeof(int));
                result.Columns.Add("GrupoAtencion", typeof(string));
                result.Columns.Add("AgenteAtendio", typeof(string));
                result.Columns.Add("Usuario", typeof(string));
                result.Columns.Add("FechaCierre", typeof(string));

                foreach (EncuestaPregunta pregunta in db.EncuestaPregunta.Where(w => w.IdEncuesta == idEncuesta))
                {
                    DataColumn col = new DataColumn { ColumnName = pregunta.Pregunta, DataType = typeof(string), DefaultValue = "0" };
                    col.ExtendedProperties.Add("IdPregunta", pregunta.Id);
                    result.Columns.Add(col);
                }


                foreach (var source in qry.Select(s => new { s.te }).Distinct().ToList())
                {
                    int idTicket = (int)source.te.IdTicket;
                    Ticket t = db.Ticket.Single(s => s.Id == idTicket);
                    GrupoUsuario gpoAtencion = (from tgu in db.TicketGrupoUsuario
                                                join gu in db.GrupoUsuario on tgu.IdGrupoUsuario equals gu.Id
                                                where tgu.IdTicket == t.Id && gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente
                                                select gu).Distinct().FirstOrDefault();
                    if (gpoAtencion != null)
                        result.Rows.Add(idTicket, gpoAtencion.Descripcion,
                            t.IdUsuarioUltimoAgenteAsignado != null ? new BusinessUsuarios().ObtenerDetalleUsuario((int)t.IdUsuarioUltimoAgenteAsignado).NombreCompleto : string.Empty,
                            new BusinessUsuarios().ObtenerDetalleUsuario(t.IdUsuarioSolicito).NombreCompleto,
                            source.te.FechaMovimiento);
                }
                foreach (DataRow row in result.Rows)
                {
                    for (int i = 0; i < result.Columns.Count; i++)
                    {
                        if (result.Columns[i].ColumnName != "Ticket" && result.Columns[i].ColumnName != "GrupoAtencion" && result.Columns[i].ColumnName != "AgenteAtendio" && result.Columns[i].ColumnName != "Usuario" && result.Columns[i].ColumnName != "FechaCierre")
                        {

                            int idTicket = int.Parse(row[0].ToString());
                            int idPregunta = (int)result.Columns[i].ExtendedProperties["IdPregunta"];
                            RespuestaEncuesta respuesta =
                                db.RespuestaEncuesta.SingleOrDefault(
                                    w => w.IdPregunta == idPregunta && w.IdTicket == idTicket);
                            if (respuesta != null)
                                row[i] = respuesta.ValorRespuesta;
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
        #endregion Satisfaccion

        #region Logica
        public HelperReporteEncuesta ObtenerGraficoLogica(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
            int conteo = 1;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool restaMes;

                result = new HelperReporteEncuesta { IdArbolAcceso = idArbolAcceso };
                string titulo = db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).Descripcion;
                int idEncuesta = (int)db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).IdEncuesta;

                string rango = string.Empty;
                DateTime fechaInicio;
                DateTime fechaFin;

                var qryFiltro = from re in db.RespuestaEncuesta
                                join ep in db.EncuestaPregunta on re.IdPregunta equals ep.Id
                                join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.SiNo
                                      && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                      && aa.Id == idArbolAcceso
                                select new { re, te, ep };

                var qrySi = from q in qryFiltro
                            where q.re.ValorRespuesta == 1
                            select q;

                var qryNo = from q in qryFiltro
                            where q.re.ValorRespuesta == 0
                            select q;

                result.Preguntas = new List<DataTable>();
                foreach (EncuestaPregunta encuestaPregunta in db.EncuestaPregunta.Where(w => w.IdEncuesta == idEncuesta))
                {
                    DataTable dt = new DataTable(encuestaPregunta.Id.ToString());
                    dt.Columns.Add("Descripcion", typeof(string));
                    dt.Columns.Add("Color", typeof(string));
                    dt.Rows.Add("Si", ConfigurationManager.AppSettings["ColorSi"]);
                    dt.Rows.Add("No", ConfigurationManager.AppSettings["Colorno"]);
                    dt.ExtendedProperties.Add("Pregunta", encuestaPregunta.Pregunta);
                    result.Preguntas.Add(dt);
                }

                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));
                dtBarras.Columns.Add("Color", typeof(string));

                dtBarras.Rows.Add("Si", ConfigurationManager.AppSettings["ColorSi"]);
                dtBarras.Rows.Add("No", ConfigurationManager.AppSettings["ColorNo"]);

                if (fechas != null)
                {
                    restaMes = true;
                    fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    DateTime tmpFecha = fechaInicio;
                    bool continua = true;
                    while (continua)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                if (tmpFecha < fechaFin)
                                {
                                    dtBarras.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy"), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        dt.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy"), typeof(int)));
                                    }
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
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy"), typeof(int)));
                                    }
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
                                        dtBarras.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                    }
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
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy")));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.ToString("dd/MM/yyyy")))
                                            dt.Columns.Add(new DataColumn(tmpFecha.ToString("dd/MM/yyyy")));
                                    }
                                    tmpFecha = tmpFecha.AddYears(1);
                                }
                                else
                                    continua = false;
                                rango = "Anual";
                                break;
                        }
                    }

                    qrySi = from q in qrySi
                            where q.te.FechaMovimiento >= fechaInicio
                                   && q.te.FechaMovimiento < fechaFin
                            select q;

                    qryNo = from q in qryNo
                            where q.te.FechaMovimiento >= fechaInicio
                                      && q.te.FechaMovimiento < fechaFin
                            select q;

                    var resultSi = qrySi.Distinct().ToList();
                    var resultNo = qryNo.Distinct().ToList();

                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 2:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                    dtBarras.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null)
                                            && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                            && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                    dtBarras.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                    dtBarras.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                                    {

                                        dt.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(column.ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    restaMes = false;
                    var rateSi = qrySi.Distinct().ToList();
                    var rateNo = qryNo.Distinct().ToList();

                    List<string> lstFechas = qryFiltro.OrderBy(o => o.te.FechaMovimiento).Distinct().ToList().Select(s => s.te.FechaMovimiento.ToString("dd/MM/yyyy")).Distinct().ToList();
                    fechaInicio = DateTime.ParseExact(lstFechas.First(), "dd/MM/yyyy", null);
                    fechaFin = DateTime.ParseExact(lstFechas.Last(), "dd/MM/yyyy", null);
                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (string fecha in lstFechas)
                            {
                                dtBarras.Columns.Add(fecha, typeof(int));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Columns.Add(fecha, typeof(int));
                                }
                            }
                            break;
                        case 2:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.ParseExact(fecha, "dd/MM/yyyy", null), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("dd/MM/yyyy")));
                                }
                                conteo++;
                            }
                            rango = "Semanal";
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                var firstDayOfMonth = new DateTime(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Month, 1);

                                if (!dtBarras.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(firstDayOfMonth.ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(firstDayOfMonth.ToString("dd/MM/yyyy"), typeof(int)));
                                }
                            }
                            rango = "Mensual";
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                DateTime firstDay = new DateTime(DateTime.ParseExact(fecha, "dd/MM/yyyy", null).Year, 1, 1);
                                if (!dtBarras.Columns.Contains(firstDay.ToString("dd/MM/yyyy")))
                                    dtBarras.Columns.Add(new DataColumn(firstDay.ToString("dd/MM/yyyy")));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(firstDay.ToString("dd/MM/yyyy")))
                                        dt.Columns.Add(new DataColumn(firstDay.ToString("dd/MM/yyyy"), typeof(int)));
                                }
                            }
                            rango = "Anual";
                            break;
                    }

                    for (int i = 2; i < dtBarras.Columns.Count; i++)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                dtBarras.Rows[0][i] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                dtBarras.Rows[1][i] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName);
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == dtBarras.Columns[i].ColumnName && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 2:
                                dtBarras.Rows[0][i] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                dtBarras.Rows[1][i] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                    && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null)
                                        && DateTime.ParseExact(w.te.FechaMovimiento.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) < DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).AddDays(7)
                                        && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 3:
                                dtBarras.Rows[0][i] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                dtBarras.Rows[1][i] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 4:
                                dtBarras.Rows[0][i] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                dtBarras.Rows[1][i] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][i] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][i] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == DateTime.ParseExact(dtBarras.Columns[i].ColumnName, "dd/MM/yyyy", null).ToString("yyyy") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                        }

                    }
                }
                result.GraficoBarras = dtBarras;

                switch (tipoFecha)
                {
                    case 1:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.Parse(dtBarras.Columns[i].ColumnName, cultureinfo).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 2:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.Parse(dtBarras.Columns[i].ColumnName, cultureinfo).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 3:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.Parse(dtBarras.Columns[i].ColumnName, cultureinfo).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                    case 4:
                        for (int i = 2; i < dtBarras.Columns.Count; i++)
                        {
                            dtBarras.Columns[i].ColumnName = DateTime.Parse(dtBarras.Columns[i].ColumnName, cultureinfo).ToString("dd MMM yy").Replace(".", string.Empty);
                        }
                        break;
                }

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Color", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));


                dtPie.Rows.Add("Si", ConfigurationManager.AppSettings["ColorSi"]);
                dtPie.Rows.Add("No", ConfigurationManager.AppSettings["ColorNo"]);

                int totalSi = 0;
                int totalNo = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion" && column.ColumnName != "Color")
                    {
                        totalSi += int.Parse(dtBarras.Rows[0][column.ColumnName].ToString());
                        totalNo += int.Parse(dtBarras.Rows[1][column.ColumnName].ToString());
                    }
                }

                dtPie.Rows[0][2] = totalSi;
                dtPie.Rows[1][2] = totalNo;
                result.GraficoPie = dtPie;
                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy"));
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy"));
                        break;
                    case 3:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("MMM"), restaMes ? fechaFin.AddDays(-1).ToString("MMM") : fechaFin.ToString("MMM"));
                        break;
                    case 4:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToString("yyyy"), restaMes ? fechaFin.AddDays(-1).ToString("yyyy") : fechaFin.ToString("yyyy"));
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

        public DataTable ObtenerGraficoLogicaDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas)
        {
            DataTable result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                int idEncuesta = (int)db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).IdEncuesta;

                var qryFiltro = from re in db.RespuestaEncuesta
                                join ep in db.EncuestaPregunta on re.IdPregunta equals ep.Id
                                join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.SiNo
                                      && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                      && aa.Id == idArbolAcceso
                                select new { re, te, ep };


                if (fechas != null)
                {

                    DateTime fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    DateTime fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    if (fechaInicio > fechaFin)
                        throw new Exception("Fechas incorrectas");

                    qryFiltro = from q in qryFiltro
                                where q.te.FechaMovimiento >= fechaInicio
                                       && q.te.FechaMovimiento < fechaFin
                                select q;
                }
                var qry = qryFiltro.ToList();

                result = new DataTable(idEncuesta.ToString());
                result.Columns.Add("Ticket", typeof(int));
                result.Columns.Add("GrupoAtencion", typeof(string));
                result.Columns.Add("AgenteAtendio", typeof(string));
                result.Columns.Add("Usuario", typeof(string));
                result.Columns.Add("FechaCierre", typeof(string));

                foreach (EncuestaPregunta pregunta in db.EncuestaPregunta.Where(w => w.IdEncuesta == idEncuesta))
                {
                    DataColumn col = new DataColumn { ColumnName = pregunta.Pregunta, DataType = typeof(string), DefaultValue = "0" };
                    col.ExtendedProperties.Add("IdPregunta", pregunta.Id);
                    result.Columns.Add(col);
                }


                foreach (var source in qry.Select(s => new { s.te }).Distinct().ToList())
                {
                    int idTicket = (int)source.te.IdTicket;
                    Ticket t = db.Ticket.Single(s => s.Id == idTicket);
                    GrupoUsuario gpoAtencion = (from tgu in db.TicketGrupoUsuario
                                                join gu in db.GrupoUsuario on tgu.IdGrupoUsuario equals gu.Id
                                                where tgu.IdTicket == t.Id && gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente
                                                select gu).Distinct().FirstOrDefault();
                    if (gpoAtencion != null)
                        result.Rows.Add(idTicket, gpoAtencion.Descripcion,
                            t.IdUsuarioUltimoAgenteAsignado != null ? new BusinessUsuarios().ObtenerDetalleUsuario((int)t.IdUsuarioUltimoAgenteAsignado).NombreCompleto : string.Empty,
                            new BusinessUsuarios().ObtenerDetalleUsuario(t.IdUsuarioSolicito).NombreCompleto,
                            source.te.FechaMovimiento);
                }
                foreach (DataRow row in result.Rows)
                {
                    for (int i = 0; i < result.Columns.Count; i++)
                    {
                        if (result.Columns[i].ColumnName != "Ticket" && result.Columns[i].ColumnName != "GrupoAtencion" && result.Columns[i].ColumnName != "AgenteAtendio" && result.Columns[i].ColumnName != "Usuario" && result.Columns[i].ColumnName != "FechaCierre")
                        {

                            int idTicket = int.Parse(row[0].ToString());
                            int idPregunta = (int)result.Columns[i].ExtendedProperties["IdPregunta"];
                            RespuestaEncuesta respuesta =
                                db.RespuestaEncuesta.SingleOrDefault(
                                    w => w.IdPregunta == idPregunta && w.IdTicket == idTicket);
                            if (respuesta != null)
                                row[i] = respuesta.ValorRespuesta;
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
        #endregion Logica

        #endregion Graficos
    }
}

