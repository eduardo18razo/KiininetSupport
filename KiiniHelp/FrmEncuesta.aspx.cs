using System;
using System.Web;
using System.Web.UI;
using KiiniHelp.ServiceSeguridad;
using KinniNet.Business.Utils;

namespace KiiniHelp
{
    public partial class FrmEncuesta : Page
    {
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
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
        private void GeneraCoockie()
        {
            try
            {
                if (Request.Cookies["miui"] != null)
                {
                    var value = BusinessQueryString.Decrypt(Request.Cookies["miui"]["iuiu"]);
                }
                else
                {
                    string llave = _servicioSeguridad.GeneraLlaveMaquina();
                    HttpCookie myCookie = new HttpCookie("miui");
                    myCookie.Values.Add("iuiu", BusinessQueryString.Encrypt(llave));
                    myCookie.Expires = DateTime.Now.AddYears(10);
                    Response.Cookies.Add(myCookie);
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                    GeneraCoockie();
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