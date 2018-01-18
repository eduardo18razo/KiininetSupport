using System;


using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceConsultas;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;




namespace KiiniHelp.Users
{
    public partial class DashBoard : System.Web.UI.Page
    {

        private readonly ServiceConsultasClient _servicioConsultas = new ServiceConsultasClient();

        private List<string> _lstError = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            //BusinessGraficoStack.Pareto.GenerarGrafica(cGrafico, _servicioConsultas.GraficarConsultaTicket(((Usuario)Session["UserData"]).Id,
            //    ucFiltrosGraficas.FiltroGrupos, 
            //    ucFiltrosGraficas.FiltroTipoUsuario,
            //    ucFiltrosGrafico.OrganizacionesGraficar.Select(s => s.Id).ToList(),
            //                ucFiltrosGrafico.UbicacionesGraficar.Select(s => s.Id).ToList(),
            //                ucFiltrosGrafico.TipoTicket.Select(s => s.Id).ToList(),
            //                ucFiltrosGrafico.TipificacionesGraficar.Select(s => s.Id).ToList(),
            //                ucFiltrosGraficas.FiltroPrioridad, ucFiltrosGrafico.EstatusStack.Select(s => s.Id).ToList(),
            //                ucFiltrosGrafico.SlaGraficar.Select(s => (bool?)s.Key).ToList(), ucFiltrosGraficas.FiltroVip,
            //                ucFiltrosGraficas.FiltroFechas, ucFiltrosGrafico.FiltroStack, ucFiltrosGrafico.Stack, ucFiltrosGraficas.TipoPeriodo), 
            //                ucFiltrosGrafico.Stack);
        
            cGrafico.Visible = true;
            //upGrafica.Visible = true;



        }
    }
}