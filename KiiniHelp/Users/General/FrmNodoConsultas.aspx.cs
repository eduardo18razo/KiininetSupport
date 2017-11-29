using System;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceInformacionConsulta;
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
                    ucVisorConsultainformacion.MuestraEvaluacion = _servicoArbol.ObtenerArbolAcceso(idArbol).Evaluacion;
                    ucVisorConsultainformacion.IdArbol = idArbol;
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