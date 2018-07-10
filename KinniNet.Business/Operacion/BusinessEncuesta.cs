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
using KiiniNet.Entities.Reportes;
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

        public HelperReporteEncuesta ObtenerReporteNps(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            HelperReporteEncuesta result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                result = new HelperReporteEncuesta { IdArbolAcceso = idArbolAcceso };
                string titulo = db.InventarioArbolAcceso.Single(s => s.IdArbolAcceso == idArbolAcceso).Descripcion;

                string rango = string.Empty;
                DateTime fechaInicio;
                DateTime fechaFin;

                var qry = from re in db.RespuestaEncuesta
                                    join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                    join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                    where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                          && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                    select new { re, te };

                var qryPromotores = from re in db.RespuestaEncuesta
                                    join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                    join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                    where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                          && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                          && re.ValorRespuesta >= 9
                                    select new { re, te };

                List<int> neutros = new List<int> {7, 8};
                var qryNeutros = from re in db.RespuestaEncuesta
                                 join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                 join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                 where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                       && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                       && neutros.Contains(re.ValorRespuesta)
                                 select new { re, te };

                var qryDetractores = from re in db.RespuestaEncuesta
                                     join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                     join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                     where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                           && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                           && re.ValorRespuesta <= 7
                                     select new { re, te };

                DataTable dtBarras = new DataTable("dt");
                dtBarras.Columns.Add("Descripcion", typeof(string));

                dtBarras.Rows.Add("Promotores; 9 y 10");
                dtBarras.Rows.Add("Neutros; 7 y 8");
                dtBarras.Rows.Add("Detractores; menor a 8");

                if (fechas != null)
                {
                    fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                    fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
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
                                    dtBarras.Rows[0][column.ColumnName] = resultDetractores.Count(w => w.re.IdArbol == idArbolAcceso
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
                    var ratePromotores = qryPromotores.Distinct().ToList();
                    var rateNeutros = qryNeutros.Distinct().ToList();
                    var rateDetractores = qryDetractores.Distinct().ToList();
                    List<string> lstFechas = qry.OrderBy(o => o.te.FechaMovimiento).Distinct().ToList().Select(s => s.te.FechaMovimiento.ToString("dd/MM/yyyy")).Distinct().ToList();
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
                    }
                    foreach (string fecha in lstFechas)
                    {
                        dtBarras.Rows[0][fecha] = ratePromotores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                        dtBarras.Rows[1][fecha] = rateNeutros.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                        dtBarras.Rows[2][fecha] = rateDetractores.Count(w => w.re.IdArbol == idArbolAcceso && w.te.FechaMovimiento.ToString("dd/MM/yyyy") == fecha);
                    }
                }
                result.GraficoBarras = dtBarras;

                DataTable dtPie = new DataTable("dtPie");
                dtPie.Columns.Add("Descripcion", typeof(string));
                dtPie.Columns.Add("Total", typeof(int));

                dtPie.Rows.Add("Promotores; 9 y 10");
                dtPie.Rows.Add("Neutros; 7 y 8");
                dtPie.Rows.Add("Detractores; menor a 8");

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
                titulo += string.Format(" {0} {1} - {2}", rango, fechaInicio.ToShortDateString(), fechaFin.ToShortDateString());
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

