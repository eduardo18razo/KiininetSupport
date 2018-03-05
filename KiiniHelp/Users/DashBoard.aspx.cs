using System;


using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceDashboard;
using KiiniNet.Entities.Operacion.Dashboard;
using KinniNet.Business.Utils;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;


namespace KiiniHelp.Users
{
    public partial class DashBoard : System.Web.UI.Page
    {

        private readonly ServiceDashboardsClient _servicioDashboard = new ServiceDashboardsClient();

        private List<string> _lstError = new List<string>();

        private void LlenaDatos()
        {
            try
            {
                DashboardAdministrador datos = _servicioDashboard.GetDashboardAdministrador();
                lblUsuariosRegistrados.Text = datos.UsuariosRegistrados.ToString();
                lblUsuariosActivos.Text = datos.UsuariosActivos.ToString();
                lblTicketsCreados.Text = datos.TicketsCreados.ToString();
                lblOperadoresAcumulados.Text = datos.Operadores.ToString();

                lblEspacio.Text = string.Format("{0} MB de {1} Mb en uso ", Math.Truncate(double.Parse(datos.GraficoAlmacenamiento.Rows[0][0].ToString())), Math.Truncate(double.Parse(datos.GraficoAlmacenamiento.Rows[0][0].ToString()) + double.Parse(datos.GraficoAlmacenamiento.Rows[0][1].ToString())));
                lblArchivos.Text = string.Format("{0} archivos adjuntos", datos.TotalArchivos);

                lblCategorias.Text = datos.Categorias.ToString();
                lblArticulos.Text = datos.Articulos.ToString();
                lblFormularios.Text = datos.Formularios.ToString();
                lblCatalogos.Text = datos.Catalogos.ToString();

                lblOrganizaciones.Text = datos.Organizacion.ToString();
                lblUbicaciones.Text = datos.Ubicacion.ToString();
                lblPuestos.Text = datos.Puestos.ToString();

                lblGrupos.Text = datos.Grupos.ToString();
                lblHorarios.Text = datos.Horarios.ToString();
                lblFeriados.Text = datos.Feriados.ToString();
                rptOperadorRol.DataSource = datos.OperadorRol;
                rptOperadorRol.DataBind();

                GeneraGraficaPie(rhcTicketsCanal, datos.GraficoTicketsCreadosCanal);
                GeneraGraficaPie(rhcUsuarios, datos.GraficoUsuariosRegistrados);
                GeneraGraficaStackedAdministrador(rhcEspacio, datos.GraficoAlmacenamiento);
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GeneraGraficaStackedAdministrador(RadHtmlChart grafico, DataTable dt)
        {
            grafico.Width = Unit.Percentage(100);
            grafico.Height = Unit.Pixel(250);
            grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;

            int maxValue = int.Parse(Math.Truncate(double.Parse(dt.Rows[0][0].ToString()) + double.Parse(dt.Rows[0][1].ToString())).ToString());
            for (int c = 0; c < dt.Columns.Count; c++)
            {
                BarSeries serie = new BarSeries();
                serie.Stacked = true;
                serie.LabelsAppearance.Visible = false;
                serie.LabelsAppearance.Position = BarColumnLabelsPosition.Center;
                serie.LabelsAppearance.DataFormatString = "{0} MB";
                serie.TooltipsAppearance.DataFormatString = "{0} MB";
                serie.DataFieldY = dt.Columns[c].ColumnName;
                grafico.PlotArea.Series.Add(serie);
            }
            grafico.PlotArea.XAxis.MaxValue = maxValue;
            grafico.PlotArea.YAxis.MaxValue = maxValue;

            grafico.PlotArea.XAxis.DataLabelsField = dt.Columns[dt.Columns.Count - 1].ColumnName;
            grafico.PlotArea.XAxis.MajorGridLines.Width = 0;
            grafico.PlotArea.XAxis.MinorGridLines.Width = 0;
            grafico.PlotArea.YAxis.MajorGridLines.Width = 0;
            grafico.PlotArea.YAxis.MinorGridLines.Width = 0;
            grafico.DataSource = dt;
            grafico.DataBind();
        }

        private void GeneraGraficaPie(RadHtmlChart grafico, DataTable dt)
        {
            grafico.PlotArea.Series.Clear();
            PieSeries pieSerie = new PieSeries();
            pieSerie.DataFieldY = "Total";
            pieSerie.NameField = "Descripcion";
            pieSerie.LabelsAppearance.Visible = true;
            pieSerie.LabelsAppearance.Position = PieAndDonutLabelsPosition.Center;

            grafico.PlotArea.Series.Add(pieSerie);
            grafico.DataSource = dt;
            grafico.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    LlenaDatos();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}