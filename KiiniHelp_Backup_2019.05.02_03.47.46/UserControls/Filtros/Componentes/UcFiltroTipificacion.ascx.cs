using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceArbolAcceso;
using KiiniNet.Entities.Cat.Operacion;
using Telerik.Web.UI;

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
        public List<int> TipoUsuario
        {
            get { return Session["TipoUsuarioFiltroTipificacion"] == null ? null : (List<int>)Session["TipoUsuarioFiltroTipificacion"]; }
            set
            {
                Session["TipoUsuarioFiltroTipificacion"] = value;
                LlenaArbol();
            }
        }

        public List<int> TipoArbol
        {
            get { return Session["TipoArbolFiltroTipificacion"] == null ? null : (List<int>) Session["TipoArbolFiltroTipificacion"]; }
            set
            {
                Session["TipoArbolFiltroTipificacion"] = value;
                LlenaArbol();
            }
        }
        public List<int> TipificacionesSeleccionadas
        {
            get
            {
                return (from RadComboBoxItem item in rcbFiltroTipificacion.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }
        private void LlenaArbol()
        {
            try
            {
                List<ArbolAcceso> lst = _servicioArbolAcceso.ObtenerArbolesAccesoTerminalAll(null, null, null, null, null, null, null, null, null, null);
                if (TipoUsuario != null && TipoUsuario.Count > 0)
                    lst = lst.Where(w => TipoUsuario.Contains(w.IdTipoUsuario)).ToList();
                if (TipoArbol != null && TipoArbol.Any())
                    lst = lst.Where(w => TipoArbol.Contains(w.IdTipoArbolAcceso)).ToList();
                rcbFiltroTipificacion.DataSource = lst;
                rcbFiltroTipificacion.DataTextField = "Tipificacion";
                rcbFiltroTipificacion.DataValueField = "Id";
                rcbFiltroTipificacion.DataBind();
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
                    LlenaArbol();
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

        protected void rdcFiltroTipificacion_OnSelectedIndexChanged(object sender, EventArgs e)
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