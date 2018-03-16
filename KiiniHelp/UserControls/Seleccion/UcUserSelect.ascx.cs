using System;
using System.Web.UI;
using System.Linq;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceFrecuencia;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using System.Web.UI.WebControls;

namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcUserSelect : UserControl
    {
        private readonly ServiceTipoUsuarioClient _servicioTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceFrecuenciaClient _servicioFrecuencia = new ServiceFrecuenciaClient();

        private void GetData(int idTipoUsuario)
        {
            try
            {
                int idArea = Request.Params["idArea"] != null ? int.Parse(Request.Params["idArea"]) : 0;
                if (idArea > 0)
                {

                    rptCatTodos.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(idArea, idTipoUsuario, null, null, null, null, null, null, null, null);
                    rptCatTodos.DataBind();

                    rptCatConsultas.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(idArea, idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion, null, null, null, null, null, null, null);
                    rptCatConsultas.DataBind();

                    rptCatServicios.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(idArea, idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.SolicitarServicio, null, null, null, null, null, null, null);
                    rptCatServicios.DataBind();

                    rptCatProblemas.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(idArea, idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ReportarProblemas, null, null, null, null, null, null, null);
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
                    if (Request.Params["userTipe"] != null)
                    {
                        idtipoUsuario = int.Parse(Request.Params["userTipe"]);
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

        protected void lbtnTypeUser_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Publico/FrmUserSelect.aspx?userTipe=" + int.Parse(Request.Params["userTipe"]));
        }

        protected void lbtnCategoria_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Publico/FrmCategoria.aspx?userType=" + int.Parse(Request.Params["userTipe"]));
        }

        protected void verOpcion_Click(object sender, EventArgs e)
        {
            int tipoArbol = Convert.ToInt32(((LinkButton)sender).CommandName);

            switch (tipoArbol)
            {
                case (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion:
                    Response.Redirect("~/Publico/FrmConsulta.aspx?idArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));   
                    break;

                case (int)BusinessVariables.EnumTipoArbol.SolicitarServicio:
                    Response.Redirect("~/Publico/FrmTicketPublico.aspx?Canal=" + BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "?idArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));            
                    break;

                case (int)BusinessVariables.EnumTipoArbol.ReportarProblemas:
                    Response.Redirect("~/Publico/FrmTicketPublico.aspx?Canal=" + BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "?idArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));                                
                    break;
            }

        }
    }
}