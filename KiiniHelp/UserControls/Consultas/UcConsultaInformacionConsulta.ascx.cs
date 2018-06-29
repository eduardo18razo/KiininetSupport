using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniHelp.ServiceSistemaTipoInformacionConsulta;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;
using Calendar = System.Globalization.Calendar;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaInformacionConsulta : UserControl
    {
        private readonly ServiceTipoInfConsultaClient _servicioSistemaTipoInformacion = new ServiceTipoInfConsultaClient();
        private readonly ServiceInformacionConsultaClient _servicioInformacionConsulta = new ServiceInformacionConsultaClient();

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

        private void LlenaInformacionConsulta(bool isPageIndexChanging)
        {
            try
            {
                string filtro = txtFiltro.Text;
                tblResults.DataSource = SortList(_servicioInformacionConsulta.ObtenerConsulta(filtro, null), isPageIndexChanging); ;
                tblResults.DataBind();
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
                    LlenaInformacionConsulta(false);
                }
                Alerta = new List<string>();
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

                Response.Redirect("~/Users/Administracion/InformaciondeConsulta/FrmAltaInfConsulta.aspx?IdInformacionConsulta=" + ((LinkButton)sender).CommandArgument);
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
                Response.Redirect("~/Users/Administracion/InformaciondeConsulta/FrmAltaInfConsulta.aspx");
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
                LlenaInformacionConsulta(false);
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
                _servicioInformacionConsulta.HabilitarInformacion(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
                LlenaInformacionConsulta(false);
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
                string filtro = txtFiltro.Text.Trim().ToUpper();
                List<HelperInformacionConsulta> lstInformacion = _servicioInformacionConsulta.ObtenerConsulta(filtro, null);

                Response.Clear();
                string ultimaEdicion = "Últ. edición";
                MemoryStream ms = new MemoryStream(BusinessFile.ExcelManager.ListToExcel(lstInformacion.Select(
                                s => new
                                {
                                    Nombre = s.Titulo,
                                    Creación = s.Creacion.ToShortDateString().ToString(),
                                    ultimaEdicion = s.UltEdicion == null ? "" : s.UltEdicion.Value.ToShortDateString().ToString(),
                                    Habilitado = s.Habilitado ? "Si" : "No"
                                })
                                .ToList()).GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Articulos.xlsx");
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

        #region Paginador
        protected void gvPaginacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            tblResults.PageIndex = e.NewPageIndex;
            LlenaInformacionConsulta(true);
        }

        #endregion

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
        protected List<HelperInformacionConsulta> SortList(List<HelperInformacionConsulta> data, bool isPageIndexChanging)
        {
            List<HelperInformacionConsulta> result = data;
            if (data != null)
            {
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
            }
            _sorted = true;
            return result;
        }

        protected void tblResults_OnSorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            LlenaInformacionConsulta(false);
        }
    }
}