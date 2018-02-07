using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceSistemaEstatus;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Operacion
{
    public partial class UcCambiarEstatusTicket : UserControl, IControllerModal
    {
        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();
        private readonly ServiceAtencionTicketClient _servicioAtencionTicket = new ServiceAtencionTicketClient();
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private List<string> _lstError = new List<string>();

        public int IdTicket
        {
            get { return Convert.ToInt32(lblIdticket.Text); }
            set { lblIdticket.Text = value.ToString(); }
        }
        public int IdUsuario
        {
            get { return Convert.ToInt32(ViewState["IdUsuarioTicket"]); }
            set
            {
                ViewState["IdUsuarioTicket"] = value;
                LLenaEstatus();
            }
        }
        public int IdGrupo
        {
            get { return Convert.ToInt32(ViewState["IdGrupoTicket"]); }
            set
            {
                ViewState["IdGrupoTicket"] = value;
            }
        }
        public int IdEstatusActual
        {
            get { return int.Parse(hfEstatusActual.Value); }
            set { hfEstatusActual.Value = value.ToString(); }
        }
        public bool CerroTicket
        {
            get { return Convert.ToBoolean(hfTicketCerrado.Value); }
            set { hfTicketCerrado.Value = value.ToString(); }
        }

        public bool EsPropietario
        {
            get { return ddlEstatus.Enabled; }
            set { ddlEstatus.Enabled = value; }
        }

        private void LLenaEstatus()
        {
            try
            {
                ddlEstatus.DataSource = _servicioEstatus.ObtenerEstatusTicketUsuario(IdUsuario, IdGrupo, IdEstatusActual, EsPropietario, true);
                ddlEstatus.DataTextField = "Descripcion";
                ddlEstatus.DataValueField = "Id";
                ddlEstatus.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                //lblBrandingModal.Text = WebConfigurationManager.AppSettings["Brand"];
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

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlEstatus.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Debe seleccionar un estatus");

                if (ddlEstatus.SelectedValue != BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString())
                {
                    CerroTicket = Convert.ToInt32(ddlEstatus.SelectedValue) == (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado;
                    _servicioAtencionTicket.CambiarEstatus(IdTicket, Convert.ToInt32(ddlEstatus.SelectedValue), IdUsuario, txtComentarios.Text.Trim());
                }
                
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