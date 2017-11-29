using System;
using System.Web.UI;
using KiiniHelp.ServiceFrecuencia;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcUserSelect : UserControl
    {
        private readonly ServiceFrecuenciaClient _servicioFrecuencia = new ServiceFrecuenciaClient();

        private void GetData(int idTipoUsuario)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GetData(((Usuario)Session["UserData"]).IdTipoUsuario);
                lbltipoUsuario.Text = ((Usuario)Session["UserData"]).TipoUsuario.Descripcion;

                ////navCategoria.NavigateUrl = "~/manager/uploadTrainingPlan.aspx?id=" + Request.QueryString["userTipe"];
                ////switch (((Usuario)Session["UserData"]).IdTipoUsuario)
                ////{
                ////    case (int)BusinessVariables.EnumTiposUsuario.Empleado:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.Cliente:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.Proveedor:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.EmpleadoPersonaFisica:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.ClientaPersonaFisica:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.ProveedorPersonaFisica:
                ////        break;
                ////    case (int)BusinessVariables.EnumTiposUsuario.NuestraInstitucion:
                ////        break;
                ////}


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void lbtnTypeUser_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Publico/FrmUserSelect.aspx?userTipe=" + ((Usuario)Session["UserData"]).IdTipoUsuario);
        }

        protected void lbtnCategoria_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Publico/FrmCategoria.aspx?userType=" + ((Usuario)Session["UserData"]).IdTipoUsuario);
        }
    }
}