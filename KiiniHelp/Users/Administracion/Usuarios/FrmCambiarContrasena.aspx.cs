using System;
using KinniNet.Business.Utils;

namespace KiiniHelp.Users.Administracion.Usuarios
{
    public partial class FrmCambiarContrasena : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucCambiarContrasena.OnAceptarModal += ucCambiarContrasena_OnAceptarModal;
        }

        private int? RolSeleccionado
        {
            get { return Session["RolSeleccionado"] == null ? null : string.IsNullOrEmpty(Session["RolSeleccionado"].ToString()) ? null : (int?)int.Parse(Session["RolSeleccionado"].ToString()); }
            set { Session["RolSeleccionado"] = value.ToString(); }
        }
        void ucCambiarContrasena_OnAceptarModal()
        {
            try
            {
                switch (RolSeleccionado)
                {
                    case (int)BusinessVariables.EnumRoles.Agente:
                        Response.Redirect("~/Agente/DashBoardAgente.aspx");
                        break;
                    case (int)BusinessVariables.EnumRoles.Administrador:
                        Response.Redirect("~/Users/DashBoard.aspx");
                        break;
                    default:
                        Response.Redirect("~/Users/FrmDashboardUser.aspx");
                        break;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}