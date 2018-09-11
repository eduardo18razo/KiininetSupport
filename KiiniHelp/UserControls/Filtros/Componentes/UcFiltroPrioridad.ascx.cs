using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceImpactourgencia;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroPrioridad : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceImpactoUrgenciaClient _servicioImpacto = new ServiceImpactoUrgenciaClient();
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
        public List<int> ImpactosSeleccionados
        {
            get
            {
                List<int>result = new List<int>();
                foreach (ListItem item in lstFiltroPrioridad.Items.Cast<ListItem>().Where(item => item.Selected))
                {
                    result.AddRange(_servicioImpacto.ObtenerAll(false).Where(w => w.Descripcion == item.Text).Select(s => s.Id).Distinct());
                }
                return result;
            }
            set { }
        }
        private void LlenaImpacto()
        {
            try
            {
                lstFiltroPrioridad.Items.Add("Alto");
                lstFiltroPrioridad.Items.Add("Medio");
                lstFiltroPrioridad.Items.Add("Bajo");
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
                    LlenaImpacto();
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