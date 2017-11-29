using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;

namespace KinniNet.Business.Utils
{
    public static class BusinessGraficoStack
    {
        public static class Pareto
        {
            private class ParetoArry
            {
                public int Id { get; set; }
                public string Descripcion { get; set; }
                public decimal Total { get; set; }
                public decimal Acumulado { get; set; }
            }
            public static void GenerarGrafica(Chart grafico, DataTable dts, string stack)
            {
                List<ParetoArry> lstPareto = (dts.Rows.Cast<DataRow>().Select(dataRow => new ParetoArry { Id = int.Parse(dataRow[0].ToString()), Descripcion = dataRow[1].ToString() })).ToList();

                for (int row = 0; row < dts.Rows.Count; row++)
                {
                    decimal total = 0;
                    for (int i = 2; i < dts.Columns.Count; i++)
                    {
                        total += decimal.Parse(dts.Rows[row][i].ToString());
                    }
                    lstPareto.Single(s => s.Descripcion == dts.Rows[row][1].ToString()).Total = total;
                }

                lstPareto = lstPareto.OrderByDescending(o => o.Total).ToList();
                decimal acumuladoAnterior = 0;
                foreach (ParetoArry data in lstPareto)
                {
                    data.Acumulado = ((data.Total / lstPareto.Sum(s => s.Total)) * 100) + acumuladoAnterior;
                    acumuladoAnterior += data.Acumulado;
                }

                DataTable dtParateo = ConvertToDataTable(lstPareto);
                AfterLoad(grafico, dtParateo, stack);


            }

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

            private static void MakeBarChart(Chart grafico, DataTable dt, string stack)
            {
                //foreach (DataRow row in dt.Rows)
                //{
                //    grafico.Series[row[1].ToString()].Label = "#VALY{#.##}";
                //    for (int point = 0; point < dt.Rows.Count; point++)
                //    {
                //        grafico.Series[stack].Points.Add(new DataPoint { PostBackValue = "#VALX,#VALY," + row[1].ToString(), LabelPostBackValue = "label", LegendPostBackValue = "legendVALX";, AxisLabel = row[1].ToString(), YValues = new double[] { Convert.ToDouble(row[2].ToString()) } });
                //    } 
                //}
                grafico.Series[stack].Label = "#VALY{#.##}";
                grafico.Series[stack].ToolTip = "#SERIESNAME";
                for (int point = 0; point < dt.Rows.Count; point++)
                {
                    grafico.Series[stack].Points.Add(new DataPoint
                    {
                        PostBackValue = dt.Rows[point][0].ToString() +",#VALY," + dt.Rows[point][0].ToString().ToString(),
                        LabelPostBackValue = "label",
                        LegendPostBackValue = "legendVALX",
                        AxisLabel = dt.Rows[point][1].ToString(),
                        YValues = new double[] { Convert.ToDouble(dt.Rows[point][2].ToString()) }
                    });
                }
                foreach (Series serie in grafico.Series)
                {
                    serie.Font = new Font("Tahoma", 8, FontStyle.Bold);
                    //foreach (DataPoint dp in serie.Points)
                    //{
                    //    dp.PostBackValue = "#VALX,#VALY," + dt.Select("Descripcion = '" + serie.Name + "'").First()[0]; 
                    //    dp.LabelPostBackValue = "label";
                    //    dp.LegendPostBackValue = "legendVALX";
                    //}
                }

            }

            private static void AfterLoad(Chart grafico, DataTable dt, string stack)
            {

                //foreach (DataRow row in dt.Rows)
                //{
                //    grafico.Series.Add(row[1].ToString());  
                //}
                grafico.Series.Add(stack);
                MakeBarChart(grafico, dt, stack);
                MakeParetoChart(grafico, stack, "Pareto");
                grafico.Series["Pareto"].ChartType = SeriesChartType.Line;
                grafico.Series["Pareto"].IsValueShownAsLabel = true;
                grafico.Series["Pareto"].MarkerColor = Color.Red;
                grafico.Series["Pareto"].MarkerBorderColor = Color.MidnightBlue;
                grafico.Series["Pareto"].MarkerStyle = MarkerStyle.Circle;
                grafico.Series["Pareto"].MarkerSize = 8;
                grafico.Series["Pareto"].LabelFormat = "0.#";  // format with one decimal and leading zero
                grafico.Series["Pareto"].Color = Color.FromArgb(252, 180, 65);
                grafico.Legends[0].Title = stack;

            }

