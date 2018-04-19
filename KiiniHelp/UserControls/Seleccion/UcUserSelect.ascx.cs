using System;
using System.Web.UI;
using System.Linq;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceFrecuencia;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using System.Web.UI.WebControls;

namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcUserSelect : UserControl
    {
        private readonly ServiceTipoUsuarioClient _servicioTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceFrecuenciaClient _servicioFrecuencia = new ServiceFrecuenciaClient();
        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();

        private void GetData(int idTipoUsuario)
        {
            try
            {
                int idArea = Request.Params["idArea"] != null ? int.Parse(Request.Params["idArea"]) : 0;
                lblCategoria.Visible = idArea > 0;
                if (lblCategoria.Visible)
                {
                    Area area = _servicioArea.ObtenerAreaById(idArea);
                    lblCategoria.Text = area.Descripcion;
                    rptCatTodos.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(idArea, idTipoUsuario, null, null, null, null, null, null, null, null).Where(w => w.EsTerminal);
                    rptCatTodos.DataBind();

                    rptCatConsultas.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(idArea, idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion, null, null, null, null, null, null, null).Where(w => w.EsTerminal);
                    rptCatConsultas.DataBind();

                    rptCatServicios.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(idArea, idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.SolicitarServicio, null, null, null, null, null, null, null).Where(w => w.EsTerminal);
                    rptCatServicios.DataBind();

                    rptCatProblemas.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(idArea, idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ReportarProblemas, null, null, null, null, null, null, null).Where(w => w.EsTerminal);
                    rptCatProblemas.DataBind();
                }
                else
                {
                    rpt10Frecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenGeneral(idTipoUsuario);
                    rpt10Frecuentes.DataBind();

                    rptConsultasFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenConsulta(idTipoUsuario);
                    rptConsultasFrecuentes.DataBind();

                    rptServiciosFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenServicio(idTipoUsuario);
                    rptServiciosFrecuentes.DataBind();

                    rptProblemasFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenIncidente(idTipoUsuario);
                    rptProblemasFrecuentes.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    int idtipoUsuario;
                    if (Request.Params["userType"] != null)
                    {
                        idtipoUsuario = int.Parse(Request.Params["userType"]);
                    }
                    else
                    {
                        idtipoUsuario = ((Usuario)Session["UserData"]).IdTipoUsuario;
                    }
                    TipoUsuario tipoUsuario = _servicioTipoUsuario.ObtenerTipoUsuarioById(idtipoUsuario);
                    if (tipoUsuario != null)
                    {
                        GetData(tipoUsuario.Id);
                        lbltipoUsuario.Text = tipoUsuario.Descripcion;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void lbtnCategoria_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Publico/FrmCategoria.aspx?userType=" + int.Parse(Request.Params["userType"]));
        }

        protected void verOpcion_Click(object sender, EventArgs e)
        {
            int tipoArbol = Convert.ToInt32(((LinkButton)sender).CommandName);
            if (Session["UserData"] == null)
                switch (tipoArbol)
                {
                    case (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion:
                        Response.Redirect("~/Publico/FrmConsulta.aspx?IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;

                    case (int)BusinessVariables.EnumTipoArbol.SolicitarServicio:
                        Response.Redirect("~/Publico/FrmTicketPublico.aspx?Canal=" + BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "?IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;

                    case (int)BusinessVariables.EnumTipoArbol.ReportarProblemas:
                        Response.Redirect("~/Publico/FrmTicketPublico.aspx?Canal=" + BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "?IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;
                }
            else
                switch (tipoArbol)
                {
                    case (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion:
                        Response.Redirect("~/Users/General/FrmNodoConsultas.aspx?IdArbol==" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;

                    case (int)BusinessVariables.EnumTipoArbol.SolicitarServicio:
                        Response.Redirect("~/Users/Ticket/FrmTicket.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;

                    case (int)BusinessVariables.EnumTipoArbol.ReportarProblemas:
                        Response.Redirect("~/Users/Ticket/FrmTicket.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;
                }

        }

        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + Request.Params["userType"]);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}