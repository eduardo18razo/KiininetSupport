using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroTipoUsuario : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoUsuarioClient _serviciotipoUsuario = new ServiceTipoUsuarioClient();
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

        private void LlenaTipoUsuario()
        {
            try
            {
                rptTipoUsuario.DataSource = _serviciotipoUsuario.ObtenerTiposUsuario(false).OrderBy(s => s.Descripcion);
                rptTipoUsuario.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaTipoUsuarioSeleccionado()
        {
            try
            {
                //if (Session["TipoUsuarioSeleccionado"] == null) return;
                rptTipoUsuarioSeleccionado.DataSource = Session["TipoUsuarioSeleccionado"];
                rptTipoUsuarioSeleccionado.DataBind();
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
                Session["TipoUsuarioSeleccionado"] = null;
                LlenaTipoUsuarioSeleccionado();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<int> TipoUsuarioSeleccionados
        {
            get
            {
                return (from RepeaterItem item in rptTipoUsuarioSeleccionado.Items select int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    Session["TipoUsuarioSeleccionado"] = null;
                    LlenaTipoUsuario();
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
                List<TipoUsuario> lst = Session["TipoUsuarioSeleccionado"] == null ? new List<TipoUsuario>() : (List<TipoUsuario>)Session["TipoUsuarioSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptTipoUsuario.Items[index].FindControl("lblId");
                        Label lblDescripcion = (Label)rptTipoUsuario.Items[index].FindControl("lblDescripcion");

                        if (!lst.Any(a => a.Id == int.Parse(lblId.Text)))
                            lst.Add(new TipoUsuario
                            {
                                Id = Convert.ToInt32(lblId.Text),
                                Descripcion = lblDescripcion.Text
                            });
                    }
                }
                Session["TipoUsuarioSeleccionado"] = lst;
                LlenaTipoUsuarioSeleccionado();
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
                List<TipoUsuario> lst = Session["TipoUsuarioSeleccionado"] == null ? new List<TipoUsuario>() : (List<TipoUsuario>)Session["TipoUsuarioSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptTipoUsuarioSeleccionado.Items[index].FindControl("lblId");

                        lst.Remove(lst.Single(s => s.Id == int.Parse(lblId.Text)));
                    }
                }
                Session["TipoUsuarioSeleccionado"] = lst;
                LlenaTipoUsuarioSeleccionado();
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