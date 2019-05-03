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
    public partial class FrmGraficoTicketsSla : Page
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
                ucFiltrosTicketSla.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                //if (Convert.ToBoolean(hfGraficaGenerada.Value))
                //    UcFiltrosGraficoOnAceptarModal(false);
                //cGraficoPareto.Click += CGraficoOnClick;
                //cGraficoStack.Click += CGraficoOnClick;
                //cGraficoBarra.Click += CGraficoOnClick;
                if (!IsPostBack)
                {
                    ucFiltrosTicketSla.InicializaFiltros();
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
                    ucFiltrosTicketSla.FiltroTipoUsuario,
                    ucFiltrosTicketSla.FiltroCategorias,
                    ucFiltrosTicketSla.FiltroGrupos,
                    ucFiltrosTicketSla.FiltroAgentes,
                    ucFiltrosTicketSla.FiltroEstatusAsignacion,
                    ucFiltrosTicketSla.FiltroCanalesApertura,
                    ucFiltrosTicketSla.FiltroTipoArbol,
                    ucFiltrosTicketSla.FiltroTipificacion,
                    ucFiltrosTicketSla.FiltroEstatus,
                    new List<int>(),
                    ucFiltrosTicketSla.FiltroSla,
                    new List<bool?>(),
                    ucFiltrosTicketSla.FiltroOrganizaciones,
                    ucFiltrosTicketSla.FiltroUbicaciones,
                    ucFiltrosTicketSla.FiltroFechas,
                    ucFiltrosTicketSla.FiltroEstatus,
                    "SLA", ucFiltrosTicketSla.TipoPeriodo);
                GeneradorGraficos.GraficosPareto.GraficaPareto(rhcTicketsPareto, lblPiePareto, dtDatos, ucFiltrosTicketSla.TipoPeriodo, ucFiltrosTicketSla.FiltroFechas, "SLA");
                GeneradorGraficos.GraficoDona.GeneraGraficaStackedPie(rhcTicketsPie, lblPieGeneral, dtDatos, ucFiltrosTicketSla.TipoPeriodo, ucFiltrosTicketSla.FiltroFechas, lblTotal);
                GeneradorGraficos.GraficoStack.GeneraGraficaStackedColumn(rhcTicketsStack, lblPieTendencia, dtDatos, ucFiltrosTicketSla.TipoPeriodo, ucFiltrosTicketSla.FiltroFechas);

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