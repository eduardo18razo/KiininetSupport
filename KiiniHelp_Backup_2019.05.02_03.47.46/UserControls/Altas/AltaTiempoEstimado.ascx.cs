using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceNotificacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class AltaTiempoEstimado : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private readonly ServiceNotificacionClient _servicioNotificacion = new ServiceNotificacionClient();
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

        public bool ValidarCaptura()
        {
            try
            {

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
                txtDuenoDias.Text = string.Empty;
                txtDuenoHoras.Text = string.Empty;
                txtDuenoMinutos.Text = string.Empty;
                txtDuenoSegundos.Text = string.Empty;
                ddlDuenoVia.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                chkDueno.Checked = false;
                chkDueno_OnCheckedChanged(chkDueno, null);

                txtMttoDias.Text = string.Empty;
                txtMttoHoras.Text = string.Empty;
                txtMttoMinutos.Text = string.Empty;
                txtMttoSegundos.Text = string.Empty;
                ddlMttoVia.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                chkMtto.Checked = false;
                chkMtto_OnCheckedChanged(chkMtto, null);

                txtDesarrolloDias.Text = string.Empty;
                txtDesarrolloHoras.Text = string.Empty;
                txtDesarrolloMinutos.Text = string.Empty;
                txtDesarrolloSegundos.Text = string.Empty;
                ddlDesarrolloVia.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                chkDesarrollo.Checked = false;
                chkDesarrollo_OnCheckedChanged(chkDesarrollo, null);

                txtConsultaDias.Text = string.Empty;
                txtConsultaHoras.Text = string.Empty;
                txtConsultaMinutos.Text = string.Empty;
                txtConsultaSegundos.Text = string.Empty;
                ddlConsultaVia.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                chkConsulta.Checked = false;
                chkConsulta_OnCheckedChanged(chkConsulta, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SetTiempoEstimado(List<TiempoInformeArbol> tiemposInforme)
        {
            try
            {
                foreach (TiempoInformeArbol tiempo in tiemposInforme.Where(w => w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).ToList())
                {
                    //if(tiempo.GrupoUsuario.id)
                    //TiempoDueño = tiempo;
                }
                foreach (TiempoInformeArbol tiempo in tiemposInforme.Where(w => w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).ToList())
                {
                    TiempoMantenimiento = tiempo;
                }
                foreach (TiempoInformeArbol tiempo in tiemposInforme.Where(w => w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).ToList())
                {
                    TiempoDesarrollo = tiempo;
                }
                foreach (TiempoInformeArbol tiempo in tiemposInforme.Where(w => w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).ToList())
                {
                    TiempoConsulta = tiempo;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaCombos()
        {
            try
            {
                List<TipoNotificacion> lst = _servicioNotificacion.ObtenerTipos(true);
                Metodos.LlenaComboCatalogo(ddlDuenoVia, lst);
                Metodos.LlenaComboCatalogo(ddlMttoVia, lst);
                Metodos.LlenaComboCatalogo(ddlDesarrolloVia, lst);
                Metodos.LlenaComboCatalogo(ddlConsultaVia, lst);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool TieneTiempoDueño
        {
            get { return chkDueno.Checked; }
        }

        public TiempoInformeArbol TiempoDueño
        {
            get
            {
                TiempoInformeArbol result = new TiempoInformeArbol
                {
                    Dias = Convert.ToDecimal(txtDuenoDias.Text),
                    Horas = Convert.ToDecimal(txtDuenoHoras.Text),
                    Minutos = Convert.ToDecimal(txtDuenoMinutos.Text),
                    Segundos = Convert.ToDecimal(txtDuenoSegundos.Text),
                    IdTipoNotificacion = Convert.ToInt32(ddlDuenoVia.SelectedValue) == 0 ? 1 : Convert.ToInt32(ddlDuenoVia.SelectedValue)
                };
                result.TiempoNotificacion += (((Convert.ToDecimal(txtDuenoSegundos.Text) / 60) / 60) / 8) +
                (Convert.ToDecimal(txtDuenoMinutos.Text) / 60) / 8 +
                Convert.ToDecimal(txtDuenoHoras.Text) / 8 +
                Convert.ToDecimal(txtDuenoDias.Text);
                return result;
            }
            set
            {
                txtDuenoDias.Text = value.Dias.ToString();
                txtDuenoHoras.Text = value.Horas.ToString();
                txtDuenoMinutos.Text = value.Minutos.ToString();
                txtDuenoSegundos.Text = value.Segundos.ToString();
                ddlDuenoVia.SelectedValue = value.IdTipoNotificacion.ToString();
                chkDueno.Checked = true;
                chkDueno_OnCheckedChanged(chkDueno, null);
            }
        }
        public bool TieneTiempoMantenimiento
        {
            get { return chkDueno.Checked; }
        }
        public TiempoInformeArbol TiempoMantenimiento
        {
            get
            {
                TiempoInformeArbol result = new TiempoInformeArbol
                {
                    Dias = Convert.ToDecimal(txtMttoDias.Text),
                    Horas = Convert.ToDecimal(txtMttoHoras.Text),
                    Minutos = Convert.ToDecimal(txtMttoMinutos.Text),
                    Segundos = Convert.ToDecimal(txtMttoSegundos.Text),
                    IdTipoNotificacion = Convert.ToInt32(ddlMttoVia.SelectedValue)
                };
                result.TiempoNotificacion += (((Convert.ToDecimal(txtMttoSegundos.Text) / 60) / 60) / 8) +
                (Convert.ToDecimal(txtMttoMinutos.Text) / 60) / 8 +
                Convert.ToDecimal(txtMttoHoras.Text) / 8 +
                Convert.ToDecimal(txtMttoDias.Text);
                return result;
            }
            set
            {
                txtMttoDias.Text = value.Dias.ToString();
                txtMttoHoras.Text = value.Horas.ToString();
                txtMttoMinutos.Text = value.Minutos.ToString();
                txtMttoSegundos.Text = value.Segundos.ToString();
                ddlMttoVia.SelectedValue = value.IdTipoNotificacion.ToString();
                chkMtto.Checked = true;
                chkMtto_OnCheckedChanged(chkMtto, null);
            }
        }

        public bool TieneTiempoDesarrollo
        {
            get { return chkDueno.Checked; }
        }
        public TiempoInformeArbol TiempoDesarrollo
        {
            get
            {
                TiempoInformeArbol result = new TiempoInformeArbol
                {
                    Dias = Convert.ToDecimal(txtDesarrolloDias.Text),
                    Horas = Convert.ToDecimal(txtDesarrolloHoras.Text),
                    Minutos = Convert.ToDecimal(txtDesarrolloMinutos.Text),
                    Segundos = Convert.ToDecimal(txtDesarrolloSegundos.Text),
                    IdTipoNotificacion = Convert.ToInt32(ddlDesarrolloVia.SelectedValue)
                };
                result.TiempoNotificacion += (((Convert.ToDecimal(txtDesarrolloSegundos.Text) / 60) / 60) / 8) +
                (Convert.ToDecimal(txtDesarrolloMinutos.Text) / 60) / 8 +
                Convert.ToDecimal(txtDesarrolloHoras.Text) / 8 +
                Convert.ToDecimal(txtDesarrolloDias.Text);
                return result;
            }

            set
            {
                txtDesarrolloDias.Text = value.Dias.ToString();
                txtDesarrolloHoras.Text = value.Horas.ToString();
                txtDesarrolloMinutos.Text = value.Minutos.ToString();
                txtDesarrolloSegundos.Text = value.Segundos.ToString();
                ddlDesarrolloVia.SelectedValue = value.IdTipoNotificacion.ToString();
                chkDesarrollo.Checked = true;
                chkDesarrollo_OnCheckedChanged(chkDesarrollo, null);
            }
        }
        public bool TieneTiempoConsulta
        {
            get { return chkDueno.Checked; }
        }
        public TiempoInformeArbol TiempoConsulta
        {
            get
            {
                TiempoInformeArbol result = new TiempoInformeArbol
                {
                    Dias = Convert.ToDecimal(txtConsultaDias.Text),
                    Horas = Convert.ToDecimal(txtConsultaHoras.Text),
                    Minutos = Convert.ToDecimal(txtConsultaMinutos.Text),
                    Segundos = Convert.ToDecimal(txtConsultaSegundos.Text),
                    IdTipoNotificacion = Convert.ToInt32(ddlConsultaVia.SelectedValue)
                };
                result.TiempoNotificacion += (((Convert.ToDecimal(txtConsultaSegundos.Text) / 60) / 60) / 8) +
                (Convert.ToDecimal(txtConsultaMinutos.Text) / 60) / 8 +
                Convert.ToDecimal(txtConsultaHoras.Text) / 8 +
                Convert.ToDecimal(txtConsultaDias.Text);
                return result;
            }
            set
            {
                txtConsultaDias.Text = value.Dias.ToString();
                txtConsultaHoras.Text = value.Horas.ToString();
                txtConsultaMinutos.Text = value.Minutos.ToString();
                txtConsultaSegundos.Text = value.Segundos.ToString();
                ddlConsultaVia.SelectedValue = value.IdTipoNotificacion.ToString();
                chkConsulta.Checked = true;
                chkConsulta_OnCheckedChanged(chkConsulta, null);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                { LlenaCombos(); }
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

        protected void chkDueno_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtDuenoDias.Enabled = chkDueno.Checked;
                txtDuenoHoras.Enabled = chkDueno.Checked;
                txtDuenoMinutos.Enabled = chkDueno.Checked;
                txtDuenoSegundos.Enabled = chkDueno.Checked;
                ddlDuenoVia.Enabled = chkDueno.Checked;
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

        protected void chkMtto_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtMttoDias.Enabled = chkMtto.Checked;
                txtMttoHoras.Enabled = chkMtto.Checked;
                txtMttoMinutos.Enabled = chkMtto.Checked;
                txtMttoSegundos.Enabled = chkMtto.Checked;
                ddlMttoVia.Enabled = chkMtto.Checked;
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

        protected void chkDesarrollo_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtDesarrolloDias.Enabled = chkDesarrollo.Checked;
                txtDesarrolloHoras.Enabled = chkDesarrollo.Checked;
                txtDesarrolloMinutos.Enabled = chkDesarrollo.Checked;
                txtDesarrolloSegundos.Enabled = chkDesarrollo.Checked;
                ddlDesarrolloVia.Enabled = chkDesarrollo.Checked;
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

        protected void chkConsulta_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtConsultaDias.Enabled = chkConsulta.Checked;
                txtConsultaHoras.Enabled = chkConsulta.Checked;
                txtConsultaMinutos.Enabled = chkConsulta.Checked;
                txtConsultaSegundos.Enabled = chkConsulta.Checked;
                ddlConsultaVia.Enabled = chkConsulta.Checked;
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