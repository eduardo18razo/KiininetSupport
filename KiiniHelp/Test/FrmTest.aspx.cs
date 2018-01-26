using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceSistemaCatalogos;
using KinniNet.Business.Utils;
using Telerik.Web.UI;
using Font = System.Drawing.Font;
using Menu = KiiniNet.Entities.Cat.Sistema.Menu;

namespace KiiniHelp.Test
{
    public partial class FrmTest : Page
    {

        private readonly ServiceCatalogosClient _servicioCatalogos = new ServiceCatalogosClient();
        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();
        private List<string> _lstError = new List<string>();

        //private void SendMessageAltiria()
        //{
        //    HttpWebRequest loHttp =
        //        (HttpWebRequest)WebRequest.Create("http://www.altiria.net/api/http");

        //    string lcPostData = "cmd=sendsms&domainId=demo&login=edu18&passwd=fhmdemok&dest=525554374934&msg=tercer";

        //    byte[] lbPostBuffer = System.Text.Encoding.GetEncoding("utf-8").GetBytes(lcPostData);
        //    loHttp.Method = "POST";
        //    loHttp.ContentType = "application/x-www-form-urlencoded";
        //    loHttp.ContentLength = lbPostBuffer.Length;
        //    loHttp.Timeout = 60000;
        //    String error = "";
        //    String response = "";
        //    try
        //    {
        //        Stream loPostData = loHttp.GetRequestStream();
        //        loPostData.Write(lbPostBuffer, 0, lbPostBuffer.Length);
        //        loPostData.Close();
        //        HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
        //        Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
        //        StreamReader loResponseStream =
        //            new StreamReader(loWebResponse.GetResponseStream(), enc);
        //        response = loResponseStream.ReadToEnd();
        //        loWebResponse.Close();
        //        loResponseStream.Close();
        //    }
        //    catch (WebException e)
        //    {
        //        if (e.Status == WebExceptionStatus.ConnectFailure)
        //            error = "Error en la conexión";
        //        else if (e.Status == WebExceptionStatus.Timeout)
        //            error = "Error TimeOut";
        //        else
        //            error = e.Message;
        //    }
        //    finally
        //    {
        //        if (error != "")
        //            lblerrorMensaje.Text = error;
        //        else
        //            lblerrorMensaje.Text =  response;
        //    }
        //}
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


