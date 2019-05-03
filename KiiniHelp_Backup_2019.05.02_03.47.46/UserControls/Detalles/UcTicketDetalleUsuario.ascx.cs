using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceParametrosSistema;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
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
        private readonly ServiceParametrosClient _serviciosParametros = new ServiceParametrosClient();
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

        public int IdUsuario
        {
            get
            {
                int value = hfIdUsuario.Value.Trim() == string.Empty
                    ? ((Usuario)Session["UserData"]).Id
                    : int.Parse(hfIdUsuario.Value.Trim());
                return value;
            }
            set
            {
                if (Session["UserData"] == null)
                    hfIdUsuario.Value = value.ToString();
            }
        }

        public int IdTicket
        {
            get { return int.Parse(hfIdTicket.Value); }
            set
            {
                hfIdTicket.Value = value.ToString();
                LlenaTicket(value);
            }
        }

        public int IdGrupoUsuarioTicket
        {
            get { return int.Parse(hfIdGrupoUsuario.Value); }
            set
            {
                hfIdGrupoUsuario.Value = value.ToString();
            }
        }
        public int IdEstatusActual
        {
            get { return int.Parse(hfIdEstatusActual.Value); }
            set
            {
                hfIdEstatusActual.Value = value.ToString();
            }
        }

        public int TipoTicket
        {
            get { return int.Parse(hfTipoTicket.Value); }
            set
            {
                hfTipoTicket.Value = value.ToString();
            }
        }

        public bool Propietario
        {
            get { return hfPropietario.Value.Trim() != string.Empty && bool.Parse(hfPropietario.Value); }
            set
            {
                hfPropietario.Value = value.ToString();
            }
        }

        public bool TieneEncuesta
        {
            get { return bool.Parse(hfTieneEncuesta.Value); }
            set { hfTieneEncuesta.Value = value.ToString(); }
        }
        public bool EncuestaRespondida
        {
            get { return bool.Parse(hfEncuestaRespondida.Value); }
            set { hfEncuestaRespondida.Value = value.ToString(); }
        }

        private List<HelperConversacionDetalle> ConversacionTicketActivo
        {
            get { return (List<HelperConversacionDetalle>)Session["ConversacionTicketActivo"]; }
            set { Session["ConversacionTicketActivo"] = value; }
        }
        #endregion Propiedades
        public double TamañoArchivo
        {
            get
            {
                return double.Parse(hfMaxSizeAllow.Value);
            }
            set { hfMaxSizeAllow.Value = value.ToString(); }
        }

        public string ArchivosPermitidos
        {
            get
            {
                return hfFileTypes.Value;
            }
            set
            {
                hfFileTypes.Value = value;
            }
        }

        #region Metodos
        public void LlenaTicket(int idTicket)
        {
            try
            {
                HelperTicketEnAtencion ticket = _servicioAtencionTicket.ObtenerTicketEnAtencion(idTicket, IdUsuario, true);
                if (ticket != null)
                {
                    lblNoticket.Text = ticket.IdTicket.ToString();
                    lblTituloTicket.Text = ticket.Tipificacion;
                    imgProfileNewComment.ImageUrl = new ServiceUsuariosClient().ObtenerFoto(ticket.IdUsuarioSolicito) != null ? "~/DisplayImages.ashx?id=" + ticket.IdUsuarioSolicito : "~/assets/images/profiles/profile-1.png";
                    imgUsuarioTicket.ImageUrl = new ServiceUsuariosClient().ObtenerFoto(ticket.IdUsuarioSolicito) != null ? "~/DisplayImages.ashx?id=" + ticket.IdUsuarioSolicito : "~/assets/images/profiles/profile-1.png";
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
                    IdEstatusActual = ticket.IdEstatusTicket;
                    btnEstatus.Visible = ticket.EstatusDisponibles != null && ticket.EstatusDisponibles.Any();
                    IdGrupoUsuarioTicket = ticket.IdGrupoUsuario;
                    Propietario = ticket.EsPropietario;
                    TipoTicket = ticket.IdTipoTicket;
                    TieneEncuesta = ticket.TieneEncuesta;
                    ConversacionTicketActivo = ticket.Conversaciones;
                    LlenaConversacion(1);
                    UcDetalleMascaraCaptura.IdTicket = IdTicket;
                    lblFechaSla.Text = ticket.FechaHoraFinProceso != null ? ((DateTime)ticket.FechaHoraFinProceso).ToString("dd/MM/yyyy hh:mm tt") : string.Empty;
                    if (ticket.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado ||
                        ticket.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado)
                    {
                        btnEstatus.Enabled = false;
                        txtConversacion.Enabled = false;
                        btnEnviar.Enabled = false;
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
        #endregion Metodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucCambiarEstatusTicket.OnAceptarModal += UcCambiarEstatusTicket_OnAceptarModal;
                ucCambiarEstatusTicket.OnCancelarModal += UcCambiarEstatusTicketOnCancelarModal;
                if (!IsPostBack)
                {
                    ParametrosGenerales parametros = _serviciosParametros.ObtenerParametrosGenerales();
                    if (parametros != null)
                    {
                        foreach (ArchivosPermitidos alowedFile in _serviciosParametros.ObtenerArchivosPermitidos())
                        {
                            ArchivosPermitidos += string.Format("{0}|", alowedFile.Extensiones);
                        }
                        TamañoArchivo = double.Parse(parametros.TamanoDeArchivo);
                    }
                    if (Request.QueryString["IdTicket"] != null)
                    {
                        IdTicket = int.Parse(Request.QueryString["IdTicket"]);
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
        void UcCambiarEstatusTicket_OnAceptarModal()
        {
            try
            {

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalEstatusCambio\");", true);
                if (ucCambiarEstatusTicket.CerroTicket && TieneEncuesta && !EncuestaRespondida)
                {
                    LlenaTicket(IdTicket);
                    string url = ResolveUrl("~/FrmEncuesta.aspx?IdTipoServicio=" + TipoTicket + "&IdTicket=" + IdTicket);
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptEncuesta", "OpenWindow(\"" + url + "\");", true);
                }
                else
                {
                    if (ucCambiarEstatusTicket.IdEstatusSeleccionado == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado || ucCambiarEstatusTicket.IdEstatusSeleccionado == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado)
                        if (Request.UrlReferrer != null)
                        {
                            if (Request.UrlReferrer.ToString() == Request.Url.AbsoluteUri)
                                Response.Redirect("~/Users/General/FrmMisTickets.aspx");
                            else
                                Response.Redirect(Request.UrlReferrer.ToString());
                        }
                    LlenaTicket(IdTicket);
                    if(EncuestaRespondida)
                        throw new Exception("Ya se resondio encuesta con anteriorida");
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
        protected void afuArchivo_OnUploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                if (TamañoArchivo > 0)
                {
                    if ((double.Parse(e.FileSize) / 1024) > (10 * 1024))
                    {
                        Response.Write("Size is limited to 2MB");
                        throw new Exception(string.Format("El tamaño maximo de carga es de {0}MB", "10"));
                    }
                }


                List<string> lstArchivo = Session["FilesComment"] == null ? new List<string>() : (List<string>)Session["FilesComment"];
                if (lstArchivo.Any(archivosCargados => archivosCargados.Split('_')[0] == e.FileName.Split('\\').Last()))
                    return;
                string extension = Path.GetExtension(e.FileName.Split('\\').Last());
                if (extension == null) return;
                string filename = string.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}{8}", e.FileName.Split('\\').Last().Replace(extension, string.Empty), lblNoticket.Text, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                AsyncFileUpload uploadControl = (AsyncFileUpload)sender;
                if (!Directory.Exists(BusinessVariables.Directorios.RepositorioTemporalMascara))
                    Directory.CreateDirectory(BusinessVariables.Directorios.RepositorioTemporalMascara);
                uploadControl.SaveAs(BusinessVariables.Directorios.RepositorioTemporalMascara + filename);
                lstArchivo.Add(filename);
                Session["FilesComment"] = lstArchivo;
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
                    Repeater rptArchivos = (Repeater)e.Item.FindControl("rptArchivos");
                    if (img != null)
                    {
                        byte[] foto = new ServiceUsuariosClient().ObtenerFoto(((HelperConversacionDetalle)e.Item.DataItem).IdUsuario);
                        if (foto != null)
                            img.ImageUrl = "~/DisplayImages.ashx?id=" + ((HelperConversacionDetalle)e.Item.DataItem).IdUsuario;
                        else

                            img.ImageUrl = "~/assets/images/profiles/profile-square-1.png";

                    }
                    if (rptArchivos != null)
                    {
                        rptArchivos.DataSource = ((HelperConversacionDetalle)e.Item.DataItem).Archivo;
                        rptArchivos.DataBind();
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
                string mensajeConversacion = txtConversacion.Text.Trim();
                List<string> lstArchivo = Session["FilesComment"] == null ? null : (List<string>)Session["FilesComment"];
                _servicioAtencionTicket.AgregarComentarioConversacionTicket(IdTicket, IdUsuario, mensajeConversacion, sistema, lstArchivo, false, true);
                txtConversacion.Text = string.Empty;
                Session["FilesComment"] = null;
                if (Session["UserData"] != null)
                    Response.Redirect("~/Users/General/FrmMisTickets.aspx");
                else
                    LlenaTicket(IdTicket);
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
        protected void btnCambiarEstatus_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucCambiarEstatusTicket.EsPublico = true;
                ucCambiarEstatusTicket.EsPropietario = Propietario;
                ucCambiarEstatusTicket.IdTicket = IdTicket;
                ucCambiarEstatusTicket.IdEstatusActual = IdEstatusActual;
                ucCambiarEstatusTicket.IdSubRolActual = null;
                ucCambiarEstatusTicket.IdGrupo = IdGrupoUsuarioTicket;
                ucCambiarEstatusTicket.IdUsuario = IdUsuario;
                ucCambiarEstatusTicket.EsPropietario = true;
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

        #endregion Eventos
    }
}