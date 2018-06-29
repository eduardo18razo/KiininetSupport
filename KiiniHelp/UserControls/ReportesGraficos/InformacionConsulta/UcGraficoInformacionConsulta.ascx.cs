using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniNet.Entities.Reportes;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;

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
            grafico.Width = Unit.Percentage(100);
            grafico.Height = Unit.Pixel(500);
            grafico.PlotArea.XAxis.MajorGridLines.Width = 0;
            grafico.PlotArea.XAxis.MinorGridLines.Width = 0;
            grafico.PlotArea.YAxis.MajorGridLines.Width = 0;
            grafico.PlotArea.YAxis.MinorGridLines.Width = 0;
            grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
            foreach (DataRow row in dt.Rows)
            {
                ColumnSeries column = new ColumnSeries();
                column.Name = row[0].ToString();
                column.GroupName = "Likes";
                column.Stacked = true;
                column.TooltipsAppearance.ClientTemplate = "#= series.name# Total: #= dataItem.value#";
                column.LabelsAppearance.Visible = false;
                for (int c = 1; c < dt.Columns.Count; c++)
                {
                    column.SeriesItems.Add((int)row[c]);
                }
                grafico.PlotArea.Series.Add(column);
            }
            for (int c = 1; c < dt.Columns.Count; c++)
            {
                grafico.PlotArea.XAxis.Items.Add(dt.Columns[c].ColumnName);
            }
            grafico.DataSource = dt;
            grafico.DataBind();
        }
        private void GeneraGraficaPie(RadHtmlChart grafico, DataTable dt)
        {
            grafico.Width = Unit.Percentage(100);
            grafico.Height = Unit.Pixel(500);
            grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;




            PieSeries pieSerie = new PieSeries();
            pieSerie.DataFieldY = "Total";
            pieSerie.NameField = "Descripcion";
            pieSerie.ExplodeField = "IsExploded";
            pieSerie.LabelsAppearance.Visible = true;
            pieSerie.LabelsAppearance.Position = PieAndDonutLabelsPosition.Center;

            grafico.PlotArea.Series.Add(pieSerie);
            grafico.DataSource = dt;
            grafico.DataBind();
        }

        private void LLenaDatos(int idInformacion, int idTipoFecha, string fechaInicio, string fechafin)
        {
            try
            {
                ReporteInformacionConsulta reporte = _servicioInformacionConsulta.ObtenerReporteInformacionConsulta(idInformacion, Metodos.ManejoFechas.ObtenerFechas(idTipoFecha, fechaInicio, fechafin), idTipoFecha);
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
                if (Request.Params["idInformacion"] != null && Request.Params["tipoFecha"] != null && Request.Params["fi"] != null && Request.Params["ff"] != null)
                {
                    LLenaDatos(int.Parse(Request.Params["idInformacion"]), int.Parse(Request.Params["tipoFecha"]), Request.Params["fi"], Request.Params["ff"]);
                }
                else if(Request.Params["idInformacion"] != null && Request.Params["tipoFecha"] != null)
                    LLenaDatos(int.Parse(Request.Params["idInformacion"]), int.Parse(Request.Params["tipoFecha"]), string.Empty, string.Empty);

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