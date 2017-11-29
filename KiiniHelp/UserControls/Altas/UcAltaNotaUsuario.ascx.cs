using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceNota;
using KiiniHelp.ServiceSitemaTipoNota;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaNotaUsuario : UserControl, IControllerModal
    {

        private readonly ServiceTipoNotaClient _servicioTipoNota = new ServiceTipoNotaClient();
        private readonly ServiceNotaClient _servicioNota = new ServiceNotaClient();

        private List<string> _lstError = new List<string>();
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private List<string> Alerta
        {
            set
            {
                panelAlerta.Visible = value.Any();
                if (!panelAlerta.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }

        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set { hfEsAlta.Value = value.ToString(); }
        }

        public int IdArea
        {
            get { return Convert.ToInt32(hfIdNotaGeneralUsuario.Value); }
            set
            {
                //NotaGeneralUsuario nota = _servicioArea.ObtenerAreaById(value);
                //txtDescripcionAreas.Text = puesto.Descripcion;
                hfIdNotaGeneralUsuario.Value = value.ToString();
            }
        }

        private void LlenaCombos()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlTipoNota, _servicioTipoNota.ObtenerTipoNotas(true));
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
                txtNombreNotaGralUsuario.Text = String.Empty;
                txtEditor.Text = string.Empty;
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
                Alerta = new List<string>();
                //TODO: Se elimina para bloque de boton al click
                //btnGuardarNota.OnClientClick = "this.disabled = document.getElementById('form1').checkValidity(); if(document.getElementById('form1').checkValidity()){ " + Page.ClientScript.GetPostBackEventReference(btnGuardarNota, null) + ";}";  
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
                if (txtNombreNotaGralUsuario.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar un nombre");
                if (txtEditor.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar un contenido");
                NotaGeneral nota = new NotaGeneral();
                nota.IdTipoNota = int.Parse(ddlTipoNota.SelectedValue);
                nota.IdUsuario = ((Usuario)Session["UserData"]).Id;
                nota.Nombre = txtNombreNotaGralUsuario.Text.Trim();
                nota.Contenido = txtEditor.Text.Trim();

                //if (EsAlta)
                _servicioNota.CrearNotaGeneralUsuario(nota);
                //else
                //    _servicioArea.Actualizar(int.Parse(hfIdArea.Value), area);
                LimpiarCampos();
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
    }
}