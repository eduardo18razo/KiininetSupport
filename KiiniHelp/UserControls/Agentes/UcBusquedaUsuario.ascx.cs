using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Agentes
{
    public partial class UcBusquedaUsuario : UserControl, IControllerModal
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
                if (value.Any())
                {
                    string error = value.Aggregate("<ul>", (current, s) => current + ("<li>" + s + "</li>"));
                    error += "</ul>";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('Error','" + error + "');", true);
                }
            }
        }

        public int IdUsuario
        {
            get
            {
                if (rtxtUsuario.Entries.Count <= 0)
                    throw new Exception("Ingrese un usuario");
                return int.Parse(rtxtUsuario.Entries[0].Value);
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                rtxtUsuario.TextSettings.SelectionMode = RadAutoCompleteSelectionMode.Single;
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