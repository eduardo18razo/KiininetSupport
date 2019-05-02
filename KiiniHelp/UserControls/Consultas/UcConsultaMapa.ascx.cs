using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Operacion;
using KinniNet.Business.Utils;
using OfficeOpenXml;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaMapa : UserControl
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
                List<ArbolAcceso> lstArboles = _servicioArbolAcceso.ObtenerArbolesAccesoAll(idArea, idTipoUsuario, null, null, null, null, null, null, null, null).ToList();
                
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
            try
            {
                if (!IsPostBack)
                {
                    LlenaCombos();
                    LlenaArboles();
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

        #region Paginador
        protected void gvPaginacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            tblResults.PageIndex = e.NewPageIndex;
            LlenaArboles();
        }

        #endregion
        
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
                List<ArbolAcceso> lstArboles = _servicioArbolAcceso.ObtenerArbolesAccesoAll(idArea, idTipoUsuario, null, null, null, null, null, null, null, null).Where(w => w.EsTerminal).ToList();
                Response.Clear();
                ExcelPackage excel = null;
                foreach (BusinessVariables.EnumTiposUsuario tipoUsuario in (BusinessVariables.EnumTiposUsuario[])Enum.GetValues(typeof(BusinessVariables.EnumTiposUsuario)))
                {
                    BusinessFile.ExcelManager.ListsToExcel(lstArboles.Where(w=>w.IdTipoUsuario == (int)tipoUsuario).Select(
                    s => new
                    {
                        TipoUsuario = s.TipoUsuario.Descripcion,
                        Categoría = s.Area.Descripcion,
                        Tipo_Seccion1 = s.Nivel2 != null ? "s" : "o",
                        Seccion_opcion1 = s.Nivel1 != null ? s.Nivel1.Descripcion : string.Empty,
                        Tipo_Seccion2 = s.Nivel3 != null ? "s" : "o",
                        Seccion_opcion2 = s.Nivel2 != null ? s.Nivel2.Descripcion : string.Empty,
                        Tipo_Seccion3 = s.Nivel4 != null ? "s" : "o",
                        Seccion_opcion3 = s.Nivel3 != null ? s.Nivel3.Descripcion : string.Empty,
                        Tipo_Seccion4 = s.Nivel5 != null ? "s" : "o",
                        Seccion_opcion4 = s.Nivel4 != null ? s.Nivel4.Descripcion : string.Empty,
                        Tipo_Seccion5 = s.Nivel6 != null ? "s" : "o",
                        Seccion_opcion5 = s.Nivel5 != null ? s.Nivel5.Descripcion : string.Empty,
                        Tipo_Seccion6 = s.Nivel7 != null ? "s" : "o",
                        Seccion_opcion6 = s.Nivel6 != null ? s.Nivel6.Descripcion : string.Empty,
                        Tipo_Seccion7 = "o",
                        Seccion_opcion7 = s.Nivel7 != null ? s.Nivel7.Descripcion : string.Empty,
                        Tipificación = s.TipoArbolAcceso.Descripcion,
                    })
                    .ToList(), tipoUsuario.ToString(), ref excel);
                }
                if (excel != null)
                {
                    MemoryStream ms = new MemoryStream(excel.GetAsByteArray());
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=ConfiguraciónMenu.xlsx");
                    Response.Buffer = true;
                    ms.WriteTo(Response.OutputStream);
                    Response.End();
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