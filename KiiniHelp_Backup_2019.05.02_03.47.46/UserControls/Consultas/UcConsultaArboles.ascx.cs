using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KinniNet.Business.Utils;
using System.IO;

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

        bool _sorted = false;
        private string GridViewSortDirection
        {
            get
            {
                if (ViewState["SortDirection"] == null)
                {
                    ViewState["SortDirection"] = "ASC";
                }
                return ViewState["SortDirection"].ToString();
            }

            set
            {
                ViewState["SortDirection"] = value;
            }

        }
        private string GridViewSortExpression
        {
            get
            {
                return ViewState["SortExpression"] as string ?? string.Empty;
            }

            set
            {
                ViewState["SortExpression"] = value;
            }

        }

        protected List<ArbolAcceso> SortList(List<ArbolAcceso> data, bool isPageIndexChanging)
        {
            List<ArbolAcceso> result = data;
            if (data != null)
            {
                if (chkActivos.Checked)
                    data = data.Where(w => w.Habilitado).ToList();
                if (GridViewSortExpression != string.Empty)
                {
                    if (data.Count > 0)
                    {
                        PropertyInfo[] propertys = data[0].GetType().GetProperties();
                        foreach (PropertyInfo p in propertys)
                        {
                            if (p.Name == GridViewSortExpression)
                            {
                                if (GridViewSortDirection == "ASC")
                                {
                                    if (isPageIndexChanging)
                                    {
                                        result = data.OrderByDescending(key => p.GetValue(key, null)).ToList();
                                    }
                                    else
                                    {
                                        result = data.OrderBy(key =>
                                            p.GetValue(key, null)).ToList();
                                        GridViewSortDirection = "DESC";
                                    }
                                }
                                else
                                {
                                    if (isPageIndexChanging)
                                    {
                                        result = data.OrderBy(key =>
                                            p.GetValue(key, null)).ToList();
                                    }
                                    else
                                    {
                                        result = data.OrderByDescending(key => p.GetValue(key, null)).ToList();
                                        GridViewSortDirection = "ASC";
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    result = data.OrderBy(o => o.Descripcion).ToList();
                }
            }
            _sorted = true;
            return result;
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

        private void LlenaArboles(bool isPageIndexChanging)
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
                List<ArbolAcceso> lstArboles = _servicioArbolAcceso.ObtenerArbolesAccesoAll(idArea, idTipoUsuario, null, null, null, null, null, null, null, null).Where(w => w.EsTerminal).ToList();
                if (filtro != string.Empty)
                    lstArboles = lstArboles.Where(w => w.Tipificacion.ToLower().Contains(filtro)).ToList();
                if (chkActivos.Checked)
                    lstArboles = lstArboles.Where(w => w.Habilitado).ToList();
                tblResults.DataSource = SortList(lstArboles, isPageIndexChanging);
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
            try
            {
                ucDetalleArbolAcceso.OnCancelarModal += ucDetalleArbolAcceso_OnCancelarModal;
                if (!IsPostBack)
                {
                    LlenaCombos();
                    LlenaArboles(false);
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

        void ucDetalleArbolAcceso_OnCancelarModal()
        {
            try
            {
                LlenaArboles(false);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleOpciones\");", true);
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
                LlenaArboles(false);
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
                LlenaArboles(false);
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
                int tipoArbol = int.Parse(((LinkButton)sender).CommandName);
                if (tipoArbol == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                    Response.Redirect("~/Users/Administracion/ArbolesAcceso/FrmEdicionOpcionConsulta.aspx?IdArbolAccesoConsulta=" + ((LinkButton)sender).CommandArgument);
                else
                    Response.Redirect("~/Users/Administracion/ArbolesAcceso/FrmEdicionOpcionServicio.aspx?IdArbolAccesoServicioProblema=" + ((LinkButton)sender).CommandArgument);
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
                LlenaArboles(false);
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
                LlenaArboles(false);
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
            LlenaArboles(true);
        }

        #endregion

        protected void lnkBtnDetalleOpciones_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucDetalleArbolAcceso.IdArbolAcceso = int.Parse(((LinkButton)sender).CommandArgument);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleOpciones\");", true);
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

        protected void btnDownload_OnClick(object sender, EventArgs e)
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
                List<ArbolAcceso> lstArboles = _servicioArbolAcceso.ObtenerArbolesAccesoAll(idArea, idTipoUsuario, null, null, null, null, null, null, null, null).Where(w => w.EsTerminal).ToList();
                if (filtro != string.Empty)
                    lstArboles = lstArboles.Where(w => w.Tipificacion.ToLower().Contains(filtro)).ToList();
                Response.Clear();

                MemoryStream ms = new MemoryStream(BusinessFile.ExcelManager.ListToExcel(lstArboles.Select(
                                s => new
                                {
                                    TipoUsuario = s.TipoUsuario.Descripcion,
                                    Título = s.Tipificacion,
                                    Categoría = s.Area.Descripcion,
                                    Tipificación = s.TipoArbolAcceso.Descripcion,
                                    Tipo = s.EsTerminal ? "Opción" : "Sección",
                                    Nivel = s.Nivel
                                })
                                .ToList()).GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=ConfiguraciónMenu.xlsx");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();

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

        protected void chkActivos_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaArboles(false);
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

        protected void tblResults_OnSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                LlenaArboles(true);
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

        protected void btnNuevaConsulta_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Users/Administracion/ArbolesAcceso/FrmEdicionOpcionConsulta.aspx");
        }

        protected void btnNuevoServicio_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Users/Administracion/ArbolesAcceso/FrmEdicionOpcionServicio.aspx");
        }
    }
}