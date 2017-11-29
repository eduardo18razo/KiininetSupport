using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniNet.Entities.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Preview
{
    public partial class UcPreviewConsulta : UserControl, IControllerModal
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

        public bool MuestraEvaluacion
        {
            get { return bool.Parse(hfEvaluacion.Value); }
            set { hfEvaluacion.Value = value.ToString(); } 
        }

        public void MuestraPreview(InformacionConsulta datos)
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
                if (Session["PreviewDataConsulta"] != null)
                {
                    MuestraPreview((InformacionConsulta)Session["PreviewDataConsulta"]);
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

        protected void btnPreviewDocument_OnClick(object sender, EventArgs e)
        {
            try
            {
                return;
                LinkButton btn = (LinkButton)sender;
                BusinessFile.MoverTemporales(BusinessVariables.Directorios.Carpetaemporal, BusinessVariables.Directorios.RepositorioTemporal, new List<string> { btn.CommandArgument });

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