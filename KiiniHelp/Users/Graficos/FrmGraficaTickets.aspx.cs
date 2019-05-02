using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;

namespace KiiniHelp.Users.Graficos
{
    public partial class FrmGraficaTickets : Page
    {
        private readonly ServiceConsultasClient _servicioConsultas = new ServiceConsultasClient();

        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
        {
            set
            {
                if (value.Any())
                {
                    string error = value.Aggregate("<ul>", (current, s) => current + ("<li>" + s + "</li>"));
                    error += "</ul>";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('Error','" + error + "');", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                ucFiltrosGrafico.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucFiltrosGrafico.OnCancelarModal += UcFiltrosGraficoOnOnCancelarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                if (Convert.ToBoolean(hfGraficaGenerada.Value))
                    UcFiltrosGraficoOnAceptarModal(false);
                cGrafico.Click += CGraficoOnClick;

                ucFiltrosGraficas.OnAceptarModal += ucFiltrosGraficas_OnAceptarModal;


                //.OnAceptarModal += ucFiltrosGraficasHits_OnAceptarModal;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }


        private void CGraficoOnClick(object sender, ImageMapEventArgs imageMapEventArgs)
        {
            try
            {
                List<HelperReportesTicket> lstConsulta = _servicioConsultas.ConsultarTickets(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroCanalesApertura, ucFiltrosGraficas.FiltroTipoUsuario, ucFiltrosGraficas.FiltroOrganizaciones,
                    ucFiltrosGraficas.FiltroUbicaciones, ucFiltrosGraficas.FiltroTipoArbol, ucFiltrosGraficas.FiltroTipificaciones,
                    ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGraficas.FiltroEstatus, ucFiltrosGraficas.FiltroSla, ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, 0, 1000);
                string[] selectedData = imageMapEventArgs.PostBackValue.ToString().Split(',');
                string fecha = selectedData[0];
                string total = selectedData[1];
                int idFiltro = int.Parse(selectedData[2]);
                switch (ucFiltrosGrafico.TipoGrafico)
                {
                    case "Geografico":
                        break;
                    case "Pareto":
                        switch (ucFiltrosGrafico.Stack)
                        {
                            case "Ubicaciones":
                                lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro).ToList();
                                break;
                            case "Organizaciones":

                                lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro).ToList();
                                break;
                            case "Tipificaciones":
                                lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro).ToList();
                                break;
                            case "Tipo Ticket":
                                lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro).ToList();
                                break;
                            case "Estatus Ticket":
                                lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro).ToList();
                                break;
                            case "SLA":
                                lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera")).ToList();
                                break;
                        }
                        break;
                    case "Tendencia Stack":
                    case "Tendencia Barra Comparativa":
                        switch (ucFiltrosGrafico.Stack)
                        {
                            case "Ubicaciones":
                                switch (ucFiltrosGraficas.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Organizaciones":
                                switch (ucFiltrosGraficas.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Tipo Ticket":
                                switch (ucFiltrosGraficas.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Tipificaciones":
                                switch (ucFiltrosGraficas.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Estatus Ticket":
                                switch (ucFiltrosGraficas.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "SLA":
                                switch (ucFiltrosGraficas.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera") && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera") && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera") && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera") && DateTime.ParseExact(w.FechaHora, "dd/MM/yyyy", null).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                        }
                        break;
                }
                gvResult.DataSource = lstConsulta;
                gvResult.DataBind();
                rhcTickets.DataSource = lstConsulta;
                rhcTickets.DataBind();

                upDetalleGrafico.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleGrafico\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
        private void UcFiltrosGraficoOnOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroParametros\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private void UcFiltrosGraficoOnAceptarModal()
        {
            try
            {
                switch (ucFiltrosGrafico.TipoGrafico)
                {
                    case "Geografico":
                        frameGeoCharts.Attributes.Add("src", ResolveUrl("FrmGeoChart.aspx?Data=" + _servicioConsultas.GraficarConsultaTicketGeografico(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo)));
                        frameGeoCharts.Visible = true;
                        cGrafico.Visible = false;
                        upGrafica.Visible = true;
                        break;
                    case "Pareto":
                        BusinessGraficoStack.Pareto.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo), ucFiltrosGrafico.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        GeneraGraficaPareto(rhcTickets, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo), ucFiltrosGrafico.Stack);
                        break;
                    case "Tendencia Stack":
                        BusinessGraficoStack.Stack.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo), ucFiltrosGrafico.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        GeneraGraficaStackedColumn(rhcTickets, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo));
                        break;
                    case "Tendencia Barra Comparativa":
                        BusinessGraficoStack.ColumnsClustered.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo), ucFiltrosGrafico.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                }
                hfGraficaGenerada.Value = true.ToString();
                upGrafica.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroParametros\");", true);

            }

            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        private class ParetoArry
        {
            public int Id { get; set; }
            public string Descripcion { get; set; }
            public decimal Total { get; set; }
            public decimal Acumulado { get; set; }
        }

