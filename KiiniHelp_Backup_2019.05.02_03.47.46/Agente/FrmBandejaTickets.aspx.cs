using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.Agente
{
    public partial class FrmBandejaTickets : Page
    {
        private readonly ServiceGrupoUsuarioClient _servicioGrupos = new ServiceGrupoUsuarioClient();
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private readonly ServiceAtencionTicketClient _servicioAtencionTicket = new ServiceAtencionTicketClient();
        private List<string> _lstError = new List<string>();
        private const int PageSize = 1000;

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

        private string Sorting
        {
            get
            {
                if (ViewState["SortField"] == null)
                {
                    ViewState["SortField"] = "Default";
                }
                return ViewState["SortField"].ToString();
            }
            set { ViewState["SortField"] = value; }
        }

        private string Sortingtype
        {
            get
            {
                if (Session["sortingType"] == null)
                {
                    Session["sortingType"] = "asc";
                }
                return Session["sortingType"].ToString();
            }
            set
            {
                Session["sortingType"] = value;
            }
        }

        private void GeneraPaginado(int recordCount, int currentPage)
        {
            //try
            //{
            //    double dblPageCount = (double)(recordCount / Convert.ToDecimal(_pageSize));
            //    int pageCount = (int)Math.Ceiling(dblPageCount);
            //    List<ListItem> pages = new List<ListItem>();
            //    if (pageCount > 0)
            //    {
            //        for (int i = 1; i <= pageCount; i++)
            //        {
            //            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
            //        }
            //    }
            //    rptPager.DataSource = pages;
            //    rptPager.DataBind();
            //}
            //catch (Exception e)
            //{
            //    throw new Exception(e.Message);
            //}
        }

        private void ObtenerTicketsPage(int pageIndex)
        {
            try
            {
                //List<HelperTickets> lst = _servicioTickets.ObtenerTickets(((Usuario)Session["UserData"]).Id, pageIndex, PageSize);
                List<HelperTickets> lst = null;
                if (lst != null)
                {
                    if (ddlGrupo.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    {
                        lst = lst.Where(w => w.GrupoAsignado == ddlGrupo.SelectedItem.Text).ToList();
                    }
                    if (ddlAgente.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    {
                        lst = lst.Where(w => w.UsuarioAsignado == ddlAgente.SelectedItem.Text).ToList();
                    }
                    if (Sortingtype == "asc")
                        switch (Sorting)
                        {
                            case "sla":
                                lst = lst.OrderBy(o => o.DentroSla).ToList();
                                break;
                            case "pri":
                                lst = lst.OrderBy(o => o.ImagenPrioridad).ToList();
                                break;
                            case "vip":
                                lst = lst.OrderBy(o => o.Vip).ToList();
                                break;
                            case "tu":
                                lst = lst.OrderBy(o => o.UsuarioSolicito.TipoUsuario.Descripcion).ToList();
                                break;
                            case "NumeroTicket":
                                lst = lst.OrderBy(o => o.NumeroTicket).ToList();
                                break;
                            case "Canal":
                                lst = lst.OrderBy(o => o.Canal).ToList();
                                break;
                            case "UsuarioSolicito":
                                lst = lst.OrderBy(o => o.UsuarioSolicito.NombreCompleto).ToList();
                                break;
                            case "Asunto":
                                //lst = lst.OrderBy(o => o.Canal).ToList();
                                break;
                            case "TipoTicket":
                                lst = lst.OrderBy(o => o.TipoTicketAbreviacion).ToList();
                                break;
                            case "FechaHora":
                                lst = lst.OrderBy(o => o.FechaHora).ToList();
                                break;
                            case "EstatusTicket":
                                lst = lst.OrderBy(o => o.EstatusTicket.Descripcion).ToList();
                                break;
                            case "EstatusAsignacion":
                                lst = lst.OrderBy(o => o.EstatusAsignacion.Descripcion).ToList();
                                break;
                            case "UsuarioAsignado":
                                lst = lst.OrderBy(o => o.UsuarioAsignado).ToList();
                                break;
                            case "GrupoAsignado":
                                lst = lst.OrderBy(o => o.GrupoAsignado).ToList();
                                break;
                        }
                    else
                        switch (Sorting)
                        {
                            case "sla":
                                lst = lst.OrderByDescending(o => o.DentroSla).ToList();
                                break;
                            case "pri":
                                lst = lst.OrderByDescending(o => o.ImagenPrioridad).ToList();
                                break;
                            case "vip":
                                lst = lst.OrderByDescending(o => o.Vip).ToList();
                                break;
                            case "tu":
                                lst = lst.OrderByDescending(o => o.UsuarioSolicito.TipoUsuario.Descripcion).ToList();
                                break;
                            case "NumeroTicket":
                                lst = lst.OrderByDescending(o => o.NumeroTicket).ToList();
                                break;
                            case "Canal":
                                lst = lst.OrderByDescending(o => o.Canal).ToList();
                                break;
                            case "UsuarioSolicito":
                                lst = lst.OrderByDescending(o => o.UsuarioSolicito.NombreCompleto).ToList();
                                break;
                            case "Asunto":
                                //lst = lst.OrderByDescending(o => o.Canal).ToList();
                                break;
                            case "TipoTicket":
                                lst = lst.OrderByDescending(o => o.TipoTicketAbreviacion).ToList();
                                break;
                            case "FechaHora":
                                lst = lst.OrderByDescending(o => o.FechaHora).ToList();
                                break;
                            case "EstatusTicket":
                                lst = lst.OrderByDescending(o => o.EstatusTicket.Descripcion).ToList();
                                break;
                            case "EstatusAsignacion":
                                lst = lst.OrderByDescending(o => o.EstatusAsignacion.Descripcion).ToList();
                                break;
                            case "UsuarioAsignado":
                                lst = lst.OrderByDescending(o => o.UsuarioAsignado).ToList();
                                break;
                            case "GrupoAsignado":
                                lst = lst.OrderByDescending(o => o.GrupoAsignado).ToList();
                                break;

                        }

                    ViewState["Tipificaciones"] = lst.Select(s => s.Tipificacion).Distinct().ToList();
                    gvTickets.DataSource = lst;
                    gvTickets.DataBind();
                    lblTicketAbiertosHeader.Text = lst.Count.ToString();
                    lblTicketsAbiertos.Text = lst.Count(c => c.EstatusTicket.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto).ToString();
                    lblTicketsSinAsignar.Text = lst.Count(c => c.EstatusAsignacion.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar).ToString();
                    lblTicketsPendientes.Text = lst.Count(c => c.EstatusTicket.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera).ToString();
                    lblTicketsRecienCerrados.Text = lst.Count(c => c.RecienCerrado).ToString();
                    lblTicketsFueraSla.Text = lst.Count(c => !c.DentroSla).ToString();
                    if (lst.Count == 0 && pageIndex == 1) return;
                    int recordCount = pageIndex * PageSize;
                    GeneraPaginado(recordCount, pageIndex);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        private void LlenaCombos()
        {
            try
            {
                ddlGrupo.DataSource = _servicioGrupos.ObtenerGruposAtencionByIdUsuario(((Usuario)Session["UserData"]).Id, true);
                ddlGrupo.DataTextField = "Descripcion";
                ddlGrupo.DataValueField = "Id";
                ddlGrupo.DataBind();


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
                
                ucCambiarEstatusAsignacion.OnAceptarModal += UcCambiarEstatusAsignacion_OnAceptarModal;
                ucCambiarEstatusAsignacion.OnCancelarModal += UcCambiarEstatusAsignacionOnCancelarModal;

                if (!IsPostBack)
                {
                    if (int.Parse(Session["RolSeleccionado"].ToString()) != (int)BusinessVariables.EnumRoles.Agente)
                        Response.Redirect("~/Users/DashBoard.aspx");
                    ObtenerTicketsPage(0);
                    LlenaCombos(); 
                    AgenteMaster master = Master as AgenteMaster;
                    if (master != null)
                    {
                        master.CambiaTicket = false;
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
                Alerta = _lstError;
            }
        }

        void UcCambiarEstatusAsignacion_OnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAsignacionCambio\");", true);
                ObtenerTicketsPage(0);
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

        protected void chkboxSelectAll_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkBoxHeader = (CheckBox)gvTickets.HeaderRow.FindControl("chkboxSelectAll");
                foreach (CheckBox chkBoxRows in from GridViewRow row in gvTickets.Rows select (CheckBox)row.FindControl("chkSelected"))
                {
                    chkBoxRows.Checked = chkBoxHeader.Checked;
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

        

        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            try
            {
                ObtenerTicketsPage(0);
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
                AgenteMaster master = Master as AgenteMaster;
                if (master != null)
                {
                    master.AddTicketOpen(1, "prueba");
                    master.AddTicketOpen(2, "prueba");
                    master.AddTicketOpen(3, "prueba");
                    master.AddTicketOpen(4, "prueba");
                    master.AddTicketOpen(5, "prueba");
                    master.AddTicketOpen(6, "prueba");
                    master.AddTicketOpen(7, "prueba");
                    master.AddTicketOpen(8, "prueba");
                    master.AddTicketOpen(9, "prueba");
                    master.AddTicketOpen(11, "prueba");
                    master.AddTicketOpen(12, "prueba");
                    master.AddTicketOpen(13, "prueba");
                    master.AddTicketOpen(14, "prueba");
                    master.AddTicketOpen(15, "prueba");
                    master.AddTicketOpen(16, "prueba");
                    master.AddTicketOpen(17, "prueba");
                    master.AddTicketOpen(18, "prueba");
                    master.AddTicketOpen(19, "prueba");
                    master.AddTicketOpen(20, "prueba");
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

        protected void gvTickets_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "sort":
                        Sortingtype = Sortingtype == "asc" ? "desc" : "asc";
                        Sorting = e.CommandArgument.ToString();
                        ObtenerTicketsPage(0);
                        break;
                    case "redirect":
                        AgenteMaster master = Master as AgenteMaster;
                        if (master != null)
                        {
                            master.AddTicketOpen(int.Parse(e.CommandArgument.ToString()), "prueba");
                        }
                        Response.Redirect("~/Agente/FrmTicket.aspx?id=" + e.CommandArgument);
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

        protected void ddlGrupo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlAgente);
                if (ddlGrupo.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                ddlAgente.DataSource = _servicioUsuarios.ObtenerUsuariosByGrupoAtencion(int.Parse(ddlGrupo.SelectedValue), true);
                ddlAgente.DataTextField = "NombreCompleto";
                ddlAgente.DataValueField = "Id";
                ddlAgente.DataBind();
                ObtenerTicketsPage(0);
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

        protected void ddlAgente_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObtenerTicketsPage(0);
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

        protected void gvTickets_OnSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void btnAutoasignar_OnClick(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvTickets.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelected");
                    if (chk.Checked)
                    {
                        DataKey dataKey = gvTickets.DataKeys[row.RowIndex];
                        if (dataKey != null)
                        {
                            var t = dataKey["NumeroTicket"].ToString();
                            _servicioAtencionTicket.AutoAsignarTicket(int.Parse(t), ((Usuario)Session["UserData"]).Id);
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

        protected void btnAsignar_OnClick(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvTickets.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelected");
                    if (chk.Checked)
                    {
                        var t = gvTickets.DataKeys[row.RowIndex]["NumeroTicket"].ToString();
                        var propietario = ((Label)row.FindControl("lblEsPropieatrio")).Text;
                        var gpoAsignado = ((Label)row.FindControl("lblIdGrupoAsignado")).Text;
                        var estatusasignacion = ((Label)row.FindControl("lblEstatusAsignacionActual")).Text;
                        
                        
                        ucCambiarEstatusAsignacion.IdEstatusAsignacionActual = Convert.ToInt32(estatusasignacion);
                        ucCambiarEstatusAsignacion.EsPropietario = Convert.ToBoolean(propietario);
                        ucCambiarEstatusAsignacion.IdTicket = Convert.ToInt32(t);
                        ucCambiarEstatusAsignacion.IdGrupo = Convert.ToInt32(gpoAsignado);
                        ucCambiarEstatusAsignacion.IdUsuario = ((Usuario)Session["UserData"]).Id;
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAsignacionCambio\");", true);
                        break;
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

        protected void Escalar_OnClick(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvTickets.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelected");
                    if (chk.Checked)
                    {
                        var t = gvTickets.DataKeys[row.RowIndex]["NumeroTicket"].ToString();
                        var propietario = ((Label)row.FindControl("lblEsPropieatrio")).Text;
                        var gpoAsignado = ((Label)row.FindControl("lblIdGrupoAsignado")).Text;
                        var estatusasignacion = ((Label)row.FindControl("lblEstatusAsignacionActual")).Text;

                        ucCambiarEstatusAsignacion.IdEstatusAsignacionActual = Convert.ToInt32(estatusasignacion);
                        ucCambiarEstatusAsignacion.EsPropietario = Convert.ToBoolean(propietario);
                        ucCambiarEstatusAsignacion.IdTicket = Convert.ToInt32(t);
                        ucCambiarEstatusAsignacion.IdGrupo = Convert.ToInt32(gpoAsignado);
                        ucCambiarEstatusAsignacion.IdUsuario = ((Usuario)Session["UserData"]).Id;
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAsignacionCambio\");", true);
                        break;
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
    }
}