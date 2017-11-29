using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcTicketDetalle : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTicketClient _servicioTicket = new ServiceTicketClient();
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

        private int IdTicket
        {
            get { return int.Parse(hfTicketActivo.Value); }
            set { hfTicketActivo.Value = value.ToString(); }
        }

        private bool Asigna
        {
            get { return bool.Parse(hfAsigna.Value); }
            set { hfAsigna.Value = value.ToString(); }
        }

        public int IdEstatusAsignacion
        {
            get { return Convert.ToInt32(hfIdEstatusAsignacion.Value); }
            set
            {
                hfIdEstatusAsignacion.Value = value.ToString();
            }
        }
        public int IdEstatusTicket
        {
            get { return Convert.ToInt32(hfIdEstatusTicket.Value); }
            set
            {
                hfIdEstatusTicket.Value = value.ToString();
            }
        }

        public bool EsPropietario
        {
            get { return Convert.ToBoolean(hfEsPropietario.Value); }
            set { hfEsPropietario.Value = value.ToString(); }
        }

        private int IdGrupoAsignado
        {
            get { return Convert.ToInt32(hfGrupoAsignado.Value); }
            set { hfGrupoAsignado.Value = value.ToString(); }
        }

        private int IdNivelAsignacion
        {
            get { return int.Parse(hfNivelAsignacion.Value); }
            set { hfNivelAsignacion.Value = value.ToString(); }
        }

        private List<HelperConversacionDetalle> ConversacionTicketActivo
        {
            get { return (List<HelperConversacionDetalle>)Session["ConversacionTicketActivo"]; }
            set { Session["ConversacionTicketActivo"] = value; }
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
        public void LlenaTicket(int idTicket, bool asigna)
        {
            try
            {
                HelperticketEnAtencion ticket = _servicioAtencionTicket.ObtenerTicketEnAtencion(idTicket, ((Usuario)Session["UserData"]).Id);
                //HelperTicketDetalle ticket = _servicioTicket.ObtenerTicket(idTicket, ((Usuario)Session["UserData"]).Id);
                if (ticket != null)
                {
                    Asigna = asigna;
                    IdTicket = ticket.IdTicket;
                    EsPropietario = ticket.EsPropietario;
                    IdEstatusAsignacion = ticket.IdEstatusAsignacion;
                    IdEstatusTicket = ticket.IdEstatusTicket;
                    IdGrupoAsignado = ticket.IdGrupoAsignado;

                    lblNoticket.Text = ticket.IdTicket.ToString();
                    lblTituloTicket.Text = ticket.Tipificacion;
                    lblNombreCorreo.Text = string.Format("{0} {1}", ticket.UsuarioLevanto.NombreCompleto, ticket.CorreoTicket);
                    lblFechaAlta.Text = ticket.FechaLevanto;
                    imgPrioridad.ImageUrl = "~/assets/images/icons/" + ticket.Impacto;
                    imgSLA.ImageUrl = ticket.DentroSla ? "~/assets/images/icons/SLA_verde.png" : "~/assets/images/icons/SLA_rojo.png";
                    lblTiempoRestanteSLa.Text = "Diferencia";
                    divEstatus.Style.Add("background-color", ticket.ColorEstatus);
                    lblEstatus.Text = ticket.DescripcionEstatusTicket;
                    IdNivelAsignacion = ticket.IdNivelAsignacion.HasValue ? (int)ticket.IdNivelAsignacion : 0;

                    LlenaDatosUsuario(ticket.UsuarioLevanto);
                    ConversacionTicketActivo = ticket.Conversaciones;
                    LlenaConversacion(0);
                    UcDetalleMascaraCaptura.IdTicket = idTicket;
                    btnAsignar.Enabled = asigna;
                    btnCambiaEstatus.Enabled = ticket.EsPropietario;
                    btnSendPublic.Enabled = ticket.EsPropietario;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LlenaDatosUsuario(HelperUsuario usuario)
        {
            try
            {
                if (usuario != null)
                {
                    lblNombreDetalle.Text = usuario.NombreCompleto;
                    lblTipoUsuarioDetalle.Text = usuario.TipoUsuarioDescripcion.Substring(0, 1);
                    imgVip.Visible = usuario.Vip;
                    lblFechaUltimaconexion.Text = usuario.FechaUltimoLogin;
                    ddlTicketUsuario.DataSource = usuario.TicketsAbiertos;
                    ddlTicketUsuario.DataTextField = "Tipificacion";
                    ddlTicketUsuario.DataValueField = "IdTicket";
                    ddlTicketUsuario.DataBind();

                    lblPuesto.Text = usuario.Puesto;
                    // usuario.FirstOrDefault(s => s.Obligatorio) != null ? usuario.CorreoUsuario.First(s => s.Obligatorio).Correo : string.Empty;
                    //TODO: Cambia a correo principal
                    lblCorreoPrincipal.Text = usuario.Correos.First();
                    //usuario.TelefonoUsuario.FirstOrDefault(s => s.Obligatorio) != null ? usuario.TelefonoUsuario.First(s => s.Obligatorio).Numero : string.Empty;
                    //TODO: Cambia a telefono principal
                    lblTelefonoPrincipal.Text = usuario.Telefonos.First();
                    lblOrganizacion.Text = usuario.Organizacion;
                    lblUbicacion.Text = usuario.Ubicacion;
                    lblFechaAltaDetalle.Text = usuario.Creado;
                    lblfechaUltimaActualizacion.Text = usuario.UltimaActualizacion;

                }
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
                UcCambiarEstatusTicket.OnAceptarModal += UcCambiarEstatusTicket_OnAceptarModal;
                UcCambiarEstatusTicket.OnCancelarModal += UcCambiarEstatusTicketOnCancelarModal;

                ucCambiarEstatusAsignacion.OnAceptarModal += UcCambiarEstatusAsignacion_OnAceptarModal;
                ucCambiarEstatusAsignacion.OnCancelarModal += UcCambiarEstatusAsignacionOnCancelarModal;
                if (!IsPostBack)
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["asigna"] != null)
                    {
                        LlenaTicket(int.Parse(Request.QueryString["id"]), bool.Parse(Request.QueryString["asigna"]));
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

        void UcCambiarEstatusTicket_OnAceptarModal()
        {
            try
            {

                LlenaTicket(IdTicket, Asigna);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalEstatusCambio\");", true);
                if (!UcCambiarEstatusTicket.CerroTicket)
                {
                    AgenteMaster master = Parent.Page.Master as AgenteMaster;
                    if (master != null)
                    {
                        master.RemoveTicketOpen(IdTicket);
                    }
                }
                if (UcCambiarEstatusTicket.CerroTicket)
                {
                    string url = ResolveUrl("~/FrmEncuesta.aspx?IdTipoServicio=" + (int)BusinessVariables.EnumTipoArbol.SolicitarServicio + "&IdTicket=" + hfTicketActivo.Value);
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
                AgenteMaster master = Parent.Page.Master as AgenteMaster;
                if (master != null)
                {
                    master.RemoveTicketOpen(IdTicket);
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
                ucCambiarEstatusAsignacion.IdNivelEstatusAsignacionActual = IdNivelAsignacion;
                ucCambiarEstatusAsignacion.IdEstatusAsignacionActual = IdEstatusAsignacion;
                ucCambiarEstatusAsignacion.EsPropietario = EsPropietario;
                ucCambiarEstatusAsignacion.IdTicket = IdTicket;
                ucCambiarEstatusAsignacion.IdGrupo = IdGrupoAsignado;
                ucCambiarEstatusAsignacion.IdUsuario = ((Usuario)Session["UserData"]).Id;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAsignacionCambio\");", true);
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
        protected void btnCambiarEstatus_OnClick(object sender, EventArgs e)
        {
            try
            {
                UcCambiarEstatusTicket.EsPropietario = EsPropietario;
                UcCambiarEstatusTicket.IdTicket = IdTicket;
                UcCambiarEstatusTicket.IdEstatusActual = IdEstatusTicket;
                UcCambiarEstatusTicket.IdGrupo = IdGrupoAsignado;
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
                Alerta = _lstError;
            }
        }
        protected void rbtnPublics_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtConversacion.BackColor = Color.White;
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
        protected void rbtnPrivate_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtConversacion.BackColor = Color.FromArgb(254, 246, 159);
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
        protected void btnSendPublic_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtConversacion.Text == string.Empty)
                    throw new Exception("Ingrese un comentario.");
                _servicioAtencionTicket.AgregarComentarioConversacionTicket(IdTicket, ((Usuario)Session["UserData"]).Id, txtConversacion.Text, false, null, rbtnPrivate.Checked, !rbtnPrivate.Checked);
                LlenaTicket(IdTicket, Asigna);
                if (rbtnConversacionPublico.Checked)
                    LlenaConversacion(1);
                else if (rbtnConversacionPrivado.Checked)
                {
                    LlenaConversacion(2);
                }
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
        protected void rbtnConversacionTodos_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaConversacion(0);
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
        protected void rbtnConversacionPublico_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaConversacion(1);
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
        protected void rbtnConversacionPrivado_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaConversacion(2);
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