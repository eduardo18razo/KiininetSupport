using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.Agente
{
    public partial class FrmTicket : Page
    {

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
                _lstError = new List<string>();
                UcTicketDetalle.OnCargarTicket +=UcTicketDetalle_OnCargarTicket;
                UcTicketDetalle.OnCierraTicket += UcTicketDetalle_OnCierraTicket;
                if (!IsPostBack)
                {
                    AgenteMaster master = Master as AgenteMaster;
                    if (master != null)
                    {
                        master.CambiaTicket = true;
                        
                    }
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

        private void UcTicketDetalle_OnCierraTicket(int idTicket, bool redirect)
        {
            try
            {
                AgenteMaster master = Master as AgenteMaster;
                if (master != null)
                {
                    master.CambiaTicket = true;
                    master.RemoveTicketOpen(idTicket, redirect);

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

        private void UcTicketDetalle_OnCargarTicket(int idTicket, string titulo, bool asigna)
        {
            try
            {
                AgenteMaster master = Master as AgenteMaster;
                if (master != null)
                {
                    master.CambiaTicket = true;
                    master.AddTicketOpen(idTicket, titulo, asigna);

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