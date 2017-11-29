using System;
using System.Collections.Generic;
using System.Linq;
using KiiniHelp.ServiceConsultas;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Helper;
using System.Web.UI.WebControls;

namespace KiiniHelp.Users.Consultas
{
    public partial class FrmConsultaHits : System.Web.UI.Page
    {
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
                UcFiltrosConsulta.OnAceptarModal += UcFiltrosConsulta_OnAceptarModal;
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

        protected void gvDistricts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                    e.Row.Style.Add("height", "50px");
            }
        }

        private void UcFiltrosConsulta_OnAceptarModal()
        {
            try
            {
                List<HelperHits> lstHits = _servicioConsultas.ConsultarHits(((Usuario)Session["UserData"]).Id, UcFiltrosConsulta.FiltroGrupos, UcFiltrosConsulta.FiltroTipoUsuario, UcFiltrosConsulta.FiltroOrganizaciones, UcFiltrosConsulta.FiltroUbicaciones, UcFiltrosConsulta.FiltroTipificaciones, UcFiltrosConsulta.FiltroVip, UcFiltrosConsulta.FiltroFechas, 0, 100000);

                if (lstHits != null)
                {
                    var lst = lstHits.OrderBy(s => s.IdHit).ThenBy(s => s.FechaHora).Select(s => new { s.IdHit, s.Tipificacion, s.Vip, s.TipoServicio, s.TipoUsuarioAbreviacion, s.TipoUsuarioColor, s.Ubicacion, s.Organizacion, s.FechaHora, s.Hora, s.Total }).ToList();
                    gvResult.DataSource = null;
                    gvResult.DataBind();
                    gvResult.DataSource = lst;
                    gvResult.DataBind();
                    pnlAlertaGral.Update();
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
    }
}
