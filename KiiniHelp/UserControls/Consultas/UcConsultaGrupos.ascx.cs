using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceSistemaTipoGrupo;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaGrupos : UserControl, IControllerModal
    {
        readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        readonly ServiceTipoGrupoClient _servicioTipoGrupo = new ServiceTipoGrupoClient();
        readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();

        private List<string> _lstError = new List<string>();

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        public int IdTipoUsuario
        {
            get { return Convert.ToInt32(ddlTipoUsuario.SelectedValue); }
            set { ddlTipoUsuario.SelectedValue = value.ToString(); }
        }

        public List<string> Alerta
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

        private void LlenaCombos()
        {
            try
            {
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true);
                if (lstTipoUsuario.Count >= 2)
                    lstTipoUsuario.Insert(BusinessVariables.ComboBoxCatalogo.IndexTodos, new TipoUsuario { Id = BusinessVariables.ComboBoxCatalogo.ValueTodos, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionTodos });
                Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LimpiarGrupos()
        {
            try
            {
                tblResults.DataSource = null;
                tblResults.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        private void LlenaGrupos()
        {
            try
            {
                int? idTipoUsuario = null;
                int? idTipoGrupo = null;
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    LimpiarGrupos();
                    return;
                }
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos)
                    idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);

                if (ddlTipoGrupo.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idTipoGrupo = int.Parse(ddlTipoGrupo.SelectedValue);

                string filtro = txtFiltro.Text.ToLower().Trim();
                List<GrupoUsuario> gpos = _servicioGrupoUsuario.ObtenerGruposUsuarioAll(idTipoUsuario, idTipoGrupo);
                if (filtro != string.Empty)
                    gpos = gpos.Where(w => w.Descripcion.ToLower().Contains(filtro)).ToList();
                //gpos = gpos.Where(w => w.Descripcion.ToUpper().Contains(filtro)).ToList();

                tblResults.DataSource = gpos;
                tblResults.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void FiltraCombo(DropDownList ddlFiltro, DropDownList ddlLlenar, object source)
        {
            try
            {
                ddlLlenar.Items.Clear();
                if (ddlFiltro.SelectedValue != BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString())
                {
                    ddlLlenar.Enabled = true;
                    Metodos.LlenaComboCatalogo(ddlLlenar, source);
                }
                else
                {
                    ddlLlenar.DataSource = null;
                    ddlLlenar.DataBind();
                }

                ddlLlenar.Enabled = ddlLlenar.DataSource != null;

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
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    LlenaCombos();
                }
                ucAltaGrupoUsuario.FromOpcion = false;
                ucAltaGrupoUsuario.OnAceptarModal += UcAltaGrupoUsuarioOnOnAceptarModal;
                ucAltaGrupoUsuario.OnCancelarModal += UcAltaGrupoUsuarioOnOnCancelarModal;

                ucDetalleGrupoUsuarios.OnCancelarModal += ucDetalleGrupo_OnCancelarModal;
                ucDetalleGrupoOpciones.OnCancelarModal += ucDetalleGrupoOpciones_OnCancelarModal;

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

        void ucDetalleGrupoOpciones_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleGrupoOpciones\");", true);
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

        void ucDetalleGrupo_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleGrupoUsuario\");", true);
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

        private void UcAltaGrupoUsuarioOnOnCancelarModal()
        {
            try
            {
                LlenaGrupos();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaGrupoUsuarios\");", true);
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

        private void UcAltaGrupoUsuarioOnOnAceptarModal()
        {
            try
            {
                LlenaGrupos();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaGrupoUsuarios\");", true);
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

        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlTipoGrupo);
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos)
                {
                    Metodos.LimpiarCombo(ddlTipoGrupo);
                    FiltraCombo(ddlTipoUsuario, ddlTipoGrupo, _servicioTipoGrupo.ObtenerTiposGruposByTipoUsuario(int.Parse(ddlTipoUsuario.SelectedValue), true));
                }
                LlenaGrupos();
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

        protected void ddlTipoGrupo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaGrupos();
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

        protected void btnEditar_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ucAltaGrupoUsuario.GrupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(((Button)sender).CommandArgument));
                ucAltaGrupoUsuario.GrupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(int.Parse(((ImageButton)sender).CommandArgument));
                ucAltaGrupoUsuario.Alta = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaGrupoUsuarios\");", true);
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
        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucAltaGrupoUsuario.Alta = true;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaGrupoUsuarios\");", true);
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

        protected void chklbxSubRoles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int value = 0;
                string result = Request.Form["__EVENTTARGET"];
                string[] checkedBox = result.Split('$');
                int index = int.Parse(checkedBox[checkedBox.Length - 1]);
                if (chklbxSubRoles.Items[index].Selected)
                {
                    value = Convert.ToInt32(chklbxSubRoles.Items[index].Value);
                }
                switch (int.Parse(hfOperacion.Value))
                {
                    case (int)BusinessVariables.EnumRoles.Agente:
                        bool permitir = chklbxSubRoles.Items.Cast<ListItem>().Any(item => int.Parse(item.Value) == (int)BusinessVariables.EnumSubRoles.Supervisor && item.Selected);
                        if (!permitir)
                            foreach (ListItem item in chklbxSubRoles.Items)
                            {
                                item.Selected = int.Parse(item.Value) == value;
                            }
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeContenido:
                        foreach (ListItem item in chklbxSubRoles.Items)
                        {
                            item.Selected = int.Parse(item.Value) == value;
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

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LlenaGrupos();
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

        protected void OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _servicioGrupoUsuario.HabilitarGrupo(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
                LlenaGrupos();
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

        #region Paginador
        protected void gvPaginacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            tblResults.PageIndex = e.NewPageIndex;
            LlenaGrupos();
        }

        #endregion

        protected void lnkBtnDetalleUsuario_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucDetalleGrupoUsuarios.IdGrupo = int.Parse(((LinkButton)sender).CommandArgument);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleGrupoUsuario\");", true);
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

        protected void lnkBtnDetalleOpciones_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucDetalleGrupoOpciones.IdGrupo = int.Parse(((LinkButton)sender).CommandArgument);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleGrupoOpciones\");", true);
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