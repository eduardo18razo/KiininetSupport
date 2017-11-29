using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceOrganizacion;
using KiiniHelp.ServiceUbicacion;

namespace KiiniHelp.UserControls.Filtros.Graficos
{
    public partial class UcFiltrosParametrosGraficoEficienciaTickets : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceUbicacionClient _servicioUbicacion = new ServiceUbicacionClient();
        private readonly ServiceOrganizacionClient _servicioOrganizacion = new ServiceOrganizacionClient();
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

        public string TipoGrafico
        {
            get
            {
                string result = string.Empty;
                if (rbtnGeografico.Checked)
                    result = rbtnGeografico.Text;
                else if (rbtnTendenciaStack.Checked)
                    result = rbtnTendenciaStack.Text;
                else if (rbtnTendenciaBarraCompetitiva.Checked)
                    result = rbtnTendenciaBarraCompetitiva.Text;

                return result;
            }
        }

        public string Stack
        {
            get
            {
                string result = string.Empty;
                if (rbtnUbicaciones.Checked)
                    result = rbtnUbicaciones.Text;
                else if (rbtnOrganizaciones.Checked)
                    result = rbtnOrganizaciones.Text;
                else if (rbtnTipoTicket.Checked)
                    result = rbtnTipoTicket.Text;
                else if (rbtnTipificaciones.Checked)
                    result = rbtnTipificaciones.Text;
                else if (rbtnEstaus.Checked)
                    result = rbtnEstaus.Text;
                else if (rbtnSla.Checked)
                    result = rbtnSla.Text;
                return result;
            }

        }
        

        private void Limpiar()
        {
            try
            {
                rbtnGeografico.Checked = false;
                rbtnTendenciaStack.Checked = false;
                rbtnTendenciaBarraCompetitiva.Checked = false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
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

        #region TipoGrafico
        public bool Geografico
        {
            get { return rbtnGeografico.Checked; }
        }
        public bool TendenciaStack
        {
            get { return rbtnTendenciaStack.Checked; }
        }
        public bool TendenciaBarraCompetitiva
        {
            get { return rbtnTendenciaBarraCompetitiva.Checked; }
        }

        #endregion TipoGrafico

        protected void btnGenerar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (OnAceptarModal != null)
                    OnAceptarModal();
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
        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Limpiar();
                if (OnLimpiarModal != null)
                    OnLimpiarModal();
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
        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (OnCancelarModal != null)
                    OnCancelarModal();
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