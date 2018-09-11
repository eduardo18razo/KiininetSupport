using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Graficos.Eficiencia
{
    public partial class UcFiltrosTicketTipoTicket : UserControl, IControllerModal
    {
        #region Propiedades publicas
        public List<int> FiltroEstatus
        {
            get { return ucFiltroEstatus.EstatusSeleccionados; }
        }
        public List<int> FiltroTipoUsuario
        {
            get { return ucFiltroTipoUsuario.TipoUsuarioSeleccionados; }
        }
        public List<int> FiltroCategorias
        {
            get { return ucFiltroCategoria.CategoriasSeleccionadas; }
        }
        public List<int> FiltroGrupos
        {
            get { return ucFiltroGrupo.GruposSeleccionados; }
        }
        public List<int> FiltroAgentes
        {
            get { return ucFiltroAtendedores.AtendedoresSeleccionados; }
        }
        public List<int> FiltroEstatusAsignacion
        {
            get { return ucFiltroEstatusAsignacion.EstatusSeleccionados; }
        }
        public List<int> FiltroCanalesApertura
        {
            get { return ucFiltroCanalApertura.CanalesSeleccionados; }
        }
        public List<int> FiltroTipoArbol
        {
            get { return ucFiltroServicioIncidente.TipoArbolSeleccionados; }
        }

        public List<int> FiltroTipificacion
        {
            get { return ucFiltroTipificacion.TipificacionesSeleccionadas; }
        }
        public List<bool?> FiltroSla
        {
            get { return ucFiltroSla.SlaSeleccionado; }
        }
        public List<int> FiltroOrganizaciones
        {
            get { return ucFiltroOrganizacion.OrganizacionesSeleccionadas; }
        }
        public List<int> FiltroUbicaciones
        {
            get { return ucFiltroUbicacion.UbicacionesSeleccionadas; }
        }
        public int TipoPeriodo
        {
            get { return ucFiltroFechasGrafico.TipoPeriodo; }
        }
        public Dictionary<string, DateTime> FiltroFechas
        {
            get { return ucFiltroFechasGrafico.RangoFechas; }
        }

        public void ObtenerParametros()
        {
            try
            {
                ucFiltroFechasGrafico.ObtenerFechasParametro();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion Propiedades publicas

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
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
                ucFiltroTipoUsuario.OnAceptarModal += ucFiltroTipoUsuario_OnAceptarModal;
                ucFiltroServicioIncidente.OnAceptarModal += ucFiltroServicioIncidente_OnAceptarModal;
                if (!IsPostBack)
                {
                    ucFiltroServicioIncidente.EsTicket = true;
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
        void ucFiltroTipoUsuario_OnAceptarModal()
        {
            try
            {
                ucFiltroTipificacion.TipoUsuario = ucFiltroTipoUsuario.TipoUsuarioSeleccionados;
                ucFiltroOrganizacion.TipoUsuario = ucFiltroTipoUsuario.TipoUsuarioSeleccionados;
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
                ucFiltroTipificacion.TipoArbol = ucFiltroServicioIncidente.TipoArbolSeleccionados;
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

        protected void btnGraficar_OnClick(object sender, EventArgs e)
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