            private static void MakeParetoChart(Chart chart, string srcSeriesName, string destSeriesName)
            {
                string strChartArea = chart.Series[srcSeriesName].ChartArea;
                chart.Series[srcSeriesName].ChartType = SeriesChartType.Column;
                chart.DataManipulator.Sort(PointSortOrder.Descending, srcSeriesName);
                double total = 0.0;

                foreach (DataPoint pt in chart.Series[srcSeriesName].Points)
                    total += pt.YValues[0];

                chart.ChartAreas[strChartArea].AxisY.Maximum = total;
                Series destSeries = new Series(destSeriesName);
                destSeries.ToolTip = "#SERIESNAME";
                chart.Series.Add(destSeries);
                destSeries.ChartType = SeriesChartType.Line;

                destSeries.BorderWidth = 3;
                destSeries.ChartArea = chart.Series[srcSeriesName].ChartArea;
                destSeries.YAxisType = AxisType.Secondary;
                chart.ChartAreas[strChartArea].AxisY2.Maximum = 100;
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
        }

        public static class Stack
        {
            public static void GenerarGrafica(Chart grafico, DataTable dt, string stack)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    grafico.Series.Add(dataRow[1].ToString());
                }

                string[] x = new string[dt.Columns.Count - 2];
                int[] y = new int[dt.Columns.Count - 2];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 2; j < dt.Columns.Count; j++)
                    {
                        x[j - 2] = dt.Columns[j].ColumnName.ToString();
                        y[j - 2] = Convert.ToInt32(dt.Rows[i][j]);

                    }
                    grafico.Series[dt.Rows[i][1].ToString()].Label = "#VALY{#.##}"; //"#PERCENT{P0}";
                    grafico.Series[dt.Rows[i][1].ToString()].ToolTip = "#SERIESNAME";
                    grafico.Series[dt.Rows[i][1].ToString()].Points.DataBindXY(x, y);
                    grafico.Series[dt.Rows[i][1].ToString()].ChartType = SeriesChartType.StackedColumn;
                }

                foreach (Series serie in grafico.Series)
                {
                    serie.Font = new Font("Tahoma", 8, FontStyle.Bold);
                    foreach (DataPoint dp in serie.Points)
                    {
                        dp.PostBackValue = "#VALX,#VALY," + dt.Select("Descripcion = '" + serie.Name + "'").First()[0];
                        dp.LabelPostBackValue = "label";
                        dp.LegendPostBackValue = "legendVALX";
                    }
                }

                grafico.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                grafico.ChartAreas["ChartArea1"].AxisX.Enabled = AxisEnabled.True;
                grafico.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.False;
                //grafico.Legends[0].Enabled = true;
                grafico.ChartAreas[0].AxisX.Enabled = AxisEnabled.Auto;
                grafico.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.Auto;
                grafico.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;

