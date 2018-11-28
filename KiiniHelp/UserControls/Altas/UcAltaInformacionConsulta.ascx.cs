using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniHelp.ServiceParametrosSistema;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaInformacionConsulta : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
        private readonly ServiceInformacionConsultaClient _servicioInformacionConsulta = new ServiceInformacionConsultaClient();

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

        private void LlenaCombos()
        {
            try
            {
                ParametrosGenerales generales = _servicioParametros.ObtenerParametrosGenerales();
                List<int> uploads = new List<int>();
                for (int i = 1; i <= int.Parse(generales.NumeroArchivo); i++)
                {
                    uploads.Add(i);
                }
                //rptDonloads.DataSource = uploads;
                //rptDonloads.DataBind();
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

        public void ValidaCaptura(BusinessVariables.EnumTiposInformacionConsulta tipoInformacion)
        {
            try
            {
                if (txtDescripcion.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar una descripción");
                switch (tipoInformacion)
                {
                    case BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido:
                        if (txtEditor.Text.Trim() == string.Empty)
                            throw new Exception("Debe especificar un contenido");
                        break;
                    //case BusinessVariables.EnumTiposInformacionConsulta.DocumentoOffice:
                    //    //if (!fuFile.HasFile)
                    //    //    throw new Exception("Debe especificar un documento");
                    //    break;
                    //case BusinessVariables.EnumTiposInformacionConsulta.DireccionWeb:
                    //    if (txtDescripcionUrl.Text.Trim() == string.Empty)
                    //        throw new Exception("Debe especificar una url de pagina");
                    //    break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set { hfEsAlta.Value = value.ToString(); }
        }
        public int IdTipoInformacion
        {

            get { return (int)BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido; }
        }
        public int IdInformacionConsulta
        {
            get { return Convert.ToInt32(hfIdInformacionConsulta.Value); }
            set
            {

                InformacionConsulta info = _servicioInformacionConsulta.ObtenerInformacionConsultaById(value);
                if (info == null) throw new Exception("Error al obtener informacion");
                txtDescripcion.Text = info.Descripcion;
                txtEditor.Text = info.InformacionConsultaDatos.First().Datos;
                txtBusqueda.Text = info.InformacionConsultaDatos.First().Busqueda;
                txtTags.Text = info.InformacionConsultaDatos.First().Tags;
                List<HelperFiles> lstArchivos = new List<HelperFiles>();
                foreach (InformacionConsultaDocumentos docto in info.InformacionConsultaDocumentos)
                {
                    if (!File.Exists(BusinessVariables.Directorios.RepositorioInformacionConsulta + docto.Archivo))
                    {
                        AlertaGeneral = new List<string> { string.Format("El archivo {0} no esta disponible.", docto.Archivo) };
                        continue;
                    }
                    HelperFiles hf = new HelperFiles
                    {
                        NombreArchivo = docto.Archivo,
                        Tamaño = BusinessFile.ConvertirTamaño(new FileInfo(BusinessVariables.Directorios.RepositorioInformacionConsulta + docto.Archivo).Length.ToString())
                    };
                    BusinessFile.CopiarArchivoDescarga(BusinessVariables.Directorios.RepositorioInformacionConsulta, docto.Archivo, BusinessVariables.Directorios.RepositorioTemporalInformacionConsulta);
                    lstArchivos.Add(hf);
                }
                Session["selectedFiles"] = lstArchivos;
                LlenaArchivosCargados();

                hfIdInformacionConsulta.Value = value.ToString();
            }
        }
        public double TamañoArchivo
        {
            get
            {
                return double.Parse(hfMaxSizeAllow.Value);
            }
            set { hfMaxSizeAllow.Value = value.ToString(); }
        }

        public string ArchivosPermitidos
        {
            get
            {
                return hfFileTypes.Value;
            }
            set
            {
                hfFileTypes.Value = value;
            }
        }

        private void LlenaArchivosCargados()
        {
            try
            {
                rptFiles.DataSource = Session["selectedFiles"];
                rptFiles.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void RenombraArchivos(List<string> lstArchivos, int idInformacion)
        {
            try
            {
                BusinessFile.RenombrarArchivosConsulta(lstArchivos, idInformacion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void LimpiarCampos()
        {
            try
            {
                txtDescripcion.Text = string.Empty;
                txtEditor.Text = string.Empty;
                txtBusqueda.Text = string.Empty;
                txtTags.Text = string.Empty;
                Session["FileSize"] = 0;
                Session["selectedFiles"] = new List<HelperFiles>();
                LlenaArchivosCargados();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool ValidaCaptura()
        {
            try
            {
                if (txtDescripcion.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese un Título");
                if (txtEditor.Text.Trim() == string.Empty)
                    throw new Exception("Debe ingresar información para mostrar.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        private InformacionConsulta ObtenerInformacionCapturada()
        {
            InformacionConsulta result;
            try
            {
                result = new InformacionConsulta
                {
                    Descripcion = txtDescripcion.Text.Trim(),
                    Habilitado = true,
                    IdTipoInfConsulta = IdTipoInformacion,
                    IdUsuarioAlta = ((Usuario)Session["UserData"]).Id,
                    InformacionConsultaDatos = new List<InformacionConsultaDatos>(),
                    InformacionConsultaDocumentos = new List<InformacionConsultaDocumentos>()
                };

                InformacionConsultaDatos datos = new InformacionConsultaDatos
                {
                    Datos = txtEditor.Text,
                    Busqueda = txtBusqueda.Text.Trim().ToUpper(),
                    Tags = txtTags.Text.Trim().ToUpper(),
                    Habilitado = true
                };
                result.InformacionConsultaDatos.Add(datos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                if (!IsPostBack)
                {
                    ParametrosGenerales parametros = _servicioParametros.ObtenerParametrosGenerales();
                    if (parametros != null)
                    {
                        foreach (ArchivosPermitidos alowedFile in _servicioParametros.ObtenerArchivosPermitidos())
                        {
                            ArchivosPermitidos += string.Format("{0}|", alowedFile.Extensiones);
                        }
                        TamañoArchivo = double.Parse(parametros.TamanoDeArchivo);
                    }
                    LlenaCombos();
                    Session["FileSize"] = 0;
                    Session["selectedFiles"] = new List<HelperFiles>();
                    if (Request.QueryString["IdInformacionConsulta"] != null)
                    {
                        EsAlta = false;
                        IdInformacionConsulta = int.Parse(Request.QueryString["IdInformacionConsulta"]);
                    }
                }
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Scripttagsbusqueda", "$('#txtBusqueda').tagsInput({ width: 'auto', defaultText: 'Agregar', delimiter: '|' });", true);
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Scripttags", "$('#txtTags').tagsInput({ width: 'auto', defaultText: 'Agregar', delimiter: '|' });", true);
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

        protected void afuArchivo_OnUploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                List<HelperFiles> files = Session["selectedFiles"] == null ? new List<HelperFiles>() : (List<HelperFiles>)Session["selectedFiles"];
                if (!files.Any(a => a.NombreArchivo.Contains(afuArchivo.FileName)))
                {

                    Int64 sumaArchivos = Int64.Parse(Session["FileSize"].ToString());
                    sumaArchivos += int.Parse(e.FileSize);
                    if ((sumaArchivos / 1024) > (1024 * 1024))
                        throw new Exception(string.Format("El tamaño maximo de carga es de {0}MB", "10"));
                    afuArchivo.PostedFile.SaveAs(BusinessVariables.Directorios.RepositorioTemporalInformacionConsulta + afuArchivo.FileName);
                    HelperFiles hFiles = new HelperFiles { NombreArchivo = e.FileName.Split('\\').Last(), Tamaño = BusinessFile.ConvertirTamaño(e.FileSize), Extension = Path.GetExtension(afuArchivo.FileName) };
                    files.Add(hFiles);
                    Session["FileSize"] = int.Parse(Session["FileSize"].ToString()) + int.Parse(e.FileSize);
                    Session["selectedFiles"] = files;
                }
                LlenaArchivosCargados();
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

        protected void ReloadThePanel_OnClick(object sender, EventArgs e)
        {
            try
            {
                LlenaArchivosCargados();
                Session["txtEditor"] = txtEditor.Text;

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

        protected void btnRemoveFile_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnk = (LinkButton)sender;
                List<HelperFiles> lstArchivos = (List<HelperFiles>)Session["selectedFiles"];
                List<HelperFiles> lstArchivosEliminar = lstArchivos.Where(s => s.NombreArchivo == lnk.CommandArgument).ToList();
                foreach (HelperFiles file in lstArchivosEliminar)
                {
                    if (lstArchivos.Contains(file))
                        lstArchivos.Remove(file);
                }

                Session["selectedFiles"] = lstArchivos;
                LlenaArchivosCargados();
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
                AlertaGeneral = _lstError;
            }
        }

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCaptura())
                {
                    InformacionConsulta informacion = ObtenerInformacionCapturada();
                    List<HelperFiles> lstArchivos = (List<HelperFiles>)Session["selectedFiles"];
                    if (EsAlta)
                    {
                        informacion = _servicioInformacionConsulta.GuardarInformacionConsulta(informacion, lstArchivos.Select(s => s.NombreArchivo).ToList());
                        RenombraArchivos(lstArchivos.Select(s => s.NombreArchivo).ToList(), informacion.Id);
                    }
                    else
                        informacion = _servicioInformacionConsulta.ActualizarInformacionConsulta(IdInformacionConsulta, informacion, lstArchivos.Select(s => s.NombreArchivo).ToList());

                    LimpiarCampos();
                    if (OnAceptarModal != null)
                        OnAceptarModal();
                }
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

        protected void btnPreview_OnClick(object sender, EventArgs e)
        {
            try
            {
                Session["PreviewDataConsulta"] = ObtenerInformacionCapturada();
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/"; ;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "window.open('" + url + "Publico/Consultas/FrmPreviewConsulta.aspx','_blank');", true);
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
    }
}