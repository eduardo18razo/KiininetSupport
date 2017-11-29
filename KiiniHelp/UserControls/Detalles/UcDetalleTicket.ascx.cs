using System;
using System.Globalization;
using System.Web.UI;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleTicket : UserControl
    {
        private readonly ServiceTicketClient _servicioTicket = new ServiceTicketClient();

        public int IdTicket
        {
            get { return Convert.ToInt32(ViewState["ticketDetalle"].ToString()); }
            set
            {
                ViewState["ticketDetalle"] = value; 
                ObtenerDetalle();
                ucDetalleMascaraCaptura.IdTicket = value;
            }
        }

        private void ObtenerDetalle()
        {
            try
            {
                HelperDetalleTicket detalle = _servicioTicket.ObtenerDetalleTicket(IdTicket);
                lblticket.Text = detalle.IdTicket.ToString();
                lblestatus.Text = detalle.EstatusActual;
                lblAsignacion.Text = detalle.AsignacionActual;
                lblfecha.Text = detalle.FechaCreacion.ToString(CultureInfo.InvariantCulture);
                gvEstatus.DataSource = detalle.EstatusDetalle;
                gvEstatus.DataBind();
                gvAsignaciones.DataSource = detalle.AsignacionesDetalle;
                gvAsignaciones.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
    }
}