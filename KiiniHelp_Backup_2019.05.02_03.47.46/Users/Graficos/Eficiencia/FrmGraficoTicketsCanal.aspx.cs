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
    public partial class FrmGraficoTicketsCanal : Page
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
                ucFiltrosTicketCanal.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                if (!IsPostBack)
                {
                    ucFiltrosTicketCanal.InicializaFiltros();
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
                    ucFiltrosTicketCanal.FiltroTipoUsuario,
                    ucFiltrosTicketCanal.FiltroCategorias,
                    ucFiltrosTicketCanal.FiltroGrupos,
                    ucFiltrosTicketCanal.FiltroAgentes,
                    ucFiltrosTicketCanal.FiltroEstatusAsignacion,
                    ucFiltrosTicketCanal.FiltroCanalesApertura,
                    ucFiltrosTicketCanal.FiltroTipoArbol,
                    ucFiltrosTicketCanal.FiltroTipificacion,
                    ucFiltrosTicketCanal.FiltroEstatus,
                    new List<int>(),
                    ucFiltrosTicketCanal.FiltroSla,
                    new List<bool?>(),
                    ucFiltrosTicketCanal.FiltroOrganizaciones,
                    ucFiltrosTicketCanal.FiltroUbicaciones,
                    ucFiltrosTicketCanal.FiltroFechas,
                    ucFiltrosTicketCanal.FiltroEstatus,
                    "Canal", ucFiltrosTicketCanal.TipoPeriodo);
                GeneradorGraficos.GraficosPareto.GraficaPareto(rhcTicketsPareto, lblPiePareto, dtDatos, ucFiltrosTicketCanal.TipoPeriodo, ucFiltrosTicketCanal.FiltroFechas, "Canal");
                GeneradorGraficos.GraficoDona.GeneraGraficaStackedPie(rhcTicketsPie, lblPieGeneral, dtDatos, ucFiltrosTicketCanal.TipoPeriodo, ucFiltrosTicketCanal.FiltroFechas, lblTotal);
                GeneradorGraficos.GraficoStack.GeneraGraficaStackedColumn(rhcTicketsStack, lblPieTendencia, dtDatos, ucFiltrosTicketCanal.TipoPeriodo, ucFiltrosTicketCanal.FiltroFechas);

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