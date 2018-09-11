using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceSistemaEstatus;
using KiiniHelp.ServiceSistemaSubRol;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KiiniHelp.ServiceUsuario;
using Telerik.Web.UI;
using Image = System.Web.UI.WebControls.Image;

namespace KiiniHelp.UserControls.Detalles
{
    public delegate void DelegateCargaTicket(int idTicket, string titulo, bool asigna);

    public delegate void DelegateCierraTicket(int idTicket, bool redirect);
    public partial class UcTicketDetalle : UserControl, IControllerModal
    {
        public event DelegateCargaTicket OnCargarTicket;
        public event DelegateCierraTicket OnCierraTicket;
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceAtencionTicketClient _servicioAtencionTicket = new ServiceAtencionTicketClient();
        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();
        private readonly ServiceUsuariosClient _servicioUsuario = new ServiceUsuariosClient();
        private readonly ServiceSubRolClient _servicioSubRol = new ServiceSubRolClient();
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

        private bool GrupoConSupervisor
        {
            get { return bool.Parse(hfGrupoConSupervisor.Value); }
            set { hfGrupoConSupervisor.Value = value.ToString(); }
        }

        private int? IdNivelAsignacion
        {
            get
            {
                return string.IsNullOrEmpty(hfNivelAsignacion.Value) ? null : (int?)int.Parse(hfNivelAsignacion.Value);
            }
            set { hfNivelAsignacion.Value = value.ToString(); }
        }

        public int IdSubRolActual
        {
            get
            {
                return IdNivelAsignacion == null
                    ? GrupoConSupervisor
                        ? (int)BusinessVariables.EnumSubRoles.Supervisor
                        : (int)BusinessVariables.EnumSubRoles.PrimererNivel
                    : (int)IdNivelAsignacion + 2;
            }
        }

        public int? IdNivelParaAsignacion
        {
            get { return string.IsNullOrEmpty(hdIdRolAsignacionPertenece.Value) ? null :  (int?)int.Parse(hdIdRolAsignacionPertenece.Value); }
            set { hdIdRolAsignacionPertenece.Value = value.ToString(); }
        }
        private int IdUsuarioLevanto
        {
            get { return int.Parse(hfUsuarioLevanto.Value); }
            set { hfUsuarioLevanto.Value = value.ToString(); }
        }

        private List<HelperConversacionDetalle> ConversacionTicketActivo
        {
            get { return (List<HelperConversacionDetalle>)Session["ConversacionTicketActivo"]; }
            set { Session["ConversacionTicketActivo"] = value; }
        }
        private List<HelperEvento> EventosTicket
        {
            get { return (List<HelperEvento>)Session["EventosTicket"]; }
            set { Session["EventosTicket"] = value; }
        }



        private int IdUsuarioAsignacion
        {
            get
            {
                int result = ((Usuario)Session["UserData"]).Id;
                if (hfIdUsuarioAsignacion.Value != string.Empty)
                    result = int.Parse(hfIdUsuarioAsignacion.Value);
                return result;
            }
            set { hfIdUsuarioAsignacion.Value = value.ToString(); }
        }



        #endregion Propiedades

