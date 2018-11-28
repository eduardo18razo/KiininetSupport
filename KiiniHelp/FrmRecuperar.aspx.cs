using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;

namespace KiiniHelp
{
    public partial class FrmRecuperar : Page
    {
        private readonly ServiceUsuariosClient _servicioUsuario = new ServiceUsuariosClient();
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
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
        private void ValidaCampos()
        {
            try
            {
                if (txtContrasena.Text.Trim() == string.Empty)
                    throw new Exception("Contraseña nueva es campo obligatorio");
                if (txtContrasena.Text.Trim() != txtConfirmar.Text.Trim())
                    throw new Exception("Las contraseñas no coinciden");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ChangeOption()
        {
            try
            {
                Usuario userData = _servicioUsuario.ObtenerDetalleUsuario(int.Parse(QueryString.Decrypt(Request.Params["ldata"])));
                if (rbtnCorreo.Checked)
                {
                    if (userData.CorreoUsuario.Count <= 0)
                        throw new Exception("No cuenta con correos registrados contacte a su Administrador.");
                    hfIdSend.Value = userData.CorreoUsuario.ToList().First().Id.ToString();
                    hfValueSend.Value = userData.CorreoUsuario.ToList().First().Correo;
                    hfValueNotivicacion.Value = _servicioUsuario.EnviaCodigoVerificacionCorreo(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, int.Parse(hfIdSend.Value));

                    divCodigoVerificacion.Visible = true;
                }
                else if (rbtnSms.Checked)
                {
                    if (userData.TelefonoUsuario.Count(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular && w.Principal) <= 0)
                        throw new Exception("No cuenta con telefonos registrados contacte a su Administrador.");
                    hfIdSend.Value = userData.TelefonoUsuario.Where(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular && w.Principal).ToList().First().Id.ToString();
                    hfValueSend.Value = userData.TelefonoUsuario.Where(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular && w.Principal).ToList().First().Numero;
                    _servicioUsuario.EnviaCodigoVerificacionSms(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, int.Parse(hfIdSend.Value));
                    divCodigoVerificacion.Visible = true;
                }
                else if (rbtnPreguntas.Checked)
                {
                    if (userData.PreguntaReto.Count <= 0)
                        throw new Exception("No cuenta con preguntas registradas contacte a su Administrador.");
                    rptPreguntas.DataSource = userData.PreguntaReto;
                    rptPreguntas.DataBind();
                    divPreguntas.Visible = true;
                }
                divQuestion.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GeneraCoockie()
        {
            try
            {
                if (Request.Cookies["miui"] != null)
                {
                    var value = BusinessQueryString.Decrypt(Request.Cookies["miui"]["iuiu"]);
                }
                else
                {
                    string llave = _servicioSeguridad.GeneraLlaveMaquina();
                    HttpCookie myCookie = new HttpCookie("miui");
                    myCookie.Values.Add("iuiu", BusinessQueryString.Encrypt(llave));
                    myCookie.Expires = DateTime.Now.AddYears(10);
                    Response.Cookies.Add(myCookie);
                }
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
                if (!IsPostBack)
                    GeneraCoockie();
                HttpCookie myCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (myCookie != null && Session["UserData"] != null)
                {
                    Response.Redirect("~/Users/DashBoard.aspx");
                }
                Alerta = new List<string>();
                if (!IsPostBack)
                    if (Request.Params["ldata"] != null)
                    {
                        hfEsLink.Value = false.ToString();
                    }
                    else if (Request.Params["confirmacionCodigo"] != null && Request.Params["correo"] != null && Request.Params["code"] != null)
                    {
                        rbtnCorreo.Checked = true;
                        txtCodigo.Text = BusinessQueryString.Decrypt(Request.Params["code"]);
                        hfEsLink.Value = true.ToString();
                        btncontinuar_OnClick(null, null);
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

        protected void btncontinuar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (bool.Parse(hfEsLink.Value))
                {
                    string[] values = QueryString.Decrypt(Request.Params["confirmacionCodigo"]).Split('_');
                    if (btncontinuar.CommandArgument == string.Empty)
                    {
                        if (rbtnCorreo.Checked)
                        {
                            btncontinuar.CommandArgument = "0";
                            _servicioUsuario.ValidaCodigoVerificacionCorreo(int.Parse(values[0]), (int)BusinessVariables.EnumTipoLink.Reset, values[1], int.Parse(QueryString.Decrypt(Request.Params["correo"])), txtCodigo.Text.Trim());
                            hfParametrosConfirmados.Value = true.ToString();
                        }
                        divQuestion.Visible = false;
                        divCodigoVerificacion.Visible = false;
                        divPreguntas.Visible = false;
                        divChangePwd.Visible = true;
                    }
                    else
                    {
                        ValidaCampos();
                        _servicioSeguridad.RecuperarCuenta(int.Parse(values[0]), (int)BusinessVariables.EnumTipoLink.Reset, values[1], int.Parse(QueryString.Decrypt(Request.Params["correo"])), txtCodigo.Text.Trim(), txtContrasena.Text, "0");
                        Response.Redirect("~/Default.aspx");
                    }
                }
                else
                {
                    hfEsLink.Value = false.ToString();
                    if (!bool.Parse(hfParametrosConfirmados.Value))
                    {
                        string tiporecuperacion = rbtnCorreo.Checked ? "0" : rbtnSms.Checked ? "1" : rbtnPreguntas.Checked ? "2" : "fail";
                        //string tiporecuperacion = rbtnCorreo.Checked ? "0" : rbtnPreguntas.Checked ? "2" : "fail";
                        if (rbtnCorreo.Checked)
                        {
                            _servicioUsuario.ValidaCodigoVerificacionCorreo(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, hfValueNotivicacion.Value, int.Parse(hfIdSend.Value), txtCodigo.Text.Trim());
                            btncontinuar.CommandArgument = "0";
                            hfParametrosConfirmados.Value = true.ToString();
                            divQuestion.Visible = false;
                            divCodigoVerificacion.Visible = false;
                            divPreguntas.Visible = false;
                            divChangePwd.Visible = true;
                        }
                        else if (rbtnSms.Checked)
                        {
                            _servicioUsuario.ValidaCodigoVerificacionSms(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, int.Parse(hfIdSend.Value), txtCodigo.Text.Trim());
                            btncontinuar.CommandArgument = "1";
                            hfValueNotivicacion.Value = string.Empty;
                            hfParametrosConfirmados.Value = true.ToString();
                            divQuestion.Visible = false;
                            divCodigoVerificacion.Visible = false;
                            divPreguntas.Visible = false;
                            divChangePwd.Visible = true;
                        }
                        else if (rbtnPreguntas.Checked)
                        {
                            Dictionary<int, string> preguntaRespuesta = new Dictionary<int, string>();
                            foreach (RepeaterItem item in rptPreguntas.Items)
                            {
                                preguntaRespuesta.Add(int.Parse(((Label)item.FindControl("lblId")).Text), ((TextBox)item.FindControl("txtRespuesta")).Text);
                            }
                            _servicioUsuario.ValidaRespuestasReto(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), preguntaRespuesta);
                            btncontinuar.CommandArgument = "2";
                            hfValueNotivicacion.Value = string.Empty;
                            hfParametrosConfirmados.Value = true.ToString();
                            divQuestion.Visible = false;
                            divCodigoVerificacion.Visible = false;
                            divPreguntas.Visible = false;
                            divChangePwd.Visible = true;
                        }
                    }
                    else
                    {
                        if (divChangePwd.Visible)
                        {
                            ValidaCampos();
                            _servicioSeguridad.ValidaPassword(txtContrasena.Text.Trim());
                            switch (btncontinuar.CommandArgument)
                            {
                                case "0":
                                    _servicioSeguridad.RecuperarCuenta(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, hfValueNotivicacion.Value, int.Parse(hfIdSend.Value), txtCodigo.Text, txtContrasena.Text.Trim(), "0");
                                    break;
                                case "1":
                                    _servicioSeguridad.RecuperarCuenta(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, hfValueNotivicacion.Value, int.Parse(hfIdSend.Value), txtCodigo.Text, txtContrasena.Text.Trim(), "1");
                                    break;
                                case "2":
                                    _servicioSeguridad.RecuperarCuenta(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, hfValueNotivicacion.Value, -1, txtCodigo.Text, txtContrasena.Text, "2");
                                    break;
                            }
                            Response.Redirect("~/Default.aspx");
                        }
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

        protected void rbtnCorreo_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeOption();
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

        protected void rbtnSms_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeOption();
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

        protected void rbtnPreguntas_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeOption();
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

        protected void rbtnList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnCorreo.Checked)
                {
                    hfValueNotivicacion.Value = _servicioUsuario.EnviaCodigoVerificacionCorreo(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, int.Parse(hfIdSend.Value));
                }
                else if (rbtnSms.Checked)
                {
                    _servicioUsuario.EnviaCodigoVerificacionSms(int.Parse(QueryString.Decrypt(Request.Params["ldata"])), (int)BusinessVariables.EnumTipoLink.Reset, int.Parse(hfIdSend.Value));
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
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

    }
}