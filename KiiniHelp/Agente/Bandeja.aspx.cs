using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceAtencionTicket;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceSistemaEstatus;
using KiiniHelp.ServiceTicket;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using Telerik.Web.UI;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniHelp.Agente
{
    //File change
    public partial class Bandeja : Page
    {
        private readonly ServiceTicketClient _servicioTickets = new ServiceTicketClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupos = new ServiceGrupoUsuarioClient();
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private readonly ServiceAtencionTicketClient _servicioAtencionTicket = new ServiceAtencionTicketClient();
        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();
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
        private List<int> EstatusAbierto
        {
            get
            {
                List<int> result = new List<int>
                {
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto

                };
                return result;
            }
        }
        private List<int> EstatusEspera
        {
            get
            {
                List<int> result = new List<int>
                {
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera
                };

                return result;
            }
        }
        private bool SinAsignar
        {
            get { return bool.Parse(fhFiltroSinAsignar.Value); }
            set { fhFiltroSinAsignar.Value = value.ToString(); }
        }
        private List<int> EstatusSinAsignar
        {
            get
            {
                List<int> result = new List<int>
                {
                    (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto,
                    (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto,
                    (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera
                };

                return result;
            }
        }
        private List<int> EstatusResuletos
        {
            get
            {
                List<int> result = new List<int>
                {
                    (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto,
                    (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                };

                return result;
            }
        }
        private bool FueraSla
        {
            get { return bool.Parse(hfFiltroSla.Value); }
            set { hfFiltroSla.Value = value.ToString(); }
        }

        private bool Resueltos
        {
            get { return bool.Parse(hfResueltos.Value); }
            set { hfResueltos.Value = value.ToString(); }
        }
        private bool RecienActualizados
        {
            get { return bool.Parse(hfRecienActualizados.Value); }
            set { hfRecienActualizados.Value = value.ToString(); }
        }
        private List<int> EstatusFueraSla
        {
            get
            {
                List<int> result = new List<int>
                {
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera
                };
                return result;
            }
        }
        private List<int> EstatusSeleccionado
        {
            get { return (List<int>)Session["EstatusSeleccionado"]; }
            set { Session["EstatusSeleccionado"] = value; }
        }
        public List<HelperTickets> Tickets
        {
            get
            {
                List<HelperTickets> data = Session["Helpertickets"] == null ? new List<HelperTickets>() : (List<HelperTickets>)Session["Helpertickets"];

                if (data == null)
                {
                    ObtenerTicketsPage();
                    data = (List<HelperTickets>)Session["Helpertickets"];
                }

                return data;
            }
            set { Session["Helpertickets"] = value; }
        }

        //public int? filaSeleccionada = 10;
        private void ObtieneTotales(List<HelperTickets> lst)
        {
            try
            {
                if (lst == null || lst.Count <= 0) return;
                lblTicketAbiertosHeader.Text = "Tickets Abiertos (" + lst.Count(c => EstatusAbierto.Contains(c.EstatusTicket.Id)).ToString() + ")";

                ((Label)btnFiltroTodos.FindControl("lblTicketsTodos")).Text = lst.Count().ToString();

                ((Label)btnFiltroAbierto.FindControl("lblTicketsAbiertos")).Text = lst.Count(c => EstatusAbierto.Contains(c.EstatusTicket.Id)).ToString();

                const int statusSinAsignar = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                ((Label)btnFiltroSinAsignar.FindControl("lblTicketsSinAsignar")).Text = lst.Count(w => EstatusSinAsignar.Contains(w.EstatusTicket.Id) && w.EstatusAsignacion.Id == statusSinAsignar).ToString();


                ((Label)btnFiltroEspera.FindControl("lblTicketsEspera")).Text = lst.Count(c => EstatusEspera.Contains(c.EstatusTicket.Id)).ToString();

                DateTime fechaInicio = DateTime.Now.AddHours(36);

                ((Label)btnFiltroResuelto.FindControl("lblTicketsResueltos")).Text = lst.Count(w => EstatusResuletos.Contains(w.EstatusTicket.Id) && w.FechaCambioEstatusAsignacion >= fechaInicio).ToString();

                ((Label)btnFueraSla.FindControl("lblTicketsFueraSla")).Text = lst.Count(w => EstatusFueraSla.Contains(w.EstatusTicket.Id) && !w.DentroSla).ToString();

                fechaInicio = DateTime.Now.AddMinutes(-60);

                ((Label)btnRecienActualizados.FindControl("lblTicketsRecienActualizados")).Text = lst.Count(w => w.FechaUltimoEvento >= fechaInicio).ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ObtenerTicketsPage()
        {
            try
            {

                List<HelperTickets> lst = _servicioTickets.ObtenerTickets(((Usuario)Session["UserData"]).Id, null, 0, 100000);
                if (lst != null)
                {
                    ObtieneTotales(lst);
                    if (EstatusSeleccionado != null && EstatusSeleccionado.Count > 0)
                        lst = lst.Where(w => EstatusSeleccionado.Contains(w.EstatusTicket.Id)).ToList();

                    int? idGrupo = null;
                    if (ddlGrupo.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        idGrupo = int.Parse(ddlGrupo.SelectedValue);

                    int? idAgente = null;
                    if (ddlAgente.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        idAgente = int.Parse(ddlAgente.SelectedValue);

                    if (idGrupo != null || idAgente != null)
                    {
                        if (idGrupo != null && idAgente != null)
                        {
                            lst = lst.Where(w => w.IdGrupoAsignado == idGrupo && w.IdUsuarioAsignado == idAgente).ToList();
                        }
                        else if (idGrupo != null)
                            lst = lst.Where(w => w.IdGrupoAsignado == idGrupo).ToList();
                        else
                            lst = lst.Where(w => w.IdUsuarioAsignado == idAgente).ToList();
                    }
                    if (SinAsignar)
                        lst = lst.Where(w => w.EstatusAsignacion.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar).ToList();
                    if (FueraSla)
                        lst = lst.Where(w => !w.DentroSla).ToList();
                    if (Resueltos)
                        lst = lst.Where(w => w.FechaCambioEstatusAsignacion >= DateTime.Now.AddHours(-36)).ToList();
                    if (RecienActualizados)
                        lst = lst.Where(w => w.FechaUltimoEvento >= DateTime.Now.AddMinutes(-60)).ToList();

                    ViewState["Tipificaciones"] = lst.Select(s => s.Tipificacion).Distinct().ToList();

                }
                Tickets = lst;
                gvTickets.Rebind();
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
                //ddlGrupo.DataSource = _servicioGrupos.ObtenerGruposAtencionByIdUsuario(((Usuario)Session["UserData"]).Id, true);
                //ddlGrupo.DataTextField = "Descripcion";
                //ddlGrupo.DataValueField = "Id";
                //ddlGrupo.DataBind();


                /**/
                Usuario usr = ((Usuario)Session["UserData"]);
                ddlGrupo.DataSource = _servicioGrupos.ObtenerGruposAtencionByIdUsuario(((Usuario)Session["UserData"]).Id, true);
                ddlGrupo.DataTextField = "Descripcion";
                ddlGrupo.DataValueField = "Id";
                ddlGrupo.DataBind();
                ddlAgente.DataSource = _servicioUsuarios.ObtenerAgentes(true);
                ddlAgente.DataTextField = "NombreCompleto";
                ddlAgente.DataValueField = "Id";
                ddlAgente.DataBind();





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
                ucAsignacionUsuario.OnCancelarModal += ucAsignacionUsuario_OnCancelarModal;

                ucCambiarEstatusAsignacion.OnAceptarModal += UcCambiarEstatusAsignacion_OnAceptarModal;
                ucCambiarEstatusAsignacion.OnCancelarModal += UcCambiarEstatusAsignacionOnCancelarModal;

                UcDetalleUsuario.OnAceptarModal += UcDetalleUsuario_OnAceptarModal;
                UcDetalleUsuario.OnCancelarModal += UcDetalleUsuario1OnOnCancelarModal;

                UcCambiarEstatusTicket.OnAceptarModal += UcCambiarEstatusTicket_OnAceptarModal;
                UcCambiarEstatusTicket.OnCancelarModal += UcCambiarEstatusTicketOnCancelarModal;

                if (!IsPostBack)
                {
                    if (Session["RolSeleccionado"] == null)
                    {
                        Response.Redirect("~/Default.aspx");
                        return;
                    }
                    if (int.Parse(Session["RolSeleccionado"].ToString()) != (int)BusinessVariables.EnumRoles.Agente)
                        Response.Redirect("~/Users/DashBoard.aspx");
                    Session["EstatusSeleccionado"] = EstatusAbierto;
                    ObtenerTicketsPage();
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
            //gvTickets.Rebind();
        }

        void ucAsignacionUsuario_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalNuevoTicketAgente\");", true);
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
                ObtenerTicketsPage();
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
                Alerta = _lstError;
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
                Alerta = _lstError;
            }
        }

        void UcCambiarEstatusTicket_OnAceptarModal()
        {
            try
            {

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalEstatusCambio\");", true);
                if (UcCambiarEstatusTicket.CerroTicket)
                {
                    string url = ResolveUrl("~/FrmEncuesta.aspx?IdTipoServicio=" + (int)BusinessVariables.EnumTipoArbol.SolicitarServicio + "&IdTicket=" + hfTicketActivo.Value);
                    //string s = "window.open('" + url + "', 'popup_window', 'width=600,height=600,left=300,top=100,resizable=yes');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptEncuesta", "OpenWindow(\"" + url + "\");", true);
                }
                hfTicketActivo.Value = string.Empty;
                ObtenerTicketsPage();

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

        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            try
            {
                ObtenerTicketsPage();
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
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalNuevoTicketAgente\");", true);
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
                //Metodos.LimpiarCombo(ddlAgente);
                //if (ddlGrupo.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                //    return;
                //ddlAgente.DataSource = _servicioUsuarios.ObtenerUsuariosByGrupoAtencion(int.Parse(ddlGrupo.SelectedValue), true);
                //ddlAgente.DataTextField = "NombreCompleto";
                //ddlAgente.DataValueField = "Id";
                //ddlAgente.DataBind();


                Metodos.LimpiarCombo(ddlAgente);
                if (ddlGrupo.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    ddlAgente.DataSource = _servicioUsuarios.ObtenerAgentes(true);
                    ddlAgente.DataTextField = "NombreCompleto";
                    ddlAgente.DataValueField = "Id";
                    ddlAgente.DataBind();
                }
                else
                {
                    GrupoUsuario gpo = _servicioGrupos.ObtenerGrupoUsuarioById(int.Parse(ddlGrupo.SelectedValue));
                    if (gpo != null)
                    {
                        ddlAgente.DataSource = _servicioUsuarios.ObtenerUsuariosByGrupoAtencion(gpo.Id, true);
                        ddlAgente.DataTextField = "NombreCompleto";
                        ddlAgente.DataValueField = "Id";
                        ddlAgente.DataBind();
                        Usuario usr = ((Usuario)Session["UserData"]);
                        switch (gpo.TieneSupervisor)
                        {
                            case true:
                                if (usr.UsuarioGrupo.Any(s => s.IdGrupoUsuario == gpo.Id && s.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor))
                                {
                                    ddlAgente.Enabled = true;
                                }
                                else
                                {
                                    ddlAgente.SelectedValue = usr.Id.ToString();
                                    ddlAgente.Enabled = false;
                                }
                                break;
                            case false:
                                if (usr.UsuarioGrupo.Any(s => s.IdGrupoUsuario == gpo.Id && s.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.PrimererNivel))
                                {
                                    ddlAgente.Enabled = true;
                                }
                                else
                                {
                                    ddlAgente.SelectedValue = usr.Id.ToString();
                                    ddlAgente.Enabled = false;
                                }
                                break;
                        }
                    }
                }




                ObtenerTicketsPage();

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
                if (ddlAgente.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    ObtenerTicketsPage();
                    return;
                }
                ObtenerTicketsPage();
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

        protected void btnAutoasignar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (gvTickets.SelectedItems.Count <= 0)
                    throw new Exception("Seleccione un ticket");
                foreach (GridDataItem item in gvTickets.SelectedItems)
                {
                    int idTicket = int.Parse(item.GetDataKeyValue("NumeroTicket").ToString());
                    int gpoAsignado = int.Parse(item["IdGrupoAsignado"].Text);
                    int nivelasignacion = int.Parse(item["IdNivelAsignado"].Text);
                    int estatusTicket = int.Parse(item["IdEstatusTicket"].Text);
                    bool propietario = bool.Parse(item["EsPropietario"].Text);

                    if (_servicioEstatus.HasComentarioObligatorio(((Usuario)Session["UserData"]).Id, gpoAsignado, nivelasignacion, estatusTicket, (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado, propietario))
                        if (txtComentarioAsignacion.Text.Trim() == string.Empty)
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalComentarioObligado\");", true);
                            return;
                        }
                    _servicioAtencionTicket.AutoAsignarTicket(idTicket, ((Usuario)Session["UserData"]).Id, txtComentarioAsignacion.Text.Trim());
                }

                //AgenteMaster master = Master as AgenteMaster;
                //if (master != null && idTicket != 0)
                //{
                //    master.AddTicketOpen(idTicket, titulo);
                //}

                ObtenerTicketsPage();

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
        protected void btnCerrarComentarios_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtComentarioAsignacion.Text.Trim()))
                    throw new Exception("Debe ingresar un comentario.");
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalComentarioObligado\");", true);
                btnAutoasignar_OnClick(null, null);
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
                if (gvTickets.SelectedItems.Count <= 0)
                    throw new Exception("Seleccione un ticket");
                foreach (GridDataItem item in gvTickets.SelectedItems)
                {
                    int idTicket = int.Parse(item.GetDataKeyValue("NumeroTicket").ToString());
                    var propietario = item["EsPropietario"].Text;
                    var gpoAsignado = item["IdGrupoAsignado"].Text;
                    var estatusasignacion = item["IdEstatusAsignacion"].Text;
                    var nivelasignacion = item["IdNivelAsignado"].Text;
                    ucCambiarEstatusAsignacion.IdNivelEstatusAsignacionActual = int.Parse(nivelasignacion);
                    ucCambiarEstatusAsignacion.IdEstatusAsignacionActual = Convert.ToInt32(estatusasignacion);
                    ucCambiarEstatusAsignacion.EsPropietario = Convert.ToBoolean(propietario);
                    ucCambiarEstatusAsignacion.IdTicket = Convert.ToInt32(idTicket);
                    ucCambiarEstatusAsignacion.IdGrupo = Convert.ToInt32(gpoAsignado);
                    ucCambiarEstatusAsignacion.IdUsuario = ((Usuario)Session["UserData"]).Id;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAsignacionCambio\");", true);
                }

                ObtenerTicketsPage();

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

        protected void btnEscalar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (gvTickets.SelectedItems.Count <= 0)
                    throw new Exception("Seleccione un ticket");
                foreach (GridDataItem item in gvTickets.SelectedItems)
                {
                    int idTicket = int.Parse(item.GetDataKeyValue("NumeroTicket").ToString());
                    var propietario = item["EsPropietario"].Text;
                    var gpoAsignado = item["IdGrupoAsignado"].Text;
                    var estatusasignacion = item["IdEstatusAsignacion"].Text;

                    ucCambiarEstatusAsignacion.IdEstatusAsignacionActual = Convert.ToInt32(estatusasignacion);
                    ucCambiarEstatusAsignacion.EsPropietario = Convert.ToBoolean(propietario);
                    ucCambiarEstatusAsignacion.IdTicket = Convert.ToInt32(idTicket);
                    ucCambiarEstatusAsignacion.IdGrupo = Convert.ToInt32(gpoAsignado);
                    ucCambiarEstatusAsignacion.IdUsuario = ((Usuario)Session["UserData"]).Id;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAsignacionCambio\");", true);
                    break;
                }

                ObtenerTicketsPage();
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
                if (gvTickets.SelectedItems.Count <= 0)
                    throw new Exception("Seleccione un ticket");
                foreach (GridDataItem item in gvTickets.SelectedItems)
                {
                    var gpoAsignado = item["IdGrupoAsignado"].Text;

                    int idTicket = int.Parse(item.GetDataKeyValue("NumeroTicket").ToString());
                    int estatusTicket = int.Parse(item["IdEstatusTicket"].Text);
                    int idNivelAsignado = int.Parse(item["IdNivelAsignado"].Text) + 2;
                    hfTicketActivo.Value = idTicket.ToString();
                    UcCambiarEstatusTicket.EsPropietario = true;
                    UcCambiarEstatusTicket.IdTicket = idTicket;
                    UcCambiarEstatusTicket.IdEstatusActual = estatusTicket;
                    UcCambiarEstatusTicket.IdSubRolActual = idNivelAsignado;
                    UcCambiarEstatusTicket.IdGrupo = Convert.ToInt32(gpoAsignado);
                    UcCambiarEstatusTicket.IdUsuario = ((Usuario)Session["UserData"]).Id;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalEstatusCambio\");", true);
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

        protected void btnFiltro_OnClick(object sender, EventArgs e)
        {
            try
            {
                RadButton btn = (RadButton)sender;
                if (btn != null)
                {
                    switch (btn.CommandArgument)
                    {
                        case "Abierto":
                            SinAsignar = false;
                            FueraSla = false;
                            Resueltos = false;
                            RecienActualizados = false;
                            EstatusSeleccionado = EstatusAbierto;
                            FiltroTodos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroAbiertos.CssClass = "row borderbootom padding-10-bottom btn-seleccione";
                            FiltroEspera.CssClass = "row borderbootom padding-10-bottom";
                            FiltroSinAsignar.CssClass = "row borderbootom padding-10-bottom";
                            FiltroResueltos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroFueraSLA.CssClass = "row borderbootom padding-10-bottom";
                            FiltroRecienActualizados.CssClass = "row borderbootom padding-10-bottom";
                            break;
                        case "Espera":
                            SinAsignar = false;
                            FueraSla = false;
                            Resueltos = false;
                            RecienActualizados = false;
                            EstatusSeleccionado = EstatusEspera;
                            FiltroTodos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroAbiertos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroEspera.CssClass = "row borderbootom padding-10-bottom btn-seleccione";
                            FiltroSinAsignar.CssClass = "row borderbootom padding-10-bottom";
                            FiltroResueltos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroFueraSLA.CssClass = "row borderbootom padding-10-bottom";
                            FiltroRecienActualizados.CssClass = "row borderbootom padding-10-bottom";
                            break;
                        case "SinAsignar":
                            SinAsignar = true;
                            FueraSla = false;
                            Resueltos = false;
                            RecienActualizados = false;
                            EstatusSeleccionado = EstatusSinAsignar;
                            FiltroTodos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroAbiertos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroEspera.CssClass = "row borderbootom padding-10-bottom";
                            FiltroSinAsignar.CssClass = "row borderbootom padding-10-bottom btn-seleccione";
                            FiltroResueltos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroFueraSLA.CssClass = "row borderbootom padding-10-bottom";
                            FiltroRecienActualizados.CssClass = "row borderbootom padding-10-bottom";
                            break;
                        case "Resuelto":
                            SinAsignar = false;
                            FueraSla = false;
                            Resueltos = true;
                            RecienActualizados = false;
                            EstatusSeleccionado = EstatusResuletos;
                            FiltroTodos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroAbiertos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroEspera.CssClass = "row borderbootom padding-10-bottom";
                            FiltroSinAsignar.CssClass = "row borderbootom padding-10-bottom";
                            FiltroResueltos.CssClass = "row borderbootom padding-10-bottom btn-seleccione";
                            FiltroFueraSLA.CssClass = "row borderbootom padding-10-bottom";
                            FiltroRecienActualizados.CssClass = "row borderbootom padding-10-bottom";
                            break;
                        case "FueraSla":
                            SinAsignar = false;
                            FueraSla = true;
                            Resueltos = false;
                            RecienActualizados = false;
                            EstatusSeleccionado = EstatusFueraSla;
                            FiltroTodos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroAbiertos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroEspera.CssClass = "row borderbootom padding-10-bottom";
                            FiltroSinAsignar.CssClass = "row borderbootom padding-10-bottom";
                            FiltroResueltos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroFueraSLA.CssClass = "row borderbootom padding-10-bottom btn-seleccione";
                            FiltroRecienActualizados.CssClass = "row borderbootom padding-10-bottom";
                            break;
                        case "recienActualizados":
                            SinAsignar = false;
                            FueraSla = false;
                            Resueltos = false;
                            RecienActualizados = true;
                            EstatusSeleccionado = EstatusAbierto;
                            FiltroTodos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroAbiertos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroEspera.CssClass = "row borderbootom padding-10-bottom";
                            FiltroSinAsignar.CssClass = "row borderbootom padding-10-bottom";
                            FiltroResueltos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroFueraSLA.CssClass = "row borderbootom padding-10-bottom";
                            FiltroRecienActualizados.CssClass = "row borderbootom padding-10-bottom  btn-seleccione";
                            break;
                        default:
                            SinAsignar = false;
                            FueraSla = false;
                            Resueltos = false;
                            RecienActualizados = false;
                            EstatusSeleccionado = EstatusAbierto;
                            FiltroTodos.CssClass = "row borderbootom padding-10-bottom  btn-seleccione";
                            FiltroAbiertos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroEspera.CssClass = "row borderbootom padding-10-bottom";
                            FiltroSinAsignar.CssClass = "row borderbootom padding-10-bottom";
                            FiltroResueltos.CssClass = "row borderbootom padding-10-bottom";
                            FiltroFueraSLA.CssClass = "row borderbootom padding-10-bottom";
                            FiltroRecienActualizados.CssClass = "row borderbootom padding-10-bottom";
                            break;
                    }
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

        protected void gvTickets_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem row = (GridDataItem)e.Item;
                int idTicket = int.Parse(row.GetDataKeyValue("NumeroTicket").ToString());
                string titulo = row["Tipificacion"].Text;
                bool asigna = bool.Parse(row["puedeasignar"].Text);
                bool propietaro = bool.Parse(row["EsPropietario"].Text);
                switch (e.CommandName)
                {
                    case "RowClick":
                        bool seleccionado = e.Item.Selected;
                        int itemIndex = e.Item.ItemIndex;


                        if ((seleccionado == false) && (hfFilaSeleccionada.Value.ToString() != itemIndex.ToString()))
                        {
                            AgenteMaster master = Master as AgenteMaster;
                            if (master != null)
                            {
                                master.AddTicketOpen(idTicket, titulo, asigna);
                            }
                        }
                        else
                        {
                            btnAutoasignar.Enabled = asigna;
                            btnAsignar.Enabled = asigna;
                            btnCambiarEstatus.Enabled = propietaro;
                        }
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

        protected void gvTickets_OnItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridFilteringItem)
                {
                    GridFilteringItem filteringItem = e.Item as GridFilteringItem;
                    TextBox box = filteringItem["TipoUsuario"].Controls[0] as TextBox;
                    if (box != null)
                        box.Width = Unit.Pixel(25);
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

        protected void RadComboBox1_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                string filterValue = ((RadComboBox)sender).SelectedValue;
                gvTickets.EnableLinqExpressions = false;
                gvTickets.MasterTableView.FilterExpression = "([ImagenPrioridad] LIKE '%" + filterValue + "%') ";
                GridColumn column = gvTickets.MasterTableView.GetColumnSafe("ImagenPrioridad");
                column.CurrentFilterFunction = GridKnownFunction.Contains;
                gvTickets.MasterTableView.Rebind();
                gvTickets.EnableLinqExpressions = true;
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
                Alerta = _lstError;
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
                Alerta = _lstError;
            }
        }

        protected void btnTipificacion_OnClick(object sender, EventArgs e)
        {
            try
            {
                AgenteMaster master = Master as AgenteMaster;
                if (master != null)
                {
                    master.CambiaTicket = true;
                    master.AddTicketOpen(Convert.ToInt32(((LinkButton)sender).CommandArgument), ((LinkButton)sender).Text, bool.Parse(((LinkButton)sender).CommandName));
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

        protected void gvTickets_OnFilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
        {
            try
            {
                IGridDataColumn gridDataColumn = e.Column as IGridDataColumn;
                if (gridDataColumn != null)
                {
                    string dataField = gridDataColumn.GetActiveDataField();
                    switch (dataField)
                    {
                        case "DentroSla":
                            e.ListBox.DataSource = Tickets.Select(s => s.DentroSla).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "ImagenPrioridad":
                            e.ListBox.DataSource = Tickets.Select(s => s.ImagenPrioridad).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "UsuarioSolicito.TipoUsuario.Descripcion":
                            e.ListBox.DataSource = Tickets.Select(s => s.UsuarioSolicito.TipoUsuario.Descripcion).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "NumeroTicket":
                            e.ListBox.DataSource = Tickets.Select(s => s.NumeroTicket).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "Canal":
                            e.ListBox.DataSource = Tickets.Select(s => s.Canal).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "UsuarioSolicito.NombreCompleto":
                            e.ListBox.DataSource = Tickets.Select(s => s.UsuarioSolicito.NombreCompleto).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "Tipificacion":
                            e.ListBox.DataSource = Tickets.Select(s => s.Tipificacion).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "TipoTicketAbreviacion":
                            e.ListBox.DataSource = Tickets.Select(s => s.TipoTicketAbreviacion).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "FechaHora":
                            e.ListBox.DataSource = Tickets.Select(s => s.FechaHora).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "EstatusTicket.Descripcion":
                            e.ListBox.DataSource = Tickets.Select(s => s.EstatusTicket.Descripcion).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "UsuarioAsignado":
                            e.ListBox.DataSource = Tickets.Select(s => s.UsuarioAsignado).Distinct().ToList();
                            e.ListBox.DataBind();
                            break;
                        case "GrupoAsignado":
                            e.ListBox.DataSource = Tickets.Select(s => s.GrupoAsignado).Distinct().ToList();
                            e.ListBox.DataBind();
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

        protected void chkSelecciona_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkSel = (CheckBox)gvTickets.FindControl("chkSelecciona");
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

        protected void gvTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (gvTickets.SelectedItems.Count > 0)
                {
                    hfFilaSeleccionada.Value = gvTickets.SelectedItems[0].ItemIndex.ToString();
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

        //protected void gvTickets_SelectedCellChanged(object sender, EventArgs e)
        //{
        //    int selected = gvTickets.SelectedCells[0].Column.OrderIndex;
        //    string text = gvTickets.SelectedCells[0].Text;

        //}
    }
}