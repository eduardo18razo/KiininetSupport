using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceOrganizacion;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.UserControls.Filtros.Graficos
{
    public partial class UcFiltrosParametrosGraficoTicket : UserControl, IControllerModal
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
            get
            {
                List<HelperFiltroGrafico> result = new List<HelperFiltroGrafico>();
                foreach (RepeaterItem item in rptUbicaciones.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkUbicacion");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            result.Add(new HelperFiltroGrafico
                            {
                                Id = Convert.ToInt32(((Label)item.FindControl("lblId")).Text)
                            });
                        }
                    }
                }
                return result;
            }
            set
            {
                rptUbicaciones.DataSource = value.OrderBy(o => o.Id);
                rptUbicaciones.DataBind();
            }
        }

        public List<HelperFiltroGrafico> OrganizacionesGraficar
        {
            get
            {
                List<HelperFiltroGrafico> result = new List<HelperFiltroGrafico>();
                foreach (RepeaterItem item in rptOrganizaciones.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkOrganizacion");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            result.Add(new HelperFiltroGrafico
                            {
                                Id = Convert.ToInt32(((Label)item.FindControl("lblId")).Text)
                            });
                        }
                    }
                }
                return result;
            }
            set
            {
                rptOrganizaciones.DataSource = value.OrderBy(o => o.Id);
                rptOrganizaciones.DataBind();
            }
        }

        public List<HelperFiltroGrafico> TipoTicket
        {
            get
            {
                List<HelperFiltroGrafico> result = new List<HelperFiltroGrafico>();
                foreach (RepeaterItem item in rptTipoTicket.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkTipoTicket");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            result.Add(new HelperFiltroGrafico
                            {
                                Id = Convert.ToInt32(((Label)item.FindControl("lblId")).Text)
                            });
                        }
                    }
                }
                return result;
            }
            set
            {
                rptTipoTicket.DataSource = value;
                rptTipoTicket.DataBind();
            }
        }

        public List<HelperFiltroGrafico> TipificacionesGraficar
        {
            get
            {
                List<HelperFiltroGrafico> result = new List<HelperFiltroGrafico>();
                foreach (RepeaterItem item in rptTipicaciones.Items)
                {
                    CheckBox chk = (CheckBox) item.FindControl("chkTipificacion");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            result.Add(new HelperFiltroGrafico
                            {
                                Id = Convert.ToInt32(((Label)item.FindControl("lblId")).Text)
                            });
                        }
                    }
                }
                return result;
            }
            set
            {
                rptTipicaciones.DataSource = value;
                rptTipicaciones.DataBind();
            }
        }

        public List<HelperFiltroGrafico> EstatusStack
        {
            get
            {
                List<HelperFiltroGrafico> result = new List<HelperFiltroGrafico>();
                foreach (RepeaterItem item in rptEstatusStack.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkEstatusStack");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            result.Add(new HelperFiltroGrafico
                            {
                                Id = Convert.ToInt32(((Label)item.FindControl("lblId")).Text)
                            });
                        }
                    }
                }
                return result;
            }
            set
            {
                rptEstatusStack.DataSource = value;
                rptEstatusStack.DataBind();
            }
        }

        public Dictionary<bool, string> SlaGraficar
        {
            get
            {
                Dictionary<bool, string> result = new Dictionary<bool, string>();
                foreach (RepeaterItem item in rptSla.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSla");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            result.Add(Convert.ToBoolean(((Label)item.FindControl("lblId")).Text), chk.Text);
                        }
                    }
                }
                return result;
            }

            set
            {
                rptSla.DataSource = value;
                rptSla.DataBind();
            }
        }

        public List<HelperFiltroGrafico> CanalGraficar
        {
            get
            {
                List<HelperFiltroGrafico> result = new List<HelperFiltroGrafico>();
                foreach (RepeaterItem item in rptCanal.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkCanal");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            result.Add(new HelperFiltroGrafico
                            {
                                Id = Convert.ToInt32(((Label)item.FindControl("lblId")).Text)
                            });
                        }
                    }
                }
                return result;
            }
            set
            {
                rptCanal.DataSource = value;
                rptCanal.DataBind();
            }
        }

        public string TipoGrafico
        {
            get
            {
                string result = string.Empty;
                if (rbtnGeografico.Checked)
                    result = rbtnGeografico.Text;
                else if (rbtnPareto.Checked)
                    result = rbtnPareto.Text;
                else if (rbtnTendenciaStack.Checked)
                    result = rbtnTendenciaStack.Text;
                else if (rbtnTendenciaBarraCompetitiva.Checked)
                    result = rbtnTendenciaBarraCompetitiva.Text;

                return result;
            }
        }

        public string Stack
        {
            get
            {
                string result = string.Empty;
                if (rbtnUbicaciones.Checked)
                    result = rbtnUbicaciones.Text;
                else if (rbtnOrganizaciones.Checked)
                    result = rbtnOrganizaciones.Text;
                else if (rbtnTipoTicket.Checked)
                    result = rbtnTipoTicket.Text;
                else if (rbtnTipificaciones.Checked)
                    result = rbtnTipificaciones.Text;
                else if (rbtnEstaus.Checked)
                    result = rbtnEstaus.Text;
                else if (rbtnSla.Checked)
                    result = rbtnSla.Text;
                else if (rbtnCanal.Checked)
                    result = rbtnCanal.Text;
                return result;
            }

        }

        public List<int> FiltroStack
        {
            get
            {
                List<int> result = new List<int>();
                if (rbtnUbicaciones.Checked)
                {
                    foreach (RepeaterItem item in rptUbicaciones.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkUbicacion");
                        if (chk.Checked)
                        {
                            Label lblId = (Label)item.FindControl("lblId");
                            result.Add(Convert.ToInt32(lblId.Text));
                        }
                    }
                }
                else if (rbtnOrganizaciones.Checked)
                {
                    foreach (RepeaterItem item in rptOrganizaciones.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkOrganizacion");
                        if (chk.Checked)
                        {
                            Label lblId = (Label)item.FindControl("lblId");
                            result.Add(Convert.ToInt32(lblId.Text));
                        }
                    }
                }
                else if (rbtnTipoTicket.Checked)
                {
                    foreach (RepeaterItem item in rptTipoTicket.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkTipoTicket");
                        if (chk.Checked)
                        {
                            Label lblId = (Label)item.FindControl("lblId");
                            result.Add(Convert.ToInt32(lblId.Text));
                        }
                    }
                }
                else if (rbtnTipificaciones.Checked)
                {
                    foreach (RepeaterItem item in rptTipicaciones.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkTipificacion");
                        if (chk.Checked)
                        {
                            Label lblId = (Label)item.FindControl("lblId");
                            result.Add(Convert.ToInt32(lblId.Text));
                        }
                    }
                }
                else if (rbtnEstaus.Checked)
                {
                    foreach (RepeaterItem item in rptEstatusStack.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkEstatusStack");
                        if (chk.Checked)
                        {
                            Label lblId = (Label)item.FindControl("lblId");
                            result.Add(Convert.ToInt32(lblId.Text));
                        }
                    }
                }
                else if (rbtnSla.Checked)
                {
                    foreach (RepeaterItem item in rptSla.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkSla");
                        if (chk.Checked)
                        {
                            Label lblId = (Label)item.FindControl("lblId");
                            result.Add(Convert.ToBoolean(lblId.Text) ? 1 : 0);
                        }
                    }
                }

                return result;
            }
        }

        private void Limpiar()
        {
            try
            {
                rbtnGeografico.Checked = false;
                rbtnPareto.Checked = false;
                rbtnTendenciaStack.Checked = false;
                rbtnTendenciaBarraCompetitiva.Checked = false;
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

        #region TipoGrafico
        public bool Geografico
        {
            get { return rbtnGeografico.Checked; }
        }
        public bool Pareto
        {
            get { return rbtnPareto.Checked; }
        }
        public bool TendenciaStack
        {
            get { return rbtnTendenciaStack.Checked; }
        }
        public bool TendenciaBarraCompetitiva
        {
            get { return rbtnTendenciaBarraCompetitiva.Checked; }
        }

        #endregion TipoGrafico

        protected void chkUbicacion_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Ubicacion ubicacion = _servicioUbicacion.ObtenerUbicacionById(Convert.ToInt32((((Label)((CheckBox)sender).NamingContainer.FindControl("lblId")).Text)));
                if (ubicacion != null)
                {
                    foreach (int idUbicacion in _servicioUbicacion.ObtenerUbicacionesByIdUbicacion(Convert.ToInt32((((Label)((CheckBox)sender).NamingContainer.FindControl("lblId")).Text))))
                    {
                        foreach (RepeaterItem item in rptUbicaciones.Items)
                        {
                            if (((Label)item.FindControl("lblId")).Text == idUbicacion.ToString())
                            {
                                ((CheckBox)item.FindControl("chkUbicacion")).Checked = ((CheckBox)sender).Checked;
                                ((CheckBox)item.FindControl("chkUbicacion")).Enabled = !((CheckBox)sender).Checked;
                                break;
                            }
                        }
                    }
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
                pnlCanal.Visible = false;
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
                pnlCanal.Visible = false;
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
                pnlCanal.Visible = false;
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
                pnlCanal.Visible = false;
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
                pnlCanal.Visible = false;
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
                pnlCanal.Visible = false;
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
        protected void rbtnCanal_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlCanal.Visible = ((RadioButton)sender).Checked;
                pnlOrganizacion.Visible = false;
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
        protected void chkOrganizacion_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Organizacion organizacion = _servicioOrganizacion.ObtenerOrganizacionById(Convert.ToInt32((((Label)((CheckBox)sender).NamingContainer.FindControl("lblId")).Text)));
                if (organizacion != null)
                {
                    foreach (int idOrganizacion in _servicioOrganizacion.ObtenerOrganizacionesByIdOrganizacion(Convert.ToInt32((((Label)((CheckBox)sender).NamingContainer.FindControl("lblId")).Text))))
                    {
                        foreach (RepeaterItem item in rptOrganizaciones.Items)
                        {
                            if (((Label)item.FindControl("lblId")).Text == idOrganizacion.ToString())
                            {
                                ((CheckBox)item.FindControl("chkOrganizacion")).Checked = ((CheckBox)sender).Checked;
                                ((CheckBox)item.FindControl("chkOrganizacion")).Enabled = !((CheckBox)sender).Checked;
                                break;
                            }
                        }
                    }
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
        protected void btnGenerar_OnClick(object sender, EventArgs e)
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