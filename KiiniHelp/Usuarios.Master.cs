using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using Menu = KiiniNet.Entities.Cat.Sistema.Menu;

namespace KiiniHelp
{
    public partial class UsuariosMaster : MasterPage
    {
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
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

        private int? RolSeleccionado
        {
            get { return Session["RolSeleccionado"] == null ? null : string.IsNullOrEmpty(Session["RolSeleccionado"].ToString()) ? null : (int?)int.Parse(Session["RolSeleccionado"].ToString()); }
            set { Session["RolSeleccionado"] = value.ToString(); }
        }
        public void AlertaSucces(string value = "")
        {
            value = value.Trim() == string.Empty ? BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Exito) : value;
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptSuccessAlert", "SuccsessAlert('','" + value + "');", true);
        }
        public void AlertaError(string value = "")
        {
            value = value.Trim() == string.Empty ? BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Exito) : value;
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('','" + value + "');", true);
        }
        private void ObtenerRoles()
        {
            try
            {
                List<Rol> lstRoles = _servicioSeguridad.ObtenerRolesUsuario(((Usuario)Session["UserData"]).Id).Where(w => w.Id != (int)BusinessVariables.EnumRoles.Notificaciones).ToList();
                if (lstRoles.Count > 0 && RolSeleccionado == null)
                {
                    RolSeleccionado = lstRoles.Any(rol => rol.Id == (int)BusinessVariables.EnumRoles.Agente) ? (int)BusinessVariables.EnumRoles.Agente : lstRoles.First().Id;
                    Session["CargaInicialModal"] = "True";
                    if (MenuActivo == null)
                        lnkBtnRol_OnClick(new LinkButton
                        {
                            CommandArgument = lstRoles.Any(rol => rol.Id == (int)BusinessVariables.EnumRoles.Agente) ? ((int)BusinessVariables.EnumRoles.Agente).ToString() : lstRoles.First().Id.ToString(),
                            CommandName = lstRoles.Any(rol => rol.Id == (int)BusinessVariables.EnumRoles.Agente) ? BusinessVariables.EnumRoles.Agente.ToString() : lstRoles.First().Descripcion,
                            Text = lstRoles.Any(rol => rol.Id == (int)BusinessVariables.EnumRoles.Agente) ? BusinessVariables.EnumRoles.Agente.ToString() : lstRoles.First().Descripcion
                        }, null);
                }
                if (RolSeleccionado != null)
                    lblAreaSeleccionada.Text = lstRoles.Single(s => s.Id == int.Parse(RolSeleccionado.ToString())).Descripcion;
                if (lstRoles.Count <= 1)
                    divRoles.Visible = false;
                if (lstRoles.Any(a => a.Id == (int) BusinessVariables.EnumRoles.Administrador))
                    lstRoles.RemoveAll(r => r.Id == (int) BusinessVariables.EnumRoles.AccesoAnalíticos);
                rptRolesPanel.DataSource = lstRoles;
                rptRolesPanel.DataBind();
                lblBadgeRoles.Text = lstRoles.Count.ToString();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
                AlertaSucces(_lstError.ToString());
            }
        }

        public void ActualizarFoto()
        {
            try
            {
                if (Session["UserData"] != null)
                {
                    Usuario userData = (Usuario)Session["UserData"];
                    ((Usuario)Session["UserData"]).Foto = _servicioUsuarios.ObtenerFoto(userData.Id);
                    userData = (Usuario)Session["UserData"];
                    imgPerfil.ImageUrl = userData.Foto != null ? "~/DisplayImages.ashx?id=" + userData : "~/assets/images/profiles/profile-1.png";
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
                AlertaSucces(_lstError.ToString());
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

        private List<Menu> MenuActivo
        {
            get { return (List<Menu>)Session["MenuRol"]; }
            set { Session["MenuRol"] = value; }
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
        private void LlenaMenu(int idUsuario, int idRolSeleccionado, bool arboles)
        {
            try
            {
                //MenuActivo = _servicioSeguridad.ObtenerMenuUsuario(idUsuario, idRolSeleccionado, arboles).Where(w=>w.Id != (int)BusinessVariables.EnumMenu.Analiticos).ToList();
                MenuActivo = _servicioSeguridad.ObtenerMenuUsuario(idUsuario, idRolSeleccionado, arboles).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void Buscador()
        {
            try
            {
                if (string.IsNullOrEmpty(main_search_input.Text.Trim()))
                    throw new Exception("Debe espicificar un parametro de busqueda");
                Response.Redirect("~/Users/FrmBusqueda.aspx?w=" + main_search_input.Text.Trim() + "&tu=" + ((Usuario)Session["UserData"]).IdTipoUsuario);
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

                HttpCookie myCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (myCookie == null || Session["UserData"] == null || (((Usuario)Session["UserData"]).IdTipoUsuario != (int)BusinessVariables.EnumTiposUsuario.Agentes && ((Usuario)Session["UserData"]).IdTipoUsuario != (int)BusinessVariables.EnumTiposUsuario.Empleado && ((Usuario)Session["UserData"]).IdTipoUsuario != (int)BusinessVariables.EnumTiposUsuario.Cliente && ((Usuario)Session["UserData"]).IdTipoUsuario != (int)BusinessVariables.EnumTiposUsuario.Proveedor) || bool.Parse(hfSesionExpiro.Value))
                {
                    Session.RemoveAll();
                    Session.Clear();
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                    Response.Redirect("~/Default.aspx");
                }
                if (Session["UserData"] != null)
                {
                    if (!_servicioSeguridad.ValidaSesion(((Usuario)Session["UserData"]).Id, SecurityUtils.CreateShaHash(Maquina)))
                        btnsOut_OnClick(null, null);
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
                lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
                ucTicketPortal.OnAceptarModal += UcTicketPortal_OnAceptarModal;
                
                if (!IsPostBack && Session["UserData"] != null)
                {
                    Session["Reset"] = true;
                    double tiempoSesion = double.Parse(ConfigurationManager.AppSettings["TiempoSession"]);
                    double timeout = (double)tiempoSesion * 1000 * 60;
                    Page.ClientScript.RegisterStartupScript(GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);

                    bool administrador = false, agente = false;
                    Usuario usuario = ((Usuario)Session["UserData"]);
                    if (usuario.UsuarioRol.Any(rol => rol.RolTipoUsuario.IdRol == (int)BusinessVariables.EnumRoles.Administrador))
                    {
                        administrador = true;
                    }
                    if (usuario.UsuarioRol.Any(rol => rol.RolTipoUsuario.IdRol == (int)BusinessVariables.EnumRoles.Agente))
                    {
                        agente = true;
                    }
                    if (administrador || agente)
                        Session["CargaInicialModal"] = true.ToString();
                    hfCargaInicial.Value = (Session["CargaInicialModal"] ?? "False").ToString();
                    lblUsuario.Text = usuario.Nombre;
                    lblTipoUsr.Text = usuario.TipoUsuario.Descripcion;
                    int idUsuario = usuario.Id;
                    imgPerfil.ImageUrl = usuario.Foto != null ? "~/DisplayImages.ashx?id=" + idUsuario : "~/assets/images/profiles/profile-1.png";
                    ObtenerRoles();
                    int rolSeleccionado = agente ? (int)BusinessVariables.EnumRoles.Agente : 0;
                    if (RolSeleccionado != null)
                        rolSeleccionado = int.Parse(RolSeleccionado.ToString());
                    if (MenuActivo == null)
                        LlenaMenu(usuario.Id, rolSeleccionado, rolSeleccionado != 0);

                    divTickets.Visible = rolSeleccionado != (int)BusinessVariables.EnumRoles.Administrador;
                    //divMensajes.Visible = rolSeleccionado != (int)BusinessVariables.EnumRoles.Administrador;
                    divTickets.Visible = rolSeleccionado == (int)BusinessVariables.EnumRoles.Agente;
                    //divMensajes.Visible = rolSeleccionado == (int)BusinessVariables.EnumRoles.Usuario;
                    divSearch.Visible = rolSeleccionado == (int)BusinessVariables.EnumRoles.AccesoCentroSoporte;
                }
                

                rptMenu.DataSource = MenuActivo;
                rptMenu.DataBind();
                Session["ParametrosGenerales"] = _servicioParametros.ObtenerParametrosGenerales();

                if (Session["UserData"] != null && HttpContext.Current.Request.Url.Segments[HttpContext.Current.Request.Url.Segments.Count() - 1] != "FrmCambiarContrasena.aspx")
                    if (_servicioSeguridad.CaducaPassword(((Usuario)Session["UserData"]).Id))
                        Response.Redirect(ResolveUrl("~/Users/Administracion/Usuarios/FrmCambiarContrasena.aspx?confirmaCuenta=true"));

                if (IsPostBack)
                {
                    if (Page.Request.Params["__EVENTTARGET"] == "Buscador")
                    {
                        Buscador();
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
        protected void rptMenu_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu1"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu1_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu2"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu2_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu3"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu3_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu4"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu4_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu5"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu5_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu6"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu6_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu7"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void btnsOut_OnClick(object sender, EventArgs e)
        {
            try
            {
                Usuario user = (Usuario) Session["UserData"];
                if(user != null)
                    _servicioSeguridad.TerminarSesion(user.Id, SecurityUtils.CreateShaHash(Maquina));
                Session.RemoveAll();
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
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

        protected void lnkBtnCerrar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(ResolveUrl("~/Users/DashBoard.aspx"));
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

        protected void lnkBtnRol_OnClick(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    Usuario usuario = ((Usuario)Session["UserData"]);
                    RolSeleccionado = int.Parse(((LinkButton)sender).CommandArgument);
                    lblAreaSeleccionada.Text = ((LinkButton)sender).CommandName;

                    LlenaMenu(usuario.Id, (int)RolSeleccionado, RolSeleccionado != 0);

                    rptMenu.DataSource = MenuActivo;
                    rptMenu.DataBind();
                    Session["CargaInicialModal"] = "True";
                    switch (RolSeleccionado)
                    {
                        case (int)BusinessVariables.EnumRoles.Agente:
                            divSearch.Visible = false;
                            Response.Redirect("~/Agente/DashBoardAgente.aspx");
                            break;
                        case (int)BusinessVariables.EnumRoles.Administrador:
                            divSearch.Visible = false;
                            Response.Redirect("~/Users/DashBoard.aspx");
                            break;
                        case (int)BusinessVariables.EnumRoles.ResponsableDeCategoría:
                            divSearch.Visible = true;
                            Response.Redirect("~/Users/FrmDashboardUser.aspx?");
                            break;
                        case (int)BusinessVariables.EnumRoles.AccesoAnalíticos:
                            divSearch.Visible = true;
                            Response.Redirect("~/Users/FrmReportes.aspx");
                            break;
                        default:
                            divSearch.Visible = true;
                            Response.Redirect("~/Users/FrmDashboardUser.aspx?");
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

        protected void btnSwitchRol_OnClick(object sender, EventArgs e)
        {
            try
            {
                
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

        protected void btnMiPerfil_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Users/Administracion/Usuarios/FrmEdicionUsuario.aspx?Detail=1");
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnCambiarContraseña_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Users/Administracion/Usuarios/FrmCambiarContrasena.aspx");
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void lnkHome_OnClick(object sender, EventArgs e)
        {
            try
            {
                switch (RolSeleccionado)
                {
                    case (int)BusinessVariables.EnumRoles.Agente:
                        Response.Redirect("~/Agente/DashBoardAgente.aspx");
                        break;
                    case (int)BusinessVariables.EnumRoles.Administrador:
                        Response.Redirect("~/Users/DashBoard.aspx");
                        break;
                    default:
                        Response.Redirect("~/Users/FrmDashboardUser.aspx");
                        break;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Users/General/FrmMisTickets.aspx");
        }

        protected void btnMisTickets_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Users/General/FrmMisTickets.aspx");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}