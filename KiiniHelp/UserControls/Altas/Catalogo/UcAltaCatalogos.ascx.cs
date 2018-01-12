using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.Catalogo
{
    public partial class UcAltaCatalogos : UserControl, IControllerModal
    {
        private readonly ServiceCatalogosClient _servicioCatalogo = new ServiceCatalogosClient();
        private List<string> _lstError = new List<string>();

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
                rbtnManual.Enabled = value;
                rbtnArchivo.Enabled = value;
                if (value)
                {
                    LimpiarCampos();
                    lblOperacion.Text = "Agregar Catálogo";
                }
                else
                {
                    lblOperacion.Text = "Actualizar Catálogo";
                }
            }
        }

        public int IdCatalogo
        {
            get { return Convert.ToInt32(hfIdCatalogo.Value); }
            set
            {
                Catalogos puesto = _servicioCatalogo.ObtenerCatalogos(false).Single(s => s.Id == value);
                txtNombreCatalogo.Text = puesto.Descripcion;
                txtDescripcionCatalogo.Text = puesto.DescripcionLarga;
                hfIdCatalogo.Value = value.ToString();
                rbtnManual.Checked = !puesto.Archivo;
                rbtnArchivo.Checked = puesto.Archivo;
                divManual.Visible = !puesto.Archivo;
                divArchivo.Visible = puesto.Archivo;
                if (!puesto.Archivo)
                {
                    Session["registrosCatalogos"] = _servicioCatalogo.ObtenerRegistrosSistemaCatalogo(value, false, true);
                    LlenaRegistros();
                }
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

        private void LimpiarCampos()
        {
            try
            {
                txtNombreCatalogo.Text = String.Empty;
                txtDescripcionCatalogo.Text = string.Empty;
                Metodos.LimpiarRadioList(rbtnHojas);
                Session["registrosCatalogos"] = new List<CatalogoGenerico>();
                Session["ArchivoCarga"] = null;
                rbtnManual.Checked = false;
                rbtnArchivo.Checked = false;
                LlenaRegistros();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LlenaRegistros()
        {
            try
            {
                rptRegistros.DataSource = Session["registrosCatalogos"];
                rptRegistros.DataBind();
                if (!EsAlta)
                {
                    if (rptRegistros.Items.Count > 0)
                    {
                        foreach (RepeaterItem item in rptRegistros.Items)
                        {
                            ((TextBox)item.FindControl("txtDescripcionRegistro")).Enabled = false;
                            ((LinkButton)item.FindControl("btnBorrarRegistro")).Enabled = false;
                        }
                        ((TextBox)
                            rptRegistros.Controls[rptRegistros.Controls.Count - 1].Controls[0].FindControl(
                                "txtRegistroNew")).Enabled = false;
                        ((LinkButton)
                            rptRegistros.Controls[rptRegistros.Controls.Count - 1].Controls[0].FindControl(
                                "btnAgregarRegistro")).Enabled = false;
                    }
                }


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
                    Session["registrosCatalogos"] = Session["registrosCatalogos"] ?? new List<CatalogoGenerico>();
                    //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "disableUpload", "var elms = document.getElementById('ContentPlaceHolder1_ucConsultaCatalogos_ucAltaCatalogos_afuArchivo').getElementsByTagName(\"*\");" + "for (var i = 0; i < elms.length; i++) " + "{" + "elms[i].disabled  = " + true.ToString().ToLower() + "" + "};", true);
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

        protected void btnBorrarRegistro_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton remover = (LinkButton)sender;
                List<CatalogoGenerico> tmpList = Session["registrosCatalogos"] == null ? new List<CatalogoGenerico>() : (List<CatalogoGenerico>)Session["registrosCatalogos"];
                tmpList.RemoveAt(int.Parse(remover.CommandArgument));
                Session["registrosCatalogos"] = tmpList;
                LlenaRegistros();
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

        protected void btnAgregarRegistro_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton adder = (LinkButton)sender;
                var itemFooter = adder.NamingContainer;
                TextBox txtFooter = (TextBox)itemFooter.FindControl("txtRegistroNew");
                if (txtFooter != null)
                {
                    if (txtFooter.Text.Trim() == string.Empty)
                        throw new Exception("Ingrese una descripción.");
                    List<CatalogoGenerico> tmpList = Session["registrosCatalogos"] == null ? new List<CatalogoGenerico>() : (List<CatalogoGenerico>)Session["registrosCatalogos"];
                    tmpList.Add(new CatalogoGenerico { Descripcion = txtFooter.Text.Trim() });
                    Session["registrosCatalogos"] = tmpList;
                    LlenaRegistros();
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

        protected void btnLeerArchivo_OnClick(object sender, EventArgs e)
        {
            try
            {
                Console.Write("Entro a leer");
                hfFileName.Value = Session["ArchivoCarga"].ToString();
                if (hfFileName.Value.Trim() == string.Empty)
                    throw new Exception("Seleccione un archivo");
                Console.Write("Si tiene archivo");
                var path = BusinessVariables.Directorios.RepositorioTemporal;
                Console.Write(path);
                rbtnHojas.DataSource = BusinessFile.ExcelManager.ObtenerHojasExcel(path + afuArchivo.FileName);
                rbtnHojas.DataTextField = "TABLE_NAME";
                rbtnHojas.DataValueField = "TABLE_NAME";
                rbtnHojas.DataBind();
                Console.Write("termino");
            }
            catch (Exception ex)
            {
                Console.Write("Error");
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

                if (txtNombreCatalogo.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar un nombre.");
                if (!rbtnManual.Checked && !rbtnArchivo.Checked)
                    throw new Exception("Debe seleccionar tipo de carga.");
                Catalogos cat = new Catalogos
                {
                    Descripcion = txtNombreCatalogo.Text,
                    DescripcionLarga = txtDescripcionCatalogo.Text,
                    IdUsuarioAlta = ((Usuario)Session["UserData"]).Id
                };
                if (rbtnArchivo.Checked)
                {
                    if (Session["ArchivoCarga"].ToString() != String.Empty)
                    {
                        if (rbtnHojas.SelectedItem.Value.Trim() == string.Empty) throw new Exception("Seleccione una horja del archivo.");
                        if (EsAlta)
                        {
                            _servicioCatalogo.CrearCatalogoExcel(cat, true, hfFileName.Value, rbtnHojas.SelectedValue);
                        }
                        else
                        {
                            cat.Id = IdCatalogo;
                            cat.IdUsuarioModifico = ((Usuario)Session["UserData"]).Id;
                            _servicioCatalogo.ActualizarCatalogoExcel(cat, true, hfFileName.Value, rbtnHojas.SelectedValue);
                        }

                        BusinessFile.LimpiarRepositorioTemporal(new List<string> { hfFileName.Value });
                        LimpiarCampos();
                    }
                }
                else
                {

                    if (EsAlta)
                    {
                        _servicioCatalogo.CrearCatalogo(cat, true, ((List<CatalogoGenerico>)Session["registrosCatalogos"]));
                    }
                    else
                    {
                        cat.Id = IdCatalogo;
                        cat.IdUsuarioModifico = ((Usuario)Session["UserData"]).Id;
                        _servicioCatalogo.ActualizarCatalogo(cat, true, ((List<CatalogoGenerico>)Session["registrosCatalogos"]));
                    }
                }
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

        protected void rbtnTipoCatalogo_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {

                foreach (RepeaterItem item in rptRegistros.Items)
                {
                    ((TextBox)item.FindControl("txtDescripcionRegistro")).Enabled = rbtnManual.Checked;
                    ((LinkButton)item.FindControl("btnBorrarRegistro")).Enabled = rbtnManual.Checked;

                }

                foreach (Control control in rptRegistros.Controls[rptRegistros.Controls.Count - 1].Controls)
                {
                    ((TextBox)control.FindControl("txtRegistroNew")).Enabled = rbtnManual.Checked;
                    ((LinkButton)control.FindControl("btnAgregarRegistro")).Enabled = rbtnManual.Checked;
                }
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "disableUpload", "document.getElementById('" + afuArchivo.ClientID +"').disabled = "+ (!rbtnArchivo.Checked).ToString().ToLower() + ";", true);
                divManual.Visible = rbtnManual.Checked;
                divArchivo.Visible = rbtnArchivo.Checked;
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "disableUpload", "var elms = document.getElementById('ContentPlaceHolder1_ucConsultaCatalogos_ucAltaCatalogos_afuArchivo').getElementsByTagName(\"*\");" + "for (var i = 0; i < elms.length; i++) " + "{" + "elms[i].disabled  = " + (!rbtnArchivo.Checked).ToString().ToLower() + "" + "};", true);
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