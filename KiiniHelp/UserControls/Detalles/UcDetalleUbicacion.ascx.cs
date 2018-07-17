using System;
using System.Collections.Generic;
using System.Web.UI;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Cat.Operacion;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleUbicacion : UserControl
    {
        private List<string> _lstError = new List<string>();

        public int IdUbicacion
        {
            set
            {
                Ubicacion ub = new ServiceUbicacionClient().ObtenerUbicacionById(value);
                if (ub == null) return;
                List<Ubicacion> source = new List<Ubicacion> { ub };
                rptUbicacion.DataSource = source;
                rptUbicacion.DataBind();
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