<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTest.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Test.FrmTest" Culture="Auto" UICulture="Auto" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function OnClientEntryAdding(sender, args) {
            var path = args.get_entry().get_fullPath();
            var s = path.split("/");
            if (s.length == 1) {
                args.set_cancel(true);
            }
            else {
                sender.closeDropDown();
            }
        }
    </script>
</head>
<body>
    <div id="full">
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
                function BeginRequestHandler(sender, args) {
                    var oControl = args.get_postBackElement();
                    oControl.disabled = true;
                }
            </script>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <tc:RadDropDownTree runat="server" CssClass="form-control" ID="ddlUsuarioAsignacion" AutoPostBack="True" RenderMode="Lightweight" ExpandNodeOnSingleClick="true" CheckNodeOnClick="False"
                        DefaultMessage="-" OnEntriesAdded="ddlUsuarioAsignacion_OnEntriesAdded" EnableFiltering="True" OnClientEntryAdding="OnClientEntryAdding" >
                        <DropDownSettings Width="350px" CloseDropDownOnSelection="True" />
                    </tc:RadDropDownTree>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button runat="server" Text="texto" />

                    <tc:RadHtmlChart runat="server" ID="rchArea">
                        <PlotArea>
                            <Series>
                                <tc:AreaSeries Name="Creados">
                                    <Appearance>
                                        <FillStyle BackgroundColor="Blue"></FillStyle>
                                    </Appearance>
                                    <LabelsAppearance Position="Above"></LabelsAppearance>
                                    <LineAppearance Width="1"></LineAppearance>
                                    <MarkersAppearance MarkersType="Circle" BackgroundColor="White" Size="6" BorderColor="Blue"
                                        BorderWidth="2"></MarkersAppearance>
                                    <TooltipsAppearance Color="White"></TooltipsAppearance>
                                    <SeriesItems>
                                        <tc:CategorySeriesItem Y="20"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="30"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="40"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="50"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="60"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="70"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="80"></tc:CategorySeriesItem>
                                    </SeriesItems>
                                </tc:AreaSeries>
                                <tc:AreaSeries Name="Resuletos">
                                    <Appearance>
                                        <FillStyle BackgroundColor="Red"></FillStyle>
                                    </Appearance>
                                    <LabelsAppearance Position="Above"></LabelsAppearance>
                                    <LineAppearance Width="1"></LineAppearance>
                                    <MarkersAppearance MarkersType="Circle" BackgroundColor="White" Size="6" BorderColor="Red"
                                        BorderWidth="2"></MarkersAppearance>
                                    <TooltipsAppearance Color="White"></TooltipsAppearance>
                                    <SeriesItems>
                                        <tc:CategorySeriesItem Y="1"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="10"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="25"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="100"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="10"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="15"></tc:CategorySeriesItem>
                                        <tc:CategorySeriesItem Y="30"></tc:CategorySeriesItem>
                                    </SeriesItems>
                                </tc:AreaSeries>
                            </Series>
                            <Appearance>
                                <FillStyle BackgroundColor="Transparent"></FillStyle>
                            </Appearance>
                            <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside" Reversed="false">
                                <Items>
                                    <tc:AxisItem LabelText="24/01"></tc:AxisItem>
                                    <tc:AxisItem LabelText="25/01"></tc:AxisItem>
                                    <tc:AxisItem LabelText="26/01"></tc:AxisItem>
                                    <tc:AxisItem LabelText="27/01"></tc:AxisItem>
                                    <tc:AxisItem LabelText="28/01"></tc:AxisItem>
                                    <tc:AxisItem LabelText="29/01"></tc:AxisItem>
                                    <tc:AxisItem LabelText="30/01"></tc:AxisItem>

                                </Items>
                                <TitleAppearance Position="Center" RotationAngle="0" Text=""></TitleAppearance>
                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1">
                                </LabelsAppearance>
                            </XAxis>
                            <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="4" MajorTickType="Outside"
                                MaxValue="200" MinorTickType="None" MinValue="0" Reversed="false"
                                Step="20">
                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1">
                                </LabelsAppearance>
                                <TitleAppearance RotationAngle="0" Position="Center" Text=""></TitleAppearance>
                            </YAxis>
                        </PlotArea>
                        <Appearance>
                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                        </Appearance>
                        <ChartTitle Text="Company performance">
                            <Appearance Align="Center" BackgroundColor="Transparent" Position="Top">
                            </Appearance>
                        </ChartTitle>
                        <Legend>
                            <Appearance BackgroundColor="Transparent" Position="Bottom">
                            </Appearance>
                        </Legend>
                    </tc:RadHtmlChart>

                    <tc:RadHtmlChart runat="server" ID="rhcTickets">
                    </tc:RadHtmlChart>
                    <tc:RadHtmlChart runat="server" ID="rhcEspacio">
                    </tc:RadHtmlChart>
                    <tc:RadHtmlChart runat="server" ID="rcTest" Height="250px" Width="100%" Transitions="True">
                        <PlotArea>
                            <XAxis>
                                <MajorGridLines Width="0"></MajorGridLines>
                                <MinorGridLines Width="0"></MinorGridLines>
                            </XAxis>
                            <YAxis>
                                <MajorGridLines Width="0"></MajorGridLines>
                                <MinorGridLines Width="0"></MinorGridLines>
                            </YAxis>
                            <Series>
                                <tc:BarSeries Name="Ocupado" DataFieldY="Ocupado" Stacked="true" StackType="Normal">
                                    <LabelsAppearance Visible="true" Position="Center" DataFormatString="{0} MB"></LabelsAppearance>
                                    <TooltipsAppearance DataFormatString="{0} MB"></TooltipsAppearance>
                                </tc:BarSeries>
                                <tc:BarSeries Name="Libre" DataFieldY="Libre">
                                    <LabelsAppearance Visible="true" Position="Center" DataFormatString="{0} MB"></LabelsAppearance>
                                    <TooltipsAppearance DataFormatString="{0} MB"></TooltipsAppearance>
                                </tc:BarSeries>
                            </Series>
                            <XAxis DataLabelsField="Titulo"></XAxis>
                        </PlotArea>
                        <ChartTitle Text="Stacked Bar Series"></ChartTitle>
                    </tc:RadHtmlChart>





                    <asp:Chart ID="cGraficoEspacio" runat="server" Height="300px" Width="400px" Visible="True" BorderlineColor="Blue">
                        <Series>
                            <asp:Series Name="Ocupado" Label="Ocupado"></asp:Series>
                            <asp:Series Name="Libre" Label="Libre"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderWidth="0" BorderColor="Blue">
                                <AxisX IsMarginVisible="False" LineDashStyle="DashDotDot" IsMarksNextToAxis="True">
                                    <LabelStyle Enabled="False"></LabelStyle>
                                    <MajorTickMark LineColor="Blue" LineWidth="50" LineDashStyle="NotSet" Enabled="False"></MajorTickMark>
                                    <MinorTickMark LineColor="Yellow" LineDashStyle="NotSet"></MinorTickMark>
                                </AxisX>
                                <AxisX2>
                                    <MajorGrid LineColor="Red" LineDashStyle="NotSet"></MajorGrid>
                                </AxisX2>
                            </asp:ChartArea>

                        </ChartAreas>
                    </asp:Chart>

                    <asp:Chart ID="Chart1" runat="server" Height="300px" Width="400px" Visible="True">
                        <Titles>
                            <asp:Title ShadowOffset="3" Name="Items" />
                        </Titles>
                        <Legends>
                            <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
                        </Legends>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                        </ChartAreas>
                    </asp:Chart>
                    <%--<uc1:UcDetalleArbolAcceso runat="server" id="ucDetalleArbolAcceso" />--%>
                    <%--<uc1:UcDetalleGrupoUsuarios runat="server" id="ucDetalleGrupoUsuarios" />--%>
                    <%-- <div class="form-horizontal col-lg-6 col-lg-offset-3">
                        <div class="form-group">
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" required />
                            <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server"
                                TargetControlID="TextBox2"
                                Mask="9,999,999.99"
                                MessageValidatorTip="true"
                                OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError"
                                MaskType="Number"
                                InputDirection="LeftToRight"
                                AcceptNegative="None"
                                DisplayMoney="None"
                                ErrorTooltipEnabled="True" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" required />
                            <ajax:MaskedEditExtender ID="meeDate" runat="server"
                                TargetControlID="txtFecha"
                                Mask="99/99/9999"
                                UserDateFormat="DayMonthYear"
                                MessageValidatorTip="true"
                                OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError"
                                MaskType="Date"
                                InputDirection="LeftToRight"
                                AcceptNegative="None"
                                DisplayMoney="None"
                                ErrorTooltipEnabled="True" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" required MaxLength="10"></asp:TextBox>
                            <asp:ImageButton ID="imgPopup" ImageUrl="~/assets/images/like_S1.png" ImageAlign="Bottom"
                                runat="server" />
                            <ajax:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" CssClass="styleCalendar" TargetControlID="txtDOB" Format="dd/MM/yyyy" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtValorMaximo" required minlength="10" MaxLength="20" CssClass="form-control" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="TextBox1" required type="number" min="1" max="2147483646" step="any" CssClass="form-control" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                        </div>
                        <div class="form-group">
                            <asp:CheckBox runat="server" Text="Sin Validación" AutoPostBack="True" ID="chkValidation" />
                        </div>
                        <div class="form-group">
                            <asp:Button Text="enter" ID="btnPrueba" runat="server" CssClass="btn btn-primary" OnClick="OnClick1" />
                        </div>
                        <div class="form-group">
                            <asp:Button Text="boton 2" runat="server" CssClass="btn btn-primary" OnClick="OnClick1" />
                        </div>
                    </div>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
