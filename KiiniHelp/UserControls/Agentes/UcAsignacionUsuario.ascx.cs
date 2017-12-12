using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceUsuario;

namespace KiiniHelp.UserControls.Agentes
{
    public partial class UcAsignacionUsuario : UserControl, IControllerModal
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

        protected void btnSeleccionarUsuario_OnClick(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = 0;
                switch (int.Parse(hfSelected.Value))
                {
                    case 1:
                        idUsuario = ucBusquedaUsuario.IdUsuario;
                        break;
                    case 2:
                        ucAltaUsuarioRapida.RegistraUsuario();
                        idUsuario = ucAltaUsuarioRapida.IdUsuario;
                        break;
                }
                AgenteMaster master = Page.Master as AgenteMaster;
                if (master != null)
                {
                    master.AddNewTicket(idUsuario);
                }
                Response.Redirect("~/Agente/FrmAgenteNuevoTicket.aspx?idUsuarioSolicitante=" + idUsuario);
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