using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Graficos
{
    public partial class UcFiltrosGraficasTicket : UserControl, IControllerModal
    {
        #region Propiedades publicas
        public List<int> FiltroGrupos
        {
            get { return ucFiltroGrupo.GruposSeleccionados; }
        }
        public List<int> FiltroCanalesApertura
        {
            get { return ucFiltroCanalApertura.CanalesSeleccionados; }
        }

        public List<int> FiltroTipoUsuario
        {
            get { return ucFiltroTipoUsuario.TipoUsuarioSeleccionados; }
        }
        public List<int> FiltroOrganizaciones
        {
            get { return ucFiltroOrganizacion.OrganizacionesSeleccionadas; }
        }
        public List<int> FiltroUbicaciones
        {
            get { return ucFiltroUbicacion.UbicacionesSeleccionadas; }
        }
        public List<int> FiltroTipoArbol
        {
            get { return ucFiltroServicioIncidente.TipoArbolSeleccionados; }
        }
        public List<int> FiltroTipificaciones
        {
            get { return ucFiltroTipificacion.TipificacionesSeleccionadas; }
        }
        public List<int> FiltroPrioridad
        {
            get { return ucFiltroPrioridad.ImpactosSeleccionados; }
        }
        public List<int> FiltroEstatus
        {
            get { return ucFiltroEstatus.EstatusSeleccionados; }
        }
        public List<bool?> FiltroSla
        {
            get { return ucFiltroSla.SlaSeleccionado; }
        }
        public List<bool?> FiltroVip
        {
            get { return ucFiltroVip.VipSeleccionado; }
        }
        public int TipoPeriodo
        {
            get { return ucFiltroFechasGrafico.TipoPeriodo; }
        }
        public Dictionary<string, DateTime> FiltroFechas
        {
            get { return ucFiltroFechasGrafico.RangoFechas; }
        }
        #endregion Propiedades publicas

        private List<string> _lstError = new List<string>();
        private List<string> Alerta
        {
            set
            {
                panelAlerta.Visible = value.Any();
                if (!panelAlerta.Visible) return;
                rptError.DataSource = value;
                rptError.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                ucFiltroGrupo.OnAceptarModal += ucFiltroGrupo_OnAceptarModal;
                ucFiltroGrupo.OnCancelarModal += ucFiltroGrupo_OnCancelarModal;

                ucFiltroCanalApertura.OnAceptarModal += UcFiltroCanalAperturaOnOnAceptarModal;
                ucFiltroCanalApertura.OnCancelarModal += UcFiltroCanalAperturaOnOnCancelarModal;

                ucFiltroTipoUsuario.OnAceptarModal += UcFiltroTipoUsuarioOnOnAceptarModal;
                ucFiltroTipoUsuario.OnCancelarModal += UcFiltroTipoUsuarioOnOnCancelarModal;

                ucFiltroOrganizacion.OnAceptarModal += ucFiltroOrganizacion_OnAceptarModal;
                ucFiltroOrganizacion.OnCancelarModal += ucFiltroOrganizacion_OnCancelarModal;

                ucFiltroUbicacion.OnAceptarModal += ucFiltroUbicacion_OnAceptarModal;
                ucFiltroUbicacion.OnCancelarModal += ucFiltroUbicacion_OnCancelarModal;

                ucFiltroServicioIncidente.OnAceptarModal += ucFiltroServicioIncidente_OnAceptarModal;
                ucFiltroServicioIncidente.OnCancelarModal += ucFiltroServicioIncidente_OnCancelarModal;

                ucFiltroTipificacion.OnAceptarModal += ucFiltroTipificacion_OnAceptarModal;
                ucFiltroTipificacion.OnCancelarModal += ucFiltroTipificacion_OnCancelarModal;

                ucFiltroPrioridad.OnAceptarModal += ucFiltroPrioridad_OnAceptarModal;
                ucFiltroPrioridad.OnCancelarModal += ucFiltroPrioridad_OnCancelarModal;

                ucFiltroEstatus.OnAceptarModal += ucFiltroEstatus_OnAceptarModal;
                ucFiltroEstatus.OnCancelarModal += UcFiltroEstatusOnOnCancelarModal;

                ucFiltroSla.OnAceptarModal += ucFiltroSla_OnAceptarModal;
                ucFiltroSla.OnCancelarModal += UcFiltroSlaOnOnCancelarModal;

                ucFiltroVip.OnAceptarModal += UcFiltroVipOnOnAceptarModal;
                ucFiltroVip.OnCancelarModal += UcFiltroVipOnOnCancelarModal;

                ucFiltroFechasGrafico.OnAceptarModal += ucFiltroFechas_OnAceptarModal;
                ucFiltroFechasGrafico.OnCancelarModal += ucFiltroFechas_OnCancelarModal;
                if (!IsPostBack)
                {
                    ucFiltroServicioIncidente.EsTicket = true;
                    upFiltroServicioIncidente.Update();
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

        #region Delegados
        void ucFiltroGrupo_OnAceptarModal()
        {
            try
            {
                btnFiltroGrupo.CssClass = ucFiltroGrupo.GruposSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ucFiltroOrganizacion.Grupos = ucFiltroGrupo.GruposSeleccionados;
                ucFiltroUbicacion.Grupos = ucFiltroGrupo.GruposSeleccionados;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroGrupo\");", true);
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
        void ucFiltroGrupo_OnCancelarModal()
        {
            try
            {
                btnFiltroGrupo.CssClass = ucFiltroGrupo.GruposSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroGrupo\");", true);
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

        private void UcFiltroCanalAperturaOnOnAceptarModal()
        {
            try
            {
                btnFiltroCanal.CssClass = ucFiltroCanalApertura.CanalesSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroCanal\");", true);
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
        private void UcFiltroCanalAperturaOnOnCancelarModal()
        {
            try
            {
                btnFiltroCanal.CssClass = ucFiltroCanalApertura.CanalesSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroCanal\");", true);
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

        void UcFiltroTipoUsuarioOnOnAceptarModal()
        {
            try
            {
                btnFiltroTipoUsuario.CssClass = ucFiltroTipoUsuario.TipoUsuarioSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroTipoUsuario\");", true);
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
        void UcFiltroTipoUsuarioOnOnCancelarModal()
        {
            try
            {
                btnFiltroTipoUsuario.CssClass = ucFiltroTipoUsuario.TipoUsuarioSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroTipoUsuario\");", true);
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

        void ucFiltroOrganizacion_OnAceptarModal()
        {
            try
            {
                btnFiltroOrganizacion.CssClass = ucFiltroOrganizacion.OrganizacionesSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroOrganizacion\");", true);
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
        void ucFiltroOrganizacion_OnCancelarModal()
        {
            try
            {
                btnFiltroOrganizacion.CssClass = ucFiltroOrganizacion.OrganizacionesSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroOrganizacion\");", true);
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

        void ucFiltroUbicacion_OnAceptarModal()
        {
            try
            {
                btnFiltroUbicacion.CssClass = ucFiltroUbicacion.UbicacionesSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroUbicacion\");", true);
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
        void ucFiltroUbicacion_OnCancelarModal()
        {
            try
            {
                btnFiltroUbicacion.CssClass = ucFiltroUbicacion.UbicacionesSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroUbicacion\");", true);
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

        void ucFiltroServicioIncidente_OnAceptarModal()
        {
            try
            {
                btnFiltroServicioIncidente.CssClass = ucFiltroServicioIncidente.TipoArbolSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                btnFiltroTipificacion.CssClass = ucFiltroServicioIncidente.TipoArbolSeleccionados.Count > 0 ? ucFiltroTipificacion.TipificacionesSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary" : "btn btn-primary disabled";
                ucFiltroTipificacion.TipoArbol = ucFiltroServicioIncidente.TipoArbolSeleccionados.First();
                upFiltroTipificacion.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroServicioIncidente\");", true);
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
        void ucFiltroServicioIncidente_OnCancelarModal()
        {
            try
            {
                btnFiltroServicioIncidente.CssClass = ucFiltroServicioIncidente.TipoArbolSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                btnFiltroTipificacion.CssClass = ucFiltroServicioIncidente.TipoArbolSeleccionados.Count > 0 ? ucFiltroTipificacion.TipificacionesSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary" : "btn btn-primary disabled";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroServicioIncidente\");", true);
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

        void ucFiltroTipificacion_OnAceptarModal()
        {
            try
            {
                btnFiltroTipificacion.CssClass = ucFiltroTipificacion.TipificacionesSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroTipificacion\");", true);
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
        void ucFiltroTipificacion_OnCancelarModal()
        {
            try
            {
                btnFiltroTipificacion.CssClass = ucFiltroTipificacion.TipificacionesSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroTipificacion\");", true);
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

        void ucFiltroPrioridad_OnAceptarModal()
        {
            try
            {
                btnFiltroPrioridad.CssClass = ucFiltroPrioridad.ImpactosSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroPrioridad\");", true);
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
        void ucFiltroPrioridad_OnCancelarModal()
        {
            try
            {
                btnFiltroPrioridad.CssClass = ucFiltroPrioridad.ImpactosSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroPrioridad\");", true);
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

        void ucFiltroEstatus_OnAceptarModal()
        {
            try
            {
                btnFiltroEstatus.CssClass = ucFiltroEstatus.EstatusSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroEstatus\");", true);
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
        void UcFiltroEstatusOnOnCancelarModal()
        {
            try
            {
                btnFiltroEstatus.CssClass = ucFiltroEstatus.EstatusSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroEstatus\");", true);
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

        void ucFiltroSla_OnAceptarModal()
        {
            try
            {
                btnFiltroSla.CssClass = ucFiltroSla.SlaSeleccionado.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroSla\");", true);
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
        void UcFiltroSlaOnOnCancelarModal()
        {
            try
            {
                btnFiltroSla.CssClass = ucFiltroSla.SlaSeleccionado.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroSla\");", true);
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

        void UcFiltroVipOnOnCancelarModal()
        {
            try
            {
                btnFiltroVip.CssClass = ucFiltroVip.VipSeleccionado.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroVip\");", true);
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
        void UcFiltroVipOnOnAceptarModal()
        {
            try
            {
                btnFiltroVip.CssClass = ucFiltroVip.VipSeleccionado.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroVip\");", true);
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

        void ucFiltroFechas_OnAceptarModal()
        {
            try
            {
                btnFiltroFechas.CssClass = ucFiltroFechasGrafico.RangoFechas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroFechas\");", true);
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
        void ucFiltroFechas_OnCancelarModal()
        {
            try
            {
                btnFiltroFechas.CssClass = ucFiltroFechasGrafico.RangoFechas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroFechas\");", true);
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
        #endregion Delegados

        #region AbreModales
        protected void btnFiltroGrupo_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroGrupo\");", true);
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

        protected void btnFiltroCanal_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroCanal\");", true);
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

        protected void btnFiltroTipoUsuario_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroTipoUsuario\");", true);
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

        protected void btnFiltroOrganizacion_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroOrganizacion\");", true);
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

        protected void btnFiltroUbicacion_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroUbicacion\");", true);
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

        protected void btnFiltroServicioIncidente_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroServicioIncidente\");", true);
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

        protected void btnFiltroTipificacion_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroTipificacion\");", true);
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

        protected void btnFiltroPrioridad_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroPrioridad\");", true);
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

        protected void btnFiltroEstatus_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroEstatus\");", true);
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

        protected void btnFiltroSla_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroSla\");", true);
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

        protected void btnFiltroVip_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroVip\");", true);
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

        protected void btnFiltroFechas_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroFechas\");", true);

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
        #endregion AbreModales

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        protected void btnGraficar_Click(object sender, EventArgs e)
        {
            try
            {
                if (OnAceptarModal != null)
                {
                    OnAceptarModal();
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

    }
}