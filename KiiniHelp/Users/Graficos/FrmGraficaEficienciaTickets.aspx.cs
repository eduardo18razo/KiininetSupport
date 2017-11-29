using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.Users.Graficos
{
    public partial class FrmGraficaEficienciaTickets : Page
    {
        private readonly ServiceConsultasClient _servicioConsultas = new ServiceConsultasClient();

        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
        {
            set
            {
                pnlAlertaGeneral.Visible = value.Any();
                if (!pnlAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                ucFiltrosParametrosGraficoEficienciaTickets.OnAceptarModal += UcFiltrosParametrosGraficoEncuestasOnOnAceptarModal;
                ucFiltrosParametrosGraficoEficienciaTickets.OnCancelarModal += UcFiltrosParametrosGraficoEncuestasOnOnCancelarModal;
                if (Convert.ToBoolean(hfGraficado.Value))
                    UcFiltrosParametrosGraficoEncuestasOnOnAceptarModal(false);
                cGrafico.Click += CGraficoOnClick;
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
                List<HelperReportesTicket> lstConsulta = _servicioConsultas.ConsultarEficienciaTickets(((Usuario)Session["UserData"]).Id, ucFiltrosEficienciaTickets.FiltroGrupos,
                    ucFiltrosEficienciaTickets.FiltroResponsables, ucFiltrosEficienciaTickets.FiltroTipoArbol, ucFiltrosEficienciaTickets.FiltroTipificaciones, new List<int>(),
                    ucFiltrosEficienciaTickets.FiltroAtendedores, ucFiltrosEficienciaTickets.FiltroFechas, ucFiltrosEficienciaTickets.FiltroTipoUsuario, ucFiltrosEficienciaTickets.FiltroPrioridad,
                    ucFiltrosEficienciaTickets.FiltroUbicaciones, ucFiltrosEficienciaTickets.FiltroOrganizaciones, ucFiltrosEficienciaTickets.FiltroVip, 0, 1000);
                string[] selectedData = imageMapEventArgs.PostBackValue.ToString().Split(',');
                string fecha = selectedData[0];
                string total = selectedData[1];
                int idFiltro = int.Parse(selectedData[2]);
                switch (ucFiltrosParametrosGraficoEficienciaTickets.TipoGrafico)
                {
                    case "Geografico":
                        break;
                    case "Pareto":
                        switch (ucFiltrosParametrosGraficoEficienciaTickets.Stack)
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
                        switch (ucFiltrosParametrosGraficoEficienciaTickets.Stack)
                        {
                            case "Ubicaciones":
                                switch (ucFiltrosEficienciaTickets.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Organizaciones":
                                switch (ucFiltrosEficienciaTickets.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Tipo Ticket":
                                switch (ucFiltrosEficienciaTickets.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro && DateTime.Parse(w.FechaHora).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdServicioIncidente == idFiltro && DateTime.Parse(w.FechaHora).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Tipificaciones":
                                switch (ucFiltrosEficienciaTickets.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Estatus Ticket":
                                switch (ucFiltrosEficienciaTickets.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro && DateTime.Parse(w.FechaHora).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdEstatus == idFiltro && DateTime.Parse(w.FechaHora).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "SLA":
                                switch (ucFiltrosEficienciaTickets.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera") && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera") && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo) && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera") && DateTime.Parse(w.FechaHora).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.Sla == (idFiltro == 1 ? "Dentro" : "Fuera") && DateTime.Parse(w.FechaHora).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                        }
                        break;
                }
                gvResult.DataSource = lstConsulta;
                gvResult.DataBind();
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

        private void UcFiltrosParametrosGraficoEncuestasOnOnCancelarModal()
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

        private void UcFiltrosParametrosGraficoEncuestasOnOnAceptarModal(bool cierraModal)
        {
            try
            {
                switch (ucFiltrosParametrosGraficoEficienciaTickets.TipoGrafico)
                {
                    case "Geografico":
                        frameGeoCharts.Attributes.Add("src", ResolveUrl("FrmGeoChart.aspx?Data=" + _servicioConsultas.GraficarConsultaEficienciaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosEficienciaTickets.FiltroGrupos,
                    ucFiltrosEficienciaTickets.FiltroResponsables, ucFiltrosEficienciaTickets.FiltroTipoArbol, ucFiltrosEficienciaTickets.FiltroTipificaciones, new List<int>(),
                    ucFiltrosEficienciaTickets.FiltroAtendedores, ucFiltrosEficienciaTickets.FiltroFechas, ucFiltrosEficienciaTickets.FiltroTipoUsuario, ucFiltrosEficienciaTickets.FiltroPrioridad,
                    ucFiltrosEficienciaTickets.FiltroUbicaciones, ucFiltrosEficienciaTickets.FiltroOrganizaciones, ucFiltrosEficienciaTickets.FiltroVip, ucFiltrosParametrosGraficoEficienciaTickets.Stack, ucFiltrosEficienciaTickets.TipoPeriodo)));
                        frameGeoCharts.Visible = true;
                        cGrafico.Visible = false;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Stack":
                        BusinessGraficoStack.Encuestas.Linear.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaEficienciaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosEficienciaTickets.FiltroGrupos,
                    ucFiltrosEficienciaTickets.FiltroResponsables, ucFiltrosEficienciaTickets.FiltroTipoArbol, ucFiltrosEficienciaTickets.FiltroTipificaciones, new List<int>(),
                    ucFiltrosEficienciaTickets.FiltroAtendedores, ucFiltrosEficienciaTickets.FiltroFechas, ucFiltrosEficienciaTickets.FiltroTipoUsuario, ucFiltrosEficienciaTickets.FiltroPrioridad,
                    ucFiltrosEficienciaTickets.FiltroUbicaciones, ucFiltrosEficienciaTickets.FiltroOrganizaciones, ucFiltrosEficienciaTickets.FiltroVip, ucFiltrosParametrosGraficoEficienciaTickets.Stack, ucFiltrosEficienciaTickets.TipoPeriodo));
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Barra Comparativa":
                        BusinessGraficoStack.Encuestas.Columns.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaEficienciaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosEficienciaTickets.FiltroGrupos,
                    ucFiltrosEficienciaTickets.FiltroResponsables, ucFiltrosEficienciaTickets.FiltroTipoArbol, ucFiltrosEficienciaTickets.FiltroTipificaciones, new List<int>(),
                    ucFiltrosEficienciaTickets.FiltroAtendedores, ucFiltrosEficienciaTickets.FiltroFechas, ucFiltrosEficienciaTickets.FiltroTipoUsuario, ucFiltrosEficienciaTickets.FiltroPrioridad,
                    ucFiltrosEficienciaTickets.FiltroUbicaciones, ucFiltrosEficienciaTickets.FiltroOrganizaciones, ucFiltrosEficienciaTickets.FiltroVip, ucFiltrosParametrosGraficoEficienciaTickets.Stack, ucFiltrosEficienciaTickets.TipoPeriodo));
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                }
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

        private void UcFiltrosParametrosGraficoEncuestasOnOnAceptarModal()
        {
            try
            {
                switch (ucFiltrosParametrosGraficoEficienciaTickets.TipoGrafico)
                {
                    case "Geografico":
                        frameGeoCharts.Attributes.Add("src", ResolveUrl("FrmGeoChart.aspx?Data=" + _servicioConsultas.GraficarConsultaEficienciaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosEficienciaTickets.FiltroGrupos,
                    ucFiltrosEficienciaTickets.FiltroResponsables, ucFiltrosEficienciaTickets.FiltroTipoArbol, ucFiltrosEficienciaTickets.FiltroTipificaciones, new List<int>(),
                    ucFiltrosEficienciaTickets.FiltroAtendedores, ucFiltrosEficienciaTickets.FiltroFechas, ucFiltrosEficienciaTickets.FiltroTipoUsuario, ucFiltrosEficienciaTickets.FiltroPrioridad,
                    ucFiltrosEficienciaTickets.FiltroUbicaciones, ucFiltrosEficienciaTickets.FiltroOrganizaciones, ucFiltrosEficienciaTickets.FiltroVip, ucFiltrosParametrosGraficoEficienciaTickets.Stack, ucFiltrosEficienciaTickets.TipoPeriodo)));
                        frameGeoCharts.Visible = true;
                        cGrafico.Visible = false;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Stack":
                        BusinessGraficoStack.Encuestas.Linear.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaEficienciaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosEficienciaTickets.FiltroGrupos,
                    ucFiltrosEficienciaTickets.FiltroResponsables, ucFiltrosEficienciaTickets.FiltroTipoArbol, ucFiltrosEficienciaTickets.FiltroTipificaciones, new List<int>(),
                    ucFiltrosEficienciaTickets.FiltroAtendedores, ucFiltrosEficienciaTickets.FiltroFechas, ucFiltrosEficienciaTickets.FiltroTipoUsuario, ucFiltrosEficienciaTickets.FiltroPrioridad,
                    ucFiltrosEficienciaTickets.FiltroUbicaciones, ucFiltrosEficienciaTickets.FiltroOrganizaciones, ucFiltrosEficienciaTickets.FiltroVip, ucFiltrosParametrosGraficoEficienciaTickets.Stack, ucFiltrosEficienciaTickets.TipoPeriodo));
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Barra Comparativa":
                        BusinessGraficoStack.Encuestas.Columns.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaEficienciaTicket(((Usuario)Session["UserData"]).Id, ucFiltrosEficienciaTickets.FiltroGrupos,
                    ucFiltrosEficienciaTickets.FiltroResponsables, ucFiltrosEficienciaTickets.FiltroTipoArbol, ucFiltrosEficienciaTickets.FiltroTipificaciones, new List<int>(),
                    ucFiltrosEficienciaTickets.FiltroAtendedores, ucFiltrosEficienciaTickets.FiltroFechas, ucFiltrosEficienciaTickets.FiltroTipoUsuario, ucFiltrosEficienciaTickets.FiltroPrioridad,
                    ucFiltrosEficienciaTickets.FiltroUbicaciones, ucFiltrosEficienciaTickets.FiltroOrganizaciones, ucFiltrosEficienciaTickets.FiltroVip, ucFiltrosParametrosGraficoEficienciaTickets.Stack, ucFiltrosEficienciaTickets.TipoPeriodo));
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                }
                upGrafica.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroParametros\");", true);
                hfGraficado.Value = true.ToString();
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
            try
            {
                //TODO: Cambiar filtro new list por niveles
                List<HelperReportesTicket> lstConsulta = _servicioConsultas.ConsultarEficienciaTickets(((Usuario)Session["UserData"]).Id, ucFiltrosEficienciaTickets.FiltroGrupos,
                    ucFiltrosEficienciaTickets.FiltroResponsables, ucFiltrosEficienciaTickets.FiltroTipoArbol, ucFiltrosEficienciaTickets.FiltroTipificaciones, new List<int>(),
                    ucFiltrosEficienciaTickets.FiltroAtendedores, ucFiltrosEficienciaTickets.FiltroFechas, ucFiltrosEficienciaTickets.FiltroTipoUsuario, ucFiltrosEficienciaTickets.FiltroPrioridad,
                    ucFiltrosEficienciaTickets.FiltroUbicaciones, ucFiltrosEficienciaTickets.FiltroOrganizaciones, ucFiltrosEficienciaTickets.FiltroVip, 0, 1000);

                if (lstConsulta == null) return;
                hfGraficado.Value = false.ToString();
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
        protected void btnCerrar_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleGrafico\");", true);
        }
    }
}