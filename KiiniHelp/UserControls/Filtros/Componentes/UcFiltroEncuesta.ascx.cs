using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceEncuesta;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroEncuesta : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceEncuestaClient _servicioEncuestas = new ServiceEncuestaClient();
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
                LlenaEncuestas(value);
            }
        }
        
        public List<int?> EncuestasSeleccionadas
        {
            get
            {
                return (from RepeaterItem item in rptEncuestasSeleccionadas.Items select (int?)int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
        }
        private void LlenaEncuestas(List<int> grupos)
        {
            try
            {
                rptEncuestas.DataSource = _servicioEncuestas.ObtenerEncuestaByGrupos(grupos, false);
                rptEncuestas.DataBind();
                upEncuestas.Update();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaEncuestasSeleccionadas()
        {
            try
            {
                rptEncuestasSeleccionadas.DataSource = Session["EncuestasSeleccionadas"];
                rptEncuestasSeleccionadas.DataBind();
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
                Session["EncuestasSeleccionadas"] = null;
                LlenaEncuestasSeleccionadas();
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
                    Session["EncuestasSeleccionadas"] = null;
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
                List<Encuesta> lst = Session["EncuestasSeleccionadas"] == null ? new List<Encuesta>() : (List<Encuesta>)Session["EncuestasSeleccionadas"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptEncuestas.Items[index].FindControl("lblId");

                        if (!lst.Any(a => a.Id == int.Parse(lblId.Text)))
                            lst.Add(_servicioEncuestas.ObtenerEncuestaById(int.Parse(lblId.Text)));
                    }
                }
                Session["EncuestasSeleccionadas"] = lst;
                LlenaEncuestasSeleccionadas();
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
                List<Encuesta> lst = Session["EncuestasSeleccionadas"] == null ? new List<Encuesta>() : (List<Encuesta>)Session["EncuestasSeleccionadas"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptEncuestasSeleccionadas.Items[index].FindControl("lblId");

                        lst.Remove(lst.Single(s => s.Id == int.Parse(lblId.Text)));
                    }
                }
                Session["EncuestasSeleccionadas"] = lst;
                LlenaEncuestasSeleccionadas();
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