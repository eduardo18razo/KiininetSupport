using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.Users.Consultas
{
    public partial class FrmConsultaTickets : Page
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
                ucFiltrosTicket.OnAceptarModal += ucFiltrosTicket_OnAceptarModal; 
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

        void ucFiltrosTicket_OnAceptarModal()
        {
            try
            {
                if (!ucFiltrosTicket.FiltroGrupos.Any())
                    throw new Exception("Debe seleccionar al menos un grupo");

                List<HelperReportesTicket> lstConsulta = _servicioConsultas.ConsultarTickets(((Usuario)Session["UserData"]).Id, ucFiltrosTicket.FiltroGrupos, ucFiltrosTicket.FiltroCanalesApertura, ucFiltrosTicket.FiltroTipoUsuario, ucFiltrosTicket.FiltroOrganizaciones,
                    ucFiltrosTicket.FiltroUbicaciones, ucFiltrosTicket.FiltroTipoArbol, ucFiltrosTicket.FiltroTipificaciones,
                    ucFiltrosTicket.FiltroPrioridad, ucFiltrosTicket.FiltroEstatus, ucFiltrosTicket.FiltroSla, ucFiltrosTicket.FiltroVip, ucFiltrosTicket.FiltroFechas, 0, 1000);
                gvResult.DataSource = lstConsulta;
                gvResult.DataBind();               
                pnlAlertaGral.Update();
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

        protected void gvResult_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //e.Row.Cells[0].Visible = false;               
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