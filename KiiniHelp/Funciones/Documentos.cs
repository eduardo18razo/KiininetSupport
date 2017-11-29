using System;
using System.IO;
using KinniNet.Business.Utils;
using Page = System.Web.UI.Page;

namespace KiiniHelp.Funciones
{
    public static class Documentos
    {
        public static void MostrarDocumento(string nombrearchivo, Page page, string directorio)
        {
            try
            {
                string rutaHtml = BusinessVariables.Directorios.RepositorioInformacionConsultaHtml;

                string htmlFilePath = BusinessVariables.Directorios.RepositorioInformacionConsultaHtml + Path.GetFileNameWithoutExtension(nombrearchivo) + ".htm";
                //string directoryPath = page.Server.MapPath(rutaHtml) + Path.GetFileNameWithoutExtension(nombrearchivo) + "_archivos";
                //string directorioTemporal = directorio + Path.GetFileNameWithoutExtension(nombrearchivo) + "_archivos";
                //CopyFilesRecursively(new DirectoryInfo(directoryPath), new DirectoryInfo(directorio));
                byte[] bytes;
                using (FileStream fs = new FileStream(htmlFilePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader reader = new BinaryReader(fs);
                    bytes = reader.ReadBytes((int)fs.Length);
                    fs.Close();
                }
                page.Response.BinaryWrite(bytes);
                page.Response.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void EliminarTemporales()
        {
            
        }

        private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            try
            {
                if (Directory.Exists(Path.Combine(target.FullName, source.Name))) return;
                target.CreateSubdirectory(source.Name);
                foreach (FileInfo file in source.GetFiles())
                {
                    file.CopyTo(Path.Combine(Path.Combine(target.FullName, source.Name), file.Name));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}