﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSeguridad;
using KinniNet.Business.Utils;
using Menu = KiiniNet.Entities.Cat.Sistema.Menu;

namespace KiiniHelp
{
    public partial class Public : MasterPage
    {
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();


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

        public int TipoUsuario
        {
            get { return int.Parse(ViewState["TipoUsuario"].ToString()); }
            set { ViewState["TipoUsuario"] = value.ToString(); }
        }

        private void ObtenerMenuPublico(int areaSeleccionada, bool arboles)
        {
            try
            {
                List<Menu> lstMenu = _servicioSeguridad.ObtenerMenuPublico(TipoUsuario, areaSeleccionada, arboles);
                rptMenu.DataSource = lstMenu;
                rptMenu.DataBind();
                divMenuBtn.Visible = lstMenu.Count > 0;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpCookie myCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (myCookie != null && Session["UserData"] != null)
                {
                    Response.Redirect("~/Users/DashBoard.aspx");
                }

                divMenuBtn.Visible = false;
                lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
                UcLogIn.OnAceptarModal += UcLogInOnOnCancelarModal;
                UcLogIn.OnCancelarModal += UcLogInOnOnCancelarModal;
                ucTicketPortal.OnAceptarModal += UcTicketPortal_OnAceptarModal;
                if (!IsPostBack)
                    if (Request.Params["userType"] != null)
                    {
                        int areaSeleccionada;
                        TipoUsuario = int.Parse(Request.Params["userType"]);
                        switch (TipoUsuario)
                        {
                            case (int)BusinessVariables.EnumTiposUsuario.Empleado:
                                areaSeleccionada = 6;
                                ViewState["AreaSeleccionada"] = areaSeleccionada;
                                break;
                            case (int)BusinessVariables.EnumTiposUsuario.Cliente:
                                areaSeleccionada = 6;
                                ViewState["AreaSeleccionada"] = areaSeleccionada;
                                break;
                            case (int)BusinessVariables.EnumTiposUsuario.Proveedor:
                                areaSeleccionada = 6;
                                ViewState["AreaSeleccionada"] = areaSeleccionada;
                                break;
                        }
                    }
                if (Request.Params["userType"] != null)
                    ObtenerMenuPublico(6, 6 != 0);
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

        private void UcLogInOnOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalSingIn\");", true);
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

        protected void rptSubMenu7_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu8"));
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

        protected void lbtnCteArea_OnClick(object sender, EventArgs e)
        {
            try
            {
                int areaSeleccionada = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                ViewState["AreaSeleccionada"] = areaSeleccionada;
                rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico(TipoUsuario, areaSeleccionada, areaSeleccionada != 0);
                rptMenu.DataBind();
                UcLogIn.AutenticarUsuarioPublico((int)BusinessVariables.EnumTiposUsuario.Cliente);
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

        public void CargaPerfil(int idUsuario)
        {
            try
            {
                int areaSeleccionada = (int)BusinessVariables.EnumRoles.AccesoCentroSoporte;
                ViewState["AreaSeleccionada"] = areaSeleccionada;
                rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico(idUsuario, areaSeleccionada, areaSeleccionada != 0);
                rptMenu.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void lbtnEmpleadoArea_OnClick(object sender, EventArgs e)
        {
            try
            {
                int areaSeleccionada = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                ViewState["AreaSeleccionada"] = areaSeleccionada;
                rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico((int)BusinessVariables.EnumTiposUsuario.Empleado, areaSeleccionada, areaSeleccionada != 0);
                rptMenu.DataBind();
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

        protected void lbtnProveedorArea_OnClick(object sender, EventArgs e)
        {
            try
            {
                int areaSeleccionada = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                ViewState["AreaSeleccionada"] = areaSeleccionada;
                rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico((int)BusinessVariables.EnumTiposUsuario.Proveedor, areaSeleccionada, areaSeleccionada != 0);
                rptMenu.DataBind();
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

        protected void btnLogIn_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalSingIn\");", true);
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

        protected void lnkConsultaticket_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(ResolveUrl("~/Publico/Consultas/FrmConsultaTicket.aspx"));
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

        protected void btnConsultarTicket_OnClick(object sender, EventArgs e)
        {
            try
            {
                CargaPerfil((int)BusinessVariables.EnumTiposUsuario.Cliente);
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
                UcLogIn.ResetCaptcha();
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
                UcLogIn.DesbloquearUsuario();
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