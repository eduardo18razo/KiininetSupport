﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSeguridad;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using Telerik.Web.UI;
using Menu = KiiniNet.Entities.Cat.Sistema.Menu;

namespace KiiniHelp
{
    public partial class AgenteMaster : MasterPage
    {
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
        private readonly ServiceAtencionTicketClient _servicioAtencion = new ServiceAtencionTicketClient();

        private List<string> _lstError = new List<string>();
        private class TicketSeleccionado
        {
            public int IdTicket { get; set; }
            public string Title { get; set; }
            public bool Nuevo { get; set; }
        }

        private void LlenaTicketsAbiertos()
        {
            try
            {
                int totalTickets = TicketsAbiertos.Count;

                if (totalTickets > 6)
                {
                    upMasTickets.Visible = true;
                    rptTicketsAbiertosExtra.DataSource = TicketsAbiertos.Skip(6).ToList();
                    rptTicketsAbiertosExtra.DataBind();
                }
                else
                {
                    upMasTickets.Visible = false;
                }

                rptTicketsAbiertos.DataSource = TicketsAbiertos.Take(6);
                rptTicketsAbiertos.DataBind();

                upTabsTickets.Update();
                upMasTickets.Update();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private List<TicketSeleccionado> TicketsAbiertos
        {
            get
            {
                if (Session["TicketsAbiertos"] == null)
                    Session["TicketsAbiertos"] = new List<TicketSeleccionado>();
                return (List<TicketSeleccionado>)Session["TicketsAbiertos"];
            }
            set { Session["TicketsAbiertos"] = value; }
        }


        public void AddNewTicket(int idUsuarioSolicita)
        {
            try
            {
                TicketsAbiertos.Add(new TicketSeleccionado { IdTicket = idUsuarioSolicita, Title = "Nuevo Ticket", Nuevo = true });
                LlenaTicketsAbiertos();
                Response.Redirect("~/Agente/FrmAgenteNuevoTicket.aspx?idUsuarioSolicitante=" + idUsuarioSolicita);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void AddTicketOpen(int idTicket, string titulo)
        {
            try
            {
                if (!TicketsAbiertos.Any(a => a.IdTicket == idTicket))
                {
                    TicketsAbiertos.Add(new TicketSeleccionado { IdTicket = idTicket, Title = titulo, Nuevo = false });
                    LlenaTicketsAbiertos();
                }
                else
                {

                }
                Response.Redirect("~/Agente/FrmTicket.aspx?id=" + idTicket);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool CambiaTicket
        {
            get
            {
                hfCambiaTickes.Value = hfCambiaTickes.Value.Trim() == string.Empty ? true.ToString() : hfCambiaTickes.Value.Trim();
                return bool.Parse(hfCambiaTickes.Value);
            }
            set { hfCambiaTickes.Value = value.ToString(); }
        }
        public void RemoveTicketOpen(int idTicket)
        {
            try
            {
                int index = TicketsAbiertos.FindIndex(a => a.IdTicket == idTicket);
                TicketsAbiertos.Remove(TicketsAbiertos.Single(s => s.IdTicket == idTicket));
                LlenaTicketsAbiertos();


                if (CambiaTicket)
                {
                    if (index >= TicketsAbiertos.Count)
                        index = TicketsAbiertos.Count - 1;
                    if (index < 0)
                        Response.Redirect("~/Agente/Bandeja.aspx");
                    else
                        Response.Redirect("~/Agente/FrmTicket.aspx?id=" + TicketsAbiertos[index].IdTicket);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

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

        private void ObtenerAreas()
        {
            try
            {
                List<Rol> lstRoles = _servicioSeguridad.ObtenerRolesUsuario(((Usuario)Session["UserData"]).Id);
                if (lstRoles.Count == 1)
                {
                    Session["RolSeleccionado"] = lstRoles.First().Id;
                    Session["CargaInicialModal"] = "True";
                }
                if (Session["RolSeleccionado"] != null) hfAreaSeleccionada.Value =
                        lstRoles.Single(s => s.Id == int.Parse(Session["RolSeleccionado"].ToString())).Descripcion;
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

        private void ActualizaTicketsAsignados()
        {
            try
            {
                lblNoTicketsAsignados.Text = _servicioAtencion.ObtenerNumeroTicketsEnAtencionNuevos(((Usuario)Session["UserData"]).Id).ToString();
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
                if (myCookie == null || Session["UserData"] == null)
                {
                    Response.Redirect(ResolveUrl("~/Default.aspx"));
                }
                lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
                ucTicketPortal.OnAceptarModal += UcTicketPortal_OnAceptarModal;

                if (Session["UserData"] != null && HttpContext.Current.Request.Url.Segments[HttpContext.Current.Request.Url.Segments.Count() - 1] != "FrmCambiarContrasena.aspx")
                    if (_servicioSeguridad.CaducaPassword(((Usuario)Session["UserData"]).Id))
                        Response.Redirect(ResolveUrl("~/Users/Administracion/Usuarios/FrmCambiarContrasena.aspx"));
                //lnkBtnCerrar.Visible = !ContentPlaceHolder1.Page.ToString().ToUpper().Contains("DASHBOARD");

                if (!IsPostBack && Session["UserData"] != null)
                {
                    bool administrador = false;
                    Usuario usuario = ((Usuario)Session["UserData"]);
                    if (usuario.UsuarioRol.Any(rol => rol.RolTipoUsuario.IdRol == (int)BusinessVariables.EnumRoles.Agente))
                    {
                        administrador = true;
                    }
                    if (administrador)
                        Session["CargaInicialModal"] = true.ToString();
                    hfCargaInicial.Value = (Session["CargaInicialModal"] ?? "False").ToString();
                    lblUsuario.Text = usuario.NombreCompleto;
                    int IdUsuario = usuario.Id;

                    imgPerfil.ImageUrl = usuario.Foto != null ? "~/DisplayImages.ashx?id=" + IdUsuario : "~/assets/images/profiles/profile-1.png";
                    //lblTipoUsr.Text = usuario.TipoUsuario.Descripcion;
                    ObtenerAreas();
                    int rolSeleccionado = 0;
                    if (Session["RolSeleccionado"] != null)
                        rolSeleccionado = int.Parse(Session["RolSeleccionado"].ToString());
                    rptMenu.DataSource = _servicioSeguridad.ObtenerMenuUsuario(usuario.Id, rolSeleccionado, rolSeleccionado != 0);
                    rptMenu.DataBind();
                }
                ActualizaTicketsAsignados();
                Session["ParametrosGenerales"] = _servicioParametros.ObtenerParametrosGenerales();
                LlenaTicketsAbiertos();
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
                    Session["RolSeleccionado"] = ((LinkButton)sender).CommandArgument;
                    hfAreaSeleccionada.Value = ((LinkButton)sender).Text;
                    int areaSeleccionada = 0;
                    if (Session["RolSeleccionado"] != null)
                        areaSeleccionada = int.Parse(Session["RolSeleccionado"].ToString());
                    Session["MenuRol"] = _servicioSeguridad.ObtenerMenuUsuario(usuario.Id, areaSeleccionada, areaSeleccionada != 0);
                    rptMenu.DataSource = Session["MenuRol"];
                    rptMenu.DataBind();
                    Session["CargaInicialModal"] = "True";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalRol\");", true);
                    Response.Redirect("~/Users/DashBoard.aspx");
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
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalRol\");", true);
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

        protected void lbtnTabTicket_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnk = (LinkButton)sender;
                if (lnk != null)
                {
                    if (bool.Parse(lnk.CommandName))
                        Response.Redirect("~/Agente/FrmAgenteNuevoTicket.aspx?idUsuarioSolicitante=" + ((LinkButton)sender).CommandArgument);
                    else
                        Response.Redirect("~/Agente/FrmTicket.aspx?id=" + ((LinkButton)sender).CommandArgument);
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

        protected void btnRemoveTab_OnClick(object sender, EventArgs e)
        {
            try
            {
                int idTicket = int.Parse(((LinkButton)sender).CommandArgument);
                RemoveTicketOpen(idTicket);
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
                Response.Redirect("~/Users/Operacion/FrmLevantaTicketAgente.aspx");
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