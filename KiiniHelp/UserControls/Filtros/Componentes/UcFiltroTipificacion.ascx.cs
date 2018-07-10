using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniNet.Entities.Cat.Operacion;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroTipificacion : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
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

        public int TipoArbol
        {
            get { return Convert.ToInt32(hfTipoArbol.Value); }
            set
            {
                hfTipoArbol.Value = value.ToString();
                LlenaArbol();
            }
        }
        public List<int> TipificacionesSeleccionadas
        {
            get
            {
                return (from ListItem item in lstFiltroTipificacion.Items where item.Selected select int.Parse(item.Value)).ToList();
            }
        }
        private void LlenaArbol()
        {
            try
            {
                lstFiltroTipificacion.DataSource = _servicioArbolAcceso.ObtenerArbolesAccesoTerminalAll(null, null, TipoArbol, null, null, null, null, null, null, null);
                lstFiltroTipificacion.DataTextField = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                    "Nivel1.Descripcion",
                    "Nivel2.Descripcion",
                    "Nivel3.Descripcion",
                    "Nivel4.Descripcion",
                    "Nivel5.Descripcion",
                    "Nivel6.Descripcion",
                    "Nivel7.Descripcion");
                lstFiltroTipificacion.DataValueField = "Id";
                lstFiltroTipificacion.DataBind(); 
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
                    //LlenaArbol();
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

        protected void lstFiltroTipificacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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
    }
}