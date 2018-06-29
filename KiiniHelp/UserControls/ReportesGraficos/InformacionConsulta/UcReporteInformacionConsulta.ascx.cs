using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniNet.Entities.Helper;
using Calendar = System.Globalization.Calendar;

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
                tblResults.DataSource = SortList(_servicioInformacionConsulta.ObtenerConsulta(filtro, Metodos.ManejoFechas.ObtenerFechas(int.Parse(ddlTipoFiltro.SelectedValue), txtFechaInicio.Text.Trim(), txtFechaFin.Text.Trim())), isPageIndexChanging); ;
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
            }
            _sorted = true;
            return result;
        }

        protected void tblResults_OnSorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            LlenaInformacionConsulta(false);
        }

        protected void btnDetalle_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                    Response.Redirect("~/Users/ReportesGraficos/InformacionConsulta/FrmGraficoInformacionConsulta.aspx?idInformacion=" + int.Parse(((LinkButton)sender).CommandArgument) + "&tipoFecha=" + ddlTipoFiltro.SelectedValue);
                else
                {
                    DateTime fechainicio, fechafin;
                    try
                    {
                        DateTime.TryParse(txtFechaInicio.Text, out fechainicio);
                        DateTime.TryParse(txtFechaFin.Text, out fechafin);
                    }
                    catch
                    {
                        throw new Exception("Verifique fechas");
                    }
                    Response.Redirect("~/Users/ReportesGraficos/InformacionConsulta/FrmGraficoInformacionConsulta.aspx?idInformacion=" + int.Parse(((LinkButton)sender).CommandArgument) + "&tipoFecha=" + ddlTipoFiltro.SelectedValue + "&fi=" + txtFechaInicio.Text + "&ff=" + txtFechaFin.Text);
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

        protected void ddlTipoFiltro_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFechaInicio.Attributes["type"] != null)
                    txtFechaInicio.Attributes.Remove("type");
                if (txtFechaFin.Attributes["type"] != null)
                    txtFechaFin.Attributes.Remove("type");

                if (txtFechaInicio.Attributes["min"] != null)
                    txtFechaInicio.Attributes.Remove("type");
                if (txtFechaFin.Attributes["max"] != null)
                    txtFechaFin.Attributes.Remove("max");

                switch (ddlTipoFiltro.SelectedValue)
                {
                    case "1":
                        txtFechaInicio.Attributes["type"] = "date";
                        txtFechaFin.Attributes["type"] = "date";
                        txtFechaFin.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
                        break;
                    case "2":
                        txtFechaInicio.Attributes["type"] = "week";
                        txtFechaFin.Attributes["type"] = "week";
                        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                        DateTime date1 = new DateTime(DateTime.Now.Year, 12, 31);
                        Calendar cal = dfi.Calendar;
                        txtFechaInicio.Attributes["min"] = string.Format("{0}-W{1}", DateTime.Now.Year, "01");
                        txtFechaInicio.Attributes["max"] = string.Format("{0}-W{1}", DateTime.Now.Year, cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek));

                        break;
                    case "3":
                        txtFechaInicio.Attributes["type"] = "month";
                        txtFechaFin.Attributes["type"] = "month";
                        txtFechaInicio.Attributes["min"] = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM");
                        txtFechaInicio.Attributes["max"] = new DateTime(DateTime.Now.Year, 12, 31).ToString("yyyy-MM");

                        break;
                    case "4":
                        txtFechaInicio.Attributes["type"] = "number";
                        txtFechaFin.Attributes["type"] = "number";
                        txtFechaInicio.Attributes["min"] = "2000";
                        txtFechaInicio.Attributes["max"] = DateTime.Now.Year.ToString();
                        txtFechaInicio.Text = DateTime.Now.AddYears(-1).Year.ToString();
                        txtFechaFin.Text = DateTime.Now.Year.ToString();
                        break;
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