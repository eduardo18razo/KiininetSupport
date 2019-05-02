using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceConsultas;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.Users.Graficos.Eficiencia
{
    public partial class FrmGraficoTicketsTipoTicket : Page
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
                AlertaGeneral = new List<string>();
                ucFiltrosTicketTipoTicket.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                if (!IsPostBack)
                {
                    ucFiltrosTicketTipoTicket.InicializaFiltros();
                    GeneraGraficas();
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

        private void GeneraGraficas()
        {
            try
            {
                upGrafica.Visible = true;

                DataTable dtDatos = _servicioConsultas.GraficarConsultaTicketEficiencia(((Usuario)Session["UserData"]).Id,
                    ucFiltrosTicketTipoTicket.FiltroTipoUsuario,
                    ucFiltrosTicketTipoTicket.FiltroCategorias,
                    ucFiltrosTicketTipoTicket.FiltroGrupos,
                    ucFiltrosTicketTipoTicket.FiltroAgentes,
                    ucFiltrosTicketTipoTicket.FiltroEstatusAsignacion,
                    ucFiltrosTicketTipoTicket.FiltroCanalesApertura,
                    ucFiltrosTicketTipoTicket.FiltroTipoArbol,
                    ucFiltrosTicketTipoTicket.FiltroTipificacion,
                    ucFiltrosTicketTipoTicket.FiltroEstatus,
                    new List<int>(),
                    ucFiltrosTicketTipoTicket.FiltroSla,
                    new List<bool?>(),
                    ucFiltrosTicketTipoTicket.FiltroOrganizaciones,
                    ucFiltrosTicketTipoTicket.FiltroUbicaciones,
                    ucFiltrosTicketTipoTicket.FiltroFechas,
                    ucFiltrosTicketTipoTicket.FiltroEstatus,
                    "Tipo Ticket", ucFiltrosTicketTipoTicket.TipoPeriodo);
                GeneradorGraficos.GraficosPareto.GraficaPareto(rhcTicketsPareto, lblPiePareto, dtDatos, ucFiltrosTicketTipoTicket.TipoPeriodo, ucFiltrosTicketTipoTicket.FiltroFechas, "Tipo Ticket");
                GeneradorGraficos.GraficoDona.GeneraGraficaStackedPie(rhcTicketsPie, lblPieGeneral, dtDatos, ucFiltrosTicketTipoTicket.TipoPeriodo, ucFiltrosTicketTipoTicket.FiltroFechas, lblTotal);
                GeneradorGraficos.GraficoStack.GeneraGraficaStackedColumn(rhcTicketsStack, lblPieTendencia, dtDatos, ucFiltrosTicketTipoTicket.TipoPeriodo, ucFiltrosTicketTipoTicket.FiltroFechas);

                rhcTicketsPie.Visible = true;
                rhcTicketsPareto.Visible = true;
                rhcTicketsStack.Visible = true;

                hfGraficaGenerada.Value = true.ToString();
                upGrafica.Update();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        private void UcFiltrosGraficoOnAceptarModal()
        {
            try
            {
                GeneraGraficas();
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
        
        private void UcDetalleGeograficoTicketsOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDetalleGeografico\");", true);
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

        protected void btnConsultar_OnClick(object sender, EventArgs e)
        {

        }
    }
}