using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.Publico.Consultas
{
    public partial class FrmConsultaTicket : Page
    {
        private readonly ServiceTicketClient _servicioticket = new ServiceTicketClient();

        private List<string> _lstError = new List<string>();

        private List<string> Alerta
        {
            set
            {
                if (value.Any())
                {
                    string error = value.Aggregate("<ul>", (current, s) => current + ("<li>" + s + "</li>"));
                    error += "</ul>";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('Error','" + error + "');", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnConsultar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtTicket.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese número de ticket");
                if (txtClave.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese clave de registro");
                HelperDetalleTicket detalle = _servicioticket.ObtenerDetalleTicketNoRegistrado(int.Parse(txtTicket.Text.Trim()), txtClave.Text.Trim());
                if (detalle != null)
                {
                    divTitle.Visible = false;
                    divConsulta.Visible = false;
                    divDetalle.Visible = true;
                    divDetalleTicket.Visible = true;
                    ucTicketDetalleUsuario.IdUsuario = detalle.IdUsuarioLevanto;
                    ucTicketDetalleUsuario.IdTicket = detalle.IdTicket;
                    hfMuestraEncuesta.Value = detalle.TieneEncuesta.ToString();
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }
    }
}