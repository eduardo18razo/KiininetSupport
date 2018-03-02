using System;
using System.Web.UI;
using System.Linq;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceFrecuencia;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

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
                int idArea = int.Parse(Request.Params["idArea"]);


                if (idArea > 0)
                {

                    rptCatTodos.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(int.Parse(Request.Params["idArea"]), idTipoUsuario, null, null, null, null, null, null, null, null);
                    rptCatTodos.DataBind();


                    rptCatConsultas.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(int.Parse(Request.Params["idArea"]), idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion, null, null, null, null, null, null, null);
                       // .ObtenerArbolesAccesoTerminalAllTipificacion(int.Parse(Request.Params["idArea"]), idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion, null, null, null, null, null, null, null);
                    rptCatConsultas.DataBind();


                    rptCatServicios.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(int.Parse(Request.Params["idArea"]), idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.SolicitarServicio, null, null, null, null, null, null, null);
                    rptCatServicios.DataBind();

                    rptCatProblemas.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoAll(int.Parse(Request.Params["idArea"]), idTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ReportarProblemas, null, null, null, null, null, null, null);
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
                    int idtipoUsuario = int.Parse(Request.Params["userTipe"]);
                   

                    TipoUsuario tipoUsuario = _servicioTipoUsuario.ObtenerTipoUsuarioById(idtipoUsuario);
                    if (tipoUsuario != null)
                    {
                        GetData(tipoUsuario.Id);
                        lbltipoUsuario.Text = tipoUsuario.Descripcion;
                    }
                }
                //    navCategoria.NavigateUrl = "~/manager/uploadTrainingPlan.aspx?id=" + Request.QueryString["userTipe"];
                //    switch (((Usuario)Session["UserData"]).IdTipoUsuario)
                //    {
                //        case (int)BusinessVariables.EnumTiposUsuario.Empleado:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.Cliente:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.Proveedor:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.EmpleadoPersonaFisica:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.ClientaPersonaFisica:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.ProveedorPersonaFisica:
                //            break;
                //        case (int)BusinessVariables.EnumTiposUsuario.NuestraInstitucion:
                //            break;
                //    }
                //}
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
    }
}