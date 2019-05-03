using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceSistemaCanal;
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroCanalApertura : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceCanalClient _servicioCanal = new ServiceCanalClient();
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

        public List<int> CanalesSeleccionados
        {
            get
            {
                return (from RadComboBoxItem item in rcbFiltroCanalApertura.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }
        private void LlenaCanal()
        {
            try
            {
                rcbFiltroCanalApertura.DataSource = _servicioCanal.ObtenerCanalesAll(false);
                rcbFiltroCanalApertura.DataTextField = "Descripcion";
                rcbFiltroCanalApertura.DataValueField = "Id";
                rcbFiltroCanalApertura.DataBind();
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
                    LlenaCanal();
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