        #region Metodos
        public void LlenaTicket(int idTicket, bool asigna)
        {
            try
            {
                HelperTicketEnAtencion ticket = _servicioAtencionTicket.ObtenerTicketEnAtencion(idTicket, ((Usuario)Session["UserData"]).Id, false);
                if (ticket != null)
                {
                    Asigna = ticket.PuedeAsignar;
                    IdTicket = ticket.IdTicket;
                    EsPropietario = ticket.EsPropietario;
                    IdEstatusAsignacion = ticket.IdEstatusAsignacion;
                    IdEstatusTicket = ticket.IdEstatusTicket;
                    IdGrupoAsignado = ticket.IdGrupoAsignado;
                    GrupoConSupervisor = ticket.GrupoConSupervisor;
                    lblNoticket.Text = ticket.IdTicket.ToString();
                    lblTituloTicket.Text = ticket.Tipificacion;

                    lblNombreCorreo.Text = string.Format("{0} &#60;{1}&#62;", ticket.UsuarioLevanto.NombreCompleto, ticket.CorreoTicket);
                    lblNombreU.Text = ticket.UsuarioSolicito.NombreCompleto;
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
                    IdNivelAsignacion = ticket.IdNivelAsignacion;
                    IdUsuarioLevanto = ticket.UsuarioLevanto.IdUsuario;

                    LlenaDatosUsuario(ticket.UsuarioSolicito);
                    ConversacionTicketActivo = ticket.Conversaciones;
                    EventosTicket = ticket.Eventos;
                    LlenaConversacion(0);
                    LlenaEventos();
                    UcDetalleMascaraCaptura.IdTicket = idTicket;
                    divMovimientos.Visible = asigna;
                    ddlCambiarAsignar.Enabled = asigna;
                    ddlCambiarEstatus.Enabled = ticket.EsPropietario;
                    btnEnviar.Enabled = ticket.EsPropietario || asigna;
                    if (Asigna)
                    {
                        LlenaAsignaciones(((Usuario)Session["UserData"]).Id);
                    }
                    if (ticket.EsPropietario)
                    {
                        LlenaEstatus(EsPropietario, IdSubRolActual);
                    }

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

        private void LlenaEventos()
        {
            try
            {
                rptEventos.DataSource = EventosTicket;
                rptEventos.DataBind();
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
                    spanTu.Style.Add("background", usuario.TipoUsuarioColor);
                    lblTipoUsuarioDetalle.Text = usuario.TipoUsuarioDescripcion.Substring(0, 1);
                    iVip.Visible = usuario.Vip;
                    lblFechaUltimaconexion.Text = usuario.FechaUltimoLogin;

                    rddConcentradoTicketsUsuario.DataSource = usuario.TicketsAbiertos;
                    rddConcentradoTicketsUsuario.DataBind();

                    //ddlTicketUsuario.DataSource = usuario.TicketsAbiertos;
                    //ddlTicketUsuario.DataTextField = "Tipificacion"; //"IdTicket" +
                    //ddlTicketUsuario.DataValueField = "IdTicket";
                    //ddlTicketUsuario.DataBind();
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
                    imgProfileNewComment.ImageUrl = ((Usuario)Session["UserData"]).Foto != null ? "~/DisplayImages.ashx?id=" + ((Usuario)Session["UserData"]).Id : "~/assets/images/profiles/profile-1.png";
                    byte[] foto = new ServiceUsuariosClient().ObtenerFoto(usuario.IdUsuario);
                    if (foto != null)
                    {

                        imgUsuarioTicket.ImageUrl = "~/DisplayImages.ashx?id=" + usuario.IdUsuario;
                        imgUsuarioDetalle.ImageUrl = "~/DisplayImages.ashx?id=" + usuario.IdUsuario;
                    }
                    else
                    {
                        imgUsuarioTicket.ImageUrl = "~/assets/images/profiles/profile-square-1.png";
                        imgUsuarioDetalle.ImageUrl = "~/assets/images/profiles/profile-square-1.png";
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        void ValidaCaptura()
        {
            try
            {
                if (ddlCambiarAsignar.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione && divUsuariosAsignacion.Visible)
                {
                    if (ddlUsuarioAsignacion.Entries.Count <= 0)
                        throw new Exception("Debe seleccionar un agente para asignar");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                txtConversacion.BackColor = Color.FromArgb(255, 246, 211);
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
        protected void rddConcentradoTicketsUsuario_OnSelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                if (rddConcentradoTicketsUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                Label lblIdTicket = (Label)rddConcentradoTicketsUsuario.Items[rddConcentradoTicketsUsuario.SelectedIndex].FindControl("lblIdTicket");
                Label lblTitulo = (Label)rddConcentradoTicketsUsuario.Items[rddConcentradoTicketsUsuario.SelectedIndex].FindControl("lblTitulo");
                Label lblAcceso = (Label)rddConcentradoTicketsUsuario.Items[rddConcentradoTicketsUsuario.SelectedIndex].FindControl("lblAcceso");
                Label lblAsigna = (Label)rddConcentradoTicketsUsuario.Items[rddConcentradoTicketsUsuario.SelectedIndex].FindControl("lblAsigna");
                if (bool.Parse(lblAcceso.Text))
                    if (lblIdTicket != null && lblTitulo != null && lblAsigna != null)
                    {
                        int idTicket = int.Parse(lblIdTicket.Text);
                        string titulo = lblTitulo.Text;
                        bool asigna = bool.Parse(lblAsigna.Text);
                        if (idTicket != 0)
                            if (OnCargarTicket != null)
                                OnCargarTicket(idTicket, titulo, asigna);
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
                int? idEstatusTicket = null, idEstatusAsignacion = null, idNivelAsignado = null, idUsuarioAsignado = null;
                string mensajeConversacion = string.Empty;
                bool conversacionPrivada = false, enviaCorreo = false;
                const bool sistema = false;
                int idUsuarioGeneraEvento = (((Usuario)Session["UserData"]).Id);

                ValidaCaptura();

                if (ddlCambiarEstatus.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idEstatusTicket = int.Parse(ddlCambiarEstatus.SelectedValue);
                if (ddlCambiarAsignar.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idEstatusAsignacion = int.Parse(ddlCambiarAsignar.SelectedValue);

                if (ddlCambiarAsignar.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    if (idEstatusAsignacion != null && _servicioEstatus.HasComentarioObligatorio(idUsuarioGeneraEvento, IdGrupoAsignado, IdSubRolActual, IdEstatusAsignacion, (int)idEstatusAsignacion, EsPropietario))
                        if (txtComentarioAsignacion.Text.Trim() == string.Empty)
                        {

                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalComentarioObligado\");", true);
                            return;
                        }

                if (!string.IsNullOrEmpty(txtConversacion.Text.Trim()))
                {
                    mensajeConversacion = txtConversacion.Text.Trim();
                    conversacionPrivada = rbtnPrivate.Checked;
                    enviaCorreo = !rbtnPrivate.Checked;
                }

                if (ddlCambiarEstatus.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    if (idEstatusTicket != null && _servicioEstatus.HasCambioEstatusComentarioObligatorio(Session["UserData"] != null ? idUsuarioGeneraEvento : (int?) null, IdTicket, (int)idEstatusTicket, EsPropietario, false))
                        if (txtComentarioAsignacion.Text.Trim() == string.Empty)
                        {

                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalComentarioObligado\");", true);
                            return;
                        }

                string comentarioAsignacion = txtComentarioAsignacion.Text.Trim();
                idNivelAsignado = IdNivelParaAsignacion;
                idUsuarioAsignado = IdUsuarioAsignacion;

                

                _servicioAtencionTicket.GenerarEvento(IdTicket, idUsuarioGeneraEvento, idEstatusTicket, idEstatusAsignacion, idNivelAsignado, idUsuarioAsignado, mensajeConversacion, conversacionPrivada, enviaCorreo, sistema, null, comentarioAsignacion, EsPropietario);

                if (OnCierraTicket != null)
                    OnCierraTicket(IdTicket, idEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado || idEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado || idEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto);
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
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
        protected void btnCerrarComentarios_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtComentarioAsignacion.Text.Trim()))
                    throw new Exception("Debe ingresar un comentario.");
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalComentarioObligado\");", true);
                btnEnviar_OnClick(null, null);
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

        #region Asignacion

        private void LlenaAsignaciones(int idUsuario)
        {
            try
            {
                Metodos.LimpiarCombo(ddlCambiarAsignar);
                ddlCambiarAsignar.DataSource = _servicioEstatus.ObtenerEstatusAsignacionUsuario(idUsuario, IdGrupoAsignado, IdEstatusAsignacion, EsPropietario, IdSubRolActual, true);
                ddlCambiarAsignar.DataTextField = "Descripcion";
                ddlCambiarAsignar.DataValueField = "Id";
                ddlCambiarAsignar.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void LLenaUsuarios()
        {
            try
            {
                List<int> lstSubRoles = ((Usuario)Session["UserData"]).UsuarioGrupo.Where(w => w.SubGrupoUsuario != null && w.IdGrupoUsuario == IdGrupoAsignado).Select(s => s.SubGrupoUsuario).Select(subRol => subRol.IdSubRol).ToList();
                List<SubRolEscalacionPermitida> lstAsignacionesPermitidas = new List<SubRolEscalacionPermitida>();
                switch (int.Parse(ddlCambiarAsignar.SelectedValue))
                {
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Asignado:
                        foreach (int subRol in lstSubRoles)
                        {
                            lstAsignacionesPermitidas.AddRange(_servicioSubRol.ObtenerEscalacion(subRol, int.Parse(ddlCambiarAsignar.SelectedValue), null));
                        }

                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.ReAsignado:
                        lstAsignacionesPermitidas.AddRange(_servicioSubRol.ObtenerEscalacion(IdSubRolActual, int.Parse(ddlCambiarAsignar.SelectedValue), IdNivelAsignacion));
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Escalado:
                        foreach (int subRol in lstSubRoles)
                        {
                            lstAsignacionesPermitidas.AddRange(_servicioSubRol.ObtenerEscalacion(subRol, int.Parse(ddlCambiarAsignar.SelectedValue), IdNivelAsignacion));
                        }
                        break;
                }
                int idUsuario = ((Usuario)Session["UserData"]).Id;
                List<int> sbrls = lstAsignacionesPermitidas.Select(s => s.IdSubRolPermitido).Distinct().ToList();
                List<HelperUsuarioAgente> lstUsuario = _servicioUsuario.ObtenerUsuarioAgenteByGrupoUsuario(IdGrupoAsignado, idUsuario, sbrls).ToList();
                ddlUsuarioAsignacion.DataFieldID = "IdUsuario";
                ddlUsuarioAsignacion.DataFieldParentID = "IdSubRol";
                ddlUsuarioAsignacion.DataValueField = "IdUsuario";
                ddlUsuarioAsignacion.DataTextField = "NombreUsuario";
                ddlUsuarioAsignacion.DataSource = lstUsuario;
                ddlUsuarioAsignacion.DataBind();
                divUsuariosAsignacion.Visible = lstUsuario.Any();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        protected void ddlCambiarAsignar_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlCambiarEstatus);
                divUsuariosAsignacion.Visible = false;
                if (ddlCambiarAsignar.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    LlenaEstatus(EsPropietario, IdSubRolActual);
                    return;
                }
                if (int.Parse(ddlCambiarAsignar.SelectedValue) !=
                    (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado)
                    LLenaUsuarios();
                else
                {
                    LlenaEstatus(true, !IdNivelAsignacion.HasValue ? GrupoConSupervisor ? (int)BusinessVariables.EnumSubRoles.Supervisor : (int)BusinessVariables.EnumSubRoles.PrimererNivel : (int)IdNivelAsignacion);
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
        protected void ddlUsuarioAsignacion_OnEntriesAdded(object sender, DropDownTreeEntriesEventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlCambiarEstatus);
                if (!divUsuariosAsignacion.Visible)
                    hfIdUsuarioAsignacion.Value = string.Empty;
                IdUsuarioAsignacion = int.Parse(ddlUsuarioAsignacion.SelectedValue);
                int parent = int.Parse(ddlUsuarioAsignacion.EmbeddedTree.SelectedNode.ParentNode.Value);
                IdNivelParaAsignacion = parent;
                LlenaEstatus(false, IdNivelParaAsignacion);
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

        #endregion Asignacion

        #region Estatus
        private void LlenaEstatus(bool espropietario, int? nivelAsignacion)
        {
            try
            {
                Metodos.LimpiarCombo(ddlCambiarEstatus);
                if (nivelAsignacion == null)
                    return;
                List<EstatusTicket> lstEstatus = _servicioEstatus.ObtenerEstatusTicketUsuario(IdUsuarioAsignacion, IdGrupoAsignado, IdEstatusTicket, espropietario, nivelAsignacion, true);
                ddlCambiarEstatus.DataSource = lstEstatus;
                ddlCambiarEstatus.DataTextField = "Descripcion";
                ddlCambiarEstatus.DataValueField = "Id";
                ddlCambiarEstatus.DataBind();
                ddlCambiarEstatus.Enabled = lstEstatus.Any();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion Estatus

        protected void ddlHistorial_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlHistorial.SelectedValue == "1")
                {
                    divHistorial.Visible = true;
                    divtabHistorial.Visible = true;
                    divEventos.Visible = false;
                }
                else
                {
                    divHistorial.Visible = false;
                    divtabHistorial.Visible = false;
                    divEventos.Visible = true;
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

        protected void lnkBtndeshacer_OnClick(object sender, EventArgs e)
        {
            try
            {
                ddlCambiarAsignar.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddlCambiarAsignar_OnSelectedIndexChanged(null, null);
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