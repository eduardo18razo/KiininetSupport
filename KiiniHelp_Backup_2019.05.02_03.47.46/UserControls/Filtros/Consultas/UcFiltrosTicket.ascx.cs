using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Consultas
{
    public partial class UcFiltrosTicket : UserControl, IControllerModal
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
        public Dictionary<string, DateTime> FiltroFechas
        {
            get { return ucFiltroFechasConsultas.RangoFechas; }
        }

        //private readonly ServiceConsultasClient _servicioConsultas = new ServiceConsultasClient();
        //public event DelegateAceptarModal btnConsultar_OnClick;

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
        void ucFiltroGrupo_OnAceptarModal()
        {
            try
            {
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


        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        protected void btnConsultar_Click(object sender, EventArgs e)
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