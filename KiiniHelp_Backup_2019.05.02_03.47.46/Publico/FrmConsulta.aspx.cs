using System;
using System.Web.UI;
using KiiniHelp.ServiceArbolAcceso;

namespace KiiniHelp.Publico
{
    public partial class FrmConsulta : Page
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}