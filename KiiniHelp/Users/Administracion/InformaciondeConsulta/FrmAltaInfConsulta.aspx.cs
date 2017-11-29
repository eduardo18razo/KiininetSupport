using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.Users.Administracion.InformaciondeConsulta
{
    public partial class FrmAltaInfConsulta : System.Web.UI.Page
    {
        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
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
                AlertaGeneral = new List<string>();
                ucAltaInformacionConsulta.OnAceptarModal += UcAltaInformacionConsultaOnOnAceptarModal;
                ucAltaInformacionConsulta.OnCancelarModal += UcAltaInformacionConsultaOnOnAceptarModal;
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

        private void UcAltaInformacionConsultaOnOnAceptarModal()
        {
            try
            {
                Response.Redirect("~/Users/Administracion/InformaciondeConsulta/FrmConsultaInformacion.aspx");
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