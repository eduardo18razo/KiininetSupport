using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceArea;
using KiiniNet.Entities.Operacion;
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroCategoria : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();
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
            get { return Session["TipoUsuarioFiltroGrupo"] == null ? null : (List<int>)Session["TipoUsuarioFiltroGrupo"]; }
            set
            {
                Session["TipoUsuarioFiltroGrupo"] = value.ToString();
                LlenaCategorias();
            }
        }
        public List<int> CategoriasSeleccionadas
        {
            get
            {
                return (from RadComboBoxItem item in rcbFiltroCategoria.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }

        private void LlenaCategorias()
        {
            try
            {
                List<Area> lstCategorias = _servicioArea.ObtenerAreas(false);
                rcbFiltroCategoria.DataSource = lstCategorias;
                rcbFiltroCategoria.DataTextField = "Descripcion";
                rcbFiltroCategoria.DataValueField = "Id";
                rcbFiltroCategoria.DataBind();
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
                    LlenaCategorias();
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