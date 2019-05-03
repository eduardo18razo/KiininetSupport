using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceSistemaTipoArbolAcceso;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;

namespace KiiniHelp.Users.Administracion.ArbolesAcceso
{
    public partial class FrmAltaArbolAcceso : Page
    {
        #region Variables
        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceTipoArbolAccesoClient _servicioSistemaTipoArbol = new ServiceTipoArbolAccesoClient();
        private readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();

        private readonly ServiceAreaClient _servicioAreas = new ServiceAreaClient();

        private List<string> _lstError = new List<string>();
        #endregion Variables

        #region Propiedades privadas
        private List<string> AlertaGeneral
        {
            set
            {
                panelAlert.Visible = value.Any();
                if (!panelAlert.Visible) return;
                rptHeaderError.DataSource = value;
                rptHeaderError.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "UpScroll();", true);
            }
        }


        #endregion Propiedades privadas

        #region Metodos
        private void LlenaCombos()
        {
            try
            {
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuario(true);
                Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion Metodos

        #region Delegados
        void AltaAreas_OnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAreas\");", true);
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
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

        void AltaAreas_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAreas\");", true);
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

        private void UcAltaNivelArbolOnOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editNivel\");", true);
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

        private void UcAltaNivelArbolOnOnAceptarModal()
        {
            try
            {
                switch (int.Parse(ucAltaNivelArbol.Catalogo))
                {
                    case 1:
                        ddlTipoArbol_OnSelectedIndexChanged(ddlTipoArbol, null);
                        break;
                    case 2:
                        ddlNivel1_OnSelectedIndexChanged(ddlNivel1, null);
                        break;
                    case 3:
                        ddlNivel2_OnSelectedIndexChanged(ddlNivel2, null);
                        break;
                    case 4:
                        ddlNivel3_OnSelectedIndexChanged(ddlNivel3, null);
                        break;
                    case 5:
                        ddlNivel4_OnSelectedIndexChanged(ddlNivel4, null);
                        break;
                    case 6:
                        ddlNivel5_OnSelectedIndexChanged(ddlNivel5, null);
                        break;
                    case 7:
                        ddlNivel6_OnSelectedIndexChanged(ddlNivel6, null);
                        break;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editNivel\");", true);
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



        #endregion Delegados

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                AltaAreas.OnCancelarModal += AltaAreas_OnCancelarModal;
                AltaAreas.OnAceptarModal += AltaAreas_OnAceptarModal;
                ucAltaNivelArbol.OnAceptarModal += UcAltaNivelArbolOnOnAceptarModal;
                ucAltaNivelArbol.OnCancelarModal += UcAltaNivelArbolOnOnCancelarModal;
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
                AlertaGeneral = _lstError;
            }
        }
        protected void btnAgregarNivel_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    throw new Exception("Seleccione un tipo de usuario");
                }
                if (ddlTipoArbol.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    throw new Exception("Seleccione un tipo de arbol");
                }
                Button lbtn = (Button)sender;
                int nivel = 7;
                switch (lbtn.CommandArgument)
                {
                    case "1":
                        nivel = 1;
                        break;
                    case "2":
                        nivel = 2;
                        if (ddlNivel1.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 1");
                        }
                        break;
                    case "3":
                        nivel = 3;
                        if (ddlNivel1.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 1");
                        }
                        if (ddlNivel2.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 2");
                        }
                        break;
                    case "4":
                        nivel = 4;
                        if (ddlNivel1.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 1");
                        }
                        if (ddlNivel2.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 2");
                        }
                        if (ddlNivel3.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 3");
                        }
                        break;
                    case "5":
                        nivel = 5;
                        if (ddlNivel1.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 1");
                        }
                        if (ddlNivel2.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 2");
                        }
                        if (ddlNivel3.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 3");
                        }
                        if (ddlNivel4.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 4");
                        }
                        break;
                    case "6":
                        nivel = 6;
                        if (ddlNivel1.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 1");
                        }
                        if (ddlNivel2.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 2");
                        }
                        if (ddlNivel3.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 3");
                        }
                        if (ddlNivel4.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 4");
                        }
                        if (ddlNivel5.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 5");
                        }
                        break;
                    case "7":
                        nivel = 7;
                        if (ddlNivel1.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 1");
                        }
                        if (ddlNivel2.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 2");
                        }
                        if (ddlNivel3.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 3");
                        }
                        if (ddlNivel4.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 4");
                        }
                        if (ddlNivel5.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 5");
                        }
                        if (ddlNivel6.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        {
                            throw new Exception("Seleccione SubMenu/Opcion 6");
                        }
                        break;
                }
                string sTitle;
                switch (nivel)
                {
                    case 1:
                        sTitle = ddlTipoArbol.SelectedItem.Text + ">";
                        break;
                    case 2:
                        sTitle = ddlTipoArbol.SelectedItem.Text + ">" + ddlNivel1.SelectedItem.Text + ">";
                        break;
                    case 3:
                        sTitle = ddlTipoArbol.SelectedItem.Text + ">" + ddlNivel1.SelectedItem.Text + ">" + ddlNivel2.SelectedItem.Text + ">";
                        break;
                    case 4:
                        sTitle = ddlTipoArbol.SelectedItem.Text + ">" + ddlNivel1.SelectedItem.Text + ">" + ddlNivel2.SelectedItem.Text + ">" + ddlNivel3.SelectedItem.Text + ">";
                        break;
                    case 5:
                        sTitle = ddlTipoArbol.SelectedItem.Text + ">" + ddlNivel1.SelectedItem.Text + ">" + ddlNivel2.SelectedItem.Text + ">" + ddlNivel3.SelectedItem.Text + ">" + ddlNivel4.SelectedItem.Text + ">";
                        break;
                    case 6:
                        sTitle = ddlTipoArbol.SelectedItem.Text + ">" + ddlNivel1.SelectedItem.Text + ">" + ddlNivel2.SelectedItem.Text + ">" + ddlNivel3.SelectedItem.Text + ">" + ddlNivel4.SelectedItem.Text + ">" + ddlNivel5.SelectedItem.Text + ">";
                        break;
                    case 7:
                        sTitle = ddlTipoArbol.SelectedItem.Text + ">" + ddlNivel1.SelectedItem.Text + ">" + ddlNivel2.SelectedItem.Text + ">" + ddlNivel3.SelectedItem.Text + ">" + ddlNivel4.SelectedItem.Text + ">" + ddlNivel5.SelectedItem.Text + ">" + ddlNivel6.SelectedItem.Text + ">";
                        break;
                    default:
                        throw new Exception("Error al intentar agregar. Intente Nuevamente");
                }
                sTitle += lbtn.CommandName;
                ucAltaNivelArbol.Titulo = sTitle;
                ucAltaNivelArbol.IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);
                ucAltaNivelArbol.IdArea = int.Parse(ddlArea.SelectedValue);
                ucAltaNivelArbol.IdTipoArbol = int.Parse(ddlTipoArbol.SelectedValue);
                ucAltaNivelArbol.Catalogo = lbtn.CommandArgument;
                ucAltaNivelArbol.IdNivel1 = ddlNivel1.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? Convert.ToInt32(ddlNivel1.SelectedValue) : 0;
                ucAltaNivelArbol.IdNivel2 = ddlNivel2.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? Convert.ToInt32(ddlNivel2.SelectedValue) : 0;
                ucAltaNivelArbol.IdNivel3 = ddlNivel3.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? Convert.ToInt32(ddlNivel3.SelectedValue) : 0;
                ucAltaNivelArbol.IdNivel4 = ddlNivel4.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? Convert.ToInt32(ddlNivel4.SelectedValue) : 0;
                ucAltaNivelArbol.IdNivel5 = ddlNivel5.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? Convert.ToInt32(ddlNivel5.SelectedValue) : 0;
                ucAltaNivelArbol.IdNivel6 = ddlNivel6.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? Convert.ToInt32(ddlNivel6.SelectedValue) : 0;
                ucAltaNivelArbol.IdNivel7 = ddlNivel7.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? Convert.ToInt32(ddlNivel7.SelectedValue) : 0;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#editNivel\");", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region Seleccion Arbol
        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlArea);
                Metodos.LimpiarCombo(ddlTipoArbol);
                Metodos.LimpiarCombo(ddlNivel1);
                Metodos.LimpiarCombo(ddlNivel2);
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                btnAddArea.Enabled = false;
                btnAgregarNivel1.Enabled = false;
                btnAgregarNivel2.Enabled = false;
                btnAgregarNivel3.Enabled = false;
                btnAgregarNivel4.Enabled = false;
                btnAgregarNivel5.Enabled = false;
                btnAgregarNivel6.Enabled = false;
                btnAgregarNivel7.Enabled = false;
                if (ddlTipoUsuario.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    Metodos.LlenaComboCatalogo(ddlArea, _servicioAreas.ObtenerAreas(true));
                    btnAddArea.Enabled = true;
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
                btnAgregarNivel1.Enabled = false;
                btnAgregarNivel2.Enabled = false;
                btnAgregarNivel3.Enabled = false;
                btnAgregarNivel4.Enabled = false;
                btnAgregarNivel5.Enabled = false;
                btnAgregarNivel6.Enabled = false;
                btnAgregarNivel7.Enabled = false;
                if (ddlArea.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione) return;
                Metodos.LlenaComboCatalogo(ddlTipoArbol, _servicioSistemaTipoArbol.ObtenerTiposArbolAcceso(true));
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
                int idTipoArbol = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                Metodos.LlenaComboCatalogo(ddlNivel1, _servicioArbolAcceso.ObtenerNivel1(int.Parse(ddlArea.SelectedValue), idTipoArbol, idTipoUsuario, true));
                if (ddlTipoArbol.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    btnAgregarNivel1.Enabled = true;
                    btnAgregarNivel2.Enabled = false;
                    btnAgregarNivel3.Enabled = false;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
                }
                else
                {
                    btnAgregarNivel1.Enabled = false;
                    btnAgregarNivel2.Enabled = false;
                    btnAgregarNivel3.Enabled = false;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
                }
                ucAltaNivelArbol.IdTipoArbol = int.Parse(ddlTipoArbol.SelectedValue);

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
                Metodos.LimpiarCombo(ddlNivel2);
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                int idTipoArbol = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                int idNivelFiltro = Convert.ToInt32(ddlNivel1.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminal(idTipoUsuario, idTipoArbol, idNivelFiltro, null, null, null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel1, ddlNivel2, _servicioArbolAcceso.ObtenerNivel2(int.Parse(ddlArea.SelectedValue), idTipoArbol, idTipoUsuario, idNivelFiltro, true));
                    btnAgregarNivel2.Enabled = true;
                    btnAgregarNivel3.Enabled = false;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
                }
                else
                {
                    btnAgregarNivel2.Enabled = false;
                    btnAgregarNivel3.Enabled = false;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
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
                Metodos.LimpiarCombo(ddlNivel3);
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                int idTipoArbol = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                int idNivelFiltro = Convert.ToInt32(ddlNivel2.SelectedValue);

                if (!_servicioArbolAcceso.EsNodoTerminal(idTipoUsuario, idTipoArbol, Convert.ToInt32(ddlNivel1.SelectedValue), idNivelFiltro, null, null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel2, ddlNivel3, _servicioArbolAcceso.ObtenerNivel3(int.Parse(ddlArea.SelectedValue), idTipoArbol, idTipoUsuario, idNivelFiltro, true));
                    btnAgregarNivel3.Enabled = true;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
                }
                else
                {
                    btnAgregarNivel3.Enabled = false;
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
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
                Metodos.LimpiarCombo(ddlNivel4);
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                int idTipoArbol = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                int idNivelFiltro = Convert.ToInt32(ddlNivel3.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminal(idTipoUsuario, idTipoArbol, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), null, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel3, ddlNivel4, _servicioArbolAcceso.ObtenerNivel4(int.Parse(ddlArea.SelectedValue), idTipoArbol, idTipoUsuario, idNivelFiltro, true));
                    btnAgregarNivel4.Enabled = true;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
                }
                else
                {
                    btnAgregarNivel4.Enabled = false;
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
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
                Metodos.LimpiarCombo(ddlNivel5);
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                int idTipoArbol = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                int idNivelFiltro = Convert.ToInt32(ddlNivel4.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminal(idTipoUsuario, idTipoArbol, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), idNivelFiltro, null, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel4, ddlNivel5, _servicioArbolAcceso.ObtenerNivel5(int.Parse(ddlArea.SelectedValue), idTipoArbol, idTipoUsuario, idNivelFiltro, true));
                    btnAgregarNivel5.Enabled = true;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
                }
                else
                {
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
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
                Metodos.LimpiarCombo(ddlNivel6);
                Metodos.LimpiarCombo(ddlNivel7);
                int idTipoArbol = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                int idNivelFiltro = Convert.ToInt32(ddlNivel5.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminal(idTipoUsuario, idTipoArbol, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), idNivelFiltro, null, null))
                {
                    Metodos.FiltraCombo(ddlNivel5, ddlNivel6, _servicioArbolAcceso.ObtenerNivel6(int.Parse(ddlArea.SelectedValue), idTipoArbol, idTipoUsuario, idNivelFiltro, true));
                    btnAgregarNivel6.Enabled = true;
                    btnAgregarNivel7.Enabled = false;
                }
                else
                {
                    btnAgregarNivel5.Enabled = false;
                    btnAgregarNivel6.Enabled = false;
                    btnAgregarNivel7.Enabled = false;
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
                Metodos.LimpiarCombo(ddlNivel7);
                int idTipoArbol = Convert.ToInt32(ddlTipoArbol.SelectedValue);
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                int idNivelFiltro = Convert.ToInt32(ddlNivel6.SelectedValue);
                if (!_servicioArbolAcceso.EsNodoTerminal(idTipoUsuario, idTipoArbol, Convert.ToInt32(ddlNivel1.SelectedValue), Convert.ToInt32(ddlNivel2.SelectedValue), Convert.ToInt32(ddlNivel3.SelectedValue), Convert.ToInt32(ddlNivel4.SelectedValue), Convert.ToInt32(ddlNivel5.SelectedValue), idNivelFiltro, null))
                {
                    Metodos.FiltraCombo(ddlNivel6, ddlNivel7, _servicioArbolAcceso.ObtenerNivel7(int.Parse(ddlArea.SelectedValue), idTipoArbol, idTipoUsuario, idNivelFiltro, true));
                    btnAgregarNivel7.Enabled = true;
                }
                else
                {
                    btnAgregarNivel7.Enabled = false;
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
        #endregion Seleccion Arbol
        #endregion Eventos#endregion Eventos

        #region Abre modales Maestros
        protected void btnAddArea_OnClick(object sender, EventArgs e)
        {
            try
            {
                AltaAreas.EsAlta = true;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAreas\");", true);
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

        #endregion Abre modales Maestros
    }
}

