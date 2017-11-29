using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniHelp.ServiceEncuesta;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.Users.Graficos
{
    public partial class FrmGraficaEncuestaNombre : Page
    {
        private readonly ServiceEncuestaClient _servicioEncuestas = new ServiceEncuestaClient();
        private readonly ServiceConsultasClient _servicioConsultas = new ServiceConsultasClient();

        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
        {
            set
            {
                pnlAlertaGeneral.Visible = value.Any();
                if (!pnlAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                ucFiltroFechasGrafico.OnAceptarModal += ucFiltroFechas_OnAceptarModal;
                ucFiltroFechasGrafico.OnCancelarModal += ucFiltroFechas_OnCancelarModal;
                if (!IsPostBack)
                {
                    ddlEncuesta.DataSource = _servicioEncuestas.ObtenerEncuestasContestadas(true);
                    ddlEncuesta.DataTextField = "Descripcion";
                    ddlEncuesta.DataValueField = "Id";
                    ddlEncuesta.DataBind();
                }
                if (Convert.ToBoolean(hfGraficaGenerada.Value))
                    btnbtnGraficar_OnClick(null, null);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private void ucFiltroFechas_OnAceptarModal()
        {
            try
            {
                btnFiltroFechas.CssClass = ucFiltroFechasGrafico.RangoFechas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroFechas\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private void ucFiltroFechas_OnCancelarModal()
        {
            try
            {
                btnFiltroFechas.CssClass = ucFiltroFechasGrafico.RangoFechas.Count > 0 ? "btn btn-success" : "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalFiltroFechas\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnbtnGraficar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlEncuesta.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione una encuesta");
                Encuesta encuesta = _servicioEncuestas.ObtenerEncuestaById(Convert.ToInt32(ddlEncuesta.SelectedValue));
                switch (encuesta.IdTipoEncuesta)
                {
                    case (int)BusinessVariables.EnumTipoEncuesta.SiNo:
                        rptGraficos.DataSource = _servicioConsultas.GraficarConsultaEncuestaPregunta(((Usuario)Session["UserData"]).Id, encuesta.Id, ucFiltroFechasGrafico.RangoFechas, ucFiltroFechasGrafico.TipoPeriodo, encuesta.IdTipoEncuesta);
                        rptGraficos.DataBind();
                        break;
                    case (int)BusinessVariables.EnumTipoEncuesta.Calificacion:
                        rptGraficos.DataSource = _servicioConsultas.GraficarConsultaEncuestaPregunta(((Usuario)Session["UserData"]).Id, encuesta.Id, ucFiltroFechasGrafico.RangoFechas, ucFiltroFechasGrafico.TipoPeriodo, encuesta.IdTipoEncuesta);
                        rptGraficos.DataBind();
                        break;
                    case (int)BusinessVariables.EnumTipoEncuesta.OpcionMultiple:
                        rptGraficos.DataSource = _servicioConsultas.GraficarConsultaEncuestaPregunta(((Usuario)Session["UserData"]).Id, encuesta.Id, ucFiltroFechasGrafico.RangoFechas, ucFiltroFechasGrafico.TipoPeriodo, encuesta.IdTipoEncuesta);
                        rptGraficos.DataBind();
                        break;
                }
                hfGraficaGenerada.Value = true.ToString();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void rptGraficos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Chart grafico = ((Chart)e.Item.FindControl("cGrafico"));
                    BusinessGraficoStack.Encuestas.Linear.GenerarGrafica(grafico, (DataTable)e.Item.DataItem);
                    grafico.Click += CGraficoOnClick;
                }
            }
            catch (Exception ex)
            {
                //if (_lstError == null)
                //{
                //    _lstError = new List<string>();
                //}
                //_lstError.Add(ex.Message);
                //AlertaGeneral = _lstError;
            }
        }

        private void CGraficoOnClick(object sender, ImageMapEventArgs imageMapEventArgs)
        {
            try
            {
                string[] selectedData = imageMapEventArgs.PostBackValue.ToString().Split(',');
                string fecha = selectedData[0];
                string total = selectedData[1];
                int idPregunta = int.Parse(selectedData[2]);
                int idRespuesta;
                Encuesta encuesta = _servicioEncuestas.ObtenerEncuestaById(Convert.ToInt32(ddlEncuesta.SelectedValue));
                switch (encuesta.IdTipoEncuesta)
                {
                    case(int)BusinessVariables.EnumTipoEncuesta.SiNo:
                        idRespuesta = selectedData[2] == "NO" ? 0 : 1;
                        break;
                    default:
                        idRespuesta = int.Parse(selectedData[3]);
                        break;
                }
                
                List<HelperReportesTicket> lstConsulta = _servicioConsultas.ConsultaEncuestaPregunta(((Usuario)Session["UserData"]).Id, encuesta.Id,
                            ucFiltroFechasGrafico.RangoFechas, ucFiltroFechasGrafico.TipoPeriodo,
                            encuesta.IdTipoEncuesta, idPregunta, idRespuesta);
                if (fecha != "Total")
                {
                        lstConsulta = lstConsulta.Where(w=>w.FechaHora == fecha).ToList();
                }
                gvResult.DataSource = lstConsulta;
                gvResult.DataBind();
                upDetalleGrafico.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalDetalleGrafico\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }


        protected void btnCerrar_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleGrafico\");", true);
        }

        protected void btnFiltroFechas_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalFiltroFechas\");", true);
                upFiltroFechas.Update();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
    }
}