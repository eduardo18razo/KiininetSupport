using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniHelp.ServiceSistemaEstatus;
using KiiniNet.Entities.Operacion.Usuarios;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;

namespace KiiniHelp.Users.Graficos.Eficiencia
{
    public partial class FrmGraficoTicketsTipificacion : Page
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
                ucFiltrosTicketTipificacion.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                //if (Convert.ToBoolean(hfGraficaGenerada.Value))
                //    UcFiltrosGraficoOnAceptarModal(false);
                //cGraficoPareto.Click += CGraficoOnClick;
                //cGraficoStack.Click += CGraficoOnClick;
                //cGraficoBarra.Click += CGraficoOnClick;
                if (!IsPostBack)
                {
                    ucFiltrosTicketTipificacion.ObtenerParametros();
                    GeneraGraficas();
                }
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


        private void GeneraGraficas()
        {
            try
            {
                upGrafica.Visible = true;

                DataTable dtDatos = _servicioConsultas.GraficarConsultaTicketEficiencia(((Usuario)Session["UserData"]).Id,
                    ucFiltrosTicketTipificacion.FiltroTipoUsuario,
                    ucFiltrosTicketTipificacion.FiltroCategorias,
                    ucFiltrosTicketTipificacion.FiltroGrupos,
                    ucFiltrosTicketTipificacion.FiltroAgentes,
                    ucFiltrosTicketTipificacion.FiltroEstatusAsignacion,
                    ucFiltrosTicketTipificacion.FiltroCanalesApertura,
                    ucFiltrosTicketTipificacion.FiltroTipoArbol,
                    ucFiltrosTicketTipificacion.FiltroTipificacion,
                    ucFiltrosTicketTipificacion.FiltroEstatus,
                    new List<int>(),
                    ucFiltrosTicketTipificacion.FiltroSla,
                    new List<bool?>(),
                    ucFiltrosTicketTipificacion.FiltroOrganizaciones,
                    ucFiltrosTicketTipificacion.FiltroUbicaciones,
                    ucFiltrosTicketTipificacion.FiltroFechas,
                    ucFiltrosTicketTipificacion.FiltroEstatus,
                    "Tipificaciones", ucFiltrosTicketTipificacion.TipoPeriodo);
                GeneraGraficaPareto(rhcTicketsPareto, dtDatos, "Tipificaciones");

                GeneraGraficaStackedPie(rhcTicketsPie, dtDatos);

                GeneraGraficaStackedColumn(rhcTicketsStack, dtDatos);



                hfGraficaGenerada.Value = true.ToString();
                upGrafica.Update();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void UcFiltrosGraficoOnAceptarModal()
        {
            try
            {
                GeneraGraficas();
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
            public string Color { get; set; }
            public string Descripcion { get; set; }
            public decimal Total { get; set; }
            public decimal Acumulado { get; set; }
        }

        private void GeneraGraficaPareto(RadHtmlChart grafico, DataTable dt, string stack)
        {
            try
            {
                switch (ucFiltrosTicketTipificacion.TipoPeriodo)
                {
                    case 1:
                        grafico.ChartTitle.Text = string.Format("Pareto diario: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 2:
                        grafico.ChartTitle.Text = string.Format("Pareto semanal: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 3:
                        grafico.ChartTitle.Text = string.Format("Pareto mensual: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                        break;
                    case 4:
                        grafico.ChartTitle.Text = string.Format("Pareto anual: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                }
                grafico.PlotArea.Series.Clear();
                DataTable dtData = null;
                if (dt != null)
                {
                    List<ParetoArry> lstPareto = (dt.Rows.Cast<DataRow>().Select(dataRow => new ParetoArry
                    {
                        Id = int.Parse(dataRow[0].ToString()),
                        Descripcion = dataRow[1].ToString(),
                        Color = dataRow[2].ToString()
                    })).ToList();

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        decimal total = 0;
                        for (int i = 3; i < dt.Columns.Count; i++)
                        {
                            total += decimal.Parse(dt.Rows[row][i].ToString());
                        }
                        if (lstPareto.Any(s => s.Id == int.Parse(dt.Rows[row][0].ToString())))
                            lstPareto.Single(s => s.Id == int.Parse(dt.Rows[row][0].ToString())).Total = total;
                    }

                    lstPareto = lstPareto.OrderByDescending(o => o.Total).ToList();
                    decimal acumuladoAnterior = 0;
                    var sumaTotales = lstPareto.Sum(s => s.Total);

                    foreach (ParetoArry data in lstPareto)
                    {
                        data.Acumulado = (data.Total / sumaTotales) + acumuladoAnterior;
                        acumuladoAnterior = data.Acumulado;
                    }

                    dtData = ConvertToDataTable(lstPareto);
                    grafico.Width = Unit.Percentage(100);
                    grafico.Height = Unit.Pixel(500);
                    grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
                    grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Vertical;
                    grafico.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";
                    ColumnSeries column = new ColumnSeries { Name = stack, Stacked = false };
                    column.TooltipsAppearance.ClientTemplate = "#= series.name# Total: #= dataItem.value#";
                    column.LabelsAppearance.Visible = false;
                    grafico.PlotArea.XAxis.Items.Clear();
                    foreach (DataRow row in dtData.Rows)
                    {
                        if (row[2].ToString() != "Total")
                        {
                            column.SeriesItems.Add(int.Parse(row[3].ToString()));
                            grafico.PlotArea.XAxis.Items.Add(row[2].ToString());
                        }
                    }
                    grafico.PlotArea.Series.Add(column);

                    //grafico.PlotArea.XAxis.Items.Add(dtData.Columns[2].ColumnName);

                    MakePareto(grafico, dtData, stack);
                }
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
                grafico.PlotArea.AdditionalYAxes.Clear();
                grafico.PlotArea.AdditionalYAxes.Add(new AxisY { Name = "Lines" });
                grafico.PlotArea.AdditionalYAxes[0].MaxValue = 1;
                grafico.PlotArea.AdditionalYAxes[0].LabelsAppearance.DataFormatString = "{0:P}";
                LineSeries lineSerie = new LineSeries();
                lineSerie.LabelsAppearance.DataFormatString = "{0:P}";
                lineSerie.TooltipsAppearance.DataFormatString = "{0:P}";
                lineSerie.Name = "Pareto";
                lineSerie.AxisName = "Lines";
                foreach (DataRow row in dt.Rows)
                {
                    //for (int c = 1; c < dt.Columns.Count; c++)
                    //{
                    lineSerie.SeriesItems.Add((decimal)row[4]);
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
                switch (ucFiltrosTicketTipificacion.TipoPeriodo)
                {
                    case 1:
                        grafico.ChartTitle.Text = string.Format("Tendencia diario: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 2:
                        grafico.ChartTitle.Text = string.Format("Tendencia semanal: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 3:
                        grafico.ChartTitle.Text = string.Format("Tendencia mensual: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                        break;
                    case 4:
                        grafico.ChartTitle.Text = string.Format("Tendencia anual: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                }
                grafico.PlotArea.Series.Clear();
                grafico.PlotArea.XAxis.Items.Clear();
                if (dt != null)
                {
                    dt.Columns.Add(new DataColumn("Totales", typeof(int)));

                    foreach (DataRow fila in dt.Rows)
                    {
                        int suma = 0;
                        for (int i = 3; i < dt.Columns.Count - 2; i++)
                        {
                            suma += int.Parse(fila[i].ToString());
                        }
                        fila[dt.Columns.Count - 1] = suma;
                    }

                    DataView dv = dt.DefaultView;
                    dv.Sort = "Totales desc";
                    dt = dv.ToTable();
                    dt.Columns.Remove("Totales");

                    grafico.Width = Unit.Percentage(100);
                    grafico.Height = Unit.Pixel(500);
                    grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
                    grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Horizontal;
                    foreach (DataRow row in dt.Rows)
                    {
                        ColumnSeries column = new ColumnSeries();
                        column.Name = row[1].ToString();
                        column.Appearance.FillStyle.BackgroundColor = ColorTranslator.FromHtml(row[2].ToString());
                        column.GroupName = "Stacked";
                        column.Stacked = true;

                        column.TooltipsAppearance.ClientTemplate = "#= series.name# Total: #= dataItem.value#";
                        column.LabelsAppearance.Visible = false;
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            column.SeriesItems.Add(int.Parse(row[c].ToString()));
                        }
                        grafico.PlotArea.Series.Add(column);
                    }
                    for (int c = 3; c < dt.Columns.Count; c++)
                    {
                        grafico.PlotArea.XAxis.Items.Add(dt.Columns[c].ColumnName);
                    }
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

        private void GeneraGraficaStackedPie(RadHtmlChart grafico, DataTable dt)
        {
            try
            {
                switch (ucFiltrosTicketTipificacion.TipoPeriodo)
                {
                    case 1:
                        grafico.ChartTitle.Text = string.Format("Total diario: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 2:
                        grafico.ChartTitle.Text = string.Format("Total semanal: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 3:
                        grafico.ChartTitle.Text = string.Format("Total mensual: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                        break;
                    case 4:
                        grafico.ChartTitle.Text = string.Format("Total anual: {0} - {1}",
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketTipificacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketTipificacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                }
                grafico.PlotArea.Series.Clear();
                List<ParetoArry> lstPareto = null;
                if (dt != null)
                {
                    lstPareto = (dt.Rows.Cast<DataRow>().Select(dataRow => new ParetoArry { Id = int.Parse(dataRow[0].ToString()), Descripcion = dataRow[1].ToString(), Color = dataRow[2].ToString() })).ToList();

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        decimal total = 0;
                        for (int i = 3; i < dt.Columns.Count; i++)
                        {
                            total += decimal.Parse(dt.Rows[row][i].ToString());
                        }
                        lstPareto.Single(s => s.Id == int.Parse(dt.Rows[row][0].ToString())).Total = total;
                    }

                    lstPareto = lstPareto.OrderByDescending(o => o.Total).ToList();
                    PieSeries pieSerie = new PieSeries();
                    pieSerie.DataFieldY = "Total";
                    pieSerie.NameField = "Descripcion";
                    pieSerie.ColorField = "Color";
                    pieSerie.LabelsAppearance.Visible = true;
                    pieSerie.LabelsAppearance.Position = PieAndDonutLabelsPosition.Center;
                    grafico.PlotArea.Series.Add(pieSerie);
                }
                grafico.DataSource = lstPareto;
                grafico.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        }

    }
}