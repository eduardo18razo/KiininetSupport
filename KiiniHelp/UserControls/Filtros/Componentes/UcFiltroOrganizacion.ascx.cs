using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceOrganizacion;
using KiiniNet.Entities.Cat.Operacion;
using Telerik.Web.UI;

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
        
        public List<int> TipoUsuario
        {
            get { return Session["TipoUsuarioFiltroOrganizacion"] == null ? null : (List<int>)Session["TipoUsuarioFiltroOrganizacion"]; }
            set
            {
                Session["TipoUsuarioFiltroOrganizacion"] = value;
                LlenaOrganizaciones(value);
            }
        }
        public List<int> OrganizacionesSeleccionadas
        {
            get
            {
                return (from RadComboBoxItem item in rcbFiltroOrganizacion.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }

        private void LlenaOrganizaciones(List<int> tiposUsuario )
        {
            try
            {
                List<Organizacion> lst = _servicioOrganizacion.ObtenerOrganizacionesTipoUsuario(tiposUsuario);
                if (TipoUsuario != null && TipoUsuario.Count > 0)
                    lst = lst.Where(w => TipoUsuario.Contains(w.IdTipoUsuario)).ToList();
                rcbFiltroOrganizacion.Items.Clear();
                foreach (Organizacion organizacion in lst)
                {
                    rcbFiltroOrganizacion.Items.Add(new RadComboBoxItem(string.Format("{0} {1}",
                        organizacion.Holding.Descripcion,
                        organizacion.IdCompania != null ? organizacion.Compania.Descripcion : string.Empty), organizacion.Id.ToString()));
                }
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
                    LlenaOrganizaciones(null);
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