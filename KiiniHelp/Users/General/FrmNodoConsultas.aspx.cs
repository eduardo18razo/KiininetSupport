using System;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion;

namespace KiiniHelp.Users.General
{
    public partial class FrmNodoConsultas : Page
    {
        private readonly ServiceArbolAccesoClient _servicoArbol = new ServiceArbolAccesoClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    int idArbol = Convert.ToInt32(Request.QueryString["IdArbol"]);
                    ArbolAcceso arbol = _servicoArbol.ObtenerArbolAcceso(idArbol);
                    if (arbol != null)
                    {
                        ucVisorConsultainformacion.MuestraEvaluacion = arbol.Evaluacion;
                        ucVisorConsultainformacion.IdArbol = idArbol;
                    }
                    else if (Request.UrlReferrer != null) Response.Redirect(Request.UrlReferrer.ToString());
                    else
                    {
                        Response.Redirect("~/Users/FrmDashboardUser.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                UsuariosMaster master = (UsuariosMaster) Parent;
                if(master != null)
                    master.AlertaSucces(ex.Message);
            }
        }
    }
}