using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceMascaraAcceso;
using Calendar = System.Globalization.Calendar;

namespace KiiniHelp.UserControls.ReportesGraficos.Formularios
{
    public partial class UcReporteFormularios : UserControl
    {
        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
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

        private void LlenaMascaras()
        {
            try
            {
                string descripcion = txtFiltro.Text.ToLower().Trim();

                tblResults.DataSource = _servicioMascaras.Consulta(descripcion);
                tblResults.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
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
                    LlenaMascaras();
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
                LlenaMascaras();
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
            LlenaMascaras();
        }

        #endregion

        protected void btnDetalle_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Users/ReportesGraficos/Formularios/FrmConsultaDetalleFormulario.aspx?idMascara=" + ((LinkButton)sender).CommandArgument);
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