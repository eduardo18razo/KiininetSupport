using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceEncuesta;
using KiiniNet.Entities.Helper.Reportes;
using KinniNet.Business.Utils;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;
using Telerik.Web.UI.HtmlChart.Enums;

namespace KiiniHelp.UserControls.ReportesGraficos.Encuestas
{
    public partial class UcGraficoCalificaciones : UserControl
    {
        private readonly ServiceEncuestaClient _servicioEncuesta = new ServiceEncuestaClient();
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
        private void GeneraGraficaStackedColumn(RadHtmlChart grafico, DataTable dt, string titulo)
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
        private void GeneraGraficaPie(RadHtmlChart grafico, DataTable dt, string titulo)
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

        private void LLenaDatos(int idArbol, int idTipoFecha, string fechaInicio, string fechafin)
        {
            try
            {
                HelperReporteEncuesta reporte = _servicioEncuesta.ObtenerGraficoCalificacion(idArbol, Metodos.ManejoFechas.ObtenerFechas(idTipoFecha, fechaInicio, fechafin), idTipoFecha);
                lblTitulo.Text = reporte.Titulo;
                GeneraGraficaStackedColumn(rhcLikeBarra, reporte.GraficoBarras, "Totales");
                GeneraGraficaPie(rhcLikePie, reporte.GraficoPie, "Totales");
                rptPreguntas.DataSource = reporte.Preguntas;
                rptPreguntas.DataBind();
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
                    if (Request.Params["tipoFecha"] != null && Request.Params["fi"] != null && Request.Params["ff"] != null)
                    {
                        ucFiltroFechasGrafico.TipoPeriodo = int.Parse(Request.Params["tipoFecha"]);
                        ucFiltroFechasGrafico.FechaInicio = Metodos.ManejoFechas.ObtenerFechas(ucFiltroFechasGrafico.TipoPeriodo, Request.Params["fi"], Request.Params["ff"]).Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"); ;
                        ucFiltroFechasGrafico.FechaFin = Metodos.ManejoFechas.ObtenerFechas(ucFiltroFechasGrafico.TipoPeriodo, Request.Params["fi"], Request.Params["ff"]).Single(s => s.Key == "fin").Value.ToString("dd/MM/yyyy"); ;
                    }

                    if (Request.Params["idArbol"] != null && Request.Params["tipoFecha"] != null && Request.Params["fi"] != null && Request.Params["ff"] != null)
                    {
                        LLenaDatos(int.Parse(Request.Params["idArbol"]), int.Parse(Request.Params["tipoFecha"]), Request.Params["fi"], Request.Params["ff"]);
                    }
                    else if (Request.Params["idArbol"] != null && Request.Params["tipoFecha"] != null)
                        LLenaDatos(int.Parse(Request.Params["idArbol"]), int.Parse(Request.Params["tipoFecha"]), string.Empty, string.Empty);
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

        protected void rptPreguntas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Label lblTituloPreguntaPie = (Label)e.Item.FindControl("lblPreguntaTituloPie");
                Label lblTituloPreguntaColumn = (Label)e.Item.FindControl("lblPreguntaTituloColumn");
                
                RadHtmlChart chartPie = (RadHtmlChart)e.Item.FindControl("rhGraficoPreguntaPie");
                RadHtmlChart chartColumn = (RadHtmlChart)e.Item.FindControl("rhGraficoPregunta");
                if (lblTituloPreguntaPie != null && lblTituloPreguntaColumn != null && chartPie != null && chartColumn != null)
                {
                    DataTable dt = (DataTable)e.Item.DataItem;
                    lblTituloPreguntaPie.Text = dt.ExtendedProperties["Pregunta"].ToString();
                    lblTituloPreguntaColumn.Text = dt.ExtendedProperties["Pregunta"].ToString();

                    DataTable dtTotalPregunta = new DataTable();
                    dtTotalPregunta.Columns.Add("Descripcion", typeof(string));
                    dtTotalPregunta.Columns.Add("Color", typeof(string));
                    dtTotalPregunta.Columns.Add("Total", typeof(int));
                    foreach (DataRow dr in dt.Rows)
                    {
                        int sum = 0;
                        for (int i = 2; i < dt.Columns.Count; i++)
                        {
                            if (dr.RowState != DataRowState.Deleted)
                                sum += Convert.ToInt32(dr[i]);
                        }
                        dtTotalPregunta.Rows.Add(dr[0], dr[1], sum);
                    }
                    GeneraGraficaPie(chartPie, dtTotalPregunta, dt.ExtendedProperties["Pregunta"].ToString());
                    //DataTable 
                    GeneraGraficaStackedColumn(chartColumn, dt, dt.ExtendedProperties["Pregunta"].ToString());

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnDownload_OnClick(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDatos = null;
                if (Request.Params["idArbol"] != null && Request.Params["tipoFecha"] != null && Request.Params["fi"] != null && Request.Params["ff"] != null)
                {
                    dtDatos = _servicioEncuesta.ObtenerGraficoCalificacionDescarga(int.Parse(Request.Params["idArbol"]), Metodos.ManejoFechas.ObtenerFechas(int.Parse(Request.Params["tipoFecha"]), Request.Params["fi"], Request.Params["ff"]));
                }
                else if (Request.Params["idArbol"] != null && Request.Params["tipoFecha"] != null)
                    dtDatos = _servicioEncuesta.ObtenerGraficoCalificacionDescarga(int.Parse(Request.Params["idArbol"]), Metodos.ManejoFechas.ObtenerFechas(int.Parse(Request.Params["tipoFecha"]), string.Empty, string.Empty));

                Response.Clear();
                MemoryStream ms = new MemoryStream(BusinessFile.ExcelManager.DatatableToExcel(dtDatos).GetAsByteArray());

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=GraficoEncuestaCalificacion.xlsx");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
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
                    LLenaDatos(int.Parse(Request.Params["idArbol"]), ucFiltroFechasGrafico.TipoPeriodo, ucFiltroFechasGrafico.FechaInicio, ucFiltroFechasGrafico.FechaFin);
                else
                    LLenaDatos(int.Parse(Request.Params["idArbol"]), ucFiltroFechasGrafico.TipoPeriodo, string.Empty, string.Empty);
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