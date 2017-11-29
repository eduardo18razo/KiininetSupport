using System;
using System.Web.UI;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.Users.General
{
    public partial class FrmMiInformacion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UcDetalleUsuario.FromModal = false;
                if (!IsPostBack)
                    if (Session["UserData"] != null)
                        UcDetalleUsuario.IdUsuario = ((Usuario)Session["UserData"]).Id;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}