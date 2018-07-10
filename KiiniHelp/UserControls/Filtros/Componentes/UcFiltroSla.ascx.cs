using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroSla : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

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

        private void LlenaSla()
        {
            try
            {
                Dictionary<int, string> lst = new Dictionary<int, string> { { 1, "DENTRO" }, { 0, "FUERA" } };
                lstFiltroSla.DataSource = lst.ToList();
                lstFiltroSla.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<bool?> SlaSeleccionado
        {
            get
            {
                return (from ListItem item in lstFiltroSla.Items where item.Selected select item.Value == "1").Select(dummy => (bool?)dummy).ToList();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    LlenaSla();
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