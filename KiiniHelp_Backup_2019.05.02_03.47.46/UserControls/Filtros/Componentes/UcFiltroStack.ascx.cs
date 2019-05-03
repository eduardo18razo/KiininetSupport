using System;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroStack : System.Web.UI.UserControl
    {
        public void Geografico()
        {
            try
            {
                btnUbicacion.Visible = false;
                btnOrganizacion.Visible = false;
                btnTipificacion.Visible = false;
                btnTipificacionOpcion.Visible = false;
                btnEstatus.Visible = false;
                btnSla.Visible = false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void Pareto()
        {
            try
            {
                btnUbicacion.Visible = true;
                btnOrganizacion.Visible = true;
                btnTipificacion.Visible = true;
                btnTipificacionOpcion.Visible = true;
                btnEstatus.Visible = true;
                btnSla.Visible = false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void TendenciaStack()
        {
            try
            {
                btnUbicacion.Visible = true;
                btnOrganizacion.Visible = true;
                btnTipificacion.Visible = true;
                btnTipificacionOpcion.Visible = true;
                btnEstatus.Visible = true;
                btnSla.Visible = true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void TendenciaBarraCompetitiva()
        {
            try
            {
                btnUbicacion.Visible = true;
                btnOrganizacion.Visible = true;
                btnTipificacion.Visible = true;
                btnTipificacionOpcion.Visible = true;
                btnEstatus.Visible = true;
                btnSla.Visible = true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}