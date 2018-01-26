using System;


using System.Collections.Generic;
using System.Data;
using KiiniHelp.ServiceDashboard;
using KiiniNet.Entities.Operacion.Dashboard;
using KinniNet.Business.Utils;


namespace KiiniHelp.Users
{
    public partial class DashBoard : System.Web.UI.Page
    {

        private readonly ServiceDashboardsClient _servicioDashboard = new ServiceDashboardsClient();

        private List<string> _lstError = new List<string>();

        private void LlenaDatos()
        {
            try
            {
                DashboardAdministrador datos = _servicioDashboard.GetDashboardAdministrador();
                lblUsuariosRegistrados.Text = datos.UsuariosRegistrados.ToString();
                lblUsuariosActivos.Text = datos.UsuariosActivos.ToString();
                lblTicketsCreados.Text = datos.TicketsCreados.ToString();
                lblOperadoresAcumulados.Text = datos.Operadores.ToString();

                lblEspacio.Text = string.Format("{0} MB de {1} Mb en uso ", datos.GraficoAlmacenamiento.Rows[0][0], double.Parse(datos.GraficoAlmacenamiento.Rows[0][0].ToString()) + double.Parse(datos.GraficoAlmacenamiento.Rows[0][1].ToString()));
                lblArchivos.Text = string.Format("{0} archivos adjuntos", datos.TotalArchivos);

                lblCategorias.Text = datos.Categorias.ToString();
                lblArticulos.Text = datos.Articulos.ToString();
                lblFormularios.Text = datos.Formularios.ToString();
                lblCatalogos.Text = datos.Catalogos.ToString();

                lblOrganizaciones.Text = datos.Organizacion.ToString();
                lblUbicaciones.Text = datos.Ubicacion.ToString();
                lblPuestos.Text = datos.Puestos.ToString();

                lblGrupos.Text = datos.Grupos.ToString();
                lblHorarios.Text = datos.Horarios.ToString();
                lblFeriados.Text = datos.Feriados.ToString();
                rptOperadorRol.DataSource = datos.OperadorRol;
                rptOperadorRol.DataBind();

                BusinessGraficosDasboard.Administrador.GeneraGraficoPastel(cGraficoTicketsCanal, datos.GraficoTicketsCreadosCanal);
                BusinessGraficosDasboard.Administrador.GeneraGraficoPastel(cGraficoUsuarios, datos.GraficoUsuariosRegistrados);
                BusinessGraficosDasboard.Administrador.GeneraGraficoBarraApilada(cGraficoEspacio, datos.GraficoAlmacenamiento);
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    LlenaDatos();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}