﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaArboles : UserControl
    {
        #region Variables
        readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
        readonly ServiceAreaClient _servicioAreas = new ServiceAreaClient();

        private List<string> _lstError = new List<string>();
        #endregion Variables

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

        private void LlenaCombos()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlArea, _servicioAreas.ObtenerAreas(true));
                Metodos.LlenaComboCatalogo(ddlTipoUsuario, _servicioSistemaTipoUsuario.ObtenerTiposUsuario(true));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaArboles()
        {
            try
            {
                int? idArea = null;
                int? idTipoUsuario = null;

                if (ddlArea.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idArea = int.Parse(ddlArea.SelectedValue);

                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);
                string filtro = txtFiltro.Text.ToLower().Trim();
                List<ArbolAcceso> lstArboles = _servicioArbolAcceso.ObtenerArbolesAccesoAll(idArea, idTipoUsuario, null, null, null, null, null, null, null, null).Where(w=>w.EsTerminal).ToList();
                if (filtro != string.Empty)
                    lstArboles = lstArboles.Where(w => w.Tipificacion.ToLower().Contains(filtro)).ToList();

                tblResults.DataSource = lstArboles;
                tblResults.DataBind();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
            UcAltaAbrolAcceso.OnCancelarModal += UcAltaAbrolAccesoOnOnCancelarModal;
            if (!IsPostBack)
            {
                LlenaCombos();
                LlenaArboles();
            }
        }

        private void UcAltaAbrolAccesoOnOnCancelarModal()
        {
            try
            {
                UcAltaAbrolAcceso.Cancelar();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAtaOpcion\");", true);
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

        protected void ddlArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaArboles();
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
                LlenaArboles();
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
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAtaOpcion\");", true);
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
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAtaOpcion\");", true);
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
                LlenaArboles();
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
                _servicioArbolAcceso.HabilitarArbol(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
                LlenaArboles();
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
            LlenaArboles();
        }

        #endregion 
    }
}