using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniMaster.ServiceSistemaRoles;
using KiiniMaster.ServiceSistemaPoliticas;
using KiiniMaster.ServiceSistemaSubRol;
using KiiniMaster.ServiceSistemaEstatus;


using KinniNet.Business.Utils;
using KiiniNet.Entities.Parametros;


namespace KiiniMaster.Configuracion.Politicas
{
    public partial class FrmPoliticasEstatusTicketDefault : System.Web.UI.Page
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
                _servicePoliticasAsignacion.HabilitarEstatusTicketSubRolGeneralDefault(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
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
            
        }
        protected void ddlTipoRol_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                ddlSubRolSolicita.DataSource = null;
                ddlSubRolSolicita.DataSource = _servicioSubRoles.ObtenerSubRolesByTipoGrupo(int.Parse(ddlRolSolicita.SelectedValue), true);
                ddlSubRolSolicita.DataTextField = "Descripcion";
                ddlSubRolSolicita.DataValueField = "Id";
                ddlSubRolSolicita.DataBind();
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

        protected void ddlRolPertenece_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                ddlSubRolPertenece.DataSource = _servicioSubRoles.ObtenerSubRolesByTipoGrupo(int.Parse(ddlRolPertenece.SelectedValue), true);
                ddlSubRolPertenece.DataTextField = "Descripcion";
                ddlSubRolPertenece.DataValueField = "Id";
                ddlSubRolPertenece.DataBind();

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

        protected void ddlSubRolPertenece_SelectedIndexChanged(object sender, EventArgs e)
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
                List<EstatusTicketSubRolGeneralDefault> lstResult = _servicePoliticasAsignacion.ObtenerEstatusTicketSubRolGeneralDefault();
               
                int? idRolSolicita = ddlRolSolicita.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlRolSolicita.SelectedValue);
                int? idSubRolSolicita = ddlSubRolSolicita.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlSubRolSolicita.SelectedValue);

                int? idRolPertenece = ddlRolPertenece.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlRolPertenece.SelectedValue);
                int? idSubRolPertenece = ddlSubRolPertenece.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlSubRolPertenece.SelectedValue);
                
                int? idEstatusActual = ddlEstatusActual.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlEstatusActual.SelectedValue);
                int? idEstatusAccion = ddlEstatusAccion.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlEstatusAccion.SelectedValue);
                
                if (idRolSolicita != null)
                {
                    lstResult = lstResult.Where(w => w.IdRolSolicita == idRolSolicita).ToList();
                }
                if (idSubRolSolicita != null)
                {
                    lstResult = lstResult.Where(w => w.IdSubRolSolicita == idSubRolSolicita).ToList();
                }

                if (idRolPertenece != null)
                {
                    lstResult = lstResult.Where(w => w.IdRolPertenece == idRolPertenece).ToList();
                }
                if (idSubRolPertenece != null)
                {
                    lstResult = lstResult.Where(w => w.IdSubRolPertenece == idSubRolPertenece).ToList();
                }

                if (idEstatusActual != null)
                {
                    lstResult = lstResult.Where(w => w.IdEstatusTicketActual == idEstatusActual).ToList();
                }
                if (idEstatusAccion != null)
                {
                    lstResult = lstResult.Where(w => w.IdEstatusTicketAccion == idEstatusAccion).ToList();
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
                rolespermitidos.Add((int)BusinessVariables.EnumRoles.AccesoCentroSoporte);

                
                ddlRolSolicita.DataSource = _servicioRoles.ObtenerRoles((int)BusinessVariables.EnumTiposUsuario.Agente, true).Where(w => rolespermitidos.Contains(w.Id));
                ddlRolSolicita.DataTextField = "Descripcion";
                ddlRolSolicita.DataValueField = "Id";
                ddlRolSolicita.DataBind();

                ddlRolPertenece.DataSource = _servicioRoles.ObtenerRoles((int)BusinessVariables.EnumTiposUsuario.Agente, true).Where(w => rolespermitidos.Contains(w.Id));
                ddlRolPertenece.DataTextField = "Descripcion";
                ddlRolPertenece.DataValueField = "Id";
                ddlRolPertenece.DataBind();

                ddlEstatusActual.DataSource = _servicioEstatus.ObtenerEstatusTicket(true);
                ddlEstatusActual.DataTextField = "Descripcion";
                ddlEstatusActual.DataValueField = "Id";
                ddlEstatusActual.DataBind();

                ddlEstatusAccion.DataSource = _servicioEstatus.ObtenerEstatusTicket(true);
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
                ddlRolSolicita.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddlRolPertenece.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddlSubRolSolicita.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddlSubRolPertenece.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
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
                    CargaCombos();
                    CargaDatos();
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