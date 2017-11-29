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
                panelAlerta.Visible = value.Any();
                if (!panelAlerta.Visible) return;
                rptError.DataSource = value;
                rptError.DataBind();
            }
        }

        private void LlenaSla()
        {
            try
            {
                Dictionary<int, string> lst = new Dictionary<int, string> { { 1, "DENTRO" }, { 0, "FUERA" } };
                rptSla.DataSource = lst.ToList();
                rptSla.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaSlaSeleccionado()
        {
            try
            {
                rptSlaSeleccionado.DataSource = Session["SlaSeleccionado"];
                rptSlaSeleccionado.DataBind();
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
                Session["SlaSeleccionado"] = null;
                LlenaSlaSeleccionado();
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
                List<bool?> result = new List<bool?>();
                foreach (RepeaterItem item in rptSlaSeleccionado.Items)
                {
                    result.Add(((Label)item.FindControl("lblId")).Text != "0");
                }
                return result;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    Session["SlaSeleccionado"] = null;
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

        protected void btnSeleccionar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<int, string> lst = Session["SlaSeleccionado"] == null ? new Dictionary<int, string>() : (Dictionary<int, string>)Session["SlaSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptSla.Items[index].FindControl("lblId");
                        Label lblDescripcion = (Label)rptSla.Items[index].FindControl("lblDescripcion");

                        if (lst.Count <= 0)
                            lst.Add(Convert.ToInt32(lblId.Text), lblDescripcion.Text);
                    }
                }
                Session["SlaSeleccionado"] = lst;
                LlenaSlaSeleccionado();
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
                Dictionary<int, string> lst = Session["SlaSeleccionado"] == null ? new Dictionary<int, string>() : (Dictionary<int, string>)Session["SlaSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupo = (Label)rptSlaSeleccionado.Items[index].FindControl("lblId");

                        lst.Remove(int.Parse(lblIdGrupo.Text));
                    }
                }
                Session["SlaSeleccionado"] = lst;
                LlenaSlaSeleccionado();
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