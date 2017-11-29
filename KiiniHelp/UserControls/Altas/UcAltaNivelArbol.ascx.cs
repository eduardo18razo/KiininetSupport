using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceEncuesta;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceSistemaTipoInformacionConsulta;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaNivelArbol : UserControl, IControllerModal
    {
        #region Variables
        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceTipoInfConsultaClient _servicioSistemaTipoInformacionConsulta = new ServiceTipoInfConsultaClient();
        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private readonly ServiceEncuestaClient _servicioEncuesta = new ServiceEncuestaClient();
        private readonly ServiceInformacionConsultaClient _servicioInformacionConsulta = new ServiceInformacionConsultaClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();
        private readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
        private List<string> _lstError = new List<string>();
        #endregion Variables

        #region Propiedades privadas
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
                panelAlertaInfoConsulta.Visible = value.Any();
                if (!panelAlertaInfoConsulta.Visible) return;
                rptErrorInfoConsulta.DataSource = value;
                rptErrorInfoConsulta.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "UpScroll();", true);
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
        #endregion Propiedades privadas

        #region Propiedades publicas
        public string Titulo
        {
            set { lblTitleCatalogo.Text = value; }
        }
        public int IdTipoUsuario
        {
            get { return int.Parse(ddlTipoUsuarioNivel.SelectedValue); }
            set
            {
                ddlTipoUsuarioNivel.SelectedValue = value.ToString();
                AsociarGrupoUsuario.IdTipoUsuario = value;
            }
        }
        public int IdArea
        {
            get { return int.Parse(hfIdArea.Value); }
            set
            {
                hfIdArea.Value = value.ToString();
            }
        }
        public int IdTipoArbol
        {
            get { return int.Parse(hfIdTipoArbol.Value); }
            set
            {
                hfIdTipoArbol.Value = value.ToString();
                switch (value)
                {
                    case (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion:
                        btnModalConsultas.Visible = true;
                        btnModalTicket.Visible = false;
                        break;
                    default:
                        btnModalConsultas.Visible = false;
                        btnModalTicket.Visible = true;
                        break;
                }
            }
        }
        public string Catalogo
        {
            get { return hfCatalogo.Value; }
            set { hfCatalogo.Value = value; }
        }
        public int IdNivel1
        {
            get { return int.Parse(hfNivel1.Value); }
            set { hfNivel1.Value = value.ToString(); }
        }
        public int IdNivel2
        {
            get { return int.Parse(hfNivel2.Value); }
            set { hfNivel2.Value = value.ToString(); }
        }
        public int IdNivel3
        {
            get { return int.Parse(hfNivel3.Value); }
            set { hfNivel3.Value = value.ToString(); }
        }
        public int IdNivel4
        {
            get { return int.Parse(hfNivel4.Value); }
            set { hfNivel4.Value = value.ToString(); }
        }
        public int IdNivel5
        {
            get { return int.Parse(hfNivel5.Value); }
            set { hfNivel5.Value = value.ToString(); }
        }
        public int IdNivel6
        {
            get { return int.Parse(hfNivel6.Value); }
            set { hfNivel6.Value = value.ToString(); }
        }
        public int IdNivel7
        {
            get { return int.Parse(hfNivel7.Value); }
            set { hfNivel7.Value = value.ToString(); }
        }
        public bool NivelTerminal
        {
            get { return chkNivelTerminal.Checked; }
            set
            {
                chkNivelTerminal.Checked = value;
                if (value)
                    chkNivelTerminal_OnCheckedChanged(chkNivelTerminal, null);
                divDatos.Visible = value;
            }
        }


        #endregion Propiedades publicas

        #region Metodos
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
                    sb.AppendLine("<li>Debe especificar un Formulario de Cliente.</li>");

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
                if (ddlEncuesta.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    sb.AppendLine("<li>Debe especificar una encuesta.</li>");
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

        private void LlenaCombos()
        {
            try
            {
                ddlTipoNivel.Items.Add(new ListItem(BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString()));
                ddlTipoNivel.Items.Add(new ListItem("SubMenú", ((int)BusinessVariables.EnumTipoNivel.SubMenu).ToString()));
                ddlTipoNivel.Items.Add(new ListItem("Opcion terminal", ((int)BusinessVariables.EnumTipoNivel.OpcionTerminal).ToString()));
                ddlTipoNivel.DataBind();
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuario(true);
                Metodos.LlenaComboCatalogo(ddlTipoUsuarioNivel, lstTipoUsuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
        #endregion Metodos

        #region Delegados
        private void UcAltaInformacionConsulta_OnAceptarModal()
        {
            try
            {
                foreach (RepeaterItem item in rptInformacion.Items)
                {
                    DropDownList ddl;

                    ddl = (DropDownList)item.FindControl("ddlPropietario");
                    if (ddl != null)
                    {
                        Metodos.LlenaComboCatalogo(ddl, _servicioInformacionConsulta.ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido, true));
                    }
                    ddl = (DropDownList)item.FindControl("ddlDocumento");
                    if (ddl != null)
                    {
                        Metodos.LlenaComboCatalogo(ddl, _servicioInformacionConsulta.ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta.DocumentoOffice, true));
                    }
                    ddl = (DropDownList)item.FindControl("ddlUrl");
                    if (ddl != null)
                    {
                        Metodos.LlenaComboCatalogo(ddl, _servicioInformacionConsulta.ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta.DireccionWeb, true));

                    }
                }
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
        private void UcAltaInformacionConsulta_OnCancelarModal()
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

        private void UcAltaMascaraAcceso_OnAceptarModal()
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
        private void UcAltaMascaraAcceso_OnCancelarModal()
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

        private void UcSla_OnCancelarModal()
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
        private void UcSla_OnAceptarModal()
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

        private void AltaTiempoEstimadoOnOnCancelarModal()
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
        private void AltaTiempoEstimadoOnOnAceptarModal()
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

        private void UcEncuesta_OnAceptarModal()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlEncuesta, _servicioEncuesta.ObtenerEncuestas(true));
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaEncuesta\");", true);
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
        private void UcEncuesta_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaEncuesta\");", true);
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

        private void UcImpactoUrgencia_OnAceptarModal()
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
        private void UcImpactoUrgenciaOnOnCancelarModal()
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
        #endregion Delegados

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaNivel = new List<string>();
                ucAltaInformacionConsulta.EsAlta = true;
                ucAltaInformacionConsulta.OnAceptarModal += UcAltaInformacionConsulta_OnAceptarModal;
                ucAltaInformacionConsulta.OnCancelarModal += UcAltaInformacionConsulta_OnCancelarModal;

                ucAltaFormulario.OnAceptarModal += UcAltaMascaraAcceso_OnAceptarModal;
                ucAltaFormulario.OnCancelarModal += UcAltaMascaraAcceso_OnCancelarModal;

                AsociarGrupoUsuario.AsignacionAutomatica = true;

                UcSla.FromModal = false;
                UcSla.OnAceptarModal += UcSla_OnAceptarModal;
                UcSla.OnCancelarModal += UcSla_OnCancelarModal;

                ucAltaTiempoEstimado.OnAceptarModal += AltaTiempoEstimadoOnOnAceptarModal;
                ucAltaTiempoEstimado.OnCancelarModal += AltaTiempoEstimadoOnOnCancelarModal;

                ucAltaEncuesta.OnAceptarModal += UcEncuesta_OnAceptarModal;
                ucAltaEncuesta.OnCancelarModal += UcEncuesta_OnCancelarModal;

                UcImpactoUrgencia.OnAceptarModal += UcImpactoUrgencia_OnAceptarModal;
                UcImpactoUrgencia.OnCancelarModal += UcImpactoUrgenciaOnOnCancelarModal;
                if (!IsPostBack)
                {
                    LlenaCombos();
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
        protected void ddlTipoNivel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoNivel.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    divDatos.Visible = false;
                    return;
                }
                NivelTerminal = int.Parse(ddlTipoNivel.SelectedValue) != (int)BusinessVariables.EnumTipoNivel.SubMenu;
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

                btnModalConsultas.Visible = IdTipoArbol == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                btnModalTicket.Visible = chkNivelTerminal.Checked && IdTipoArbol != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                btnModalSla.Visible = chkNivelTerminal.Checked && IdTipoArbol != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                btnModalImpactoUrgencia.Visible = chkNivelTerminal.Checked && IdTipoArbol != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                btnModalInforme.Visible = chkNivelTerminal.Checked && IdTipoArbol != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion;
                Metodos.LlenaComboCatalogo(ddlEncuesta, _servicioEncuesta.ObtenerEncuestas(true));
                if (IdTipoArbol == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                {
                    AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.Usuario, true);
                    AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeCategoría, true);
                    AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeContenido, true);
                    AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ConsultasEspeciales, true);
                    //Información de Consulta
                    List<InformacionConsulta> infoCons = _servicioSistemaTipoInformacionConsulta.ObtenerTipoInformacionConsulta(false).Select(tipoInf => new InformacionConsulta { TipoInfConsulta = tipoInf }).ToList();
                    rptInformacion.DataSource = infoCons;
                    rptInformacion.DataBind();
                    AsociarGrupoUsuario.Limpiar();
                    UcSla.LimpiarCampos();
                    return;
                }
                UcImpactoUrgencia.Limpiar();
                ucAltaTiempoEstimado.LimpiarCampos();
                //Grupos
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.Usuario, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeContenido, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeOperación, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.Agente, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ConsultasEspeciales, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeCategoría, true);
                AsociarGrupoUsuario.HabilitaGrupos((int)BusinessVariables.EnumRoles.AgenteUniversal, true);
                AsociarGrupoUsuario.Limpiar();

                //Ticket
                Metodos.LlenaComboCatalogo(ddlMascaraAcceso, _servicioMascaras.ObtenerMascarasAcceso(true));
                

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
        protected void btnGuardarNivel_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoNivel.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    throw new Exception("Seleccione tipo de nivel");
                }
                if (Metodos.ValidaCapturaCatalogo(txtDescripcionNivel.Text))
                {
                    AsociarGrupoUsuario.ValidaCapturaGrupos();
                    ArbolAcceso arbol = new ArbolAcceso
                    {
                        IdArea = IdArea,
                        IdTipoUsuario = IdTipoUsuario,
                        IdTipoArbolAcceso = IdTipoArbol,
                        EsTerminal = chkNivelTerminal.Checked,
                        Habilitado = chkNivelHabilitado.Checked,
                        Sistema = false,
                        IdUsuarioAlta = ((Usuario)Session["UserData"]).Id
                    };
                    if (chkNivelTerminal.Checked)
                    {
                        if (IdTipoArbol == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        {
                            ValidaCapturaConsulta();
                            ValidaCapturaEncuesta();
                        }
                        if (IdTipoArbol != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        {
                            ValidaCapturaTicket();
                            ValidaCapturaEncuesta();
                            arbol.IdImpacto = UcImpactoUrgencia.ObtenerImpactoUrgencia().Id;
                        }
                        ValidaCapturaGrupos();
                        arbol.InventarioArbolAcceso = new List<InventarioArbolAcceso> { new InventarioArbolAcceso() };
                        if (IdTipoArbol != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        {
                            arbol.InventarioArbolAcceso.First().IdMascara = Convert.ToInt32(ddlMascaraAcceso.SelectedValue) == 0 ? (int?)null : Convert.ToInt32(ddlMascaraAcceso.SelectedValue);
                            arbol.InventarioArbolAcceso.First().Sla = UcSla.Sla;
                        }
                        arbol.InventarioArbolAcceso.First().IdEncuesta = Convert.ToInt32(ddlEncuesta.SelectedValue) == BusinessVariables.ComboBoxCatalogo.ValueSeleccione ? (int?)null : Convert.ToInt32(ddlEncuesta.SelectedValue);
                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol = new List<GrupoUsuarioInventarioArbol>();
                        arbol.TiempoInformeArbol = new List<TiempoInformeArbol>();
                        foreach (RepeaterItem item in AsociarGrupoUsuario.GruposAsociados)
                        {
                            Label lblIdGrupoUsuario = (Label)item.FindControl("lblIdGrupoUsuario");
                            Label lblIdRol = (Label)item.FindControl("lblIdTipoSubGrupo");
                            Label lblIdSubGrupoUsuario = (Label)item.FindControl("lblIdSubGrupo");
                            if (lblIdGrupoUsuario != null && lblIdRol != null && lblIdSubGrupoUsuario != null)
                            {
                                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                                {
                                    IdGrupoUsuario = Convert.ToInt32(lblIdGrupoUsuario.Text),
                                    IdRol = Convert.ToInt32(lblIdRol.Text),
                                    IdSubGrupoUsuario = lblIdSubGrupoUsuario.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(lblIdSubGrupoUsuario.Text)
                                });
                                var gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(Convert.ToInt32(lblIdGrupoUsuario.Text));
                                if (btnModalInforme.Visible)
                                    switch (gpo.IdTipoGrupo)
                                    {
                                        //TODO: AGREGAR GRUPO DE DUEÑO
                                        case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría:
                                            if (ucAltaTiempoEstimado.TieneTiempoDueño)
                                                arbol.TiempoInformeArbol.Add(new TiempoInformeArbol
                                                {
                                                    IdTipoGrupo = gpo.IdTipoGrupo,
                                                    IdGrupoUsuario = gpo.Id,
                                                    Dias = ucAltaTiempoEstimado.TiempoDueño.Dias,
                                                    Horas = ucAltaTiempoEstimado.TiempoDueño.Horas,
                                                    Minutos = ucAltaTiempoEstimado.TiempoDueño.Minutos,
                                                    Segundos = ucAltaTiempoEstimado.TiempoDueño.Segundos,
                                                    TiempoNotificacion = ucAltaTiempoEstimado.TiempoDueño.TiempoNotificacion,
                                                    IdTipoNotificacion = ucAltaTiempoEstimado.TiempoDueño.IdTipoNotificacion

                                                });
                                            break;
                                        case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido:
                                            if (ucAltaTiempoEstimado.TieneTiempoDueño)
                                                arbol.TiempoInformeArbol.Add(new TiempoInformeArbol
                                                {
                                                    IdTipoGrupo = gpo.IdTipoGrupo,
                                                    IdGrupoUsuario = gpo.Id,
                                                    Dias = ucAltaTiempoEstimado.TiempoDueño.Dias,
                                                    Horas = ucAltaTiempoEstimado.TiempoDueño.Horas,
                                                    Minutos = ucAltaTiempoEstimado.TiempoDueño.Minutos,
                                                    Segundos = ucAltaTiempoEstimado.TiempoDueño.Segundos,
                                                    TiempoNotificacion = ucAltaTiempoEstimado.TiempoDueño.TiempoNotificacion,
                                                    IdTipoNotificacion = ucAltaTiempoEstimado.TiempoDueño.IdTipoNotificacion

                                                });
                                            break;
                                        case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo:
                                            if (ucAltaTiempoEstimado.TieneTiempoDesarrollo)
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
                                            if (ucAltaTiempoEstimado.TieneTiempoConsulta)
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
                                    arbol.InventarioArbolAcceso.First().InventarioInfConsulta.Add(new InventarioInfConsulta { IdInfConsulta = Convert.ToInt32(ddl.SelectedValue) });
                            }
                        }
                    }

                    switch (int.Parse(Catalogo))
                    {
                        case 1:
                            arbol.Nivel1 = new Nivel1
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionNivel.Text.Trim(),
                                Habilitado = chkNivelHabilitado.Checked
                            };
                            _servicioArbolAcceso.GuardarArbol(arbol);
                            break;
                        case 2:
                            arbol.IdNivel1 = IdNivel1;
                            arbol.Nivel2 = new Nivel2
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionNivel.Text.Trim(),
                                Habilitado = chkNivelHabilitado.Checked
                            };
                            _servicioArbolAcceso.GuardarArbol(arbol);
                            break;
                        case 3:
                            arbol.IdNivel1 = IdNivel1;
                            arbol.IdNivel2 = IdNivel2;
                            arbol.Nivel3 = new Nivel3
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionNivel.Text.Trim(),
                                Habilitado = chkNivelHabilitado.Checked
                            };
                            _servicioArbolAcceso.GuardarArbol(arbol);
                            break;
                        case 4:
                            arbol.IdNivel1 = IdNivel1;
                            arbol.IdNivel2 = IdNivel2;
                            arbol.IdNivel3 = IdNivel3;
                            arbol.Nivel4 = new Nivel4
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionNivel.Text.Trim(),
                                Habilitado = chkNivelHabilitado.Checked
                            };
                            _servicioArbolAcceso.GuardarArbol(arbol);
                            break;
                        case 5:
                            arbol.IdNivel1 = IdNivel1;
                            arbol.IdNivel2 = IdNivel2;
                            arbol.IdNivel3 = IdNivel3;
                            arbol.IdNivel4 = IdNivel4;
                            arbol.Nivel5 = new Nivel5
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionNivel.Text.Trim(),
                                Habilitado = chkNivelHabilitado.Checked
                            };
                            _servicioArbolAcceso.GuardarArbol(arbol);
                            break;
                        case 6:
                            arbol.IdNivel1 = IdNivel1;
                            arbol.IdNivel2 = IdNivel2;
                            arbol.IdNivel3 = IdNivel3;
                            arbol.IdNivel4 = IdNivel4;
                            arbol.IdNivel5 = IdNivel5;
                            arbol.Nivel6 = new Nivel6
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionNivel.Text.Trim(),
                                Habilitado = chkNivelHabilitado.Checked
                            };
                            _servicioArbolAcceso.GuardarArbol(arbol);
                            break;
                        case 7:
                            arbol.IdNivel1 = IdNivel1;
                            arbol.IdNivel2 = IdNivel2;
                            arbol.IdNivel3 = IdNivel3;
                            arbol.IdNivel4 = IdNivel4;
                            arbol.IdNivel5 = IdNivel5;
                            arbol.IdNivel6 = IdNivel6;
                            arbol.Nivel7 = new Nivel7
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionNivel.Text.Trim(),
                                Habilitado = chkNivelHabilitado.Checked
                            };
                            _servicioArbolAcceso.GuardarArbol(arbol);
                            break;
                    }
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
        protected void btnModalConsultas_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalGrupos.CssClass = "btn btn-primary disabled";
                btnModalConsultas.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalConsultas\");", true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        protected void btnModalTicket_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalTicket.CssClass = "btn btn-primary";
                btnModalGrupos.CssClass = "btn btn-primary disabled";
                btnModalSla.CssClass = "btn btn-primary disabled";
                btnModalImpactoUrgencia.CssClass = "btn btn-primary disabled";
                btnModalInforme.CssClass = "btn btn-primary disabled";
                btnModalEncuesta.CssClass = "btn btn-primary disabled";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalTicket\");", true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        protected void btnModalGrupos_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalGrupos.CssClass = "btn btn-primary";
                btnModalSla.CssClass = "btn btn-primary disabled";
                btnModalImpactoUrgencia.CssClass = "btn btn-primary disabled";
                btnModalInforme.CssClass = "btn btn-primary disabled";
                btnModalEncuesta.CssClass = "btn btn-primary disabled";
                AsociarGrupoUsuario.AsignacionAutomatica = true;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalGruposNodo\");", true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        protected void btnModalSla_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalSla.CssClass = "btn btn-primary";
                btnModalImpactoUrgencia.CssClass = "btn btn-primary disabled";
                btnModalInforme.CssClass = "btn btn-primary disabled";
                btnModalEncuesta.CssClass = "btn btn-primary disabled";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalSla\");", true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        protected void btnModalInforme_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalInforme.CssClass = "btn btn-primary";
                btnModalEncuesta.CssClass = "btn btn-primary disabled";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalTiempoInforme\");", true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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

                throw new Exception(ex.Message);
            }
        }
        protected void btnModalImpactoUrgencia_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnModalImpactoUrgencia.CssClass = "btn btn-primary";
                btnModalInforme.CssClass = "btn btn-primary disabled";
                btnModalEncuesta.CssClass = "btn btn-primary disabled";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalImpacto\");", true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        #region Cerrar Modales

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
                if (IdTipoArbol == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
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
                if (idGrupo != 0)
                    UcSla.IdGrupo = idGrupo;
                if (IdTipoArbol == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                {
                    btnModalEncuesta.CssClass = "btn btn-primary";
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
                ValidaCapturaEncuesta();
                btnModalEncuesta.CssClass = "btn btn-success";
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
        #endregion Eventos

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
    }
}