using System;
using System.Web.UI;

namespace KiiniHelp
{
    public partial class FrmEncuesta : Page
    {
        public int? IdTicket
        {
            get
            {
                int result = 0;
                if (hfIdTicket.Value != string.Empty)
                    result = Convert.ToInt32(hfIdTicket.Value);
                else
                    result = (int)Session["IdTicketTicket"];
                return result;
            }
            set
            {
                if (hfIdTicket != null)
                {
                    hfIdTicket.Value = value.ToString();
                    Session.Remove("IdTicketTicket");
                }
                else
                    Session["IdTicketTicket"] = value;
            }
        }

        public int? IdTipoServicio
        {
            get
            {
                int result = 0;
                if (hfIdTipoServicio.Value != string.Empty)
                    result = Convert.ToInt32(hfIdTipoServicio.Value);
                else
                    result = (int)Session["IdTipoServicio"];
                return result;
            }
            set
            {
                if (hfIdTipoServicio != null)
                {
                    hfIdTipoServicio.Value = value.ToString();
                    Session.Remove("IdTipoServicio");
                }
                else
                    Session["IdTipoServicio"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucEncuestaCaptura.OnAceptarModal += UcEncuestaCaptura_OnAceptarModal;
                ucEncuestaCaptura.OnCancelarModal += UcEncuestaCaptura_OnCancelarModal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void UcEncuestaCaptura_OnAceptarModal()
        {
            Response.Redirect(ResolveUrl("FrmCloseWindow.aspx"));
        }

        void UcEncuestaCaptura_OnCancelarModal()
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                IdTipoServicio = Convert.ToInt32(Request.QueryString["IdTipoServicio"]);
                IdTicket = Convert.ToInt32(Request.QueryString["IdTicket"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}