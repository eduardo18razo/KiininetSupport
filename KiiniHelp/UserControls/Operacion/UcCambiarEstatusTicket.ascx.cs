using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceSistemaEstatus;
using KiiniNet.Entities.Cat.Sistema;
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

        public int IdEstatusSeleccionado
        {
            get { return Convert.ToInt32(rbtnLstEstatus.SelectedValue); }
        }

        public int? IdSubRolActual
        {
            get { return string.IsNullOrEmpty(hfIdSubRolActual.Value) ? null : (int?)int.Parse(hfIdSubRolActual.Value); }
            set { hfIdSubRolActual.Value = value.ToString(); }
        }
        public bool CerroTicket
        {
            get { return Convert.ToBoolean(hfTicketCerrado.Value); }
            set { hfTicketCerrado.Value = value.ToString(); }
        }

        public bool EsPropietario
        {
            get { return rbtnLstEstatus.Enabled; }
            set { rbtnLstEstatus.Enabled = value; }
        }

        public bool EsPublico
        {
            get { return bool.Parse(hfPublico.Value); }
            set { hfPublico.Value = value.ToString(); }
        }
        private void LLenaEstatus()
        {
            try
            {
                List<EstatusTicket> lst = null;
                if (EsPublico)
                    lst = _servicioEstatus.ObtenerEstatusTicketUsuarioPublico(IdTicket, IdGrupo, IdEstatusActual, EsPropietario, IdSubRolActual, false);
                else
                    lst = _servicioEstatus.ObtenerEstatusTicketUsuario(IdUsuario, IdGrupo, IdEstatusActual, EsPropietario, IdSubRolActual, false);
                rbtnLstEstatus.DataSource = lst;
                rbtnLstEstatus.DataTextField = "Descripcion";
                rbtnLstEstatus.DataValueField = "Id";
                rbtnLstEstatus.DataBind();
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
                if (rbtnLstEstatus.SelectedValue == string.Empty)
                    throw new Exception("Debe seleccionar un estatus");

                if (int.Parse(rbtnLstEstatus.SelectedValue) <= BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                    throw new Exception("Debe seleccionar un estatus");
                
                if (_servicioEstatus.HasCambioEstatusComentarioObligatorio(Session["UserData"] != null ? (int?)((Usuario)Session["UserData"]).Id : null, IdTicket, Convert.ToInt32(rbtnLstEstatus.SelectedValue), EsPropietario, EsPublico))
                    if (txtComentarios.Text.Trim() == string.Empty)
                        throw new Exception("Debe agregar un comentario");

                if (rbtnLstEstatus.SelectedValue != BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString())
                {
                    CerroTicket = Convert.ToInt32(rbtnLstEstatus.SelectedValue) == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado;
                    _servicioAtencionTicket.CambiarEstatus(IdTicket, Convert.ToInt32(rbtnLstEstatus.SelectedValue), IdUsuario, txtComentarios.Text.Trim());
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