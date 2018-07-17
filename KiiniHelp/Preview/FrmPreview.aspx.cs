using System;
using System.Web.UI;

namespace KiiniHelp.Preview
{
    public partial class FrmPreview : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Params["urlGoogle"] != null)
                {
                    iPage.Visible = true;
                    iPage.Attributes.Add("src", Request.Params["urlGoogle"] + "&embedded=true");
                }
                
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}