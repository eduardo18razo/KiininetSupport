using System;
using System.Collections.Generic;
using KiiniHelp.ServiceOrganizacion;
using KiiniNet.Entities.Cat.Operacion;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleOrganizacion : System.Web.UI.UserControl
    {
        public int IdOrganizacion
        {
            set
            {
                Organizacion ub = new ServiceOrganizacionClient().ObtenerOrganizacionById(value);
                List<Organizacion> source = new List<Organizacion> {ub};
                rptOrganizacion.DataSource = source;
                rptOrganizacion.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}