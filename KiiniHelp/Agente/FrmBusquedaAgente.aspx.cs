using System;
using System.Web.UI;

namespace KiiniHelp.Agente
{
    public partial class FrmBusquedaAgente : Page
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