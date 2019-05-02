using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceSistemaEstatus;
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroEstatus : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();
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

        private void LlenaEstatus()
        {
            try
            {
                rcbFiltroEstatus.DataSource = _servicioEstatus.ObtenerEstatusTicket(false);
                rcbFiltroEstatus.DataTextField = "Descripcion";
                rcbFiltroEstatus.DataValueField = "Id";
                rcbFiltroEstatus.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<int> EstatusSeleccionados
        {
            get
            {
                return (from RadComboBoxItem item in rcbFiltroEstatus.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    LlenaEstatus();
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