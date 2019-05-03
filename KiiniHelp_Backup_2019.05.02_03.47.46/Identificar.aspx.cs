using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;
using System.Web.UI.WebControls;
using KinniNet.Business.Utils;


namespace KiiniHelp
{
    public partial class Identificar : Page
    {
        private bool ValidCaptcha = false;
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
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
                if (Request.Params["confirmacionalta"] != null)
                {
                    string[] values = Request.Params["confirmacionalta"].Split('_');
                    if (!_servicioUsuarios.ValidaConfirmacion(int.Parse(values[0]), values[1]))
                    {
                        Alerta = new List<string> { "Link Invalido !!!" };
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

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    List<Usuario> usuarios = _servicioUsuarios.BuscarUsuarios(txtUserName.Text.Trim());
                    if (usuarios.Any())
                    {
                        if (!_servicioUsuarios.ObtenerDetalleUsuario(usuarios.First().Id).Activo)
                        {
                            _servicioUsuarios.ReenviarActivacion(usuarios.First().Id);
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptOpen", "MostrarPopup(\"#modalRegistroExito\");", true);
                            return;
                        }
                        Response.Redirect("~/FrmRecuperar.aspx?ldata=" + QueryString.Encrypt(usuarios.First().Id.ToString()));
                    }
                    else
                    {
                        throw new Exception("Usuario no valido !!!");
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
                Captcha1.ValidateCaptcha(txtCaptcha.Text.Trim());
                e.IsValid = Captcha1.UserValidated;
                ValidCaptcha = e.IsValid;
                if (!e.IsValid)
                {
                    if (txtUserName.Text.Trim() == string.Empty || txtCaptcha.Text.Trim() == string.Empty) return;
                    List<string> sb = new List<string>();
                    sb.Add("Captcha incorrecto.");
                    if (sb.Count > 0)
                    {
                        _lstError = sb;
                        throw new Exception("");
                    }
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
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

    }
}