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
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaMisTickets : UserControl
    {
        private readonly ServiceTicketClient _servicioTickets = new ServiceTicketClient();
        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();
        private List<string> _lstError = new List<string>();

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

        private void ObtenerTicketsPage(string filter = "")
        {
            try
            {
                List<HelperTickets> lst = _servicioTickets.ObtenerTicketsUsuario(((Usuario)Session["UserData"]).Id);

                if (filter.Trim() != string.Empty)
                {
                    int idTicket;
                    bool numeroFiltro = int.TryParse(filter, out idTicket);
                    lst = numeroFiltro ?
                        lst.Where(w => w.IdTicket == idTicket || w.Tipificacion.ToLower().Contains(filter.ToLower())).ToList() :
                        lst.Where(w => w.Tipificacion.ToLower().Contains(filter.ToLower())).ToList();
                }
                Tickets = lst;
                gvTickets.Rebind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public List<HelperTickets> Tickets
        {
            get
            {
                List<HelperTickets> data = Session["HelperMisTickets"] == null ? new List<HelperTickets>() : (List<HelperTickets>)Session["HelperMisTickets"];

                if (data == null)
                {
                    ObtenerTicketsPage();
                    data = (List<HelperTickets>)Session["HelperMisTickets"];
                }

                return data;
            }
            set { Session["HelperMisTickets"] = value; }
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
                    ObtenerTicketsPage();
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
        void ucCambiarEstatusTicket_OnCancelarModal()
        {
            try
            {
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
                    string url = ResolveUrl("~/FrmEncuesta.aspx?IdTipoServicio=" + (int)BusinessVariables.EnumTipoArbol.SolicitarServicio + "&IdTicket=" + ucCambiarEstatusTicket.IdTicket);
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptEncuesta", "OpenWindow(\"" + url + "\");", true);
                }
            }
            ObtenerTicketsPage();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script",
                "CierraPopup(\"#modalCambiaEstatus\");", true);
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ObtenerTicketsPage(txtFiltro.Text.Trim());
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

                ucCambiarEstatusTicket.EsPublico = false;
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


        protected void gvTickets_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem row = (GridDataItem)e.Item;
                if (row == null) return;
                switch (e.CommandName)
                {
                    case "RowClick":
                        int idTicket = int.Parse(row.GetDataKeyValue("IdTicket").ToString());
                        Response.Redirect("~/Users/General/FrmDetalleTicketUsuario.aspx?IdTicket=" + idTicket);
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

        protected void gvTickets_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                gvTickets.DataSource = Tickets;
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