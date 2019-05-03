<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTest.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Test.FrmTest" Culture="Auto" UICulture="Auto" %>

<%@ Register TagPrefix="ajx" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=18.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Test</title>
</head>
<body>
    <div>
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <tc:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="800" Height="500">
                <plotarea>
                <Series>
                    <tc:ColumnSeries Name="Estatus">
                        <TooltipsAppearance Color="White" />
                        <SeriesItems>
                            <tc:CategorySeriesItem Y="1000" />
                            <tc:CategorySeriesItem Y="2000" />
                            <tc:CategorySeriesItem Y="2500" />
                            <tc:CategorySeriesItem Y="1750" />
                        </SeriesItems>
                    </tc:ColumnSeries>
                    <tc:ColumnSeries AxisName="AdditionalAxis" Name="Outstanding Cases">
                        <Appearance>
                            <FillStyle BackgroundColor="Red" />
                        </Appearance>
                        <TooltipsAppearance Color="White" />
                        <SeriesItems>
                            <tc:CategorySeriesItem Y="200" />
                            <tc:CategorySeriesItem Y="230" />
                            <tc:CategorySeriesItem Y="170" />
                            <tc:CategorySeriesItem Y="190" />
                        </SeriesItems>
                    </tc:ColumnSeries>
                </Series>
                <YAxis Color="#2DABC1" Width="3">
                </YAxis>
                <AdditionalYAxes>
                    <tc:AxisY Name="AdditionalAxis" Color="blue" Width="3">
                    </tc:AxisY>
                </AdditionalYAxes>
                <XAxis>
                    <AxisCrossingPoints>
                        <tc:AxisCrossingPoint Value="0" />
                        <tc:AxisCrossingPoint Value="4" />
                    </AxisCrossingPoints>
                    <LabelsAppearance DataFormatString="{0}" />
                    <Items>
                        <tc:AxisItem LabelText="Abierto" />
                        <tc:AxisItem LabelText="Cerrado" />
                        <tc:AxisItem LabelText="3" />
                        <tc:AxisItem LabelText="4" />
                    </Items>
                </XAxis>
            </plotarea>
                <legend>
                    <appearance position="Top" />
                </legend>
            </tc:RadHtmlChart>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="uppanel">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>

        </form>
    </div>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTest.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Test.FrmTest" Culture="Auto" UICulture="Auto" %>

<%@ Register TagPrefix="ajx" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=18.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Test</title>
</head>
<body>
    <div>
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <tc:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="800" Height="500">
                <plotarea>
                <Series>
                    <tc:ColumnSeries Name="Estatus">
                        <TooltipsAppearance Color="White" />
                        <SeriesItems>
                            <tc:CategorySeriesItem Y="1000" />
                            <tc:CategorySeriesItem Y="2000" />
                            <tc:CategorySeriesItem Y="2500" />
                            <tc:CategorySeriesItem Y="1750" />
                        </SeriesItems>
                    </tc:ColumnSeries>
                    <tc:ColumnSeries AxisName="AdditionalAxis" Name="Outstanding Cases">
                        <Appearance>
                            <FillStyle BackgroundColor="Red" />
                        </Appearance>
                        <TooltipsAppearance Color="White" />
                        <SeriesItems>
                            <tc:CategorySeriesItem Y="200" />
                            <tc:CategorySeriesItem Y="230" />
                            <tc:CategorySeriesItem Y="170" />
                            <tc:CategorySeriesItem Y="190" />
                        </SeriesItems>
                    </tc:ColumnSeries>
                </Series>
                <YAxis Color="#2DABC1" Width="3">
                </YAxis>
                <AdditionalYAxes>
                    <tc:AxisY Name="AdditionalAxis" Color="blue" Width="3">
                    </tc:AxisY>
                </AdditionalYAxes>
                <XAxis>
                    <AxisCrossingPoints>
                        <tc:AxisCrossingPoint Value="0" />
                        <tc:AxisCrossingPoint Value="4" />
                    </AxisCrossingPoints>
                    <LabelsAppearance DataFormatString="{0}" />
                    <Items>
                        <tc:AxisItem LabelText="Abierto" />
                        <tc:AxisItem LabelText="Cerrado" />
                        <tc:AxisItem LabelText="3" />
                        <tc:AxisItem LabelText="4" />
                    </Items>
                </XAxis>
            </plotarea>
                <legend>
                    <appearance position="Top" />
                </legend>
            </tc:RadHtmlChart>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="uppanel">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>

        </form>
    </div>
</body>
</html>
