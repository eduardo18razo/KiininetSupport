using System;

namespace KiiniHelp
{
    public partial class FrmCloseWindow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        }
    }
}