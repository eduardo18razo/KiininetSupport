using System;
using System.Web.UI;

namespace KiiniHelp.Publico
{
    public partial class FrmBusqueda : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Params["w"] != null && !string.IsNullOrEmpty(Request.Params["w"]))
                {
                    string urlName = Request.UrlReferrer.ToString();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}