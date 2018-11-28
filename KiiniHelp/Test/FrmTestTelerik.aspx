<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTestTelerik.aspx.cs" Inherits="KiiniHelp.Test.FrmTestTelerik" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>




<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <<meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="cache-control" content="max-age=0" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="pragma" content="no-cache" />
    <title></title>
    <link rel="Shortcut Icon" href="../favicon.ico" type="image/x-icon" />

    <%----%><link rel='stylesheet' href="../assets/css/font.css" />
    <link rel="stylesheet" href="../assets/css/font-awesome.css" />
    <link rel="stylesheet" href="../assets/css/pe-7-icons.css" />

    <link rel="stylesheet" href="../assets/css/bootstrap.css" />
    <link rel="stylesheet" href="../assets/css/styles.css" />
    <link rel="stylesheet" href="../assets/css/medias.css" />
    <link rel="stylesheet" href="../assets/css/menuStyle.css" />
    <link rel="stylesheet" href="../assets/css/divs.css" />
    <link rel="stylesheet" href="../assets/css/checkBox.css" />
    <link rel="stylesheet" href="../assets/tmp/chosen.css" />
    <link rel="stylesheet" href="../assets/css/sumoselect.css" />
    <link rel="stylesheet" href="../assets/tmp/editor/ui/trumbowyg.css" />
    <link rel="stylesheet" href="../assets/tmp/editor/ui/trumbowyg.min.css" />
    <link rel="stylesheet" href="../assets/css/fileupload.css" />
    <link rel="stylesheet" href="../assets/tmp/jquery.tagsinput.min.css" />
    <link rel="stylesheet" href="../assets/css/elusive-icons.css" />
    <link rel="stylesheet" href="../assets/css/telerikControl.css" />

    <script type="text/javascript">
        (function (global) {
            var chart;

            function chartLoad(sender, args) {
                chart = sender.get_kendoWidget(); //store a reference to the Kendo Chart widget, we will use its methods
            }

            global.chartLoad = chartLoad;

            function resizeChart() {
                if (chart)
                    chart.resize(); //redraw the chart so it takes the new size of its container when it changes (e.g., browser window size change, parent container size change)
            }


            //this logic ensures that the chart resizing will happen only once, at most - every 200ms
            //to prevent calling the handler too often if old browsers fire the window.onresize event multiple times
            var TO = false;
            window.onresize = function () {
                if (TO !== false)
                    clearTimeout(TO);
                TO = setTimeout(resizeChart, 200);
            }

        })(window);
    </script>
</head>
<body>


    <div id="chart_div" style="width: 900px; height: 500px;"></div>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/assets/js/jquery.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
                <asp:ScriptReference Path="~/assets/js/imagesloaded.js" />
                <asp:ScriptReference Path="~/assets/js/masonry.js" />
                <asp:ScriptReference Path="~/assets/js/main.js" />
                <asp:ScriptReference Path="~/assets/js/modernizr.custom.js" />
                <asp:ScriptReference Path="~/assets/js/pmenu.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.min.js" />
                <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                <asp:ScriptReference Path="~/assets/tmp/chosen.jquery.js" />
                <asp:ScriptReference Path="~/assets/tmp/editor/trumbowyg.min.js" />
                <asp:ScriptReference Path="~/assets/tmp/editor/plugins/base64/trumbowyg.base64.min.js" />
                <asp:ScriptReference Path="~/assets/tmp/editor/plugins/upload/trumbowyg.upload.min.js" />
                <asp:ScriptReference Path="~/assets/tmp/jquery.tagsinput.min.js" />
                <asp:ScriptReference Path="~/assets/js/jquery.sumoselect.min.js" />
                <asp:ScriptReference Path="~/assets/js/validation.js" />
                <asp:ScriptReference Path="~/assets/js/functions.js" />
                <asp:ScriptReference Path="https://code.jquery.com/ui/1.12.1/jquery-ui.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:Label runat="server" ID="lblMaquina"></asp:Label>
        <tc:RadDropDownTree runat="server" ID="RadDropDownTree1"></tc:RadDropDownTree>


        <tc:RadComboBox RenderMode="Lightweight" ID="RadComboBoxProduct" runat="server" DropDownWidth="315" CssClass="form-control"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
            <HeaderTemplate>
                <table style="width: 275px" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="width: 175px;">Product Name
                        </td>
                        <td style="width: 60px;">Quantity
                        </td>
                        <td style="width: 40px;">Price
                        </td>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table style="width: 275px" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="width: 175px;">
                            <%# DataBinder.Eval(Container, "Text")%>
                        </td>
                        <td style="width: 60px;">
                            <%# DataBinder.Eval(Container, "Attributes['UnitsInStock']")%>
                        </td>
                        <td style="width: 40px;">
                            <%# DataBinder.Eval(Container, "Attributes['UnitPrice']")%>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </tc:RadComboBox>


        <tc:RadHtmlChart runat="server" ID="BarChart" Height="300px" Width="100%">
            <ClientEvents OnLoad="chartLoad" />
            <PlotArea>
                <XAxis>
                    <Items>
                        <tc:AxisItem LabelText="Item 1"></tc:AxisItem>
                        <tc:AxisItem LabelText="Item 2"></tc:AxisItem>
                        <tc:AxisItem LabelText="Item 3"></tc:AxisItem>
                    </Items>
                </XAxis>
                <Series>
                    <tc:BarSeries Name="Series 1">
                        <SeriesItems>
                            <tc:CategorySeriesItem Y="25"></tc:CategorySeriesItem>
                            <tc:CategorySeriesItem Y="-12"></tc:CategorySeriesItem>
                            <tc:CategorySeriesItem Y="39"></tc:CategorySeriesItem>
                        </SeriesItems>
                    </tc:BarSeries>
                    <tc:BarSeries Name="Series 2">
                        <SeriesItems>
                            <tc:CategorySeriesItem Y="-15"></tc:CategorySeriesItem>
                            <tc:CategorySeriesItem Y="38"></tc:CategorySeriesItem>
                            <tc:CategorySeriesItem Y="-11"></tc:CategorySeriesItem>
                        </SeriesItems>
                    </tc:BarSeries>
                </Series>
            </PlotArea>
        </tc:RadHtmlChart>
    </form>
</body>
</html>
