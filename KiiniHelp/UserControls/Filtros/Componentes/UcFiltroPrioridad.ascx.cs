using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
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
                panelAlerta.Visible = value.Any();
                if (!panelAlerta.Visible) return;
                rptError.DataSource = value;
                rptError.DataBind();
            }
        }
        public List<int> ImpactosSeleccionados
        {
            get
            {
                return (from RepeaterItem item in rptImpactoSeleccionado.Items select int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
            set { }
        }
        private void LlenaImpacto()
        {
            try
            {
                rptImpacto.DataSource = _servicioImpacto.ObtenerAll(false);
                rptImpacto.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaImpactoSeleccionado()
        {
            try
            {
                rptImpactoSeleccionado.DataSource = Session["ImpactoSeleccionado"];
                rptImpactoSeleccionado.DataBind();
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
                Session["ImpactoSeleccionado"] = null;
                LlenaImpactoSeleccionado();
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
                    Session["ImpactoSeleccionado"] = null;
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

        protected void btnSeleccionar_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<Impacto> lst = Session["ImpactoSeleccionado"] == null ? new List<Impacto>() : (List<Impacto>)Session["ImpactoSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptImpacto.Items[index].FindControl("lblId");

                        if (!lst.Any(a => a.Id == int.Parse(lblId.Text)))
                            lst.Add(_servicioImpacto.ObtenerImpactoById(int.Parse(lblId.Text)));
                    }
                }
                Session["ImpactoSeleccionado"] = lst;
                LlenaImpactoSeleccionado();
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
                List<Impacto> lst = Session["ImpactoSeleccionado"] == null ? new List<Impacto>() : (List<Impacto>)Session["ImpactoSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptImpactoSeleccionado.Items[index].FindControl("lblId");

                        lst.Remove(lst.Single(s => s.Id == int.Parse(lblId.Text)));
                    }
                }
                Session["ImpactoSeleccionado"] = lst;
                LlenaImpactoSeleccionado();
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