using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using Telerik.Web.UI;
using Telerik.Web.UI.HtmlChart;
using Telerik.Web.UI.HtmlChart.Enums;
using Label = System.Web.UI.WebControls.Label;

namespace KiiniHelp.Funciones
{
    public static class GeneradorGraficos
    {
        private const bool TitleVisible = false;
        private static readonly string FontFamily = ConfigurationManager.AppSettings["TipografiaFuente"];
        private const int Fontsize = 10;
        private const ChartLegendPosition LegendPosition = ChartLegendPosition.Bottom;
        private const PieAndDonutLabelsPosition DonutLabelsPosition = PieAndDonutLabelsPosition.Center;
        private const Gradients GradientsSerie = Gradients.None;

        private static void SetPerido(Label lblPeriodo, int idTipoPerioro, Dictionary<string, DateTime> rangoFechas)
        {
            try
            {
                switch (idTipoPerioro)
                {
                    case 1:
                        lblPeriodo.Text = string.Format("Diario: {0} - {1}",
                            rangoFechas == null ? "Historico" : rangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            rangoFechas == null ? string.Empty : rangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 2:
                        lblPeriodo.Text = string.Format("Semanal: {0} - {1}",
                            rangoFechas == null ? "Historico" : rangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            rangoFechas == null ? string.Empty : rangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                    case 3:
                        lblPeriodo.Text = string.Format("Mensual: {0} - {1}",
                            rangoFechas == null ? "Historico" : rangoFechas.Single(s => s.Key == "inicio").Value.ToString("MMM"),
                            rangoFechas == null ? string.Empty : rangoFechas.Single(s => s.Key == "fin").Value.ToString("MMM"));
                        break;
                    case 4:
                        lblPeriodo.Text = string.Format("Anual: {0} - {1}",
                            rangoFechas == null ? "Historico" : rangoFechas.Single(s => s.Key == "inicio").Value.ToShortDateString(),
                            rangoFechas == null ? string.Empty : rangoFechas.Single(s => s.Key == "fin").Value.ToShortDateString());
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void SetEstiloFuente(RadHtmlChart grafico)
        {
            try
            {
                grafico.ChartTitle.Appearance.Visible = TitleVisible;

                grafico.Legend.Appearance.Position = LegendPosition;
                grafico.Legend.Appearance.TextStyle.FontFamily = FontFamily;
                grafico.Legend.Appearance.TextStyle.FontSize = Fontsize;

                grafico.PlotArea.XAxis.MajorGridLines.Width = 0;
                grafico.PlotArea.XAxis.MinorGridLines.Width = 0;
                grafico.PlotArea.YAxis.MajorGridLines.Width = 0;
                grafico.PlotArea.YAxis.MinorGridLines.Width = 0;

                grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontFamily = FontFamily;
                grafico.PlotArea.YAxis.LabelsAppearance.TextStyle.FontSize = Fontsize;

                if (grafico.PlotArea.AdditionalYAxes.Count > 0)
                {
                    grafico.PlotArea.AdditionalYAxes[0].LabelsAppearance.TextStyle.FontFamily = FontFamily;
                    grafico.PlotArea.AdditionalYAxes[0].LabelsAppearance.TextStyle.FontSize = Fontsize;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private class ParetoArry
        {
            public int Id { get; set; }
            public string Color { get; set; }
            public string Descripcion { get; set; }
            public decimal Total { get; set; }
            public decimal Acumulado { get; set; }
        }

        public static class GraficosPareto
        {


            private static DataTable ConvertToDataTable<T>(IList<T> data)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                return table;

            }

            private static DataTable GeneraDatosPareto(DataTable dt)
            {
                DataTable result = null;
                try
                {
                    List<ParetoArry> lstPareto = (dt.Rows.Cast<DataRow>().Select(dataRow => new ParetoArry
                    {
                        Id = int.Parse(dataRow[0].ToString()),
                        Descripcion = dataRow[1].ToString(),
                        Color = dataRow[2].ToString()
                    })).ToList();

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        decimal total = 0;
                        for (int i = 3; i < dt.Columns.Count; i++)
                        {
                            total += decimal.Parse(dt.Rows[row][i].ToString());
                        }
                        if (lstPareto.Any(s => s.Id == int.Parse(dt.Rows[row][0].ToString())))
                            lstPareto.Single(s => s.Id == int.Parse(dt.Rows[row][0].ToString())).Total = total;
                    }

                    lstPareto = lstPareto.OrderByDescending(o => o.Total).ToList();
                    decimal acumuladoAnterior = 0;
                    var sumaTotales = lstPareto.Sum(s => s.Total);

                    foreach (ParetoArry data in lstPareto)
                    {
                        data.Acumulado = (data.Total / sumaTotales) + acumuladoAnterior;
                        acumuladoAnterior = data.Acumulado;
                    }

                    result = ConvertToDataTable(lstPareto);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
            private static void ColumnsSeriePareto(RadHtmlChart grafico, string fieldData, string fieldColor, string clientTemplateTooltip, string stack)
            {
                try
                {
                    ColumnSeries donutSerie = new ColumnSeries { Name = stack, Stacked = false };
                    donutSerie.Appearance.Overlay.Gradient = Gradients.None;
                    donutSerie.DataFieldY = fieldData;
                    donutSerie.ColorField = fieldColor;
                    donutSerie.TooltipsAppearance.ClientTemplate = clientTemplateTooltip;
                    donutSerie.LabelsAppearance.Visible = true;
                    donutSerie.LabelsAppearance.Position = BarColumnLabelsPosition.Center;
                    grafico.PlotArea.Series.Add(donutSerie);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            private static void LinesSeriePareto(RadHtmlChart grafico, string aditionalAxisName, string dataField, string colorField, string dataFormatLabel, string dataFormatTooltip)
            {
                try
                {
                    grafico.PlotArea.AdditionalYAxes.Clear();
                    grafico.PlotArea.AdditionalYAxes.Add(new AxisY { Name = aditionalAxisName });
                    grafico.PlotArea.AdditionalYAxes[0].MaxValue = 1;
                    grafico.PlotArea.AdditionalYAxes[0].LabelsAppearance.DataFormatString = dataFormatLabel;
                    grafico.PlotArea.AdditionalYAxes[0].Step = 1;
                    LineSeries lineSerie = new LineSeries { Name = "Pareto" };
                    lineSerie.Appearance.Overlay.Gradient = Gradients.None;
                    lineSerie.DataFieldY = dataField;
                    lineSerie.ColorField = colorField;
                    lineSerie.LabelsAppearance.DataFormatString = dataFormatLabel;
                    lineSerie.TooltipsAppearance.DataFormatString = dataFormatTooltip;
                    lineSerie.LabelsAppearance.Visible = true;
                    lineSerie.LabelsAppearance.Position = LineAndScatterLabelsPosition.Below;
                    lineSerie.AxisName = aditionalAxisName;
                    grafico.PlotArea.Series.Add(lineSerie);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            public static void GraficaPareto(RadHtmlChart grafico, Label lblPeriodo, DataTable dt, int idTipoPerioro, Dictionary<string, DateTime> rangoFechas, string stack)
            {
                try
                {
                    grafico.PlotArea.Series.Clear();
                    SetPerido(lblPeriodo, idTipoPerioro, rangoFechas);
                    if (dt != null)
                    {
                        dt = GeneraDatosPareto(dt);
                        ColumnsSeriePareto(grafico, "Total", "Color", "#= dataItem.Descripcion#: #= dataItem.Total#", stack);
                        LinesSeriePareto(grafico, "Lines", "Acumulado", "Color", "{0:P0}", "{0:P}");
                    }
                    SetEstiloFuente(grafico);
                    grafico.DataSource = dt;
                    grafico.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static class GraficoDona
        {
            private static List<ParetoArry> GeneraDatos(DataTable dt)
            {
                List<ParetoArry> result = null;
                try
                {
                    if (dt != null)
                    {
                        result = (dt.Rows.Cast<DataRow>().Select(dataRow => new ParetoArry { Id = int.Parse(dataRow[0].ToString()), Descripcion = dataRow[1].ToString(), Color = dataRow[2].ToString() })).ToList();
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            decimal total = 0;
                            for (int i = 3; i < dt.Columns.Count; i++)
                            {
                                total += decimal.Parse(dt.Rows[row][i].ToString());
                            }
                            result.Single(s => s.Id == int.Parse(dt.Rows[row][0].ToString())).Total = total;
                        }
                        result = result.OrderByDescending(o => o.Total).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            private static void DonutSerie(RadHtmlChart grafico, string explodeField, string dataField, string nameField, string colorField, string dataFormatTooltip)
            {
                try
                {
                    DonutSeries donutSerie = new DonutSeries();
                    donutSerie.Appearance.Overlay.Gradient = GradientsSerie;
                    donutSerie.ExplodeField = explodeField;
                    donutSerie.DataFieldY = dataField;
                    donutSerie.NameField = nameField;
                    donutSerie.ColorField = colorField;
                    donutSerie.LabelsAppearance.Visible = true;
                    donutSerie.LabelsAppearance.Position = DonutLabelsPosition;
                    grafico.PlotArea.Series.Add(donutSerie);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            public static void GeneraGraficaStackedPie(RadHtmlChart grafico, Label lblPeriodo, DataTable dt, int idTipoPerioro, Dictionary<string, DateTime> rangoFechas, Label lblTotal)
            {
                try
                {
                    grafico.PlotArea.Series.Clear();
                    SetPerido(lblPeriodo, idTipoPerioro, rangoFechas);
                    if (dt != null)
                    {
                        DonutSerie(grafico, "false", "Total", "Descripcion", "Color", "#= dataItem.Descripcion#: #= dataItem.Total#");
                    }
                    SetEstiloFuente(grafico);
                    List<ParetoArry> datos = GeneraDatos(dt);
                    grafico.DataSource = datos;
                    grafico.DataBind();
                    lblTotal.Text = datos == null ? "0" : datos.Sum(s => s.Total).ToString();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static class GraficoStack
        {
            private static DataTable GeneraDatos(DataTable dt)
            {
                DataTable result = null;
                try
                {
                    dt.Columns.Add(new DataColumn("Totales", typeof(int)));

                    foreach (DataRow fila in dt.Rows)
                    {
                        int suma = 0;
                        for (int i = 3; i < dt.Columns.Count - 2; i++)
                        {
                            suma += int.Parse(fila[i].ToString());
                        }
                        fila[dt.Columns.Count - 1] = suma;
                    }
                    DataView dv = dt.DefaultView;
                    dv.Sort = "Totales desc";
                    result = dv.ToTable();
                    result.Columns.Remove("Totales");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
            private static void ColumnStackedSerie(RadHtmlChart grafico, DataTable dt)
            {
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ColumnSeries column = new ColumnSeries();
                        column.Appearance.Overlay.Gradient = Gradients.None;
                        column.Name = row[1].ToString();
                        column.Appearance.FillStyle.BackgroundColor = ColorTranslator.FromHtml(row[2].ToString());
                        column.GroupName = "Stacked";
                        column.Stacked = true;

                        column.TooltipsAppearance.ClientTemplate = "#= series.name#: #= dataItem.value#";
                        column.LabelsAppearance.Visible = false;
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            column.SeriesItems.Add(int.Parse(row[c].ToString()));
                        }
                        grafico.PlotArea.Series.Add(column);
                    }
                    for (int c = 3; c < dt.Columns.Count; c++)
                    {
                        grafico.PlotArea.XAxis.Items.Add(dt.Columns[c].ColumnName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            public static void GeneraGraficaStackedColumn(RadHtmlChart grafico, Label lblPeriodo, DataTable dt, int idTipoPerioro, Dictionary<string, DateTime> rangoFechas)
            {
                try
                {
                    grafico.PlotArea.Series.Clear();
                    grafico.PlotArea.XAxis.Items.Clear();
                    SetPerido(lblPeriodo, idTipoPerioro, rangoFechas);

                    if (dt != null)
                    {
                        dt = GeneraDatos(dt);
                        ColumnStackedSerie(grafico, dt);
                    }
                    grafico.PlotArea.XAxis.LabelsAppearance.RotationAngle = -45;
                    SetEstiloFuente(grafico);

                    grafico.DataSource = dt;
                    grafico.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}