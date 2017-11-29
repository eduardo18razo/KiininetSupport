using System;

namespace KiiniHelp.Users.Administracion.Usuarios
{
    public partial class FrmCambiarContrasena : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucCambiarContrasena.OnAceptarModal += ucCambiarContrasena_OnAceptarModal;
        }

        void ucCambiarContrasena_OnAceptarModal()
        {
            try
            {
                Response.Redirect("~/Users/DashBoard.aspx");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}