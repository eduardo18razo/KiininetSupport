using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceOrganizacion;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleGeograficoTickets : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceUbicacionClient _servicioUbicacion = new ServiceUbicacionClient();
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

        public List<HelperFiltroGrafico> UbicacionesGraficar
        {
            set
            {
                rptUbicaciones.DataSource = value.OrderBy(o => o.Id);
                rptUbicaciones.DataBind();
            }
        }

        public List<HelperFiltroGrafico> OrganizacionesGraficar
        {
            set
            {
                rptOrganizaciones.DataSource = value.OrderBy(o => o.Id);
                rptOrganizaciones.DataBind();
            }
        }

        public List<HelperFiltroGrafico> TipoTicket
        {
            set
            {
                rptTipoTicket.DataSource = value;
                rptTipoTicket.DataBind();
            }
        }

        public List<HelperFiltroGrafico> TipificacionesGraficar
        {
            set
            {
                rptTipicaciones.DataSource = value;
                rptTipicaciones.DataBind();
            }
        }

        public List<HelperFiltroGrafico> EstatusStack
        {
            set
            {
                rptEstatusStack.DataSource = value;
                rptEstatusStack.DataBind();
            }
        }

        public Dictionary<bool, string> SlaGraficar
        {
            set
            {
                rptSla.DataSource = value;
                rptSla.DataBind();
            }
        }

        private void Limpiar()
        {
            try
            {
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
        
        protected void rbtUbicaciones_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlUbicacion.Visible = ((RadioButton)sender).Checked;
                pnlOrganizacion.Visible = false;
                pnlTipoticket.Visible = false;
                pnlTipificaciones.Visible = false;
                pnlEstatusTicket.Visible = false;
                pnlSla.Visible = false;
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
        protected void rbtnOrganizaciones_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlOrganizacion.Visible = ((RadioButton)sender).Checked;
                pnlUbicacion.Visible = false;
                pnlTipoticket.Visible = false;
                pnlTipificaciones.Visible = false;
                pnlEstatusTicket.Visible = false;
                pnlSla.Visible = false;
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
        protected void rbtnTipoTicket_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlTipoticket.Visible = ((RadioButton)sender).Checked;
                pnlOrganizacion.Visible = false;
                pnlUbicacion.Visible = false;
                pnlTipificaciones.Visible = false;
                pnlEstatusTicket.Visible = false;
                pnlSla.Visible = false;
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
        protected void rbtnTipificaciones_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlTipificaciones.Visible = ((RadioButton)sender).Checked;
                pnlOrganizacion.Visible = false;
                pnlUbicacion.Visible = false;
                pnlTipoticket.Visible = false;
                pnlEstatusTicket.Visible = false;
                pnlSla.Visible = false;
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
        protected void rbtnEstaus_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlEstatusTicket.Visible = ((RadioButton)sender).Checked;
                pnlOrganizacion.Visible = false;
                pnlUbicacion.Visible = false;
                pnlTipoticket.Visible = false;
                pnlTipificaciones.Visible = false;
                pnlSla.Visible = false;
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
        protected void rbtnSla_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlSla.Visible = ((RadioButton)sender).Checked;
                pnlOrganizacion.Visible = false;
                pnlUbicacion.Visible = false;
                pnlTipoticket.Visible = false;
                pnlTipificaciones.Visible = false;
                pnlEstatusTicket.Visible = false;
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
        
        protected void btnCerrar_OnClick(object sender, EventArgs e)
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