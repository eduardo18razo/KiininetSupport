using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceDashboard;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Dashboard;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using System.Collections.Generic;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;

namespace KiiniHelp.Agente
{
    public partial class DashBoard : System.Web.UI.Page
    {
        private readonly ServiceDashboardsClient _servicioDashBoard = new ServiceDashboardsClient();
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupos = new ServiceGrupoUsuarioClient();
        private List<string> _lstError = new List<string>();
        private List<string> Alerta
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

        private void LlenaCombos()
        {
            try
            {
                Usuario usr = ((Usuario)Session["UserData"]);
                ddlGrupo.DataSource = _servicioGrupos.ObtenerGruposAtencionByIdUsuario(((Usuario)Session["UserData"]).Id, true);
                ddlGrupo.DataTextField = "Descripcion";
                ddlGrupo.DataValueField = "Id";
                ddlGrupo.DataBind();
                ddlAgente.DataSource = _servicioUsuarios.ObtenerAgentes(true);
                ddlAgente.DataTextField = "NombreCompleto";
                ddlAgente.DataValueField = "Id";
                ddlAgente.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void CargaDatosDashboard()
        {
            try
            {
                int? idGrupo = ddlGrupo.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? (int?)null : int.Parse(ddlGrupo.SelectedValue);
                int? idUsuario = ddlAgente.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? (int?)null : int.Parse(ddlAgente.SelectedValue);

                DashboardAgente datos = _servicioDashBoard.GetDashboardAgente(idGrupo, idUsuario);
                lblTicketsAcumulados.Text = datos.TotalTickets.ToString();
                lblTicketsAbiertos7dias.Text = datos.TicketsAbiertos.ToString();
                lblTicketsCreados7dias.Text = datos.TicketsCreados.ToString();
                lblTicketsResueltos7dias.Text = datos.TicketsResuletos.ToString();
                lblTicketsResCreados7dias.Text = string.Format("{0} %", datos.TicketsResuletosVsCreados);
                lblTicketsReabiertos7dias.Text = string.Format("{0} %", datos.TicketsResuletosVsReabiertos);

                lblTiempoPromedioPrimeraRespuestaActual.Text = datos.PromedioPrimeraRespuestaActual;
                lblTiempoPromedioPrimeraRespuestaPorcentaje.Text = string.Format("{0} %", datos.DiferenciaPromedioRespuestaPorcentaje.ToString("0.##"));
                lblTiempoPromedioPrimeraRespuestaAnterior.Text = datos.PromedioPrimeraRespuestaAnterior;

                lblTiempoPromedioResolucionActual.Text = datos.PromedioTiempoResolucionActual;
                lblTiempoPromedioResolucionPorcentaje.Text = string.Format("{0} %", datos.PromedioTiempoResolucionPorcentaje.ToString("0.##"));
                lblTiempoPromedioResolucionAnterior.Text = datos.PromedioTiempoResolucionAnterior;

                lblResolucionAlPrimerContactoPromedioActual.Text = datos.PromedioResolucionPrimercontactoActual;
                lblResolucionAlPrimerContactoPromedioPorcentaje.Text = string.Format("{0} %", datos.PromedioResolucionPrimercontactoPorcentaje.ToString("0.##"));
                lblResolucionAlPrimerContactoPromedioAnterior.Text = datos.PromedioResolucionPrimercontactoAnterior;

                lblIntervencionesAgenteActual.Text = datos.PromedioIntervencionesAgenteActual.ToString();
                lblIntervencionesAgentePorcentaje.Text = string.Format("{0} %", datos.PromedioIntervencionesAgentePorcentaje.ToString("0.##"));
                lblIntervencionesAgenteAnterior.Text = datos.PromedioIntervencionesAgenteAnterior.ToString();

                lblClientesUnicosAtendidosActual.Text = datos.ClientesAtendidosActual.ToString();
                lblClientesUnicosAtendidosAnterior.Text = datos.ClientesAtendidosAnterior.ToString();
                lblClientesUnicosAtendidosPorcentaje.Text = string.Format("{0} %", datos.ClientesAtendidosAnterior.ToString("0.##")); ;

                GeneraGraficaPie(rhcTicketsAbiertos, datos.GraficoTicketsAbiertos);
                GeneraGraficaStackedBar(rhcTicketsPrioridad, datos.GraficoTicketsPrioridad);
                GeneraGraficaPie(rhcTicketsCanal, datos.GraficoTicketsCanal);
                GeneraGraficaArea(datos.GraficoTicketsCreadosResueltos);

                rptMetricasGrupo.DataSource = datos.GruposMetricas;
                rptMetricasGrupo.DataBind();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GeneraGraficaArea(DataTable graficoTicketsCreadosResueltos)
        {
            try
            {
                rhcTicketsCreadosAbiertos.PlotArea.Series.Clear();
                rhcTicketsCreadosAbiertos.PlotArea.XAxis.Items.Clear();
                decimal maxvalue = 0;
                foreach (DataRow row in graficoTicketsCreadosResueltos.Rows)
                {
                    AreaSeries serie = new AreaSeries { Name = row[0].ToString() };
                    serie.LabelsAppearance.Position = LineAndScatterLabelsPosition.Above;
                    serie.MarkersAppearance.MarkersType = MarkersType.Cross;
                    serie.MarkersAppearance.BackgroundColor = Color.White;
                    serie.MarkersAppearance.Size = 6;
                    serie.MarkersAppearance.BorderWidth = 2;
                    serie.TooltipsAppearance.Color = Color.White;
                    for (int i = 1; i < graficoTicketsCreadosResueltos.Columns.Count; i++)
                    {
                        serie.SeriesItems.Add(decimal.Parse(row[i].ToString()));
                        maxvalue = maxvalue < decimal.Parse(row[i].ToString()) ? decimal.Parse(row[i].ToString()) : maxvalue;
                    }
                    rhcTicketsCreadosAbiertos.PlotArea.Series.Add(serie);
                }
                for (int i = 1; i < graficoTicketsCreadosResueltos.Columns.Count; i++)
                {
                    rhcTicketsCreadosAbiertos.PlotArea.XAxis.Items.Add(graficoTicketsCreadosResueltos.Columns[i].ColumnName);
                }

                rhcTicketsCreadosAbiertos.PlotArea.Appearance.FillStyle.BackgroundColor = Color.Transparent;
                rhcTicketsCreadosAbiertos.PlotArea.XAxis.AxisCrossingValue = 0;
                rhcTicketsCreadosAbiertos.PlotArea.XAxis.Color = Color.Black;
                rhcTicketsCreadosAbiertos.PlotArea.XAxis.MajorTickType = TickType.Outside;
                rhcTicketsCreadosAbiertos.PlotArea.XAxis.MinorTickType = TickType.Outside;
                rhcTicketsCreadosAbiertos.PlotArea.XAxis.Reversed = false;
                rhcTicketsCreadosAbiertos.PlotArea.XAxis.LabelsAppearance.RotationAngle = 90;

                rhcTicketsCreadosAbiertos.PlotArea.YAxis.AxisCrossingValue = 0;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.Color = Color.Black;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.MajorTickSize = 2;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.MajorTickType = TickType.Outside;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.MaxValue = maxvalue + 10;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.MinorTickType = TickType.None;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.MinValue = 0;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.Reversed = false;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.Step = 20;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.LabelsAppearance.Step = 0;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.TitleAppearance.Position = AxisTitlePosition.Center;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.TitleAppearance.Text = string.Empty;
                rhcTicketsCreadosAbiertos.Legend.Appearance.Position = ChartLegendPosition.Bottom;
                rhcTicketsCreadosAbiertos.Legend.Appearance.Orientation = ChartLegendOrientation.Horizontal;

                rhcTicketsCreadosAbiertos.PlotArea.YAxis.MajorGridLines.Width = 1;
                rhcTicketsCreadosAbiertos.PlotArea.YAxis.MinorGridLines.Width = 0;

                rhcTicketsCreadosAbiertos.PlotArea.XAxis.MajorGridLines.Width = 0;
                rhcTicketsCreadosAbiertos.PlotArea.XAxis.MinorGridLines.Width = 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GeneraGraficaPie(RadHtmlChart grafico, DataTable dt)
        {
            grafico.PlotArea.Series.Clear();
            PieSeries pieSerie = new PieSeries();
            pieSerie.DataFieldY = "Total";
            pieSerie.NameField = "Descripcion";
            pieSerie.LabelsAppearance.Visible = true;
            pieSerie.LabelsAppearance.Position = PieAndDonutLabelsPosition.Center;
            grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
            grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Horizontal;
            grafico.PlotArea.Series.Add(pieSerie);
            grafico.DataSource = dt;
            grafico.DataBind();
        }
        private void GeneraGraficaStackedBar(RadHtmlChart grafico, DataTable dt)
        {
            grafico.PlotArea.Series.Clear();
            grafico.PlotArea.XAxis.Items.Clear();
            decimal maxvalue = 0;
            foreach (DataRow row in dt.Rows)
            {
                BarSeries serie = new BarSeries { Name = row[0].ToString(), Stacked = true };
                serie.LabelsAppearance.Position = BarColumnLabelsPosition.Center;
                serie.TooltipsAppearance.Color = Color.White;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    serie.SeriesItems.Add(decimal.Parse(row[i].ToString()));
                    maxvalue = maxvalue < decimal.Parse(row[i].ToString()) ? decimal.Parse(row[i].ToString()) : maxvalue;
                }
                grafico.PlotArea.Series.Add(serie);
            }
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                grafico.PlotArea.XAxis.Items.Add(dt.Columns[i].ColumnName);
            }
            grafico.Legend.Appearance.Position = ChartLegendPosition.Bottom;
            grafico.Legend.Appearance.Orientation = ChartLegendOrientation.Horizontal;

            grafico.PlotArea.YAxis.MajorGridLines.Width = 1;
            grafico.PlotArea.YAxis.MinorGridLines.Width = 0;

            grafico.PlotArea.XAxis.MajorGridLines.Width = 0;
            grafico.PlotArea.XAxis.MinorGridLines.Width = 0;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LlenaCombos();
                CargaDatosDashboard();
            }
        }

        protected void ddlGrupo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlAgente);
                if (ddlGrupo.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    ddlAgente.DataSource = _servicioUsuarios.ObtenerAgentes(true);
                    ddlAgente.DataTextField = "NombreCompleto";
                    ddlAgente.DataValueField = "Id";
                    ddlAgente.DataBind();
                }
                else
                {
                    GrupoUsuario gpo = _servicioGrupos.ObtenerGrupoUsuarioById(int.Parse(ddlGrupo.SelectedValue));
                    if (gpo != null)
                    {
                        ddlAgente.DataSource = _servicioUsuarios.ObtenerUsuariosByGrupoAtencion(gpo.Id, true);
                        ddlAgente.DataTextField = "NombreCompleto";
                        ddlAgente.DataValueField = "Id";
                        ddlAgente.DataBind();
                        Usuario usr = ((Usuario)Session["UserData"]);
                        switch (gpo.TieneSupervisor)
                        {
                            case true:
                                if (usr.UsuarioGrupo.Any(s => s.IdGrupoUsuario == gpo.Id && s.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor))
                                {
                                    ddlAgente.Enabled = true;
                                }
                                else
                                {
                                    ddlAgente.SelectedValue = usr.Id.ToString();
                                    ddlAgente.Enabled = false;
                                }
                                break;
                            case false:
                                if (usr.UsuarioGrupo.Any(s => s.IdGrupoUsuario == gpo.Id && s.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.PrimererNivel))
                                {
                                    ddlAgente.Enabled = true;
                                }
                                else
                                {
                                    ddlAgente.SelectedValue = usr.Id.ToString();
                                    ddlAgente.Enabled = false;
                                }
                                break;
                        }
                    }
                }
                CargaDatosDashboard();
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

        protected void ddlAgente_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlAgente.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    CargaDatosDashboard();
                    return;
                }
                CargaDatosDashboard();
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