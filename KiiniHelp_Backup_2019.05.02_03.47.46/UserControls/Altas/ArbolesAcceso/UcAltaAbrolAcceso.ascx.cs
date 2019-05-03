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
            ucAltaConsulta.OnAceptarModal += UcAltaConsulta_OnAceptarModal;
            ucAltaConsulta.OnCancelarModal += UcAltaConsulta_OnCancelarModal;
            ucAltaServicio.OnAceptarModal += UcAltaServicio_OnAceptarModal;
            ucAltaServicio.OnCancelarModal += UcAltaServicio_OnCancelarModal;
        }



        void UcAltaConsulta_OnAceptarModal()
        {
            if (OnAceptarModal != null)
                OnAceptarModal();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAtaOpcion\");", true);
        }
        void UcAltaConsulta_OnCancelarModal()
        {
            ucAltaServicio.LimpiarPantalla();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAtaOpcion\");", true);
        }
        void UcAltaServicio_OnCancelarModal()
        {
            if (OnAceptarModal != null)
                OnAceptarModal();
            ucAltaConsulta.LimpiarPantalla();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAtaOpcion\");", true);
        }
        void UcAltaServicio_OnAceptarModal()
        {
            ucAltaConsulta.LimpiarPantalla();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAtaOpcion\");", true);
        }


        public void Cancelar()
        {
            try
            {
                ucAltaConsulta.LimpiarPantalla();
                ucAltaServicio.LimpiarPantalla();
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