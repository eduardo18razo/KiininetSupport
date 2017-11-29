using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.Office.Core;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace KinniNet.Business.Utils
{
    public static class BusinessFile
    {
        public static string ConvertirTamaño(string fileSize)
        {
            string result = null;
            try
            {
                decimal tamano = decimal.Parse(fileSize) / 1024;
                if (tamano <= 1024) //KB
                {
                    result = tamano.ToString("0.0") + " KB";
                }
                else if (tamano > 1024 && tamano <= 1024000) //MB
                {
                    result = (tamano / 1024).ToString("0.0") + " MB";
                }
                else if (tamano > 1024000 && tamano <= 1024000000) //GB
                {
                    result = ((tamano / 1024) / 1024).ToString("0.0") + " GB";
                }
                else if (tamano > 1024000000 && tamano <= 1024000000000) //TB
                {
                    result = (((tamano / 1024) / 1024) / 1024).ToString("0.0") + " TB";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Devuelve si el archivo existe
        /// </summary>
        /// <param name="file">Ruta completa y nombre de archivo</param>
        /// <returns></returns>
        public static bool ExisteArchivo(string file)
        {
            bool result;
            try
            {
                result = File.Exists(file);
            }
            catch (Exception)
            {
                throw new Exception("Error al validar archivo.");
            }
            return result;
        }

        public static void CopiarArchivoDescarga(string rutaOrigen, string nombreArchivo, string rutaDestino)
        {
            try
            {
                if (!File.Exists(rutaDestino + nombreArchivo))
                    File.Copy(rutaOrigen + nombreArchivo, rutaDestino + nombreArchivo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CopiarArchivo(string rutaOrigen, string rutaDestino, string nombreArchivo)
        {
            try
            {
                if (!File.Exists(rutaDestino + nombreArchivo))
                    File.Copy(rutaOrigen + nombreArchivo, rutaDestino + nombreArchivo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void EliminarArchivo(string ruta, string archivo)
        {
            try
            {
                if (File.Exists(ruta + archivo))
                    File.Delete(ruta + archivo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void RenombrarArchivosConsulta(List<string> archivos, int id)
        {
            try
            {
                foreach (string archivo in archivos)
                {
                    if (!Directory.Exists(BusinessVariables.Directorios.RepositorioInformacionConsulta))
                        Directory.CreateDirectory(BusinessVariables.Directorios.RepositorioInformacionConsulta);
                    if (File.Exists(BusinessVariables.Directorios.RepositorioInformacionConsulta + id + archivo))
                        File.Delete(BusinessVariables.Directorios.RepositorioInformacionConsulta + id + archivo);
                    File.Move(BusinessVariables.Directorios.RepositorioInformacionConsulta + archivo, BusinessVariables.Directorios.RepositorioInformacionConsulta + id + archivo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void MoverTemporales(string folderOrigen, string folderDestino, List<string> archivos)
        {
            try
            {
                foreach (string archivo in archivos)
                {
                    if (!Directory.Exists(folderDestino))
                        Directory.CreateDirectory(folderDestino);
                    if (File.Exists(folderDestino + archivo))
                        File.Delete(folderDestino + archivo);
                    File.Move(folderOrigen + archivo, folderDestino + archivo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CopiarSitioTemporal(string folderOrigen, string folderDestino, List<string> archivos)
        {
            try
            {
                foreach (string archivo in archivos)
                {
                    if (!Directory.Exists(folderDestino))
                        Directory.CreateDirectory(folderDestino);
                    if (File.Exists(folderDestino + archivo))
                        File.Delete(folderDestino + archivo);
                    File.Copy(folderOrigen + archivo, folderDestino + archivo);
                }
            }
            catch (FileNotFoundException ex)
            {
                if (ex is FileNotFoundException)
                {
                    throw new Exception("El archivono se encuentra disponible");
                }
                throw new Exception(ex.Message);
            }
        }

        public static void LimpiarRepositorioTemporal(List<string> archivos)
        {
            try
            {
                foreach (string archivo in archivos)
                {
                    File.Delete(BusinessVariables.Directorios.RepositorioTemporal + archivo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void LimpiarTemporales(List<string> archivos)
        {
            try
            {
                foreach (string archivo in archivos)
                {
                    File.Delete(BusinessVariables.Directorios.RepositorioTemporalInformacionConsulta + archivo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static class Imagenes
        {
            public static byte[] ImageToByteArray(Stream fileImage, int contentLenght)
            {
                byte[] data = null;
                try
                {

                    using (var binaryReader = new BinaryReader(fileImage))
                    {
                        data = binaryReader.ReadBytes(contentLenght);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return data;

            }
            public static byte[] ImageToByteArray(string fileImage)
            {
                byte[] data = null;
                try
                {

                    FileInfo fInfo = new FileInfo(fileImage);
                    long numBytes = fInfo.Length;
                    FileStream fStream = new FileStream(fileImage, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fStream);
                    data = br.ReadBytes((int)numBytes);

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return data;
            }

            public static Image ByteArrayToImage(byte[] byteArrayIn)
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        public static void ConvertirWord(string nombrearchivo)
        {
            string logFile = BusinessVariables.Directorios.RepositorioRepositorio + @"LogArchivosWord.txt";
            if (!File.Exists(logFile))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine("Inicio Log" + DateTime.Now.ToString());
                }
            }
            Word._Application winWord = null;
            using (StreamWriter file = new StreamWriter(logFile))
            {
                try
                {
                    file.WriteLine("Inicia Proceso");
                    string htmlFilePath = BusinessVariables.Directorios.RepositorioInformacionConsultaHtml + Path.GetFileNameWithoutExtension(nombrearchivo) + ".htm";
                    string fileName = BusinessVariables.Directorios.RepositorioInformacionConsulta + nombrearchivo;
                    //nuevo codigo
                    Object oMissing = System.Reflection.Missing.Value;

                    Object oTemplatePath = "D:\\MyTemplate.dotx";
                    Word.Application wordApp = new Word.Application();
                    Word.Document wordDoc = new Word.Document();
                    file.WriteLine("Inicia Proceso abre documento nuevo codigo");
                    wordDoc = wordApp.Documents.Add(fileName);
                    file.WriteLine("Inicia Proceso abrio documento nuevo codigo");
                    file.WriteLine("Inicia Proceso Guarda html nuevo codigo");
                    wordDoc.SaveAs(htmlFilePath, Word.WdSaveFormat.wdFormatHTML);
                    file.WriteLine("Inicia Proceso guardo html nuevo codigo");
                    //wordApp.Documents.Open("myFile.doc");
                    wordApp.Application.Quit();
                    //Finnuevo codigo


                    //file.WriteLine("Inicia aplicacion");
                    //winWord = new Word.ApplicationClass();
                    //winWord.Visible = true;
                    //file.WriteLine("Abre Archivo");
                    //winWord.Documents.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //file.WriteLine("Abrio Archivo");
                    //file.WriteLine("Activa documento de trabajo");
                    //Word.Document doc = winWord.ActiveDocument;
                    //file.WriteLine("Activo documento de trabajo");
                    //file.WriteLine(htmlFilePath);

                    //doc.SaveAs(htmlFilePath, Word.WdSaveFormat.wdFormatHTML, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //file.WriteLine("Guardo nuevo formato");
                    //doc.Close();
                    //file.WriteLine("cerro documento");
                    //winWord.Quit();
                    //file.WriteLine("Cerro Word");
                }
                catch (Exception e)
                {
                    if (winWord != null)
                    {
                        winWord.Quit();
                    }
                    throw new Exception(e.Message);
                }
                finally
                {
                    GC.Collect();
                }
            }
        }
        public static void ConvertirExcel(string nombrearchivo)
        {
            string logFile = BusinessVariables.Directorios.RepositorioRepositorio + @"LogArchivosExcel.txt";
            if (!File.Exists(logFile))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine("Inicio Log" + DateTime.Now.ToString());
                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logFile))
            {
                try
                {
                    file.WriteLine("Inicia Proceso");
                    string htmlFilePath = BusinessVariables.Directorios.RepositorioInformacionConsultaHtml + Path.GetFileNameWithoutExtension(nombrearchivo) + ".htm";
                    string directoryPath = BusinessVariables.Directorios.RepositorioInformacionConsultaHtml + Path.GetFileNameWithoutExtension(nombrearchivo) + "_archivos";
                    string fileName = BusinessVariables.Directorios.RepositorioInformacionConsulta + nombrearchivo;
                    file.WriteLine("Inicia aplicacion");
                    Excel._Application xls = new Excel.ApplicationClass();
                    xls.Visible = false;
                    file.WriteLine("Abre Archivo");
                    xls.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    file.WriteLine("Abrio Archivo");
                    file.WriteLine("Activa hoja de trabajo");
                    Excel.Workbook wb = xls.ActiveWorkbook;
                    file.WriteLine("Activo hoja de trabajo");
                    file.WriteLine(htmlFilePath);

                    wb.SaveAs(htmlFilePath, Excel.XlFileFormat.xlHtml, Type.Missing, Type.Missing, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    file.WriteLine("Guardo nuevo formato");
                    wb.Close();
                    file.WriteLine("cerro libro de trabajo");
                    xls.Quit();
                    file.WriteLine("Cerro excel");

                }
                catch (Exception e)
                {
                    file.WriteLine("error proceso \n" + e.InnerException.Message + " Error gral\n" + e.Message);
                    throw new Exception(e.Message);

                }
                finally
                {
                    GC.Collect();
                }
            }
        }
        public static void ConvertirPowerPoint(string nombrearchivo)
        {
            string logFile = BusinessVariables.Directorios.RepositorioRepositorio + @"LogArchivosWord.txt";
            if (!File.Exists(logFile))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine("Inicio Log" + DateTime.Now.ToString());
                }
            }
            PowerPoint._Application winWord = null;
            using (StreamWriter file = new StreamWriter(logFile))
            {
                try
                {

                    file.WriteLine("Inicia Proceso");
                    string htmlFilePath = BusinessVariables.Directorios.RepositorioInformacionConsultaHtml + Path.GetFileNameWithoutExtension(nombrearchivo) + ".htm";
                    string fileName = BusinessVariables.Directorios.RepositorioInformacionConsulta + nombrearchivo;

                    PowerPoint.Application ppApp = new PowerPoint.Application();
                    ppApp.Visible = MsoTriState.msoTrue;
                    PowerPoint.Presentations ppPresens = ppApp.Presentations;
                    PowerPoint.Presentation objPres = ppPresens.Open(fileName, MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoTrue);
                    file.WriteLine("Abrio Presentacion");
                    file.WriteLine("Guardara Nueva Presentacion nuevo formato");
                    file.WriteLine(htmlFilePath);
                    objPres.SaveAs(htmlFilePath, PowerPoint.PpSaveAsFileType.ppSaveAsHTML, MsoTriState.msoCTrue);
                    file.WriteLine("Guardo nuevo formato");
                    PowerPoint.Slides objSlides = objPres.Slides;
                    //PowerPoint.SlideShowWindows objSSWs; 
                    //PowerPoint.SlideShowSettings objSSS;
                    ////Run the Slide show
                    //objSSS = objPres.SlideShowSettings;
                    //objSSS.Run();
                    //objSSWs = ppApp.SlideShowWindows;
                    //while (objSSWs.Count >= 1)
                    //    System.Threading.Thread.Sleep(100);
                    ////Close the presentation without saving changes and quit PowerPoint
                    //objPres.Close();
                    ppApp.Quit();



                    //file.WriteLine("Inicia aplicacion");
                    //winWord = new PowerPoint.ApplicationClass();
                    //file.WriteLine("Abre Archivo");
                    //PowerPoint.Presentation prsPres = winWord.Presentations.Open(fileName, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                    //file.WriteLine("Abrio Archivo");
                    //file.WriteLine(htmlFilePath);
                    //file.WriteLine("GuardaraNuevo Documento nuevo formato");
                    //prsPres.SaveAs(htmlFilePath, PowerPoint.PpSaveAsFileType.ppSaveAsHTML);
                    //file.WriteLine("Guardo nuevo formato");
                    //prsPres.Close();
                    //file.WriteLine("cerro documento");
                    //winWord.Quit();
                    //file.WriteLine("Cerro Word");
                }
                catch (Exception e)
                {
                    if (winWord != null)
                    {
                        winWord.Quit();
                    }
                    throw new Exception(e.Message);
                }
                finally
                {
                    GC.Collect();
                }
            }
        }
        public static class ExcelManager
        {
            public static DataTable ObtenerHojasExcel(string nombreArchivo)
            {
                OleDbConnection myConnection = null;
                DataTable result;
                try
                {
                    myConnection = new OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + nombreArchivo + "';Extended Properties=Excel 12.0;");
                    myConnection.Open();
                    result = myConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (myConnection != null && myConnection.State == ConnectionState.Open)
                    {
                        myConnection.Close();
                    }
                }
                return result;
            }

            public static DataSet LeerHojaExcel(string archivo, string hoja)
            {
                OleDbConnection excelCoon = null;
                DataSet dtSet;
                try
                {
                    excelCoon = new OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + BusinessVariables.Directorios.RepositorioTemporal + archivo + " ';Extended Properties=Excel 12.0;");
                    OleDbDataAdapter cmd = new OleDbDataAdapter("select * from [" + hoja + "]", excelCoon);
                    excelCoon.Open();
                    cmd.TableMappings.Add("Table", "tablaPaso");
                    dtSet = new DataSet();
                    cmd.Fill(dtSet);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (excelCoon != null && excelCoon.State == ConnectionState.Open)
                    {
                        excelCoon.Close();
                    }
                }
                return dtSet;
            }

            public static ExcelPackage ListToExcel<T>(List<T> query)
            {
                ExcelPackage result = null;
                try
                {
                    ExcelPackage pck = new ExcelPackage();
                    string celdaFinHeader = null;
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Result");

                    //get our column headings
                    var t = typeof(T);
                    var Headings = t.GetProperties();
                    for (int i = 0; i < Headings.Count(); i++)
                    {

                        ws.Cells[1, i + 1].Value = Headings[i].Name;
                        celdaFinHeader = ws.Cells[1, i + 1].Address;
                    }

            //        var mi = typeof(T)
            //.GetProperties()
            //.Select(pi => (MemberInfo)pi)
            //.ToArray();
                    
                    //populate our Data
                    if (query.Any())
                    {

                        ws.Cells["A2"].LoadFromCollection(query);
                        //ws.Cells["A2"].LoadFromCollection(query, false, TableStyles.Custom, BindingFlags.Public, mi);

                    }

                    //Format the header
                    using (ExcelRange rng = ws.Cells[string.Format("A1:{0}", celdaFinHeader)])
                    {
                        rng.AutoFitColumns();
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                        rng.Style.Font.Color.SetColor(Color.White);
                    }
                    ws.Cells.AutoFitColumns();
                    result = pck;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }

    }
}
