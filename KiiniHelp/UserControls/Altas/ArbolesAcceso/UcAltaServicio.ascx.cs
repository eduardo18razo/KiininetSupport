using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceEncuesta;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceImpactourgencia;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceNotificacion;
using KiiniHelp.ServiceSistemaTipoArbolAcceso;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceSubGrupoUsuario;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.ArbolesAcceso
{
    public partial class UcAltaServicio : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceTipoArbolAccesoClient _servicioSistemaTipoArbol = new ServiceTipoArbolAccesoClient();
        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();
        private readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
        private readonly ServiceSubGrupoUsuarioClient _servicioSubGrupo = new ServiceSubGrupoUsuarioClient();
        private readonly ServiceImpactoUrgenciaClient _servicioImpactoUrgencia = new ServiceImpactoUrgenciaClient();
        private readonly ServiceNotificacionClient _servicioNotificacion = new ServiceNotificacionClient();
        private readonly ServiceEncuestaClient _servicioEncuesta = new ServiceEncuestaClient();
        private List<string> _lstError = new List<string>();
        private UsuariosMaster _mp;

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

        private int IdTipoUsuario
        {
            get
            {
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione quien puede ver el contenido.");
                return int.Parse(ddlTipoUsuario.SelectedValue);
            }
            set { ddlTipoUsuario.SelectedValue = value.ToString(); }
        }

        private int TipoArbol
        {
            get { return int.Parse(ddlTipificacion.SelectedValue); }
            set { ddlTipificacion.SelectedValue = value.ToString(); }
        }
        public int IdArea
        {
            get
            {
                if (ddlArea.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione una categoria.");
                return int.Parse(ddlArea.SelectedValue);
            }
            set
            {
                ddlArea.SelectedValue = value.ToString();
            }
        }
        public int IdNivel1
        {
            get
            {
                if (ddlNivel1.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione nivel 1.");
                return int.Parse(ddlNivel1.SelectedValue);
            }
            set { ddlNivel1.SelectedValue = value.ToString(); }
        }
        public int IdNivel2
        {
            get
            {
                if (ddlNivel2.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione nivel 2.");
                return int.Parse(ddlNivel2.SelectedValue);
            }
            set { ddlNivel2.SelectedValue = value.ToString(); }
        }
        public int IdNivel3
        {
            get
            {
                if (ddlNivel3.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione nivel 3.");
                return int.Parse(ddlNivel3.SelectedValue);
            }
            set { ddlNivel3.SelectedValue = value.ToString(); }
        }
        public int IdNivel4
        {
            get
            {
                if (ddlNivel4.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione nivel 4.");
                return int.Parse(ddlNivel4.SelectedValue);
            }
            set { ddlNivel4.SelectedValue = value.ToString(); }
        }
        public int IdNivel5
        {
            get
            {
                if (ddlNivel5.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione nivel 5.");
                return int.Parse(ddlNivel5.SelectedValue);
            }
            set { ddlNivel5.SelectedValue = value.ToString(); }
        }
        public int IdNivel6
        {
            get
            {
                if (ddlNivel6.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione nivel 6.");
                return int.Parse(ddlNivel6.SelectedValue);
            }
            set { ddlNivel6.SelectedValue = value.ToString(); }
        }

        public string Catalogo
        {
            get
            {
                string result = "1";
                if (ddlNivel1.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    result = "2";
                if (ddlNivel2.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    result = "3";
                if (ddlNivel3.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    result = "4";
                if (ddlNivel4.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    result = "5";
                if (ddlNivel5.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    result = "6";
                if (ddlNivel6.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    result = "7";
                return result;
            }
        }

        private void LlenaCombos()
        {
            try
            {
                Alerta = new List<string>();
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuario(true);
                Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
                Metodos.LlenaComboCatalogo(ddlTipificacion, _servicioSistemaTipoArbol.ObtenerTiposArbolAcceso(true).Where(w => w.Id != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion));
                Metodos.LlenaComboCatalogo(ddlFormularios, _servicioMascaras.ObtenerMascarasAcceso((int)BusinessVariables.EnumTipoMascara.Ticket, false, true));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Impacto ObtenerImpactoUrgencia()
        {
            Impacto result = null;
            try
            {
                if (ddlPrioridad.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione && ddlUrgencia.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    result = _servicioImpactoUrgencia.ObtenerImpactoByPrioridadUrgencia(Convert.ToInt32(ddlPrioridad.SelectedValue), Convert.ToInt32(ddlUrgencia.SelectedValue));
                divImpacto.Visible = result != null;
                if (result != null)
                {
                    lblImpacto.Style.Remove("color");
                    if (result.Descripcion.ToUpper() == "ALTO")
                    {
                        lblImpacto.Style.Add("color", "red");
                    }
                    //if (result.Descripcion.ToUpper() == "MEDIO")
                    //{
                    //}
                    //if (result.Descripcion.ToUpper() == "BAJO")
                    //{
                    //}


                    lblImpacto.Text = result.Descripcion;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        private void ValidaCapturaPaso1()
        {
            try
            {
                List<string> errors = new List<string>();
                if (txtDescripcionNivel.Text.Trim() == string.Empty)
                    errors.Add("Ingrese titulo para la opción");
                if (txtDescripcionNivel.Text.Trim() == string.Empty)
                    errors.Add("Ingrese descripción para la opción");
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    errors.Add("Seleccione quien puede ver el contenido");
                if (ddlTipificacion.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    errors.Add("Seleccione una Tipificación");
                if (ddlFormularios.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    errors.Add("Seleccione un Formulario");
                _lstError = errors;
                if (!_lstError.Any()) return;
                _lstError = errors;
                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidaCapturaPaso2()
        {
            try
            {
                var valida = IdArea;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidaCapturaPaso3()
        {
            try
            {
                List<string> errors = new List<string>();

                if (ddlGrupoAcceso.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleccione grupo de acceso.");
                }
                if (ddlGrupoResponsableMantenimiento.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleccione grupo Responsable del contenido.");
                }
                if (ddlGruporesponsableOperacion.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleccione grupo de Operación.");
                }
                if (ddlGrupoResponsableDesarrollo.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleccione grupo de Desarrollo.");
                }
                if (ddlGrupoResponsableAtencion.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleccione grupo de Atencion.");
                }
                if (!lstGruposEc.Items.Cast<ListItem>().Any(a => a.Selected))
                {
                    errors.Add("Seleccione al menos un grupo Especial de consulta.");
                }
                if (ddlGrupoResponsableCategoria.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleccione grupo Responsable del Servicio.");
                }
                if (!lstGruposAu.Items.Cast<ListItem>().Any(a => a.Selected))
                {
                    errors.Add("Seleccione al menos un Grupo de Agente Universal");
                }
                _lstError = errors;
                if (!_lstError.Any()) return;
                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidaCapturaPaso4()
        {
            try
            {
                if (txtTiempoTotal.Text == string.Empty && !chkEstimado.Checked)
                    throw new Exception("Debe especificar un tiempo de SLA.");
                if (int.Parse(txtTiempoTotal.Text) <= 0)
                    throw new Exception("El tiempo debe ser mayor a 0.");
                if (ddlTiempoTotal.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Debe especificar unidad de medida de tiempo de SLA.");
                if (chkEstimado.Checked)
                    foreach (RepeaterItem item in rptSubRoles.Items)
                    {
                        var txtDias = (TextBox)item.FindControl("txtDias");
                        DropDownList ddl = (DropDownList)item.FindControl("ddlDuracionRepeater");
                        if (txtDias != null)
                        {
                            if (txtDias.Text.Trim() == string.Empty || int.Parse(txtDias.Text.Trim()) <= 0)
                                throw new Exception("Debe especificar el tiempo para todos los sub roles.");
                            if (ddl.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                                throw new Exception("Debe especificar unidad de medida de tiempo para todos los sub roles.");
                        }
                    }
                if (ddlPrioridad.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione || ddlUrgencia.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Especifique prioridad e Impacto.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidaCapturaPaso5()
        {
            try
            {
                List<string> errors = new List<string>();
                if (chkNotificacionResponsableCategoria.Checked)
                {
                    if (txtTiempoNotificacionResponsableCategoria.Text != string.Empty && int.Parse(txtTiempoNotificacionResponsableCategoria.Text) <= 0)
                        errors.Add("El tiempo de notificacion para el Grupo Dueño de Servicio debe ser mayor a 0.");

                    if (txtTiempoNotificacionResponsableCategoria.Text == string.Empty)
                        errors.Add("Ingrese tiempo de notificacion para el Grupo Dueño de Servicio.");

                    if (ddlNotificacionResponsableCategoria.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Seleccione unidad de medida para el Grupo Dueño de Servicio");

                    if (ddlCanalResponsableCategoria.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Seleccione un canal de notificacion para el Grupo Dueño de Servicio");
                }
                if (chkNotificacionResponsableContenido.Checked)
                {
                    if (txtTiempoNotificacionResponsableContenido.Text != string.Empty && int.Parse(txtTiempoNotificacionResponsableContenido.Text) <= 0)
                        errors.Add("El tiempo de notificacion para el Grupo Responsable de Contenido debe ser mayor a 0.");

                    if (txtTiempoNotificacionResponsableContenido.Text == string.Empty)
                        errors.Add("Ingrese tiempo de notificacion para el Grupo Responsable de Contenido.");

                    if (ddlNotificacionResponsableContenido.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Seleccione unidad de medida para el Grupo Responsable de Contenido");

                    if (ddlCanalResponsableContenido.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Seleccione un canal de notificacion para Grupo Responsable de Contenido.");
                }
                if (chkNotificacionResponsableDesarrollo.Checked)
                {
                    if (txtTiempoNotificacionResponsableDesarrollo.Text != string.Empty && int.Parse(txtTiempoNotificacionResponsableDesarrollo.Text) <= 0)
                        errors.Add("El tiempo de notificacion para el Grupo de Desarrollo debe ser mayor a 0.");

                    if (txtTiempoNotificacionResponsableDesarrollo.Text == string.Empty)
                        errors.Add("Ingrese tiempo de notificacion para el Grupo de Desarrollo.");

                    if (ddlNotificacionResponsableDesarrollo.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Seleccione unidad de medida para el Grupo Dueño de Desarrollo");

                    if (ddlCanalResponsableDesarrollo.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Seleccione un canal de notificacion para el Grupo de Desarrollo.");
                }
                if (chkNotificacionResponsableOperacion.Checked)
                {
                    if (txtTiempoNotificacionResponsableOperacion.Text != string.Empty && int.Parse(txtTiempoNotificacionResponsableOperacion.Text) <= 0)
                        errors.Add("El tiempo de notificacion para el Grupos Especial de Consultas debe ser mayor a 0.");

                    if (txtTiempoNotificacionResponsableOperacion.Text == string.Empty)
                        errors.Add("Ingrese tiempo de notificacion para los Grupos Especial de Consultas.");

                    if (ddlNotificacionResponsableOperacion.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Seleccione unidad de medida para los Grupos Especial de Consultas");

                    if (ddlCanalResponsableOperacion.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Seleccione un canal de notificacion para los Grupos Especial de Consultas.");
                }
                _lstError = errors;
                if (!_lstError.Any()) return;
                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidaCapturaPaso6()
        {
            try
            {
                List<string> errors = new List<string>();
                if (chkEncuestaActiva.Checked)
                    if (ddlEncuesta.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        errors.Add("Debe especificar una encuesta");

                _lstError = errors;
                if (!_lstError.Any()) return;
                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void ValidaCapturaNivel(int value)
        {
            try
            {
                TextBox txt = null;
                switch (value)
                {
                    case 1:
                        txt = txtDescripcionN1;
                        break;
                    case 2:
                        txt = txtDescripcionN2;
                        break;
                    case 3:
                        txt = txtDescripcionN3;
                        break;
                    case 4:
                        txt = txtDescripcionN4;
                        break;
                    case 5:
                        txt = txtDescripcionN5;
                        break;
                    case 6:
                        txt = txtDescripcionN6;
                        break;
                }
                if (txt != null && txt.Text.Trim() == string.Empty)
                    throw new Exception("Debe capturar una descripción");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void LimpiarPantalla()
        {
            try
            {
                //General
                ddlNivel1_OnSelectedIndexChanged(ddlNivel1, null);
                txtDescripcionNivel.Text = string.Empty;
                txtDescripcionOpcion.Text = string.Empty;
                LlenaCombos();
                chkPublico.Checked = false;
                chkNivelHabilitado.Checked = true;
                btnPaso_OnClick(new LinkButton { CommandArgument = "1" }, null);

                //Seccion
                txtDescripcionArea.Text = string.Empty;
                txtDescripcionN1.Text = string.Empty;
                txtDescripcionN2.Text = string.Empty;
                txtDescripcionN3.Text = string.Empty;
                txtDescripcionN4.Text = string.Empty;
                txtDescripcionN5.Text = string.Empty;
                txtDescripcionN6.Text = string.Empty;
                divNivel2.Visible = false;
                divNivel3.Visible = false;
                divNivel4.Visible = false;
                divNivel5.Visible = false;
                divNivel6.Visible = false;
                //txtDescripcionN7.Text = string.Empty;

                //Grupos

                Metodos.LimpiarCombo(ddlGrupoAcceso);
                Metodos.LimpiarCombo(ddlGrupoResponsableMantenimiento);
                Metodos.LimpiarCombo(ddlGruporesponsableOperacion);
                Metodos.LimpiarCombo(ddlGrupoResponsableDesarrollo);
                Metodos.LimpiarCombo(ddlGrupoResponsableAtencion);
                Metodos.LimpiarCombo(ddlGrupoResponsableCategoria);
                Metodos.LimpiarListBox(lstGruposAu);

                //SLA

                txtTiempoTotal.Text = string.Empty;
                ddlTiempoTotal.SelectedIndex = ddlTiempoTotal.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlTiempoTotal.SelectedIndex;
                //chkEstimado.Checked = false;
                //chkEstimado_OnCheckedChanged(chkEstimado, null);
                ddlPrioridad.SelectedIndex = ddlPrioridad.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlPrioridad.SelectedIndex;
                ddlUrgencia.SelectedIndex = ddlUrgencia.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlUrgencia.SelectedIndex;
                ddlUrgencia_OnSelectedIndexChanged(ddlUrgencia, null);

                //Notificaciones
                ddlNotificacionResponsableCategoria.SelectedIndex = ddlNotificacionResponsableCategoria.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlNotificacionResponsableCategoria.SelectedIndex;
                ddlCanalResponsableCategoria.SelectedIndex = ddlCanalResponsableCategoria.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlCanalResponsableCategoria.SelectedIndex;
                txtTiempoNotificacionResponsableCategoria.Text = string.Empty;
                chkNotificacionResponsableCategoria.Checked = false;
                chkNotificacionResponsableCategoria_OnCheckedChanged(chkNotificacionResponsableCategoria, null);
                chkVencimientoResponsableCategoria.Checked = false;

                txtTiempoNotificacionResponsableContenido.Text = string.Empty;
                ddlNotificacionResponsableContenido.SelectedIndex = ddlNotificacionResponsableContenido.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlNotificacionResponsableContenido.SelectedIndex;
                ddlCanalResponsableContenido.SelectedIndex = ddlCanalResponsableContenido.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlCanalResponsableContenido.SelectedIndex;
                chkNotificacionResponsableContenido.Checked = false;
                chkNotificacionResponsableContenido_OnCheckedChanged(chkNotificacionResponsableContenido, null);
                chkVencimientoResponsableContenido.Checked = false;

                txtTiempoNotificacionResponsableDesarrollo.Text = string.Empty;
                ddlNotificacionResponsableDesarrollo.SelectedIndex = ddlNotificacionResponsableDesarrollo.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlNotificacionResponsableDesarrollo.SelectedIndex;
                ddlCanalResponsableDesarrollo.SelectedIndex = ddlCanalResponsableDesarrollo.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlCanalResponsableDesarrollo.SelectedIndex;
                chkNotificacionResponsableDesarrollo.Checked = false;
                chkNotificacionResponsableDesarrollo_OnCheckedChanged(chkNotificacionResponsableDesarrollo, null);
                chkVencimientoResponsableDesarrollo.Checked = false;

                txtTiempoNotificacionResponsableOperacion.Text = String.Empty;
                ddlNotificacionResponsableOperacion.SelectedIndex = ddlNotificacionResponsableOperacion.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlNotificacionResponsableOperacion.SelectedIndex;
                ddlCanalResponsableOperacion.SelectedIndex = ddlCanalResponsableOperacion.Items.Count > 0 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : ddlCanalResponsableOperacion.SelectedIndex;
                chkNotificacionResponsableOperacion.Checked = false;
                chkNotificacionResponsableOperacion_OnCheckedChanged(chkNotificacionResponsableOperacion, null);
                chkVencimientoResponsableOperacion.Checked = false;

                //Encuestas
                chkEncuestaActiva.Checked = false;
                chkEncuestaActiva_OnCheckedChanged(chkEncuestaActiva, null);
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
                _mp = (UsuariosMaster)Page.Master;
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    LlenaCombos();
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        protected void btnPaso_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                switch (int.Parse(btn.CommandArgument))
                {
                    case 1:
                        divStep1.Visible = true;
                        divStep1Data.Visible = true;
                        divStep2.Visible = false;
                        divStep2Data.Visible = false;
                        divStep3.Visible = false;
                        divStep3Data.Visible = false;
                        divStep4.Visible = false;
                        divStep4Data.Visible = false;
                        divStep5.Visible = false;
                        divStep5Data.Visible = false;
                        divStep6.Visible = false;
                        divStep6Data.Visible = false;
                        Metodos.LimpiarCombo(ddlNivel1);
                        Metodos.LimpiarCombo(ddlNivel2);
                        Metodos.LimpiarCombo(ddlNivel3);
                        Metodos.LimpiarCombo(ddlNivel4);
                        Metodos.LimpiarCombo(ddlNivel5);
                        Metodos.LimpiarCombo(ddlNivel6);
                        //Metodos.LimpiarCombo(ddlNivel7);
                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btnSiguiente.CommandArgument = "1";
                        break;
                    case 2:
                        divStep1.Visible = true;
                        divStep1Data.Visible = false;
                        divStep2.Visible = true;
                        divStep2Data.Visible = true;
                        divStep3.Visible = false;
                        divStep3Data.Visible = false;
                        divStep4.Visible = false;
                        divStep4Data.Visible = false;
                        divStep5.Visible = false;
                        divStep5Data.Visible = false;
                        divStep6.Visible = false;
                        divStep6Data.Visible = false;
                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btnSiguiente.CommandArgument = "2";
                        break;
                    case 3:
                        divStep1.Visible = true;
                        divStep1Data.Visible = false;
                        divStep2.Visible = true;
                        divStep2Data.Visible = false;
                        divStep3.Visible = true;
                        divStep3Data.Visible = true;
                        divStep4.Visible = false;
                        divStep4Data.Visible = false;
                        divStep5.Visible = false;
                        divStep5Data.Visible = false;
                        divStep6.Visible = false;
                        divStep6Data.Visible = false;

                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btnSiguiente.CommandArgument = "3";
                        break;
                    case 4:
                        divStep1.Visible = true;
                        divStep1Data.Visible = false;
                        divStep2.Visible = true;
                        divStep2Data.Visible = false;
                        divStep3.Visible = true;
                        divStep3Data.Visible = false;
                        divStep4.Visible = true;
                        divStep4Data.Visible = true;
                        divStep5.Visible = false;
                        divStep5Data.Visible = false;
                        divStep6.Visible = false;
                        divStep6Data.Visible = false;

                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btnSiguiente.CommandArgument = "4";
                        break;
                    case 5:
                        divStep1.Visible = true;
                        divStep1Data.Visible = false;
                        divStep2.Visible = true;
                        divStep2Data.Visible = false;
                        divStep3.Visible = true;
                        divStep3Data.Visible = false;
                        divStep4.Visible = true;
                        divStep4Data.Visible = false;
                        divStep5.Visible = true;
                        divStep5Data.Visible = true;
                        divStep6.Visible = false;
                        divStep6Data.Visible = false;

                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btnSiguiente.CommandArgument = "5";
                        break;

                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        protected void btnSiguiente_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                switch (int.Parse(btn.CommandArgument))
                {
                    case 1:
                        ValidaCapturaPaso1();
                        divStep1Data.Visible = false;
                        divStep2.Visible = true;
                        divStep2Data.Visible = true;
                        Metodos.LimpiarCombo(ddlNivel1);
                        Metodos.LimpiarCombo(ddlNivel2);
                        Metodos.LimpiarCombo(ddlNivel3);
                        Metodos.LimpiarCombo(ddlNivel4);
                        Metodos.LimpiarCombo(ddlNivel5);
                        Metodos.LimpiarCombo(ddlNivel6);
                        //Metodos.LimpiarCombo(ddlNivel7);
                        Metodos.LlenaComboCatalogo(ddlArea, _servicioArea.ObtenerAreas(true).Where(w => !w.Sistema).ToList());
                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btn.CommandArgument = "2";
                        break;
                    case 2:
                        ValidaCapturaPaso2();
                        divStep1Data.Visible = false;
                        divStep2Data.Visible = false;
                        divStep3.Visible = true;
                        divStep3Data.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlGrupoAcceso, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.Usuario, IdTipoUsuario, true));
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableMantenimiento, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ResponsableDeContenido, (int)BusinessVariables.EnumTiposUsuario.Operador, true).Where(s => s.Habilitado));
                        Metodos.LlenaComboCatalogo(ddlGruporesponsableOperacion, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ResponsableDeOperación, (int)BusinessVariables.EnumTiposUsuario.Operador, true).Where(s => s.Habilitado));
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableDesarrollo, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo, (int)BusinessVariables.EnumTiposUsuario.Operador, true).Where(s => s.Habilitado));
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableAtencion, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.Agente, (int)BusinessVariables.EnumTiposUsuario.Operador, true).Where(s => s.Habilitado));
                        Metodos.LlenaListBoxCatalogo(lstGruposEc, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ConsultasEspeciales, (int)BusinessVariables.EnumTiposUsuario.Operador, false).Where(w => !w.Sistema));
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableCategoria, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ResponsableDeCategoría, (int)BusinessVariables.EnumTiposUsuario.Operador, true).Where(s => s.Habilitado));
                        Metodos.LlenaListBoxCatalogo(lstGruposAu, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.AgenteUniversal, (int)BusinessVariables.EnumTiposUsuario.Operador, false).Where(s => s.Habilitado));
                        //imgImpacto.ImageUrl = "";
                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btn.CommandArgument = "3";
                        break;
                    case 3:
                        ValidaCapturaPaso3();
                        divStep1Data.Visible = false;
                        divStep2Data.Visible = false;
                        divStep3Data.Visible = false;
                        divStep4.Visible = true;
                        divStep4Data.Visible = true;
                        Metodos.LlenaComboDuracionEnumerador(ddlTiempoTotal);
                        rptSubRoles.DataSource = _servicioSubGrupo.ObtenerSubGruposUsuarioByIdGrupo(int.Parse(ddlGrupoResponsableAtencion.SelectedValue));
                        rptSubRoles.DataBind();
                        Metodos.LlenaComboCatalogo(ddlPrioridad, _servicioImpactoUrgencia.ObtenerPrioridad(true));
                        Metodos.LlenaComboCatalogo(ddlUrgencia, _servicioImpactoUrgencia.ObtenerUrgencia(true));
                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btn.CommandArgument = "4";
                        break;
                    case 4:
                        ValidaCapturaPaso4();
                        divStep1Data.Visible = false;
                        divStep2Data.Visible = false;
                        divStep3Data.Visible = false;
                        divStep4Data.Visible = false;
                        divStep5.Visible = true;
                        divStep5Data.Visible = true;
                        Metodos.LlenaComboDuracionEnumerador(ddlNotificacionResponsableCategoria);
                        Metodos.LlenaComboDuracionEnumerador(ddlNotificacionResponsableContenido);
                        Metodos.LlenaComboDuracionEnumerador(ddlNotificacionResponsableDesarrollo);
                        Metodos.LlenaComboDuracionEnumerador(ddlNotificacionResponsableOperacion);
                        List<TipoNotificacion> lst = _servicioNotificacion.ObtenerTipos(true);
                        Metodos.LlenaComboCatalogo(ddlCanalResponsableCategoria, lst);
                        Metodos.LlenaComboCatalogo(ddlCanalResponsableContenido, lst);
                        Metodos.LlenaComboCatalogo(ddlCanalResponsableDesarrollo, lst);
                        Metodos.LlenaComboCatalogo(ddlCanalResponsableOperacion, lst);
                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btn.CommandArgument = "5";
                        break;
                    case 5:
                        ValidaCapturaPaso5();
                        divStep1Data.Visible = false;
                        divStep2Data.Visible = false;
                        divStep3Data.Visible = false;
                        divStep4Data.Visible = false;
                        divStep5Data.Visible = false;
                        divStep6.Visible = true;
                        divStep6Data.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlEncuesta, _servicioEncuesta.ObtenerEncuestas(true), "Titulo");
                        btnPreview.Visible = true;
                        btnSaveAll.Visible = true;
                        btnSiguiente.Visible = false;
                        btn.CommandArgument = "6";
                        break;
                }

            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        #region Configuracion
        protected void ddlTipoUsuarioNivel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        protected void ddlArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlNivel1);
                ddlNivel1_OnSelectedIndexChanged(ddlNivel1, null);
                Metodos.LlenaComboCatalogo(ddlNivel1, _servicioArbolAcceso.ObtenerNivel1(IdArea, TipoArbol, IdTipoUsuario, true));
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        protected void btnGuardarArea_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtDescripcionArea.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar una descripción de categoria");
                Area area = new Area { Descripcion = txtDescripcionArea.Text.Trim() };
                if (Session["ImagenArea"] != null)
                    if (Session["ImagenArea"].ToString() != string.Empty)
                        area.Imagen = BusinessFile.Imagenes.ImageToByteArray(Session["ImagenArea"].ToString());
                area.Habilitado = true;
                area.IdUsuarioAlta = ((Usuario)Session["UserData"]).Id;
                _servicioArea.Guardar(area);
                txtDescripcionArea.Text = String.Empty;
                Metodos.LlenaComboCatalogo(ddlArea, _servicioArea.ObtenerAreas(true).Where(w => !w.Sistema).ToList());
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        protected void ddlNivel1_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlNivel2);
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                //Metodos.LimpiarCombo(ddlNivel7);
                divNivel2.Visible = false;
                divNivel3.Visible = false;
                divNivel4.Visible = false;
                divNivel5.Visible = false;
                divNivel6.Visible = false;
                btnSiguiente.Enabled = true;
                //divNivel7.Visible = false;
                if (((DropDownList)sender).SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, null, null, null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel1, ddlNivel2, _servicioArbolAcceso.ObtenerNivel2(IdArea, TipoArbol, IdTipoUsuario, IdNivel1, true));
                    btnAgregarNivel2.Enabled = true;
                    btnAgregarNivel3.Enabled = false;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel2.Visible = true;
                    divNivel3.Visible = false;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                }
                else
                {
                    btnAgregarNivel2.Enabled = false;
                    btnAgregarNivel3.Enabled = false;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel2.Visible = false;
                    divNivel3.Visible = false;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                    //divNivel7.Visible = false;
                    btnSiguiente.Enabled = false;
                    throw new Exception("Para continuar seleccione un nivel no terminal.");
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }
        protected void ddlNivel2_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                //Metodos.LimpiarCombo(ddlNivel7);
                divNivel3.Visible = false;
                divNivel4.Visible = false;
                divNivel5.Visible = false;
                divNivel6.Visible = false;
                btnSiguiente.Enabled = true;
                //divNivel7.Visible = false;
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;

                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, null, null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel2, ddlNivel3, _servicioArbolAcceso.ObtenerNivel3(IdArea, TipoArbol, IdTipoUsuario, IdNivel2, true));
                    btnAgregarNivel3.Enabled = true;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    divNivel3.Visible = true;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                }
                else
                {
                    btnAgregarNivel3.Enabled = false;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel3.Visible = false;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                    //divNivel7.Visible = false;
                    btnSiguiente.Enabled = false;
                    throw new Exception("Para continuar seleccione un nivel no terminal.");
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }
        protected void ddlNivel3_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                //Metodos.LimpiarCombo(ddlNivel7);
                divNivel4.Visible = false;
                divNivel5.Visible = false;
                divNivel6.Visible = false;
                btnSiguiente.Enabled = true;
                //divNivel7.Visible = false;
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel3, ddlNivel4, _servicioArbolAcceso.ObtenerNivel4(IdArea, TipoArbol, IdTipoUsuario, IdNivel3, true));
                    btnAgregarNivel4.Enabled = true;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel4.Visible = true;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                    //divNivel7.Visible = false;
                }
                else
                {
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                    //divNivel7.Visible = false;
                    btnSiguiente.Enabled = false;
                    throw new Exception("Para continuar seleccione un nivel no terminal.");
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }
        protected void ddlNivel4_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                //Metodos.LimpiarCombo(ddlNivel7);
                divNivel5.Visible = false;
                divNivel6.Visible = false;
                btnSiguiente.Enabled = true;
                //divNivel7.Visible = false;
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, IdNivel4, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel4, ddlNivel5, _servicioArbolAcceso.ObtenerNivel5(IdArea, TipoArbol, IdTipoUsuario, IdNivel4, true));
                    btnAgregarNivel5.Enabled = true;
                    btnAgregarNivel6.Enabled = false;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel5.Visible = true;
                    divNivel6.Visible = false;
                    //divNivel7.Visible = false;
                }
                else
                {
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                    //divNivel7.Visible = false;
                    btnSiguiente.Enabled = false;
                    throw new Exception("Para continuar seleccione un nivel no terminal.");
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }
        protected void ddlNivel5_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlNivel6);
                //Metodos.LimpiarCombo(ddlNivel7);
                divNivel6.Visible = false;
                btnSiguiente.Enabled = true;
                //divNivel7.Visible = false;
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, IdNivel4, IdNivel5, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel5, ddlNivel6, _servicioArbolAcceso.ObtenerNivel6(IdArea, TipoArbol, IdTipoUsuario, IdNivel5, true));
                    btnAgregarNivel6.Enabled = true;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel6.Visible = true;
                    //divNivel7.Visible = false;
                }
                else
                {
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    //btnAgregarNivel7.Enabled = false;
                    divNivel6.Visible = false;
                    //divNivel7.Visible = false;
                    btnSiguiente.Enabled = false;
                    throw new Exception("Para continuar seleccione un nivel no terminal.");
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }
        protected void ddlNivel6_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSiguiente.Enabled = true;
                //Metodos.LimpiarCombo(ddlNivel7);
                //divNivel7.Visible = false;
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, IdNivel4, IdNivel5, IdNivel6, null))
                {
                    //Metodos.FiltraCombo(ddlNivel6, ddlNivel7, _servicioArbolAcceso.ObtenerNivel7(IdArea, TipoArbol, IdTipoUsuario, IdNivel6, true));
                    //btnAgregarNivel7.Enabled = true;
                    //divNivel7.Visible = true;
                }
                else
                {
                    //btnAgregarNivel7.Enabled = false;
                    //divNivel7.Visible = false;
                    btnSiguiente.Enabled = false;
                    throw new Exception("Para continuar seleccione un nivel no terminal.");
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }
        protected void ddlNivel7_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                btnSiguiente.Enabled = true;
                //if (_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, IdNivel4, IdNivel5, IdNivel6, IdNivel7))
                //{
                //    btnSiguiente.Enabled = false;
                //    throw new Exception("Para continuar seleccione un nivel no terminal.");
                //}
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        protected void btnAgregarNivel_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn = (LinkButton)sender;
                ValidaCapturaNivel(int.Parse(lbtn.CommandArgument));
                ArbolAcceso arbol = new ArbolAcceso
                {
                    IdArea = IdArea,
                    IdTipoUsuario = IdTipoUsuario,
                    IdTipoArbolAcceso = TipoArbol,
                    Evaluacion = false,
                    EsTerminal = false,
                    Habilitado = chkNivelHabilitado.Checked,
                    Sistema = false,
                    IdUsuarioAlta = ((Usuario)Session["UserData"]).Id
                };
                switch (int.Parse(lbtn.CommandArgument))
                {
                    case 1:
                        arbol.Nivel1 = new Nivel1
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtDescripcionN1.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        _servicioArbolAcceso.GuardarArbol(arbol);
                        txtDescripcionN1.Text = string.Empty;
                        Metodos.LlenaComboCatalogo(ddlNivel1, _servicioArbolAcceso.ObtenerNivel1(IdArea, TipoArbol, IdTipoUsuario, true));
                        break;
                    case 2:
                        arbol.IdNivel1 = IdNivel1;
                        arbol.Nivel2 = new Nivel2
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtDescripcionN2.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        _servicioArbolAcceso.GuardarArbol(arbol);
                        txtDescripcionN2.Text = string.Empty;
                        ddlNivel1_OnSelectedIndexChanged(ddlNivel1, null);
                        break;
                    case 3:
                        arbol.IdNivel1 = IdNivel1;
                        arbol.IdNivel2 = IdNivel2;
                        arbol.Nivel3 = new Nivel3
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtDescripcionN3.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        _servicioArbolAcceso.GuardarArbol(arbol);
                        txtDescripcionN3.Text = string.Empty;
                        ddlNivel2_OnSelectedIndexChanged(ddlNivel2, null);
                        break;
                    case 4:
                        arbol.IdNivel1 = IdNivel1;
                        arbol.IdNivel2 = IdNivel2;
                        arbol.IdNivel3 = IdNivel3;
                        arbol.Nivel4 = new Nivel4
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtDescripcionN4.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        _servicioArbolAcceso.GuardarArbol(arbol);
                        txtDescripcionN4.Text = string.Empty;
                        ddlNivel3_OnSelectedIndexChanged(ddlNivel3, null);
                        break;
                    case 5:
                        arbol.IdNivel1 = IdNivel1;
                        arbol.IdNivel2 = IdNivel2;
                        arbol.IdNivel3 = IdNivel3;
                        arbol.IdNivel4 = IdNivel4;
                        arbol.Nivel5 = new Nivel5
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtDescripcionN5.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        _servicioArbolAcceso.GuardarArbol(arbol);
                        txtDescripcionN5.Text = string.Empty;
                        ddlNivel4_OnSelectedIndexChanged(ddlNivel4, null);
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
                            Descripcion = txtDescripcionN6.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        _servicioArbolAcceso.GuardarArbol(arbol);
                        txtDescripcionN6.Text = string.Empty;
                        ddlNivel5_OnSelectedIndexChanged(ddlNivel5, null);
                        break;
                    //case 7:
                    //    arbol.IdNivel1 = IdNivel1;
                    //    arbol.IdNivel2 = IdNivel2;
                    //    arbol.IdNivel3 = IdNivel3;
                    //    arbol.IdNivel4 = IdNivel4;
                    //    arbol.IdNivel5 = IdNivel5;
                    //    arbol.IdNivel6 = IdNivel6;
                    //    arbol.Nivel7 = new Nivel7
                    //    {
                    //        IdTipoUsuario = IdTipoUsuario,
                    //        Descripcion = txtDescripcionN7.Text.Trim(),
                    //        Habilitado = chkNivelHabilitado.Checked
                    //    };
                    //    _servicioArbolAcceso.GuardarArbol(arbol);
                    //    txtDescripcionN7.Text = string.Empty;
                    //    ddlNivel6_OnSelectedIndexChanged(ddlNivel6, null);
                    //    break;
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        #endregion Configuracion

        #region Sla
        public Sla ObtenerSla()
        {
            try
            {
                Sla sla = new Sla
                {
                    SlaDetalle = new List<SlaDetalle>(),
                    Detallado = chkEstimado.Checked,
                    Habilitado = true
                };
                decimal tDias = 0, tHoras = 0, tminutos = 0, tsegundos = 0;
                if (chkEstimado.Checked)
                    foreach (RepeaterItem item in rptSubRoles.Items)
                    {
                        var lblIdSubRol = (Label)item.FindControl("lblIdSubRol");
                        var txtDias = (TextBox)item.FindControl("txtDias");
                        SlaDetalle detalle = new SlaDetalle { IdSubRol = Convert.ToInt32(lblIdSubRol.Text.Trim()) };
                        if (txtDias != null)
                        {
                            detalle.Dias = Convert.ToDecimal(txtDias.Text.Trim());
                            tDias += detalle.Dias;
                        }
                        detalle.TiempoProceso = (detalle.Dias * 24) + detalle.Horas + (detalle.Minutos / 60) + ((detalle.Minutos / 60) / 60);
                        sla.SlaDetalle.Add(detalle);
                    }
                else
                {
                    switch (int.Parse(ddlTiempoTotal.SelectedValue))
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Dias:
                            tDias = decimal.Parse(txtTiempoTotal.Text);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Horas:
                            tHoras = decimal.Parse(txtTiempoTotal.Text);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Minutos:
                            tminutos = decimal.Parse(txtTiempoTotal.Text);
                            break;
                    }
                }

                sla.Dias = tDias;
                sla.Horas = tHoras;
                sla.Minutos = tminutos;
                sla.Segundos = tsegundos;
                sla.TiempoHoraProceso = (tDias * 24) + tHoras + (tminutos / 60) + ((tsegundos / 60) / 60);
                return sla;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        protected void chkEstimado_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem repeaterItem in rptSubRoles.Items)
                {
                    TextBox txt = (TextBox)repeaterItem.FindControl("txtDias");
                    txt.Enabled = chkEstimado.Checked;
                    DropDownList ddl = (DropDownList)repeaterItem.FindControl("ddlDuracionRepeater");
                    ddl.Enabled = chkEstimado.Checked;
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

        protected void ddlImpacto_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObtenerImpactoUrgencia();
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

        protected void ddlUrgencia_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObtenerImpactoUrgencia();
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

        protected void rptSubRoles_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlDuracionRepeater");
                if (ddl != null)
                    Metodos.LlenaComboDuracionEnumerador(ddl);
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        #endregion Sla

        #region Notificaciones
        protected void chkNotificacionResponsableCategoria_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtTiempoNotificacionResponsableCategoria.Enabled = chkNotificacionResponsableCategoria.Checked;
                ddlNotificacionResponsableCategoria.Enabled = chkNotificacionResponsableCategoria.Checked;
                ddlCanalResponsableCategoria.Enabled = chkNotificacionResponsableCategoria.Checked;
                chkVencimientoResponsableCategoria.Enabled = chkNotificacionResponsableCategoria.Checked;
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

        protected void chkNotificacionResponsableContenido_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtTiempoNotificacionResponsableContenido.Enabled = chkNotificacionResponsableContenido.Checked;
                ddlNotificacionResponsableContenido.Enabled = chkNotificacionResponsableContenido.Checked;
                ddlCanalResponsableContenido.Enabled = chkNotificacionResponsableContenido.Checked;
                chkVencimientoResponsableContenido.Enabled = chkNotificacionResponsableContenido.Checked;
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

        protected void chkNotificacionResponsableDesarrollo_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtTiempoNotificacionResponsableDesarrollo.Enabled = chkNotificacionResponsableDesarrollo.Checked;
                ddlNotificacionResponsableDesarrollo.Enabled = chkNotificacionResponsableDesarrollo.Checked;
                ddlCanalResponsableDesarrollo.Enabled = chkNotificacionResponsableDesarrollo.Checked;
                chkVencimientoResponsableDesarrollo.Enabled = chkNotificacionResponsableDesarrollo.Checked;
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

        protected void chkNotificacionResponsableOperacion_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtTiempoNotificacionResponsableOperacion.Enabled = chkNotificacionResponsableOperacion.Checked;
                ddlNotificacionResponsableOperacion.Enabled = chkNotificacionResponsableOperacion.Checked;
                ddlCanalResponsableOperacion.Enabled = chkNotificacionResponsableOperacion.Checked;
                chkVencimientoResponsableOperacion.Enabled = chkNotificacionResponsableOperacion.Checked;
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
        #endregion Notificaciones

        #region Encuestas
        protected void chkEncuestaActiva_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ddlEncuesta.Enabled = chkEncuestaActiva.Checked;
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
        #endregion Encuestas

        protected void btnSaveAll_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaPaso6();
                ArbolAcceso arbol = new ArbolAcceso
                {
                    IdArea = IdArea,
                    IdTipoUsuario = IdTipoUsuario,
                    IdTipoArbolAcceso = TipoArbol,
                    Descripcion = txtDescripcionOpcion.Text,
                    Evaluacion = chkEncuestaActiva.Checked,
                    EsTerminal = true,
                    Publico = chkPublico.Checked,
                    Habilitado = chkNivelHabilitado.Checked,
                    Sistema = false,
                    IdUsuarioAlta = ((Usuario)Session["UserData"]).Id
                };
                arbol.IdImpacto = ObtenerImpactoUrgencia().Id;
                arbol.InventarioArbolAcceso = new List<InventarioArbolAcceso> { new InventarioArbolAcceso() };
                arbol.InventarioArbolAcceso.First().IdMascara = Convert.ToInt32(ddlFormularios.SelectedValue) == 0 ? (int?)null : Convert.ToInt32(ddlFormularios.SelectedValue);
                arbol.InventarioArbolAcceso.First().Sla = ObtenerSla();
                arbol.InventarioArbolAcceso.First().IdEncuesta = Convert.ToInt32(ddlEncuesta.SelectedValue) == BusinessVariables.ComboBoxCatalogo.ValueSeleccione ? (int?)null : Convert.ToInt32(ddlEncuesta.SelectedValue);
                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol = new List<GrupoUsuarioInventarioArbol>();
                arbol.TiempoInformeArbol = new List<TiempoInformeArbol>();

                var gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoAcceso.SelectedValue));
                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                {
                    IdGrupoUsuario = gpo.Id,
                    IdRol = (int)BusinessVariables.EnumRoles.Usuario,
                    IdSubGrupoUsuario = null
                });

                #region Notificacion
                
                #region Responsable Categoria
                gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoResponsableCategoria.SelectedValue));

                if (chkNotificacionResponsableCategoria.Checked)
                {
                    TiempoInformeArbol tiempoInforme = new TiempoInformeArbol
                    {
                        IdTipoGrupo = gpo.IdTipoGrupo,
                        IdGrupoUsuario = gpo.Id,
                        AntesVencimiento = !chkNotificacionResponsableCategoria.Checked
                    };
                    decimal dias = 0, horas = 0, minutos = 0;
                    const decimal segundos = 0;

                    switch (int.Parse(ddlNotificacionResponsableCategoria.SelectedValue))
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Dias:
                            dias = decimal.Parse(txtTiempoNotificacionResponsableCategoria.Text);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Horas:
                            horas = decimal.Parse(txtTiempoNotificacionResponsableCategoria.Text);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Minutos:
                            minutos = decimal.Parse(txtTiempoNotificacionResponsableCategoria.Text);
                            break;
                    }
                    tiempoInforme.Dias = dias;
                    tiempoInforme.Horas = horas;
                    tiempoInforme.Minutos = minutos;
                    tiempoInforme.Segundos = segundos;
                    tiempoInforme.TiempoNotificacion += (dias * 24) + horas + (minutos / 60) + ((segundos / 60) / 60);
                    tiempoInforme.IdTipoNotificacion = int.Parse(ddlCanalResponsableCategoria.SelectedValue);
                    tiempoInforme.AntesVencimiento = !chkVencimientoResponsableCategoria.Checked;
                    arbol.TiempoInformeArbol.Add(tiempoInforme);
                }
                #endregion Responsable Categoria

                #region Responsable de contenido

                gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoResponsableMantenimiento.SelectedValue));
                foreach (SubGrupoUsuario subGrupoUsuario in gpo.SubGrupoUsuario)
                {
                    if (chkNotificacionResponsableContenido.Checked)
                    {
                        TiempoInformeArbol tiempoInforme = new TiempoInformeArbol
                        {
                            IdTipoGrupo = gpo.IdTipoGrupo,
                            IdGrupoUsuario = gpo.Id,
                            AntesVencimiento = !chkNotificacionResponsableContenido.Checked
                        };
                        decimal dias = 0, horas = 0, minutos = 0;
                        const decimal segundos = 0;

                        switch (int.Parse(ddlNotificacionResponsableContenido.SelectedValue))
                        {
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Dias:
                                dias = decimal.Parse(txtTiempoNotificacionResponsableContenido.Text);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Horas:
                                horas = decimal.Parse(txtTiempoNotificacionResponsableContenido.Text);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Minutos:
                                minutos = decimal.Parse(txtTiempoNotificacionResponsableContenido.Text);
                                break;
                        }
                        tiempoInforme.Dias = dias;
                        tiempoInforme.Horas = horas;
                        tiempoInforme.Minutos = minutos;
                        tiempoInforme.Segundos = segundos;
                        tiempoInforme.TiempoNotificacion += (dias * 24) + horas + (minutos / 60) + ((segundos / 60) / 60);
                        tiempoInforme.IdTipoNotificacion = int.Parse(ddlCanalResponsableContenido.SelectedValue);
                        tiempoInforme.AntesVencimiento = !chkVencimientoResponsableContenido.Checked;
                        arbol.TiempoInformeArbol.Add(tiempoInforme);
                    }
                    arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                    {
                        IdGrupoUsuario = subGrupoUsuario.IdGrupoUsuario,
                        IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeContenido,
                        IdSubGrupoUsuario = subGrupoUsuario.Id
                    });
                }
                #endregion Responsable de contenido

                #region Responsable de desarrollo

                gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoResponsableDesarrollo.SelectedValue));

                if (chkNotificacionResponsableDesarrollo.Checked)
                {
                    TiempoInformeArbol tiempoInforme = new TiempoInformeArbol
                    {
                        IdTipoGrupo = gpo.IdTipoGrupo,
                        IdGrupoUsuario = gpo.Id,
                        AntesVencimiento = !chkNotificacionResponsableDesarrollo.Checked
                    };
                    decimal dias = 0, horas = 0, minutos = 0;
                    const decimal segundos = 0;

                    switch (int.Parse(ddlNotificacionResponsableDesarrollo.SelectedValue))
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Dias:
                            dias = decimal.Parse(txtTiempoNotificacionResponsableDesarrollo.Text);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Horas:
                            horas = decimal.Parse(txtTiempoNotificacionResponsableDesarrollo.Text);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Minutos:
                            minutos = decimal.Parse(txtTiempoNotificacionResponsableDesarrollo.Text);
                            break;
                    }
                    tiempoInforme.Dias = dias;
                    tiempoInforme.Horas = horas;
                    tiempoInforme.Minutos = minutos;
                    tiempoInforme.Segundos = segundos;
                    tiempoInforme.TiempoNotificacion += (dias * 24) + horas + (minutos / 60) + ((segundos / 60) / 60);
                    tiempoInforme.IdTipoNotificacion = int.Parse(ddlCanalResponsableDesarrollo.SelectedValue);
                    tiempoInforme.AntesVencimiento = !chkVencimientoResponsableDesarrollo.Checked;
                    arbol.TiempoInformeArbol.Add(tiempoInforme);
                }

                #endregion Responsable de desarrollo

                #region Responsable de operación

                gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGruporesponsableOperacion.SelectedValue));
                if (chkNotificacionResponsableOperacion.Checked)
                {
                    TiempoInformeArbol tiempoInforme = new TiempoInformeArbol
                    {
                        IdTipoGrupo = gpo.IdTipoGrupo,
                        IdGrupoUsuario = gpo.Id,
                        AntesVencimiento = !chkNotificacionResponsableOperacion.Checked
                    };
                    decimal dias = 0, horas = 0, minutos = 0;
                    const decimal segundos = 0;

                    switch (int.Parse(ddlNotificacionResponsableOperacion.SelectedValue))
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Dias:
                            dias = decimal.Parse(txtTiempoNotificacionResponsableOperacion.Text);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Horas:
                            horas = decimal.Parse(txtTiempoNotificacionResponsableOperacion.Text);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion.Minutos:
                            minutos = decimal.Parse(txtTiempoNotificacionResponsableOperacion.Text);
                            break;
                    }
                    tiempoInforme.Dias = dias;
                    tiempoInforme.Horas = horas;
                    tiempoInforme.Minutos = minutos;
                    tiempoInforme.Segundos = segundos;
                    tiempoInforme.TiempoNotificacion += (dias * 24) + horas + (minutos / 60) + ((segundos / 60) / 60);
                    tiempoInforme.IdTipoNotificacion = int.Parse(ddlCanalResponsableOperacion.SelectedValue);
                    tiempoInforme.AntesVencimiento = !chkVencimientoResponsableOperacion.Checked;
                    arbol.TiempoInformeArbol.Add(tiempoInforme);
                }
                
                #endregion Responsable de operación
                #endregion Notificacion
                
                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                {
                    IdGrupoUsuario = gpo.Id,
                    IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeOperación,
                    IdSubGrupoUsuario = null
                });

                

                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                {
                    IdGrupoUsuario = gpo.Id,
                    IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo,
                    IdSubGrupoUsuario = null
                });

                gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoResponsableAtencion.SelectedValue));
                foreach (SubGrupoUsuario subGrupoUsuario in gpo.SubGrupoUsuario)
                {
                    arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                    {
                        IdGrupoUsuario = subGrupoUsuario.IdGrupoUsuario,
                        IdRol = (int)BusinessVariables.EnumRoles.Agente,
                        IdSubGrupoUsuario = subGrupoUsuario.Id
                    });
                }

                foreach (ListItem item in lstGruposEc.Items)
                {
                    if (item.Selected)
                    {
                        gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(item.Value));
                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                        {
                            IdGrupoUsuario = gpo.Id,
                            IdRol = (int)BusinessVariables.EnumRoles.ConsultasEspeciales,
                            IdSubGrupoUsuario = null
                        });
                    }
                }

                foreach (ListItem item in lstGruposAu.Items)
                {
                    if (item.Selected)
                    {
                        gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(item.Value));
                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                        {
                            IdGrupoUsuario = gpo.Id,
                            IdRol = (int)BusinessVariables.EnumRoles.AgenteUniversal,
                            IdSubGrupoUsuario = null
                        });
                    }
                }

                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                {
                    IdGrupoUsuario = gpo.Id,
                    IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeCategoría,
                    IdSubGrupoUsuario = null
                });

                arbol.InventarioArbolAcceso.First().Descripcion = txtDescripcionNivel.Text.Trim();
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
                LimpiarPantalla();
                _mp.AlertaSucces();
                                
                if (OnCancelarModal != null)
                    OnCancelarModal();

            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }

        protected void btnPreview_OnClick(object sender, EventArgs e)
        {
            try
            {
                Session["PreviewDataFormulario"] = _servicioMascaras.ObtenerMascaraCaptura(Convert.ToInt32(ddlFormularios.SelectedValue));
                if (Request.ApplicationPath != null)
                {
                    string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "window.open('" + url + "Users/Administracion/Formularios/FrmPreviewFormulario.aspx','_blank');", true);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null || !_lstError.Any())
                {
                    _lstError = new List<string> { ex.Message };
                }
                Alerta = _lstError;
            }
        }
    }
}