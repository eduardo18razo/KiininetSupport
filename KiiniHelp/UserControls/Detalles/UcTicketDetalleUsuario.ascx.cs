using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceAtencionTicket;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniHelp.ServiceUsuario;
using Image = System.Web.UI.WebControls.Image;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcTicketDetalleUsuario : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceAtencionTicketClient _servicioAtencionTicket = new ServiceAtencionTicketClient();
        private List<string> _lstError = new List<string>();

        #region Propiedades
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

        private int IdTicket
        {
            get { return int.Parse(lblIdTicket.Text); }
            set { lblIdTicket.Text = value.ToString(); }
        }

        private List<HelperConversacionDetalle> ConversacionTicketActivo
        {
            get { return (List<HelperConversacionDetalle>)Session["ConversacionTicketActivo"]; }
            set { Session["ConversacionTicketActivo"] = value; }
        }
        #endregion Propiedades

        #region Metodos
        public void LlenaTicket(int idTicket)
        {
            try
            {
                HelperTicketEnAtencion ticket = _servicioAtencionTicket.ObtenerTicketEnAtencion(idTicket, ((Usuario)Session["UserData"]).Id);
                if (ticket != null)
                {
                    IdTicket = ticket.IdTicket;
                    lblNoticket.Text = ticket.IdTicket.ToString();
                    lblTituloTicket.Text = ticket.Tipificacion;

                    lblNombreCorreo.Text = string.Format("{0} &#60;{1}&#62;", ticket.UsuarioLevanto.NombreCompleto, ticket.CorreoTicket);
                    lblNombreU.Text = ticket.UsuarioLevanto.NombreCompleto;
                    lblFechaAlta.Text = ticket.FechaLevanto;
                    lblFecha.Text = ticket.FechaLevanto;
                    lblAsignacion.Text = ticket.DescripcionEstatusAsignacion;
                    lblAgenteAsignado.Text = ticket.UsuarioAsignado;
                    lblAgenteAsignado.Attributes.Add("title", ticket.UsuarioAsignado);
                    iPrioridad.Visible = ticket.Impacto == "prioridadalta.png";
                    string colorSla = ticket.DentroSla ? "green" : "red";
                    iSLA.Style.Add("color", colorSla);
                    divEstatus.Style.Add("background-color", ticket.ColorEstatus);
                    lblEstatus.Text = ticket.DescripcionEstatusTicket;

                    ConversacionTicketActivo = ticket.Conversaciones;
                    LlenaConversacion(1);
                    UcDetalleMascaraCaptura.IdTicket = idTicket;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LlenaConversacion(int tipoConversacion)
        {
            try
            {
                List<HelperConversacionDetalle> lst = ConversacionTicketActivo;
                switch (tipoConversacion)
                {
                    case 1:
                        lst = lst.Where(w => !w.Privado).ToList();
                        break;
                    case 2:
                        lst = lst.Where(w => w.Privado).ToList();
                        break;
                }
                rptConversaciones.DataSource = lst;
                rptConversaciones.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion Metodos

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["IdTicket"] != null)
                    {
                        LlenaTicket(int.Parse(Request.QueryString["IdTicket"]));
                    }
                }
                else
                    UcDetalleMascaraCaptura.IdTicket = IdTicket;
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
        protected void rptConversaciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Image img = (Image)e.Item.FindControl("imgAgente");
                    if (img != null)
                    {
                        byte[] foto = new ServiceUsuariosClient().ObtenerFoto(((HelperConversacionDetalle)e.Item.DataItem).IdUsuario);
                        if (foto != null)
                            img.ImageUrl = "~/DisplayImages.ashx?id=" + ((HelperConversacionDetalle)e.Item.DataItem).IdUsuario;
                        else

                            img.ImageUrl = "~/assets/images/profiles/profile-square-1.png";

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
        protected void btnEnviar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtConversacion.Text.Trim() == string.Empty)
                    throw new Exception("Debe Especificar un comentario");

                const bool sistema = false;
                int idUsuarioGeneraEvento = (((Usuario)Session["UserData"]).Id);
                string mensajeConversacion = txtConversacion.Text.Trim();

                _servicioAtencionTicket.AgregarComentarioConversacionTicket(IdTicket, idUsuarioGeneraEvento, mensajeConversacion, sistema, null, false, true);
                LlenaTicket(IdTicket);
                txtConversacion.Text = string.Empty;
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
        protected void btnCerrarModalComentarios_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalComentarioObligado\");", true);
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
        #endregion Eventos

        protected void rptEventos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rptMovimientos = (Repeater)e.Item.FindControl("rptMovimientos");
                    if (rptMovimientos != null)
                    {
                        rptMovimientos.DataSource = ((HelperEvento)e.Item.DataItem).Movimientos;
                        rptMovimientos.DataBind();
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