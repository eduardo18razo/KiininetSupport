using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaUsuarios : UserControl
    {
        readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private List<string> _lstError = new List<string>();

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
                //if (lstTipoUsuario.Count >= 2)
                //    lstTipoUsuario.Insert(BusinessVariables.ComboBoxCatalogo.IndexTodos, new TipoUsuario { Id = BusinessVariables.ComboBoxCatalogo.ValueTodos, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionTodos });
                Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaUsuarios()
        {
            try
            {
                int? idTipoUsuario = null;

                LimpiarUsuarios();
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);
                }
                List<Usuario> lstUsuarios = _servicioUsuarios.ObtenerUsuarios(idTipoUsuario);
                
                if (txtFiltro.Text.Trim() != string.Empty)
                    lstUsuarios = lstUsuarios.Where(w => w.ApellidoPaterno.ToLower().Contains(txtFiltro.Text.ToLower().Trim()) || w.ApellidoPaterno.ToLower().Contains(txtFiltro.Text.ToLower().Trim()) || w.Nombre.ToLower().Contains(txtFiltro.Text.ToLower().Trim()) || w.NombreCompleto.ToLower().Contains(txtFiltro.Text.ToLower().Trim())).ToList();
                tblResults.DataSource = lstUsuarios;
                tblResults.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UcDetalleUsuario1.FromModal = true;
                UcDetalleUsuario1.OnCancelarModal += UcDetalleUsuario1OnOnCancelarModal;

                //ucAltaUsuarioMoral.OnAceptarModal += UcAltaUsuarioMoral_OnAceptarModal;
                //ucAltaUsuarioMoral.OnCancelarModal += UcAltaUsuarioMoral_OnCancelarModal;

                //ucAltaUsuarioFisico.OnAceptarModal += UcAltaUsuarioFisico_OnAceptarModal;
                //ucAltaUsuarioFisico.OnCancelarModal += UcAltaUsuarioFisico_OnCancelarModal;

                if (!IsPostBack)
                {
                    LlenaCombos();
                    LlenaUsuarios();
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

        void UcAltaUsuarioMoral_OnAceptarModal()
        {
            try
            {
                LlenaUsuarios();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalPersonaMoral\");", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        void UcAltaUsuarioMoral_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalPersonaMoral\");", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void UcAltaUsuarioFisico_OnAceptarModal()
        {
            try
            {
                LlenaUsuarios();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalPersonaFisica\");", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void UcAltaUsuarioFisico_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalPersonaFisica\");", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }
        }

        protected void btnUsuario_OnClick(object sender, EventArgs e)
        {
            try
            {

                UcDetalleUsuario1.IdUsuario = Convert.ToInt32(((LinkButton)sender).CommandArgument);

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleUsuario\");", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnEditar_OnClick(object sender, EventArgs e)
        {
            try
            {
                // TODO: eDICION DE USUARIO
                Response.Redirect("~/Users/Administracion/Usuarios/FrmEdicionUsuario.aspx?Consulta=true&IdUsuario=" + ((ImageButton)sender).CommandArgument);
                //Usuario user = _servicioUsuarios.ObtenerDetalleUsuario(int.Parse(hfId.Value));
                //if (user == null) return;
                ////if (user.TipoUsuario.EsMoral)
                ////{
                //ucAltaUsuarioMoral.IdUsuario = user.Id;
                //ucAltaUsuarioMoral.Alta = false;
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalPersonaMoral\");", true);
                ////}
                ////else
                ////{
                ////    ucAltaUsuarioFisico.IdUsuario = user.Id;
                ////    ucAltaUsuarioFisico.Alta = false;
                ////    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalPersonaFisica\");", true);
                ////}
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
                //TODO: ALTA USUARIO
                Response.Redirect("~/Users/Administracion/Usuarios/FrmEdicionUsuario.aspx?Consulta=true");
                ////if (_servicioSistemaTipoUsuario.ObtenerTipoUsuarioById(int.Parse(ddlTipoUsuario.SelectedValue)).EsMoral)
                ////{
                //    ucAltaUsuarioMoral.Alta = true;
                //    ucAltaUsuarioMoral.IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);
                //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalPersonaMoral\");", true);
                ////}
                ////else
                ////{
                ////    ucAltaUsuarioFisico.Alta = true;
                ////    ucAltaUsuarioFisico.IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);
                ////    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalPersonaFisica\");", true);
                ////}


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

        private void LimpiarUsuarios()
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

        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaUsuarios();
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
                LlenaUsuarios();
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
                _servicioUsuarios.HabilitarUsuario(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked, ResolveUrl("~/ConfirmacionCuenta.aspx"));
                LlenaUsuarios();
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
            LlenaUsuarios();
        }

        #endregion 
        
    }
}