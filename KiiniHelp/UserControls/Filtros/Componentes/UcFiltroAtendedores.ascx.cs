using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroAtendedores : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceUsuariosClient _servicioUsuario = new ServiceUsuariosClient();
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

        public List<int?> Encuestas
        {
            set { LlenaUsuarios(value); }
        }
        public List<int> TipoUsuario
        {
            get { return Session["TipoUsuarioFiltroAtendedores"] == null ? null : (List<int>)Session["TipoUsuarioFiltroAtendedores"]; }
            set
            {
                Session["TipoUsuarioFiltroAtendedores"] = value;
                LlenaUsuarios(new List<int?>());
            }
        }
        public List<int> AtendedoresSeleccionados
        {
            get
            {
                return (from RadComboBoxItem item in rcbFiltroAgentes.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }
        private void LlenaUsuarios(List<int?> encuestas)
        {
            try
            {
                List<Usuario> lst = _servicioUsuario.ObtenerAgentes(false);
                if (TipoUsuario != null)
                    lst = lst.Where(w => TipoUsuario.Contains(w.IdTipoUsuario)).ToList();
                rcbFiltroAgentes.DataSource = lst;
                rcbFiltroAgentes.DataTextField = "NombreCompleto";
                rcbFiltroAgentes.DataValueField = "Id";
                rcbFiltroAgentes.DataBind();
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
                    LlenaUsuarios(new List<int?>());
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