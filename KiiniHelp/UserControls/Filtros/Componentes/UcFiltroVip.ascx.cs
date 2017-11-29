using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroVip : System.Web.UI.UserControl, IControllerModal
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
                panelAlerta.Visible = value.Any();
                if (!panelAlerta.Visible) return;
                rptError.DataSource = value;
                rptError.DataBind();
            }
        }

        private void LlenaVip()
        {
            try
            {
                Dictionary<int, string> lst = new Dictionary<int, string> { { 1, "VIP" }, { 0, "NO VIP" } };
                rptVip.DataSource = lst.ToList();
                rptVip.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaVipSeleccionado()
        {
            try
            {
                rptVipSeleccionado.DataSource = Session["VipSeleccionado"];
                rptVipSeleccionado.DataBind();
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
                Session["VipSeleccionado"] = null;
                LlenaVipSeleccionado();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<bool?> VipSeleccionado
        {
            get
            {
                return (from RepeaterItem item in rptVipSeleccionado.Items select (bool?)(((Label) item.FindControl("lblId")).Text == "1")).ToList();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    Session["VipSeleccionado"] = null;
                    LlenaVip();
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
                Dictionary<int, string> lst = Session["VipSeleccionado"] == null ? new Dictionary<int, string>() : (Dictionary<int, string>)Session["VipSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptVip.Items[index].FindControl("lblId");
                        Label lblDescripcion = (Label)rptVip.Items[index].FindControl("lblDescripcion");

                        if (lst.Count <= 0)
                            lst.Add(Convert.ToInt32(lblId.Text), lblDescripcion.Text);
                    }
                }
                Session["VipSeleccionado"] = lst;
                LlenaVipSeleccionado();
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
                Dictionary<int, string> lst = Session["VipSeleccionado"] == null ? new Dictionary<int, string>() : (Dictionary<int, string>)Session["VipSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupo = (Label)rptVipSeleccionado.Items[index].FindControl("lblId");

                        lst.Remove(int.Parse(lblIdGrupo.Text));
                    }
                }
                Session["VipSeleccionado"] = lst;
                LlenaVipSeleccionado();
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