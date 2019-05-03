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
    public partial class FrmGraficoTicketsEstatusAsignacion : Page
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
                ucFiltrosTicketEstatusAsignacion.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                //if (Convert.ToBoolean(hfGraficaGenerada.Value))
                //    UcFiltrosGraficoOnAceptarModal(false);
                //cGraficoPareto.Click += CGraficoOnClick;
                //cGraficoStack.Click += CGraficoOnClick;
                //cGraficoBarra.Click += CGraficoOnClick;
                if (!IsPostBack)
                {
                    ucFiltrosTicketEstatusAsignacion.InicializaFiltros();
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
                    ucFiltrosTicketEstatusAsignacion.FiltroTipoUsuario,
                    ucFiltrosTicketEstatusAsignacion.FiltroCategorias,
                    ucFiltrosTicketEstatusAsignacion.FiltroGrupos,
                    ucFiltrosTicketEstatusAsignacion.FiltroAgentes,
                    ucFiltrosTicketEstatusAsignacion.FiltroEstatusAsignacion,
                    ucFiltrosTicketEstatusAsignacion.FiltroCanalesApertura,
                    ucFiltrosTicketEstatusAsignacion.FiltroTipoArbol,
                    ucFiltrosTicketEstatusAsignacion.FiltroTipificacion,
                    ucFiltrosTicketEstatusAsignacion.FiltroEstatus,
                    new List<int>(),
                    ucFiltrosTicketEstatusAsignacion.FiltroSla,
                    new List<bool?>(),
                    ucFiltrosTicketEstatusAsignacion.FiltroOrganizaciones,
                    ucFiltrosTicketEstatusAsignacion.FiltroUbicaciones,
                    ucFiltrosTicketEstatusAsignacion.FiltroFechas,
                    ucFiltrosTicketEstatusAsignacion.FiltroEstatus,
                    "Estatus Asignacion", ucFiltrosTicketEstatusAsignacion.TipoPeriodo);
                GeneradorGraficos.GraficosPareto.GraficaPareto(rhcTicketsPareto, lblPiePareto, dtDatos, ucFiltrosTicketEstatusAsignacion.TipoPeriodo, ucFiltrosTicketEstatusAsignacion.FiltroFechas, "Estatus ASignacion");
                GeneradorGraficos.GraficoDona.GeneraGraficaStackedPie(rhcTicketsPie, lblPieGeneral, dtDatos, ucFiltrosTicketEstatusAsignacion.TipoPeriodo, ucFiltrosTicketEstatusAsignacion.FiltroFechas, lblTotal);
                GeneradorGraficos.GraficoStack.GeneraGraficaStackedColumn(rhcTicketsStack, lblPieTendencia, dtDatos, ucFiltrosTicketEstatusAsignacion.TipoPeriodo, ucFiltrosTicketEstatusAsignacion.FiltroFechas);

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