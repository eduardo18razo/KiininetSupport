<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTest.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Test.FrmTest" Culture="Auto" UICulture="Auto" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Src="~/UserControls/Configuracion/UcUploadCarrusel.ascx" TagPrefix="uc1" TagName="UcUploadCarrusel" %>
<%@ Register Src="~/UserControls/Consultas/UcConsultaDetalleFormulario.ascx" TagPrefix="uc1" TagName="UcConsultaDetalleFormulario" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Test</title>
    <link rel="Shortcut Icon" href="../favicon.ico" type="image/x-icon" />

    <link rel='stylesheet' href="../assets/css/font.css" />
    <link rel="stylesheet" href="../assets/css/font-awesome.css" />
    <link rel="stylesheet" href="../assets/css/pe-7-icons.css" />

    <link rel="stylesheet" href="../assets/css/bootstrap.css" />
    <link rel="stylesheet" href="../assets/css/styles.css" />
    <link rel="stylesheet" href="../assets/css/medias.css" />
    <link rel="stylesheet" href="../assets/css/menuStyle.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap-markdown.css" />
    <link rel="stylesheet" href="../assets/css/fileupload.css" />
    <script src="../assets/js/functions.js"></script>
    <script type="text/javascript">
        function checkExtension(sender, args) {


            try {
                var filename = args.get_fileName().toLowerCase();
                var ext = filename.substring(filename.lastIndexOf(".") + 1);
                if (ext != 'jpg' && ext != 'png' && ext != 'jpeg' && ext != 'gif') {

                    alert('Invalid File Type (Only .png, .gif and .jpg)');

                    return false;
                }
                else
                    return true;
            }
            catch (e) {
            }
        }
    </script>
    <%--<script type="text/javascript">
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
    </script>--%>
