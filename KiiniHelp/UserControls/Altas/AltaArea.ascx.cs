using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using AjaxControlToolkit;
using KiiniHelp.ServiceArea;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class AltaArea : UserControl, IControllerModal
    {
        readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();
        private List<string> _lstError = new List<string>();
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private UsuariosMaster _mp;
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

        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set
            {
                hfEsAlta.Value = value.ToString();
                lblOperacion.Text = value ? "Alta de Categoría" : "Editar Categoría";
            }
        }

        public int IdArea
        {
            get { return Convert.ToInt32(hfIdArea.Value); }
            set
            {
                Area puesto = _servicioArea.ObtenerAreaById(value);
                txtDescripcionAreas.Text = puesto.Descripcion;
                hfIdArea.Value = value.ToString();
            }
        }

        private void LimpiarCampos()
        {
            try
            {
                hfFileName.Value = string.Empty;
                txtDescripcionAreas.Text = String.Empty;
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
                //lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
                //TODO: Se elimina para bloque de boton al click
                //btnGuardarArea.OnClientClick = "this.disabled = document.getElementById('form1').checkValidity(); if(document.getElementById('form1').checkValidity()){ " + Page.ClientScript.GetPostBackEventReference(btnGuardarArea, null) + ";}";  
                _mp = (UsuariosMaster)Page.Master;
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
                if (txtDescripcionAreas.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar una descripción");
                Area area = new Area();
                area.Descripcion = txtDescripcionAreas.Text.Trim();
                if (Session["ImagenArea"] != null)
                    if (Session["ImagenArea"].ToString() != string.Empty)
                        area.Imagen = BusinessFile.Imagenes.ImageToByteArray(Session["ImagenArea"].ToString());
                area.Habilitado = true;
                if (EsAlta)
                {
                    area.IdUsuarioAlta = ((Usuario)Session["UserData"]).Id;
                    _servicioArea.Guardar(area);
                    _mp.AlertaSucces(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Exito));
                }
                else
                {
                    area.IdUsuarioModifico = ((Usuario)Session["UserData"]).Id;
                    _servicioArea.Actualizar(int.Parse(hfIdArea.Value), area);
                    _mp.AlertaSucces(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Actualizacion));
                }
                LimpiarCampos();
                if (!EsAlta)
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

        protected void afDosnload_OnUploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                AsyncFileUpload uploadControl = (AsyncFileUpload)sender;
                //ParametrosGenerales generales = _servicioParametros.ObtenerParametrosGenerales();
                //Int64 sumaArchivos = Int64.Parse(Session["FileSize"].ToString());
                //sumaArchivos += int.Parse(e.FileSize);
                //if (!Directory.Exists(BusinessVariables.Directorios.RepositorioTemporalInformacionConsulta))
                //    Directory.CreateDirectory(BusinessVariables.Directorios.RepositorioTemporalInformacionConsulta);
                //if ((sumaArchivos / 1024) > int.Parse(generales.TamanoDeArchivo))
                //    throw new Exception(string.Format("El tamaño maximo de carga es de {0}MB", generales.TamanoDeArchivo));
                uploadControl.SaveAs(BusinessVariables.Directorios.RepositorioTemporal + e.FileName.Split('\\').Last());
                Session["ImagenArea"] = e.FileName.Split('\\').Last();
                //hfFileName.Value = e.FileName.Split('\\').Last();
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