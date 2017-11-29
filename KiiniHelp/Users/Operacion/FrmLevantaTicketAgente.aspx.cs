using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceSistemaCanal;
using KiiniHelp.ServiceSistemaTipoArbolAcceso;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;

namespace KiiniHelp.Users.Operacion
{
    public partial class FrmLevantaTicketAgente : Page
    {
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
        private readonly ServiceTipoArbolAccesoClient _servicioSistemaTipoArbol = new ServiceTipoArbolAccesoClient();
        private readonly ServiceAreaClient _servicioAreas = new ServiceAreaClient();
        private readonly ServiceCanalClient _servicioCanal = new ServiceCanalClient();

        private List<string> _lstError = new List<string>();
        private List<string> AlertaGeneral
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
                AlertaGeneral = new List<string>();
                ParametrosGenerales generales = (ParametrosGenerales)Session["ParametrosGenerales"];
                if (!IsPostBack)
                {
                    if (!generales.LevantaTickets)
                    {
                        Response.Redirect("~/Users/DashBoard.aspx");
                    }
                    if (generales.LevantaTickets && (!((Usuario)Session["UserData"]).LevantaTickets && !((Usuario)Session["UserData"]).LevantaRecado))
                    {
                        Response.Redirect("~/Users/DashBoard.aspx");
                    }
                }

                if (generales.ValidaUsuario)
                {
                    ucMensajeValidacion.Mensaje = generales.MensajeValidacion;
                    ucMensajeValidacion.OnCancelarModal += ucMensajeValidacion_OnCancelarModal;
                }
                ucAltaPreticket.OnAceptarModal += UcAltaPreticketOnOnAceptarModal;
                ucAltaPreticket.OnCancelarModal += UcAltaPreticketOnOnCancelarModal;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private void UcAltaPreticketOnOnCancelarModal() 
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalPreTicket\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private void UcAltaPreticketOnOnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalPreTicket\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        void ucMensajeValidacion_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalValidaUsuario\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<Usuario> usuarios = _servicioUsuarios.BuscarUsuarios(txtUserName.Text.Trim());
                rbtnLstUsuarios.DataSource = usuarios;
                rbtnLstUsuarios.DataTextField = "NombreCompleto";
                rbtnLstUsuarios.DataValueField = "Id";
                rbtnLstUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void rbtnLstUsuarios_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlArea.DataSource = _servicioAreas.ObtenerAreasUsuario(int.Parse(rbtnLstUsuarios.SelectedValue), true);
                ddlArea.DataTextField = "Descripcion";
                ddlArea.DataValueField = "Id";
                ddlArea.DataBind();

