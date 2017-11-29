using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSistemaTipoArbolAcceso;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroServicioIncidente : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoArbolAccesoClient _servicioGrupoUsuario = new ServiceTipoArbolAccesoClient();
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
        public List<int> TipoArbolSeleccionados
        {
            get
            {
                return (from RepeaterItem item in rptTipoArbolSeleccionado.Items select int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
            set { }
        }
        private void LlenaTipoArbolTicket()
        {
            try
            {
                rptTipoArbol.DataSource = _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false).Where(w => w.Id != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion);
                rptTipoArbol.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaTipoArbolConsulta()
        {
            try
            {
                rptTipoArbol.DataSource = _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false).Where(w => w.Id == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion);
                rptTipoArbol.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaTipoArbolEncuesta()
        {
            try
            {
                rptTipoArbol.DataSource = _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false);
                rptTipoArbol.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool EsTicket
        {
            get { return Convert.ToBoolean(hfticket.Value); }
            set
            {
                if (value)
                    LlenaTipoArbolTicket();
                hfticket.Value = value.ToString();
            }
        }

        public bool EsConsulta
        {
            get { return Convert.ToBoolean(hfticket.Value); }
            set
            {
                if (value)
                    LlenaTipoArbolConsulta();
                hfticket.Value = value.ToString();
            }
        }

        public bool EsEncuesta
        {
            get { return Convert.ToBoolean(hfticket.Value); }
            set
            {
                if (value)
                    LlenaTipoArbolEncuesta();
                hfticket.Value = value.ToString();
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