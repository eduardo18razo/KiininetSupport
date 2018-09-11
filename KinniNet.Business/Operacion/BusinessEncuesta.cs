using System;
using System.Collections.Generic;
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

        public HelperReporteEncuesta ObtenerGraficoNps(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
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

                dtBarras.Rows.Add("Promotores; 9 y 10");
                dtBarras.Rows.Add("Neutros; 7 y 8");
                dtBarras.Rows.Add("Detractores; menor a 7");

                if (fechas != null)
                {
                    restaMes = true;
                    fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                    fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
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
                                    dtBarras.Columns.Add(new DataColumn(tmpFecha.ToShortDateString(), typeof(int)));
                                    tmpFecha = tmpFecha.AddDays(1);
                                }
                                else
                                    continua = false;
                                rango = "Diario";
                                break;
                            case 2:
                                if (tmpFecha < fechaFin)
                                {
                                    if (!dtBarras.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString()))
                                        dtBarras.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString(), typeof(int)));
                                    tmpFecha = tmpFecha.AddDays(7);
                                }
                                else
                                    continua = false;
                                rango = "Semanal";
                                break;
                            case 3:
                                if (tmpFecha < fechaFin)
                                {
                                    if (!dtBarras.Columns.Contains(tmpFecha.Month.ToString()))
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.Month.ToString(), typeof(int)));
                                    tmpFecha = tmpFecha.AddMonths(1);
                                }
                                else
                                    continua = false;
                                rango = "Mensual";
                                break;
                            case 4:
                                if (tmpFecha < fechaFin)
                                {
                                    if (!dtBarras.Columns.Contains(tmpFecha.Year.ToString()))
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.Year.ToString(), typeof(int)));
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
                                if (column.ColumnName != "Descripcion")
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
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPromotores.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[1][column.ColumnName] = resultNeutros.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[2][column.ColumnName] = resultDetractores.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                }
                            }
                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[1][column.ColumnName] = resultNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[2][column.ColumnName] = resultDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[1][column.ColumnName] = resultNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[2][column.ColumnName] = resultDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
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
                    fechaInicio = DateTime.Parse(lstFechas.First());
                    fechaFin = DateTime.Parse(lstFechas.Last());
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
                                if (!dtBarras.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                    dtBarras.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                            }
                            rango = "Semanal";
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                    dtBarras.Columns.Add(new DataColumn(DateTime.Parse(fecha).Month.ToString(), typeof(int)));
                            }
                            rango = "Mensual";
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                    dtBarras.Columns.Add(new DataColumn(DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                            }
                            rango = "Anual";
                            break;
                    }

                    foreach (string fecha in lstFechas)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                dtBarras.Rows[0][fecha] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[1][fecha] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[2][fecha] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                break;
                            case 2:
                                dtBarras.Rows[0]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[1]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[2]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                break;
                            case 3:
                                dtBarras.Rows[0][DateTime.Parse(fecha).Month.ToString()] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[1][DateTime.Parse(fecha).Month.ToString()] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[2][DateTime.Parse(fecha).Month.ToString()] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                break;
                            case 4:
                                dtBarras.Rows[0][DateTime.Parse(fecha).Year.ToString()] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[1][DateTime.Parse(fecha).Year.ToString()] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[2][DateTime.Parse(fecha).Year.ToString()] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                break;
                        }

                    }
                }
                result.GraficoBarras = dtBarras;

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));

                dtPie.Rows.Add("Promotores; 9 y 10");
                dtPie.Rows.Add("Neutros; 7 y 8");
                dtPie.Rows.Add("Detractores; menor a 7");

                int totalPromotores = 0;
                int totalNeutros = 0;
                int totalDetractores = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion")
                    {
                        totalPromotores += (int)dtBarras.Rows[0][column.ColumnName];
                        totalNeutros += (int)dtBarras.Rows[1][column.ColumnName];
                        totalDetractores += (int)dtBarras.Rows[2][column.ColumnName];
                    }
                }

                dtPie.Rows[0][1] = totalPromotores;
                dtPie.Rows[1][1] = totalNeutros;
                dtPie.Rows[2][1] = totalDetractores;
                result.GraficoPie = dtPie;
                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
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

        public HelperReporteEncuesta ObtenerGraficoCalificacion(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
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
                    dt.Rows.Add("0 - 5");
                    dt.Rows.Add("6 - 7");
                    dt.Rows.Add("8 - 9");
                    dt.Rows.Add("10");
                    dt.ExtendedProperties.Add("Pregunta", encuestaPregunta.Pregunta);
                    result.Preguntas.Add(dt);
                }


                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));

                dtBarras.Rows.Add("0 - 5");
                dtBarras.Rows.Add("6 - 7");
                dtBarras.Rows.Add("8 - 9");
                dtBarras.Rows.Add("10");

                if (fechas != null)
                {
                    restaMes = true;
                    fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                    fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
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
                                    dtBarras.Columns.Add(new DataColumn(tmpFecha.ToShortDateString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        dt.Columns.Add(new DataColumn(tmpFecha.ToShortDateString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString()))
                                        dtBarras.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString()))
                                            dt.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains(tmpFecha.Month.ToString()))
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.Month.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.Month.ToString()))
                                            dt.Columns.Add(new DataColumn(tmpFecha.Month.ToString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains(tmpFecha.Year.ToString()))
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.Year.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.Year.ToString()))
                                            dt.Columns.Add(new DataColumn(tmpFecha.Year.ToString(), typeof(int)));
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
                                if (column.ColumnName != "Descripcion")
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
                                    if (column.ColumnName != "Descripcion")
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
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                }
                            }

                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }

                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
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
                    fechaInicio = DateTime.Parse(lstFechas.First());
                    fechaFin = DateTime.Parse(lstFechas.Last());
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
                                if (!dtBarras.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                    dtBarras.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                        dt.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                }
                            }
                            rango = "Semanal";
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                    dtBarras.Columns.Add(new DataColumn(DateTime.Parse(fecha).Month.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                        dt.Columns.Add(new DataColumn(DateTime.Parse(fecha).Month.ToString(), typeof(int)));
                                }
                            }
                            rango = "Mensual";
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                    dtBarras.Columns.Add(new DataColumn(DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                        dt.Columns.Add(new DataColumn(DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                }
                            }
                            rango = "Anual";
                            break;
                    }

                    foreach (string fecha in lstFechas)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                dtBarras.Rows[0][fecha] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[1][fecha] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[2][fecha] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[3][fecha] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][fecha] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][fecha] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][fecha] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][fecha] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 2:
                                dtBarras.Rows[0]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[1]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[2]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[3]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 3:
                                dtBarras.Rows[0][DateTime.Parse(fecha).Month.ToString()] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[1][DateTime.Parse(fecha).Month.ToString()] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[2][DateTime.Parse(fecha).Month.ToString()] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[3][DateTime.Parse(fecha).Month.ToString()] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][DateTime.Parse(fecha).Month.ToString()] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][DateTime.Parse(fecha).Month.ToString()] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][DateTime.Parse(fecha).Month.ToString()] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][DateTime.Parse(fecha).Month.ToString()] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 4:
                                dtBarras.Rows[0][DateTime.Parse(fecha).Year.ToString()] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[1][DateTime.Parse(fecha).Year.ToString()] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[2][DateTime.Parse(fecha).Year.ToString()] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[3][DateTime.Parse(fecha).Year.ToString()] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][DateTime.Parse(fecha).Year.ToString()] = rateCeroCinco.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][DateTime.Parse(fecha).Year.ToString()] = rateSeisSiete.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][DateTime.Parse(fecha).Year.ToString()] = rateOchoNueve.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][DateTime.Parse(fecha).Year.ToString()] = rateDiez.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                        }

                    }
                }
                result.GraficoBarras = dtBarras;

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));

                dtPie.Rows.Add("0 - 5");
                dtPie.Rows.Add("6 - 7");
                dtPie.Rows.Add("8 - 9");
                dtPie.Rows.Add("10");


                int totalCeroCinco = 0;
                int totalSeisSiete = 0;
                int totalOchoNueve = 0;
                int totalDiez = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion")
                    {
                        totalCeroCinco += (int)dtBarras.Rows[0][column.ColumnName];
                        totalSeisSiete += (int)dtBarras.Rows[1][column.ColumnName];
                        totalOchoNueve += (int)dtBarras.Rows[2][column.ColumnName];
                        totalDiez += (int)dtBarras.Rows[3][column.ColumnName];
                    }
                }

                dtPie.Rows[0][1] = totalCeroCinco;
                dtPie.Rows[1][1] = totalSeisSiete;
                dtPie.Rows[2][1] = totalOchoNueve;
                dtPie.Rows[3][1] = totalDiez;
                result.GraficoPie = dtPie;

                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
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

                    DateTime fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                    DateTime fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
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
                            source.te.FechaMovimiento);
                }
                foreach (DataRow row in result.Rows)
                {
                    for (int i = 0; i < result.Columns.Count - 1; i++)
                    {
                        if (result.Columns[i].ColumnName != "Ticket" && result.Columns[i].ColumnName != "GrupoAtencion" && result.Columns[i].ColumnName != "AgenteAtendio" && result.Columns[i].ColumnName != "FechaCierre")
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

        public HelperReporteEncuesta ObtenerGraficoSatisfaccion(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
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
                    dt.Rows.Add("Pesimo");
                    dt.Rows.Add("Malo");
                    dt.Rows.Add("Regular");
                    dt.Rows.Add("Bueno");
                    dt.Rows.Add("Excelente");
                    dt.ExtendedProperties.Add("Pregunta", encuestaPregunta.Pregunta);
                    result.Preguntas.Add(dt);
                }

                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));

                dtBarras.Rows.Add("Pesimo");
                dtBarras.Rows.Add("Malo");
                dtBarras.Rows.Add("Regular");
                dtBarras.Rows.Add("Bueno");
                dtBarras.Rows.Add("Excelente");

                if (fechas != null)
                {
                    restaMes = true;
                    fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                    fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
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
                                    dtBarras.Columns.Add(new DataColumn(tmpFecha.ToShortDateString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        dt.Columns.Add(new DataColumn(tmpFecha.ToShortDateString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString()))
                                        dtBarras.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString()))
                                            dt.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains(tmpFecha.Month.ToString()))
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.Month.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.Month.ToString()))
                                            dt.Columns.Add(new DataColumn(tmpFecha.Month.ToString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains(tmpFecha.Year.ToString()))
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.Year.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.Year.ToString()))
                                            dt.Columns.Add(new DataColumn(tmpFecha.Year.ToString(), typeof(int)));
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
                                if (column.ColumnName != "Descripcion")
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
                                    if (column.ColumnName != "Descripcion")
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
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[4][column.ColumnName] = qryExcelente.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[4][column.ColumnName] = qryExcelente.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[4][column.ColumnName] = qryExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {
                                        dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dtBarras.Rows[4][column.ColumnName] = qryExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {
                                        dtBarras.Rows[0][column.ColumnName] = resultPesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dtBarras.Rows[1][column.ColumnName] = resultMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dtBarras.Rows[2][column.ColumnName] = resultRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dtBarras.Rows[3][column.ColumnName] = resultBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dtBarras.Rows[4][column.ColumnName] = resultExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
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
                    fechaInicio = DateTime.Parse(lstFechas.First());
                    fechaFin = DateTime.Parse(lstFechas.Last());
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
                                if (!dtBarras.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                    dtBarras.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                        dt.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                }
                            }
                            rango = "Semanal";
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                    dtBarras.Columns.Add(new DataColumn(DateTime.Parse(fecha).Month.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                        dt.Columns.Add(new DataColumn(DateTime.Parse(fecha).Month.ToString(), typeof(int)));
                                }
                            }
                            rango = "Mensual";
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                    dtBarras.Columns.Add(new DataColumn(DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                        dt.Columns.Add(new DataColumn(DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                }
                            }
                            rango = "Anual";
                            break;
                    }

                    foreach (string fecha in lstFechas)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                dtBarras.Rows[0][fecha] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[1][fecha] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[2][fecha] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[3][fecha] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[4][fecha] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][fecha] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][fecha] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][fecha] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][fecha] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[4][fecha] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));

                                }
                                break;
                            case 2:
                                dtBarras.Rows[0]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[1]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[2]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[3]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[4]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[4]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 3:
                                dtBarras.Rows[0][DateTime.Parse(fecha).Month.ToString()] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[1][DateTime.Parse(fecha).Month.ToString()] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[2][DateTime.Parse(fecha).Month.ToString()] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[3][DateTime.Parse(fecha).Month.ToString()] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[4][DateTime.Parse(fecha).Month.ToString()] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][DateTime.Parse(fecha).Month.ToString()] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][DateTime.Parse(fecha).Month.ToString()] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][DateTime.Parse(fecha).Month.ToString()] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][DateTime.Parse(fecha).Month.ToString()] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[4][DateTime.Parse(fecha).Month.ToString()] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));

                                }
                                break;
                            case 4:
                                dtBarras.Rows[0][DateTime.Parse(fecha).Year.ToString()] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[1][DateTime.Parse(fecha).Year.ToString()] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[2][DateTime.Parse(fecha).Year.ToString()] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[3][DateTime.Parse(fecha).Year.ToString()] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[4][DateTime.Parse(fecha).Year.ToString()] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][DateTime.Parse(fecha).Year.ToString()] = ratePesimo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][DateTime.Parse(fecha).Year.ToString()] = rateMalo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[2][DateTime.Parse(fecha).Year.ToString()] = rateRegular.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[3][DateTime.Parse(fecha).Year.ToString()] = rateBueno.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[4][DateTime.Parse(fecha).Year.ToString()] = rateExcelente.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                        }

                    }
                }
                result.GraficoBarras = dtBarras;

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));

                dtPie.Rows.Add("Pesimo");
                dtPie.Rows.Add("Malo");
                dtPie.Rows.Add("Regular");
                dtPie.Rows.Add("Bueno");
                dtPie.Rows.Add("Excelente");


                int totalPesimo = 0;
                int totalMalo = 0;
                int totalRegular = 0;
                int totalBueno = 0;
                int totalExcelente = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion")
                    {
                        totalPesimo += (int)dtBarras.Rows[0][column.ColumnName];
                        totalMalo += (int)dtBarras.Rows[1][column.ColumnName];
                        totalRegular += (int)dtBarras.Rows[2][column.ColumnName];
                        totalBueno += (int)dtBarras.Rows[3][column.ColumnName];
                        totalExcelente += (int)dtBarras.Rows[4][column.ColumnName];
                    }
                }

                dtPie.Rows[0][1] = totalPesimo;
                dtPie.Rows[1][1] = totalMalo;
                dtPie.Rows[2][1] = totalRegular;
                dtPie.Rows[3][1] = totalBueno;
                dtPie.Rows[4][1] = totalExcelente;
                result.GraficoPie = dtPie;
                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
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

                    DateTime fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                    DateTime fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
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
                            source.te.FechaMovimiento);
                }
                foreach (DataRow row in result.Rows)
                {
                    for (int i = 0; i < result.Columns.Count - 1; i++)
                    {
                        if (result.Columns[i].ColumnName != "Ticket" && result.Columns[i].ColumnName != "GrupoAtencion" && result.Columns[i].ColumnName != "AgenteAtendio" && result.Columns[i].ColumnName != "FechaCierre")
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

        public HelperReporteEncuesta ObtenerGraficoLogica(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
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
                    dt.Rows.Add("Si");
                    dt.Rows.Add("No");
                    dt.ExtendedProperties.Add("Pregunta", encuestaPregunta.Pregunta);
                    result.Preguntas.Add(dt);
                }

                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));

                dtBarras.Rows.Add("Si");
                dtBarras.Rows.Add("No");

                if (fechas != null)
                {
                    restaMes = true;
                    fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                    fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
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
                                    dtBarras.Columns.Add(new DataColumn(tmpFecha.ToShortDateString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        dt.Columns.Add(new DataColumn(tmpFecha.ToShortDateString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString()))
                                        dtBarras.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString()))
                                            dt.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + tmpFecha.Year.ToString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains(tmpFecha.Month.ToString()))
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.Month.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.Month.ToString()))
                                            dt.Columns.Add(new DataColumn(tmpFecha.Month.ToString(), typeof(int)));
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
                                    if (!dtBarras.Columns.Contains(tmpFecha.Year.ToString()))
                                        dtBarras.Columns.Add(new DataColumn(tmpFecha.Year.ToString(), typeof(int)));
                                    foreach (DataTable dt in result.Preguntas)
                                    {
                                        if (!dt.Columns.Contains(tmpFecha.Year.ToString()))
                                            dt.Columns.Add(new DataColumn(tmpFecha.Year.ToString(), typeof(int)));
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
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                    dtBarras.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == column.ColumnName);
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
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
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                    dtBarras.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(column.ColumnName.Split(' ')[3]), int.Parse(column.ColumnName.Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 3:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                    dtBarras.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0'));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {
                                        dt.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == column.ColumnName.PadLeft(2, '0') && w.ep.Id == int.Parse(dt.TableName));
                                    }
                                }
                            }
                            break;
                        case 4:
                            foreach (DataColumn column in dtBarras.Columns)
                            {
                                if (column.ColumnName != "Descripcion")
                                {
                                    dtBarras.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                    dtBarras.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0'));
                                }
                            }
                            foreach (DataTable dt in result.Preguntas)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName != "Descripcion")
                                    {

                                        dt.Rows[0][column.ColumnName] = resultSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
                                        dt.Rows[1][column.ColumnName] = resultNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yyyy") == column.ColumnName.PadLeft(4, '0') && w.ep.Id == int.Parse(dt.TableName));
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
                    fechaInicio = DateTime.Parse(lstFechas.First());
                    fechaFin = DateTime.Parse(lstFechas.Last());
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
                                if (!dtBarras.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                    dtBarras.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                        dt.Columns.Add(new DataColumn("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                }
                            }
                            rango = "Semanal";
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                    dtBarras.Columns.Add(new DataColumn(DateTime.Parse(fecha).Month.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                        dt.Columns.Add(new DataColumn(DateTime.Parse(fecha).Month.ToString(), typeof(int)));
                                }
                            }
                            rango = "Mensual";
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                if (!dtBarras.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                    dtBarras.Columns.Add(new DataColumn(DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    if (!dt.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                        dt.Columns.Add(new DataColumn(DateTime.Parse(fecha).Year.ToString(), typeof(int)));
                                }
                            }
                            rango = "Anual";
                            break;
                    }

                    foreach (string fecha in lstFechas)
                    {
                        switch (tipoFecha)
                        {
                            case 1:
                                dtBarras.Rows[0][fecha] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                dtBarras.Rows[1][fecha] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][fecha] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][fecha] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 2:
                                dtBarras.Rows[0]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                dtBarras.Rows[1]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                        && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1]["SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1]))
                                            && DateTime.Parse(w.te.FechaMovimiento.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[3]), int.Parse(("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()).ToString().Split(' ')[1])) && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 3:
                                dtBarras.Rows[0][DateTime.Parse(fecha).Month.ToString()] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                dtBarras.Rows[1][DateTime.Parse(fecha).Month.ToString()] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][DateTime.Parse(fecha).Month.ToString()] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][DateTime.Parse(fecha).Month.ToString()] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("MM") == DateTime.Parse(fecha.ToString()).ToString("MM") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                            case 4:
                                dtBarras.Rows[0][DateTime.Parse(fecha).Year.ToString()] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                dtBarras.Rows[1][DateTime.Parse(fecha).Year.ToString()] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy"));
                                foreach (DataTable dt in result.Preguntas)
                                {
                                    dt.Rows[0][DateTime.Parse(fecha).Year.ToString()] = rateSi.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                    dt.Rows[1][DateTime.Parse(fecha).Year.ToString()] = rateNo.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("yy") == DateTime.Parse(fecha.ToString()).ToString("yy") && w.ep.Id == int.Parse(dt.TableName));
                                }
                                break;
                        }

                    }
                }
                result.GraficoBarras = dtBarras;

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));


                dtPie.Rows.Add("Si");
                dtPie.Rows.Add("No");

                int totalSi = 0;
                int totalNo = 0;
                foreach (DataColumn column in dtBarras.Columns)
                {
                    if (column.ColumnName != "Descripcion")
                    {
                        totalSi += (int)dtBarras.Rows[0][column.ColumnName];
                        totalNo += (int)dtBarras.Rows[1][column.ColumnName];
                    }
                }

                dtPie.Rows[0][1] = totalSi;
                dtPie.Rows[1][1] = totalNo;
                result.GraficoPie = dtPie;
                switch (tipoFecha)
                {
                    case 1:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
                        break;
                    case 2:
                        titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
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

                    DateTime fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                    DateTime fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
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
                            source.te.FechaMovimiento);
                }
                foreach (DataRow row in result.Rows)
                {
                    for (int i = 0; i < result.Columns.Count - 1; i++)
                    {
                        if (result.Columns[i].ColumnName != "Ticket" && result.Columns[i].ColumnName != "GrupoAtencion" && result.Columns[i].ColumnName != "AgenteAtendio" && result.Columns[i].ColumnName != "FechaCierre")
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
    }
}

