using System;
using System.Web.UI;

namespace KiiniHelp.Users.Administracion.ArbolesAcceso
{
    public partial class FrmDetalleOpcion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Params["IdArbol"] != null)
                {
                    ucDetalleArbolAcceso.IdArbolAcceso = int.Parse(Request.Params["IdArbol"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}