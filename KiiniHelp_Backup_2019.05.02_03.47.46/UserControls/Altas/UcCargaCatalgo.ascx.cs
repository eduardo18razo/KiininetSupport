using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using AjaxControlToolkit;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcCargaCatalgo : UserControl, IControllerModal
    {
        private readonly ServiceCatalogosClient _servicioCatalogo = new ServiceCatalogosClient();
        private List<string> _lstError = new List<string>();

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
            set
            {
                hfEsAlta.Value = value.ToString();
                txtDescripcionCatalogo.Enabled = value;
            }
        }

        public int IdCatalogo
        {
            get { return int.Parse(hfIdCatalogo.Value); }
            set
            {
                hfIdCatalogo.Value = value.ToString();
                Catalogos catalogo = _servicioCatalogo.ObtenerCatalogo(value);
                if (catalogo != null)
                {
                    txtDescripcionCatalogo.Text = catalogo.Descripcion;
                }
            }
        }

        private void ValidaCaptura()
        {
            StringBuilder sb = new StringBuilder();

            if (txtDescripcionCatalogo.Text.Trim() == string.Empty)
                sb.AppendLine("<li>Descripción es un campo obligatorio.</li>");
            if (hfFileName.Value.Trim() == string.Empty)
                sb.AppendLine("<li>Seleccione un archivo.</li>");
            if (rbtnHojas.SelectedItem.Value.Trim() == string.Empty)
                sb.AppendLine("<li>Seleccione una horja del archivo.</li>");

            if (sb.ToString() != string.Empty)
            {
                sb.Append("</ul>");
                sb.Insert(0, "<ul>");
                sb.Insert(0, "<h3>Datos Generales</h3>");
                throw new Exception(sb.ToString());
            }
        }

        private void LimpiarPantalla()
        {
            try
            {
                txtDescripcionCatalogo.Text = string.Empty;
                Metodos.LimpiarRadioList(rbtnHojas);
                Session["ArchivoCarga"] = string.Empty;
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

        protected void afuArchivo_OnUploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                bool fileOk = false;
                var path = BusinessVariables.Directorios.RepositorioTemporal;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (afuArchivo.HasFile)
                {
                    string extension = Path.GetExtension(afuArchivo.FileName);
                    if (extension != null)
                    {
                        var fileExtension = extension.ToLower();
                        string[] allowedExtensions = { ".xls", ".xlsx" };
                        foreach (string t in allowedExtensions.Where(t => fileExtension == t))
                        {
                            fileOk = true;
                        }
                    }
                }

                if (fileOk)
                {
                    try
                    {
                        hfFileName.Value = afuArchivo.FileName;
                        afuArchivo.PostedFile.SaveAs(path + afuArchivo.FileName);
                        Session["ArchivoCarga"] = afuArchivo.FileName;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                    throw new Exception("Formato incorrecto");
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
        protected void btnLeer_OnClick(object sender, EventArgs e)
        {
            try
            {
                hfFileName.Value = Session["ArchivoCarga"].ToString();
                if (hfFileName.Value.Trim() == string.Empty)
                    throw new Exception("Seleccione un archivo");
                var path = BusinessVariables.Directorios.RepositorioTemporal;
                rbtnHojas.DataSource = BusinessFile.ExcelManager.ObtenerHojasExcel(path + afuArchivo.FileName);
                rbtnHojas.DataTextField = "TABLE_NAME";
                rbtnHojas.DataValueField = "TABLE_NAME";
                rbtnHojas.DataBind();
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
                ValidaCaptura();
                if (EsAlta)
                {
                    //_servicioCatalogo.CrearCatalogoExcel(txtDescripcionCatalogo.Text, true, hfFileName.Value, rbtnHojas.SelectedValue);
                }
                else
                {
                    //_servicioCatalogo.ActualizarCatalogoExcel(IdCatalogo, true, hfFileName.Value, rbtnHojas.SelectedValue);
                }
                BusinessFile.LimpiarRepositorioTemporal(new List<string> { hfFileName.Value });
                LimpiarPantalla();
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
                LimpiarPantalla();
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
                LimpiarPantalla();
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

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
    }
}