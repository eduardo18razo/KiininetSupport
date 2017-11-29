using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.Agente
{
    public partial class FrmTickets : Page
    {
        readonly ServiceTicketClient _servicioTickets = new ServiceTicketClient();
        private List<string> _lstError = new List<string>();
        private int _pageSize = 1000;
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

        private void ObtenerTicketsPage(int pageIndex, Dictionary<string, string> filtros, bool orden, bool asc, string ordering = "")
        {
            try
            {
                //List<HelperTickets> lst = _servicioTickets.ObtenerTickets(((Usuario)Session["UserData"]).Id, pageIndex, _pageSize);
                List<HelperTickets> lst = null;
                if (lst != null)
                {
                    foreach (KeyValuePair<string, string> filtro in filtros)
                    {
                        switch (filtro.Key)
                        {
                            case "NumeroTicket":
                                lst = lst.Where(w => w.NumeroTicket == int.Parse(filtro.Value)).ToList();
                                break;
                        }
                    }
                    if (orden && asc)
                        switch (ordering)
                        {
                            case "DateTime":
                                lst = lst.OrderBy(o => o.FechaHora).ToList();
                                break;
                        }
                    else
                        switch (ordering)
                        {
                            case "DateTime":
                                lst = lst.OrderByDescending(o => o.FechaHora).ToList();
                                break;
                        }

                    ViewState["Tipificaciones"] = lst.Select(s => s.Tipificacion).Distinct().ToList();
                    rptTickets.DataSource = lst;
                    rptTickets.DataBind();
                    if (lst.Count == 0 && pageIndex == 1) return;
                    int recordCount = pageIndex * _pageSize;
                    GeneraPaginado(recordCount, pageIndex);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        private void GeneraPaginado(int recordCount, int currentPage)
        {
            try
            {
                double dblPageCount = (double)(recordCount / Convert.ToDecimal(_pageSize));
                int pageCount = (int)Math.Ceiling(dblPageCount);
                List<ListItem> pages = new List<ListItem>();
                if (pageCount > 0)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                rptPager.DataSource = pages;
                rptPager.DataBind();
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
                _lstError = new List<string>();
                if (!IsPostBack)
                {
                    if (int.Parse(Session["RolSeleccionado"].ToString()) != (int)BusinessVariables.EnumRoles.ResponsableDeAtención)
                        Response.Redirect("~/Users/DashBoard.aspx");
                    ViewState["Column"] = "DateTime";
                    ViewState["Sortorder"] = "ASC";
                    ViewState["PageIndex"] = "0";
                    ViewState["Filtros"] = new Dictionary<string, string>();
                    ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());

                }
                UcDetalleUsuario.OnAceptarModal += UcDetalleUsuario_OnAceptarModal;
                UcDetalleUsuario.OnCancelarModal += UcDetalleUsuario1OnOnCancelarModal;

                UcCambiarEstatusTicket.OnAceptarModal += UcCambiarEstatusTicket_OnAceptarModal;
                UcCambiarEstatusTicket.OnCancelarModal += UcCambiarEstatusTicketOnCancelarModal;

                UcCambiarEstatusAsignacion.OnAceptarModal += UcCambiarEstatusAsignacion_OnAceptarModal;
                UcCambiarEstatusAsignacion.OnCancelarModal += UcCambiarEstatusAsignacionOnCancelarModal;
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

        protected void Page_Changed(object sender, EventArgs e)
        {
            try
            {
                int pageIndex = int.Parse(((LinkButton)(sender)).CommandArgument);
                ViewState["PageIndex"] = pageIndex;
                ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
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

        protected void rptTickets_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                //DropDownList ddlEstatus = (DropDownList)e.Item.FindControl("ddlEstatus");
                //DropDownList ddlAsignacion = (DropDownList)e.Item.FindControl("ddlAsignacion");
                //DropDownList ddlTipificacion = (DropDownList)e.Item.FindControl("ddlTipificacion");
                //if (ddlEstatus != null)
                //{
                //    ddlEstatus.DataSource = _servicioEstatus.ObtenerEstatusTicket(true);
                //    ddlEstatus.DataTextField = "Descripcion";
                //    ddlEstatus.DataValueField = "Id";
                //    ddlEstatus.DataBind();
                //}
                //if (ddlAsignacion != null)
                //{
                //    ddlAsignacion.DataSource = _servicioEstatus.ObtenerEstatusAsignacion(true);
                //    ddlAsignacion.DataTextField = "Descripcion";
                //    ddlAsignacion.DataValueField = "Id";
                //    ddlAsignacion.DataBind();
                //}
                //if (ddlTipificacion != null)
                //{
                //    ddlTipificacion.DataSource = ViewState["Tipificaciones"];
                //    ddlTipificacion.DataBind();
                //}
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

        protected void rptTickets_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == ViewState["Column"].ToString())
                {
                    if (ViewState["Sortorder"].ToString() == "ASC")
                        ViewState["Sortorder"] = "DESC";
                    else
                        ViewState["Sortorder"] = "ASC";
                }
                else
                {
                    ViewState["Column"] = e.CommandName;
                    ViewState["Sortorder"] = "ASC";
                }
                ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
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

        protected void txtFilerNumeroTicket_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dictionary = (Dictionary<string, string>)ViewState["Filtros"];
                if (dictionary.Any(a => a.Key == "NumeroTicket"))
                    dictionary[dictionary.SingleOrDefault(s => s.Key == "NumeroTicket").Key] = ((TextBox)sender).Text;
                else
                    dictionary.Add("NumeroTicket", ((TextBox)sender).Text);
                ViewState["Filtros"] = dictionary;
                ((TextBox)sender).Text = ((TextBox)sender).Text;
                ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
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

        protected void btnUsuario_OnClick(object sender, EventArgs e)
        {
            try
            {
                UcDetalleUsuario.FromModal = true;
                UcDetalleUsuario.IdUsuario = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleUsuario\");", true);
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

        void UcDetalleUsuario_OnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleUsuario\");", true);
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

        private void UcDetalleUsuario1OnOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleUsuario\");", true);
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

        void UcCambiarEstatusTicket_OnAceptarModal()
        {
            try
            {

                ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalEstatusCambio\");", true);
                if (UcCambiarEstatusTicket.CerroTicket)
                {
                    string url = ResolveUrl("~/FrmEncuesta.aspx?IdTipoServicio=" + (int)BusinessVariables.EnumTipoArbol.SolicitarServicio + "IdTicket=" + hfTicketActivo.Value);
                    //string s = "window.open('" + url + "', 'popup_window', 'width=600,height=600,left=300,top=100,resizable=yes');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptEncuesta", "OpenWindow(\"" + url + "\");", true);
                }
                hfTicketActivo.Value = string.Empty;
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
        void UcCambiarEstatusTicketOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalEstatusCambio\");", true);
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

        void UcCambiarEstatusAsignacionOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAsignacionCambio\");", true);
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

        void UcCambiarEstatusAsignacion_OnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAsignacionCambio\");", true);
                ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
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

        protected void btnCambiarEstatus_OnClick(object sender, EventArgs e)
        {
            try
            {
                UcCambiarEstatusTicket.EsPropietario = true;
                UcCambiarEstatusTicket.IdTicket = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                UcCambiarEstatusTicket.IdEstatusActual = int.Parse(((Label)((LinkButton)sender).NamingContainer.FindControl("lblEstatusActual")).Text);
                hfTicketActivo.Value = ((LinkButton)sender).CommandArgument;

                UcCambiarEstatusTicket.IdUsuario = ((Usuario)Session["UserData"]).Id;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalEstatusCambio\");", true);
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

        protected void btnAsignar_OnClick(object sender, EventArgs e)
        {
            try
            {
                UcCambiarEstatusAsignacion.IdEstatusAsignacionActual = Convert.ToInt32(((Label)rptTickets.Items[((RepeaterItem)((LinkButton)sender).NamingContainer).ItemIndex].FindControl("lblEstatusAsignacionActual")).Text);
                UcCambiarEstatusAsignacion.EsPropietario = Convert.ToBoolean(((Label)rptTickets.Items[((RepeaterItem)((LinkButton)sender).NamingContainer).ItemIndex].FindControl("lblEsPropietario")).Text);
                UcCambiarEstatusAsignacion.IdTicket = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                UcCambiarEstatusAsignacion.IdGrupo = Convert.ToInt32(((Label)rptTickets.Items[((RepeaterItem)((LinkButton)sender).NamingContainer).ItemIndex].FindControl("lblIdGrupoAsignado")).Text);
                UcCambiarEstatusAsignacion.IdUsuario = ((Usuario)Session["UserData"]).Id;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAsignacionCambio\");", true);
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

        protected void btnautoAsignar_OnClick(object sender, EventArgs e)
        {
            try
            {
                _servicioTickets.AutoAsignarTicket(Convert.ToInt32(((LinkButton)sender).CommandArgument), ((Usuario)Session["UserData"]).Id);
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

        protected void lbntIdticket_OnClick(object sender, EventArgs e)
        {
            try
            {
                UcDetalleTicket.IdTicket = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleTicket\");", true);
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