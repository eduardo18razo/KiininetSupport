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
    public partial class FrmPoliticasEscalacionPermitida : System.Web.UI.Page
    {        
            private readonly ServicePoliticasClient _servicePoliticas = new ServicePoliticasClient();
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
           
            protected void OnCheckedChanged(object sender, EventArgs e)
            {
                try
                {
                    _servicioSubRoles.HabilitarPoliticaEscalacion(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
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
                    //ddlSubRol.DataSource = _servicioSubRoles.ObtenerSubRolesByTipoGrupo(int.Parse(ddlRol.SelectedValue), true);
                    //ddlSubRol.DataTextField = "Descripcion";
                    //ddlSubRol.DataValueField = "Id";
                    //ddlSubRol.DataBind();
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

            protected void ddlSubRolPermitido_SelectedIndexChanged(object sender, EventArgs e)
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

           private void CargaDatos()
            {
                try
                {
                    List<SubRolEscalacionPermitida> lstResult = _servicioSubRoles.ObtenerSubRolEscalacionPermitida();
                    int? idSubRol = ddlSubRol.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlSubRol.SelectedValue);
                    int? idSubRolPermitido = ddlSubRolPermitido.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlSubRol.SelectedValue);
                    int? idEstatusAsignacion = ddlEstatusAsignacion.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? null : (int?)int.Parse(ddlEstatusAsignacion.SelectedValue);
                    
                    if (idSubRol != null)
                    {
                        lstResult = lstResult.Where(w => w.IdSubRol == idSubRol).ToList();
                    }
                    if (idSubRolPermitido != null)
                    {
                        lstResult = lstResult.Where(w => w.IdSubRolPermitido == idSubRolPermitido).ToList();
                    }
                    if (idEstatusAsignacion != null)
                    {
                        lstResult = lstResult.Where(w => w.IdEstatusAsignacion == idEstatusAsignacion).ToList();
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


                    ddlSubRol.DataSource = _servicioSubRoles.ObtenerSubRolesByTipoGrupo((int)BusinessVariables.EnumRoles.Agente, true);
                    ddlSubRol.DataTextField = "Descripcion";
                    ddlSubRol.DataValueField = "Id";
                    ddlSubRol.DataBind();

                    ddlSubRolPermitido.DataSource = _servicioSubRoles.ObtenerSubRolesByTipoGrupo((int)BusinessVariables.EnumRoles.Agente, true);
                    ddlSubRolPermitido.DataTextField = "Descripcion";
                    ddlSubRolPermitido.DataValueField = "Id";
                    ddlSubRolPermitido.DataBind();

                    ddlEstatusAsignacion.DataSource = _servicioEstatus.ObtenerEstatusAsignacion(true);
                    ddlEstatusAsignacion.DataTextField = "Descripcion";
                    ddlEstatusAsignacion.DataValueField = "Id";
                    ddlEstatusAsignacion.DataBind();
                    
                    
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
                    ddlSubRol.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
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