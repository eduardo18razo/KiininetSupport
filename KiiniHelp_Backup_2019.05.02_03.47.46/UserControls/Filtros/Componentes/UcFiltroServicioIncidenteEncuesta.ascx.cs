using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSistemaTipoArbolAcceso;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroServicioIncidenteEncuesta : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoArbolAccesoClient _serviceTipoArbolAcceso = new ServiceTipoArbolAccesoClient();
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
        public List<int> TipoArbolSeleccionados
        {
            get
            {
                List<int> result = new List<int>();
                foreach (RepeaterItem item in rptTipoArbol.Items)
                {
                    result.Add(int.Parse(((Label) item.FindControl("lblId")).Text));
                }
                return result;
            }
            set { }
        }

        public List<int> Grupos
        {
            set
            {
                LlenaTipoArbolTicket(value);
            }
        }

        public void LlenaTipoArbolTicket(List<int> grupos)
        {
            try
            {
                rptTipoArbol.DataSource = _serviceTipoArbolAcceso.ObtenerTiposArbolAccesoByGrupos(grupos, false);
                rptTipoArbol.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaTipoArbolSeleccionado()
        {
            try
            {
                rptTipoArbolSeleccionado.DataSource = Session["TipoArbolSeleccionado"];
                rptTipoArbolSeleccionado.DataBind();
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
                Session["TipoArbolSeleccionado"] = null;
                LlenaTipoArbolSeleccionado();
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
                    Session["TipoArbolSeleccionado"] = null;
                    LlenaTipoArbolTicket(new List<int>());
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
                List<TipoArbolAcceso> lst = Session["TipoArbolSeleccionado"] == null ? new List<TipoArbolAcceso>() : (List<TipoArbolAcceso>)Session["TipoArbolSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupo = (Label)rptTipoArbol.Items[index].FindControl("lblId");
                        Label lblDescripcion = (Label)rptTipoArbol.Items[index].FindControl("lblDescripcion");

                        if (lst.Count <= 0)
                            lst.Add(new TipoArbolAcceso
                            {
                                Id = Convert.ToInt32(lblIdGrupo.Text),
                                Descripcion = lblDescripcion.Text
                            });
                    }
                }
                Session["TipoArbolSeleccionado"] = lst;
                LlenaTipoArbolSeleccionado();
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
                List<TipoArbolAcceso> lst = Session["TipoArbolSeleccionado"] == null ? new List<TipoArbolAcceso>() : (List<TipoArbolAcceso>)Session["TipoArbolSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupo = (Label)rptTipoArbolSeleccionado.Items[index].FindControl("lblId");

                        lst.Remove(lst.Single(s => s.Id == int.Parse(lblIdGrupo.Text)));
                    }
                }
                Session["TipoArbolSeleccionado"] = lst;
                LlenaTipoArbolSeleccionado();
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