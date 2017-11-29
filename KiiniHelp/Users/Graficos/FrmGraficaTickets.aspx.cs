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
    public partial class FrmGraficaTickets : Page
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
                ucFiltrosGrafico.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucFiltrosGrafico.OnCancelarModal += UcFiltrosGraficoOnOnCancelarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                if (Convert.ToBoolean(hfGraficaGenerada.Value))
                    UcFiltrosGraficoOnAceptarModal(false);
                cGrafico.Click += CGraficoOnClick;
               
                ucFiltrosGraficas.OnAceptarModal +=ucFiltrosGraficas_OnAceptarModal;
                    
                    
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
                                switch (ucFiltrosGraficas.TipoPeriodo)
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
                                switch (ucFiltrosGraficas.TipoPeriodo)
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
                                switch (ucFiltrosGraficas.TipoPeriodo)
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
                                switch (ucFiltrosGraficas.TipoPeriodo)
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
                                switch (ucFiltrosGraficas.TipoPeriodo)
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