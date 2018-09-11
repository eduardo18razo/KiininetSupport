using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Graficos
{
    public partial class UcFiltrosGraficasEncuestas : UserControl, IControllerModal
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
            get { return ucFiltroFechasGrafico.RangoFechas; }
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
        public int TipoPeriodo
        {
            get { return ucFiltroFechasGrafico.TipoPeriodo; }
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
                ucFiltroGrupoEncuesta.OnAceptarModal += ucFiltroGrupoEncuesta_OnAceptarModal;
                ucFiltroGrupoEncuesta.OnCancelarModal += ucFiltroGrupoEncuesta_OnCancelarModal;

                ucFiltroServicioIncidenteEncuesta.OnAceptarModal += ucFiltroServicioIncidente_OnAceptarModal;
                ucFiltroServicioIncidenteEncuesta.OnCancelarModal += ucFiltroServicioIncidente_OnCancelarModal;

                ucFiltroResponsablesEncuesta.OnAceptarModal += UcFiltroResponsablesEncuestaOnOnAceptarModal;
                ucFiltroResponsablesEncuesta.OnCancelarModal += UcFiltroResponsablesEncuestaOnOnCancelarModal;

                ucFiltroEncuesta.OnAceptarModal += UcFiltroEncuestaOnOnAceptarModal;
                ucFiltroEncuesta.OnCancelarModal += UcFiltroEncuestaOnOnCancelarModal;

                ucFiltroAtendedores.OnAceptarModal += UcFiltroAtendedoresOnOnAceptarModal;
                ucFiltroAtendedores.OnCancelarModal += UcFiltroAtendedoresOnOnCancelarModal;

                ucFiltroFechasGrafico.OnAceptarModal += ucFiltroFechas_OnAceptarModal;
                ucFiltroFechasGrafico.OnCancelarModal += ucFiltroFechas_OnCancelarModal;

                ucFiltroTipoUsuario.OnAceptarModal += UcFiltroTipoUsuarioOnOnAceptarModal;
                ucFiltroTipoUsuario.OnCancelarModal += UcFiltroTipoUsuarioOnOnCancelarModal;

                ucFiltroPrioridad.OnAceptarModal += ucFiltroPrioridad_OnAceptarModal;
                ucFiltroPrioridad.OnCancelarModal += ucFiltroPrioridad_OnCancelarModal;

                ucFiltroSla.OnAceptarModal += ucFiltroSla_OnAceptarModal;
                ucFiltroSla.OnCancelarModal += UcFiltroSlaOnOnCancelarModal;

                ucFiltroUbicacion.OnAceptarModal += ucFiltroUbicacion_OnAceptarModal;
                ucFiltroUbicacion.OnCancelarModal += ucFiltroUbicacion_OnCancelarModal;

                ucFiltroOrganizacion.OnAceptarModal += ucFiltroOrganizacion_OnAceptarModal;
                ucFiltroOrganizacion.OnCancelarModal += ucFiltroOrganizacion_OnCancelarModal;

                ucFiltroVip.OnAceptarModal += UcFiltroVipOnOnAceptarModal;
                ucFiltroVip.OnCancelarModal += UcFiltroVipOnOnCancelarModal;


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
        void ucFiltroGrupoEncuesta_OnAceptarModal()
        {
            try
            {
                btnFiltroGrupo.CssClass = ucFiltroGrupoEncuesta.GruposSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ucFiltroServicioIncidenteEncuesta.Grupos = ucFiltroGrupoEncuesta.GruposSeleccionados;
                ucFiltroResponsablesEncuesta.LlenaGrupos(ucFiltroGrupoEncuesta.GruposSeleccionados, ucFiltroServicioIncidenteEncuesta.TipoArbolSeleccionados);
                ucFiltroEncuesta.Grupos = ucFiltroGrupoEncuesta.GruposSeleccionados;
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
        void ucFiltroGrupoEncuesta_OnCancelarModal()
        {
            try
            {
                btnFiltroGrupo.CssClass = ucFiltroGrupoEncuesta.GruposSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
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

        void ucFiltroServicioIncidente_OnAceptarModal()
        {
            try
            {
                btnFiltroServicioIncidente.CssClass = ucFiltroServicioIncidenteEncuesta.TipoArbolSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                //TODO: Filtra Responsables
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
                btnFiltroServicioIncidente.CssClass = ucFiltroServicioIncidenteEncuesta.TipoArbolSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
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

        private void UcFiltroResponsablesEncuestaOnOnAceptarModal()
        {
            try
            {
                btnFiltroResponsables.CssClass = ucFiltroResponsablesEncuesta.GruposSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroResponsable\");", true);
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
        private void UcFiltroResponsablesEncuestaOnOnCancelarModal()
        {
            try
            {
                btnFiltroResponsables.CssClass = ucFiltroResponsablesEncuesta.GruposSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroResponsable\");", true);
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
                btnFiltroEncuestas.CssClass = ucFiltroEncuesta.EncuestasSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ucFiltroAtendedores.Encuestas = ucFiltroEncuesta.EncuestasSeleccionadas;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroEncuestas\");", true);
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
        private void UcFiltroEncuestaOnOnCancelarModal()
        {
            try
            {
                btnFiltroEncuestas.CssClass = ucFiltroEncuesta.EncuestasSeleccionadas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroEncuestas\");", true);
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

        private void UcFiltroAtendedoresOnOnAceptarModal()
        {
            try
            {
                btnFiltroAtendedores.CssClass = ucFiltroAtendedores.AtendedoresSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroAtendedores\");", true);
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
        private void UcFiltroAtendedoresOnOnCancelarModal()
        {
            try
            {
                btnFiltroAtendedores.CssClass = ucFiltroAtendedores.AtendedoresSeleccionados.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroAtendedores\");", true);
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

        private void UcFiltroTipoUsuarioOnOnAceptarModal()
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
        private void UcFiltroTipoUsuarioOnOnCancelarModal()
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



        private void UcFiltroVipOnOnCancelarModal()
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
        private void UcFiltroVipOnOnAceptarModal()
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
        protected void btnFiltroResponsables_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroResponsable\");", true);
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
        protected void btnFiltroEncuestas_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroEncuestas\");", true);
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
        protected void btnFiltroAtendedores_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroAtendedores\");", true);
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