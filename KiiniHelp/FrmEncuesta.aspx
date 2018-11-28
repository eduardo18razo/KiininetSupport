<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FrmEncuesta.aspx.cs" Inherits="KiiniHelp.FrmEncuesta" %>

<%@ Register Src="~/UserControls/Temporal/UcEncuestaCaptura.ascx" TagPrefix="uc1" TagName="UcEncuestaCaptura" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kiininet CXP</title>
    <link rel="Shortcut Icon" href="favicon.ico" type="image/x-icon" />
    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/css/elegant-icons.css" />
    <link rel="stylesheet" href="assets/css/bootstrap-markdown.css"/>
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link rel="stylesheet" href="assets/js/jquery.js"/>
    <link rel="stylesheet" href="assets/js/Notificaciones.js"/>
    <script>
        function CerrarVentana() {
            window.close();
        }
    </script>
</head>
<body class="bg-blanco">
    <div id="full">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
                <Scripts>
                    <asp:ScriptReference Path="~/assets/js/jquery.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-markdown.js" />
                <asp:ScriptReference Path="assets/js/imagesloaded.js" />
                <asp:ScriptReference Path="assets/js/masonry.js" />
                <asp:ScriptReference Path="assets/js/main.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.min.js" />
                <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                <asp:ScriptReference Path="~/assets/js/validation.js" />
                </Scripts>
            </asp:ScriptManager>
            <asp:UpdateProgress ID="updateProgress" runat="server" ClientIDMode="Static">
                <ProgressTemplate>
                    <div class="progressBar">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..."/>
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
                                <div class="float-left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div class="float-left">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix" />
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
