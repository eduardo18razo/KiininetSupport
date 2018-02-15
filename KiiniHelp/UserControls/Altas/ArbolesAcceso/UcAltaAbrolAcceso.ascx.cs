using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.UserControls.Altas.ArbolesAcceso
{
    public partial class UcAltaAbrolAcceso : UserControl, IControllerModal
    {
        private List<string> _lstError = new List<string>();

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        protected void Page_Load(object sender, EventArgs e)
        {
            UcAltaConsulta.OnCancelarModal += UcAltaConsulta_OnCancelarModal;
            UcAltaServicio.OnCancelarModal += UcAltaServicio_OnCancelarModal;

        }

        void UcAltaServicio_OnCancelarModal()
        {
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAtaOpcion\");", true);
        }

        void UcAltaConsulta_OnCancelarModal()
        {

            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAtaOpcion\");", true);
        }

        public void Cancelar()
        {
            try
            {
                UcAltaConsulta.LimpiarPantalla();
                UcAltaServicio.LimpiarPantalla();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
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