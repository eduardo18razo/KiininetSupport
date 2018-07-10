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
                if (value.Any())
                {
                    string error = value.Aggregate("<ul>", (current, s) => current + ("<li>" + s + "</li>"));
                    error += "</ul>";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('Error','" + error + "');", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                ucFiltroGrupo.OnAceptarModal += ucFiltroGrupo_OnAceptarModal;
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
                ucFiltroOrganizacion.Grupos = ucFiltroGrupo.GruposSeleccionados;
                ucFiltroUbicacion.Grupos = ucFiltroGrupo.GruposSeleccionados;
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