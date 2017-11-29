using System;
using System.Net;
using KiiniHelp.Funciones;
using KinniNet.Business.Utils;
using Page = System.Web.UI.Page;

namespace KiiniHelp.Users.General
{
    public partial class FrmMostrarDocumento : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string nombreDocto = Request.QueryString["NombreDocumento"];
                    int tipoInformacion = Convert.ToInt32(Request.QueryString["TipoDocumento"]);
                    string directorio = Server.MapPath("~/Users/General/");
                    if (!IsPostBack)
                    {
                        switch (tipoInformacion)
                        {
                            case (int)BusinessVariables.EnumTiposDocumento.Word:
                                Documentos.MostrarDocumento(nombreDocto, this, directorio);
                                break;
                            case (int)BusinessVariables.EnumTiposDocumento.PowerPoint:
                                Documentos.MostrarDocumento(nombreDocto, this, directorio);
                                break;
                            case (int)BusinessVariables.EnumTiposDocumento.Excel:
                                Documentos.MostrarDocumento(nombreDocto, this, directorio);
                                break;
                            case (int)BusinessVariables.EnumTiposDocumento.Pdf:
                                MostrarPdf(nombreDocto, BusinessVariables.Directorios.RepositorioInformacionConsulta);
                                break;
                            case (int)BusinessVariables.EnumTiposDocumento.Imagen:
                                MostrarImagen(nombreDocto, BusinessVariables.Directorios.RepositorioInformacionConsulta);
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void MostrarPdf(string nombreDocumento, string directorio)
        {
            try
            {
                // PDF
                //string FilePath = Server.MapPath("CELE860311HDFRCD04.pdf");
                string filePath = directorio + nombreDocumento;
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                }
            }
            catch (Exception)
            {

            }
        }

        private void MostrarImagen(string nombreDocumento, string directorio)
        {

            try
            {
                const string tmpPath = "~/Users/General/";
                BusinessFile.CopiarArchivoDescarga(directorio, nombreDocumento, Server.MapPath(tmpPath));
                imgDinamic.ImageUrl = tmpPath + nombreDocumento;
                imgDinamic.Visible = true;
            }
            catch
            {
            }
        }

    }
}