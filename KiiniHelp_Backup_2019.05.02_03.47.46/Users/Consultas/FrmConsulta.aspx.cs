using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.Users.Consultas
{
    public partial class FrmConsulta : Page
    {
        private readonly ServiceTicketClient _servicioticket = new ServiceTicketClient();

        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
        {
            set
            {
                pnlAlertaGeneral.Visible = value.Any();
                if (!pnlAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnConsultar_OnClick(object sender, EventArgs e)
        {
            try
            {
                HelperDetalleTicket detalle = _servicioticket.ObtenerDetalleTicketNoRegistrado(int.Parse(txtTicket.Text.Trim()), txtClave.Text.Trim());
                divResultado.Visible = detalle != null;
                if (detalle != null)
                {
                    lblticket.Text = detalle.IdTicket.ToString();
                    lblestatus.Text = detalle.EstatusActual;
                    lblAsignacion.Text = detalle.AsignacionActual;
                    lblfecha.Text = detalle.FechaCreacion.ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
    }
}