using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using System.Web.UI;

namespace KiiniHelp
{
    public partial class ConfirmacionCuenta : Page
    {
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
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

        private void SendNotificacion(int idUsuario, int idTelefono)
        {
            try
            {
                _servicioUsuarios.EnviaCodigoVerificacionSms(idUsuario, (int)BusinessVariables.EnumTipoLink.Confirmacion, idTelefono);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void ValidaCaptura()
        {
            try
            {
                if (txtContrasena.Text.Trim() == string.Empty)
                    throw new Exception("Contraseña es campo obligatorio");
                if (txtContrasena.Text.Trim() != txtConfirmaContrasena.Text.Trim())
                    throw new Exception("Las contraseñas no coinciden");
                if (txtCodigo.Text.Trim() == string.Empty)
                    throw new Exception(string.Format("el numero de telefono {0} no ha sido confirmado", txtNumeroEdit.Text));
                if (txtPregunta.Text.Trim() == string.Empty || txtRespuesta.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar una pregunta y respuesta");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void CondicioinPassword()
        {
            try
            {
                ParametrosGenerales parametrosGenerales = _servicioParametros.ObtenerParametrosGenerales();
                if (parametrosGenerales != null)
                {
                    ParametroPassword parametrosPassword = _servicioParametros.ObtenerParemtrosPassword();
                    if (parametrosPassword != null)
                    {
                        lblCaracteristicas.Visible = parametrosGenerales.StrongPassword;
                        listParamtros.Visible = parametrosGenerales.StrongPassword;
                        lblLongitud.Text = string.Format("Longitud minima de {0} caracteres", parametrosPassword.Min);
                        lblLongitud.Text = string.Format("{0} Mayuscula", parametrosPassword.Mayusculas);
                        paramMayuscula.Visible = parametrosPassword.Mayusculas > 0;
                        paramNumero.Visible = parametrosPassword.Numeros;
                        paramEspecial.Visible = parametrosPassword.Especiales;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void CargaTelefonosObligatorios(int idUsuario)
        {
            try
            {
                Usuario userData = _servicioUsuarios.ObtenerDetalleUsuario(idUsuario);
                TelefonoUsuario telefono = userData.TelefonoUsuario.Where(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular && w.Obligatorio && !w.Confirmado).OrderBy(o => o.Id).Take(1).FirstOrDefault();
                if (telefono != null)
                {
                    lblId.Text = telefono.Id.ToString();
                    lblTelefono.Text = telefono.Numero;
                    lblIdUsuario.Text = telefono.IdUsuario.ToString();
                    txtNumeroEdit.Text = telefono.Numero;
                    hfNumeroInicial.Value = telefono.Numero;
                    if (telefono.Numero.Trim() == string.Empty)
                        btnChangeNumber.CommandArgument = "1";
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                if (!IsPostBack)
                {
                    if (Request.Params["confirmacionalta"] != null)
                    {
                        string[] values = Request.Params["confirmacionalta"].Split('_');
                        if (!_servicioUsuarios.ValidaConfirmacion(int.Parse(values[0]), values[1]))
                        {
                            Response.Redirect(ResolveUrl("~/Default.aspx"));
                        }
                        CargaTelefonosObligatorios(int.Parse(values[0]));
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
                AlertaGeneral = _lstError;
            }
        }

        protected void btnChangeNumber_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                switch (btn.CommandArgument)
                {
                    case "0":
                        txtNumeroEdit.ReadOnly = false;
                        btn.CssClass = "btn btn-sm btn-success";
                        btn.Text = "Aceptar";
                        btn.CommandArgument = "1";
                        btnSendNotification.Enabled = false;
                        break;
                    case "1":
                        _servicioUsuarios.ActualizarTelefono(int.Parse(lblIdUsuario.Text), int.Parse(lblId.Text), txtNumeroEdit.Text);
                        txtNumeroEdit.ReadOnly = true;
                        btn.CssClass = "btn btn-sm btn-primary";
                        btn.Text = "Cambiar Numero";
                        btn.CommandArgument = "0";
                        btnSendNotification.Enabled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnSendNotification_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtNumeroEdit.Text.Trim() == txtNumeroConfirmEdit.Text.Trim())
                {
                    txtNumeroEdit.Enabled = false;
                    txtNumeroConfirmEdit.Enabled = false;
                    if (hfNumeroInicial.Value != txtNumeroEdit.Text.Trim())
                    {
                        _servicioUsuarios.ActualizarTelefono(int.Parse(lblIdUsuario.Text), int.Parse(lblId.Text), txtNumeroEdit.Text);
                        hfNumeroInicial.Value = txtNumeroEdit.Text.Trim();
                    }
                    SendNotificacion(int.Parse(lblIdUsuario.Text), int.Parse(lblId.Text));
                    ((Button)sender).Enabled = false;
                    timebtn.Enabled = true;
                    divConfirmacion.Visible = true;
                    ((Button)sender).Visible = false;
                    btnReenviarcodigo.Enabled = false;
                }
                else
                    throw new Exception("Los numeros no coinciden");
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCaptura();
                new ServiceSecurityClient().ValidaPassword(txtContrasena.Text.Trim());
                string result = string.Empty;
                Dictionary<int, string> confirmaciones = new Dictionary<int, string>();
                result += _servicioUsuarios.ValidaCodigoVerificacionSms(int.Parse(lblIdUsuario.Text), (int)BusinessVariables.EnumTipoLink.Confirmacion, int.Parse(lblId.Text), txtCodigo.Text);
                confirmaciones.Add(int.Parse(lblId.Text), txtCodigo.Text.Trim());
                if (result.Trim() != string.Empty)
                    throw new Exception(result);
                _servicioUsuarios.ConfirmaCuenta(int.Parse(Request.Params["confirmacionalta"].Split('_')[0]), txtContrasena.Text.Trim(), confirmaciones, new List<PreguntaReto> { new PreguntaReto { Pregunta = txtPregunta.Text.Trim(), Respuesta = txtRespuesta.Text.Trim(), IdUsuario = int.Parse(Request.Params["confirmacionalta"].Split('_')[0]) } }, Request.Params["confirmacionalta"].Split('_')[1]);
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void timebtn_OnTick(object sender, EventArgs e)
        {
            try
            {
                if (btnReenviarcodigo.Text == "Reenviar código")
                    btnReenviarcodigo.Text = "60";
                else
                    btnReenviarcodigo.Text = (int.Parse(btnReenviarcodigo.Text) - 1).ToString();
                if (btnReenviarcodigo.Text == "0")
                {
                    btnReenviarcodigo.Text = "Reenviar Código";
                    timebtn.Enabled = false;
                    btnReenviarcodigo.Enabled = true;
                }
                upReenvio.Update();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
    }
}