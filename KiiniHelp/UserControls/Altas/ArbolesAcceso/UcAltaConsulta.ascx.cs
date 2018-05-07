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
    public partial class UcAltaConsulta : UserControl, IControllerModal
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
                if (ddlTipoUsuario.SelectedIndex < BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return 0;
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione quien puede ver el contenido.");
                return int.Parse(ddlTipoUsuario.SelectedValue);
            }
            set { ddlTipoUsuario.SelectedValue = value.ToString(); }
        }
        private int TipoArbol
        {
            get { return (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion; }
        }
        public int IdArea
        {
            get
            {
                if (ddlArea.SelectedIndex < BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    return 0;
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
        //public int IdNivel7
        //{
        //    get
        //    {
        //        if (ddlNivel7.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
        //            throw new Exception("Seleccione nivel 7.");
        //        return int.Parse(ddlNivel7.SelectedValue);
        //    }
        //    set { ddlNivel7.SelectedValue = value.ToString(); }
        //}
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
                //if (ddlNivel7.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                //    result = "7";
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
                Metodos.LlenaComboCatalogo(ddlConsultas, _servicioInformacionConsulta.ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido, true));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void ValidaCapturaPaso1()
        {
            try
            {
                List<string> errors = new List<string>();
                if (txtDescripcionNivel.Text.Trim() == string.Empty)
                    errors.Add("Ingrese título para la opción");
                if (txtDescripcionNivel.Text.Trim() == string.Empty)
                    errors.Add("Ingrese descripción para la opción");
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    errors.Add("Seleccione quien puede ver el contenido");
                if (ddlConsultas.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    errors.Add("Seleccione un artículo para mostrar");
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
                    errors.Add("Seleeccione grupo de acceso.");
                }
                if (ddlDuenoServicio.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleeccione grupo Responsable del Servicio.");
                }
                if (ddlGrupoResponsableMantenimiento.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    errors.Add("Seleeccione grupo Responsable del contenido.");
                }
                if (!lstGrupoEspecialConsulta.Items.Cast<ListItem>().Any(a => a.Selected))
                {
                    errors.Add("Seleccione al menos un grupo Especial de consulta.");
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
                    //case 7:
                    //    txt = txtDescripcionN7;
                    //    break;
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
                //Step 1
                ddlNivel1_OnSelectedIndexChanged(ddlNivel1, null);
                ddlTipoUsuario.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                txtDescripcionNivel.Text = string.Empty;
                txtDescripcionOpcion.Text = string.Empty;
                LlenaCombos();
                chkPublico.Checked = false;
                chkEvaluacion.Checked = true;
                chkNivelHabilitado.Checked = true;
                btnPaso_OnClick(new LinkButton { CommandArgument = "1" }, null);

                //Step 2
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
                        divStep1Data.Visible = true;
                        divStep2.Visible = false;
                        divStep2Data.Visible = false;
                        divStep3.Visible = false;
                        divStep3Data.Visible = false;
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
                        divStep1Data.Visible = false;
                        divStep2.Visible = true;
                        divStep2Data.Visible = true;
                        divStep3.Visible = false;
                        divStep3Data.Visible = false;
                        btnPreview.Visible = false;
                        btnSaveAll.Visible = false;
                        btnSiguiente.Visible = true;
                        btnSiguiente.CommandArgument = "2";
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
                        Metodos.LlenaComboCatalogo(ddlDuenoServicio, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ResponsableDeCategoría, (int)BusinessVariables.EnumTiposUsuario.Operador, true));
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableMantenimiento, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ResponsableDeContenido, (int)BusinessVariables.EnumTiposUsuario.Operador, true));
                        Metodos.LlenaListBoxCatalogo(lstGrupoEspecialConsulta, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumRoles.ConsultasEspeciales, (int)BusinessVariables.EnumTiposUsuario.Operador, false).Where(w=> !w.Sistema));
                        btnPreview.Visible = true;
                        btnSaveAll.Visible = true;
                        btnSiguiente.Visible = false;
                        btn.CommandArgument = "3";
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
                    //divNivel7.Visible = false;
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
                    //btnAgregarNivel7.Enabled = false;
                    divNivel3.Visible = true;
                    divNivel4.Visible = false;
                    divNivel5.Visible = false;
                    divNivel6.Visible = false;
                    //divNivel7.Visible = false;
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
        protected void ddlArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlNivel1);
                ddlNivel1_OnSelectedIndexChanged(ddlNivel1, null);
                //Lo descomento ECL
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
        protected void btnSaveAll_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaPaso3();
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

                var gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoAcceso.SelectedValue));
                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                {
                    IdGrupoUsuario = gpo.Id,
                    IdRol = (int)BusinessVariables.EnumRoles.Usuario,
                    IdSubGrupoUsuario = null
                });

                gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlDuenoServicio.SelectedValue));
                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                {
                    IdGrupoUsuario = gpo.Id,
                    IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeCategoría,
                    IdSubGrupoUsuario = null
                });

                gpo = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(ddlGrupoResponsableMantenimiento.SelectedValue));
                foreach (SubGrupoUsuario subGrupoUsuario in gpo.SubGrupoUsuario)
                {
                    arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                    {
                        IdGrupoUsuario = subGrupoUsuario.IdGrupoUsuario,
                        IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeContenido,
                        IdSubGrupoUsuario = subGrupoUsuario.Id
                    });
                }

                foreach (ListItem item in lstGrupoEspecialConsulta.Items)
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
                arbol.InventarioArbolAcceso.First().Descripcion = txtDescripcionNivel.Text.Trim();
                arbol.InventarioArbolAcceso.First().InventarioInfConsulta = new List<InventarioInfConsulta>();
                arbol.InventarioArbolAcceso.First().InventarioInfConsulta.Add(new InventarioInfConsulta { IdInfConsulta = Convert.ToInt32(ddlConsultas.SelectedValue) });
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
                if (OnAceptarModal != null)
                    OnAceptarModal();

                //agregado
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
                Session["PreviewAltaDataConsulta"] = _servicioInformacionConsulta.ObtenerInformacionConsultaById(int.Parse(ddlConsultas.SelectedValue));
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
    }
}
