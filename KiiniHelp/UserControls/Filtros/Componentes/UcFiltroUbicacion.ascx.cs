using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Cat.Operacion;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroUbicacion : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceUbicacionClient _servicioUbicacion = new ServiceUbicacionClient();
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
                LlenaUbicaciones(value);
            }
        }
        public List<int> UbicacionesSeleccionadas
        {
            get
            {
                return (from ListItem item in lstFiltroUbicacion.Items where item.Selected select int.Parse(item.Value)).ToList();
            }
        }
        private void LlenaUbicaciones(List<int> grupos)
        {
            try
            {
                lstFiltroUbicacion.DataSource = _servicioUbicacion.ObtenerUbicacionesGrupos(grupos);
                lstFiltroUbicacion.DataTextField = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                    "Pais.Descripcion",
                    "Campus.Descripcion",
                    "Torre.Descripcion",
                    "Piso.Descripcion",
                    "Zona.Descripcion",
                    "SubZona.Descripcion",
                    "SiteRack.Descripcion");
                lstFiltroUbicacion.DataValueField = "Id";
                lstFiltroUbicacion.DataBind();          
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
                    //LlenaUbicaciones();
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