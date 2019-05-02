using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Preview
{
    public partial class UcPreviewOpcionConsulta : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private List<string> _lstError = new List<string>();

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

        private void MuestraPreview(InformacionConsulta datos)
        {
            try
            {
                lblTitle.Text = datos.Descripcion;
                TextPreview.InnerHtml = datos.InformacionConsultaDatos.First().Datos;
                rptArchivos.DataSource = Session["selectedFiles"];
                rptArchivos.DataBind();
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
                if (Session["PreviewAltaDataConsulta"] != null && Request.QueryString["evaluacion"] != null)
                {
                    InformacionConsulta info = (InformacionConsulta) Session["PreviewAltaDataConsulta"];
                    List<HelperFiles> lstArchivos = new List<HelperFiles>();
                    foreach (InformacionConsultaDocumentos docto in info.InformacionConsultaDocumentos)
                    {
                        if (!File.Exists(BusinessVariables.Directorios.RepositorioInformacionConsulta + docto.Archivo))
                        {
                            Alerta = new List<string> { string.Format("El archivo {0} no esta disponible.", docto.Archivo) };
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
                    MuestraPreview(info);
                    divEvaluacion.Visible = bool.Parse(Request.QueryString["evaluacion"]);
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

        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton) sender;
                BusinessFile.MoverTemporales(BusinessVariables.Directorios.CarpetaTemporal, BusinessVariables.Directorios.RepositorioTemporal, new List<string>{btn.CommandArgument});

                string script = string.Format("window.open('https://docs.google.com/viewer?url=http://{0}/tmp/{1}','_blank')", WebConfigurationManager.AppSettings["siteUrl"], btn.CommandArgument);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptViewer", script, true);
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