using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Graficos
{
    public partial class UcFiltrosParametrosGraficoEncuestas : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

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
                else if (rbtnTendenciaLinear.Checked)
                    result = rbtnTendenciaLinear.Text;
                else if (rbtnTendenciaBarraCompetitiva.Checked)
                    result = rbtnTendenciaBarraCompetitiva.Text;

                return result;
            }
        }

        private void Limpiar()
        {
            try
            {
                rbtnGeografico.Checked = false;
                rbtnTendenciaLinear.Checked = false;
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
        public bool TendenciaLinear
        {
            get { return rbtnTendenciaLinear.Checked; }
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