                grafico.Legends[0].Title = stack;
            }
        }

        public static class Encuestas
        {
            public static class Columns
            {
                public static void GenerarGrafica(Chart grafico, DataTable dt)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        grafico.Series.Add(dataRow[1].ToString());
                    }
                    string[] x = new string[dt.Columns.Count - 2];
                    decimal[] y = new decimal[dt.Columns.Count - 2];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 2; j < dt.Columns.Count; j++)
                        {
                            x[j - 2] = dt.Columns[j].ColumnName.ToString();
                            y[j - 2] = Convert.ToDecimal(dt.Rows[i][j]);

                        }
                        grafico.Series[dt.Rows[i][1].ToString()].Label = "#VALY{#.##}"; //"#PERCENT{P0}";
                        grafico.Series[dt.Rows[i][1].ToString()].ToolTip = "#SERIESNAME";
                        grafico.Series[dt.Rows[i][1].ToString()].Points.DataBindXY(x, y);
                        grafico.Series[dt.Rows[i][1].ToString()].ChartType = SeriesChartType.Column;
                    }

                    foreach (Series serie in grafico.Series)
                    {
                        serie.Font = new Font("Tahoma", 8, System.Drawing.FontStyle.Bold);
                        foreach (DataPoint dp in serie.Points)
                        {
                            dp.PostBackValue = "#VALX,#VALY," + dt.Select("Descripcion = '" + serie.Name + "'").First()[0] + "," + dt.Select("Descripcion = '" + serie.Name + "'").First()[1];
                            dp.LabelPostBackValue = "label";
                            dp.LegendPostBackValue = "legendVALX";
                        }
                    }

                    grafico.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                    grafico.ChartAreas["ChartArea1"].AxisX.Enabled = AxisEnabled.True;
                    grafico.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.False;
                    grafico.Legends[0].Enabled = true;
                    grafico.ChartAreas[0].AxisX.Enabled = AxisEnabled.Auto;
                    grafico.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.Auto;
                    grafico.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
                }
            }

            public static class Linear
            {
                public static void GenerarGrafica(Chart grafico, DataTable dt)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        grafico.Series.Add(dataRow[1].ToString());
                    }
                    string[] x = new string[dt.Columns.Count - 2];
                    decimal[] y = new decimal[dt.Columns.Count - 2];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 2; j < dt.Columns.Count; j++)
                        {
                            x[j - 2] = dt.Columns[j].ColumnName.ToString();
                            y[j - 2] = Convert.ToDecimal(dt.Rows[i][j]);
                        }
                        grafico.Series[dt.Rows[i][1].ToString()].Label = "#VALY{#.##}"; //"#PERCENT{P0}";
                        grafico.Series[dt.Rows[i][1].ToString()].ToolTip = "#SERIESNAME";
                        grafico.Series[dt.Rows[i][1].ToString()].Points.DataBindXY(x, y);
                        grafico.Series[dt.Rows[i][1].ToString()].ChartType = SeriesChartType.StackedColumn100;
                    }

                    foreach (Series serie in grafico.Series)
                    {
                        serie.Font = new Font("Tahoma", 8, FontStyle.Bold);
                        foreach (DataPoint dp in serie.Points)
                        {
                            dp.PostBackValue = "#VALX,#VALY," + dt.Select("Descripcion = '" + serie.Name + "'").First()[0] + "," + dt.Select("Descripcion = '" + serie.Name + "'").First()[1];
                            dp.LabelPostBackValue = "#SERIE";
                            dp.LegendPostBackValue = "legendVALX";
                        }
                    }

                    grafico.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                    grafico.ChartAreas["ChartArea1"].AxisX.Enabled = AxisEnabled.True;
                    grafico.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.False;
                    grafico.Legends[0].Enabled = true;
                    grafico.ChartAreas[0].AxisX.Enabled = AxisEnabled.Auto;
                    grafico.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.Auto;
                    grafico.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
                    grafico.Legends[0].Title = dt.TableName;
                }
            }
        }

        public static class ColumnsClustered
        {
            public static void GenerarGrafica(Chart grafico, DataTable dt, string stack)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    grafico.Series.Add(dataRow[1].ToString());
                }
                string[] x = new string[dt.Columns.Count - 2];
                int[] y = new int[dt.Columns.Count - 2];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 2; j < dt.Columns.Count; j++)
                    {
                        x[j - 2] = dt.Columns[j].ColumnName.ToString();
                        y[j - 2] = Convert.ToInt32(dt.Rows[i][j]);

                    }
                    grafico.Series[dt.Rows[i][1].ToString()].Label = "#VALY{#.##}"; //"#PERCENT{P0}";
                    grafico.Series[dt.Rows[i][1].ToString()].ToolTip = "#SERIESNAME";
                    grafico.Series[dt.Rows[i][1].ToString()].Points.DataBindXY(x, y);
                    grafico.Series[dt.Rows[i][1].ToString()].ChartType = SeriesChartType.Column;
                }

                foreach (Series serie in grafico.Series)
                {
                    serie.Font = new Font("Tahoma", 8, FontStyle.Bold);
                    foreach (DataPoint dp in serie.Points)
                    {
                        dp.PostBackValue = "#VALX,#VALY," + dt.Select("Descripcion = '" + serie.Name + "'").First()[0];
                        dp.LabelPostBackValue = "label";
                        dp.LegendPostBackValue = "legendVALX";
                    }
                }

                grafico.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                grafico.ChartAreas["ChartArea1"].AxisX.Enabled = AxisEnabled.True;
                grafico.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.False;
                grafico.Legends[0].Enabled = true;
                grafico.ChartAreas[0].AxisX.Enabled = AxisEnabled.Auto;
                grafico.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.Auto;
                grafico.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;

                grafico.Legends[0].Title = stack;

            }
        }

        public static class Geografico
        {
            private static string[] DtToArray(DataTable dt)
            {
                string[] result;
                try
                {
                    result = dt.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
            public static string GenerarGraficoHits(DataTable dt)
            {
                string result = null;
                try
                {
                    result = @"<script type='text/javascript'>
            google.load('visualization', '1.0', { 'packages': ['geochart'] });
            google.setOnLoadCallback(drawChart);
            
            function drawChart() {


                // Create the data table.
                var data = google.visualization.arrayToDataTable(" + DtToArray(dt) + @")

                // Set chart options
                var options = { 
                    'title': 'Stats of my blog',
                    'width': 800,
                    'region': 'MX',
                    'height': 600,
                    'legend': 'none',
                    'resolution': 'provinces',
                    'magnifyingGlass': {enable: true, zoomFactor: 10.5},
                    'colorAxis': {colors: ['green', 'blue']},
                    'legend':{textStyle: {color: 'navy', fontSize: 12}},
                    
                };

                // Instantiate and draw our chart, passing in some options.
                var chart = new 
                google.visualization.GeoChart(document.getElementById('chart_container'));

                google.visualization.events.addListener(chart,
                'regionClick',function(eventOption){
                        alert('Region : ' + eventOption.region);
                      }); 
                chart.draw(data, options);
            }
        </script>";
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
