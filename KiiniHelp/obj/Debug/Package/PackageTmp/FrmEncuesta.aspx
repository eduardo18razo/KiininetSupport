<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FrmEncuesta.aspx.cs" Inherits="KiiniHelp.FrmEncuesta" %>

<%@ Register Src="~/UserControls/Temporal/UcEncuestaCaptura.ascx" TagPrefix="uc1" TagName="UcEncuestaCaptura" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KiiniNet</title>
    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link href="assets/css/elegant-icons.css" rel="stylesheet" />
    <link href="assets/css/bootstrap-markdown.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link href="assets/js/jquery.js"/>
    <link href="assets/js/Notificaciones.js"/>
    <script>
        function CerrarVentana() {
            window.close();
        }
    </script>
</head>
<body style="background: white;">
    <div id="full">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
                <Scripts>
                    <asp:ScriptReference Path="~/assets/js/jquery.js" />
                    <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
                    <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                </Scripts>
            </asp:ScriptManager>
            <asp:UpdateProgress ID="updateProgress" runat="server" ClientIDMode="Static">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; background-color: rgba(0,0,0, .1); opacity: 1.0;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 25%; left: 40%; border: 10px;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hfIdTicket" />
                    <asp:HiddenField runat="server" ID="hfIdTipoServicio" />
                    <asp:HiddenField runat="server" ID="hfIdEncuesta" />
                    <header>
                        <div class="alert alert-danger" id="panelAlert" runat="server" visible="False">
                            <div>
                                <div style="float: left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div style="float: left">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix clear-fix" />
                            </div>
                            <hr />
                            <asp:Repeater runat="server" ID="rptHeaderError">
                                <ItemTemplate>
                                    <%# Container.DataItem %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </header>
                    <div class="panel panel-primary">
                        <div class="panel-body">
                            <uc1:UcEncuestaCaptura runat="server" ID="ucEncuestaCaptura" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
