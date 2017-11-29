
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KinniNet.Business.Utils;
using KiiniNet.Entities.Parametros;
using KiiniMaster.ServiceSistemaPoliticas;
using KiiniMaster.ServiceSistemaRoles;
using KiiniMaster.ServiceSistemaSubRol;
using KiiniMaster.ServiceSistemaEstatus;


namespace KiiniMaster.Configuracion.Politicas
{
    public partial class FrmPoliticasAsignacionDefault : System.Web.UI.Page
    {
        private readonly ServicePoliticasClient _servicePoliticasAsignacion = new ServicePoliticasClient();
        private readonly ServiceRolesClient _servicioRoles = new ServiceRolesClient();
        private readonly ServiceSubRolClient _servicioSubRoles = new ServiceSubRolClient();
        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();

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
                _servicePoliticasAsignacion.HabilitarEstatusAsignacionSubRolGeneralDefault(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
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
        protected void ddlTipoRol_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSubRol.DataSource = _servicioSubRoles.ObtenerSubRolesByTipoGrupo(int.Parse(ddlRol.SelectedValue), true);
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
                List<EstatusAsignacionSubRolGeneralDefault> lstResult = _servicePoliticasAsignacion.ObtenerEstatusAsignacionSubRolGeneralDefault();
                int? idSubRol = ddlSubRol.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlSubRol.SelectedValue);
                int? idEstatusActual = ddlEstatusActual.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlEstatusActual.SelectedValue);
                int? idEstatusAccion = ddlEstatusAccion.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlEstatusAccion.SelectedValue);
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

                //ddlRol.Items.Add(new
                //ListItem
                //{
                //    Value = BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString(),
                //    Text = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
                //});
                //ddlRol.Items.Add(new
                //ListItem
                //{
                //    Value = ((int)BusinessVariables.EnumRoles.ResponsableDeAtención).ToString(),
                //    Text = BusinessVariables.EnumRoles.ResponsableDeAtención.ToString()
                //});
                List<int> rolespermitidos = new List<int>();
                rolespermitidos.Add(BusinessVariables.ComboBoxCatalogo.ValueSeleccione);
                rolespermitidos.Add((int)BusinessVariables.EnumRoles.Agente);
                ddlRol.DataSource = _servicioRoles.ObtenerRoles((int)BusinessVariables.EnumTiposUsuario.Operador, true).Where(w => rolespermitidos.Contains(w.Id));
                ddlRol.DataTextField = "Descripcion";
                ddlRol.DataValueField = "Id";
                ddlRol.DataBind();

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
                ddlRol.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
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