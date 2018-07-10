using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceOrganizacion;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroOrganizacion : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceOrganizacionClient _servicioOrganizacion = new ServiceOrganizacionClient();
        private List<string> _lstError = new List<string>();
        private List<string> Alerta
        {
            set
            {
                if (value.Any())
                {
                    string error = value.Aggregate("<ul>", (current, s) => current + ("<li>" + s + "</li>"));
                    error += "</ul>";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('Error','" + error + "');", true);
                }
            }
        }

        public List<int> Grupos
        {
            set
            {
                LlenaOrganizaciones(value);
            }
        }
        public List<int> OrganizacionesSeleccionadas
        {
            get
            {
                return (from ListItem item in lstFiltroOrganizacion.Items where item.Selected select int.Parse(item.Value)).ToList();
            }
        }

        private void LlenaOrganizaciones(List<int> grupos)
        {
            try
            {

                lstFiltroOrganizacion.DataSource = _servicioOrganizacion.ObtenerOrganizacionesGrupos(grupos);
                lstFiltroOrganizacion.DataTextField = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                    "Holding.Descripcion",
                    "Compania.Descripcion",
                    "Direccion.Descripcion",
                    "SubDireccion.Descripcion",
                    "Gerencia.Descripcion",
                    "SubGerencia.Descripcion",
                    "Jefatura.Descripcion");
                lstFiltroOrganizacion.DataValueField = "Id";
                lstFiltroOrganizacion.DataBind();
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
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    //LlenaOrganizaciones();
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }
    }
}