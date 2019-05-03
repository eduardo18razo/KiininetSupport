using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServicePuesto;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaPuesto : UserControl, IControllerModal
    {
        private readonly ServicePuestoClient _servicioPuesto = new ServicePuestoClient();
        private readonly ServiceTipoUsuarioClient _servicioTipoUsuario = new ServiceTipoUsuarioClient();
        private List<string> _lstError = new List<string>();
        UsuariosMaster mp;

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        
        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set
            {
                hfEsAlta.Value = value.ToString();
                lblOperacion.Text = value ? "Nuevo Puesto" : "Editar Puesto";
                ddlTipoUsuario.Enabled = value;
            }
        }
        public int IdTipoUsuario
        {
            get { return Convert.ToInt32(ddlTipoUsuario.SelectedValue); }
            set
            {
                ddlTipoUsuario.SelectedValue = value.ToString();
                if (value > BusinessVariables.ComboBoxCatalogo.IndexSeleccione && EsAlta)
                    ddlTipoUsuario.Enabled = false;
            }
        }
        public int IdPuesto
        {
            get { return Convert.ToInt32(hfIdPuesto.Value); }
            set
            {
                Puesto puesto = _servicioPuesto.ObtenerPuestoById(value);
                IdTipoUsuario = puesto.IdTipoUsuario;
                txtDescripcionPuesto.Text = puesto.Descripcion;
                hfIdPuesto.Value = value.ToString();
            }
        }

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


        private void LlenaCombos()
        {
            try
            {
                List<TipoUsuario> lstTipoUsuario = _servicioTipoUsuario.ObtenerTiposUsuarioResidentes(true, false);
                Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LimpiarCampos()
        {
            try
            {
                ddlTipoUsuario.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                txtDescripcionPuesto.Text = String.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                mp = (UsuariosMaster)Page.Master;
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    LlenaCombos();

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

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.FaltaTipoUsuario));
                if (txtDescripcionPuesto.Text.Trim() == string.Empty)
                    throw new Exception(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.FaltaDescripcion));
                Puesto puesto = new Puesto { IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue), Descripcion = txtDescripcionPuesto.Text.Trim(), Habilitado = true };
                if (EsAlta)
                {
                    _servicioPuesto.Guardar(puesto);
                    mp.AlertaSucces(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Exito));
                }
                else
                {
                    _servicioPuesto.Actualizar(int.Parse(hfIdPuesto.Value), puesto);
                    mp.AlertaSucces(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Actualizacion));
                }
                LimpiarCampos();
                if (!EsAlta || !ddlTipoUsuario.Enabled)
                    btnTerminar_OnClick(btnTerminar, null);

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
                LimpiarCampos();
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


        protected void btnTerminar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos();
                if (OnTerminarModal != null)
                    OnTerminarModal();
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