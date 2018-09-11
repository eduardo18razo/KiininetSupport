using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.ReportesGraficos.Encuestas
{
    public partial class UcReporteEncuestasLogica : UserControl
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
                List<ArbolAcceso> lstArboles = _servicioArbolAcceso.ObtenerArbolesAccesoAllReporte(idArea, idTipoUsuario, null, (int)BusinessVariables.EnumTipoEncuesta.SiNo, Metodos.ManejoFechas.ObtenerFechas(int.Parse(ddlTipoFiltro.SelectedValue), txtFechaInicio.Text.Trim(), txtFechaFin.Text.Trim())).Where(w => w.EsTerminal).ToList();
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
            if (!IsPostBack)
            {
                LlenaCombos();
                LlenaArboles();
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
                        System.Globalization.Calendar cal = dfi.Calendar;
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

        protected void btnGraficar_OnClick(object sender, EventArgs e)
        {
            try
            {
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                string path = string.Empty;
                if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                    path = "Users/ReportesGraficos/Encuestas/FrmGraficoEncuestaLogica.aspx?idArbol=" + int.Parse(((LinkButton)sender).CommandArgument) + "&tipoFecha=" + ddlTipoFiltro.SelectedValue;
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
                    path = "Users/ReportesGraficos/Encuestas/FrmGraficoEncuestaLogica.aspx?idArbol=" + int.Parse(((LinkButton)sender).CommandArgument) + "&tipoFecha=" + ddlTipoFiltro.SelectedValue + "&fi=" + txtFechaInicio.Text + "&ff=" + txtFechaFin.Text;
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