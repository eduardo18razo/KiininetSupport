
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KinniNet.Business.Utils;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Parametros;
using KiiniMaster.ServiceSistemaPoliticas;
using KiiniMaster.ServiceSistemaRoles;
using KiiniMaster.ServiceSistemaSubRol;
using KiiniMaster.ServiceSistemaEstatus;
using KiiniMaster.ServiceGrupoUsuario;


namespace KiiniMaster.Configuracion.Politicas
{
    //JGB
    public partial class FrmPoliticasAsignacionGrupo : System.Web.UI.Page
    {
        private readonly ServicePoliticasClient _servicePoliticasAsignacion = new ServicePoliticasClient();
        private readonly ServiceRolesClient _servicioRoles = new ServiceRolesClient();
        private readonly ServiceSubRolClient _servicioSubRoles = new ServiceSubRolClient();
        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();  

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
       
        protected void btnEditar_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ucAltaArea.EsAlta = false;
                //Area puesto = _servicioAreas.ObtenerAreaById(int.Parse(((Button)sender).CommandArgument));
                //if (puesto == null) return;
                //ucAltaArea.IdArea = puesto.Id;
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaArea\");", true);
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
                _servicePoliticasAsignacion.HabilitarEstatusAsignacionSubRolGeneral(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
                CargaDatos();
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
            //try
            //{
            //    int? idTipoUsuario = null;
            //    if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos)
            //        idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);

            //    rptResultados.DataSource = _servicioOrganizacion.BuscarPorPalabra(idTipoUsuario, null, null, null, null, null, null, null, txtFiltroDecripcion.Text.Trim());
            //    rptResultados.DataBind();
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    AlertaOrganizacion = _lstError;
            //}
        }
        //protected void ddlTipoRol_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ddlSubRol.DataSource = _servicioSubRoles.ObtenerSubRolesByTipoGrupo(int.Parse(ddlGrupoUsuario.SelectedValue), true);
        //        ddlSubRol.DataTextField = "Descripcion";
        //        ddlSubRol.DataValueField = "Id";
        //        ddlSubRol.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_lstError == null)
        //        {
        //            _lstError = new List<string>();
        //        }
        //        _lstError.Add(ex.Message);
        //        Alerta = _lstError;
        //    }
        //}

        protected void ddlGrupoUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSubRol.DataSource = _servicioSubRoles.ObtenerSubRolesByTipoGrupo((int)BusinessVariables.EnumTiposGrupos.Agente, true);
                ddlSubRol.DataTextField = "Descripcion";
                ddlSubRol.DataValueField = "Id";
                ddlSubRol.DataBind();
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

        protected void ddlSubRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaDatos();
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

        protected void ddlEstatusActual_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaDatos();
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

        protected void ddlEstatusAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                CargaDatos();
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

        private void CargaDatos()
        {
            try
            {
                List<EstatusAsignacionSubRolGeneral> lstResult = _servicePoliticasAsignacion.ObtenerEstatusAsignacionSubRolGeneral();
                int? idGrupo = ddlGrupoUsuario.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlGrupoUsuario.SelectedValue);
                int? idSubRol = ddlSubRol.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlSubRol.SelectedValue);
                int? idEstatusActual = ddlEstatusActual.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlEstatusActual.SelectedValue);
                int? idEstatusAccion = ddlEstatusAccion.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlEstatusAccion.SelectedValue);
                if (idGrupo != null)
                {
                    lstResult = lstResult.Where(w => w.IdGrupoUsuario == idGrupo).ToList();
                }
                if (idSubRol != null)
                {
                    lstResult = lstResult.Where(w => w.IdSubRol == idSubRol).ToList();
                }
                if (idEstatusActual != null)
                {
                    lstResult = lstResult.Where(w => w.IdEstatusAsignacionActual == idEstatusActual).ToList();
                }
                if (idEstatusAccion != null)
                {
                    lstResult = lstResult.Where(w => w.IdEstatusAsignacionAccion == idEstatusAccion).ToList();
                }
                rptResultados.DataSource = lstResult;
                rptResultados.DataBind();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CargaCombos()
        {
            try
            {                

                List<int> tiposGruposPermitidos = new List<int>();
                tiposGruposPermitidos.Add(BusinessVariables.ComboBoxCatalogo.ValueSeleccione);
                tiposGruposPermitidos.Add((int)BusinessVariables.EnumTiposGrupos.Agente);


               // ddlGrupoUsuario.DataSource = _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario((int)BusinessVariables.EnumTiposGrupos.ResponsableDeAtención, (int)BusinessVariables.EnumTiposUsuario.Empleado, true);
                ddlGrupoUsuario.DataSource = _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol((int)BusinessVariables.EnumTiposGrupos.Agente, true);
                ddlGrupoUsuario.DataTextField = "Descripcion";
                ddlGrupoUsuario.DataValueField = "Id";
                ddlGrupoUsuario.DataBind();

                ddlEstatusActual.DataSource = _servicioEstatus.ObtenerEstatusAsignacion(true);
                ddlEstatusActual.DataTextField = "Descripcion";
                ddlEstatusActual.DataValueField = "Id";
                ddlEstatusActual.DataBind();

                ddlEstatusAccion.DataSource = _servicioEstatus.ObtenerEstatusAsignacion(true);
                ddlEstatusAccion.DataTextField = "Descripcion";
                ddlEstatusAccion.DataValueField = "Id";
                ddlEstatusAccion.DataBind();

                LimpiarSeleccion();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LimpiarSeleccion()
        {
            try
            {
                ddlGrupoUsuario.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
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
                    LimpiarSeleccion();
                    CargaDatos();
                    CargaCombos();
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

        
       
    }

}