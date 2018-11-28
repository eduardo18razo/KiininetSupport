using System;
using System.Web;
using System.Web.UI;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceUsuario;
using KinniNet.Business.Utils;

namespace KiiniHelp
{
    public partial class FrmConfirmacionCorreo : Page
    {
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
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
                if (Request.Params["confirmacionCorreo"] != null)
                {
                    string[] values = Request.Params["confirmacionCorreo"].Split('_');
                    if (_servicioUsuarios.ValidaConfirmacionCambioCorreo(int.Parse(values[0]), values[1]))
                    {
                        lblMsg.Text = "Su correo ha sido confirmado.";
                        Response.Redirect(ResolveUrl("~/Default.aspx"));
                    }
                    else
                    {
                        lblMsg.Text = "Token invalido";
                    }
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void lnkBtnContinuar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}