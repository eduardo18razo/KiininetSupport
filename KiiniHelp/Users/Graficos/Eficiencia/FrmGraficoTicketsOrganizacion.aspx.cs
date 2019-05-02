using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceConsultas;
using KiiniHelp.ServiceParametrosSistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;

namespace KiiniHelp.Users.Graficos.Eficiencia
{
    public partial class FrmGraficoTicketsOrganizacion : Page
    {
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
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
                ucFiltrosTicketOrganizacion.OnAceptarModal += UcFiltrosGraficoOnAceptarModal;
                ucDetalleGeograficoTickets.OnCancelarModal += UcDetalleGeograficoTicketsOnCancelarModal;
                if (!IsPostBack)
                {
                    ucFiltrosTicketOrganizacion.InicializaFiltros();
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

        private class Top
        {
            public int Id { get; set; }
            public string Descripcion { get; set; }
            public double Total { get; set; }
        }

        private void GeneraGraficas()
        {
            try
            {
                upGrafica.Visible = true;

                DataTable dtDatos = _servicioConsultas.GraficarConsultaTicketEficiencia(((Usuario)Session["UserData"]).Id,
                    ucFiltrosTicketOrganizacion.FiltroTipoUsuario,
                    ucFiltrosTicketOrganizacion.FiltroCategorias,
                    ucFiltrosTicketOrganizacion.FiltroGrupos,
                    ucFiltrosTicketOrganizacion.FiltroAgentes,
                    ucFiltrosTicketOrganizacion.FiltroEstatusAsignacion,
                    ucFiltrosTicketOrganizacion.FiltroCanalesApertura,
                    ucFiltrosTicketOrganizacion.FiltroTipoArbol,
                    ucFiltrosTicketOrganizacion.FiltroTipificacion,
                    ucFiltrosTicketOrganizacion.FiltroEstatus,
                    new List<int>(),
                    ucFiltrosTicketOrganizacion.FiltroSla,
                    new List<bool?>(),
                    ucFiltrosTicketOrganizacion.FiltroOrganizaciones,
                    ucFiltrosTicketOrganizacion.FiltroUbicaciones,
                    ucFiltrosTicketOrganizacion.FiltroFechas,
                    ucFiltrosTicketOrganizacion.FiltroEstatus,
                    "Organizaciones", ucFiltrosTicketOrganizacion.TipoPeriodo);
                
                DataTable dtResultTop = null;
                if (dtDatos != null)
                {
                    dtResultTop = dtDatos.Copy();
                    DataTable dtOtros = dtDatos.Copy();
                    List<ColoresTop> lstColores = _servicioParametros.ObtenerColoresTop();
                    List<Top> topResult = new List<Top>();
                    foreach (DataRow row in dtDatos.Rows)
                    {
                        if (topResult.All(a => a.Id != int.Parse(row[0].ToString())))
                        {
                            double total = 0;
                            for (int i = 3; i < dtDatos.Columns.Count; i++)
                            {
                                total += double.Parse(row[i].ToString());
                            }
                            topResult.Add(new Top { Id = int.Parse(row[0].ToString()), Descripcion = row[1].ToString(), Total = total });
                        }
                    }

                    topResult = topResult.OrderByDescending(o => o.Total).ToList();

                    foreach (Top top in topResult.Skip(20))
                    {
                        DataRow[] dr = dtResultTop.Select("Id =" + top.Id);
                        foreach (DataRow row in dr)
                        {
                            dtResultTop.Rows.Remove(row);
                        }
                    }

                    if (topResult.Count > 20)
                    {

                        foreach (Top top in topResult.Take(20))
                        {
                            DataRow[] dr = dtOtros.Select("Id =" + top.Id);
                            foreach (DataRow row in dr)
                            {
                                dtOtros.Rows.Remove(row);
                            }
                        }

                        dtResultTop.Rows.Add(-1, "Otros");

                        for (int i = 3; i < dtOtros.Columns.Count; i++)
                        {
                            DataRow dr = dtResultTop.Select("Id=-1").FirstOrDefault();
                            if (dr != null)
                            {
                                dr[i] = (from item in dtOtros.AsEnumerable()
                                         select item.Field<int>(dtDatos.Columns[i].ColumnName)).Sum(); ;

                            }
                        }
                    }

                    int contadorColor = 0;
                    foreach (ColoresTop color in lstColores)
                    {
                        if (contadorColor < dtResultTop.Rows.Count)
                        {
                            dtResultTop.Rows[contadorColor][2] = color.Color;
                            contadorColor++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                GeneradorGraficos.GraficosPareto.GraficaPareto(rhcTicketsPareto, lblPiePareto, dtResultTop, ucFiltrosTicketOrganizacion.TipoPeriodo, ucFiltrosTicketOrganizacion.FiltroFechas, "Organizaciones");
                GeneradorGraficos.GraficoDona.GeneraGraficaStackedPie(rhcTicketsPie, lblPieGeneral, dtResultTop, ucFiltrosTicketOrganizacion.TipoPeriodo, ucFiltrosTicketOrganizacion.FiltroFechas, lblTotal);
                GeneradorGraficos.GraficoStack.GeneraGraficaStackedColumn(rhcTicketsStack, lblPieTendencia, dtResultTop, ucFiltrosTicketOrganizacion.TipoPeriodo, ucFiltrosTicketOrganizacion.FiltroFechas);

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