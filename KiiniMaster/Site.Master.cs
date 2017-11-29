using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace KiiniMaster
{    
    public partial class SiteMaster : MasterPage
    {
        //private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();


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

        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
            //    UcLogIn.OnAceptarModal += UcLogInOnOnCancelarModal;
            //    UcLogIn.OnCancelarModal += UcLogInOnOnCancelarModal;
            //    ucTicketPortal.OnAceptarModal += UcTicketPortal_OnAceptarModal;
            //    if (Request.Params["userTipe"] != null)
            //    {
            //        int areaSeleccionada;
            //        int tipoUsuario = int.Parse(Request.Params["userTipe"]);
            //        switch (tipoUsuario)
            //        {
            //            case (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado:
            //                areaSeleccionada = 6;
            //                Session["AreaSeleccionada"] = areaSeleccionada;
            //                rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico((int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado, areaSeleccionada, areaSeleccionada != 0);
            //                rptMenu.DataBind();
            //                UcLogIn.AutenticarUsuarioPublico((int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado);
            //                break;
            //            case (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado:
            //                areaSeleccionada = 6;
            //                Session["AreaSeleccionada"] = areaSeleccionada;
            //                rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico((int)BusinessVariables.EnumTiposUsuario.ClienteInvitado, areaSeleccionada, areaSeleccionada != 0);
            //                rptMenu.DataBind();
            //                UcLogIn.AutenticarUsuarioPublico((int)BusinessVariables.EnumTiposUsuario.ClienteInvitado);
            //                break;
            //            case (int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado:
            //                areaSeleccionada = 6;
            //                Session["AreaSeleccionada"] = areaSeleccionada;
            //                rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico((int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado, areaSeleccionada, areaSeleccionada != 0);
            //                rptMenu.DataBind();
            //                UcLogIn.AutenticarUsuarioPublico((int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado);
            //                break;

            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
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
            //try
            //{
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptClose", "CierraPopup(\"#modal-new-ticket\");", true);

            //    lblNoTicket.Text = ucTicketPortal.TicketGenerado.ToString();
            //    lblRandom.Text = ucTicketPortal.RandomGenerado;
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptOpen", "MostrarPopup(\"#modalExitoTicket\");", true);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void rptMenu_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //try
            //{
            //    Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu1"));
            //    if (rptSubMenu != null)
            //    {
            //        rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
            //        rptSubMenu.DataBind();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }
        protected void rptSubMenu1_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //try
            //{
            //    Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu2"));
            //    if (rptSubMenu != null)
            //    {
            //        rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
            //        rptSubMenu.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}

        }

        protected void rptSubMenu2_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //try
            //{
            //    Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu3"));
            //    if (rptSubMenu != null)
            //    {
            //        rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
            //        rptSubMenu.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void rptSubMenu3_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //try
            //{
            //    Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu4"));
            //    if (rptSubMenu != null)
            //    {
            //        rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
            //        rptSubMenu.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void rptSubMenu4_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //try
            //{
            //    Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu5"));
            //    if (rptSubMenu != null)
            //    {
            //        rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
            //        rptSubMenu.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void rptSubMenu5_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //try
            //{
            //    Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu6"));
            //    if (rptSubMenu != null)
            //    {
            //        rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
            //        rptSubMenu.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void rptSubMenu6_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //try
            //{
            //    Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu7"));
            //    if (rptSubMenu != null)
            //    {
            //        rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
            //        rptSubMenu.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void lbtnCteArea_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    int areaSeleccionada = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            //    Session["AreaSeleccionada"] = areaSeleccionada;
            //    rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico((int)BusinessVariables.EnumTiposUsuario.ClienteInvitado, areaSeleccionada, areaSeleccionada != 0);
            //    rptMenu.DataBind();
            //    UcLogIn.AutenticarUsuarioPublico((int)BusinessVariables.EnumTiposUsuario.ClienteInvitado);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        public void CargaPerfil(int idUsuario)
        {
            //try
            //{
            //    int areaSeleccionada = (int)BusinessVariables.EnumRoles.Usuario;
            //    Session["AreaSeleccionada"] = areaSeleccionada;
            //    rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico(idUsuario, areaSeleccionada, areaSeleccionada != 0);
            //    rptMenu.DataBind();
            //    UcLogIn.AutenticarUsuarioPublico((int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }
        protected void lbtnEmpleadoArea_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    int areaSeleccionada = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            //    Session["AreaSeleccionada"] = areaSeleccionada;
            //    rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico((int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado, areaSeleccionada, areaSeleccionada != 0);
            //    rptMenu.DataBind();
            //    UcLogIn.AutenticarUsuarioPublico((int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void lbtnProveedorArea_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    int areaSeleccionada = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            //    Session["AreaSeleccionada"] = areaSeleccionada;
            //    rptMenu.DataSource = _servicioSeguridad.ObtenerMenuPublico((int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado, areaSeleccionada, areaSeleccionada != 0);
            //    rptMenu.DataBind();
            //    UcLogIn.AutenticarUsuarioPublico((int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void btnLogIn_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalSingIn\");", true);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void lnkConsultaticket_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    Response.Redirect(ResolveUrl("~/Publico/Consultas/FrmConsultaTicket.aspx"));
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void btnCerrarTicket_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    ucTicketPortal.Limpiar();
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptClose", "CierraPopup(\"#modal-new-ticket\");", true);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }

        protected void btnCerrarExito_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    ucTicketPortal.Limpiar();
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptOpen", "MostrarPopup(\"#modalExitoTicket\");", true);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    Alerta = _lstError;
            //}
        }
    }
}



