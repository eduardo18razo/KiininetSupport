using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniHelp.ServiceSistemaRol;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.ArbolesAcceso
{
    public partial class UcEdicionOpcionConsulta : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceInformacionConsultaClient _servicioInformacionConsulta = new ServiceInformacionConsultaClient();
        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();
        private readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
        private readonly ServiceRolesClient _servicioRoles = new ServiceRolesClient();
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

        private bool EsAlta
        {
            get { return bool.Parse(ViewState["EsAlta"].ToString()); }
            set { ViewState["EsAlta"] = value.ToString(); }
        }

        private int IdArbol
        {
            get { return int.Parse(ViewState["idConsulta"].ToString()); }
            set { ViewState["idConsulta"] = value.ToString(); }
        }
        private int IdTipoUsuario
        {
            get
            {
                if (ddlTipoUsuario.SelectedIndex < BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return 0;
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione quien puede ver el contenido.");
                return int.Parse(ddlTipoUsuario.SelectedValue);
            }
            set { ddlTipoUsuario.SelectedValue = value.ToString(); }
        }

        private List<RolTipoArbolAcceso> RolesActivos
        {
            get
            {
                return _servicioRoles.ObtenerRolesArbolAcceso((int)BusinessVariables.EnumTipoArbol.ConsultarInformacion);
            }
        }
        private int TipoArbol
        {
            get { return (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion; }
        }
        public int IdArea
        {
            get
            {
                if (ddlCategoria.SelectedIndex < BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return 0;
                if (ddlCategoria.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione una categoria.");
                return int.Parse(ddlCategoria.SelectedValue);
            }
            set
            {
                ddlCategoria.SelectedValue = value.ToString();
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
                Metodos.LlenaComboCatalogo(ddlArticulo, _servicioInformacionConsulta.ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido, true));
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

        private void ValidaCaptura()
        {
            try
            {
                List<string> errors = new List<string>();
                if (txtTitulo.Text.Trim() == string.Empty)
                    errors.Add("Ingrese título para la opción");
                if (txtDescripcionOpcion.Text.Trim() == string.Empty)
                    errors.Add("Ingrese descripción para la opción");
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    errors.Add("Seleccione quien puede ver el contenido");
                if (ddlArticulo.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    errors.Add("Seleccione un artículo para mostrar");
                try
                {
                    var valida = IdArea;
                }
                catch (Exception e)
                {
                    errors.Add(e.Message);
                }
                if (ddlGrupoAccesoCentroSoporte.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleeccione grupo de acceso.");
                }
                if (divGpoResponsableCategoria.Visible && ddlGrupoResponsableCategoria.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleeccione grupo Responsable de Categoría.");
                }
                if (divGpoResponsableContenido.Visible && ddlGrupoResponsableContenido.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleeccione grupo Responsable del contenido.");
                }

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
        public void LimpiarPantalla()
        {
            try
            {
                //Step 1
                ddlNivel1_OnSelectedIndexChanged(ddlNivel1, null);
                ddlTipoUsuario.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                txtTitulo.Text = string.Empty;
                txtDescripcionOpcion.Text = string.Empty;
                LlenaCombos();
                chkPublico.Checked = false;
                chkEvaluacion.Checked = true;
                chkNivelHabilitado.Checked = true;
                Metodos.LimpiarCombo(ddlNivel1);
                Metodos.LimpiarCombo(ddlNivel2);
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);

                //Step 2
                txtDescripcionCategoria.Text = string.Empty;
                txtDescripcionN1.Text = string.Empty;
                txtDescripcionN2.Text = string.Empty;
                txtDescripcionN3.Text = string.Empty;
                txtDescripcionN4.Text = string.Empty;
                txtDescripcionN5.Text = string.Empty;
                txtDescripcionN6.Text = string.Empty;
                Metodos.LimpiarCombo(ddlCategoria);
                Metodos.LimpiarCombo(ddlNivel1);
                divNivel2.Visible = false;
                divNivel3.Visible = false;
                divNivel4.Visible = false;
                divNivel5.Visible = false;
                divNivel6.Visible = false;
                Metodos.LimpiarCombo(ddlGrupoAccesoCentroSoporte);
                Metodos.LimpiarCombo(ddlGrupoResponsableCategoria);
                Metodos.LimpiarCombo(ddlGrupoResponsableContenido);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void CargaConsulta()
        {
            try
            {
                ArbolAcceso consulta = _servicioArbolAcceso.ObtenerArbolAcceso(IdArbol);
                if (consulta != null)
                {
                    ddlTipoUsuario.SelectedValue = consulta.IdTipoUsuario.ToString();
                    ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                    txtTitulo.Text = consulta.InventarioArbolAcceso.First().Descripcion;
                    txtDescripcionOpcion.Text = consulta.Descripcion;
                    chkPublico.Checked = consulta.Publico;
                    ddlArticulo.SelectedValue = consulta.InventarioArbolAcceso.First().InventarioInfConsulta.First().IdInfConsulta.ToString();
                    chkEvaluacion.Checked = consulta.Evaluacion;
                    chkNivelHabilitado.Checked = consulta.Habilitado;

                    ddlCategoria.SelectedValue = consulta.IdArea.ToString();
                    ddlCategoria_OnSelectedIndexChanged(ddlCategoria, null);

                    if (consulta.Nivel1 != null)
                    {
                        ddlNivel1.SelectedValue = consulta.IdNivel1.ToString();
                        ddlNivel1_OnSelectedIndexChanged(ddlNivel1, null);
                    }
                    if (consulta.Nivel2 != null)
                    {
                        ddlNivel2.SelectedValue = consulta.IdNivel2.ToString();
                        ddlNivel2_OnSelectedIndexChanged(ddlNivel2, null);
                    }
                    if (consulta.Nivel3 != null)
                    {
                        ddlNivel3.SelectedValue = consulta.IdNivel3.ToString();
                        ddlNivel3_OnSelectedIndexChanged(ddlNivel3, null);
                    }
                    if (consulta.Nivel4 != null)
                    {
                        ddlNivel4.SelectedValue = consulta.IdNivel4.ToString();
                        ddlNivel4_OnSelectedIndexChanged(ddlNivel4, null);
                    }
                    if (consulta.Nivel5 != null)
                    {
                        ddlNivel5.SelectedValue = consulta.IdNivel5.ToString();
                        ddlNivel5_OnSelectedIndexChanged(ddlNivel5, null);
                    }
                    if (consulta.Nivel6 != null)
                    {
                        ddlNivel6.SelectedValue = consulta.IdNivel6.ToString();
                        ddlNivel6_OnSelectedIndexChanged(ddlNivel6, null);
                    }
                    ddlGrupoAccesoCentroSoporte.SelectedValue =
                        consulta.InventarioArbolAcceso.First()
                            .GrupoUsuarioInventarioArbol.Single(s => s.GrupoUsuario.IdTipoGrupo ==(int) BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).IdGrupoUsuario.ToString();
                    ddlTipoUsuario.Enabled = false;
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
                _mp = (UsuariosMaster)Page.Master;
                Alerta = new List<string>();
                faq5_1.CssClass = "panel-collapse collapse in";
                faq5_2.CssClass = "panel-collapse collapse in";
                faq5_3.CssClass = "panel-collapse collapse in";
                if (!IsPostBack)
                {
                    LlenaCombos();
                    if (Request.Params["IdArbolAccesoConsulta"] != null)
                    {
                        EsAlta = false;
                        IdArbol = int.Parse(Request.Params["IdArbolAccesoConsulta"]);
                        CargaConsulta();
                        lblMovimiento.Text = "Edicion opción Consulta";
                    }
                    else
                    {
                        EsAlta = true;
                        lblMovimiento.Text = "Alta opción Consulta";
                    }
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
        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                Metodos.LlenaComboCatalogo(ddlCategoria, _servicioArea.ObtenerAreas(true).Where(w => !w.Sistema).ToList());
                Metodos.LlenaComboCatalogo(ddlGrupoAccesoCentroSoporte, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.AccesoCentroSoporte, IdTipoUsuario, true));
                if (RolesActivos.Any(a => a.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeCategoría))
                {
                    divGpoResponsableCategoria.Visible = true;
                    Metodos.LlenaComboCatalogo(ddlGrupoResponsableCategoria, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ResponsableDeCategoría, (int)BusinessVariables.EnumTiposUsuario.Agentes, true));
                }
                if (RolesActivos.Any(a => a.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeContenido))
                {
                    divGpoResponsableContenido.Visible = true;
                    Metodos.LlenaComboCatalogo(ddlGrupoResponsableContenido, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ResponsableDeContenido, (int)BusinessVariables.EnumTiposUsuario.Agentes, true));
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
        protected void ddlCategoria_OnSelectedIndexChanged(object sender, EventArgs e)
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
        protected void btnGuardarCategoria_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtDescripcionCategoria.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar una descripción de categoria");
                Area area = new Area { Descripcion = txtDescripcionCategoria.Text.Trim() };
                if (Session["ImagenArea"] != null)
                    if (Session["ImagenArea"].ToString() != string.Empty)
                        area.Imagen = BusinessFile.Imagenes.ImageToByteArray(Session["ImagenArea"].ToString());
                area.Habilitado = true;
                area.IdUsuarioAlta = ((Usuario)Session["UserData"]).Id;
                _servicioArea.Guardar(area);
                txtDescripcionCategoria.Text = String.Empty;
                Metodos.LlenaComboCatalogo(ddlCategoria, _servicioArea.ObtenerAreas(true).Where(w => !w.Sistema).ToList());
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
                divNivel2.Visible = false;
                divNivel3.Visible = false;
                divNivel4.Visible = false;
                divNivel5.Visible = false;
                divNivel6.Visible = false;
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
                    divNivel2.Visible = false;
                    divNivel3.Visible = false;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
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
                divNivel3.Visible = false;
                divNivel4.Visible = false;
                divNivel5.Visible = false;
                divNivel6.Visible = false;
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
                    divNivel3.Visible = false;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
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
                divNivel4.Visible = false;
                divNivel5.Visible = false;
                divNivel6.Visible = false;
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel3, ddlNivel4, _servicioArbolAcceso.ObtenerNivel4(IdArea, TipoArbol, IdTipoUsuario, IdNivel3, true));
                    btnAgregarNivel4.Enabled = true;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    divNivel4.Visible = true;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                }
                else
                {
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
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
                divNivel5.Visible = false;
                divNivel6.Visible = false;
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, IdNivel4, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel4, ddlNivel5, _servicioArbolAcceso.ObtenerNivel5(IdArea, TipoArbol, IdTipoUsuario, IdNivel4, true));
                    btnAgregarNivel5.Enabled = true;
                    btnAgregarNivel6.Enabled = false;
                    divNivel5.Visible = true;
                    divNivel6.Visible = false;
                }
                else
                {
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
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
                divNivel6.Visible = false;
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, IdNivel4, IdNivel5, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel5, ddlNivel6, _servicioArbolAcceso.ObtenerNivel6(IdArea, TipoArbol, IdTipoUsuario, IdNivel5, true));
                    btnAgregarNivel6.Enabled = true;
                    divNivel6.Visible = true;
                }
                else
                {
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    divNivel6.Visible = false;
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
                if (((DropDownList)sender).SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return;
                if (!_servicioArbolAcceso.EsNodoTerminal(IdTipoUsuario, TipoArbol, IdNivel1, IdNivel2, IdNivel3, IdNivel4, IdNivel5, IdNivel6, null))
                {
                }
                else
                {
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
                    Evaluacion = chkEvaluacion.Checked,
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
        
        protected void btnCancelarEdicion_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Users/Administracion/ArbolesAcceso/FrmConsultaArbolAcceso.aspx");
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
                Session["PreviewAltaDataConsulta"] = _servicioInformacionConsulta.ObtenerInformacionConsultaById(int.Parse(ddlArticulo.SelectedValue));
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "window.open('" + url + "Users/Administracion/ArbolesAcceso/FrmPreviewAltaOpcionConsulta.aspx?evaluacion=" + chkEvaluacion.Checked + "','_blank');", true);
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
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCaptura();
                ArbolAcceso arbol = new ArbolAcceso
                {
                    IdArea = IdArea,
                    IdTipoUsuario = IdTipoUsuario,
                    IdTipoArbolAcceso = TipoArbol,
                    Descripcion = txtDescripcionOpcion.Text,
                    Evaluacion = chkEvaluacion.Checked,
                    EsTerminal = true,
                    Publico = chkPublico.Checked,
                    Habilitado = chkNivelHabilitado.Checked,
                    Sistema = false,
                    IdUsuarioAlta = ((Usuario)Session["UserData"]).Id
                };
                arbol.InventarioArbolAcceso = new List<InventarioArbolAcceso> { new InventarioArbolAcceso() };
                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol = new List<GrupoUsuarioInventarioArbol>();

                var gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoAccesoCentroSoporte.SelectedValue));
                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                {
                    IdGrupoUsuario = gpo.Id,
                    IdRol = (int)BusinessVariables.EnumRoles.AccesoCentroSoporte,
                    IdSubGrupoUsuario = null
                });


                if (RolesActivos.Any(a => a.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeCategoría))
                {
                    gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoResponsableCategoria.SelectedValue));
                    arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                    {
                        IdGrupoUsuario = gpo.Id,
                        IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeCategoría,
                        IdSubGrupoUsuario = null
                    });
                }

                if (RolesActivos.Any(a => a.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeContenido))
                {
                    gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoResponsableContenido.SelectedValue));
                    foreach (SubGrupoUsuario subGrupoUsuario in gpo.SubGrupoUsuario)
                    {
                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                        {
                            IdGrupoUsuario = subGrupoUsuario.IdGrupoUsuario,
                            IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeContenido,
                            IdSubGrupoUsuario = subGrupoUsuario.Id
                        });
                    }
                }


                arbol.InventarioArbolAcceso.First().Descripcion = txtTitulo.Text.Trim();
                arbol.InventarioArbolAcceso.First().InventarioInfConsulta = new List<InventarioInfConsulta>();
                arbol.InventarioArbolAcceso.First().InventarioInfConsulta.Add(new InventarioInfConsulta { IdInfConsulta = Convert.ToInt32(ddlArticulo.SelectedValue) });
                switch (int.Parse(Catalogo))
                {
                    case 1:
                        arbol.Nivel1 = new Nivel1
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtTitulo.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        if (EsAlta)
                            _servicioArbolAcceso.GuardarArbol(arbol);
                        else
                        {
                            arbol.Id = IdArbol;
                            _servicioArbolAcceso.ActualizardArbol(IdArbol, arbol, txtTitulo.Text);
                        }
                        break;
                    case 2:
                        arbol.IdNivel1 = IdNivel1;
                        arbol.Nivel2 = new Nivel2
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtTitulo.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        if (EsAlta)
                            _servicioArbolAcceso.GuardarArbol(arbol);
                        else
                        {
                            arbol.Id = IdArbol;
                            _servicioArbolAcceso.ActualizardArbol(IdArbol, arbol, txtTitulo.Text);
                        }
                        break;
                    case 3:
                        arbol.IdNivel1 = IdNivel1;
                        arbol.IdNivel2 = IdNivel2;
                        arbol.Nivel3 = new Nivel3
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtTitulo.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        if (EsAlta)
                            _servicioArbolAcceso.GuardarArbol(arbol);
                        else
                        {
                            arbol.Id = IdArbol;
                            _servicioArbolAcceso.ActualizardArbol(IdArbol, arbol, txtTitulo.Text);
                        }
                        break;
                    case 4:
                        arbol.IdNivel1 = IdNivel1;
                        arbol.IdNivel2 = IdNivel2;
                        arbol.IdNivel3 = IdNivel3;
                        arbol.Nivel4 = new Nivel4
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtTitulo.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        if (EsAlta)
                            _servicioArbolAcceso.GuardarArbol(arbol);
                        else
                        {
                            arbol.Id = IdArbol;
                            _servicioArbolAcceso.ActualizardArbol(IdArbol, arbol, txtTitulo.Text);
                        }
                        break;
                    case 5:
                        arbol.IdNivel1 = IdNivel1;
                        arbol.IdNivel2 = IdNivel2;
                        arbol.IdNivel3 = IdNivel3;
                        arbol.IdNivel4 = IdNivel4;
                        arbol.Nivel5 = new Nivel5
                        {
                            IdTipoUsuario = IdTipoUsuario,
                            Descripcion = txtTitulo.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        if (EsAlta)
                            _servicioArbolAcceso.GuardarArbol(arbol);
                        else
                        {
                            arbol.Id = IdArbol;
                            _servicioArbolAcceso.ActualizardArbol(IdArbol, arbol, txtTitulo.Text);
                        }
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
                            Descripcion = txtTitulo.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        if (EsAlta)
                            _servicioArbolAcceso.GuardarArbol(arbol);
                        else
                        {
                            arbol.Id = IdArbol;
                            _servicioArbolAcceso.ActualizardArbol(IdArbol, arbol, txtTitulo.Text);
                        }
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
                            Descripcion = txtTitulo.Text.Trim(),
                            Habilitado = chkNivelHabilitado.Checked
                        };
                        if (EsAlta)
                            _servicioArbolAcceso.GuardarArbol(arbol);
                        else
                        {
                            arbol.Id = IdArbol;
                            _servicioArbolAcceso.ActualizardArbol(IdArbol, arbol, txtTitulo.Text);
                        }
                        break;
                }
                LimpiarPantalla();
                Response.Redirect("~/Users/Administracion/ArbolesAcceso/FrmConsultaArbolAcceso.aspx");
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
