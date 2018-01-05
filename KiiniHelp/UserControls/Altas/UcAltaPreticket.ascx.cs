using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaPreticket : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTicketClient _servicioticket = new ServiceTicketClient();
        private List<string> _lstError = new List<string>();

        private List<string> Alerta
        {
            set
            {
                panelAlerta.Visible = value.Any();
                if (!panelAlerta.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }

        public int IdArbol
        {
            get { return Convert.ToInt32(hfIdArbol.Value); }
            set
            {
                hfIdArbol.Value = value.ToString();
            }
        }

        public int IdUsuarioSolicito
        {
            get { return Convert.ToInt32(hfIdUsuarioSolicito.Value); }
            set
            {
                hfIdUsuarioSolicito.Value = value.ToString();
            }
        }

        public int IdUsuarioLevanto
        {
            get { return Convert.ToInt32(hfIdUsuarioLevanta.Value); }
            set
            {
                hfIdUsuarioLevanta.Value = value.ToString();
            }
        }

        private void LimpiarCampos()
        {
            try
            {
                txtObservaciones.Text = String.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
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

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtObservaciones.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar una descripción");
                PreTicket result = _servicioticket.GeneraPreticket(IdArbol, IdUsuarioSolicito, IdUsuarioLevanto, txtObservaciones.Text);
                lblNoTicket.Text = result.Id.ToString();
                lblDescRandom.Visible = true;
                lblRandom.Visible = true;
                lblRandom.Text = result.ClaveRegistro;
                upConfirmacion.Update();
                LimpiarCampos();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalExito\");", true);
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
                LimpiarCampos();
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
                LimpiarCampos();
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

        protected void btnCerrar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalExito\");", true);
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
    }
}