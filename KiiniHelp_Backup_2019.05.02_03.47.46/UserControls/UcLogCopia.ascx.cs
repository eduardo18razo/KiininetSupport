﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSeguridad;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls
{
    public partial class UcLogCopia : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private bool _validCaptcha;
        readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
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

        private string Maquina
        {
            get
            {
                string result;
                if (Request.Cookies["miui"] != null)
                {
                    result = BusinessQueryString.Decrypt(Request.Cookies["miui"]["iuiu"]);
                }
                else
                {
                    result = _servicioSeguridad.GeneraLlaveMaquina();
                    HttpCookie myCookie = new HttpCookie("miui");
                    myCookie.Values.Add("iuiu", BusinessQueryString.Encrypt(result));
                    myCookie.Expires = DateTime.Now.AddYears(10);
                    Response.Cookies.Add(myCookie);
                }
                return result;
            }
        }
        public void AutenticarUsuarioPublico(int idTipoUsuario)
        {
            try
            {
                Usuario user = _servicioSeguridad.GetUserInvitadoDataAutenticate(idTipoUsuario);
                if (user == null)
                    Response.Redirect("~/Default.aspx");
                Session["UserData"] = user;
                if (user != null)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.NombreUsuario, DateTime.Now, DateTime.Now.AddMinutes(30), true, Session["UserData"].ToString(), FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
            }
        }

        public void DesbloquearUsuario()
        {
            try
            {
                txtpwd.Text = lblPswAbierto.Text;
                ValidaCaptura();
                _servicioSeguridad.CerrarSessionActiva(txtUsuario.Text.Trim(), txtpwd.Text.Trim());
                btnLogin_Click(btnLogin, null);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public void ResetCaptcha()
        {
            try
            {
                customvalidatorCaptcha.Enabled = true;
                LimpiarPantalla();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool Fail
        {
            get { return bool.Parse(fhFallo.Value); }
            set { fhFallo.Value = value.ToString(); }
        }
        private void ValidaCaptura()
        {
            List<string> sb = new List<string>();

            if (txtUsuario.Text.Trim() == string.Empty)
                sb.Add("Usuario es un campo obligatorio.");
            if (txtpwd.Text.Trim() == string.Empty)
                sb.Add("Contraseña es un campo obligatorio.");
            if (txtCaptcha.Text.Trim() == string.Empty && customvalidatorCaptcha.Enabled)
                sb.Add("Ingrese codigo Captcha.");

            if (sb.Count > 0)
            {
                _lstError = sb;
                throw new Exception("");
            }
        }
        private void LimpiarPantalla()
        {
            try
            {
                txtUsuario.Text = string.Empty;
                txtpwd.Text = string.Empty;
                txtCaptcha.Text = string.Empty;
                lblPswAbierto.Text = string.Empty;
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
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                ValidaCaptura();
                if (!_validCaptcha && customvalidatorCaptcha.Enabled)
                {
                    txtCaptcha.Text = string.Empty;
                    throw new Exception("Captcha incorrecto");
                }

                double tiempoSesion = double.Parse(ConfigurationManager.AppSettings["TiempoSession"]) + 5;
                if (_servicioSeguridad.UsuariorActivo(txtUsuario.Text.Trim(), txtpwd.Text.Trim(), SecurityUtils.CreateShaHash(Maquina)))
                {
                    customvalidatorCaptcha.Enabled = false;
                    lblPswAbierto.Text = txtpwd.Text.Trim();
                    DesbloquearUsuario();
                    //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptOpen", "MostrarPopup(\"#modalSessionAbierta\");", true);
                    //return;
                }
                if (!_servicioSeguridad.Autenticate(txtUsuario.Text.Trim(), txtpwd.Text.Trim(), SecurityUtils.CreateShaHash(Maquina)))
                    throw new Exception("Usuario y/o contraseña no validos");

                Usuario user = _servicioSeguridad.GetUserDataAutenticate(txtUsuario.Text.Trim(), txtpwd.Text.Trim());

                Session["UserData"] = user;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.NombreUsuario, DateTime.Now, DateTime.Now.AddDays(tiempoSesion + 5), true, Session["UserData"].ToString(), FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Name = FormsAuthentication.FormsCookieName,
                    Expires = DateTime.Now.AddDays(tiempoSesion),
                    Value = encTicket
                });
                //List<int> roles = user.UsuarioRol.Select(s => s.RolTipoUsuario.IdRol).ToList();
                //if (roles.Any(a => a == (int)BusinessVariables.EnumRoles.Administrador)) ;
                LimpiarPantalla();
                if (OnCancelarModal != null)
                    OnCancelarModal();

                Response.Redirect("~/Users/DashBoard.aspx");
            }
            catch (Exception ex)
            {
                Fail = true;
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
                if (txtUsuario.Text.Trim() == string.Empty || txtpwd.Text.Trim() == string.Empty || txtCaptcha.Text.Trim() == string.Empty) return;
                Captcha1.ValidateCaptcha(txtCaptcha.Text.Trim());
                e.IsValid = Captcha1.UserValidated;
                _validCaptcha = e.IsValid;
                if (!e.IsValid)
                {
                    List<string> sb = new List<string> {"Captcha incorrecto."};
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
                    _lstError = new List<string> {ex.Message};
                }

                Alerta = _lstError;
            }
        }

    }
}