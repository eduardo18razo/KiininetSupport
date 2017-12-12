using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceTicket;
using KiiniHelp.UserControls.Operacion;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.Publico.Consultas
{
    public partial class FrmConsultaTicket : Page
    {
        private readonly ServiceTicketClient _servicioticket = new ServiceTicketClient();
        private readonly ServiceAtencionTicketClient _servicioAtencionTicket = new ServiceAtencionTicketClient();

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
            try
            {
                Alerta = new List<string>();
                ucCambiarEstatusTicket.OnAceptarModal += UcCambiarEstatusTicket_OnAceptarModal;
                ucCambiarEstatusTicket.OnCancelarModal += UcCambiarEstatusTicketOnCancelarModal;
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

        void UcCambiarEstatusTicket_OnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalEstatusCambio\");", true);
                if (ucCambiarEstatusTicket.CerroTicket)
                {
                    if (bool.Parse(hfMuestraEncuesta.Value))
                    {
                        string urlHome = ResolveUrl("~/Default.aspx");
                        string url = ResolveUrl("~/FrmEncuesta.aspx?IdTipoServicio=" + (int)BusinessVariables.EnumTipoArbol.SolicitarServicio + "&IdTicket=" + lblticket.Text);
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptEncuesta", "OpenWindow(\"" + url + "\");window.location.replace(\"" + urlHome + "\");", true);
                    }
                }
                Response.Redirect("~/Publico/Consultas/FrmConsultaTicket.aspx?userTipe=" + (int)BusinessVariables.EnumTiposUsuario.Cliente);
                lblticket.Text = string.Empty;

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
                Alerta = _lstError;
            }
        }

        protected void btnConsultar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtTicket.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese número de ticket");
                if (txtClave.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese clave de registro");
                HelperDetalleTicket detalle = _servicioticket.ObtenerDetalleTicketNoRegistrado(int.Parse(txtTicket.Text.Trim()), txtClave.Text.Trim());
                if (detalle != null)
                {
                    ucDetalleMascaraCaptura.IdTicket = detalle.IdTicket;
                    divConsulta.Visible = false;
                    divDetalle.Visible = true;
                    lblticket.Text = detalle.IdTicket.ToString();
                    hfMuestraEncuesta.Value = detalle.TieneEncuesta.ToString();
                    //lblCveRegistro.Text = detalle.CveRegistro;
                    lblFechaActualiza.Text = detalle.AsignacionesDetalle.OrderBy(o => o.FechaMovimiento).First().FechaMovimiento.ToShortDateString();
                    lblestatus.Text = detalle.EstatusActual;
                    hfEstatusActual.Value = detalle.IdEstatusTicket.ToString();
                    hfIdUsuarioTicket.Value = detalle.IdUsuarioLevanto.ToString();
                    lblfecha.Text = detalle.FechaCreacion.ToString(CultureInfo.InvariantCulture);
                    rptComentrios.DataSource = detalle.ConversacionDetalle.Where(w=> !w.Privado);
                    rptComentrios.DataBind();
                    btnCambiaEstatus.Visible = detalle.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto;
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

        protected void rptComentrios_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptDownloads = ((Repeater)e.Item.FindControl("rptDownloads"));
                if (rptDownloads != null)
                {
                    rptDownloads.DataSource = ((HelperConversacionDetalle)e.Item.DataItem).Archivo;
                    rptDownloads.DataBind();
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

        protected void btnCambiaEstatus_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucCambiarEstatusTicket.EsPropietario = true;
                ucCambiarEstatusTicket.IdTicket = Convert.ToInt32(lblticket.Text);
                ucCambiarEstatusTicket.IdEstatusActual = int.Parse(hfEstatusActual.Value);
                ucCambiarEstatusTicket.IdGrupo = 0;
                ucCambiarEstatusTicket.IdUsuario = Session["UserData"] != null ? ((Usuario)Session["UserData"]).Id : new ServiceSecurityClient().GetUserInvitadoDataAutenticate((int)BusinessVariables.EnumTiposUsuario.Cliente).Id;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalEstatusCambio\");", true);
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

        protected void btnComentar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (hfComentario.Value == string.Empty)
                    throw new Exception("Ingrese un comentario.");
                _servicioAtencionTicket.AgregarComentarioConversacionTicket(int.Parse(lblticket.Text), int.Parse(hfIdUsuarioTicket.Value), hfComentario.Value.Trim(), false, null, false, true);
                HelperDetalleTicket detalle = _servicioticket.ObtenerDetalleTicketNoRegistrado(int.Parse(txtTicket.Text.Trim()), txtClave.Text.Trim());
                if (detalle != null)
                {
                    rptComentrios.DataSource = detalle.ConversacionDetalle;
                    rptComentrios.DataBind();
                }
                txtEditor.Text = string.Empty;
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
                Response.Redirect("~/Publico/FrmUserSelect.aspx?userTipe=" + (int)BusinessVariables.EnumTiposUsuario.Cliente);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}