        public int IdCatalogo { get { return 10; } }
        protected void btnGetSelectedValues_Click(object sender, EventArgs e)
        {
            //string selectedValues = string.Empty;
            //foreach (ListItem li in lstBoxTest.Items)
            //{
            //    if (li.Selected == true)
            //    {
            //        selectedValues += li.Text + ",";
            //    }
            //}
            //Response.Write(selectedValues.ToString());
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TextBox1.Text = GridView1.SelectedRow.Cells[2].Text;
        }
        void asyncFileUpload_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                List<string> lstArchivo = Session["Files"] == null ? new List<string>() : (List<string>)Session["Files"];
                if (lstArchivo.Contains(e.FileName.Split('\\').Last())) return;
                AsyncFileUpload uploadControl = (AsyncFileUpload)sender;
                if (!Directory.Exists(BusinessVariables.Directorios.RepositorioTemporalMascara))
                    Directory.CreateDirectory(BusinessVariables.Directorios.RepositorioTemporalMascara);
                uploadControl.SaveAs(BusinessVariables.Directorios.RepositorioTemporalMascara + e.FileName.Split('\\').Last());
                Session[uploadControl.ID] = e.FileName.Split('\\').Last();
                lstArchivo.Add(e.FileName.Split('\\').Last());
                Session["Files"] = lstArchivo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, "[^a-zA-Z0-9]+", string.Empty, RegexOptions.Compiled);
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (Exception e)
            {
                return String.Empty;
            }
        }
        public List<InfoClass> ObtenerPropiedadesObjeto(object obj)
        {
            List<InfoClass> result;
            try
            {
                var propertiesArea = GetProperties(obj);
                result = (from info in propertiesArea
                          where info.PropertyType.Name == "String" || info.PropertyType.Name == "Int32" || info.PropertyType.Name == "DateTime"
                          select new InfoClass
                          {
                              Name = info.Name,
                              Type = info.PropertyType.Name
                          }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        private IEnumerable<PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetProperties().ToList();
        }

        public class InfoClass
        {
            public string Name { get; set; }
            public string Type { get; set; }

        }
        private void Doing()
        {

        }

        private void GeneraGraficaTelerik(DataTable dt )
        {
            try
            {
                graficaBack.Width = Unit.Pixel(680);
                graficaBack.Height = Unit.Pixel(400);
                graficaBack.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;


                foreach (DataColumn column in dt.Columns)
                {
                    ScatterSeries serie = new ScatterSeries();
                }
                

                graficaBack.PlotArea.XAxis.TitleAppearance.Text = "Volts";
                graficaBack.PlotArea.YAxis.TitleAppearance.Text = "mA";

                ScatterLineSeries theoreticalData = new ScatterLineSeries();
                theoreticalData.Name = "Theoretical Data";
                theoreticalData.LabelsAppearance.Visible = false;
                theoreticalData.TooltipsAppearance.Color = System.Drawing.Color.White;
                theoreticalData.TooltipsAppearance.DataFormatString = "{0} Volts, {1} mA";

                ScatterSeries experimentalData = new ScatterSeries();
                experimentalData.Name = "Experimental Data";
                experimentalData.LabelsAppearance.Visible = false;
                experimentalData.TooltipsAppearance.DataFormatString = "{0} Volts, {1} mA";
                experimentalData.TooltipsAppearance.Color = System.Drawing.Color.White;

                foreach (DataRow row in GetChartData().Rows)
                {
                    decimal? currentVolts = (decimal?)row["Volts"];
                    if (!(row["mATheoretical"] is DBNull))
                    {
                        decimal? currentMATheoretical = (decimal?)row["mATheoretical"];
                        ScatterSeriesItem theoreticalItem = new ScatterSeriesItem(currentVolts, currentMATheoretical);
                        theoreticalData.SeriesItems.Add(theoreticalItem);
                    }
                    decimal? currentMAExperimental = (decimal?)row["mAExperimental"];
                    ScatterSeriesItem experimentalItem = new ScatterSeriesItem(currentVolts, currentMAExperimental);
                    experimentalData.SeriesItems.Add(experimentalItem);
                }
                scatterChart.PlotArea.Series.Add(theoreticalData);
                scatterChart.PlotArea.Series.Add(experimentalData);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        //    cGraficoEspacio.Series[0].Points.Add(new DataPoint(0, 200));
        //    cGraficoEspacio.Series[1].Points.Add(new DataPoint(0, 1000));

            //// October Data
            //cGraficoEspacio.Series[1].Points.Add(new DataPoint(0, 20));
            //cGraficoEspacio.Series[1].Points.Add(new DataPoint(1, 16));

            //// April Data
            //cGraficoEspacio.Series[2].Points.Add(new DataPoint(0, 15));
            //cGraficoEspacio.Series[2].Points.Add(new DataPoint(1, 18));

            //foreach (Series cs in cGraficoEspacio.Series)
            //    cs.ChartType = SeriesChartType.StackedBar100;

            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Ocupado", typeof(double));
            dt.Columns.Add("Libre", typeof(double));
            dt.Columns.Add("Titulo", typeof(string));

            dt.Rows.Add(123.5 , 900.5, "Almacenado");
            
            List<Object> dataSource = new List<object>();
            dataSource.Add(new { Ocupado = 350, Libre = 700, Titulo = "Almacenado" });

            rcTest.DataSource = dt;
            rcTest.DataBind();


            


            //BusinessGraficosDasboard.Pastel.GeneraGraficoBarraApilada(cGraficoEspacio, dt);
            //ucDetalleArbolAcceso.IdArbolAcceso = 7;
        }

        //protected void Submit(object sender, EventArgs e)
        //{
        //    string message = "";
        //    foreach (ListItem item in lstFruits.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            message += item.Text + " " + item.Value + "\\n";
        //        }
        //    }
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + message + "');", true);
        //}


        protected void btnModalImpactoUrgencia_OnClick(object sender, EventArgs e)
        {
            try
            {
                //btnModalImpactoUrgencia.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalImpacto\");", true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private void ChartStackOnClick(object sender, ImageMapEventArgs imageMapEventArgs)
        {
            try
            {
                //lblFormatMonth.Text = sender;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnConsultar_OnClick(object sender, EventArgs e)
        {

        }

        private void MakeBarChart(Chart grafico, DataTable dt)
        {
            grafico.Series["Ubicacion"].Label = "#VALY{#.##}";
            for (int point = 0; point < dt.Rows.Count; point++)
            {
                grafico.Series["Ubicacion"].Points.Add(new DataPoint
                {
                    AxisLabel = dt.Rows[point][0].ToString(),
                    YValues = new double[] { Convert.ToDouble(dt.Rows[point][1].ToString()) }
                });
            }
            foreach (Series serie in grafico.Series)
            {
                serie.Font = new Font("Tahoma", 8, FontStyle.Bold);
                foreach (DataPoint dp in serie.Points)
                {
                    dp.PostBackValue = "#VALX,#VALY";
                    dp.LabelPostBackValue = "label";
                    dp.LegendPostBackValue = "legendVALX";
                }
            }

        }

        public void AfterLoad(DataTable dt)
        {

            //Chart1.Series.Add("Ubicacion");
            //MakeBarChart(Chart1, dt);
            //MakeParetoChart(Chart1, "Ubicacion", "Pareto");
            //Chart1.Series["Pareto"].ChartType = SeriesChartType.Line;
            //Chart1.Series["Pareto"].IsValueShownAsLabel = true;
            //Chart1.Series["Pareto"].MarkerColor = Color.Red;
            //Chart1.Series["Pareto"].MarkerBorderColor = Color.MidnightBlue;
            //Chart1.Series["Pareto"].MarkerStyle = MarkerStyle.Circle;
            //Chart1.Series["Pareto"].MarkerSize = 8;
            //Chart1.Series["Pareto"].LabelFormat = "0.#";
            //Chart1.Series["Pareto"].Color = Color.FromArgb(252, 180, 65);
            //Chart1.Legends[0].Title = "Stack";
        }

        protected void rptMenu_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu1"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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
        protected void rptSubMenu1_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu2"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu2_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu3"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu3_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu4"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu4_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu5"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu5_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu6"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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

        protected void rptSubMenu6_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater rptSubMenu = ((Repeater)e.Item.FindControl("rptSubMenu7"));
                if (rptSubMenu != null)
                {
                    rptSubMenu.DataSource = ((Menu)e.Item.DataItem).Menu1;
                    rptSubMenu.DataBind();
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


        private void MakeParetoChart(Chart chart, string srcSeriesName, string destSeriesName)
        {

            // get name of the ChartAre of the source series

            string strChartArea = chart.Series[srcSeriesName].ChartArea;

            // ensure that the source series is a column chart type

            chart.Series[srcSeriesName].ChartType = SeriesChartType.Column;

            // sort the data in all series by their values in descending order

            chart.DataManipulator.Sort(PointSortOrder.Descending, srcSeriesName);

            // find the total of all points in the source series

            double total = 0.0;

            foreach (DataPoint pt in chart.Series[srcSeriesName].Points)

                total += pt.YValues[0];

            // set the max value on the primary axis to total

            chart.ChartAreas[strChartArea].AxisY.Maximum = total;

            // create the destination series and add it to the chart

            Series destSeries = new Series(destSeriesName);

            chart.Series.Add(destSeries);

            // ensure that the destination series is either a Line or Spline chart type

            destSeries.ChartType = SeriesChartType.Line;

            destSeries.BorderWidth = 3;

            // assign the series to the same chart area as the column chart is assigned

            destSeries.ChartArea = chart.Series[srcSeriesName].ChartArea;

            // assign this series to use the secondary axis and set it maximum to be 100%

            destSeries.YAxisType = AxisType.Secondary;

            chart.ChartAreas[strChartArea].AxisY2.Maximum = 100;

            // locale specific percentage format with no decimals

            //chart.ChartAreas[strChartArea].AxisY2.LabelStyle.Format = "P0";

            // turn off the end point values of the primary X axis

            chart.ChartAreas[strChartArea].AxisX.LabelStyle.IsEndLabelVisible = false;

            double percentage = 0.0;

            foreach (DataPoint pt in chart.Series[srcSeriesName].Points)
            {

                percentage += (pt.YValues[0] / total * 100.0);
                if (percentage >= 1000)
                {

                }
                destSeries.Points.Add(Math.Round(percentage, 2));

            }

        }

        //protected void OnClick(object sender, EventArgs e)
        //{
        //    string url = ResolveUrl("~/FrmEncuesta.aspx?IdTicket=3");
        //    string s = "window.open('" + url + "', 'popup_window', 'width=600,height=600,left=300,top=100,resizable=yes');";
        //    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        //}

        //protected void OnClick(object sender, EventArgs e)
        //{

        //    //lblFormatWeek.Text = txtWeek.Text;
        //    //lblFormatMonth.Text = txtMonth.Text;

        //    //BusinessCadenas.Fechas.ObtenerRangoFechasNumeroSemana(Convert.ToInt32(txtWeek.Text.Split('-')[0]),
        //    //    Convert.ToInt32(txtWeek.Text.Split('-')[1].Substring(1)));
        //    //DateTime jan1 = new DateTime(Convert.ToInt32(txtWeek.Text.Split('-')[0]), 1, 1);

        //    //int daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;

        //    //DateTime firstMonday = jan1.AddDays(daysOffset);

        //    //var cal = CultureInfo.CurrentCulture.Calendar;

        //    //int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        //    //int weekNum = Convert.ToInt32(txtWeek.Text.Split('-')[1].Substring(1));

        //    //if (firstWeek <= 1)
        //    //{
        //    //    weekNum -= 1;
        //    //}

        //    //var result = firstMonday.AddDays(weekNum * 7 + 0 - 1);
        //}

        protected void btnAbrirModal_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ucAltaNivelArbol.IdArea = 1;
                //ucAltaNivelArbol.IdNivel1 = 1;
                //ucAltaNivelArbol.IdTipoArbol = 1;
                //ucAltaNivelArbol.IdTipoUsuario = 1;
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#editNivel\");", true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //private void LlenaArchivosCargados()
        //{
        //    try
        //    {
        //        rptFiles.DataSource = Session["selectedFiles"];
        //        rptFiles.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //protected void aUploadFiles_OnUploadedComplete(object sender, AsyncFileUploadEventArgs e)
        //{
        //    try
        //    {
        //        List<string> files = Session["selectedFiles"] == null ? new List<string>() : (List<string>)Session["selectedFiles"];
        //        if (!files.Contains(aUploadFiles.FileName))
        //        {
        //            files.Add(aUploadFiles.FileName);
        //            Session["selectedFiles"] = files;
        //        }
        //        LlenaArchivosCargados();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_lstError == null)
        //        {
        //            _lstError = new List<string>();
        //        }
        //        _lstError.Add(ex.Message);
        //        AlertaGeneral = _lstError;
        //    }
        //}
        //private InformacionConsulta ObtenerInformacionCapturada()
        //{
        //    //InformacionConsulta result;
        //    //try
        //    //{
        //    //    result = new InformacionConsulta
        //    //    {
        //    //        Descripcion = "Descripcion",
        //    //        Habilitado = true,
        //    //        IdTipoInfConsulta = (int)BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido,
        //    //        IdUsuarioAlta = ((Usuario)Session["UserData"]).Id,
        //    //        InformacionConsultaDatos = new List<InformacionConsultaDatos>(),
        //    //        InformacionConsultaDocumentos = new List<InformacionConsultaDocumentos>()
        //    //    };

        //    //    InformacionConsultaDatos datos = new InformacionConsultaDatos();
        //    //    datos.Datos = txtEditor.Text;
        //    //    datos.Busqueda = "busqueda";
        //    //    datos.Tags = "tags";
        //    //    datos.Habilitado = true;
        //    //    result.InformacionConsultaDatos.Add(datos);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw new Exception(ex.Message);
        //    //}
        //    //return result;
        //}
        protected void OnClick(object sender, EventArgs e)
        {

            //Session["PreviewDataConsulta"] = ObtenerInformacionCapturada();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "window.open('/Publico/Consultas/FrmPreviewConsulta.aspx','_blank');", true);
        }

        protected void btnTest_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ucTicketPortal.Limpiar();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptClose", "CierraPopup(\"#modal-new-ticket\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                //Alerta = _lstError;
            }
        }

        protected void btnCerrarExito_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ucTicketPortal.Limpiar();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptOpen", "MostrarPopup(\"#modalExitoTicket\");", true);
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

        protected void OnClick1(object sender, EventArgs e)
        {
            try
            {
                //btnPrueba.Enabled = false;
                Thread.Sleep(1000);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}