                ddlCanal.DataSource = _servicioCanal.ObtenerCanales(true);
                ddlCanal.DataTextField = "Descripcion";
                ddlCanal.DataValueField = "Id";
                ddlCanal.DataBind();
                Metodos.LimpiarCombo(ddlTipoArbol);
                Metodos.LimpiarCombo(ddlNivel1);
                Metodos.LimpiarCombo(ddlNivel2);
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                divArbol.Visible = true;
                if (((ParametrosGenerales)Session["ParametrosGenerales"]).ValidaUsuario)
                {
                    ucMensajeValidacion.IdUsuarioValidar = int.Parse(rbtnLstUsuarios.SelectedValue);
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalValidaUsuario\");", true);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void ddlArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlTipoArbol);
                Metodos.LimpiarCombo(ddlNivel1);
                Metodos.LimpiarCombo(ddlNivel2);
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                if (ddlArea.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {

                    btnLevantar.Visible = false;
                    return;
                }
                int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                Metodos.LlenaComboCatalogo(ddlTipoArbol, _servicioSistemaTipoArbol.ObtenerTiposArbolAccesoByGruposTercero(int.Parse(rbtnLstUsuarios.SelectedValue), ((Usuario)Session["UserData"]).Id, idArea, true));

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
        protected void ddlTipoArbol_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlNivel1);
                Metodos.LimpiarCombo(ddlNivel2);
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                if (ddlTipoArbol.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    btnLevantar.Visible = false;
                    return;
                }
                int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                int idTipoArbolAcceso = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                Metodos.LlenaComboCatalogo(ddlNivel1, _servicioArbolAcceso.ObtenerNivel1ByGrupos(int.Parse(rbtnLstUsuarios.SelectedValue), ((Usuario)Session["UserData"]).Id, idArea, idTipoArbolAcceso, true));
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
        protected void ddlNivel1_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnLevantar.Visible = false;
                btnNotificacion.Visible = false;
                Metodos.LimpiarCombo(ddlNivel2);
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                if (ddlNivel1.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    return;
                }
                int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                int idTipoArbolAcceso = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                int idNivelFiltro = Convert.ToInt32(ddlNivel1.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminalByGrupos(idArea, idTipoUsuario, idTipoArbolAcceso, idNivelFiltro,null, null, null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel1, ddlNivel2,_servicioArbolAcceso.ObtenerNivel2ByGrupos(int.Parse(rbtnLstUsuarios.SelectedValue),((Usuario) Session["UserData"]).Id, idArea, idTipoArbolAcceso, idNivelFiltro, true));
                }
                else
                {
                    btnLevantar.Visible = _servicioArbolAcceso.LevantaTicket(((Usuario) Session["UserData"]).Id, idArea,
                        idTipoUsuario, idTipoArbolAcceso, idNivelFiltro, null, null, null, null, null, null);
                    if (!btnLevantar.Visible)
                    {
                        btnNotificacion.Visible =
                            _servicioArbolAcceso.RecadoTicketTicket(((Usuario) Session["UserData"]).Id, idArea,
                                _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue))
                                    .IdTipoUsuario, idTipoArbolAcceso, idNivelFiltro, null, null, null, null, null, null);
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
                AlertaGeneral = _lstError;
            }
        }
        protected void ddlNivel2_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                btnLevantar.Visible = false;
                btnNotificacion.Visible = false;
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                if (ddlNivel2.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    return;
                }
                int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                int idTipoArbolAcceso = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                int idNivelFiltro = Convert.ToInt32(ddlNivel2.SelectedValue);

                if (!_servicioArbolAcceso.EsNodoTerminalByGrupos(idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), idNivelFiltro, null, null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel2, ddlNivel3, _servicioArbolAcceso.ObtenerNivel3ByGrupos(int.Parse(rbtnLstUsuarios.SelectedValue), ((Usuario)Session["UserData"]).Id, idArea, idTipoArbolAcceso, idNivelFiltro, true));
                }
                else
                {
                    btnLevantar.Visible = _servicioArbolAcceso.LevantaTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), idNivelFiltro, null, null, null, null, null);
                    if (!btnLevantar.Visible)
                    {
                        btnNotificacion.Visible = _servicioArbolAcceso.RecadoTicketTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), idNivelFiltro, null, null, null, null, null);
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
                AlertaGeneral = _lstError;
            }
        }
        protected void ddlNivel3_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnLevantar.Visible = false;
                btnNotificacion.Visible = false;
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                if (ddlNivel3.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    return;
                }
                int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                int idTipoArbolAcceso = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                int idNivelFiltro = Convert.ToInt32(ddlNivel3.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminalByGrupos(idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel3, ddlNivel4, _servicioArbolAcceso.ObtenerNivel4ByGrupos(int.Parse(rbtnLstUsuarios.SelectedValue), ((Usuario)Session["UserData"]).Id, idArea, idTipoArbolAcceso, idNivelFiltro, true));
                }
                else
                {
                    btnLevantar.Visible = _servicioArbolAcceso.LevantaTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), null, null, null, null);
                    if (!btnLevantar.Visible)
                    {
                        btnNotificacion.Visible = _servicioArbolAcceso.RecadoTicketTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), null, null, null, null);
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
                AlertaGeneral = _lstError;
            }
        }
        protected void ddlNivel4_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                btnLevantar.Visible = false;
                btnNotificacion.Visible = false;
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                if (ddlNivel4.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    return;
                }
                int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                int idTipoArbolAcceso = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                int idNivelFiltro = Convert.ToInt32(ddlNivel4.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminalByGrupos(idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), idNivelFiltro, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel4, ddlNivel5, _servicioArbolAcceso.ObtenerNivel5ByGrupos(int.Parse(rbtnLstUsuarios.SelectedValue), ((Usuario)Session["UserData"]).Id, idArea, idTipoArbolAcceso, idNivelFiltro, true));
                }
                else
                {
                    btnLevantar.Visible = _servicioArbolAcceso.LevantaTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), idNivelFiltro, null, null, null);
                    if (!btnLevantar.Visible)
                    {
                        btnNotificacion.Visible = _servicioArbolAcceso.RecadoTicketTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), idNivelFiltro, null, null, null);
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
                AlertaGeneral = _lstError;
            }
        }
        protected void ddlNivel5_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnLevantar.Visible = false;
                btnNotificacion.Visible = false;
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                if (ddlNivel5.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    return;
                }
                int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                int idTipoArbolAcceso = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                int idNivelFiltro = Convert.ToInt32(ddlNivel5.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminalByGrupos(idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), idNivelFiltro, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel5, ddlNivel6, _servicioArbolAcceso.ObtenerNivel6ByGrupos(int.Parse(rbtnLstUsuarios.SelectedValue), ((Usuario)Session["UserData"]).Id, idArea, idTipoArbolAcceso, idNivelFiltro, true));
                }
                else
                {
                    btnLevantar.Visible = _servicioArbolAcceso.LevantaTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), idNivelFiltro, null, null);
                    if (!btnLevantar.Visible)
                    {
                        btnNotificacion.Visible = _servicioArbolAcceso.RecadoTicketTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), idNivelFiltro, null, null);
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
                AlertaGeneral = _lstError;
            }
        }
        protected void ddlNivel6_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnLevantar.Visible = false;
                btnNotificacion.Visible = false;
                Metodos.LimpiarCombo(ddlNivel7);
                if (ddlNivel6.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    return;
                }
                int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                int idTipoArbolAcceso = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                int idNivelFiltro = Convert.ToInt32(ddlNivel6.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminalByGrupos(idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), Convert.ToInt32(ddlNivel5.SelectedValue), idNivelFiltro, null))
                {
                    Metodos.FiltraCombo(ddlNivel6, ddlNivel7, _servicioArbolAcceso.ObtenerNivel7ByGrupos(int.Parse(rbtnLstUsuarios.SelectedValue), ((Usuario)Session["UserData"]).Id, idArea, idTipoArbolAcceso, idNivelFiltro, true));
                }
                else
                {
                    btnLevantar.Visible = _servicioArbolAcceso.LevantaTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), Convert.ToInt32(ddlNivel5.SelectedValue), idNivelFiltro, null);
                    if (!btnLevantar.Visible)
                    {
                        btnNotificacion.Visible = _servicioArbolAcceso.RecadoTicketTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), Convert.ToInt32(ddlNivel5.SelectedValue), idNivelFiltro, null);
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
                AlertaGeneral = _lstError;
            }
        }
        protected void ddlNivel7_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnLevantar.Visible = false;
                btnNotificacion.Visible = false;
                if (ddlNivel7.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    return;
                }
                else
                {
                    int idArea = Convert.ToInt32(ddlArea.SelectedValue);
                    int idTipoArbolAcceso = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                    int idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                    int idNivelFiltro = Convert.ToInt32(ddlNivel7.SelectedValue);
                    btnLevantar.Visible = _servicioArbolAcceso.LevantaTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), Convert.ToInt32(ddlNivel5.SelectedValue), Convert.ToInt32(ddlNivel6.SelectedValue), idNivelFiltro);
                    if (!btnLevantar.Visible)
                    {
                        btnNotificacion.Visible = _servicioArbolAcceso.RecadoTicketTicket(((Usuario)Session["UserData"]).Id, idArea, idTipoUsuario, idTipoArbolAcceso, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), Convert.ToInt32(ddlNivel5.SelectedValue), Convert.ToInt32(ddlNivel6.SelectedValue), idNivelFiltro);
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
                AlertaGeneral = _lstError;
            }
        }

        protected void btnLevantar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlCanal.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione un medio de comunicación");
                int? idArea = null;
                int? idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                int? idTipoArbol = null;
                int? nivel1 = null;
                int? nivel2 = null;
                int? nivel3 = null;
                int? nivel4 = null;
                int? nivel5 = null;
                int? nivel6 = null;
                int? nivel7 = null;

                if (ddlArea.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idArea = int.Parse(ddlArea.SelectedValue);

                if (ddlTipoArbol.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idTipoArbol = int.Parse(ddlTipoArbol.SelectedValue);

                if (ddlNivel1.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel1 = int.Parse(ddlNivel1.SelectedValue);

                if (ddlNivel2.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel2 = int.Parse(ddlNivel2.SelectedValue);

                if (ddlNivel3.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel3 = int.Parse(ddlNivel3.SelectedValue);

                if (ddlNivel4.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel4 = int.Parse(ddlNivel4.SelectedValue);

                if (ddlNivel5.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel5 = int.Parse(ddlNivel5.SelectedValue);

                if (ddlNivel6.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel6 = int.Parse(ddlNivel6.SelectedValue);

                if (ddlNivel7.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel7 = int.Parse(ddlNivel7.SelectedValue);

                var y = _servicioArbolAcceso.ObtenerArbolesAccesoTerminalAll(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                string url = ResolveUrl("~/Users/Ticket/FrmTicket.aspx?IdArbol=" + y.First().Id + "&Canal=" + ddlCanal.SelectedValue + "&UsuarioSolicita=" + rbtnLstUsuarios.SelectedValue);
                string s = "window.open('" + url + "', 'popup_window', 'width=' + screen.availWidth + ',height=' + screen.availHeight,screenX=1,screenY=1,left=1,top=1,resizable=no');";
                ScriptManager.RegisterStartupScript(this, GetType(), "script", s, true);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptEncuesta", "OpenWindow(\"" + url + "\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }


        protected void btnNotificacion_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlCanal.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione un medio de comunicación");
                int? idArea = null;
                int? idTipoUsuario = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(rbtnLstUsuarios.SelectedValue)).IdTipoUsuario;
                int? idTipoArbol = null;
                int? nivel1 = null;
                int? nivel2 = null;
                int? nivel3 = null;
                int? nivel4 = null;
                int? nivel5 = null;
                int? nivel6 = null;
                int? nivel7 = null;

                if (ddlArea.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idArea = int.Parse(ddlArea.SelectedValue);

                if (ddlTipoArbol.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idTipoArbol = int.Parse(ddlTipoArbol.SelectedValue);

                if (ddlNivel1.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel1 = int.Parse(ddlNivel1.SelectedValue);

                if (ddlNivel2.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel2 = int.Parse(ddlNivel2.SelectedValue);

                if (ddlNivel3.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel3 = int.Parse(ddlNivel3.SelectedValue);

                if (ddlNivel4.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel4 = int.Parse(ddlNivel4.SelectedValue);

                if (ddlNivel5.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel5 = int.Parse(ddlNivel5.SelectedValue);

                if (ddlNivel6.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel6 = int.Parse(ddlNivel6.SelectedValue);

                if (ddlNivel7.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    nivel7 = int.Parse(ddlNivel7.SelectedValue);

                var y = _servicioArbolAcceso.ObtenerArbolesAccesoTerminalAll(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                ucAltaPreticket.IdArbol = y.First().Id;
                ucAltaPreticket.IdUsuarioLevanto = ((Usuario)Session["UserData"]).Id;
                ucAltaPreticket.IdUsuarioSolicito = int.Parse(rbtnLstUsuarios.SelectedValue);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Scriptss", "MostrarPopup(\"#modalPreTicket\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
    }
}