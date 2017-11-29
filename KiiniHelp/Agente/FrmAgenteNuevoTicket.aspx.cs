using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.Agente
{
    public partial class FrmAgenteNuevoTicket : Page
    {
        private readonly ServiceUsuariosClient _servicioUsuario = new ServiceUsuariosClient();
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

        public int IdUsuarioSolicito
        {
            get { return int.Parse(hfIdUsuarioSolicito.Value); }
            set { hfIdUsuarioSolicito.Value = value.ToString(); }
        }
        private void LlenaDatosUsuario(int idUsuario)
        {
            try
            {
                HelperUsuario usuario = _servicioUsuario.ObtenerDatosTicketUsuario(idUsuario);
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

                if (!IsPostBack)
                {
                    divFormulario.Visible = Request.Params["idArbol"] != null;
                    if (Request.Params["idUsuarioSolicitante"] != null)
                    {
                        IdUsuarioSolicito = int.Parse(Request.Params["idUsuarioSolicitante"]);
                        LlenaDatosUsuario(IdUsuarioSolicito);
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