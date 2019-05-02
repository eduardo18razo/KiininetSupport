using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.Publico.Consultas
{
    public partial class FrmConsultaTicket : Page
    {
        private readonly ServiceTicketClient _servicioticket = new ServiceTicketClient();

        private List<string> _lstError = new List<string>();
        private bool ValidCaptcha = false;

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

        private bool EsLink
        {
            get { return bool.Parse(hfLink.Value); }
            set { hfLink.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (Request.Params["idTicket"] != null && Request.Params["cveRandom"] != null)
                {
                    EsLink = true;
                    txtTicket.Text = Request.Params["idTicket"];
                    txtClave.Text = Request.Params["cveRandom"];
                    btnConsultar_OnClick(btnConsultar, null);
                }
                else
                {
                    EsLink = false;
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

        protected void btnConsultar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!ValidCaptcha && !EsLink)
                {
                    txtCaptcha.Text = string.Empty;
                    throw new Exception("Captcha incorrecto");
                }
                if (txtTicket.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese número de ticket");
                if (txtClave.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese clave de registro");
                HelperDetalleTicket detalle = _servicioticket.ObtenerDetalleTicketNoRegistrado(int.Parse(txtTicket.Text.Trim()), txtClave.Text.Trim());
                if (detalle != null)
                {
                    divTitle.Visible = false;
                    divConsulta.Visible = false;
                    divDetalle.Visible = true;
                    divDetalleTicket.Visible = true;
                    ucTicketDetalleUsuario.IdUsuario = detalle.IdUsuarioLevanto;
                    ucTicketDetalleUsuario.IdTicket = detalle.IdTicket;
                    hfMuestraEncuesta.Value = detalle.TieneEncuesta.ToString();
                }
                else
                {
                    throw new Exception("Datos incorrectos");
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
        protected void OnServerValidate(object source, ServerValidateEventArgs e)
        {
            try
            {
                if (txtCaptcha.Text.Trim() == string.Empty) return;
                captchaTicket.ValidateCaptcha(txtCaptcha.Text.Trim());
                e.IsValid = captchaTicket.UserValidated;
                ValidCaptcha = e.IsValid;
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