        private void GeneraGraficaPareto(RadHtmlChart grafico, DataTable dt, string stack)
        {
            try
            {
                List<ParetoArry> lstPareto = (dt.Rows.Cast<DataRow>().Select(dataRow => new ParetoArry { Id = int.Parse(dataRow[0].ToString()), Descripcion = dataRow[1].ToString() })).ToList();

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    decimal total = 0;
                    for (int i = 2; i < dt.Columns.Count; i++)
                    {
                        total += decimal.Parse(dt.Rows[row][i].ToString());
                    }
                    lstPareto.Single(s => s.Descripcion == dt.Rows[row][1].ToString()).Total = total;
                }

                lstPareto = lstPareto.OrderByDescending(o => o.Total).ToList();
                decimal acumuladoAnterior = 0;
                foreach (ParetoArry data in lstPareto)
                {
                    data.Acumulado = ((data.Total / lstPareto.Sum(s => s.Total)) * 100) + acumuladoAnterior;
                    acumuladoAnterior += data.Acumulado;
                }

                DataTable dtData = ConvertToDataTable(lstPareto);
                grafico.Width = Unit.Percentage(100);
                grafico.Height = Unit.Pixel(500);
                grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
                grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Vertical;
                ColumnSeries column = new ColumnSeries();
                column.Name = stack;
                column.Stacked = false;
                column.TooltipsAppearance.ClientTemplate = "#= series.name# Total: #= dataItem.value#";
                column.LabelsAppearance.Visible = false;
                foreach (DataRow row in dtData.Rows)
                {
                    
                    column.SeriesItems.Add(int.Parse(row[2].ToString()));
                    
                }
                grafico.PlotArea.Series.Add(column);

                grafico.PlotArea.XAxis.Items.Add(dtData.Columns[2].ColumnName);
                
                MakePareto(grafico, dtData, stack);

                grafico.PlotArea.XAxis.LabelsAppearance.RotationAngle = 270;
                grafico.PlotArea.XAxis.MajorGridLines.Width = 0;
                grafico.PlotArea.XAxis.MinorGridLines.Width = 0;
                grafico.PlotArea.YAxis.MajorGridLines.Width = 0;
                grafico.PlotArea.YAxis.MinorGridLines.Width = 0;
                grafico.DataSource = dtData;
                grafico.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void MakePareto(RadHtmlChart grafico, DataTable dt, string name)
        {
            try
            {
                grafico.PlotArea.AdditionalYAxes.Add(new AxisY {Name = "Lines"});
                LineSeries lineSerie = new LineSeries();
                lineSerie.Name = name;
                lineSerie.AxisName = "Lines";
                foreach (DataRow row in dt.Rows)
                {
                    //for (int c = 1; c < dt.Columns.Count; c++)
                    //{
                        lineSerie.SeriesItems.Add((decimal)row[3]);
                    //}
                }

                grafico.PlotArea.Series.Add(lineSerie);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GeneraGraficaStackedColumn(RadHtmlChart grafico, DataTable dt)
        {
            try
            {
                grafico.Width = Unit.Percentage(100);
                grafico.Height = Unit.Pixel(500);
                grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
                grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Vertical;
                foreach (DataRow row in dt.Rows)
                {
                    ColumnSeries column = new ColumnSeries();
                    column.Name = row[1].ToString();
                    column.GroupName = "Stacked";
                    column.Stacked = true;
                    column.TooltipsAppearance.ClientTemplate = "#= series.name# Total: #= dataItem.value#";
                    column.LabelsAppearance.Visible = false;
                    for (int c = 2; c < dt.Columns.Count; c++)
                    {
                        column.SeriesItems.Add(int.Parse(row[c].ToString()));
                    }
                    grafico.PlotArea.Series.Add(column);
                }
                for (int c = 2; c < dt.Columns.Count; c++)
                {
                    grafico.PlotArea.XAxis.Items.Add(dt.Columns[c].ColumnName);
                }
                grafico.PlotArea.XAxis.LabelsAppearance.RotationAngle = 270;
                grafico.PlotArea.XAxis.MajorGridLines.Width = 0;
                grafico.PlotArea.XAxis.MinorGridLines.Width = 0;
                grafico.PlotArea.YAxis.MajorGridLines.Width = 0;
                grafico.PlotArea.YAxis.MinorGridLines.Width = 0;
                grafico.DataSource = dt;
                grafico.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void UcFiltrosGraficoOnAceptarModal(bool cierraModal)
        {
            try
            {
                switch (ucFiltrosGrafico.TipoGrafico)
                {
                    case "Geografico":
                        frameGeoCharts.Attributes.Add("src", ResolveUrl("FrmGeoChart.aspx?Data=" + _servicioConsultas.GraficarConsultaTicketGeografico(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo)));
                        frameGeoCharts.Visible = true;
                        cGrafico.Visible = false;
                        upGrafica.Visible = true;
                        break;
                    case "Pareto":
                        BusinessGraficoStack.Pareto.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo), ucFiltrosGrafico.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Stack":
                        BusinessGraficoStack.Stack.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo), ucFiltrosGrafico.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Barra Comparativa":
                        BusinessGraficoStack.ColumnsClustered.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroTipoUsuario,
                            ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
                            ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo), ucFiltrosGrafico.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                }
                hfGraficaGenerada.Value = true.ToString();
                upGrafica.Update();
                if (cierraModal)
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroParametros\");", true);

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private void UcDetalleGeograficoTicketsOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleGeografico\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnConsultar_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    List<HelperReportesTicket> lstConsulta = _servicioConsultas.ConsultarTickets(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroCanalesApertura, ucFiltrosGraficas.FiltroTipoUsuario, ucFiltrosGraficas.FiltroOrganizaciones,
            //        ucFiltrosGraficas.FiltroUbicaciones, ucFiltrosGraficas.FiltroTipoArbol, ucFiltrosGraficas.FiltroTipificaciones,
            //        ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGraficas.FiltroEstatus, ucFiltrosGraficas.FiltroSla, ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, 0, 1000);
            //    if (lstConsulta == null) throw new Exception("No existen datos a graficar");
            //    ucFiltrosGrafico.UbicacionesGraficar = lstConsulta.GroupBy(g => new { g.IdUbicacion, g.Ubicacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdUbicacion, Descripcion = s.Key.Ubicacion, Total = s.Count() }).ToList();
            //    ucFiltrosGrafico.OrganizacionesGraficar = lstConsulta.GroupBy(g => new { g.IdOrganizacion, g.Organizacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdOrganizacion, Descripcion = s.Key.Organizacion, Total = s.Count() }).ToList();
            //    ucFiltrosGrafico.TipoTicket = lstConsulta.GroupBy(g => new { g.IdServicioIncidente, g.ServicioIncidente }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdServicioIncidente, Descripcion = s.Key.ServicioIncidente, Total = s.Count() }).ToList();
            //    ucFiltrosGrafico.TipificacionesGraficar = lstConsulta.GroupBy(g => new { g.IdTipificacion, g.Tipificacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdTipificacion, Descripcion = s.Key.Tipificacion, Total = s.Count() }).ToList();
            //    ucFiltrosGrafico.EstatusStack = lstConsulta.GroupBy(g => new { g.IdEstatus, g.Estatus }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdEstatus, Descripcion = s.Key.Estatus, Total = s.Count() }).ToList();
            //    ucFiltrosGrafico.SlaGraficar = lstConsulta.GroupBy(g => new { g.DentroSla, g.Sla }).ToDictionary(var => var.Key.DentroSla, var => var.Key.Sla);
            //    ucFiltrosGrafico.CanalGraficar = lstConsulta.GroupBy(g => new { g.IdCanal, g.Canal }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdCanal, Descripcion = s.Key.Canal, Total = s.Count() }).ToList();
            //    gvResult.DataSource = lstConsulta;
            //    gvResult.DataBind();
            //    hfGraficaGenerada.Value = false.ToString();
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroParametros\");", true);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    AlertaGeneral = _lstError;
            //}
        }

        protected void btnDetalleGeografico_OnClick(object sender, EventArgs e)
        {
            try
            {
                try
                {

                    List<HelperReportesTicket> lstConsulta = _servicioConsultas.ConsultarTickets(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroCanalesApertura, ucFiltrosGraficas.FiltroTipoUsuario, ucFiltrosGraficas.FiltroOrganizaciones,
                    ucFiltrosGraficas.FiltroUbicaciones, ucFiltrosGraficas.FiltroTipoArbol, ucFiltrosGraficas.FiltroTipificaciones,
                    ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGraficas.FiltroEstatus, ucFiltrosGraficas.FiltroSla, ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, 0, 1000);
                    if (lstConsulta == null) return;
                    ucDetalleGeograficoTickets.UbicacionesGraficar = lstConsulta.GroupBy(g => new { g.IdUbicacion, g.Ubicacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdUbicacion, Descripcion = s.Key.Ubicacion, Total = s.Count() }).ToList();
                    ucDetalleGeograficoTickets.OrganizacionesGraficar = lstConsulta.GroupBy(g => new { g.IdOrganizacion, g.Organizacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdOrganizacion, Descripcion = s.Key.Organizacion, Total = s.Count() }).ToList();
                    ucDetalleGeograficoTickets.TipoTicket = lstConsulta.GroupBy(g => new { g.IdServicioIncidente, g.ServicioIncidente }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdServicioIncidente, Descripcion = s.Key.ServicioIncidente, Total = s.Count() }).ToList();
                    ucDetalleGeograficoTickets.TipificacionesGraficar = lstConsulta.GroupBy(g => new { g.IdTipificacion, g.Tipificacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdTipificacion, Descripcion = s.Key.Tipificacion, Total = s.Count() }).ToList();
                    ucDetalleGeograficoTickets.EstatusStack = lstConsulta.GroupBy(g => new { g.IdEstatus, g.Estatus }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdEstatus, Descripcion = s.Key.Estatus, Total = s.Count() }).ToList();
                    ucDetalleGeograficoTickets.SlaGraficar = lstConsulta.GroupBy(g => new { g.DentroSla, g.Sla }).ToDictionary(var => var.Key.DentroSla, var => var.Key.Sla);
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleGeografico\");", true);
                }
                catch (Exception ex)
                {
                    if (_lstError == null)
                    {
                        _lstError = new List<string>();
                    }
                    _lstError.Add(ex.Message);
                    AlertaGeneral = _lstError;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleGeografico\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnCerrar_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleGrafico\");", true);
        }

