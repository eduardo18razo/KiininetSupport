using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniHelp
{
    public partial class FrmConfirmarTicket : Page
    {
        private readonly ServiceTicketClient _servicioTicket = new ServiceTicketClient();
        private bool _validCaptcha = false;
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


        private void CargaDatosPreTicket(string guid)
        {
            try
            {
                PreTicketCorreo preticket = _servicioTicket.ObtenerPreticketCorreo(guid);
                if (preticket != null)
                {
                    txtAsunto.Text = preticket.Asunto;
                    txtComentarios.Text = preticket.Comentario;
                    ucAltaUsuarioRapida.LLenaDatosDeCorreo(preticket.Nombre, preticket.ApellidoPaterno, preticket.ApellidoMaterno, preticket.Correo);
                }
                else
                    Response.Redirect("~/Default.aspx");

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
                lblBrandingModal.Text = WebConfigurationManager.AppSettings["Brand"];
                Alerta = new List<string>();
                ucAltaUsuarioRapida.OnAceptarModal += ucAltaUsuarioRapida_OnAceptarModal;
                txtComentarios.Text = Server.HtmlDecode(txtComentarios.Text);
                if (!IsPostBack)
                {
                    if (Request.Params["confirmacionguid"] != null)
                    {
                        CargaDatosPreTicket(Request.Params["confirmacionguid"]);
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx");
                    }
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

        protected void btnRegistrar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!_validCaptcha)
                {
                    txtCaptcha.Text = string.Empty;
                    throw new Exception("Captcha incorrecto");
                }
                if (!ucAltaUsuarioRapida.ValidaUsuarioExiste())
                {
                    ucAltaUsuarioRapida.RegistraUsuario();
                    _servicioTicket.ConfirmaPreTicket(Request.Params["confirmacionguid"], ucAltaUsuarioRapida.IdUsuario);
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

        void ucAltaUsuarioRapida_OnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptOpen", "MostrarPopup(\"#modalRegistroExito\");", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnCerrarExito_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Default.aspx");
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void OnServerValidate(object source, ServerValidateEventArgs e)
        {
            try
            {
                if (txtCaptcha.Text.Trim() == string.Empty) return;
                captchaTicket.ValidateCaptcha(txtCaptcha.Text.Trim());
                e.IsValid = captchaTicket.UserValidated;
                _validCaptcha = e.IsValid;
                if (!e.IsValid)
                {
                    throw new Exception("Captcha incorrecto.");
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                    _lstError.Add(ex.Message);
                }

                Alerta = _lstError;
            }
        }
    }
}