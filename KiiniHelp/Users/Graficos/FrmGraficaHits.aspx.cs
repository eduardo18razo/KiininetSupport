using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.Users.Graficos
{
    public partial class FrmGraficaHits : System.Web.UI.Page
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
                UcFiltrosParametrosGraficoHits.OnAceptarModal += UcFiltrosParametrosGraficoHitsOnOnAceptarModal;
                UcFiltrosParametrosGraficoHits.OnCancelarModal += UcFiltrosParametrosGraficoHits_OnCancelarModal;
                ucDetalleGeograficoHit.OnCancelarModal += UcDetalleGeograficoHitOnOnCancelarModal;
                if (Convert.ToBoolean(hfGraficaGenerada.Value))
                    UcFiltrosParametrosGraficoHitsOnOnAceptarModal(false);
                cGrafico.Click += CGraficoOnClick;
               
                ucFiltrosGraficasHits.OnAceptarModal += ucFiltrosGraficasHits_OnAceptarModal;
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
                List<HelperHits> lstConsulta = _servicioConsultas.ConsultarHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario, ucFiltrosGraficasHits.FiltroOrganizaciones,
                    ucFiltrosGraficasHits.FiltroUbicaciones, ucFiltrosGraficasHits.FiltroTipificaciones,
                    ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, 0, 1000);
                string[] selectedData = imageMapEventArgs.PostBackValue.ToString().Split(',');
                string fecha = selectedData[0];
                string total = selectedData[1];
                int idFiltro = int.Parse(selectedData[2]);
                switch (UcFiltrosParametrosGraficoHits.TipoGrafico)
                {
                    case "Geografico":
                        break;
                    case "Pareto":
                        switch (UcFiltrosParametrosGraficoHits.Stack)
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
                        }
                        break;
                    case "Tendencia Stack":
                    case "Tendencia Barra Comparativa":
                        switch (UcFiltrosParametrosGraficoHits.Stack)
                        {
                            case "Ubicaciones":
                                switch (ucFiltrosGraficasHits.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdUbicacion == idFiltro
                                            && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo)
                                            && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)
                                            ).ToList();
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
                                switch (ucFiltrosGraficasHits.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro
                                            && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo)
                                            && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)
                                            ).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdOrganizacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
                                        break;
                                }
                                break;
                            case "Tipificaciones":
                                switch (ucFiltrosGraficasHits.TipoPeriodo)
                                {
                                    case 1:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && w.FechaHora == fecha).ToList();
                                        break;
                                    case 2:
                                        int semanaNo = int.Parse(fecha.Split(' ')[1]);
                                        int anio = int.Parse(fecha.Split(' ')[3]);
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro
                                            && DateTime.Parse(w.FechaHora) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anio, semanaNo)
                                            && DateTime.Parse(w.FechaHora) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(anio, semanaNo)
                                            ).ToList();
                                        break;
                                    case 3:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("MM") == fecha.PadLeft(2, '0')).ToList();
                                        break;
                                    case 4:
                                        lstConsulta = lstConsulta.Where(w => w.IdTipificacion == idFiltro && DateTime.Parse(w.FechaHora).ToString("yyyy") == fecha.PadLeft(4, '0')).ToList();
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

        #region Delegados
        private void UcFiltrosParametrosGraficoHits_OnCancelarModal()
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

        private void UcDetalleGeograficoHitOnOnCancelarModal()
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

        private void UcFiltrosParametrosGraficoHitsOnOnAceptarModal()
        {
            try
            {
                switch (UcFiltrosParametrosGraficoHits.TipoGrafico)
                {
                    case "Geografico":
                        frameGeoCharts.Attributes.Add("src", ResolveUrl("FrmGeoChart.aspx?Data=" + _servicioConsultas.GraficarConsultaHitsGeografico(
                                          ((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos,
                                          ucFiltrosGraficasHits.FiltroTipoUsuario,
                                          UcFiltrosParametrosGraficoHits.OrganizacionesGraficar.Select(s => s.Id)
                                              .ToList(),
                                          UcFiltrosParametrosGraficoHits.UbicacionesGraficar.Select(s => s.Id).ToList(),
                                          UcFiltrosParametrosGraficoHits.TipificacionesGraficar.Select(s => s.Id)
                                              .ToList(),
                                          ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas,
                                          UcFiltrosParametrosGraficoHits.FiltroStack,
                                          UcFiltrosParametrosGraficoHits.Stack, ucFiltrosGraficasHits.TipoPeriodo)));
                        frameGeoCharts.Visible = true;
                        cGrafico.Visible = false;
                        upGrafica.Visible = true;
                        break;
                    case "Pareto":
                        BusinessGraficoStack.Pareto.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario,
                            UcFiltrosParametrosGraficoHits.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, UcFiltrosParametrosGraficoHits.FiltroStack, UcFiltrosParametrosGraficoHits.Stack, ucFiltrosGraficasHits.TipoPeriodo), UcFiltrosParametrosGraficoHits.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Stack":
                        BusinessGraficoStack.Stack.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario,
                            UcFiltrosParametrosGraficoHits.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, UcFiltrosParametrosGraficoHits.FiltroStack, UcFiltrosParametrosGraficoHits.Stack, ucFiltrosGraficasHits.TipoPeriodo), UcFiltrosParametrosGraficoHits.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Barra Comparativa":
                        BusinessGraficoStack.ColumnsClustered.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario,
                            UcFiltrosParametrosGraficoHits.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, UcFiltrosParametrosGraficoHits.FiltroStack, UcFiltrosParametrosGraficoHits.Stack, ucFiltrosGraficasHits.TipoPeriodo), UcFiltrosParametrosGraficoHits.Stack);
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

        private void UcFiltrosParametrosGraficoHitsOnOnAceptarModal(bool cierraModal)
        {
            try
            {
                switch (UcFiltrosParametrosGraficoHits.TipoGrafico)
                {
                    case "Geografico":
                        frameGeoCharts.Attributes.Add("src", ResolveUrl("FrmGeoChart.aspx?Data=" + _servicioConsultas.GraficarConsultaHitsGeografico(
                                          ((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos,
                                          ucFiltrosGraficasHits.FiltroTipoUsuario,
                                          UcFiltrosParametrosGraficoHits.OrganizacionesGraficar.Select(s => s.Id)
                                              .ToList(),
                                          UcFiltrosParametrosGraficoHits.UbicacionesGraficar.Select(s => s.Id).ToList(),
                                          UcFiltrosParametrosGraficoHits.TipificacionesGraficar.Select(s => s.Id)
                                              .ToList(),
                                          ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas,
                                          UcFiltrosParametrosGraficoHits.FiltroStack,
                                          UcFiltrosParametrosGraficoHits.Stack, ucFiltrosGraficasHits.TipoPeriodo)));
                        frameGeoCharts.Visible = true;
                        cGrafico.Visible = false;
                        upGrafica.Visible = true;
                        break;
                    case "Pareto":
                        BusinessGraficoStack.Pareto.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario,
                            UcFiltrosParametrosGraficoHits.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, UcFiltrosParametrosGraficoHits.FiltroStack, UcFiltrosParametrosGraficoHits.Stack, ucFiltrosGraficasHits.TipoPeriodo), UcFiltrosParametrosGraficoHits.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Stack":
                        BusinessGraficoStack.Stack.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario,
                            UcFiltrosParametrosGraficoHits.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, UcFiltrosParametrosGraficoHits.FiltroStack, UcFiltrosParametrosGraficoHits.Stack, ucFiltrosGraficasHits.TipoPeriodo), UcFiltrosParametrosGraficoHits.Stack);
                        frameGeoCharts.Visible = false;
                        cGrafico.Visible = true;
                        upGrafica.Visible = true;
                        break;
                    case "Tendencia Barra Comparativa":
                        BusinessGraficoStack.ColumnsClustered.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario,
                            UcFiltrosParametrosGraficoHits.OrganizacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.UbicacionesGraficar.Select(s => s.Id).ToList(),
                            UcFiltrosParametrosGraficoHits.TipificacionesGraficar.Select(s => s.Id).ToList(),
                            ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, UcFiltrosParametrosGraficoHits.FiltroStack, UcFiltrosParametrosGraficoHits.Stack, ucFiltrosGraficasHits.TipoPeriodo), UcFiltrosParametrosGraficoHits.Stack);
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

        private void ucFiltrosGraficasHits_OnAceptarModal()
        {
            try
            {
                List<HelperHits> lstConsulta = _servicioConsultas.ConsultarHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario, ucFiltrosGraficasHits.FiltroOrganizaciones,
                    ucFiltrosGraficasHits.FiltroUbicaciones, ucFiltrosGraficasHits.FiltroTipificaciones,
                    ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, 0, 1000);
                if (lstConsulta == null) return;
                UcFiltrosParametrosGraficoHits.UbicacionesGraficar = lstConsulta.GroupBy(g => new { g.IdUbicacion, g.Ubicacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdUbicacion, Descripcion = s.Key.Ubicacion, Total = s.Count() }).ToList();
                UcFiltrosParametrosGraficoHits.OrganizacionesGraficar = lstConsulta.GroupBy(g => new { g.IdOrganizacion, g.Organizacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdOrganizacion, Descripcion = s.Key.Organizacion, Total = s.Count() }).ToList();
                UcFiltrosParametrosGraficoHits.TipificacionesGraficar = lstConsulta.GroupBy(g => new { g.IdTipificacion, g.Tipificacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdTipificacion, Descripcion = s.Key.Tipificacion, Total = s.Count() }).ToList();
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

        #endregion

        protected void btnConsultar_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    List<HelperHits> lstConsulta = _servicioConsultas.ConsultarHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario, ucFiltrosGraficasHits.FiltroOrganizaciones,
            //        ucFiltrosGraficasHits.FiltroUbicaciones, ucFiltrosGraficasHits.FiltroTipificaciones,
            //        ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, 0, 1000);
            //    if (lstConsulta == null) return;
            //    UcFiltrosParametrosGraficoHits.UbicacionesGraficar = lstConsulta.GroupBy(g => new { g.IdUbicacion, g.Ubicacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdUbicacion, Descripcion = s.Key.Ubicacion, Total = s.Count() }).ToList();
            //    UcFiltrosParametrosGraficoHits.OrganizacionesGraficar = lstConsulta.GroupBy(g => new { g.IdOrganizacion, g.Organizacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdOrganizacion, Descripcion = s.Key.Organizacion, Total = s.Count() }).ToList();
            //    UcFiltrosParametrosGraficoHits.TipificacionesGraficar = lstConsulta.GroupBy(g => new { g.IdTipificacion, g.Tipificacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdTipificacion, Descripcion = s.Key.Tipificacion, Total = s.Count() }).ToList();
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
                    List<HelperHits> lstConsulta = _servicioConsultas.ConsultarHits(((Usuario)Session["UserData"]).Id, ucFiltrosGraficasHits.FiltroGrupos, ucFiltrosGraficasHits.FiltroTipoUsuario, ucFiltrosGraficasHits.FiltroOrganizaciones,
                        new ServiceUbicacionClient().ObtenerUbicacionByRegionCode(hfRegion.Value).Select(s => s.Id).ToList(), ucFiltrosGraficasHits.FiltroTipificaciones,
                        ucFiltrosGraficasHits.FiltroVip, ucFiltrosGraficasHits.FiltroFechas, 0, 1000);
                    if (lstConsulta == null) return;
                    ucDetalleGeograficoHit.UbicacionesGraficar = lstConsulta.GroupBy(g => new { g.IdUbicacion, g.Ubicacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdUbicacion, Descripcion = s.Key.Ubicacion, Total = s.Count() }).ToList();
                    ucDetalleGeograficoHit.OrganizacionesGraficar = lstConsulta.GroupBy(g => new { g.IdOrganizacion, g.Organizacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdOrganizacion, Descripcion = s.Key.Organizacion, Total = s.Count() }).ToList();
                    ucDetalleGeograficoHit.TipificacionesGraficar = lstConsulta.GroupBy(g => new { g.IdTipificacion, g.Tipificacion }).Select(s => new HelperFiltroGrafico { Id = s.Key.IdTipificacion, Descripcion = s.Key.Tipificacion, Total = s.Count() }).ToList();
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