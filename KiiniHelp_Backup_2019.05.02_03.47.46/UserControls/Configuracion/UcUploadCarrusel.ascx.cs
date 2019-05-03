using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Configuracion
{
    public partial class UcUploadCarrusel : UserControl
    {

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

        private void GetImages()
        {
            try
            {
                List<string> images = new List<string>();
                string[] arch = Directory.GetFiles(ConfigurationManager.AppSettings["RepositorioCarousel"], "*.*", SearchOption.TopDirectoryOnly);
                foreach (string img in arch)
                {
                    FileInfo fi = new FileInfo(img);
                    images.Add("~/assets/carouselImage/" + fi.Name);
                }
                rptImages.DataSource = images;
                rptImages.DataBind();
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
                GetImages();
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
                if (File.Exists(BusinessVariables.Directorios.RepositorioCarrusel + e.FileName.Split('\\').Last()))
                    File.Delete(BusinessVariables.Directorios.RepositorioCarrusel + e.FileName.Split('\\').Last());
                uploadControl.SaveAs(BusinessVariables.Directorios.RepositorioCarrusel + e.FileName.Split('\\').Last());
                
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

        protected void ReloadThePanel_OnClick(object sender, EventArgs e)
        {
            try
            {
                GetImages();
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

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                if (btn != null)
                {
                    string[] file = btn.CommandArgument.Split('/');
                    File.Delete(BusinessVariables.Directorios.RepositorioCarrusel + file.Last());
                }
                GetImages();
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