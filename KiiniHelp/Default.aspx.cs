using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;

namespace KiiniHelp
{
    public partial class Default1 : Page
    {
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
        private readonly ServiceTipoUsuarioClient _servicioTipoUsuario = new ServiceTipoUsuarioClient();
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

        private void UcTicketPortal_OnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptClose", "CierraPopup(\"#modal-new-ticket\");", true);

                lblNoTicket.Text = ucTicketPortal.TicketGenerado.ToString();
                lblRandom.Text = ucTicketPortal.RandomGenerado;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptOpen", "MostrarPopup(\"#modalExitoTicket\");", true);
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

        private void Buscador()
        {
            try
            {
                if (string.IsNullOrEmpty(main_search_input.Text.Trim()))
                    throw new Exception("Debe espicificar un parametro de busqueda");
                Response.Redirect("~/Publico/FrmBusqueda.aspx?w=" + main_search_input.Text.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void HabilitaTiposDeUsuario()
        {
            try
            {
                List<TipoUsuario> tiposUsuario = _servicioTipoUsuario.ObtenerTiposUsuario(false);
                divEmpleados.Visible = tiposUsuario.Any(s => s.Id == (int)BusinessVariables.EnumTiposUsuario.Empleado);
                divClientes.Visible = tiposUsuario.Any(s => s.Id == (int)BusinessVariables.EnumTiposUsuario.Cliente);
                divProveedores.Visible = tiposUsuario.Any(s => s.Id == (int)BusinessVariables.EnumTiposUsuario.Proveedor);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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

                lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
                ucTicketPortal.OnAceptarModal += UcTicketPortal_OnAceptarModal;
                if (UcLogCopia.Fail)
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "OpenDropLogin();", true);
                if (IsPostBack)
                {
                    if (Page.Request.Params["__EVENTTARGET"] == "Buscador")
                    {
                        Buscador();
                    }
                }
                if (!IsPostBack)
                {
                    HabilitaTiposDeUsuario();
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

        protected void lnkBtnEmpleado_OnClick(object sender, EventArgs e)
        {
            try
            {
                Session["TipoUsuarioPublico"] = (int)BusinessVariables.EnumTiposUsuario.Empleado;
                Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + (int)BusinessVariables.EnumTiposUsuario.Empleado + "&idArea=-1");
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

        protected void lnkBtnCliente_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + (int)BusinessVariables.EnumTiposUsuario.Cliente + "&idArea=-1");
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

        protected void lnkBtnProveedor_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + (int)BusinessVariables.EnumTiposUsuario.Proveedor + "&idArea=-1");
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

        protected void btnCerrarTicket_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucTicketPortal.Limpiar();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptClose", "CierraPopup(\"#modal-new-ticket\");", true);
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

        protected void btnCerrarExito_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucTicketPortal.Limpiar();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptOpen", "MostrarPopup(\"#modalExitoTicket\");", true);
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

        protected void btnconsultarTicket_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Publico/Consultas/FrmConsultaTicket.aspx?userType=" + (int)BusinessVariables.EnumTiposUsuario.Cliente);
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

        protected void btnCerramodalSessionAbierta_OnClick(object sender, EventArgs e)
        {
            try
            {
                UcLogCopia.ResetCaptcha();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptClose", "CierraPopup(\"#modalSessionAbierta\");", true);
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

        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                UcLogCopia.DesbloquearUsuario();
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