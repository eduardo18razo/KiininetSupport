using System;

namespace KiiniHelp.Users.Graficos
{
    public partial class FrmGeoChart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Params["Data"] != null)
                {
                    Literal1.Text = @"<script type='text/javascript'>
                        google.load('visualization', '1.0', { 'packages': ['geochart'] });
                        google.setOnLoadCallback(drawChart);
            
                        function drawChart() {


                            // Create the data table.
                            var data = google.visualization.arrayToDataTable(" + Request.Params["Data"] + @")

                            // Set chart options
                            var options = { 
                                'title': 'Stats of my blog',
                                'width': 800,
                                'region': 'MX',
                                'height': 600,
                                'legend': 'none',
                                'resolution': 'provinces',
                                'magnifyingGlass': {enable: true, zoomFactor: 10.5},
                                'colorAxis': {colors: ['red', 'red']},
                                'backgroundColor': '#AAd7ff',
                                'datalessRegionColor': 'green',
                                'defaultColor': '#0B610B',
                                'legend':{textStyle: {color: 'navy', fontSize: 12}},                   
                            };

                            // Instantiate and draw our chart, passing in some options.
                            var chart = new 
                            google.visualization.GeoChart(document.getElementById('chart_container'));

                            google.visualization.events.addListener(chart,
                            'regionClick',function(eventOption){
                                    getValue(eventOption.region);
                                    alert('Region : ' + eventOption.region);
                                    }); 
                            chart.draw(data, options);
                        }
                        </script>";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnSelectRegion_OnClick(object sender, EventArgs e)
        {
            try
            {
                //var test = new FrmGraficaHits().Test(hfRegion.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}