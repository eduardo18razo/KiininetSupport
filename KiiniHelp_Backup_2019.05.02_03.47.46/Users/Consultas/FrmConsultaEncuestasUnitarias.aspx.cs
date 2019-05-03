using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.Users.Consultas
{
    public partial class FrmConsultaEncuestasUnitarias : Page
    {
        private readonly ServiceConsultasClient _servicioConsultas = new ServiceConsultasClient();

        private List<string> _lstError = new List<string>();
        

        private List<string> AlertaGeneral
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


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucFiltrosEncuestas.OnAceptarModal += ucFiltrosEncuestas_OnAceptarModal;
               
                if (!IsPostBack)
                {
                    
                }
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

        private void LlenaConsulta()
        {
            try
            {
                gvResult.DataSource = _servicioConsultas.ConsultarEncuestas(((Usuario)Session["UserData"]).Id, ucFiltrosEncuestas.FiltroGrupos, ucFiltrosEncuestas.FiltroTipoArbol, ucFiltrosEncuestas.FiltroResponsables, ucFiltrosEncuestas.FiltroEncuestas, ucFiltrosEncuestas.FiltroAtendedores, ucFiltrosEncuestas.FiltroFechas, ucFiltrosEncuestas.FiltroTipoUsuario, ucFiltrosEncuestas.FiltroPrioridad, ucFiltrosEncuestas.FiltroSla, ucFiltrosEncuestas.FiltroUbicaciones, ucFiltrosEncuestas.FiltroOrganizaciones, ucFiltrosEncuestas.FiltroVip, 0, 1000);
                gvResult.DataBind();
                pnlResult.Update();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ucFiltrosEncuestas_OnAceptarModal()
        {
            try
            {
                if (!ucFiltrosEncuestas.FiltroGrupos.Any())
                    throw new Exception("Debe seleccionar al menos un grupo");
                LlenaConsulta();
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
        protected void gvPaginacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResult.PageIndex = e.NewPageIndex;
            LlenaConsulta();
        }

        protected void gvResult_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[20].Visible = false;
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