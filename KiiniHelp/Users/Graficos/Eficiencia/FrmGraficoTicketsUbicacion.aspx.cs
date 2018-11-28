using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniHelp.ServiceParametrosSistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;
using Telerik.Web.UI.HtmlChart.Enums;

namespace KiiniHelp.Users.Graficos.Eficiencia
{
    public partial class FrmGraficoTicketsUbicacion : Page
    {
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
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
                ucFiltrosTicketUbicacion.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                if (!IsPostBack)
                {
                    ucFiltrosTicketUbicacion.ObtenerParametros();
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

        private class Top
        {
            public int Id { get; set; }
            public string Descripcion { get; set; }
            public double Total { get; set; }
        }

        private void GeneraGraficas()
        {
            try
            {
                upGrafica.Visible = true;

                DataTable dtDatos = _servicioConsultas.GraficarConsultaTicketEficiencia(((Usuario)Session["UserData"]).Id,
                    ucFiltrosTicketUbicacion.FiltroTipoUsuario,
                    ucFiltrosTicketUbicacion.FiltroCategorias,
                    ucFiltrosTicketUbicacion.FiltroGrupos,
                    ucFiltrosTicketUbicacion.FiltroAgentes,
                    ucFiltrosTicketUbicacion.FiltroEstatusAsignacion,
                    ucFiltrosTicketUbicacion.FiltroCanalesApertura,
                    ucFiltrosTicketUbicacion.FiltroTipoArbol,
                    ucFiltrosTicketUbicacion.FiltroTipificacion,
                    ucFiltrosTicketUbicacion.FiltroEstatus,
                    new List<int>(),
                    ucFiltrosTicketUbicacion.FiltroSla,
                    new List<bool?>(),
                    ucFiltrosTicketUbicacion.FiltroOrganizaciones,
                    ucFiltrosTicketUbicacion.FiltroUbicaciones,
                    ucFiltrosTicketUbicacion.FiltroFechas,
                    ucFiltrosTicketUbicacion.FiltroEstatus,
                    "Ubicaciones", ucFiltrosTicketUbicacion.TipoPeriodo);

                List<ColoresTop> lstColores = _servicioParametros.ObtenerColoresTop();
                List<Top> topResult = new List<Top>();
                foreach (DataRow row in dtDatos.Rows)
                {
                    if (topResult.All(a => a.Id != int.Parse(row[0].ToString())))
                    {
                        double total = 0;
                        for (int i = 3; i < dtDatos.Columns.Count; i++)
                        {
                            total += double.Parse(row[i].ToString());
                        }
                        topResult.Add(new Top { Id = int.Parse(row[0].ToString()), Descripcion = row[1].ToString(), Total = total });
                    }
                }

                topResult = topResult.OrderByDescending(o => o.Total).ToList();

                DataTable dtResultTop = dtDatos.Copy();
                DataTable dtOtros = dtDatos.Copy();

                foreach (Top top in topResult.Skip(20))
                {
                    DataRow[] dr = null;
                    dr = dtResultTop.Select("Id =" + top.Id);
                    foreach (DataRow row in dr)
                    {
                        dtResultTop.Rows.Remove(row);
                    }
                }

                if (topResult.Count > 20)
                {

                    foreach (Top top in topResult.Take(20))
                    {
                        DataRow[] dr = null;
                        dr = dtOtros.Select("Id =" + top.Id);
                        foreach (DataRow row in dr)
                        {
                            dtOtros.Rows.Remove(row);
                        }
                    }

                    dtResultTop.Rows.Add(-1, "Otros");

                    for (int i = 3; i < dtOtros.Columns.Count; i++)
                    {
                        DataRow dr = dtResultTop.Select("Id=-1").FirstOrDefault();
                        if (dr != null)
                        {
                            dr[i] = (from item in dtOtros.AsEnumerable()
                                     select item.Field<int>(dtDatos.Columns[i].ColumnName)).Sum(); ;

                        }
                    }
                }


                int contadorColor = 0;
                foreach (ColoresTop color in lstColores)
                {
                    if (contadorColor < dtResultTop.Rows.Count)
                    {
                        dtResultTop.Rows[contadorColor][2] = color.Color;
                        contadorColor++;
                    }
                    else
                    {
                        break;
                    }
                }

                GeneraGraficaPareto(rhcTicketsPareto, dtResultTop, "Ubicaciones");

                GeneraGraficaStackedPie(rhcTicketsPie, dtResultTop);

                GeneraGraficaStackedColumn(rhcTicketsStack, dtResultTop);



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
                grafico.ChartTitle.Appearance.Visible = false;
                switch (ucFiltrosTicketUbicacion.TipoPeriodo)
                {
                    case 1:
                        lblPiePareto.Text = string.Format("Diario: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 2:
                        lblPiePareto.Text = string.Format("Semanal: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 3:
                        lblPiePareto.Text = string.Format("Mensual: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                        break;
                    case 4:
                        lblPiePareto.Text = string.Format("Anual: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
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
                    grafico.Legend.Appearance.Position = ChartLegendPosition.Right;
                    grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Vertical;
                    grafico.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";
                    ColumnSeries column = new ColumnSeries { Name = stack, Stacked = false };
                    column.ColorField = "Color";

                    column.Appearance.Overlay.Gradient = Gradients.None;
                    column.TooltipsAppearance.ClientTemplate = "#= series.name#: #= dataItem.value#";
                    column.LabelsAppearance.Visible = false;
                    grafico.PlotArea.XAxis.Items.Clear();
                    foreach (DataRow row in dtData.Rows)
                    {
                        if (row[2].ToString() != "Total")
                        {
                            column.SeriesItems.Add(int.Parse(row[3].ToString()), ColorTranslator.FromHtml(row[1].ToString()));
                            grafico.PlotArea.XAxis.Items.Add(row[2].ToString());
                        }
                    }
                    grafico.PlotArea.Series.Add(column);

                    //grafico.PlotArea.XAxis.Items.Add(dtData.Columns[2].ColumnName);

                    MakePareto(grafico, dtData, stack);
                }
                grafico.PlotArea.XAxis.LabelsAppearance.RotationAngle = -45;

                grafico.Legend.Appearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                grafico.Legend.Appearance.TextStyle.FontSize = 10;

                grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontSize = 10;
                grafico.PlotArea.XAxis.LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                grafico.PlotArea.XAxis.LabelsAppearance.TextStyle.FontSize = 10;

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
                grafico.PlotArea.AdditionalYAxes[0].LabelsAppearance.DataFormatString = "{0:P0}";

                grafico.PlotArea.AdditionalYAxes[0].LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                grafico.PlotArea.AdditionalYAxes[0].LabelsAppearance.TextStyle.FontSize = 10;

                LineSeries lineSerie = new LineSeries();

                lineSerie.Appearance.Overlay.Gradient = Gradients.None;
                lineSerie.LabelsAppearance.Position = LineAndScatterLabelsPosition.Below;
                lineSerie.LabelsAppearance.DataFormatString = "{0:P0}";
                lineSerie.TooltipsAppearance.DataFormatString = "{0:P}";
                lineSerie.Name = "Pareto";
                lineSerie.AxisName = "Lines";
                foreach (DataRow row in dt.Rows)
                {
                    lineSerie.SeriesItems.Add((decimal)row[4]);
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
                grafico.ChartTitle.Appearance.Visible = false;
                switch (ucFiltrosTicketUbicacion.TipoPeriodo)
                {
                    case 1:
                        lblPieTendencia.Text = string.Format("Diario: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 2:
                        lblPieTendencia.Text = string.Format("Semanal: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 3:
                        lblPieTendencia.Text = string.Format("Mensual: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                        break;
                    case 4:
                        lblPieTendencia.Text = string.Format("Anual: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
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

                    grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
                    grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Horizontal;
                    foreach (DataRow row in dt.Rows)
                    {
                        ColumnSeries column = new ColumnSeries();

                        column.Appearance.Overlay.Gradient = Gradients.None;
                        column.Name = row[1].ToString();
                        column.Appearance.FillStyle.BackgroundColor = ColorTranslator.FromHtml(row[2].ToString());
                        column.GroupName = "Stacked";
                        column.Stacked = true;

                        column.TooltipsAppearance.ClientTemplate = "#= series.name#: #= dataItem.value#";
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
                grafico.PlotArea.XAxis.LabelsAppearance.RotationAngle = -45;

                grafico.Legend.Appearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                grafico.Legend.Appearance.TextStyle.FontSize = 10;

                grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontSize = 10;
                grafico.PlotArea.XAxis.LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                grafico.PlotArea.XAxis.LabelsAppearance.TextStyle.FontSize = 10;

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
                grafico.ChartTitle.Appearance.Visible = false;
                switch (ucFiltrosTicketUbicacion.TipoPeriodo)
                {
                    case 1:
                        lblPieGeneral.Text = string.Format("Diario: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 2:
                        lblPieGeneral.Text = string.Format("Semanal: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 3:
                        lblPieGeneral.Text = string.Format("Mensual: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                        break;
                    case 4:
                        lblPieGeneral.Text = string.Format("Anual: {0} - {1}",
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? "Historico" : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            ucFiltrosTicketUbicacion.FiltroFechas == null ? string.Empty : ucFiltrosTicketUbicacion.FiltroFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
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
                    DonutSeries donutSerie = new DonutSeries();
                    donutSerie.Appearance.Overlay.Gradient = Gradients.None;
                    donutSerie.ExplodeField = "false";
                    donutSerie.DataFieldY = "Total";
                    donutSerie.NameField = "Descripcion";
                    donutSerie.ColorField = "Color";
                    donutSerie.LabelsAppearance.Visible = true;
                    donutSerie.LabelsAppearance.Position = PieAndDonutLabelsPosition.Center;

                    grafico.Legend.Appearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                    grafico.Legend.Appearance.TextStyle.FontSize = 10;

                    grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                    grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontSize = 10;
                    grafico.PlotArea.XAxis.LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
                    grafico.PlotArea.XAxis.LabelsAppearance.TextStyle.FontSize = 10;

                    grafico.PlotArea.Series.Add(donutSerie);
                }

                //}
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