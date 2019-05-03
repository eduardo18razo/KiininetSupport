using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.UserControls.ReportesGraficos.InformacionConsulta
{
    public partial class UcReporteInformacionConsulta : UserControl
    {
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
                tblResults.DataSource = SortList(_servicioInformacionConsulta.ObtenerInformacionReporte(filtro, ucFiltroFechasGrafico.RangoFechas), isPageIndexChanging); ;
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
                    ucFiltroFechasGrafico.ObtenerFechasParametro();
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
                else
                {
                    result = data.OrderBy(o => o.Titulo).ToList();
                }
            }
            _sorted = true;
            return result;
        }

        protected void tblResults_OnSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
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

        protected void btnDetalle_OnClick(object sender, EventArgs e)
        {
            try
            {
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                string path = string.Empty;
                if (ucFiltroFechasGrafico.RangoFechas == null)
                    path = "Users/ReportesGraficos/InformacionConsulta/FrmGraficoInformacionConsulta.aspx?idInformacion=" + int.Parse(((LinkButton)sender).CommandArgument) + "&tipoFecha=" + ucFiltroFechasGrafico.TipoPeriodo;
                else
                {
                    path = "Users/ReportesGraficos/InformacionConsulta/FrmGraficoInformacionConsulta.aspx?idInformacion=" + int.Parse(((LinkButton)sender).CommandArgument) + "&tipoFecha=" + ucFiltroFechasGrafico.TipoPeriodo + "&fi=" + ucFiltroFechasGrafico.FechaInicio + "&ff=" + ucFiltroFechasGrafico.FechaFin;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "window.open('" + url + path + "','_blank');", true);
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