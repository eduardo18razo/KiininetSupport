using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Filtros.Graficos
{
    public partial class UcFiltrosGraficasHits : UserControl, IControllerModal
    {
        #region Propiedades publicas
        public List<int> FiltroGrupos
        {
            get { return ucFiltroGrupo.GruposSeleccionados; }
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
        public List<int> FiltroTipificaciones
        {
            get { return ucFiltroTipificacion.TipificacionesSeleccionadas; }
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

                ucFiltroTipoUsuario.OnAceptarModal += UcFiltroTipoUsuarioOnOnAceptarModal;
                ucFiltroTipoUsuario.OnCancelarModal += UcFiltroTipoUsuarioOnOnCancelarModal;

                ucFiltroOrganizacion.OnAceptarModal += ucFiltroOrganizacion_OnAceptarModal;
                ucFiltroOrganizacion.OnCancelarModal += ucFiltroOrganizacion_OnCancelarModal;

                ucFiltroUbicacion.OnAceptarModal += ucFiltroUbicacion_OnAceptarModal;
                ucFiltroUbicacion.OnCancelarModal += ucFiltroUbicacion_OnCancelarModal;

                ucFiltroTipificacion.OnAceptarModal += ucFiltroTipificacion_OnAceptarModal;
                ucFiltroTipificacion.OnCancelarModal += ucFiltroTipificacion_OnCancelarModal;

                ucFiltroVip.OnAceptarModal += UcFiltroVipOnOnAceptarModal;
                ucFiltroVip.OnCancelarModal += UcFiltroVipOnOnCancelarModal;

                ucFiltroFechasGrafico.OnAceptarModal += ucFiltroFechas_OnAceptarModal;
                ucFiltroFechasGrafico.OnCancelarModal += ucFiltroFechas_OnCancelarModal;
                if (!IsPostBack)
                {
                    ucFiltroTipificacion.TipoArbol = (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
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