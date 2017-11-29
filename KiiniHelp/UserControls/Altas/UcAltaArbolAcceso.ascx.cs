using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using KinniNet.Business.Utils;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceEncuesta;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceSistemaTipoInformacionConsulta;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaArbolAcceso : UserControl, IControllerModal
    {
        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
        private readonly ServiceInformacionConsultaClient _servicioInformacionConsulta = new ServiceInformacionConsultaClient();
        private readonly ServiceTipoInfConsultaClient _servicioSistemaTipoInformacionConsulta = new ServiceTipoInfConsultaClient();
        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private readonly ServiceEncuestaClient _servicioEncuesta = new ServiceEncuestaClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();
        private List<string> _lstError = new List<string>();

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        public int IdArbol
        {
            set
            {
                PoblaDatos(value);
                hfIdArbol.Value = value.ToString();
            }
        }

        private List<string> AlertaNivel
        {
            set
            {
                panelAlertaNivel.Visible = value.Any();
                if (!panelAlertaNivel.Visible) return;
                rptErrorNivel.DataSource = value;
                rptErrorNivel.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "UpScroll();", true);
            }
        }

        private List<string> AlertaInfoConsulta
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

        private List<string> AlertaTicket
        {
            set
            {
                panelAlertaTicket.Visible = value.Any();
                if (!panelAlertaTicket.Visible) return;
                rptErrorTicket.DataSource = value;
                rptErrorTicket.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "UpScroll();", true);
            }
        }

        private void ValidaCapturaConsulta()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if ((from RepeaterItem item in rptInformacion.Items select (CheckBox)item.FindControl("chkInfoConsulta")).Count(chk => chk.Checked) <= 0)
                {
                    sb.AppendLine("<li>Debe especificar al menos un tipo de información de consulta.</li>");
                }
                DropDownList ddl = null;
                foreach (RepeaterItem item in rptInformacion.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkInfoConsulta");
                    if (chk.Checked)
                    {
                        Label lbl = (Label)item.FindControl("lblIdTipoInformacion");
                        switch (int.Parse(lbl.Text))
                        {
                            case (int)BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido:
                                ddl = (DropDownList)item.FindControl("ddlPropietario");
                                break;
                            case (int)BusinessVariables.EnumTiposInformacionConsulta.DocumentoOffice:
                                ddl = (DropDownList)item.FindControl("ddlDocumento");
                                break;
                            case (int)BusinessVariables.EnumTiposInformacionConsulta.DireccionWeb:
                                ddl = (DropDownList)item.FindControl("ddlUrl");
                                break;
                        }
                        if (ddl != null && ddl.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            sb.AppendLine("<li>Seleccioine alguna información de consulta.</li>");
                    }

                }
                if (sb.ToString() != string.Empty)
                {
                    sb.Append("</ul>");
                    sb.Insert(0, "<ul>");
                    sb.Insert(0, "<h3>Consulta</h3>");
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidaCapturaTicket()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (ddlMascaraAcceso.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    sb.AppendLine("<li>Debe especificar Formulario de Cliente.</li>");

                if (sb.ToString() != string.Empty)
                {
                    sb.Append("</ul>");
                    sb.Insert(0, "<ul>");
                    sb.Insert(0, "<h3>Ticket</h3>");
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidaCapturaEncuesta()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //todo: eliminar
                //if (ddlEncuesta.SelectedIndex == BusinessVariables.ComboBoxCatalogo.Index)
                //    sb.AppendLine("<li>Debe especificar una encuesta.</li>");
                if (sb.ToString() != string.Empty)
                {
                    sb.Append("</ul>");
                    sb.Insert(0, "<ul>");
                    sb.Insert(0, "<h3>Ticket</h3>");
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void ValidaCapturaGrupos()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (!AsociarGrupoUsuario.ValidaCapturaGrupos())
                    sb.AppendLine("<li>Debe asignar al menos un Grupo.</li>");
                if (sb.ToString() != string.Empty)
                {
                    sb.Append("</ul>");
                    sb.Insert(0, "<ul>");
                    sb.Insert(0, "<h3>Grupos</h3>");
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void PoblaDatos(int idarbol)
        {
            try
            {
                ArbolAcceso opcion = _servicioArbolAcceso.ObtenerArbolAcceso(idarbol);
                if (opcion != null)
                {
                    btnModalConsultas.CssClass = "btn btn-primary";
                    btnModalTicket.CssClass = "btn btn-primary";
                    btnModalGrupos.CssClass = "btn btn-primary";
                    btnModalSla.CssClass = "btn btn-primary";
                    btnModalImpactoUrgencia.CssClass = "btn btn-primary";
                    btnModalInforme.CssClass = "btn btn-primary";
                    btnModalEncuesta.CssClass = "btn btn-primary";
                    AsociarGrupoUsuario.AsignacionAutomatica = true;
                    ucAltaInformacionConsulta.EsAlta = true;
                    List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuario(true);
                    Metodos.LlenaComboCatalogo(ddlTipoUsuarioNivel, lstTipoUsuario);
                    ddlTipoUsuarioNivel.SelectedValue = opcion.IdTipoUsuario.ToString();
                    AsociarGrupoUsuario.IdTipoUsuario = opcion.IdTipoUsuario;
                    txtDescripcionNivel.Text = ObtenerDescripcion(opcion);
                    hfIdTipoArbol.Value = opcion.IdTipoArbolAcceso.ToString();
                    divDatos.Visible = opcion.EsTerminal;
                    chkNivelTerminal.Checked = opcion.EsTerminal;
                    chkNivelTerminal_OnCheckedChanged(chkNivelTerminal, null);
                    DropDownList ddl;
                    Button btn;
                    CheckBox chk = null;
                    foreach (InventarioInfConsulta invInfo in opcion.InventarioArbolAcceso.First().InventarioInfConsulta)
                    {
                        BusinessVariables.EnumTiposInformacionConsulta seleccion = Metodos.Enumeradores.GetValueEnumFromString<BusinessVariables.EnumTiposInformacionConsulta>(invInfo.InformacionConsulta.TipoInfConsulta.Descripcion);
                        switch (seleccion)
                        {
                            case BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido:
                                chk = (CheckBox)rptInformacion.Items[0].FindControl("chkInfoConsulta");
                                ddl = (DropDownList)rptInformacion.Items[0].FindControl("ddlPropietario");
                                btn = (Button)rptInformacion.Items[0].FindControl("btnAgregarPropietario");
                                break;
                            case BusinessVariables.EnumTiposInformacionConsulta.DocumentoOffice:
                                chk = (CheckBox)rptInformacion.Items[1].FindControl("chkInfoConsulta");
                                ddl = (DropDownList)rptInformacion.Items[1].FindControl("ddlDocumento");
                                btn = (Button)rptInformacion.Items[1].FindControl("btnAgregarDocumento");
                                break;
                            case BusinessVariables.EnumTiposInformacionConsulta.DireccionWeb:
                                chk = (CheckBox)rptInformacion.Items[2].FindControl("chkInfoConsulta");
                                ddl = (DropDownList)rptInformacion.Items[2].FindControl("ddlUrl");
                                btn = (Button)rptInformacion.Items[2].FindControl("btnAgregarUrl");
                                break;
                            default:
                                ddl = null;
                                btn = null;
                                break;
                        }
                        if (chk != null) chk.Checked = true;
                        if (ddl != null)
                        {
                            ddl.Enabled = true;
                            ddl.SelectedValue = invInfo.InformacionConsulta.Id.ToString();
                        }
                        if (btn != null) btn.Enabled = true;
                    }

                    if (opcion.IdTipoArbolAcceso != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                    {
                        Metodos.LlenaComboCatalogo(ddlMascaraAcceso, _servicioMascaras.ObtenerMascarasAcceso(true));
                        ddlMascaraAcceso.SelectedValue = opcion.InventarioArbolAcceso.First().IdMascara.ToString();
                        Metodos.LlenaComboCatalogo(ddlEncuesta, _servicioEncuesta.ObtenerEncuestas(true));
                        ddlEncuesta.SelectedValue = opcion.InventarioArbolAcceso.First().IdEncuesta.ToString();
                        UcAltaSla.IdGrupo = opcion.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).GroupBy(g => g.IdGrupoUsuario).Single().Key;
                        int? idSla = opcion.InventarioArbolAcceso.First().IdSla;
                        if (idSla != null) UcAltaSla.IdSla = (int)idSla;
                        ucAltaTiempoEstimado.SetTiempoEstimado(opcion.TiempoInformeArbol);
                    }

                    AsociarGrupoUsuario.GruposAsignados = opcion.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol;

                    UcImpactoUrgencia.SetImpactoUrgencia(opcion.IdImpacto);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string ObtenerDescripcion(ArbolAcceso arbol)
        {
            string result;
            try
            {
                result = arbol.Nivel1.Descripcion;
                if (arbol.Nivel2 != null)
                    result = arbol.Nivel2.Descripcion;
                if (arbol.Nivel3 != null)
                    result = arbol.Nivel3.Descripcion;
                if (arbol.Nivel4 != null)
                    result = arbol.Nivel4.Descripcion;
                if (arbol.Nivel5 != null)
                    result = arbol.Nivel5.Descripcion;
                if (arbol.Nivel6 != null)
                    result = arbol.Nivel6.Descripcion;
                if (arbol.Nivel7 != null)
                    result = arbol.Nivel7.Descripcion;
                if (arbol.Nivel2 != null)
                    result = arbol.Nivel1.Descripcion;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error al obtener opcion a editar \n{0}", e.Message));
            }
            return result;
        }

        private void LimpiarNivel()
        {
            try
            {
                txtDescripcionNivel.Text = string.Empty;
                chkNivelTerminal.Checked = false;
                chkNivelTerminal_OnCheckedChanged(chkNivelTerminal, null);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AsociarGrupoUsuario.AsignacionAutomatica = true;
                ucAltaInformacionConsulta.EsAlta = true;
                UcAltaSla.FromModal = true;

                ucAltaInformacionConsulta.OnAceptarModal += UcAltaInformacionConsulta_OnAceptarModal;
                ucAltaInformacionConsulta.OnCancelarModal += UcAltaInformacionConsulta_OnCancelarModal;

                ucAltaFormulario.OnAceptarModal += UcAltaMascaraAcceso_OnAceptarModal;
                ucAltaFormulario.OnCancelarModal += UcAltaMascaraAcceso_OnCancelarModal;

                UcAltaSla.OnAceptarModal += UcSla_OnAceptarModal;
                UcAltaSla.OnCancelarModal += UcSla_OnCancelarModal;

                UcImpactoUrgencia.OnAceptarModal += UcImpactoUrgencia_OnAceptarModal;
                UcImpactoUrgencia.OnCancelarModal += UcImpactoUrgenciaOnOnCancelarModal;

                ucAltaTiempoEstimado.OnAceptarModal += AltaTiempoEstimadoOnOnAceptarModal;
                ucAltaTiempoEstimado.OnCancelarModal += AltaTiempoEstimadoOnOnCancelarModal;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void UcSla_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalSla\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void UcSla_OnAceptarModal()
        {
            try
            {
                btnModalSla.CssClass = "btn btn-success";
                btnModalImpactoUrgencia.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalSla\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void AltaTiempoEstimadoOnOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalTiempoInforme\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void AltaTiempoEstimadoOnOnAceptarModal()
        {
            try
            {
                btnModalInforme.CssClass = "btn btn-success";
                btnModalEncuesta.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalTiempoInforme\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void UcAltaMascaraAcceso_OnAceptarModal()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlMascaraAcceso, _servicioMascaras.ObtenerMascarasAcceso(true));
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaMascara\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void UcAltaMascaraAcceso_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaMascara\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void UcImpactoUrgenciaOnOnCancelarModal()
        {
            try
            {
                btnModalImpactoUrgencia.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalImpacto\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void UcAltaInformacionConsulta_OnAceptarModal()
        {
            try
            {
                rptInformacion.DataSource = null;
                rptInformacion.DataBind();
                List<InformacionConsulta> infoCons = _servicioSistemaTipoInformacionConsulta.ObtenerTipoInformacionConsulta(false).Select(tipoInf => new InformacionConsulta { TipoInfConsulta = tipoInf }).ToList();
                rptInformacion.DataSource = infoCons;
                rptInformacion.DataBind();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaInfCons\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void UcAltaInformacionConsulta_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaInfCons\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        void UcImpactoUrgencia_OnAceptarModal()
        {
            try
            {
                btnModalImpactoUrgencia.CssClass = "btn btn-success";
                btnModalInforme.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalImpacto\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnModalConsultas_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalConsultas.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalConsultas\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnModalTicket_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalTicket.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalTicket\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnModalGrupos_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalGrupos.CssClass = "btn btn-primary";
                AsociarGrupoUsuario.AsignacionAutomatica = true;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalGruposNodo\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnModalSla_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalSla.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalSla\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnModalInforme_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalInforme.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalTiempoInforme\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnModalEncuesta_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalEncuesta.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalEncuesta\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnModalImpactoUrgencia_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalImpactoUrgencia.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalImpacto\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnGuardarNivel_OnClick(object sender, EventArgs e)
        {
            try
            {

                if (Metodos.ValidaCapturaCatalogo(txtDescripcionNivel.Text))
                {
                    AsociarGrupoUsuario.ValidaCapturaGrupos();
                    ArbolAcceso arbol = new ArbolAcceso
                    {
                        EsTerminal = chkNivelTerminal.Checked,
                        Habilitado = chkNivelHabilitado.Checked
                    };
                    if (chkNivelTerminal.Checked)
                    {
                        arbol.IdImpacto = UcImpactoUrgencia.ObtenerImpactoUrgencia().Id;
                        if (int.Parse(hfIdTipoArbol.Value) == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                            ValidaCapturaConsulta();
                        if (int.Parse(hfIdTipoArbol.Value) != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        {
                            ValidaCapturaTicket();
                            ValidaCapturaEncuesta();
                        }
                        ValidaCapturaGrupos();
                        arbol.InventarioArbolAcceso = new List<InventarioArbolAcceso> { new InventarioArbolAcceso() };
                        if (hfIdTipoArbol.Value != ((int)BusinessVariables.EnumTipoArbol.ConsultarInformacion).ToString())
                        {
                            arbol.InventarioArbolAcceso.First().IdMascara =
                                Convert.ToInt32(ddlMascaraAcceso.SelectedValue) == 0
                                    ? (int?)null
                                    : Convert.ToInt32(ddlMascaraAcceso.SelectedValue);
                            arbol.InventarioArbolAcceso.First().Sla = UcAltaSla.Sla;
                            arbol.InventarioArbolAcceso.First().IdEncuesta =
                                Convert.ToInt32(ddlEncuesta.SelectedValue) == BusinessVariables.ComboBoxCatalogo.ValueSeleccione
                                    ? (int?)null
                                    : Convert.ToInt32(ddlEncuesta.SelectedValue);
                        }
                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol =
                            new List<GrupoUsuarioInventarioArbol>();
                        arbol.TiempoInformeArbol = new List<TiempoInformeArbol>();
                        foreach (RepeaterItem item in AsociarGrupoUsuario.GruposAsociados)
                        {
                            Label lblIdGrupoUsuario = (Label)item.FindControl("lblIdGrupoUsuario");
                            Label lblIdRol = (Label)item.FindControl("lblIdTipoSubGrupo");
                            Label lblIdSubGrupoUsuario = (Label)item.FindControl("lblIdSubGrupo");
                            if (lblIdGrupoUsuario != null && lblIdRol != null && lblIdSubGrupoUsuario != null)
                            {
                                arbol.InventarioArbolAcceso.First()
                                    .GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                                    {
                                        IdGrupoUsuario = Convert.ToInt32(lblIdGrupoUsuario.Text),
                                        IdRol = Convert.ToInt32(lblIdRol.Text),
                                        IdSubGrupoUsuario =
                                            lblIdSubGrupoUsuario.Text.Trim() == string.Empty
                                                ? (int?)null
                                                : Convert.ToInt32(lblIdSubGrupoUsuario.Text)
                                    });
                                var gpo =
                                    _servicioGrupoUsuario.ObtenerGrupoUsuarioById(Convert.ToInt32(lblIdGrupoUsuario.Text));
                                switch (gpo.IdTipoGrupo)
                                {
                                    //TODO: AGREGAR GRUPO DE DUEÑO
                                    //case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeMantenimiento:
                                    //    arbol.TiempoInformeArbol.Add(new TiempoInformeArbol
                                    //    {
                                    //        IdTipoGrupo = gpo.IdTipoGrupo,
                                    //        IdGrupoUsuario = gpo.Id,
                                    //        Dias = AltaTiempoEstimado.TiempoDueño.Dias,
                                    //        Horas = AltaTiempoEstimado.TiempoDueño.Horas,
                                    //        Minutos = AltaTiempoEstimado.TiempoDueño.Minutos,
                                    //        Segundos = AltaTiempoEstimado.TiempoDueño.Segundos,
                                    //        TiempoNotificacion = AltaTiempoEstimado.TiempoDueño.TiempoNotificacion,
                                    //        IdTipoNotificacion = AltaTiempoEstimado.TiempoDueño.IdTipoNotificacion

                                    //    });
                                    //    break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido:
                                        arbol.TiempoInformeArbol.Add(new TiempoInformeArbol
                                        {
                                            IdTipoGrupo = gpo.IdTipoGrupo,
                                            IdGrupoUsuario = gpo.Id,
                                            Dias = ucAltaTiempoEstimado.TiempoMantenimiento.Dias,
                                            Horas = ucAltaTiempoEstimado.TiempoMantenimiento.Horas,
                                            Minutos = ucAltaTiempoEstimado.TiempoMantenimiento.Minutos,
                                            Segundos = ucAltaTiempoEstimado.TiempoMantenimiento.Segundos,
                                            TiempoNotificacion = ucAltaTiempoEstimado.TiempoMantenimiento.TiempoNotificacion,
                                            IdTipoNotificacion = ucAltaTiempoEstimado.TiempoMantenimiento.IdTipoNotificacion

                                        });
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo:
                                        arbol.TiempoInformeArbol.Add(new TiempoInformeArbol
                                        {
                                            IdTipoGrupo = gpo.IdTipoGrupo,
                                            IdGrupoUsuario = gpo.Id,
                                            Dias = ucAltaTiempoEstimado.TiempoDesarrollo.Dias,
                                            Horas = ucAltaTiempoEstimado.TiempoDesarrollo.Horas,
                                            Minutos = ucAltaTiempoEstimado.TiempoDesarrollo.Minutos,
                                            Segundos = ucAltaTiempoEstimado.TiempoDesarrollo.Segundos,
                                            TiempoNotificacion = ucAltaTiempoEstimado.TiempoDesarrollo.TiempoNotificacion,
                                            IdTipoNotificacion = ucAltaTiempoEstimado.TiempoDesarrollo.IdTipoNotificacion

                                        });
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales:
                                        arbol.TiempoInformeArbol.Add(new TiempoInformeArbol
                                        {
                                            IdTipoGrupo = gpo.IdTipoGrupo,
                                            IdGrupoUsuario = gpo.Id,
                                            Dias = ucAltaTiempoEstimado.TiempoConsulta.Dias,
                                            Horas = ucAltaTiempoEstimado.TiempoConsulta.Horas,
                                            Minutos = ucAltaTiempoEstimado.TiempoConsulta.Minutos,
                                            Segundos = ucAltaTiempoEstimado.TiempoConsulta.Segundos,
                                            TiempoNotificacion = ucAltaTiempoEstimado.TiempoConsulta.TiempoNotificacion,
                                            IdTipoNotificacion = ucAltaTiempoEstimado.TiempoConsulta.IdTipoNotificacion

                                        });
                                        break;

                                }
                            }
                        }
                        arbol.InventarioArbolAcceso.First().Descripcion = txtDescripcionNivel.Text.Trim();
                        arbol.InventarioArbolAcceso.First().InventarioInfConsulta = new List<InventarioInfConsulta>();
                        foreach (RepeaterItem item in rptInformacion.Items)
                        {
                            if (((CheckBox)item.FindControl("chkInfoConsulta")).Checked)
                            {
                                Label lblId = (Label)item.FindControl("lblIdTipoInformacion");
                                DropDownList ddl = null;
                                switch (Convert.ToInt32(lblId.Text))
                                {
                                    case (int)BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido:
                                        ddl = (DropDownList)item.FindControl("ddlPropietario");
                                        break;
                                    case (int)BusinessVariables.EnumTiposInformacionConsulta.DocumentoOffice:
                                        ddl = (DropDownList)item.FindControl("ddlDocumento");
                                        break;
                                    case (int)BusinessVariables.EnumTiposInformacionConsulta.DireccionWeb:
                                        ddl = (DropDownList)item.FindControl("ddlUrl");
                                        break;
                                }
                                if (ddl != null)
                                    arbol.InventarioArbolAcceso.First().InventarioInfConsulta.Add(
                                        new InventarioInfConsulta
                                        {
                                            IdInfConsulta = Convert.ToInt32(ddl.SelectedValue),

                                        });
                            }
                        }
                    }

                    _servicioArbolAcceso.ActualizardArbol(Convert.ToInt32(hfIdArbol.Value), arbol, txtDescripcionNivel.Text);
                    LimpiarNivel();
                    if (OnAceptarModal != null)
                        OnAceptarModal();
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }

        }

        protected void btnCancelarNivel_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarNivel();
                if (OnCancelarModal != null)
                    OnCancelarModal();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnLimpiarNivel_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarNivel();
                if (OnLimpiarModal != null)
                    OnLimpiarModal();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void chkInfoConsulta_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl;
                Button btn;
                CheckBox chk = (CheckBox)sender;
                if (chk == null) return;
                BusinessVariables.EnumTiposInformacionConsulta seleccion = Metodos.Enumeradores.GetStringEnum<BusinessVariables.EnumTiposInformacionConsulta>(chk.Text);
                switch (seleccion)
                {
                    case BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido:
                        ddl = (DropDownList)rptInformacion.Items[0].FindControl("ddlPropietario");
                        btn = (Button)rptInformacion.Items[0].FindControl("btnAgregarPropietario");
                        break;
                    case BusinessVariables.EnumTiposInformacionConsulta.DocumentoOffice:
                        ddl = (DropDownList)rptInformacion.Items[1].FindControl("ddlDocumento");
                        btn = (Button)rptInformacion.Items[1].FindControl("btnAgregarDocumento");
                        break;
                    case BusinessVariables.EnumTiposInformacionConsulta.DireccionWeb:
                        ddl = (DropDownList)rptInformacion.Items[2].FindControl("ddlUrl");
                        btn = (Button)rptInformacion.Items[2].FindControl("btnAgregarUrl");
                        break;
                    default:
                        ddl = null;
                        btn = null;
                        break;
                }

                if (ddl == null) return;
                if (!chk.Checked)
                    ddl.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddl.Enabled = chk.Checked;
                btn.Enabled = chk.Checked;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void rptInformacion_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                DropDownList ddlPropietario = (DropDownList)e.Item.FindControl("ddlPropietario");
                DropDownList ddlDocumento = (DropDownList)e.Item.FindControl("ddlDocumento");
                DropDownList ddlUrl = (DropDownList)e.Item.FindControl("ddlUrl");
                if (ddlPropietario == null && ddlDocumento == null && ddlUrl == null) return;
                Metodos.LlenaComboCatalogo(ddlPropietario, _servicioInformacionConsulta.ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido, true));
                Metodos.LlenaComboCatalogo(ddlDocumento, _servicioInformacionConsulta.ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta.DocumentoOffice, true));
                Metodos.LlenaComboCatalogo(ddlUrl, _servicioInformacionConsulta.ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta.DireccionWeb, true));
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void chkNivelTerminal_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {

                btnModalConsultas.Visible = Convert.ToInt32(hfIdTipoArbol.Value) == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                btnModalTicket.Visible = chkNivelTerminal.Checked && Convert.ToInt32(hfIdTipoArbol.Value) != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                btnModalSla.Visible = chkNivelTerminal.Checked && Convert.ToInt32(hfIdTipoArbol.Value) != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                btnModalInforme.Visible = chkNivelTerminal.Checked && Convert.ToInt32(hfIdTipoArbol.Value) != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                btnModalEncuesta.Visible = chkNivelTerminal.Checked && Convert.ToInt32(hfIdTipoArbol.Value) != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.Usuario, false);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeContenido, false);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeOperación, false);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo, false);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.Agente, false);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ConsultasEspeciales, false);
                if (hfIdTipoArbol.Value == ((int)BusinessVariables.EnumTipoArbol.ConsultarInformacion).ToString())
                {
                    AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.Usuario, true);
                    //Información de Consulta
                    List<InformacionConsulta> infoCons = _servicioSistemaTipoInformacionConsulta.ObtenerTipoInformacionConsulta(false).Select(tipoInf => new InformacionConsulta { TipoInfConsulta = tipoInf }).ToList();
                    rptInformacion.DataSource = infoCons;
                    rptInformacion.DataBind();
                    AsociarGrupoUsuario.Limpiar();
                    return;
                }

                //Grupos
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.Usuario, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeContenido, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeOperación, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.Agente, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ConsultasEspeciales, true);
                AsociarGrupoUsuario.Limpiar();

                //Ticket
                Metodos.LlenaComboCatalogo(ddlMascaraAcceso, _servicioMascaras.ObtenerMascarasAcceso(true));
                Metodos.LlenaComboCatalogo(ddlEncuesta, _servicioEncuesta.ObtenerEncuestas(true));

                upGrupos.Update();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }

        }


        #region Cerrar Modales

        protected void btnCerrarConsultas_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaConsulta();
                btnModalGrupos.CssClass = "btn btn-primary";
                btnModalConsultas.CssClass = "btn btn-success";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalConsultas\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaInfoConsulta = _lstError;
            }
        }

        protected void btnCerrarModalAltaGrupoUsuario_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!AsociarGrupoUsuario.ValidaCapturaGrupos()) return;
                btnModalGrupos.CssClass = "btn btn-success btn-lg";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaGrupoUsuario\");", true);
                upGrupos.Update();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnCerrarTicket_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaTicket();
                btnModalTicket.CssClass = "btn btn-success";
                btnModalGrupos.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalTicket\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaTicket = _lstError;
            }
        }

        protected void btnCerraGrupos_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaGrupos();
                btnModalGrupos.CssClass = "btn btn-success";
                if (Convert.ToInt32(hfIdTipoArbol.Value) == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                    btnModalImpactoUrgencia.CssClass = "btn btn-primary";
                else
                    btnModalSla.CssClass = "btn btn-primary";
                int idGrupo = 0;
                foreach (RepeaterItem item in AsociarGrupoUsuario.GruposAsociados)
                {
                    Label lblIdGrupoUsuario = (Label)item.FindControl("lblIdGrupoUsuario");
                    Label lblIdRol = (Label)item.FindControl("lblIdTipoSubGrupo");
                    if (Convert.ToInt32(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.Agente)
                        idGrupo = Convert.ToInt32(lblIdGrupoUsuario.Text);
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalGruposNodo\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaNivel = _lstError;
            }
        }

        protected void btnCerrarEncuesta_OnClick(object o, EventArgs e)
        {
            try
            {
                ValidaCapturaTicket();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalEncuesta\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaTicket = _lstError;
            }
        }

        #endregion Cerrar Modales

        #endregion Eventos#endregion Eventos


    }
}