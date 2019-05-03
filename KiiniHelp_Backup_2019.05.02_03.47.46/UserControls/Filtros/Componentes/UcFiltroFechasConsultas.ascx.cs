using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroFechasConsultas : UserControl
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;

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

        public Dictionary<string, DateTime> RangoFechas
        {
            get
            {
                Dictionary<string, DateTime> result = null;
                if (txtFechaInicio.Text.Trim() != string.Empty && txtFechaFin.Text.Trim() != string.Empty)
                    result = new Dictionary<string, DateTime>
                {
                    {"inicio", DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", null)},
                    {"fin", DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", null)}
                };
                return result;
            }
        }

        private void ValidaFechas()
        {
            try
            {
                if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                    throw new Exception("Debe Seleccionar un rango de fechas");
                if (DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", null) > DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", null))
                    throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void LlenaFechas()
        {
            try
            {
                //txtFechaInicio.Text = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
                //txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                if (!IsPostBack)
                {
                    LlenaFechas();
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

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaFechas();
                if (OnAceptarModal != null)
                    OnAceptarModal();
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

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            try
            {
                txtFechaInicio.Text = string.Empty;
                txtFechaFin.Text = string.Empty;
                if (OnLimpiarModal != null)
                    OnLimpiarModal();
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

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                var valida = RangoFechas;
                if (OnCancelarModal != null)
                    OnCancelarModal();
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