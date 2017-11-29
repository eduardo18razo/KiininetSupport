using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
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
                panelAlerta.Visible = value.Any();
                if (!panelAlerta.Visible) return;
                rptError.DataSource = value;
                rptError.DataBind();
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
                return (from RepeaterItem item in rptArbolSeleccionado.Items select int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
            set { }
        }
        private void LlenaArbol()
        {
            try
            {
                rptArbol.DataSource = _servicioArbolAcceso.ObtenerArbolesAccesoTerminalAll(null, null, TipoArbol, null, null, null, null, null, null, null);
                rptArbol.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaArbolSeleccionados()
        {
            try
            {
                rptArbolSeleccionado.DataSource = Session["ArbolSeleccionado"];
                rptArbolSeleccionado.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void Limpiar()
        {
            try
            {
                Session["ArbolSeleccionado"] = null;
                LlenaArbolSeleccionados();
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
                    Session["ArbolSeleccionado"] = null;
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

        protected void btnSeleccionar_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<ArbolAcceso> lst = Session["ArbolSeleccionado"] == null ? new List<ArbolAcceso>() : (List<ArbolAcceso>)Session["ArbolSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptArbol.Items[index].FindControl("lblId");

                        if (!lst.Any(a => a.Id == int.Parse(lblId.Text)))
                            lst.Add(_servicioArbolAcceso.ObtenerArbolAcceso(int.Parse(lblId.Text)));
                    }
                }
                Session["ArbolSeleccionado"] = lst;
                LlenaArbolSeleccionados();
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

        protected void btnQuitar_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<ArbolAcceso> lst = Session["ArbolSeleccionado"] == null ? new List<ArbolAcceso>() : (List<ArbolAcceso>)Session["ArbolSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupo = (Label)rptArbolSeleccionado.Items[index].FindControl("lblId");

                        lst.Remove(lst.Single(s => s.Id == int.Parse(lblIdGrupo.Text)));
                    }
                }
                Session["ArbolSeleccionado"] = lst;
                LlenaArbolSeleccionados();
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
                Limpiar();
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