<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTestTelerik.aspx.cs" Inherits="KiiniHelp.Test.FrmTestTelerik" %>

<%@ Register Src="~/UserControls/Consultas/UcBusqueda.ascx" TagPrefix="uc1" TagName="UcBusqueda" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script>
    <script type='text/javascript'>
        google.charts.load('current', {
            'packages': ['geochart'],
            // Note: you will need to get a mapsApiKey for your project.
            // See: https://developers.google.com/chart/interactive/docs/basic_load_libs#load-settings
            'mapsApiKey': 'AIzaSyD-9tSrke72PouQMnMX-a7eZSW0jkFMBWY'
        });
        google.charts.setOnLoadCallback(drawMarkersMap);

        function drawMarkersMap() {
            var data = google.visualization.arrayToDataTable([
              ['City', 'Population', 'Area'],
              ['Puebla', 2761477, 1285.31]
            ]);

            var options = {
                region: 'MX',
                displayMode: 'markers',
                colorAxis: { colors: ['green', 'blue'] }
            };

            var chart = new google.visualization.GeoChart(document.getElementById('chart_div'));
            chart.draw(data, options);
        };
    </script>
</head>
<body>
    

    <div id="chart_div" style="width: 900px; height: 500px;"></div>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <tc:RadDatePicker RenderMode="Lightweight" ID="RadDatePicker1" Width="50%" runat="server" DateInput-Label="From Date" >
                   
        </tc:RadDatePicker>

    <tc:RadMonthYearPicker RenderMode="Lightweight" ID="RadMonthYearPicker1" runat="server" Width="238px">
            </tc:RadMonthYearPicker>
                <uc1:UcBusqueda runat="server" ID="UcBusqueda" />
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
