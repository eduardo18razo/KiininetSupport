﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServicePuesto;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;
using System.Resources;
using System.IO;
using System.Reflection;

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

        //Assembly _assembly;   
        //Stream _imageStream;   
        //StreamReader _textStreamReader;

        //ResourceManager _rM = new ResourceManager("Notificaciones", Assembly.GetExecutingAssembly());

        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set
            {
                hfEsAlta.Value = value.ToString();
                lblOperacion.Text = value ? "Nuevo Puesto" : "Editar Puesto";
            }
        }
        public int IdTipoUsuario
        {
            get { return Convert.ToInt32(ddlTipoUsuario.SelectedValue); }
            set
            {
                ddlTipoUsuario.SelectedValue = value.ToString();
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

        //private string AlertaSucces ()
        //{
        //    //set
        //    //{
        //    //    if (value.Trim() != string.Empty)
        //    //    {
        //            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "SuccsessAlert('"+ value + "');", true);                    
        //            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "SuccsessAlert('Exito','" + value + "');", true);
        //    //    }
        //    //}
        //}

        private void LlenaCombos()
        {
            try
            {
                List<TipoUsuario> lstTipoUsuario = _servicioTipoUsuario.ObtenerTiposUsuarioResidentes(true);
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
                    mp.AlertaError(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.FaltaTipoUsuario));
                if (txtDescripcionPuesto.Text.Trim() == string.Empty)
                    mp.AlertaError(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.FaltaDescripcion));
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
                if (!EsAlta)
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