using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Consultas
{
    public partial class UcFiltrosEncuestas : UserControl, IControllerModal
    {
        #region Propiedades publicas
        public List<int> FiltroGrupos
        {
            get { return ucFiltroGrupoEncuesta.GruposSeleccionados; }
        }
        public List<int> FiltroTipoArbol
        {
            get { return ucFiltroServicioIncidenteEncuesta.TipoArbolSeleccionados; }
        }
        public List<int> FiltroResponsables
        {
            get { return ucFiltroResponsablesEncuesta.GruposSeleccionados; }
        }

        public List<int?> FiltroEncuestas
        {
            get { return ucFiltroEncuesta.EncuestasSeleccionadas; }
        }

        public List<int> FiltroAtendedores
        {
            get { return ucFiltroAtendedores.AtendedoresSeleccionados; }
        }

        public Dictionary<string, DateTime> FiltroFechas
        {
            get { return ucFiltroFechasConsultas.RangoFechas; }
        }

        public List<int> FiltroTipoUsuario
        {
            get { return ucFiltroTipoUsuario.TipoUsuarioSeleccionados; }
        }
        public List<int> FiltroPrioridad
        {
            get { return ucFiltroPrioridad.ImpactosSeleccionados; }
        }
        public List<bool?> FiltroSla
        {
            get { return ucFiltroSla.SlaSeleccionado; }
        }
        public List<int> FiltroUbicaciones
        {
            get { return ucFiltroUbicacion.UbicacionesSeleccionadas; }
        }
        public List<int> FiltroOrganizaciones
        {
            get { return ucFiltroOrganizacion.OrganizacionesSeleccionadas; }
        }
        public List<bool?> FiltroVip
        {
            get { return ucFiltroVip.VipSeleccionado; }
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
                ucFiltroGrupoEncuesta.OnAceptarModal += ucFiltroGrupoEncuesta_OnAceptarModal;

                ucFiltroServicioIncidenteEncuesta.OnAceptarModal += ucFiltroServicioIncidente_OnAceptarModal;

                ucFiltroEncuesta.OnAceptarModal += UcFiltroEncuestaOnOnAceptarModal;

                
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
        
        void ucFiltroGrupoEncuesta_OnAceptarModal()
        {
            try
            {
                ucFiltroServicioIncidenteEncuesta.Grupos = ucFiltroGrupoEncuesta.GruposSeleccionados;
                ucFiltroResponsablesEncuesta.LlenaGrupos(ucFiltroGrupoEncuesta.GruposSeleccionados, ucFiltroServicioIncidenteEncuesta.TipoArbolSeleccionados);
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
                //TODO: Filtra Responsables
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

        private void UcFiltroEncuestaOnOnAceptarModal()
        {
            try
            {
                ucFiltroAtendedores.Encuestas = ucFiltroEncuesta.EncuestasSeleccionadas;
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