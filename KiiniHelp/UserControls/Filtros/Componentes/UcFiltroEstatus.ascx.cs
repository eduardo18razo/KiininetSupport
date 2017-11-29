using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSistemaEstatus;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroEstatus : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceEstatusClient _servicioEstatus = new ServiceEstatusClient();
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

        private void LlenaEstatus()
        {
            try
            {
                rptEstatus.DataSource = _servicioEstatus.ObtenerEstatusTicket(false);
                rptEstatus.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaEstatuslSeleccionado()
        {
            try
            {
                //if (Session["EstatusSeleccionado"] == null) return;
                rptEstatusSeleccionado.DataSource = Session["EstatusSeleccionado"];
                rptEstatusSeleccionado.DataBind();
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
                Session["EstatusSeleccionado"] = null;
                LlenaEstatuslSeleccionado();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<int> EstatusSeleccionados
        {
            get
            {
                return (from RepeaterItem item in rptEstatusSeleccionado.Items select int.Parse(((Label) item.FindControl("lblId")).Text)).ToList();
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
                    Session["EstatusSeleccionado"] = null;
                    LlenaEstatus();
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
                List<EstatusTicket> lst = Session["EstatusSeleccionado"] == null ? new List<EstatusTicket>() : (List<EstatusTicket>)Session["EstatusSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptEstatus.Items[index].FindControl("lblId");
                        Label lblDescripcion = (Label)rptEstatus.Items[index].FindControl("lblDescripcion");

                        if (!lst.Any(a=>a.Id == int.Parse(lblId.Text)))
                            lst.Add(new EstatusTicket
                            {
                                Id = Convert.ToInt32(lblId.Text),
                                Descripcion = lblDescripcion.Text
                            });
                    }
                }
                Session["EstatusSeleccionado"] = lst;
                LlenaEstatuslSeleccionado();
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
                List<EstatusTicket> lst = Session["EstatusSeleccionado"] == null ? new List<EstatusTicket>() : (List<EstatusTicket>)Session["EstatusSeleccionado"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblId = (Label)rptEstatusSeleccionado.Items[index].FindControl("lblId");

                        lst.Remove(lst.Single(s => s.Id == int.Parse(lblId.Text)));
                    }
                }
                Session["EstatusSeleccionado"] = lst;
                LlenaEstatuslSeleccionado();
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