</head>
<body>
    <div id="full">
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
                <Scripts>
                    <asp:ScriptReference Path="~/assets/js/jquery.js" />
                    <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
                    <asp:ScriptReference Path="~/assets/js/bootstrap-markdown.js" />
                    <asp:ScriptReference Path="~/assets/js/imagesloaded.js" />
                    <asp:ScriptReference Path="~/assets/js/masonry.js" />
                    <asp:ScriptReference Path="~/assets/js/main.js" />
                    <asp:ScriptReference Path="~/assets/js/bootstrap-notify.js" />
                    <asp:ScriptReference Path="~/assets/js/bootstrap-notify.min.js" />
                    <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                    <asp:ScriptReference Path="~/assets/js/validation.js" />
                </Scripts>
            </asp:ScriptManager>
            <%--<uc1:UcConsultaDetalleFormulario runat="server" ID="UcConsultaDetalleFormulario" />
            <asp:TextBox runat="server" ID="txtTelefono" type="email"></asp:TextBox>


            <asp:Button runat="server" ID="btnTest" Text="submit" />
            <uc1:UcUploadCarrusel runat="server" ID="UcUploadCarrusel" />--%>
            
            <tc:RadHtmlChart runat="server" ID="rhcLike">
                                                    </tc:RadHtmlChart>

            <tc:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="1000" Height="500">
                <PlotArea>
                    <Series >
                        <tc:ColumnSeries Name="Like" GroupName="Likes" Stacked="true">
                            <SeriesItems>
                                <tc:CategorySeriesItem Y="7" />
                                <tc:CategorySeriesItem Y="9" />
                                <tc:CategorySeriesItem Y="11" />
                                <tc:CategorySeriesItem Y="13" />
                                <tc:CategorySeriesItem Y="12" />
                                <tc:CategorySeriesItem Y="9" />
                                <tc:CategorySeriesItem Y="12" />
                                <tc:CategorySeriesItem Y="14" />
                                <tc:CategorySeriesItem Y="18" />
                                <tc:CategorySeriesItem Y="22" />
                                <tc:CategorySeriesItem Y="24" />
                                <tc:CategorySeriesItem Y="26" />
                            </SeriesItems>
                            <TooltipsAppearance ClientTemplate="#= series.name# medals: #= dataItem.value#" />
                            <LabelsAppearance Visible="true"></LabelsAppearance>
                        </tc:ColumnSeries>
                        <tc:ColumnSeries Name="Dont Like" GroupName="Likes" Stacked="true">
                            <Appearance>
                                <FillStyle BackgroundColor="#C4BD97" />
                            </Appearance>
                            <SeriesItems>
                                <tc:CategorySeriesItem Y="16" />
                                <tc:CategorySeriesItem Y="7" />
                                <tc:CategorySeriesItem Y="14" />
                                <tc:CategorySeriesItem Y="5" />
                                <tc:CategorySeriesItem Y="8" />
                                <tc:CategorySeriesItem Y="8" />
                                <tc:CategorySeriesItem Y="8" />
                                <tc:CategorySeriesItem Y="8" />
                                <tc:CategorySeriesItem Y="10" />
                                <tc:CategorySeriesItem Y="7" />
                                <tc:CategorySeriesItem Y="6" />
                                <tc:CategorySeriesItem Y="4" />
                            </SeriesItems>
                            <TooltipsAppearance ClientTemplate="#= series.name# medals: #= dataItem.value#" />
                            <LabelsAppearance Visible="false"></LabelsAppearance>
                        </tc:ColumnSeries>
                    </Series>
                    <XAxis>
                        <LabelsAppearance DataFormatString="{0}" />
                        <Items>
                            <tc:AxisItem LabelText="Enero" />
                            <tc:AxisItem LabelText="Febrero" />
                            <tc:AxisItem LabelText="Marzo" />
                            <tc:AxisItem LabelText="Abril" />
                            <tc:AxisItem LabelText="Mayo" />
                            <tc:AxisItem LabelText="Junio" />
                            <tc:AxisItem LabelText="Julio" />
                            <tc:AxisItem LabelText="Agosto" />
                            <tc:AxisItem LabelText="Septiembre" />
                            <tc:AxisItem LabelText="Octubre" />
                            <tc:AxisItem LabelText="Noviembre" />
                            <tc:AxisItem LabelText="Diciembre" />
                        </Items>
                    </XAxis>
                </PlotArea>
                <ChartTitle Text="Olympic Medals Per Country Over the Years">
                </ChartTitle>
                <Legend>
                    <Appearance Position="Right" />
                </Legend>
            </tc:RadHtmlChart>

            <%--<asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true" />
            <tc:RadEditor RenderMode="Lightweight" runat="server" ID="RadEditor1" Width="800px">
                <tools>
                <tc:EditorToolGroup>
                    <tc:EditorTool Name="PageProperties" Text="Page Properties"></tc:EditorTool>
                </tc:EditorToolGroup>
                <tc:EditorToolGroup>
                    <tc:EditorTool Name="Bold"></tc:EditorTool>
                    <tc:EditorTool Name="Italic"></tc:EditorTool>
                    <tc:EditorTool Name="Underline"></tc:EditorTool>
                    <tc:EditorTool Name="Cut"></tc:EditorTool>
                    <tc:EditorTool Name="Copy"></tc:EditorTool>
                    <tc:EditorTool Name="Paste"></tc:EditorTool>
                    <tc:EditorTool Name="FontName"></tc:EditorTool>
                    <tc:EditorTool Name="RealFontSize"></tc:EditorTool>
                </tc:EditorToolGroup>
                <tc:EditorToolGroup>
                    <tc:EditorTool Name="InsertTable"></tc:EditorTool>
                    <tc:EditorTool Name="InsertImage"></tc:EditorTool>
                    <tc:EditorTool Name="LinkManager"></tc:EditorTool>
                    <tc:EditorTool Name="Unlink"></tc:EditorTool>
                    <tc:EditorTool Name="InsertOrderedList"></tc:EditorTool>
                    <tc:EditorTool Name=""/>
                    <tc:EditorTool Name="InsertUnorderedList"></tc:EditorTool>
                </tc:EditorToolGroup>
            </tools>
                <imagemanager viewpaths="~/Editor/images/UserDir/Marketing,~/Editor/images/UserDir/PublicRelations"></imagemanager>
                <content>
            </content>
            </tc:RadEditor>--%>
            <%--<asp:RadioButton runat="server" Text="Quiero recibir un correo con un enlace" />
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:TextBox
                        ID="txtComments"
                        TextMode="MultiLine"
                        Columns="50"
                        Rows="8"
                        runat="server" />--%>
            <%--<ajax:HtmlEditorExtender ID="txtDescricao_HtmlEditorExtender" runat="server" DisplaySourceTab="True"
                        Enabled="True" TargetControlID="txtComments" >
                        <Toolbar>
                            <ajax:Bold />
                            <ajax:Italic />
                            <ajax:InsertImage />
                        </Toolbar>
                    </ajax:HtmlEditorExtender>

                    <ajax:Editor
                        ID="Editor1"
                        Width="450px"
                        Height="200px"
                        runat="server">
                    </ajax:Editor>
                    <br />
                    <asp:Button
                        ID="btnSubmit"
                        Text="Submit"
                        runat="server" OnClick="btnSubmit_Click" />

                    <hr />
                    <h1>You Entered:</h1>
                    <hr />

                    <asp:Label
                        ID="lblComment"
                        runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>--%>
            <%--<asp:ListBox runat="server">
                <Items>
                    <asp:ListItem Text="Red" Value="#FF0000" Selected="True" />
                    <asp:ListItem Text="Blue" Value="#0000FF" />
                    <asp:ListItem Text="Green" Value="#008000" />
                </Items>
            </asp:ListBox>
            <tc:RadGrid runat="server" ID="gvTest" EnableHeaderContextFilterMenu="true" FilterType="HeaderContext" EnableHeaderContextMenu="true">
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="Id">
                    <Columns>
                        <tc:GridBoundColumn DataField="Id" FilterControlAltText="Filter ContactName column" HeaderText="Id" SortExpression="Id" UniqueName="Id" AutoPostBackOnFilter="true" EnableHeaderContextMenu="False">
                        </tc:GridBoundColumn>
                        <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Descripcion" FilterControlAltText="Filter ContactName column" HeaderText="Descripcion" SortExpression="Descripcion" UniqueName="Descripcion" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                        </tc:GridBoundColumn>
                        <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Sistema" FilterControlAltText="Filter ContactName column" HeaderText="Sistema" SortExpression="Sistema" UniqueName="Sistema" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                        </tc:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </tc:RadGrid>--%>
        </form>
    </div>
</body>
</html>
