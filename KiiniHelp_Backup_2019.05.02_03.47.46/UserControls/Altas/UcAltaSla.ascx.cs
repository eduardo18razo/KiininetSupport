using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceSla;
using KiiniHelp.ServiceSubGrupoUsuario;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaSla : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private readonly ServiceSubGrupoUsuarioClient _servicioSubGrupo = new ServiceSubGrupoUsuarioClient();
        private readonly ServiceSlaClient _servicioSla = new ServiceSlaClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupos = new ServiceGrupoUsuarioClient();
        private List<string> _lstError = new List<string>();

        private List<string> Alerta
        {
            set
            {
                panelAlert.Visible = value.Any();
                if (!panelAlert.Visible) return;
                rptHeaderError.DataSource = value;
                rptHeaderError.DataBind();
            }
        }

        public int IdGrupo
        {
            get { return Convert.ToInt32(ddlGrupo.SelectedValue); }
            set
            {
                LlenaCombo();
                ddlGrupo.SelectedValue = value.ToString();
                ddlGrupo_OnSelectedIndexChanged(ddlGrupo, null);
            }
        }

        public int IdSla
        {
            get { return Convert.ToInt32(hfIdSla.Value); }
            set
            {
                Sla = _servicioSla.ObtenerSlaById(value);
                hfIdSla.Value = value.ToString();
            }
        }
        public Sla Sla
        {
            get
            {
                Sla sla = new Sla
                {
                    SlaDetalle = new List<SlaDetalle>(),
                    Detallado = chkEstimado.Checked,
                    Habilitado = true
                };
                decimal tDias = 0, tHoras = 0, tminutos = 0, tsegundos = 0;
                foreach (RepeaterItem item in rptSubRoles.Items)
                {
                    var lblIdSubRol = (Label)item.FindControl("lblIdSubRol");
                    var txtDias = (TextBox)item.FindControl("txtDias");
                    var txtHoras = (TextBox)item.FindControl("txtHoras");
                    var txtMinutos = (TextBox)item.FindControl("txtMinutos");
                    var txtSegundos = (TextBox)item.FindControl("txtSegundos");
                    SlaDetalle detalle = new SlaDetalle { IdSubRol = Convert.ToInt32(lblIdSubRol.Text.Trim()) };
                    if (txtDias != null)
                    {
                        detalle.Dias = Convert.ToDecimal(txtDias.Text.Trim());
                        tDias += detalle.Dias;
                    }
                    if (txtHoras != null)
                    {
                        detalle.Horas = Convert.ToDecimal(txtHoras.Text.Trim());
                        tHoras += detalle.Horas;
                    }
                    if (txtMinutos != null)
                    {
                        detalle.Minutos = Convert.ToDecimal(txtMinutos.Text.Trim());
                        tminutos += detalle.Minutos;
                    }
                    if (txtSegundos != null)
                    {
                        detalle.Segundos = Convert.ToDecimal(txtSegundos.Text.Trim());
                        tsegundos += detalle.Segundos;
                    }
                    detalle.TiempoProceso = (detalle.Dias / 24) + detalle.Horas + (detalle.Minutos / 60) + ((detalle.Minutos / 60) / 60);
                    sla.SlaDetalle.Add(detalle);
                }

                sla.Dias = tDias;
                sla.Horas = tHoras;
                sla.Minutos = tminutos;
                sla.Segundos = tsegundos;
                sla.TiempoHoraProceso = (tDias / 24) + tHoras + (tminutos / 60) + ((tsegundos / 60) / 60);
                return sla;
            }
            set
            {
                chkEstimado.Checked = value.Detallado;
                chkEstimado_OnCheckedChanged(chkEstimado, null);
                if (value.Detallado)
                {
                    foreach (SlaDetalle detalle in value.SlaDetalle)
                    {
                        foreach (RepeaterItem item in rptSubRoles.Items)
                        {
                            var lblIdSubRol = (Label)item.FindControl("lblIdSubRol");

                            if (detalle.IdSubRol == Convert.ToInt32(lblIdSubRol.Text))
                            {
                                var txtDias = (TextBox)item.FindControl("txtDias");
                                var txtHoras = (TextBox)item.FindControl("txtHoras");
                                var txtMinutos = (TextBox)item.FindControl("txtMinutos");
                                var txtSegundos = (TextBox)item.FindControl("txtSegundos");
                                if (txtDias.Text != null)
                                    txtDias.Text = detalle.Dias.ToString();
                                if (txtHoras.Text != null)
                                    txtHoras.Text = detalle.Horas.ToString();
                                if (txtMinutos.Text != null)
                                    txtMinutos.Text = detalle.Minutos.ToString();
                                if (txtSegundos.Text != null)
                                    txtSegundos.Text = detalle.Segundos.ToString();
                                break;
                            }
                        }

                    }

                    TextBox txtTDias = (TextBox)rptSubRoles.Controls[rptSubRoles.Controls.Count - 1].Controls[0].FindControl("txtTotalDias");
                    TextBox txtTHors = (TextBox)rptSubRoles.Controls[rptSubRoles.Controls.Count - 1].Controls[0].FindControl("txtTotalHoras");
                    TextBox txtTMins = (TextBox)rptSubRoles.Controls[rptSubRoles.Controls.Count - 1].Controls[0].FindControl("txtTotalMinutos");
                    TextBox txtTSegs = (TextBox)rptSubRoles.Controls[rptSubRoles.Controls.Count - 1].Controls[0].FindControl("txtTotalSegundos");
                    if (txtTDias != null)
                        txtTDias.Text = Sla.Dias.ToString();
                    if (txtTHors != null)
                        txtTHors.Text = Sla.Horas.ToString();
                    if (txtTMins != null)
                        txtTMins.Text = Sla.Minutos.ToString();
                    if (txtTSegs != null)
                        txtTSegs.Text = Sla.Segundos.ToString();
                }
            }
        }

        public bool FromModal
        {
            get { return Convert.ToBoolean(hfFromModal.Value); }
            set
            {
                hfFromModal.Value = value.ToString();
            }
        }

        public bool ValidarCaptura()
        {
            try
            {
                if (chkEstimado.Checked)
                    foreach (RepeaterItem item in rptSubRoles.Items)
                    {
                        var txtDias = (TextBox)item.FindControl("txtDias");
                        var txtHoras = (TextBox)item.FindControl("txtHoras");
                        var txtMinutos = (TextBox)item.FindControl("txtMinutos");
                        var txtSegundos = (TextBox)item.FindControl("txtSegundos");
                        if (txtDias != null)
                            if (txtDias.Text.Trim() == string.Empty)
                                throw new Exception("Debe especificar el tiempo para todos los sub roles");
                        if (txtHoras != null)
                            if (txtHoras.Text.Trim() == string.Empty)
                                throw new Exception("Debe especificar el tiempo para todos los sub roles");
                        if (txtMinutos != null)
                            if (txtMinutos.Text.Trim() == string.Empty)
                                throw new Exception("Debe especificar el tiempo para todos los sub roles");
                        if (txtSegundos != null)
                            if (txtSegundos.Text.Trim() == string.Empty)
                                throw new Exception("Debe especificar el tiempo para todos los sub roles");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        public void LimpiarCampos()
        {
            try
            {
                chkEstimado.Checked = false;
                txtTiempoTotal.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void LlenaCombo()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlGrupo, _servicioGrupos.ObtenerGruposUsuarioAll(null, null));
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

        protected void chkEstimado_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                divDetalle.Visible = chkEstimado.Checked;
                divSimple.Visible = !chkEstimado.Checked;
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

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidarCaptura();
                if (OnAceptarModal != null)
                    OnAceptarModal();
                //if (FromModal)
                //    _servicioSla.Guardar(Sla);
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
                LimpiarCampos();
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

        protected void ddlGrupo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                rptSubRoles.DataSource = _servicioSubGrupo.ObtenerSubGruposUsuarioByIdGrupo(IdGrupo);
                rptSubRoles.DataBind();
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