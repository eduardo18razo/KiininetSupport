using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSeguridad;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls
{
    public partial class UcLogIn : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private bool ValidCaptcha = false;
        readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
        {
            set
            {
                pnlAlerta.Visible = value.Any();
                if (!pnlAlerta.Visible) return;
                rptErrorGeneral.DataSource = value.Select(s => new { Detalle = s }).ToList();
                rptErrorGeneral.DataBind();
            }
        }

        public void AutenticarUsuarioPublico(int idTipoUsuario)
        {
            try
            {
                Usuario user = _servicioSeguridad.GetUserInvitadoDataAutenticate(idTipoUsuario);
                Session["UserData"] = user;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.NombreUsuario, DateTime.Now, DateTime.Now.AddMinutes(30), true, Session["UserData"].ToString(), FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
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

        private void ValidaCaptura()
        {
            StringBuilder sb = new StringBuilder();

            if (txtUsuario.Text.Trim() == string.Empty)
                sb.AppendLine("<li>Usuario es un campo obligatorio.</li>");
            if (txtpwd.Text.Trim() == string.Empty)
                sb.AppendLine("<li>Contraseña es un campo obligatorio.</li>");
            if (txtCaptcha.Text.Trim() == string.Empty)
                sb.AppendLine("<li>Ingrese codigo Captcha.</li>");
            if (sb.ToString() != string.Empty)
            {
                sb.Append("</ul>");
                sb.Insert(0, "<ul>");
                sb.Insert(0, "<h3>Acceso</h3>");
                throw new Exception(sb.ToString());
            }
        }

        private void LimpiarPantalla()
        {
            try
            {
                AlertaGeneral = new List<string>();
                txtUsuario.Text = string.Empty;
                txtpwd.Text = string.Empty;
                txtCaptcha.Text = string.Empty;
                txtUsuario.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
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
                AlertaGeneral = _lstError;
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                ValidaCaptura();
                if (!ValidCaptcha)
                {
                    txtCaptcha.Text = string.Empty;
                    StringBuilder sb = new StringBuilder();
                    throw new Exception(sb.ToString());
                }
                if (!_servicioSeguridad.Autenticate(txtUsuario.Text.Trim(), txtpwd.Text.Trim())) throw new Exception("Usuario y/o contraseña no validos");
                Usuario user = _servicioSeguridad.GetUserDataAutenticate(txtUsuario.Text.Trim(), txtpwd.Text.Trim());
                Session["UserData"] = user;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.NombreUsuario, DateTime.Now, DateTime.Now.AddMinutes(15), true, Session["UserData"].ToString(), FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                List<int> roles = user.UsuarioRol.Select(s => s.RolTipoUsuario.IdRol).ToList();
                if (roles.Any(a => a == (int)BusinessVariables.EnumRoles.Administrador)) ;
                LimpiarPantalla();
                if (OnCancelarModal != null)
                    OnCancelarModal();
                Response.Redirect("~/Users/DashBoard.aspx");
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

        protected void lnkBtnRecuperar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Identificar.aspx");
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

        protected void btnCacelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarPantalla();
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
                AlertaGeneral = _lstError;
            }
        }

        protected void OnServerValidate(object source, ServerValidateEventArgs e)
        {
            try
            {
                if (txtUsuario.Text.Trim() == string.Empty || txtpwd.Text.Trim() == string.Empty || txtCaptcha.Text.Trim() == string.Empty) return;
                Captcha1.ValidateCaptcha(txtCaptcha.Text.Trim());
                e.IsValid = Captcha1.UserValidated;
                ValidCaptcha = e.IsValid;
                if (!e.IsValid)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<li>Captcha incorrecto.</li>");
                    if (sb.ToString() != string.Empty)
                    {
                        sb.Append("</ul>");
                        sb.Insert(0, "<ul>");
                        sb.Insert(0, "<h3>Acceso</h3>");
                        throw new Exception(sb.ToString());
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
    }
}