        private void ucFiltrosGraficas_OnAceptarModal()
        {
            try
            {
                List<HelperReportesTicket> lstConsulta = _servicioConsultas.ConsultarTickets(((Usuario)Session["UserData"]).Id, ucFiltrosGraficas.FiltroGrupos, ucFiltrosGraficas.FiltroCanalesApertura, ucFiltrosGraficas.FiltroTipoUsuario, ucFiltrosGraficas.FiltroOrganizaciones,
                    ucFiltrosGraficas.FiltroUbicaciones, ucFiltrosGraficas.FiltroTipoArbol, ucFiltrosGraficas.FiltroTipificaciones,
                    ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGraficas.FiltroEstatus, ucFiltrosGraficas.FiltroSla, ucFiltrosGraficas.FiltroVip, ucFiltrosGraficas.FiltroFechas, 0, 1000);
                if (lstConsulta == null) throw new Exception("No existen datos a graficar");
                ucFiltrosGrafico.UbicacionesGraficar = lstConsulta.GroupBy(g => new { g.IdUbicacion, g.Ubicacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdUbicacion, Descripcion = s.Key.Ubicacion, Total = s.Count() }).ToList();
                ucFiltrosGrafico.OrganizacionesGraficar = lstConsulta.GroupBy(g => new { g.IdOrganizacion, g.Organizacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdOrganizacion, Descripcion = s.Key.Organizacion, Total = s.Count() }).ToList();
                ucFiltrosGrafico.TipoTicket = lstConsulta.GroupBy(g => new { g.IdServicioIncidente, g.ServicioIncidente }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdServicioIncidente, Descripcion = s.Key.ServicioIncidente, Total = s.Count() }).ToList();
                ucFiltrosGrafico.TipificacionesGraficar = lstConsulta.GroupBy(g => new { g.IdTipificacion, g.Tipificacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdTipificacion, Descripcion = s.Key.Tipificacion, Total = s.Count() }).ToList();
                ucFiltrosGrafico.EstatusStack = lstConsulta.GroupBy(g => new { g.IdEstatus, g.Estatus }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdEstatus, Descripcion = s.Key.Estatus, Total = s.Count() }).ToList();
                ucFiltrosGrafico.SlaGraficar = lstConsulta.GroupBy(g => new { g.DentroSla, g.Sla }).ToDictionary(var => var.Key.DentroSla, var => var.Key.Sla);
                ucFiltrosGrafico.CanalGraficar = lstConsulta.GroupBy(g => new { g.IdCanal, g.Canal }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdCanal, Descripcion = s.Key.Canal, Total = s.Count() }).ToList();
                gvResult.DataSource = lstConsulta;
                gvResult.DataBind();
                hfGraficaGenerada.Value = false.ToString();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroParametros\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

    }
}