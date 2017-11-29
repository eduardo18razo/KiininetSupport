using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniNet.Entities.Helper;

namespace KiiniHelp.UserControls.Operacion
{
    public partial class UcRegistroCatalogo : UserControl, IControllerModal
    {
        private readonly ServiceCatalogosClient _servicioCatalogo = new ServiceCatalogosClient();
        private List<string> _lstError = new List<string>();
        private List<string> AlertaGeneral
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
        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set { hfEsAlta.Value = value.ToString(); }
        }
        public string Titulo
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public int IdCatalogo
        {
            get { return int.Parse(hfIdCatalogo.Value); }
            set { hfIdCatalogo.Value = value.ToString(); }
        }
        public int IdRegistro
        {
            get { return int.Parse(hfIdRegistro.Value); }
            set
            {
                hfIdRegistro.Value = value.ToString();
                CatalogoGenerico registro = _servicioCatalogo.ObtenerRegistrosSistemaCatalogo(IdCatalogo, false, true).SingleOrDefault(s => s.Id == value);
                if (registro != null)
                {
                    txtDescripcion.Text = registro.Descripcion;
                }
            }
        }

        private void Limpiar()
        {
            try
            {
                txtDescripcion.Text = string.Empty;
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
                lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
                AlertaGeneral = new List<string>();
                //TODO: Se elimina para bloque de boton al click
                //btnGuardarArea.OnClientClick = "this.disabled = document.getElementById('form1').checkValidity(); if(document.getElementById('form1').checkValidity()){ " + Page.ClientScript.GetPostBackEventReference(btnGuardarArea, null) + ";}";  
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtDescripcion.Text.Trim() == string.Empty)
                    throw new Exception("Descripcion es campo obligatorio");
                if (EsAlta)
                    _servicioCatalogo.AgregarRegistroSistema(IdCatalogo, txtDescripcion.Text);
                else
                    _servicioCatalogo.ActualizarRegistroSistema(IdCatalogo, txtDescripcion.Text, IdRegistro);
                //_servicioCatalogo.ac
                Limpiar();
                if (OnAceptarModal != null)
                    OnAceptarModal();
                if(!EsAlta)
                    btnTerminar_OnClick(btnTerminar, null);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnTerminar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Limpiar();
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
                AlertaGeneral = _lstError;
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
                AlertaGeneral = _lstError;
            }
        }

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Limpiar();
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
                AlertaGeneral = _lstError;
            }
        }

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        
    }
}