using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceOrganizacion;
using KiiniNet.Entities.Cat.Operacion;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroOrganizacion : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceOrganizacionClient _servicioOrganizacion = new ServiceOrganizacionClient();
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

        public List<int> Grupos
        {
            set
            {
                LlenaOrganizaciones(value);
            }
        }
        public List<int> OrganizacionesSeleccionadas
        {
            get
            {
                return (from RepeaterItem item in rptOrganizacionSeleccionada.Items select int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
            set { }
        }

        private void LlenaOrganizaciones(List<int> grupos)
        {
            try
            {

                rptOrganizaciones.DataSource = _servicioOrganizacion.ObtenerOrganizacionesGrupos(grupos);
                rptOrganizaciones.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaOrganizacionesSeleccionadas()
        {
            try
            {
                rptOrganizacionSeleccionada.DataSource = Session["OrganizacionesSeleccionadas"];
                rptOrganizacionSeleccionada.DataBind();
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
                Session["OrganizacionesSeleccionadas"] = null;
                LlenaOrganizacionesSeleccionadas();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Alerta = new List<string>();
            if (!IsPostBack)
            {
                Session["OrganizacionesSeleccionadas"] = null;
                //LlenaOrganizaciones();
            }
        }

        protected void btnSeleccionar_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<Organizacion> lst = Session["OrganizacionesSeleccionadas"] == null ? new List<Organizacion>() : (List<Organizacion>)Session["OrganizacionesSeleccionadas"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptOrganizaciones.Items[index].FindControl("lblId");

                        if (!lst.Any(a => a.Id == int.Parse(lblId.Text)))
                            lst.Add(_servicioOrganizacion.ObtenerOrganizacionById(Convert.ToInt32(lblId.Text)));
                    }
                }
                Session["OrganizacionesSeleccionadas"] = lst;
                LlenaOrganizacionesSeleccionadas();
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
                List<Organizacion> lst = Session["OrganizacionesSeleccionadas"] == null ? new List<Organizacion>() : (List<Organizacion>)Session["OrganizacionesSeleccionadas"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupo = (Label)rptOrganizacionSeleccionada.Items[index].FindControl("lblId");

                        lst.Remove(lst.Single(s => s.Id == int.Parse(lblIdGrupo.Text)));
                    }
                }
                Session["OrganizacionesSeleccionadas"] = lst;
                LlenaOrganizacionesSeleccionadas();
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