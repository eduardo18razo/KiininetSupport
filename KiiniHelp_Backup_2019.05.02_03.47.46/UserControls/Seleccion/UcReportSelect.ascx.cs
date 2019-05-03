using System;
using System.Web.UI;

namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcReportSelect : UserControl, IControllerModal
    {

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserData"] == null)
                    Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + Request.Params["userType"]);
                else
                    Response.Redirect("~/Users/DashBoard.aspx");
            }
            catch
            {
            }
        }
    }
}