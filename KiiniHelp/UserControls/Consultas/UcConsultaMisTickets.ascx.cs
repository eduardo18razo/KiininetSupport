using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSistemaEstatus;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaMisTickets : UserControl
    {
        private readonly ServiceTicketClient _servicioTickets = new ServiceTicketClient();
        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();
        private List<string> _lstError = new List<string>();
        private const int PageSize = 100000;

        public List<string> Alerta
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

        private void LlenaEstatus()
        {
            try
            {
                ddlEstatus.DataSource = _servicioEstatus.ObtenerEstatusTicket(true);
                ddlEstatus.DataTextField = "Descripcion";
                ddlEstatus.DataValueField = "Id";
                ddlEstatus.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ObtenerTicketsPage(int pageIndex, Dictionary<string, string> filtros, bool orden, bool asc, string ordering = "")
        {
            try
            {
                List<HelperTickets> lst = _servicioTickets.ObtenerTicketsUsuario(((Usuario)Session["UserData"]).Id, pageIndex, PageSize);
                if (lst != null)
                {
                    if (ddlEstatus.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    {
                        int idEstatus = int.Parse(ddlEstatus.SelectedValue);
                        lst = lst.Where(w => w.EstatusTicket.Id == idEstatus).ToList();
                    }
                    foreach (KeyValuePair<string, string> filtro in filtros)
                    {
                        switch (filtro.Key)
                        {
                            case "NumeroTicket":
                                lst = lst.Where(w => w.NumeroTicket == int.Parse(filtro.Value)).ToList();
                                break;
                            case "Asunto":
                                lst = lst.Where(w => w.Tipificacion.Contains(filtro.Value)).ToList();
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
                }
                tblResults.DataSource = lst;
                tblResults.DataBind();
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
                Alerta = new List<string>();
                ucCambiarEstatusTicket.OnAceptarModal += ucCambiarEstatusTicket_OnAceptarModal;
                ucCambiarEstatusTicket.OnCancelarModal += ucCambiarEstatusTicket_OnCancelarModal;
                if (!IsPostBack)
                {
                    ViewState["Column"] = "DateTime";
                    ViewState["Sortorder"] = "ASC";
                    ViewState["PageIndex"] = "0";
                    ViewState["Filtros"] = new Dictionary<string, string>();
                    LlenaEstatus();
                    ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
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

        #region Paginador
        protected void gvPaginacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            tblResults.PageIndex = e.NewPageIndex;

            ViewState["Column"] = "DateTime";
            ViewState["Sortorder"] = "ASC";
            ViewState["PageIndex"] = "0";
            ViewState["Filtros"] = new Dictionary<string, string>();
            LlenaEstatus();
            ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
        }

        #endregion


        void ucCambiarEstatusTicket_OnCancelarModal()
        {
            try
            {
                //ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalCambiaEstatus\");", true);
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

        private void ucCambiarEstatusTicket_OnAceptarModal()
        {
            if (ucCambiarEstatusTicket.CerroTicket)
            {
                if (bool.Parse(hfMuestraEncuesta.Value))
                {
                    string url =
                        ResolveUrl("~/FrmEncuesta.aspx?IdTipoServicio=" +
                                   (int)BusinessVariables.EnumTipoArbol.SolicitarServicio + "&IdTicket=" +
                                   ucCambiarEstatusTicket.IdTicket);
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptEncuesta",
                        "OpenWindow(\"" + url + "\");", true);
                }
            }
            ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()),
                (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC",
                ViewState["Column"].ToString());
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script",
                "CierraPopup(\"#modalCambiaEstatus\");", true);
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {

                Dictionary<string, string> filter = new Dictionary<string, string>();
                if (txtFiltro.Text.Trim() != string.Empty)
                    filter.Add("Asunto", txtFiltro.Text.Trim().ToUpper());

                ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), filter, true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
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

        protected void ddlEstatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObtenerTicketsPage(int.Parse(ViewState["PageIndex"].ToString()), (Dictionary<string, string>)ViewState["Filtros"], true, ViewState["Sortorder"].ToString() == "ASC", ViewState["Column"].ToString());
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

        protected void btnCambiaEstatus_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (btn == null) return;
                hfMuestraEncuesta.Value = btn.Attributes["data-tieneEncuesta"];
                ucCambiarEstatusTicket.EsPropietario = true;
                ucCambiarEstatusTicket.IdTicket = int.Parse(btn.CommandArgument);
                ucCambiarEstatusTicket.IdEstatusActual = int.Parse(btn.CommandName);
                ucCambiarEstatusTicket.IdGrupo = 0;
                ucCambiarEstatusTicket.IdUsuario = ((Usuario)Session["UserData"]).Id;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalCambiaEstatus\");", true);
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