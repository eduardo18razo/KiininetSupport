using System;

namespace KiiniHelp.Users.Administracion.Formularios
{
    public partial class FrmPreviewFormulario1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucPreviewFormulario.Preview = true;
        }
    }
}