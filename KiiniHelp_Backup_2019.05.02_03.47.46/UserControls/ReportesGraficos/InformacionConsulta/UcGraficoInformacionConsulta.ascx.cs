using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniNet.Entities.Reportes;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;
using Telerik.Web.UI.HtmlChart.Enums;

namespace KiiniHelp.UserControls.ReportesGraficos.InformacionConsulta
{
    public partial class UcGraficoInformacionConsulta : UserControl
    {
        private readonly ServiceInformacionConsultaClient _servicioInformacionConsulta = new ServiceInformacionConsultaClient();
        private List<string> _lstError = new List<string>();

        public List<string> Alerta
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
        private void GeneraGraficaStackedColumn(RadHtmlChart grafico, DataTable dt)
        {
            switch (ucFiltroFechasGrafico.TipoPeriodo)
            {
                case 1:
                    lblPieTendencia.Text = string.Format("Diario: {0} - {1}",
                        ucFiltroFechasGrafico.RangoFechas == null ? "Historico" : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                        ucFiltroFechasGrafico.RangoFechas == null ? string.Empty : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                    break;
                case 2:
                    lblPieTendencia.Text = string.Format("Semanal: {0} - {1}",
                        ucFiltroFechasGrafico.RangoFechas == null ? "Historico" : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                        ucFiltroFechasGrafico.RangoFechas == null ? string.Empty : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                    break;
                case 3:
                    lblPieTendencia.Text = string.Format("Mensual: {0} - {1}",
                        ucFiltroFechasGrafico.RangoFechas == null ? "Historico" : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                        ucFiltroFechasGrafico.RangoFechas == null ? string.Empty : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                    break;
                case 4:
                    lblPieTendencia.Text = string.Format("Anual: {0} - {1}",
                        ucFiltroFechasGrafico.RangoFechas == null ? "Historico" : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                        ucFiltroFechasGrafico.RangoFechas == null ? string.Empty : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                    break;
            }

            grafico.PlotArea.Series.Clear();
            grafico.PlotArea.XAxis.Items.Clear();

            grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
            grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Horizontal;
            foreach (DataRow row in dt.Rows)
            {
                ColumnSeries column = new ColumnSeries();
                column.Appearance.Overlay.Gradient = Gradients.None;
                column.Name = row[0].ToString();
                column.GroupName = "Likes";
                column.Stacked = true;
                column.Appearance.FillStyle.BackgroundColor = ColorTranslator.FromHtml(row[1].ToString());
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
        private void GeneraGraficaPie(RadHtmlChart grafico, DataTable dt)
        {
            switch (ucFiltroFechasGrafico.TipoPeriodo)
            {
                case 1:
                    lblPieGeneral.Text = string.Format("Diario: {0} - {1}",
                        ucFiltroFechasGrafico.RangoFechas == null ? "Historico" : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                        ucFiltroFechasGrafico.RangoFechas == null ? string.Empty : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                    break;
                case 2:
                    lblPieGeneral.Text = string.Format("Semanal: {0} - {1}",
                        ucFiltroFechasGrafico.RangoFechas == null ? "Historico" : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                        ucFiltroFechasGrafico.RangoFechas == null ? string.Empty : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                    break;
                case 3:
                    lblPieGeneral.Text = string.Format("Mensual: {0} - {1}",
                        ucFiltroFechasGrafico.RangoFechas == null ? "Historico" : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                        ucFiltroFechasGrafico.RangoFechas == null ? string.Empty : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                    break;
                case 4:
                    lblPieGeneral.Text = string.Format("Anual: {0} - {1}",
                        ucFiltroFechasGrafico.RangoFechas == null ? "Historico" : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                        ucFiltroFechasGrafico.RangoFechas == null ? string.Empty : ucFiltroFechasGrafico.RangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                    break;
            }
            grafico.PlotArea.Series.Clear();
            grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;

            DonutSeries pieSerie = new DonutSeries();
            pieSerie.Appearance.Overlay.Gradient = Gradients.None;
            pieSerie.ExplodeField = "false";
            pieSerie.DataFieldY = "Total";
            pieSerie.NameField = "Descripcion";
            pieSerie.ColorField = "Color";
            pieSerie.LabelsAppearance.Visible = true;
            pieSerie.LabelsAppearance.Position = PieAndDonutLabelsPosition.Center;

            grafico.Legend.Appearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
            grafico.Legend.Appearance.TextStyle.FontSize = 10;

            grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
            grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontSize = 10;
            grafico.PlotArea.XAxis.LabelsAppearance.TextStyle.FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
            grafico.PlotArea.XAxis.LabelsAppearance.TextStyle.FontSize = 10;

            grafico.PlotArea.Series.Add(pieSerie);
            grafico.DataSource = dt;
            grafico.DataBind();
        }

        private void LLenaDatos(int idInformacion, int idTipoFecha, string fechaInicio, string fechafin)
        {
            try
            {

                ReporteInformacionConsulta reporte = _servicioInformacionConsulta.ObtenerReporteInformacionConsulta(idInformacion, Metodos.ManejoFechas.ObtenerFechas(idTipoFecha, fechaInicio, fechafin), idTipoFecha);
                lblTitulo.Text = reporte.Titulo;
                GeneraGraficaStackedColumn(rhcLikeBarra, reporte.GraficoBarras);
                GeneraGraficaPie(rhcLikePie, reporte.GraficoPie);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.Params["idInformacion"] != null && Request.Params["tipoFecha"] != null && Request.Params["fi"] != null && Request.Params["ff"] != null)
                    {
                        ucFiltroFechasGrafico.TipoPeriodo = int.Parse(Request.Params["tipoFecha"]);
                        ucFiltroFechasGrafico.FechaInicio = Metodos.ManejoFechas.ObtenerFechas(ucFiltroFechasGrafico.TipoPeriodo, Request.Params["fi"], Request.Params["ff"]).Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"); ;
                        ucFiltroFechasGrafico.FechaFin = Metodos.ManejoFechas.ObtenerFechas(ucFiltroFechasGrafico.TipoPeriodo, Request.Params["fi"], Request.Params["ff"]).Single(s => s.Key == "fin").Value.ToString("dd/MM/yyyy"); ;
                    }

                    if (Request.Params["idInformacion"] != null && Request.Params["tipoFecha"] != null && Request.Params["fi"] != null && Request.Params["ff"] != null)
                    {
                        LLenaDatos(int.Parse(Request.Params["idInformacion"]), int.Parse(Request.Params["tipoFecha"]), Request.Params["fi"], Request.Params["ff"]);
                    }
                    else if (Request.Params["idInformacion"] != null && Request.Params["tipoFecha"] != null)
                        LLenaDatos(int.Parse(Request.Params["idInformacion"]), int.Parse(Request.Params["tipoFecha"]), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ucFiltroFechasGrafico.RangoFechas != null)
                    LLenaDatos(int.Parse(Request.Params["idInformacion"]), ucFiltroFechasGrafico.TipoPeriodo, ucFiltroFechasGrafico.FechaInicio, ucFiltroFechasGrafico.FechaFin);
                else
                    LLenaDatos(int.Parse(Request.Params["idInformacion"]), ucFiltroFechasGrafico.TipoPeriodo, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }
    }
}